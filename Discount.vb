'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Discount
'
' Copyright 2016 and Beyond
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
'  Jheff [ 10/12/2016 02:58 pm ]
'     Start coding this object...
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

Imports ggcAppDriver
Imports System.Windows.Forms
Imports System.Reflection
Imports ggcRetailParams
Imports MySql.Data.MySqlClient

Public Class Discount

#Region "Constant"
    Private Const xsSignature As String = "08220326"
    Private Const pxeMODULENAME As String = "Discount"
    Private Const pxeMasterTble As String = "Discount"
    Private Const pxeDetailTble As String = "Discount_Detail"
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oMasterTable As DataTable
    Protected p_oDetailTable As DataTable
    Protected p_oCategrTable As DataTable

    'Protected p_oDataTable As DataTable
    Protected p_oSC As New MySqlCommand
    Protected p_oCard As clsDiscountCards
    Protected p_nEditMode As xeEditMode
    Protected p_oFormDiscount As frmDiscount
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
    Protected p_sCategIDx As String
    Protected p_sPOSNo As String
    Protected p_sSerial As String
    Protected p_bWithVatx As Boolean
    Protected p_sTransNox As String
    Protected p_sBranchCd As String
    Protected p_sCardDesc As String

    Protected p_bCancelled As Boolean
    Protected p_bHasDiscount As Boolean

    Protected p_sUserIDxx As String
    Protected p_sUserName As String
    Protected p_sLogNamex As String
    Protected p_nUserLevl As Integer
#End Region
    Public p_nTotalSales As Double

    'jovan aaded to get total client and with discount for printing
    Public p_nNoClient As Integer
    Public p_nWithDisc As Integer

#Region "Event"
    Public Event MasterRetrieved(ByVal Index As Object, _
                                 ByVal Value As Object)

    Public Event DetailRetreive(ByVal Row As Integer, _
                                ByVal Index As Integer, _
                                ByVal Value As String)
#End Region

#Region "Properties"
    ReadOnly Property AppDriver As GRider
        Get
            Return p_oAppDrvr
        End Get
    End Property

    ReadOnly Property isVatable
        Get
            Return p_bWithVatx
        End Get
    End Property

    ReadOnly Property HasDiscount As Boolean
        Get
            Return p_bHasDiscount
        End Get
    End Property

    'ReadOnly Property Discounts As DataTable
    '    Get
    '        Return p_oDataTable
    '    End Get
    'End Property

    ReadOnly Property DiscountsMaster As DataTable
        Get
            Return p_oMasterTable
        End Get
    End Property

    ReadOnly Property DiscountsDetail As DataTable
        Get
            Return p_oDetailTable
        End Get
    End Property

    ReadOnly Property DiscountsCategory As DataTable
        Get
            Return p_oCategrTable
        End Get
    End Property

    ReadOnly Property ItemDetailCount()
        Get
            Return p_oDetailTable.Rows.Count
        End Get
    End Property

    ReadOnly Property ItemCategoryCount()
        Get
            Return p_oCategrTable.Rows.Count
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

    WriteOnly Property Category As String
        Set(ByVal Value As String)
            p_sCategIDx = Value
        End Set
    End Property

    WriteOnly Property setTranTotal As Double
        Set(ByVal value As Double)
            p_nTotalSales = value
        End Set
    End Property

    Public ReadOnly Property GetClientNo() As Integer
        Get
            Return p_nNoClient
        End Get
    End Property

    Public ReadOnly Property getWDiscount() As Integer
        Get
            Return p_nWithDisc
        End Get
    End Property

    ReadOnly Property MasFldSize(ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "sidnumber" : Index = 1
                    Case "sdisccard" : Index = 2
                        Return 32
                    Case "sapproved" : Index = 8
                    Case "sremarksx" : Index = 9
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oMasterTable.Columns(0).MaxLength
        End Get
    End Property

    ReadOnly Property DetFldSize(ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "scategrid" : Index = 1
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oCategrTable.Columns(0).MaxLength
        End Get
    End Property

    Property Master(ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "sidnumber" : Index = 1
                    Case "scardidxx" : Index = 2
                    Case "sdisccard"
                        Return p_sCardDesc
                    Case "nnoclient" : Index = 3
                    Case "nwithdisc" : Index = 4
                    Case "ndiscrate" : Index = 5
                    Case "nadddiscx" : Index = 6
                    Case "cnonevatx" : Index = 7
                    Case "sapproved" : Index = 8
                    Case "sremarksx" : Index = 9
                    Case "nminamtxx" : Index = 10
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oMasterTable(0)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "sidnumber" : Index = 1
                    Case "sdisccard" : Index = 2
                    Case "nnoclient" : Index = 3
                        If Not IsNumeric(Value) Then Value = 0
                    Case "nwithdisc" : Index = 4
                        If Not IsNumeric(Value) Then Value = 0
                    Case "ndiscrate" : Index = 5
                        If Not IsNumeric(Value) Then Value = 0.0
                    Case "nadddiscx" : Index = 6
                        If Not IsNumeric(Value) Then Value = 0.0
                    Case "cnonevatx" : Index = 7
                    Case "sapproved" : Index = 8
                    Case "sremarksx" : Index = 9
                    Case "nminamtxx" : Index = 10
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                End Select
            End If
            p_oMasterTable(0)(Index) = Value
            RaiseEvent MasterRetrieved(Index, p_oMasterTable(0)(Index))
        End Set
    End Property

    Property Detail(ByVal Row As Object, ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "nentrynox" : Index = 1
                    Case "sidnumber" : Index = 2
                    Case "sclientnm" : Index = 3
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oDetailTable(Row)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "nentrynox" : Index = 1
                    Case "sidnumber" : Index = 2
                    Case "sclientnm" : Index = 3
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                End Select
            End If

            p_oDetailTable(Row)(Index) = Value
        End Set
    End Property

    Property Category(ByVal Row As Object, ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "scategrid" : Index = 1
                    Case "nminamtxx" : Index = 2
                    Case "ndiscrate" : Index = 3
                    Case "ndiscamtx" : Index = 4
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oCategrTable(Row)(Index)
        End Get
        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "scategrid" : Index = 1
                    Case "nminamtxx" : Index = 2
                        If Not IsNumeric(Value) Then Value = 0.0
                    Case "ndiscrate" : Index = 3
                        If Not IsNumeric(Value) Then Value = 0.0
                    Case "ndiscamtx" : Index = 4
                        If Not IsNumeric(Value) Then Value = 0.0
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                End Select
            End If
            p_oCategrTable(Row)(Index) = Value
        End Set
    End Property
#End Region

#Region "Public Function"
    Function GetDiscount() As Boolean
        Return True
    End Function

    Function OpenTransaction() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String

        lsSQL = "SELECT * FROM " & pxeMasterTble & _
                " WHERE sSourceNo = " & strParm(p_sSourceNo) & _
                    " AND sSourceCd = " & strParm(p_sSourceCd)

        If p_sSourceCd = "SO" Then
            Dim lsSQLTemp As String = AddCondition("SELECT * FROM SO_Master", "sTransNox = " & strParm(p_sSourceNo))
            loDT = p_oAppDrvr.ExecuteQuery(lsSQLTemp)
            If IFNull(loDT.Rows(0)("sMergeIDx"), "") <> "" Then
                lsSQL = "SELECT" & _
                            "  b.sTransNox" & _
                            ", a.sIDNumber" & _
                            ", a.sDiscCard" & _
                            ", a.nNoClient" & _
                            ", a.nWithDisc" & _
                            ", a.cNoneVatx" & _
                            ", c.sCategrID" & _
                            ", c.nMinAmtxx" & _
                            ", c.nDiscRate" & _
                            ", c.nDiscAmtx" & _
                        " FROM " & pxeMasterTble & " a" & _
                            ", SO_Master b" & _
                            ", Discount_Card_Detail c" & _
                        " WHERE a.sTransNox = b.sTransNox" & _
                            " AND b.sMergeIDx = " & strParm(loDT.Rows(0)("sMergeIDx")) & _
                            " AND a.sSourceCd = " & strParm(p_sSourceCd) & _
                            " AND a.sCardIDxx = c.sCardIDxx"
            End If
        End If

        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_bHasDiscount = False
            Return False
            Exit Function
        End If

        Call createMasterTable()
        Call createDetailTable()
        Call createCategoryTable()

        With p_oMasterTable
            .Rows.Add()
            .Rows(0)("sTransNox") = loDT.Rows(0)("sTransNox")
            .Rows(0)("sIDNumber") = loDT.Rows(0)("sIDNumber")
            .Rows(0)("sDiscCard") = loDT.Rows(0)("sDiscCard")
            .Rows(0)("nNoClient") = loDT.Rows(0)("nNoClient")
            .Rows(0)("nWithDisc") = loDT.Rows(0)("nWithDisc")
            .Rows(0)("nDiscRate") = loDT.Rows(0)("nDiscRate")
            .Rows(0)("nAddDiscx") = loDT.Rows(0)("nAddDiscx")
            .Rows(0)("cNoneVATx") = loDT.Rows(0)("cNoneVATx")
            .Rows(0)("sApproved") = loDT.Rows(0)("sApproved")
            .Rows(0)("sRemarksx") = loDT.Rows(0)("sRemarksx")

            'kalyptus - 2017.01.21 03:20pm
            'Include p_bWithVatx with the assignment here...
            p_bWithVatx = loDT.Rows(0)("cNoneVATx") = xeLogical.YES

            Dim loDataRow As DataRow
            loDataRow = p_oCard.SearchCard(.Rows(0)("sDiscCard"), True)

            If Not IsNothing(loDataRow) Then
                p_sCardDesc = loDataRow("sCardDesc")
                p_oCard.OpenRecord(loDataRow("sCardIDxx"))
            End If
        End With

        With p_oCategrTable
            For lnCtr As Integer = 0 To p_oCard.ItemCount - 1
                .Rows.Add()
                .Rows(lnCtr)("sTransNox") = p_oMasterTable.Rows(0)("sTransNox")
                .Rows(lnCtr)("sCategrID") = p_oCard.Detail(lnCtr, "sCategrID")
                .Rows(lnCtr)("nMinAmtxx") = p_oCard.Detail(lnCtr, "nMinAmtxx")
                .Rows(lnCtr)("nDiscRate") = p_oCard.Detail(lnCtr, "nDiscRate")
                .Rows(lnCtr)("nDiscAmtx") = p_oCard.Detail(lnCtr, "nDiscAmtx")
            Next lnCtr
        End With

        lsSQL = "SELECT" & _
                           "  sTransNox" & _
                           ", nEntryNox" & _
                           ", sIDNumber" & _
                           ", sClientNm" & _
                       " FROM " & pxeDetailTble & _
                       " WHERE sTransNox = " & strParm(p_oMasterTable(0)("sTransNox")) & _
                       " ORDER BY nEntryNox"

        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        With p_oDetailTable
            For lnCtr As Integer = 0 To loDT.Rows.Count - 1
                .Rows.Add()
                .Rows(lnCtr)("sTransNox") = loDT.Rows(lnCtr)("sTransNox")
                .Rows(lnCtr)("nEntryNox") = loDT.Rows(lnCtr)("nEntryNox")
                .Rows(lnCtr)("sIDNumber") = loDT.Rows(lnCtr)("sIDNumber")
                .Rows(lnCtr)("sClientNm") = loDT.Rows(lnCtr)("sClientNm")
            Next lnCtr
        End With

        p_bHasDiscount = True
        Return True
    End Function

    Function getUserDiscount() As Boolean
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

        If lbValid Then
            p_sUserIDxx = loDT.Rows(0).Item("sUserIDxx")
            p_sUserName = loDT.Rows(0).Item("sUserName")
            p_sLogNamex = loDT.Rows(0).Item("sLogNamex")
            p_nUserLevl = loDT.Rows(0).Item("nUserLevl")
        End If

        Return lbValid
    End Function

    Function SearchCard(Optional ByVal Value As Object = "") As Boolean
        Return GetCard(Value)
    End Function

    Function GetCard() As DataTable
        Return p_oCard.GetCard
    End Function

    Function AddDiscount() As Boolean
        Dim lnRow As Integer = p_oDetailTable.Rows.Count
        If lnRow <= 0 Then Return False

        'If Not SaveTransaction() Then Return False

        With p_oDetailTable
            .Rows.Add()
            .Rows(lnRow)("sTransNox") = p_oMasterTable(0)("sTransNox")
            .Rows(lnRow)("sIDNumber") = ""
            .Rows(lnRow)("sClientNm") = ""
        End With
        Return True
    End Function

    Function DeleteDiscount(ByVal Row As Integer) As Boolean
        With p_oDetailTable
            If .Rows.Count - 1 < Row Then Return False
            .Rows(Row).Delete()
        End With

        Return True
    End Function
#End Region

#Region "Private function"
    Function SaveTransaction() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim lsEntryNox As String

        With p_oMasterTable
            If p_bCancelled Then Return False

            'verify the required fields

            If p_oMasterTable.Rows(0)("sDiscCard") <> String.Empty Then
                'If p_oMasterTable.Rows(0)("sIDNumber") = String.Empty Then
                '    MsgBox("Invalid ID Number Detected!!!" & vbCrLf & vbCrLf & _
                '           "Verify your Entry then Try Again!!!", vbCritical, "Warning")
                '    Return False
                'End If
            Else
                If p_oCategrTable.Rows.Count = 1 Then
                    If p_oCategrTable.Rows(0)("nDiscRate") <= 0 And p_oCategrTable.Rows(0)("nAddDiscx") <= 0 Then Return True
                Else
                    Dim lbWithDiscount As Boolean
                    For lnCtr As Integer = 0 To p_oCategrTable.Rows.Count - 1
                        If Not lbWithDiscount Then
                            If p_oCategrTable.Rows(lnCtr)("nDiscRate") > 0 And p_oCategrTable.Rows(lnCtr)("nAddDiscx") > 0 Then
                                lbWithDiscount = True
                            End If
                        End If
                        If Not lbWithDiscount Then Return True
                    Next

                    If lbWithDiscount Then
                        If Not getUserDiscount() Then Return False
                    End If
                End If
            End If

            p_nNoClient = p_oMasterTable.Rows(0)("nNoClient")
            p_nWithDisc = p_oMasterTable.Rows(0)("nWithDisc")
            lsSQL = "INSERT INTO " & pxeMasterTble & " SET" & _
                        "  sTransNox = " & strParm(.Rows(0)("sTransNox")) & _
                        ", sIDNumber = " & strParm(.Rows(0)("sIDNumber")) & _
                        ", sDiscCard = " & strParm(.Rows(0)("sDiscCard")) & _
                        ", nNoClient = " & p_nNoClient & _
                        ", nWithDisc = " & p_nWithDisc & _
                        ", cNoneVATx = " & strParm(IIf(p_bWithVatx, 1, 0)) & _
                        ", sRemarksx = " & strParm(.Rows(0)("sRemarksx")) & _
                        ", sApproved = " & strParm(p_sUserIDxx) & _
                        ", sSourceCd = " & strParm(p_sSourceCd) & _
                        ", sSourceNo = " & strParm(p_sSourceNo) & _
                    " ON DUPLICATE KEY UPDATE" & _
                        "  sIDNumber = " & strParm(.Rows(0)("sIDNumber")) & _
                        ", sDiscCard = " & strParm(.Rows(0)("sDiscCard")) & _
                        ", nNoClient = " & p_nNoClient & _
                        ", nWithDisc = " & p_nWithDisc &
                        ", cNoneVATx = " & strParm(IIf(p_bWithVatx, 1, 0)) & _
                        ", sRemarksx = " & strParm(.Rows(0)("sRemarksx")) & _
                        ", sApproved = " & strParm(p_sUserIDxx) & _
                        ", sSourceCd = " & strParm(p_sSourceCd) & _
                        ", sSourceNo = " & strParm(p_sSourceNo)

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
        End With

        With p_oDetailTable
            For lnCtr As Integer = 0 To .Rows.Count - 1
                If .Rows(lnCtr)("sIDNumber") = "" Then
                    lsEntryNox = lsEntryNox & lnCtr + 1 & ","
                End If
                lsSQL = "INSERT INTO " & pxeDetailTble & " SET" & _
                                "  sTransNox = " & strParm(.Rows(lnCtr)("sTransNox")) & _
                                ", nEntryNox = " & CDbl(lnCtr + 1) & _
                                ", sIDNumber = " & strParm(.Rows(lnCtr)("sIDNumber")) & _
                                ", sClientNm = " & strParm(.Rows(lnCtr)("sClientNm")) & _
                                ", dModified = " & dateParm(p_oAppDrvr.getSysDate()) & _
                            " ON DUPLICATE KEY UPDATE" & _
                                "  sIDNumber = " & strParm(.Rows(lnCtr)("sIDNumber")) & _
                                ", sClientNm = " & strParm(.Rows(lnCtr)("sClientNm"))
                Try
                    lnRow = p_oAppDrvr.Execute(lsSQL, pxeDetailTble)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            Next

            If lsEntryNox <> "" Then
                lsSQL = "DELETE FROM " & pxeDetailTble &
                            " WHERE sTransNox = " & strParm(p_oMasterTable.Rows(0)("sTransNox")) &
                                " AND nEntryNox IN (" & Mid(lsEntryNox, 1, Len(lsEntryNox) - 1) & ")"
                p_oAppDrvr.Execute(lsSQL, pxeDetailTble)

                If p_oDetailTable.Rows.Count = 1 And p_oDetailTable.Rows(0)("sIDNumber") = "" Then
                    lsSQL = "DELETE FROM " & pxeMasterTble &
                            " WHERE sTransNox = " & strParm(p_oMasterTable.Rows(0)("sTransNox"))
                    p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                End If
            End If
        End With

        p_oAppDrvr.SaveEvent("0012", "Order TN " & p_sSourceNo & "/" & p_sSourceCd & "/" & _
                                    IFNull(p_oMasterTable.Rows(0)("sDiscCard")) & "/" & _
                                    p_oMasterTable.Rows(0)("nDiscRate") & "%/" & _
                                    p_oMasterTable.Rows(0)("nAddDiscx"), p_sSerial)

        p_bHasDiscount = True
        Return True
    End Function


    Function CancelDiscount() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer

        With p_oMasterTable
            'If Not p_bCancelled Then Return False

            'verify the required fields
            If p_bHasDiscount = False Then GoTo endProc

            lsSQL = "DELETE FROM " & pxeMasterTble & _
                        " WHERE sTransNox = " & strParm(.Rows(0)("sTransNox"))

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
        End With

        With p_oDetailTable
            For lnCtr As Integer = 0 To .Rows.Count - 1
                lsSQL = "DELETE FROM " & pxeDetailTble & _
                                " WHERE sTransNox = " & strParm(.Rows(lnCtr)("sTransNox"))
                Try
                    lnRow = p_oAppDrvr.Execute(lsSQL, pxeDetailTble)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            Next
        End With

endProc:
        p_bHasDiscount = False

        Return True
    End Function

    Private Function getCard(ByVal Value As String) As Boolean
        Dim lsCondition As String
        Dim lsProcName As String
        Dim loDataRow As DataRow

        lsProcName = "getCard"

        lsCondition = String.Empty

        If Value <> String.Empty Then
            If Value = p_sCardDesc Then Return True
        End If

        loDataRow = p_oCard.SearchCard(Value, False)
        createCategoryTable()

        p_oCategrTable.Rows.Add()
        If Not IsNothing(loDataRow) Then
            If p_oAppDrvr.SysDate > loDataRow("dExpiratn") Then
                p_oMasterTable(0)("sDiscCard") = loDataRow("sCardIDxx")
                p_sCardDesc = ""
                p_bWithVatx = False

                MsgBox("Card partnership is already expired!!!" & vbCrLf & vbCrLf & _
                       "Please contact your company card for updates!!!", vbCritical, "Warning")
                Return False
            End If

            p_oMasterTable(0)("sDiscCard") = loDataRow("sCardIDxx")
            p_sCardDesc = loDataRow("sCardDesc")
            p_bWithVatx = loDataRow("cNoneVatx") = xeLogical.YES
            'p_oDataTable(0)("nDiscRate") = loDataRow("nDiscRate")
            'p_oDataTable(0)("nAddDiscx") = loDataRow("nDiscAmtx")
            'p_bWithVatx = loDataRow("cNoneVatx") = xeLogical.NO
            p_oCategrTable(0)("nDiscRate") = 0.0
            p_oCategrTable(0)("nDiscAmtx") = 0.0
            p_oCategrTable(0)("sCategrID") = ""
            p_oCategrTable(0)("nMinAmtxx") = 0.0
            If p_oCard.OpenRecord(loDataRow("sCardIDxx")) Then
                For lnCtr As Integer = 0 To p_oCard.ItemCount - 1
                    p_oCategrTable.Rows(lnCtr)("sTransNox") = p_oMasterTable.Rows(0)("sTransNox")
                    p_oCategrTable.Rows(lnCtr)("sCategrID") = p_oCard.Detail(lnCtr, "sCategrID")
                    p_oCategrTable.Rows(lnCtr)("nMinAmtxx") = p_oCard.Detail(lnCtr, "nMinAmtxx")
                    p_oCategrTable.Rows(lnCtr)("nDiscRate") = p_oCard.Detail(lnCtr, "nDiscRate")
                    p_oCategrTable.Rows(lnCtr)("nDiscAmtx") = p_oCard.Detail(lnCtr, "nDiscAmtx")
                    If lnCtr < p_oCard.ItemCount - 1 Then p_oCategrTable.Rows.Add()
                Next lnCtr

                'If p_oCard.ItemCount > 1 Then
                '    p_oMasterTable.Rows(0)("nDiscRate") = 0.0
                '    p_oMasterTable.Rows(0)("nAddDiscx") = 0.0
                '    p_oMasterTable.Rows(0)("nMinAmtxx") = 0.0
                'Else
                p_oMasterTable.Rows(0)("nDiscRate") = p_oCard.Detail(0, "nDiscRate")
                p_oMasterTable.Rows(0)("nAddDiscx") = p_oCard.Detail(0, "nDiscAmtx")
                p_oMasterTable.Rows(0)("nMinAmtxx") = p_oCard.Detail(0, "nMinAmtxx")
                'End If
            End If
        Else
            p_oMasterTable(0)("sIDNumber") = ""
            p_oMasterTable(0)("sDiscCard") = ""
            p_oMasterTable(0)("nNoClient") = 0
            p_oMasterTable(0)("nWithDisc") = 0.0
            p_oMasterTable(0)("cNoneVATx") = ""

            p_oCategrTable(0)("sCategrID") = ""
            p_oCategrTable(0)("nMinAmtxx") = 0.0
            p_oCategrTable(0)("nDiscRate") = 0.0
            p_oCategrTable(0)("nDiscAmtx") = 0.0

            p_sCardDesc = ""
            p_bWithVatx = False
            RaiseEvent MasterRetrieved(2, String.Empty)
        End If

        RaiseEvent MasterRetrieved(3, p_oMasterTable(0)("nNoClient"))
        RaiseEvent MasterRetrieved(4, p_oMasterTable(0)("nWithDisc"))
        RaiseEvent MasterRetrieved(5, p_oMasterTable(0)("nDiscRate"))
        RaiseEvent MasterRetrieved(6, p_oMasterTable(0)("nAddDiscx"))
        RaiseEvent MasterRetrieved(10, p_oMasterTable(0)("nMinAmtxx"))

        Return True
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
#End Region

#Region "Public Procedures"
    Sub ShowDiscount()
        'p_oCard.FilterCategory = p_sCategIDx
        p_oFormDiscount = New frmDiscount
        With p_oFormDiscount
            .Discount = Me
            .TopMost = True
            .ShowDialog()
        End With
    End Sub
#End Region

#Region "Private Procedures"
    Private Sub createMasterTable()
        p_oMasterTable = New DataTable

        With p_oMasterTable
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("sIDNumber", GetType(String)).MaxLength = 16
            .Columns.Add("sDiscCard", GetType(String)).MaxLength = 4
            .Columns.Add("nNoClient", GetType(Integer))
            .Columns.Add("nWithDisc", GetType(Integer))
            .Columns.Add("nDiscRate", GetType(Decimal))
            .Columns.Add("nAddDiscx", GetType(Decimal))
            .Columns.Add("cNoneVATx", GetType(String)).MaxLength = 1
            .Columns.Add("sApproved", GetType(String)).MaxLength = 10
            .Columns.Add("sRemarksx", GetType(String)).MaxLength = 64
            .Columns.Add("nMinAmtxx", GetType(Decimal))
        End With
    End Sub

    Private Sub createDetailTable()
        p_oDetailTable = New DataTable

        With p_oDetailTable
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("nEntryNox", GetType(Integer))
            .Columns.Add("sIDNumber", GetType(String)).MaxLength = 16
            .Columns.Add("sClientNm", GetType(String)).MaxLength = 120
        End With
    End Sub

    Private Sub createCategoryTable()
        p_oCategrTable = New DataTable

        With p_oCategrTable
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("sCategrID", GetType(String)).MaxLength = 7
            .Columns.Add("nMinAmtxx", GetType(Decimal))
            .Columns.Add("nDiscRate", GetType(Decimal))
            .Columns.Add("nDiscAmtx", GetType(Decimal))
        End With
    End Sub

    Private Sub initMaster()
        With p_oMasterTable
            .Rows.Add()
            .Rows(0)("sTransNox") = getNextTransNo()
            .Rows(0)("sIDNumber") = ""
            .Rows(0)("sDiscCard") = ""
            .Rows(0)("nNoClient") = 0
            .Rows(0)("nWithDisc") = 0.0
            .Rows(0)("nDiscRate") = 0.0
            .Rows(0)("nAddDiscx") = 0.0
            .Rows(0)("nMinAmtxx") = 0.0
            .Rows(0)("cNoneVATx") = ""
            .Rows(0)("sApproved") = p_nUserLevl
            .Rows(0)("sRemarksx") = ""
        End With
    End Sub

    Private Sub initDetail()
        With p_oDetailTable
            .Rows.Add()
            .Rows(0)("sTransNox") = p_oMasterTable(0)("sTransNox")
            .Rows(0)("nEntryNox") = 0
            .Rows(0)("sIDNumber") = ""
            .Rows(0)("sClientNm") = ""
        End With
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider

        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode
        p_oCard = New clsDiscountCards(p_oAppDrvr)
        p_sUserIDxx = p_oAppDrvr.UserID

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub

    Public Sub InitTransaction()
        createMasterTable()
        createDetailTable()
        createCategoryTable()
        initMaster()
        initDetail()

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub
End Class