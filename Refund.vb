'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Refund
'
' Copyright 2017 and Beyond
' All Rights Reserved
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
' €  All  rights reserved. No part of this  software  €€  This Software is Owned by        €
' €  may be reproduced or transmitted in any form or  €€                                   €
' €  by   any   means,  electronic   or  mechanical,  €€    GUANZON MERCHANDISING CORP.    €
' €  including recording, or by information  storage  €€     Guanzon Bldg. Perez Blvd.     €
' €  and  retrieval  systems, without  prior written  €€           Dagupan City            €
' €  from the author.                                 €€  Tel No. 522-1085 ; 522-9275      €
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
'
' ==========================================================================================
'  Jheff [ 01/18/2016 10:52 am ]
'     Start coding this object...
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ggcAppDriver
Imports System.Windows.Forms
Imports System.Reflection
Imports MySql.Data.MySqlClient
Imports ggcRetailParams
Imports ggcReceipt

Public Class Refund

#Region "Constant"
    Private Const xsSignature As String = "08220326"
    Private Const pxeMODULENAME As String = "Refund"
    Private Const pxeMasterTble As String = "Refund"
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oDTMaster As DataTable
    Protected p_nEditMode As xeEditMode
    Protected p_oSC As New MySqlCommand

    Protected p_sBranchCd As String
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
    Protected p_bCancelled As Boolean
    Protected p_sPOSNo As String
#End Region

#Region "Properties"
    ReadOnly Property AppDriver As GRider
        Get
            Return p_oAppDrvr
        End Get
    End Property

    ReadOnly Property Cancelled
        Get
            Return p_bCancelled
        End Get
    End Property

    Property Branch() As String
        Get
            Return p_sBranchCd
        End Get
        Set(ByVal Value As String)
            p_sBranchCd = Value
        End Set
    End Property

    Property SourceNo() As String
        Get
            Return p_sSourceNo
        End Get
        Set(ByVal Value As String)
            p_sSourceNo = Value
        End Set
    End Property

    Property SourceCd() As String
        Get
            Return p_sSourceCd
        End Get
        Set(ByVal Value As String)
            p_sSourceCd = Value
        End Set
    End Property

    WriteOnly Property POSNumbr As String
        Set(ByVal Value As String)
            p_sPOSNo = Value
        End Set
    End Property

    Property Master(Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "dtransact" : Index = 1
                    Case "sremarksx" : Index = 2
                    Case "srequestd" : Index = 3
                    Case "sapproved" : Index = 4
                    Case "namountxx" : Index = 5
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oDTMaster(0)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "dtransact" : Index = 1
                    Case "sremarksx" : Index = 2
                    Case "srequestd" : Index = 3
                    Case "sapproved" : Index = 4
                    Case "namountxx" : Index = 5
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                End Select
            End If
            p_oDTMaster(0)(Index) = Value
        End Set
    End Property
#End Region

#Region "Private Function"
    Private Function getApproval() As Boolean
        Dim lofrmUserDisc As New frmUserDisc
        Dim loDT As New DataTable

        Dim lnCtr As Integer = 0
        Dim lbValid As Boolean = False

        With lofrmUserDisc
            Do
                .TopMost = True
                .ShowDialog()
                If .Cancelled = True Then
                    Return False
                End If

                p_oSC.Connection = p_oAppDrvr.Connection
                p_oSC.CommandText = getSQ_User()
                p_oSC.Parameters.Clear()
                p_oSC.Parameters.AddWithValue("?sLogNamex", Encrypt(lofrmUserDisc.LogName, xsSignature))
                p_oSC.Parameters.AddWithValue("?sPassword", Encrypt(lofrmUserDisc.Password, xsSignature))

                loDT = p_oAppDrvr.ExecuteQuery(p_oSC)

                If loDT.Rows.Count = 0 Then
                    MsgBox("User Does Not Exist!" & vbCrLf & "Verify log name and/or password.", vbCritical, "Warning")
                    lnCtr += 1
                Else
                    If Not isUserActive(loDT) Then
                        lnCtr = 0
                    Else
                        If loDT.Rows(0).Item("nUserLevl") > xeUserRights.DATAENTRY Then
                            lbValid = True
                        Else
                            MsgBox("User is not allowed to give discount!" & vbCrLf & "Verify user name and/or password.", vbCritical, "Warning")
                            lnCtr += 1
                        End If
                    End If
                End If
            Loop Until lbValid Or lnCtr = 3
        End With

        If lbValid Then p_oDTMaster.Rows(0)("sApproved") = loDT.Rows(0).Item("sUserIDxx")

        Return lbValid
    End Function

    Private Function isUserActive(ByRef loDT As DataTable) As Boolean
        Dim lnCtr As Integer = 0
        Dim lbMember As Boolean = False

        If loDT.Rows(0).Item("cUserType").Equals(0) Then
            For lnCtr = 0 To loDT.Rows.Count - 1
                If loDT.Rows(0).Item("sProdctID").Equals(p_oAppDrvr.ProductID) Then
                    Exit For
                    lbMember = True
                End If
            Next
        Else
            lbMember = True
        End If

        If Not lbMember Then
            MsgBox("User is not a member of this application!!!" & vbCrLf & _
               "Application used is not allowed!!!", vbCritical, "Warning")
        End If

        ' check user status
        If loDT.Rows(0).Item("cUserStat").Equals(xeUserStatus.SUSPENDED) Then
            MsgBox("User is currently suspended!!!" & vbCrLf & _
                     "Application used is not allowed!!!", vbCritical, "Warning")
            Return False
        End If
        Return True
    End Function

    Private Function getSQ_User() As String
        Return "SELECT sUserIDxx" & _
              ", sLogNamex" & _
              ", sPassword" & _
              ", sUserName" & _
              ", nUserLevl" & _
              ", cUserType" & _
              ", sProdctID" & _
              ", cUserStat" & _
              ", nSysError" & _
              ", cLogStatx" & _
              ", cLockStat" & _
              ", cAllwLock" & _
           " FROM xxxSysUser" & _
           " WHERE sLogNamex = ?sLogNamex" & _
              " AND sPassword = ?sPassword"
    End Function

    Private Function getNextTransNo() As String
        Dim loDA As New MySqlDataAdapter
        Dim loDT As New DataTable
        Dim loDS As New DataSet
        Dim lsSQL As String
        Dim lnCounter As Integer
        Dim lnCode As Long
        Dim lnLen As Long
        Dim lsStr As String = ""

        lsSQL = "SELECT sTransNox" & _
                " FROM " & pxeMasterTble & _
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy") & "%") & _
                " ORDER BY sTransNox DESC" & _
                " LIMIT 1"

        Try
            loDA.SelectCommand = New MySqlCommand(lsSQL, p_oAppDrvr.Connection)
        Catch ex As MySqlException
            MsgBox(ex.Message)
            Throw ex
        End Try

        loDT.Clear()
        loDA.Fill(loDT)
        If loDT.Rows.Count = 0 Then
            lsSQL = ""

            loDA.FillSchema(loDS, SchemaType.Source)
            lnLen = loDS.Tables(0).Columns(0).MaxLength
            lnCode = 1

            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)

            lsSQL = loDT.Rows(0).Item("sTransNox")
            lnLen = Len(lsSQL)

            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function

    Private Function printRefund(Optional ByVal bReprint As Boolean = False) As Boolean
        Dim lnCtr As Integer
        Dim loPrint As PRN_Receipt

        loPrint = New PRN_Receipt(p_oAppDrvr)

        With loPrint
            If Not .InitMachine Then Return False

            .Transaction_Date = p_oAppDrvr.getSysDate
            '.ReferenceNo = p_oDataTable(0)("sORNumber")

            'If Not IsNothing(p_oDtaOrder) Then
            '    Dim lnSlPrc As Double
            '    For lnCtr = 0 To p_oDtaOrder.Rows.Count - 1
            '        If p_oDtaOrder(lnCtr)("cReversed") = xeLogical.NO Then
            '            'Compute unit price here...
            '            lnSlPrc = (p_oDtaOrder(lnCtr).Item("nUnitPrce") * _
            '                        (100 - p_oDtaOrder(lnCtr).Item("nDiscount")) / 100 -
            '                        p_oDtaOrder(lnCtr).Item("nAddDiscx"))

            '            .AddDetail(p_oDtaOrder(lnCtr)("nQuantity"), _
            '                       p_oDtaOrder(lnCtr)("sDescript"), _
            '                       lnSlPrc, _
            '                       True, IIf(p_oDtaOrder(lnCtr)("cComboMlx") = "0", True, False))
            '        End If
            '    Next
            'End If

            Dim loDR As DataRow
            Dim loDiscCard As clsDiscountCards
            loDiscCard = New clsDiscountCards(p_oAppDrvr)

            'If Not IsNothing(p_oDtaDiscx) Then
            '    loDR = loDiscCard.SearchCard(p_oDtaDiscx(0)("sDiscCard"), True)

            '    If Not IsNothing(loDR) Then
            '        .AddDiscount(p_oDtaDiscx(0)("sIDNumber"), _
            '                     loDR("sCardDesc"), _
            '                     p_nDiscAmtx, IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))
            '    Else
            '        .AddDiscount("", _
            '                     "", _
            '                     p_nDiscAmtx, IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))
            '    End If
            'End If

            '.NonVatSales = p_nNonVATxx
            '.CashPayment = p_oDataTable(0)("nTendered")

            Dim loBank As clsBanks
            loBank = New clsBanks(p_oAppDrvr)

            'Call getCreditCard()
            'If p_oDtaCCard.Rows.Count > 0 Then
            '    For lnCtr = 0 To p_oDtaCCard.Rows.Count - 1
            '        If p_oDtaCCard(lnCtr)("nAmountxx") > 0 Then
            '            loDR = loBank.SearchBank(p_oDtaCCard(lnCtr)("sBankIDxx"), True)

            '            .AddCreditCard(loDR("sBankName"), _
            '                           p_oDtaCCard(lnCtr)("sCardNoxx"), _
            '                           p_oDtaCCard(lnCtr)("sApprovNo"), _
            '                           p_oDtaCCard(lnCtr)("nAmountxx"))
            '        End If
            '    Next
            'End If

            'Call getCheck()
            'If p_oDtaCheck.Rows.Count > 0 Then
            '    For lnCtr = 0 To p_oDtaCheck.Rows.Count - 1
            '        If p_oDtaCheck(lnCtr)("nAmountxx") > 0 Then
            '            loDR = loBank.SearchBank(p_oDtaCheck(lnCtr)("sBankIDxx"), True)

            '            .AddCheck(loDR("sBankName"), _
            '                      p_oDtaCheck(lnCtr)("sCheckNox"), _
            '                      p_oDtaCheck(lnCtr)("dCheckDte"), _
            '                      p_oDtaCheck(lnCtr)("nAmountxx"))
            '        End If
            '    Next
            'End If

            'Call getGiftCert()
            'If p_oDtaGCert.Rows.Count > 0 Then
            '    For lnCtr = 0 To p_oDtaGCert.Rows.Count - 1
            '        If p_oDtaGCert(lnCtr)("nAmountxx") > 0 Then
            '            .AddGiftCoupon(p_oDtaGCert(lnCtr)("sCompnyCd"), _
            '                           p_oDtaGCert(lnCtr)("nAmountxx"))
            '        End If
            '    Next
            'End If

            .AddFooter("")
            .AddFooter("Thank you, and please come again.")
            .AddFooter("This is a no sale transaction")
            .AddFooter("")

            If bReprint Then
                .AddFooter("REPRINT ONLY")
            End If

            .AddFooter("SAMPLE RECEIPT")
            .AddFooter("Guanzon Systems & Services")
            .AddFooter("Perez Blvd., Dagupan City")
            .AddFooter("VAT REG TIN #: 000-123-515-0000")
            .AddFooter("ACCR #: 03000033051500000712638")
            .AddFooter("Date Issued: 01/01/2016")
            .AddFooter("")
            .AddFooter("THIS INVOICE/RECEIPT SHALL BE VALID")
            .AddFooter("FOR FIVE(5) YEARS FROM THE DATE OF")
            .AddFooter("THE PERMIT TO USE.")

            Return .PrintOR()
        End With
    End Function
#End Region

#Region "Public function"
    Function SaveTransaction() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer

        With p_oDTMaster
            If p_bCancelled Then Return False
            
            lsSQL = "INSERT"
            Try
                lnRow = p_oAppDrvr.ExecuteActionQuery(lsSQL)
                If lnRow <= 0 Then
                    MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End With

        Return True
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createMasterTable()
        p_oDTMaster = New DataTable
        With p_oDTMaster
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 14
            .Columns.Add("dTransact", GetType(Date))
            .Columns.Add("sRemarksx", GetType(String)).MaxLength = 64
            .Columns.Add("sRequestd", GetType(String)).MaxLength = 32
            .Columns.Add("sApproved", GetType(String)).MaxLength = 10
            .Columns.Add("nAmountxx", GetType(Decimal))
        End With
    End Sub

    Private Sub initMaster()
        With p_oDTMaster
            .Rows.Add()
            .Rows(0)("sTransNox") = getNextTransNo()
            .Rows(0)("dTransact") = p_oAppDrvr.SysDate
            .Rows(0)("sRemarksx") = ""
            .Rows(0)("sRequestd") = ""
            .Rows(0)("sApproved") = ""
            .Rows(0)("nAmountxx") = 0.0
        End With
    End Sub

    Sub ShowRefund()
        'p_oFormComplementary = New frmComplementary
        'With p_oFormComplementary
        '    .Complementary = Me
        '    .TopMost = True
        '    .ShowDialog()

        '    p_bCancelled = .CloseForm
        'End With
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider

        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub
End Class
