'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Return
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
'  Jheff [ 01/18/2017 02:58 pm ]
'     Start coding this object...
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ggcAppDriver
Imports System.Windows.Forms
Imports System.Reflection
Imports MySql.Data.MySqlClient
Imports ggcRetailParams
Imports ggcReceipt

Public Class SalesReturn

#Region "Constant"
    Private Const xsSignature As String = "08220326"
    Private Const pxeMODULENAME As String = "Return"
    Private Const pxeMasterTble As String = "Return_Master"
    Private Const pxeDetailTble As String = "Return_Detail"
#End Region

#Region "Protected Members"
    Public Event DisplayReturn(ByVal Row As Integer)
    'added by jovan 04 15 2021
    Private p_sLogName As String
    Private p_nNoClient As Integer
    Private p_nWithDisc As Integer
    Private p_cTrnMde As String
    Private p_sACCNox As String

    Protected p_oAppDrvr As GRider
    Protected p_oDTMaster As DataTable
    Protected p_oDTDetail As DataTable
    Protected p_oDiscount As Discount
    Protected p_oDtaDiscx As DataTable

    Protected p_nEditMode As xeEditMode
    Protected p_oSC As New MySqlCommand
    Protected p_oFormReturn As New frmSalesReturn

    Protected p_sBranchCd As String
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
    Protected p_bCancelled As Boolean
    Protected p_sPOSNo As String
    Protected p_sSerial As String
    Protected p_sORNumber As String
    Protected p_sCRMNumbr As String
    Protected p_nTermnlNo As Integer
    Protected p_sCashierx As String
    Protected p_sCashrIDx As String
    Protected p_dReceiptx As Date
    Protected p_nDiscount As Decimal
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

    ReadOnly Property ItemCount As Integer
        Get
            Return p_oDTDetail.Rows.Count
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

    WriteOnly Property SerialNo As String
        Set(ByVal value As String)
            p_sSerial = value
        End Set
    End Property

    WriteOnly Property ORNumber As String
        Set(ByVal Value As String)
            p_sORNumber = Value
        End Set
    End Property

    WriteOnly Property Terminal As Integer
        Set(ByVal Value As Integer)
            p_nTermnlNo = Value
        End Set
    End Property

    WriteOnly Property CRMNmber As String
        Set(ByVal Value As String)
            p_sCRMNumbr = Value
        End Set
    End Property

    Public Property LogName As String
        Get
            Return p_sLogName
        End Get
        Set(ByVal value As String)
            p_sLogName = value
        End Set
    End Property

    Public Property NoClients() As Integer
        Get
            Return p_nNoClient
        End Get
        Set(ByVal value As Integer)
            p_nNoClient = value
        End Set
    End Property

    WriteOnly Property TranMode As Char
        Set(ByVal Value As Char)
            p_cTrnMde = Value
        End Set
    End Property

    Public Property WithDiscount() As Integer
        Get
            Return p_nWithDisc
        End Get
        Set(ByVal value As Integer)
            p_nWithDisc = value
        End Set
    End Property

    WriteOnly Property AccrdNumber As String
        Set(ByVal Value As String)
            p_sACCNox = Value
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
                    Case "ssourceno" : Index = 3
                    Case "ssourcecd" : Index = 4
                    Case "ntranamtx" : Index = 5
                    Case "sapproved" : Index = 6
                    Case "ctranstat" : Index = 7
                    Case "ntrantotl" : Index = 8
                    Case "ndiscntbl" : Index = 9
                    Case "nvatsales" : Index = 10
                    Case "nvatamtxx" : Index = 11
                    Case "nnonvatxx" : Index = 12
                    Case "nvatdiscx" : Index = 13
                    Case "npwddiscx" : Index = 14
                    Case "ndiscount" : Index = 15
                    Case "nzeroratd" : Index = 16
                    Case "nvoidtotl" : Index = 17
                    Case "scashierx"
                        Return p_sCashierx
                    Case "sornumber"
                        Return p_sORNumber
                    Case "dreceiptx"
                        Return p_dReceiptx
                    Case "ntotdiscx"
                        Return p_nDiscount
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
                    Case "ssourceno" : Index = 3
                    Case "ssourcecd" : Index = 4
                    Case "ntranamtx" : Index = 5
                    Case "sapproved" : Index = 6
                    Case "ctranstat" : Index = 7
                    Case "ntrantotl" : Index = 8
                    Case "ndiscntbl" : Index = 9
                    Case "nvatsales" : Index = 10
                    Case "nvatamtxx" : Index = 11
                    Case "nnonvatxx" : Index = 12
                    Case "nvatdiscx" : Index = 13
                    Case "npwddiscx" : Index = 14
                    Case "ndiscount" : Index = 15
                    Case "nzeroratd" : Index = 16
                    Case "nvoidtotl" : Index = 17
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                End Select
            End If
            p_oDTMaster(0)(Index) = Value
        End Set
    End Property

    Property Detail(ByVal Row As Integer, _
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "nentrynox"
                    Case "sstockidx"
                    Case "nquantity"
                    Case "nreturnxx"
                    Case "nunitprce"
                    Case "ndiscount"
                    Case "nadddiscx"
                    Case "creversex"
                    Case "sbarcodex"
                    Case "sbriefdsc"
                    Case "ncomplmnt"
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oDTDetail.Rows(Row)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "nentrynox"
                    Case "sstockidx"
                    Case "nquantity"
                    Case "nreturnxx"
                        p_oDTDetail.Rows(Row)(Index) = Value

                        RaiseEvent DisplayReturn(Row)
                        Exit Property
                    Case "nunitprce"
                    Case "ndiscount"
                    Case "nadddiscx"
                    Case "creversex"
                    Case "sbarcodex"
                    Case "sbriefdsc"
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Exit Property
                End Select
            End If
            p_oDTDetail.Rows(Row)(Index) = Value
        End Set
    End Property
#End Region

#Region "Private Function"


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



    Private Function getSQL_Master() As String
        Return "SELECT" & _
                    "  sTransNox" & _
                    ", dTransact" & _
                    ", sRemarksx" & _
                    ", sSourceNo" & _
                    ", sSourceCd" & _
                    ", nTranAmtx" & _
                    ", sApproved" & _
                    ", cTranStat" & _
                " FROM " & pxeMasterTble
    End Function

    Private Function getSQL_Detail() As String
        Return "SELECT" & _
                    "  sTransNox" & _
                    ", nEntryNox" & _
                    ", sStockIDx" & _
                    ", nQuantity" & _
                    ", nUnitPrce" & _
                    ", nComplmnt" & _
                " FROM " & pxeDetailTble & _
                " ORDER BY nEntryNox"
    End Function

    Private Function getSQL_Receipt() As String
        Return "SELECT sTransNox" & _
              ", dTransact" & _
              ", sORNumber" & _
              ", nSalesAmt" & _
              ", IF(sSourceCd = 'SO', 'Sales', 'Splitted') xSourcexx" & _
              ", sSourceNo" & _
              ", sSourceCd" & _
              ", sCashierx" & _
              ", (nDiscount + nVatDiscx + nPWDDiscx) nDiscount" & _
              ", cTranStat" & _
           " FROM Receipt_Master"
    End Function

    Private Function loadOrder(ByVal fsSourceNo As String, _
                               ByVal fsSourceCd As String) As Boolean
        Dim lsSQL As String
        Dim loSalesxx As DataTable
        Dim lsMergeID As String
        Dim loDT As DataTable
        Dim loReturn As DataTable

        If fsSourceCd = "SO" Then
            lsSQL = "SELECT" & _
                        "  sTransNox" & _
                        ", dTransact" & _
                        ", nContrlNo" & _
                        ", sReceiptx" & _
                        ", nTranTotl" & _
                        ", sCashierx" & _
                        ", sTableNox" & _
                        ", sWaiterID" & _
                        ", sMergeIDx" & _
                        ", cTranStat" & _
                        ", dModified" & _
                    " FROM SO_Master" & _
                    " WHERE sTransNox = " & strParm(fsSourceNo)

            lsMergeID = String.Empty
            loSalesxx = p_oAppDrvr.ExecuteQuery(lsSQL)
            If Trim(IFNull(loSalesxx.Rows(0)("sMergeIDx"), "")) <> "" Then
                lsSQL = "SELECT" & _
                            "  sTransNox" & _
                            ", sMergeIDx" & _
                        " FROM SO_Master" & _
                        " WHERE sMergeIDx = " & strParm(loSalesxx.Rows(0)("sMergeIDx")) & _
                        " ORDER BY sTransNox" & _
                        " LIMIT 1"

                loSalesxx = p_oAppDrvr.ExecuteQuery(lsSQL)
                fsSourceNo = loSalesxx.Rows(0)("sTransNox")
                lsMergeID = loSalesxx.Rows(0)("sMergeIDx")
            End If

            lsSQL = "SELECT b.sBarcodex" & _
                        ", b.sDescript" & _
                        ", b.sBriefDsc" & _
                        ", a.nUnitPrce" & _
                        ", a.cReversex" & _
                        ", a.nQuantity" & _
                        ", a.nDiscount" & _
                        ", a.nAddDiscx" & _
                        ", b.nDiscLev1" & _
                        ", b.nDiscLev2" & _
                        ", b.nDiscLev3" & _
                        ", b.nDealrDsc" & _
                        ", a.sStockIDx" & _
                        ", b.sCategrID" & _
                        ", a.cPrintedx" & _
                        ", a.sTransNox" & _
                        ", a.nComplmnt" & _
                        ", a.nEntryNox" & _
                        ", a.cServedxx" & _
                        ", a.cDetailxx" & _
                        ", a.sReplItem" & _
                        ", a.cReversed" & _
                        ", a.cComboMlx" & _
                        ", a.cWthPromo" & _
                        ", a.dModified" & _
                    " FROM SO_Detail a" & _
                        ", Inventory b" & _
                        ", SO_Master d" & _
                    " WHERE a.sStockIDx = b.sStockIDx" & _
                        " AND a.sTransNox = d.sTransNox" & _
                        IIf(lsMergeID = "", " AND a.sTransNox = " & strParm(fsSourceNo) _
                                            , " AND d.sMergeIDx = " & strParm(lsMergeID)) & _
                    " ORDER BY a.nEntryNox"
        Else
            lsSQL = "SELECT b.sBarcodex" & _
                        ", b.sDescript" & _
                        ", b.sBriefDsc" & _
                        ", a.nUnitPrce" & _
                        ", '+' cReversex" & _
                        ", a.nQuantity" & _
                        ", 0.00 nDiscount" & _
                        ", 0.00 nAddDiscx" & _
                        ", a.sStockIDx" & _
                        ", a.sTransNox" & _
                        ", 0 nComplmnt" & _
                        ", '0' cDetailxx" & _
                    " FROM Order_Split_Detail a" & _
                        ", Inventory b" & _
                    " WHERE a.sStockIDx = b.sStockIDx" & _
                        " AND a.sTransNox = " & strParm(fsSourceNo) & _
                    " ORDER BY a.nEntryNox"
        End If

        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery("SELECT" & _
                                            "  b.sStockIDx" & _
                                            ", SUM(b.nQuantity) nQuantity" & _
                                            ", SUM(b.nQuantity) nReturnxx" & _
                                        " FROM Return_Master a" & _
                                            ", Return_Detail b" & _
                                        " WHERE a.sTransNox = b.sTransNox" & _
                                            " AND a.sSourceNo = " & strParm(fsSourceNo) & _
                                            " AND a.sSourceCd = " & strParm(fsSourceCd) & _
                                        " GROUP BY b.sStockIDx")

        loReturn = New DataTable
        With loReturn
            .Columns.Add("sStockIDx", GetType(String)).MaxLength = 12
            .Columns.Add("nQuantity", GetType(Integer))
            .Columns.Add("nReturnxx", GetType(Integer))
        End With
        loReturn = loDT

        Call createMasterTable()
        Call initMaster()

        createDetailTable()
        loSalesxx = p_oAppDrvr.ExecuteQuery(lsSQL)
        If loSalesxx.Rows.Count = 0 Then Return False
        With loSalesxx
            For lnCtr = 0 To loSalesxx.Rows.Count - 1
                If .Rows(lnCtr)("cReversex") = "+" And _
                    .Rows(lnCtr)("cDetailxx") = "0" Then
                    p_oDTDetail.Rows.Add()
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("sTransNox") = p_oDTMaster.Rows(0)("sTransNox")
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nEntryNox") = lnCtr + 1
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("sStockIDx") = .Rows(lnCtr)("sStockIDx")
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nQuantity") = .Rows(lnCtr)("nQuantity") - .Rows(lnCtr)("nComplmnt")
                    If loReturn.Rows.Count > 0 Then
                        For nCtr As Integer = 0 To loReturn.Rows.Count - 1
                            If .Rows(lnCtr)("sStockIDx") = loReturn.Rows(nCtr)("sStockIDx") Then
                                If loReturn.Rows(nCtr)("nReturnxx") > 0 Then
                                    If loReturn.Rows(nCtr)("nQuantity") > .Rows(lnCtr)("nQuantity") - .Rows(lnCtr)("nComplmnt") Then
                                        p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nQuantity") = .Rows(lnCtr)("nQuantity") - .Rows(lnCtr)("nComplmnt")
                                        loReturn.Rows(nCtr)("nReturnxx") = loReturn.Rows(nCtr)("nReturnxx") - (.Rows(lnCtr)("nQuantity") - .Rows(lnCtr)("nComplmnt"))
                                    Else
                                        p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nQuantity") = (.Rows(lnCtr)("nQuantity") - .Rows(lnCtr)("nComplmnt")) - loReturn.Rows(nCtr)("nReturnxx")
                                        loReturn.Rows(nCtr)("nReturnxx") = (.Rows(lnCtr)("nQuantity") - .Rows(lnCtr)("nComplmnt")) - loReturn.Rows(nCtr)("nReturnxx")
                                    End If
                                End If
                            End If
                        Next nCtr
                    End If
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nReturnxx") = 0
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nUnitPrce") = .Rows(lnCtr)("nUnitPrce")
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nDiscount") = .Rows(lnCtr)("nDiscount")
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nAddDiscx") = .Rows(lnCtr)("nAddDiscx")
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("sBarcodex") = .Rows(lnCtr)("sBarcodex")
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("sBriefDsc") = .Rows(lnCtr)("sBriefDsc")
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("cReversex") = .Rows(lnCtr)("cReversex")
                    p_oDTDetail.Rows(p_oDTDetail.Rows.Count - 1)("nComplmnt") = .Rows(lnCtr)("nComplmnt")
                End If
            Next lnCtr
        End With

        p_oDTMaster.Rows(0)("nNonVATxx") = 0.0
        p_oDTMaster.Rows(0)("nDiscount") = 0.0
        p_oDTMaster.Rows(0)("nPWDDiscx") = 0.0
        p_oDTMaster.Rows(0)("nVatDiscx") = 0.0
        p_oDTMaster.Rows(0)("nTranTotl") = 0.0
        p_oDTMaster.Rows(0)("nVATSales") = 0.0
        p_oDTMaster.Rows(0)("nVATAmtxx") = 0.0
        p_oDTMaster.Rows(0)("nZeroRatd") = 0.0
        p_oDTMaster.Rows(0)("nVoidTotl") = 0.0
        p_oDTMaster.Rows(0)("sSourceNo") = fsSourceNo
        p_oDTMaster.Rows(0)("sSourceCd") = fsSourceCd
        computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

        Return True
    End Function

    Private Sub LoadDiscount()
        p_oDiscount = New Discount(p_oAppDrvr)
        p_oDiscount.POSNumbr = p_sPOSNo
        p_oDiscount.SourceNo = p_sSourceNo
        p_oDiscount.SourceCd = p_sSourceCd
        p_oDiscount.InitTransaction()
        p_oDiscount.OpenTransaction()

        If p_oDiscount.HasDiscount Then
            Dim lsSQL As String
            lsSQL = "SELECT sCategrID, nMinAmtxx, nDiscRate, nDiscAmtx" & _
                   " FROM Discount_Card_Detail" & _
                   " WHERE sCardIDxx = " & strParm(p_oDiscount.Master("sCardIDxx"))

            Try
                p_oDtaDiscx = p_oAppDrvr.ExecuteQuery(lsSQL)

                If p_oDtaDiscx.Rows.Count = 0 Then p_oDtaDiscx = Nothing
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            p_oDtaDiscx = Nothing
        End If
    End Sub

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
#End Region

#Region "Public Function"
    Function SaveTransaction() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim lnCtr As Integer
        Dim lnTotal As Integer = 0
        Dim lbEmptyDet As Boolean = True
        Dim lnEntryNox As Integer

        With p_oDTMaster
            If p_bCancelled Then Return False
            lnEntryNox = 0
            For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
                If p_oDTDetail.Rows(lnCtr)("nReturnxx") > 0 Then
                    If lbEmptyDet Then
                        If p_oDTDetail.Rows(lnCtr)("nQuantity") - p_oDTDetail.Rows(lnCtr)("nComplmnt") > p_oDTDetail.Rows(lnCtr)("nReturnxx") Then
                            lbEmptyDet = False
                        End If
                    End If

                    lnTotal += p_oDTDetail.Rows(lnCtr)("nReturnxx") * p_oDTDetail.Rows(lnCtr)("nUnitPrce")
                    lsSQL = "INSERT INTO " & pxeDetailTble & " SET" & _
                                "  sTransNox = " & strParm(p_oDTMaster.Rows(0)("sTransNox")) & _
                                ", nEntryNox = " & CDbl(lnEntryNox + 1) & _
                                ", sStockIDx = " & strParm(p_oDTDetail.Rows(lnCtr)("sStockIDx")) & _
                                ", nQuantity = " & strParm(p_oDTDetail.Rows(lnCtr)("nReturnxx")) & _
                                ", nUnitPrce = " & p_oDTDetail.Rows(lnCtr)("nUnitPrce") * (100 - p_oDTDetail.Rows(lnCtr)("nDiscount")) / 100 - p_oDTDetail.Rows(lnCtr)("nAddDiscx") & _
                                ", dModified = " & dateParm(p_oAppDrvr.SysDate)

                    Try

                        lnRow = p_oAppDrvr.Execute(lsSQL, pxeDetailTble)
                        If lnRow <= 0 Then
                            MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                            Return False
                        End If

                        lnEntryNox = lnEntryNox + 1
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If
            Next
            If lnTotal <= 0 Then Return False

            lsSQL = "INSERT INTO " & pxeMasterTble & " SET" & _
                        "  sTransNox = " & strParm(.Rows(0)("sTransNox")) & _
                        ", dTransact = " & dateParm(.Rows(0)("dTransact")) & _
                        ", sRemarksx = " & strParm(.Rows(0)("sRemarksx")) & _
                        ", sSourceNo = " & strParm(.Rows(0)("sSourceNo")) & _
                        ", sSourceCd = " & strParm(.Rows(0)("sSourceCd")) & _
                        ", nTranAmtx = " & CDbl(.Rows(0)("nTranAmtx")) & _
                        ", sApproved = " & strParm(.Rows(0)("sApproved")) & _
                        ", cTranStat = " & strParm(xeTranStat.TRANS_OPEN) & _
                        ", sModified = " & strParm(p_oAppDrvr.UserID) & _
                        ", dModified = " & dateParm(p_oAppDrvr.SysDate)
            Try
                lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                If lnRow <= 0 Then
                    MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            End Try

            lsSQL = "UPDATE Daily_Summary SET" & _
                        "  nReturnsx = nReturnsx + " & CDbl(lnTotal) & _
                    " WHERE sTranDate = " & strParm(Format(p_dReceiptx, "yyyyMMdd")) & _
                        " AND sCRMNumbr = " & strParm(p_sCRMNumbr) & _
                        " AND sCashierx = " & strParm(p_sCashrIDx)
            Try
                lnRow = p_oAppDrvr.Execute(lsSQL, "Daily_Summary")
                If lnRow <= 0 Then
                    MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            End Try

            If lbEmptyDet Then
                lsSQL = "UPDATE Receipt_Master SET" & _
                            "  cTranStat = " & strParm(xeTranStat.TRANS_UNKNOWN) & _
                        " WHERE sSourceNo = " & strParm(.Rows(0)("sSourceNo")) & _
                            " AND sSourceCd = " & strParm(.Rows(0)("sSourceCd"))

                Try
                    lnRow = p_oAppDrvr.Execute(lsSQL, "Receipt_Master")
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End If

            p_oAppDrvr.SaveEvent("0026", "Order TN " & p_sSourceNo & "/" & p_sSourceCd & "/Amount " & .Rows(0)("nTranAmtx"), p_sSerial)
        End With

        Return True
    End Function

    Function OpenTransaction(ByVal fsTransNox As String) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = AddCondition(getSQL_Master, "sTransNox = " & strParm(fsTransNox))

        p_oDTMaster = New DataTable
        p_oDTMaster = p_oAppDrvr.ExecuteQuery(lsSQL)

        If p_oDTMaster.Rows.Count = 0 Then Return False

        lsSQL = AddCondition(getSQL_Detail, "sTransNox = " & strParm(fsTransNox))
        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        createDetailTable()
        For nCtr As Integer = 0 To loDT.Rows.Count - 1
            p_oDTDetail.Rows.Add()
            p_oDTDetail.Rows(nCtr)("sTransNox") = loDT.Rows(nCtr)("sTransNox")
            p_oDTDetail.Rows(nCtr)("nEntryNox") = loDT.Rows(nCtr)("nEntryNox")
            p_oDTDetail.Rows(nCtr)("sStockIDx") = loDT.Rows(nCtr)("sStockIDx")
            p_oDTDetail.Rows(nCtr)("nQuantity") = loDT.Rows(nCtr)("nQuantity")
            p_oDTDetail.Rows(nCtr)("nReturnxx") = loDT.Rows(nCtr)("nQuantity")
            p_oDTDetail.Rows(nCtr)("nUnitPrce") = loDT.Rows(nCtr)("nUnitPrce")
            p_oDTDetail.Rows(nCtr)("nComplmnt") = loDT.Rows(nCtr)("nComplmnt")
        Next nCtr

        Return True
    End Function

    Function printReturn() As Boolean
        Dim lnCtr As Integer
        Dim loPrint As PRN_Return

        loPrint = New PRN_Return(p_oAppDrvr)

        With loPrint
            If Not .InitMachine Then Return False

            .Transaction_Date = p_oAppDrvr.getSysDate
            .ReturnNo = Right(p_oDTMaster(0)("sTransNox"), 6)
            .ReferenceNo = p_sORNumber
            .Discount = p_nDiscount
            .LogName = p_sLogName
            .Cashier = p_oAppDrvr.UserName 'getUserName(p_oDTMaster(0).Item("sModified"))
            .CashierName = getCashier(p_oAppDrvr.UserID)


            If Not IsNothing(p_oDTDetail) Then
                For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
                    If p_oDTDetail(lnCtr)("nReturnxx") > 0 Then
                        .AddDetail(p_oDTDetail(lnCtr)("nReturnxx"), _
                                   p_oDTDetail(lnCtr)("sBriefDsc"), _
                                   p_oDTDetail(lnCtr).Item("nUnitPrce"))
                    End If
                Next
            End If

            .AddFooter("")
            .AddFooter("Thank you, and please come again.")
            .AddFooter("")

            .AddFooter("RMJ Business Solutions")
            .AddFooter("32 Pogo Grande")
            .AddFooter("Dagupan City, Pangasinan 2400")
            .AddFooter("VAT REG TIN #: 469-083-682-00000")
            .AddFooter("ACC NO.: XXXXXXXXXXXXXXXXXXXXXX")
            .AddFooter("ACC Validity: XX/XX/XXXX - XX/XX/XXXX")
            .AddFooter("PTU NO.: XXXXXXXXXXXXXXXXXXXXXX")
            .AddFooter("PTU Validity: XX/XX/XXXX - XX/XX/XXXX")

            .AddFooter("")
            .AddFooter("THIS DOCUMENT SHALL BE VALID")
            .AddFooter("FOR FIVE(5) YEARS FROM THE DATE OF")
            .AddFooter("THE PERMIT TO USE")
            .AddFooter("THIS DOCUMENT IS NOT VALID")
            .AddFooter("FOR CLAIM OF INPUT TAX")

            Return .PrintReturns()
        End With

    End Function

     Public Function getCashier(ByVal sCashierx As String) As String
        Dim lsSQL As String
        Dim lsCashierNm As String
        Dim loDta As DataTable

        lsSQL = "SELECT" & _
                    " a.sUserName" & _
                    " FROM xxxSysUser a" & _
                    " WHERE a.sUserIDxx = " & strParm(sCashierx)

        loDta = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            lsCashierNm = ""
        Else
            lsCashierNm = Decrypt(loDta(0).Item("sUserName"), xsSignature)
        End If

        loDta = Nothing

        Return lsCashierNm
    End Function

    Function getORNumber(ByVal fsValue As String, _
                                 ByVal fnTermnl As Integer) As Boolean
        Dim lsCondition As String
        Dim lsProcName As String
        Dim lsSQL As String
        Dim loDataRow As DataRow

        lsProcName = "getORNumber"

        lsCondition = "sTransNox LIKE " & strParm(p_sBranchCd & Format(fnTermnl, "00") & "%") & _
                            " AND sORNumber LIKE " & strParm("%" & fsValue) & _
                            " AND cTranStat NOT IN ('3', '4')"

        lsSQL = AddCondition(getSQL_Receipt, lsCondition)
        Dim loDT As DataTable
        loDT = New DataTable

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            Return False
            'GoTo endWithClear
        ElseIf loDT.Rows.Count = 1 Then
            p_sSourceNo = loDT.Rows(0)("sSourceNo")
            p_sSourceCd = loDT.Rows(0)("sSourceCd")
            p_sORNumber = loDT.Rows(0)("sORNumber")
            p_sCashrIDx = loDT.Rows(0)("sCashierx")
            p_sCashierx = getUserName(loDT.Rows(0)("sCashierx"))
            p_dReceiptx = loDT.Rows(0)("dTransact")
            p_nDiscount = loDT.Rows(0)("nDiscount")

            p_nTermnlNo = fnTermnl

            If Not loadOrder(p_sSourceNo, p_sSourceCd) Then
                Return False
            End If
        Else
            'if OR# is 0000000011, type 11 instead of 1.
            'if OR# is 0000000001, type 01 instead of 1.
            MsgBox("Multiple OR Number Detected. Please be more specific on your entry.", MsgBoxStyle.Information, "Notice")

            Return False

            'iMac 2017.01.26
            'loDataRow = KwikSearch(p_oAppDrvr, _
            '                    lsSQL, _
            '                    "", _
            '                    "sTransNox»dTransact»sORNumber»nSalesAmt»xSourcexx", _
            '                    "TransNox»Date»OR Number»Amount»Source", _
            '                    "", _
            '                    "", _
            '                    2)

            'If Not IsNothing(loDataRow) Then
            '    p_sSourceNo = loDataRow("sSourceNo")
            '    p_sSourceCd = loDataRow("sSourceCd")
            '    p_sORNumber = loDataRow("sORNumber")
            '    p_sCashierx = getUserName(loDataRow("sCashierx"))
            '    p_dReceiptx = loDataRow("dTransact")
            '    p_nTermnlNo = fnTermnl
            '    Call loadOrder(loDataRow("sSourceNo"), loDataRow("sSourceCd"))
            'Else : GoTo endWithClear
            'End If
        End If
        loDT = Nothing

endProc:
        Return True
        Exit Function
endWithClear:
        Call createDetailTable()
        GoTo endProc
errProc:
        MsgBox(Err.Description)
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createMasterTable()
        p_oDTMaster = New DataTable
        With p_oDTMaster
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("dTransact", GetType(Date))
            .Columns.Add("sRemarksx", GetType(String)).MaxLength = 64
            .Columns.Add("sSourceNo", GetType(String)).MaxLength = 20
            .Columns.Add("sSourceCd", GetType(String)).MaxLength = 4
            .Columns.Add("nTranAmtx", GetType(Decimal))
            .Columns.Add("sApproved", GetType(String)).MaxLength = 10
            .Columns.Add("cTranStat", GetType(String)).MaxLength = 1
            .Columns.Add("nTranTotl", GetType(Decimal))
            .Columns.Add("nDiscntbl", GetType(Decimal))
            .Columns.Add("nVATSales", GetType(Decimal))
            .Columns.Add("nVATAmtxx", GetType(Decimal))
            .Columns.Add("nNonVatxx", GetType(Decimal))
            .Columns.Add("nVATDiscx", GetType(Decimal))
            .Columns.Add("nPWDDiscx", GetType(Decimal))
            .Columns.Add("nDiscount", GetType(Decimal))
            .Columns.Add("nZeroRatd", GetType(Decimal))
            .Columns.Add("nVoidTotl", GetType(Decimal))
        End With
    End Sub

    Private Sub initMaster()
        With p_oDTMaster
            .Rows.Add()
            .Rows(0)("sTransNox") = getNextTransNo()
            .Rows(0)("dTransact") = p_oAppDrvr.SysDate
            .Rows(0)("sRemarksx") = ""
            .Rows(0)("sSourceNo") = ""
            .Rows(0)("sSourceCd") = ""
            .Rows(0)("nTranAmtx") = 0.0
            .Rows(0)("sApproved") = ""
            .Rows(0)("cTranStat") = "0"
        End With
    End Sub

    Private Sub createDetailTable()
        p_oDTDetail = New DataTable
        With p_oDTDetail
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("nEntryNox", GetType(Integer))
            .Columns.Add("sStockIDx", GetType(String)).MaxLength = 12
            .Columns.Add("nQuantity", GetType(Integer))
            .Columns.Add("nReturnxx", GetType(Integer))
            .Columns.Add("nUnitPrce", GetType(Decimal))
            .Columns.Add("nDiscount", GetType(Decimal))
            .Columns.Add("nAddDiscx", GetType(Decimal))
            .Columns.Add("sBarcodex", GetType(String)).MaxLength = 12
            .Columns.Add("sBriefDsc", GetType(String)).MaxLength = 16
            .Columns.Add("cReversex", GetType(String)).MaxLength = 1
            .Columns.Add("nComplmnt", GetType(Integer))
        End With
    End Sub

    Sub ShowRetrun()
        p_oFormReturn = New frmSalesReturn
        With p_oFormReturn
            .SalesReturn = Me
            .Terminal = p_sPOSNo
            .TopMost = True
            .ShowDialog()

            If Not .Cancelled Then
                printReturn()
            End If
        End With
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

    Private Function getUserName(ByVal fsUserIDxx) As String
        Dim loDT As DataTable

        loDT = p_oAppDrvr.ExecuteQuery("SELECT sUserName FROM xxxSysUser WHERE sUserIDxx = " & strParm(fsUserIDxx))

        If loDT.Rows.Count = 0 Then
            Return ""
        Else
            Return Decrypt(loDT(0)("sUserName"), "08220326")
        End If
    End Function
End Class