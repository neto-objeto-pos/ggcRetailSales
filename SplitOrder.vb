'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Split Order
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
Imports MySql.Data.MySqlClient
Imports ggcRetailParams

Public Class SplitOrder

#Region "Constant"
    Private Const pxeMODULENAME As String = "SplitBills"
    Private Const pxeMasterTble As String = "Order_Split"
    Private Const pxeDetailTble As String = "Order_Split_Detail"
    Private Const pxeSourceCde As String = "SOSp"
#End Region

#Region "SplitType"
    Public Enum xeSplitType
        xeSplitByPercentage = 0
        xeSplitByAmount = 1
        xeSplitByMenu = 2
    End Enum
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oDTMaster As DataTable
    Protected p_oDTDetail As DataTable
    Protected p_oDTTempxx As DataTable
    Protected p_oDTOrder As DataTable
    Protected p_oDT As DataTable
    Protected p_nEditMode As xeEditMode
    Protected p_oFormSplit As frmOrderSplit
    Protected p_oInventory As clsInventory

    Private p_oDiscount As Discount
    Private p_oDtaDiscx As DataTable

    Protected p_sBranchCd As String
    Protected p_cSplitTyp As xeSplitType
    Protected p_nGroupNox As Integer = 1
    Protected p_nSetNumbr As Integer = 1
    Protected p_bCancelled As Boolean
    Protected p_sTermnl As String
    Protected p_sSourceNo As String
    Protected p_bNewRecdx As Boolean
    Protected p_bWasSplitted As Boolean
    Protected p_bWasSpltPost As Boolean
    Protected p_nSalesTotl As Double
    Protected p_nSChargexx As Double
    Protected p_bSChargexx As Boolean
#End Region

#Region "Properties"
    Property Branch() As String
        Get
            Return p_sBranchCd
        End Get
        Set(ByVal Value As String)
            p_sBranchCd = Value
        End Set
    End Property



    Property SalesTotal() As Double
        Get
            Return p_nSalesTotl
        End Get
        Set(ByVal Value As Double)
            p_nSalesTotl = Value
        End Set
    End Property


    Property SplitType As xeSplitType
        Get
            Return p_cSplitTyp
        End Get
        Set(ByVal Value As xeSplitType)
            If p_cSplitTyp <> Value Then
                p_cSplitTyp = Value
                createDetailTable()
                procDataTable(p_oDTTempxx)
            End If
        End Set
    End Property

    ReadOnly Property Cancelled As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    ReadOnly Property WasSplitted As Boolean
        Get
            Return p_bWasSplitted
        End Get
    End Property

    ReadOnly Property WasSplitPosted As Boolean
        Get
            Return p_bWasSpltPost
        End Get
    End Property

    Property GroupNo As Integer
        Get
            Return p_nGroupNox
        End Get
        Set(ByVal Value As Integer)
            If Value <= 0 Then
                MsgBox("Invalid Group Entry..." & vbCrLf & _
                            "Group must be greater than zero(0)...", MsgBoxStyle.Critical, "Warning")
                Value = 1
            End If

            If Value <> p_nGroupNox Then
                p_nGroupNox = Value
                procDataTable(p_oDTTempxx)
                computeMaster()
            End If
        End Set
    End Property

    Property SetNo As Integer
        Get

            Return p_nSetNumbr
        End Get
        Set(ByVal Value As Integer)
            If Value <= 0 Then
                MsgBox("Invalid Set Number..." & vbCrLf & _
                            "Set Number must be greater than zero(0)...", MsgBoxStyle.Critical, "Warning")
                Value = 1
            End If

            If Value <> p_nSetNumbr Then
                p_nSetNumbr = Value
                computeMaster()
            End If
        End Set
    End Property

    ReadOnly Property TransNo
        Get
            Return p_oDTMaster(0)("sTransNox")
        End Get
    End Property

    WriteOnly Property POSNumbr As String
        Set(ByVal Value As String)
            p_sTermnl = Value
        End Set
    End Property

    WriteOnly Property OrderDetail As DataTable
        Set(ByVal Value As DataTable)
            p_oDTOrder = Value
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

    ReadOnly Property ItemCount() As Long
        Get
            Return p_oDTDetail.Rows.Count
        End Get
    End Property

    ReadOnly Property MasterCount() As Long
        Get
            Return p_oDTMaster.Rows.Count
        End Get
    End Property

    Property SCharge As Double
        Get
            Return p_nSChargexx
        End Get
        Set(ByVal Value As Double)
            p_nSChargexx = Value
        End Set
    End Property

    Property IsWithSCharge As Boolean
        Get
            Return p_bSChargexx
        End Get
        Set(ByVal Value As Boolean)
            p_bSChargexx = Value
        End Set
    End Property

    ReadOnly Property Master(ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "cpaymform" : Index = 1
                    Case "srefernox" : Index = 2
                    Case "namountxx" : Index = 3
                    Case "csplittyp" : Index = 4
                    Case "cpaidxxxx" : Index = 5
                    Case "ntrantotl" : Index = 6
                    Case "ndiscntbl" : Index = 7
                    Case "nzeroratd" : Index = 8
                    Case "nvatsales" : Index = 9
                    Case "nvatamtxx" : Index = 10
                    Case "nnonvatxx" : Index = 11
                    Case "nvatdiscx" : Index = 12
                    Case "npwddiscx" : Index = 13
                    Case "ndiscount" : Index = 14
                    Case "nvoidtotl" : Index = 15
                    Case "nprntbill" : Index = 16
                    Case "dprntbill" : Index = 17
                    Case "sbillnmbr" : Index = 18
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If

            Return p_oDTMaster.Rows(p_nSetNumbr - 1)(Index)
        End Get
    End Property

    Property Detail(ByVal Row As Integer, _
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "nentrynox" : Index = 0
                    Case "sbarcodex" : Index = 1
                        If p_oDTDetail.Rows(Row)("sStockIDx") = "" Then Return String.Empty
                        If p_oInventory.OpenRecord(p_oDTDetail.Rows(Row)("sStockIDx")) Then
                            Return p_oInventory.Master("sBarcodex")
                        Else
                            Return String.Empty
                        End If
                    Case "sdescript" : Index = 2
                        If p_oDTDetail.Rows(Row)("sStockIDx") = "" Then Return String.Empty
                        If p_oInventory.OpenRecord(p_oDTDetail.Rows(Row)("sStockIDx")) Then
                            Return p_oInventory.Master("sDescript")
                        Else
                            Return String.Empty
                        End If
                    Case "sbriefdsc" : Index = 3
                        If p_oDTDetail.Rows(Row)("sStockIDx") = "" Then Return String.Empty
                        If p_oInventory.OpenRecord(p_oDTDetail.Rows(Row)("sStockIDx")) Then
                            Return p_oInventory.Master("sBriefDsc")
                        Else
                            Return String.Empty
                        End If
                    Case "nunitprce" : Index = 4
                    Case "creversex" : Index = 5
                    Case "nquantity" : Index = 6
                    Case "ndiscount" : Index = 7
                    Case "nadddiscx" : Index = 8
                    Case "cprintedx" : Index = 9
                    Case "ncomplmnt" : Index = 10
                    Case "ndisclev1" : Index = 11
                    Case "ndisclev2" : Index = 12
                    Case "ndisclev3" : Index = 13
                    Case "ndiscamtx" : Index = 14
                    Case "ndealrdsc" : Index = 15
                    Case "cwthpromo" : Index = 16
                    Case "ccombomlx" : Index = 17
                    Case "cdetailxx" : Index = 18
                    Case "sstockidx" : Index = 19
                        If p_oDTDetail.Rows(Row)("sStockIDx") = "" Then Return String.Empty
                        If p_oInventory.OpenRecord(p_oDTDetail.Rows(Row)("sStockIDx")) Then
                            Return p_oInventory.Master("sBriefDsc")
                        Else
                            Return String.Empty
                        End If
                    Case "scategrid" : Index = 20
                    Case "cforwardx" : Index = 21
                    Case "cdetSaved" : Index = 22
                    Case "creversed" : Index = 23
                    Case "cservedxx" : Index = 24
                    Case "sreplitem" : Index = 25
                    Case "dmodified" : Index = 26
                    Case "stransnox" : Index = 27
                    Case Else
                        If Left(Index, 6) = "ngrpqt" Then
                            For nCtr As Integer = 1 To p_nGroupNox
                                If Index = "ngrpqt" & Format(nCtr, "000") Then
                                    Index = (((nCtr - 1) * 3) + 4) + 27
                                    Exit For
                                Else
                                    If nCtr = p_nGroupNox Then
                                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                                        Return DBNull.Value
                                    End If
                                End If
                            Next
                        ElseIf Left(Index, 6) = "ngrpam" Then
                            For nCtr As Integer = 1 To p_nGroupNox
                                If Index = "ngrpam" & Format(nCtr, "000") Then
                                    Index = (((nCtr - 1) * 3) + 5) + 27
                                    Exit For
                                Else
                                    If nCtr = p_nGroupNox Then
                                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                                        Return DBNull.Value
                                    End If
                                End If
                            Next
                        ElseIf Left(Index, 6) = "strans" Then
                            For nCtr As Integer = 1 To p_nGroupNox
                                If Index = "strans" & Format(nCtr, "000") Then
                                    'Index = nCtr + 24
                                    Index = (((nCtr - 1) * 3) + 2) + 27
                                    Exit For
                                Else
                                    If nCtr = p_nGroupNox Then
                                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                                        Return DBNull.Value
                                    End If
                                End If
                            Next
                        Else
                            MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                            Return DBNull.Value
                        End If
                End Select
            End If

            Return p_oDTDetail.Rows(Row)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "nunitprce" : Index = 4
                    Case "nquantity" : Index = 6
                    Case "sstockidx" : Index = 19
                    Case Else
                        If Left(Index, 6) = "ngrpqt" Then
                            For nCtr As Integer = 1 To p_nGroupNox
                                If Index = "ngrpqt" & Format(nCtr, "000") Then
                                    Index = (((nCtr - 1) * 3) + 4) + 27
                                    Exit For
                                Else
                                    If nCtr = p_nGroupNox Then
                                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                                        Exit Property
                                    End If
                                End If
                            Next

                            p_oDTDetail.Rows(Row)(Index) = Value
                            computeMaster()
                            Exit Property
                        ElseIf Left(Index, 6) = "ngrpam" Then
                            For nCtr As Integer = 1 To p_nGroupNox
                                If Index = "ngrpam" & Format(nCtr, "000") Then
                                    Index = (((nCtr - 1) * 3) + 5) + 27
                                    Exit For
                                Else
                                    If nCtr = p_nGroupNox Then
                                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                                        Exit Property
                                    End If
                                End If
                            Next

                            p_oDTDetail.Rows(Row)(Index) = Value
                            computeMaster()
                            Exit Property
                        ElseIf Left(Index, 6) = "strans" Then
                            For nCtr As Integer = 1 To p_nGroupNox
                                If Index = "strans" & Format(nCtr, "000") Then
                                    Index = (((nCtr - 1) * 3) + 2) + 27
                                    Exit For
                                Else
                                    If nCtr = p_nGroupNox Then
                                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                                        Exit Property
                                    End If
                                End If
                            Next
                        Else
                            MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                            Exit Property
                        End If
                End Select
            End If

            p_oDTDetail.Rows(Row)(Index) = Value
        End Set
    End Property
#End Region

#Region "Public Function"
    Function SaveTransaction() As Boolean
        If p_bCancelled Then Return False

        If p_cSplitTyp = xeSplitType.xeSplitByMenu Then
            Return saveByMenu()
        Else
            Return saveByAmount()
        End If
    End Function

    Private Sub addMaster(Optional ByVal lnRow As Integer = 0)
        With p_oDTMaster
            .Rows.Add()
            .Rows(lnRow)("sTransNox") = ""
            .Rows(lnRow)("cPaymForm") = ""
            .Rows(lnRow)("sReferNox") = ""
            .Rows(lnRow)("nAmountxx") = 0.0
            .Rows(lnRow)("cPaidxxxx") = 0
            .Rows(lnRow)("nTranTotl") = 0.0
            .Rows(lnRow)("nDiscntbl") = 0.0
            .Rows(lnRow)("nZeroRatd") = 0.0
            .Rows(lnRow)("nVATSales") = 0.0
            .Rows(lnRow)("nVATAmtxx") = 0.0
            .Rows(lnRow)("nNonVatxx") = 0.0
            .Rows(lnRow)("nVATDiscx") = 0.0
            .Rows(lnRow)("nPWDDiscx") = 0.0
            .Rows(lnRow)("nDiscount") = 0.0
            .Rows(lnRow)("nVoidTotl") = 0.0

            Select Case p_cSplitTyp
                Case xeSplitType.xeSplitByPercentage
                    .Rows(lnRow)("cSplitTyp") = 0
                Case xeSplitType.xeSplitByAmount
                    .Rows(lnRow)("cSplitTyp") = 1
                Case xeSplitType.xeSplitByMenu
                    .Rows(lnRow)("cSplitTyp") = 2
            End Select
        End With
    End Sub

    Function OpenBySource() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String

        lsSQL = AddCondition("SELECT * FROM SO_Master", "sTransNox = " & strParm(p_sSourceNo))
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return False

        If IFNull(loDT.Rows(0)("sMergeIDx"), "") = "" Then
            lsSQL = AddCondition("SELECT * FROM " & pxeMasterTble, "sReferNox = " & strParm(p_sSourceNo) & _
                                 " ORDER BY sTransNox")
        Else
            lsSQL = "SELECT a.* FROM " & pxeMasterTble & " a" & _
                        ", SO_Master b" & _
                    " WHERE a.sReferNox = b.sTransNox" & _
                        " AND b.sMergeIDx = " & strParm(loDT.Rows(0)("sMergeIDx")) & _
                    " ORDER BY a.sTransNox"
        End If

        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        Call createMasterTable()

        If loDT.Rows.Count = 0 Then
            p_nGroupNox = 1
            p_cSplitTyp = xeSplitType.xeSplitByAmount
            p_bNewRecdx = True
            p_bWasSplitted = False

            Call addMaster()

            p_nSetNumbr = 1
            p_oDTTempxx = p_oDTOrder
            procDataTable(p_oDTTempxx)
        Else
            p_sSourceNo = loDT.Rows(0)("sReferNox")
            lsSQL = "SELECT" & _
                        "  COUNT(a.sTransNox) nGroupNox" & _
                        ", b.cSplitTyp" & _
                        ", a.sTransNox" & _
                        ", c.sTransNox xSourceNo" & _
                    " FROM SO_Master a" & _
                        ", Order_Split b" & _
                            " LEFT JOIN Receipt_Master c" & _
                                        " ON b.sTransNox = c.sSourceNo" & _
                                        " AND c.sSourceCd = 'SOSp'" & _
                    " WHERE a.sTransNox = b.sReferNox" & _
                        " AND a.sTransNox = " & strParm(p_sSourceNo)

            loDT = New DataTable
            loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

            p_nGroupNox = loDT.Rows(0)("nGroupNox")
            p_cSplitTyp = loDT.Rows(0)("cSplitTyp")

            lsSQL = "SELECT" & _
                        "  a.sTransNox" & _
                        ", a.cPaymForm" & _
                        ", a.sReferNox" & _
                        ", a.nAmountxx" & _
                        ", a.cSplitTyp" & _
                        ", IF(b.sTransNox IS NULL, 0, 1) xPaidxxxx" & _
                    " FROM Order_Split a" & _
                         " LEFT JOIN Receipt_Master b" & _
                            " ON a.sTransNox = b.sSourceNo" & _
                            " AND b.sSourceCd = 'SOSp'" & _
                    " WHERE a.sReferNox = " & strParm(p_sSourceNo)

            loDT = New DataTable
            loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

            p_nSetNumbr = 1
            p_oDTTempxx = p_oDTOrder
            procDataTable(p_oDTTempxx)
            'createMasterTable()
            With p_oDTMaster
                For nCtr As Integer = 0 To loDT.Rows.Count - 1
                    .Rows.Add()
                    .Rows(nCtr)("sTransNox") = loDT.Rows(nCtr)("sTransNox")
                    .Rows(nCtr)("cPaymForm") = loDT.Rows(nCtr)("cPaymForm")
                    .Rows(nCtr)("sReferNox") = loDT.Rows(nCtr)("sReferNox")
                    If (loDT.Rows(nCtr)("cSplitTyp") <> 2) Then
                        .Rows(nCtr)("nAmountxx") = (IIf(IsWithSCharge, Math.Round(loDT.Rows(nCtr)("nAmountxx") / 1.17, 2), Math.Round(loDT.Rows(nCtr)("nAmountxx") / 1.12, 2)))
                    Else
                    .Rows(nCtr)("nAmountxx") = loDT.Rows(nCtr)("nAmountxx")
        End If
        .Rows(nCtr)("cSplitTyp") = loDT.Rows(nCtr)("cSplitTyp")
        .Rows(nCtr)("cPaidxxxx") = loDT.Rows(nCtr)("xPaidxxxx")
                    .Rows(nCtr)("nTranTotl") = loDT.Rows(nCtr)("nAmountxx")
                    .Rows(nCtr)("nDiscntbl") = 0.0
                    .Rows(nCtr)("nZeroRatd") = 0.0
                    .Rows(nCtr)("nVATSales") = 0.0
                    .Rows(nCtr)("nVATAmtxx") = 0.0
                    .Rows(nCtr)("nNonVatxx") = 0.0
                    .Rows(nCtr)("nVATDiscx") = 0.0
                    .Rows(nCtr)("nPWDDiscx") = 0.0
                    .Rows(nCtr)("nDiscount") = 0.0
                    .Rows(nCtr)("nVoidTotl") = 0.0
                Next nCtr
            End With

            p_bNewRecdx = False
            p_bWasSplitted = True
            computeMaster()
        End If

        Return checkPosted()
    End Function

    Private Function checkPosted() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCtr As Integer

        lsSQL = AddCondition("SELECT * FROM " & pxeMasterTble, "sReferNox = " & strParm(p_sSourceNo) & _
                             " ORDER BY sTransNox")

        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        p_bWasSpltPost = False
        For lnCtr = 0 To loDT.Rows.Count - 1
            If Not IsDBNull(loDT.Rows(lnCtr)("cTranStat")) Then
                p_bWasSpltPost = True
                Return True
            End If
        Next

        Return True
    End Function

    Function OpenTransaction(ByVal fsTransNox As String) As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String

        lsSQL = "SELECT" & _
                    "  a.sTransNox" & _
                    ", a.cPaymForm" & _
                    ", a.sReferNox" & _
                    ", a.nAmountxx" & _
                    ", a.cSplitTyp" & _
                    ", IF(b.sTransNox IS NULL, 0, 1) xPaidxxxx" & _
                    ", a.nPrntBill" & _
                    ", a.dPrntBill" & _
                " FROM Order_Split a" & _
                     " LEFT JOIN Receipt_Master b" & _
                        " ON a.sTransNox = b.sSourceNo" & _
                        " AND b.sSourceCd = 'SOSp'" & _
                " WHERE a.sTransNox = " & strParm(fsTransNox)

        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then Return False

        createMasterTable()
        With p_oDTMaster
            .Rows.Add()
            .Rows(0)("sTransNox") = loDT.Rows(0)("sTransNox")
            .Rows(0)("cPaymForm") = loDT.Rows(0)("cPaymForm")
            .Rows(0)("sReferNox") = loDT.Rows(0)("sReferNox")
            .Rows(0)("nAmountxx") = loDT.Rows(0)("nAmountxx")
            .Rows(0)("cSplitTyp") = loDT.Rows(0)("cSplitTyp")
            .Rows(0)("cPaidxxxx") = loDT.Rows(0)("xPaidxxxx")
            .Rows(0)("nTranTotl") = loDT.Rows(0)("nAmountxx")
            .Rows(0)("nPrntBill") = loDT.Rows(0)("nPrntBill")
            .Rows(0)("dPrntBill") = loDT.Rows(0)("dPrntBill")
            .Rows(0)("nDiscntbl") = 0.0
            .Rows(0)("nZeroRatd") = 0.0
        End With

        lsSQL = "SELECT" & _
                    "  sTransNox" & _
                    ", nEntryNox" & _
                    ", sStockIDx" & _
                    ", nQuantity" & _
                    ", nUnitPrce" & _
                    ", nSrcEntry" & _
                " FROM Order_Split_Detail" & _
                " WHERE sTransNox = " & strParm(fsTransNox) & _
                " ORDER BY nEntryNox"

        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        Call createDetailTable()
        With p_oDTDetail
            For nCtr As Integer = 0 To loDT.Rows.Count - 1
                .Rows.Add()
                .Rows(nCtr)("sTransNox") = loDT.Rows(nCtr)("sTransNox")
                .Rows(nCtr)("nEntryNox") = loDT.Rows(nCtr)("nEntryNox")
                .Rows(nCtr)("sStockIDx") = loDT.Rows(nCtr)("sStockIDx")
                .Rows(nCtr)("nQuantity") = loDT.Rows(nCtr)("nQuantity")
                .Rows(nCtr)("nUnitPrce") = loDT.Rows(nCtr)("nUnitPrce")
                .Rows(nCtr)("nSrcEntry") = loDT.Rows(nCtr)("nSrcEntry")
            Next nCtr
        End With

        Call LoadDiscount()
        'Recompute total
        computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

        Return True
    End Function
#End Region

#Region "Private function"
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
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sTermnl & Format(p_oAppDrvr.getSysDate(), "yy") & "%") & _
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

            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)

            lsSQL = loDT.Rows(0).Item("sTransNox")
            lnLen = Len(lsSQL)

            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oAppDrvr.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function

    Private Function getSQL_Master() As String
        Return "SELECT" & _
                    "  sTransNox" & _
                    ", cPaymForm" & _
                    ", sReferNox" & _
                    ", nAmountxx" & _
                    ", cSplitTyp" & _
                    ", nPrntBill" & _
                    ", dPrntBill" & _
                " FROM " & pxeMasterTble
    End Function

    Private Function getRemQty(ByVal fsReferNox As String, _
                                ByVal fsStockIDx As String, _
                                ByVal fnQuantity As Integer, _
                                ByVal fnComplmnt As Integer, _
                                ByVal fnEntryNox As Integer) As Integer
        Dim lsSQL As String
        Dim loDt As New DataTable

        lsSQL = "SELECT" & _
                    "  b.nQuantity" & _
                    ", c.sTransNox" & _
                " FROM Order_Split a" & _
                    " LEFT JOIN Receipt_Master c" & _
                        " ON a.sTransNox = c.sSourceNo" & _
                        " AND c.sSourceCd = 'SOSp'" & _
                    ", Order_Split_Detail b" & _
                " WHERE a.sTransNox = b.sTransNox" & _
                    " AND a.sReferNox = " & strParm(fsReferNox) & _
                    " AND b.sStockIDx = " & strParm(fsStockIDx) & _
                    " AND b.nSrcEntry = " & CDbl(fnEntryNox) & _
                " HAVING NOT sTransNox IS NULL"

        loDt = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDt.Rows.Count = 0 Then
            Return fnQuantity
        Else
            With loDt
                Return fnQuantity - (fnComplmnt + loDt.Rows(0)("nQuantity"))
            End With
        End If
    End Function

    Private Function validateDetail(ByVal fsStockIDx As String, _
                                    ByVal fnEntryNox As Integer) As DataTable
        Dim lsSQL As String
        Dim loDt As New DataTable

        If p_cSplitTyp = xeSplitType.xeSplitByMenu Then
            'lsSQL = "SELECT" & _
            '            "  b.nQuantity" & _
            '            ", b.sTransNox" & _
            '            ", b.nUnitPrce" & _
            '        " FROM Order_Split a" & _
            '            ", Order_Split_Detail b" & _
            '        " WHERE a.sTransNox = b.sTransNox" & _
            '            " AND a.sReferNox = " & strParm(p_sSourceNo) & _
            '            " AND b.sStockIDx = " & strParm(fsStockIDx) & _
            '            " AND b.nSrcEntry = " & CDbl(fnEntryNox) & _
            '        " ORDER BY a.sTransNox"

            lsSQL = "SELECT" & _
                        "  b.nQuantity" & _
                        ", b.sTransNox" & _
                        ", b.nUnitPrce" & _
                    " FROM Order_Split a" & _
                        ", Order_Split_Detail b" & _
                    " WHERE a.sTransNox = b.sTransNox" & _
                        " AND a.sReferNox = " & strParm(p_sSourceNo) & _
                        " AND b.sStockIDx = " & strParm(fsStockIDx) & _
                    " ORDER BY a.sTransNox"
        Else
            lsSQL = "SELECT" &
                        "  b.nQuantity" &
                        ", b.sTransNox" &
                        ", b.nUnitPrce" &
                    " FROM Order_Split a" &
                        ", Order_Split_Detail b" &
                    " WHERE a.sTransNox = b.sTransNox" &
                        " AND a.sReferNox = " & strParm(p_sSourceNo) &
                    " ORDER BY a.sTransNox"
        End If
        Debug.Print(lsSQL)
        loDt = p_oAppDrvr.ExecuteQuery(lsSQL)

        Return loDt
    End Function

    Private Function saveByMenu() As Boolean
        Dim lnRow As Integer
        Dim lsSQL As String
        Dim lsTransNox(p_nGroupNox - 1) As String
        Dim lsTranTemp As String
        Dim lnTranCtr As Integer = 0

        Dim lnDetRow As Integer
        Dim lnTotal As Decimal
        Dim lbDelMas As Boolean = True
        Dim lsTransTemp As String

        With p_oDTDetail
            For nCol As Integer = 1 To p_nGroupNox
                For nCtr As Integer = 0 To .Rows.Count - 1
                    .DefaultView.Sort = "sTrans" & Format(nCol, "000") & " ASC"
                    If p_oDTDetail.Rows(nCtr)("nGrpQt" & Format(nCol, "000")) > 0 Then
                        If p_oDTDetail.Rows(nCtr)("sTrans" & Format(nCol, "000")) <> "" Then
                            If p_oDTDetail.Rows(nCtr)("sTrans" & Format(nCol, "000")) = "x" Then
                                If lsTransNox(nCol - 1) = "" Then
                                    lsTranTemp = getNextTransNo()
                                    lsTransNox(nCol - 1) = Left(lsTranTemp, 8) & Format(CDbl(Right(lsTranTemp, 11)) + lnTranCtr, "000000000000")
                                End If
                            Else
                                If lsTransNox(nCol - 1) = "" Then
                                    lsTransNox(nCol - 1) = p_oDTDetail.Rows(nCtr)("sTrans" & Format(nCol, "000"))
                                End If
                            End If
                        Else
                            If lsTransNox(nCol - 1) = "" Then
                                lsTranTemp = getNextTransNo()
                                lsTransNox(nCol - 1) = Left(lsTranTemp, 8) & Format(CDbl(Right(lsTranTemp, 11)) + lnTranCtr, "000000000000")
                                lnTranCtr = lnTranCtr + 1
                            End If
                        End If

                        lsSQL = "INSERT INTO " & pxeDetailTble & " SET" & _
                                    "  sTransNox = " & strParm(lsTransNox(nCol - 1)) & _
                                    ", nEntryNox = " & CDbl(lnDetRow + 1) & _
                                    ", sStockIDx = " & strParm(.Rows(nCtr)("sStockIDx")) & _
                                    ", nUnitPrce = " & CDec(.Rows(nCtr)("nUnitPrce")) & _
                                    ", nQuantity = " & CDbl(.Rows(nCtr)("nGrpQt" & Format(nCol, "000"))) & _
                                    ", nSrcEntry = " & CDbl(nCol) & _
                                    ", cComboMlx = " & strParm(.Rows(nCtr)("cComboMlx")) & _
                                    ", cDetailxx = " & strParm(.Rows(nCtr)("cDetailxx")) & _
                                " ON DUPLICATE KEY UPDATE" & _
                                    "  sStockIDx = " & strParm(.Rows(nCtr)("sStockIDx")) & _
                                    ", nUnitPrce = " & CDec(.Rows(nCtr)("nUnitPrce")) & _
                                    ", nQuantity = " & CDbl(.Rows(nCtr)("nGrpQt" & Format(nCol, "000"))) & _
                                    ", nSrcEntry = " & CDbl(nCol)

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

                        If lsTransTemp <> lsTransNox(nCol - 1) Then lnTotal = 0.0
                        lnTotal += (.Rows(nCtr)("nGrpQt" & Format(nCol, "000")) * .Rows(nCtr)("nUnitPrce"))

                        lnDetRow += 1
                        lsTransTemp = lsTransNox(nCol - 1)
                        If lbDelMas Then lbDelMas = False
                    Else
                        If p_oDTDetail.Rows(nCtr)("sTrans" & Format(nCol, "000")) <> "x" Then
                            lsSQL = "DELETE FROM " & pxeDetailTble &
                                    " WHERE sTransNox = " & strParm(p_oDTDetail.Rows(nCtr)("sTrans" & Format(nCol, "000")))
                            '& " AND nEntryNox = " & CDbl(p_oDTDetail.Rows(nCtr)("nEntryNox") + 1) '"sGrpRw" & Format(nCol, "000")

                            Try
                                lnRow = p_oAppDrvr.Execute(lsSQL, pxeDetailTble)
                                If lnRow <= 0 Then
                                    MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                    Return False
                                End If
                            Catch ex As Exception
                                Throw ex
                            End Try


                            If lbDelMas Then
                                lsSQL = "DELETE FROM " & pxeMasterTble &
                                        " WHERE sTransNox = " & strParm(p_oDTDetail.Rows(nCtr)("sTrans" & Format(nCol, "000")))
                            End If

                            Try
                                lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                                If lnRow <= 0 Then
                                    MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                    Return False
                                End If
                            Catch ex As Exception
                                Throw ex
                            End Try
                        End If
                    End If
                Next nCtr

                If Not lbDelMas Then
                    Dim lnVatSales As Decimal
                    Dim lnServiceCharge As Decimal
                    Dim lnAmountDue As Decimal
                    lnVatSales = Math.Round(lnTotal / 1.12, 2)
                    lnServiceCharge = Math.Round(lnVatSales * 0.05, 2)
                    lnAmountDue = Math.Round(lnTotal + lnServiceCharge, 2)

                    lsSQL = "INSERT INTO " & pxeMasterTble & " SET" &
                                "  sTransNox = " & strParm(lsTransNox(nCol - 1)) &
                                ", sReferNox = " & strParm(p_sSourceNo) &
                                ", nAmountxx = " & CDec(lnAmountDue) &
                                ", cSplitTyp = " & strParm(p_cSplitTyp) &
                            " ON DUPLICATE KEY UPDATE" &
                                "  nAmountxx = " & CDec(lnAmountDue) &
                                ", cSplitTyp = " & strParm(p_cSplitTyp)
                Else
                    lsSQL = "DELETE FROM " & pxeMasterTble & _
                            " WHERE sTransNox = " & strParm(lsTransNox(nCol - 1))
                End If

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

                lnDetRow = 0
            Next nCol
        End With

        If Not cancelSODiscount() Then Return False

        Return True
    End Function

    Private Function saveByAmount() As Boolean
        Dim lnRow As Integer
        Dim lsSQL As String
        Dim lsTransNox(p_nGroupNox - 1) As String
        Dim lsTranTemp As String
        Dim lnTranCtr As Integer = 0

        Dim lnTotal As Decimal
        Dim lbDelMas As Boolean = True
        Dim lsTransTemp As String

        With p_oDTDetail
            For nCol As Integer = 1 To p_nGroupNox
                If p_oDTDetail.Rows(0)("nGrpAm" & Format(nCol, "000")) > 0 Then
                    If p_oDTDetail.Rows(0)("sTrans" & Format(nCol, "000")) <> "" Then
                        If p_oDTDetail.Rows(0)("sTrans" & Format(nCol, "000")) = "x" Then
                            If lsTransNox(nCol - 1) = "" Then

                                lsTranTemp = getNextTransNo()
                                lsTransNox(nCol - 1) = Left(lsTranTemp, 8) & Format(CDbl(Right(lsTranTemp, 11)) + lnTranCtr, "000000000000")
                            End If
                        Else
                            If lsTransNox(nCol - 1) = "" Then
                                lsTransNox(nCol - 1) = p_oDTDetail.Rows(0)("sTrans" & Format(nCol, "000"))
                            End If
                        End If
                    Else
                        If lsTransNox(nCol - 1) = "" Then
                            lsTranTemp = getNextTransNo()
                            lsTransNox(nCol - 1) = Left(lsTranTemp, 8) & Format(CDbl(Right(lsTranTemp, 11)) + lnTranCtr, "000000000000")
                            lnTranCtr = lnTranCtr + 1
                        End If
                    End If

                    lsSQL = "INSERT INTO " & pxeDetailTble & " SET" & _
                                "  sTransNox = " & strParm(lsTransNox(nCol - 1)) & _
                                ", nEntryNox = " & CDbl(1) & _
                                ", sStockIDx = " & strParm("") & _
                                ", nUnitPrce = " & CDec(.Rows(0)("nGrpAm" & Format(nCol, "000"))) & _
                                ", nQuantity = " & CDbl(0) & _
                            " ON DUPLICATE KEY UPDATE" & _
                                "  sStockIDx = " & strParm("") & _
                                ", nUnitPrce = " & CDec(.Rows(0)("nGrpAm" & Format(nCol, "000"))) & _
                                ", nQuantity = " & CDbl(0) & _
                                ", nSrcEntry = " & CDbl(.Rows(0)("nEntryNox"))

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

                    If lsTransTemp <> lsTransNox(nCol - 1) Then lnTotal = 0.0
                    lnTotal = lnTotal + .Rows(0)("nGrpAm" & Format(nCol, "000"))

                    lsTransTemp = lsTransNox(nCol - 1)
                    If lbDelMas Then lbDelMas = False
                Else
                    If p_oDTDetail.Rows(0)("sTrans" & Format(nCol, "000")) <> "x" Then
                        lsSQL = "DELETE FROM " & pxeDetailTble & _
                                " WHERE sTransNox = " & strParm(p_oDTDetail.Rows(0)("sTrans" & Format(nCol, "000"))) & _
                                    " AND nEntryNox = " & CDbl(p_oDTDetail.Rows(0)("nEntryNox")) '"sGrpRw" & Format(nCol, "000")

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
                    End If
                End If

                If Not lbDelMas Then
                    lsSQL = "INSERT INTO " & pxeMasterTble & " SET" & _
                                "  sTransNox = " & strParm(lsTransNox(nCol - 1)) & _
                                ", sReferNox = " & strParm(p_sSourceNo) & _
                                ", nAmountxx = " & CDec(lnTotal) & _
                                ", cSplitTyp = " & strParm(p_cSplitTyp) & _
                            " ON DUPLICATE KEY UPDATE" & _
                                "  nAmountxx = " & CDec(lnTotal) & _
                                ", cSplitTyp = " & strParm(p_cSplitTyp)
                Else
                    lsSQL = "DELETE FROM " & pxeMasterTble & _
                            " WHERE sTransNox = " & strParm(lsTransNox(nCol - 1))
                End If

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
            Next nCol
        End With

        If Not cancelSODiscount() Then Return False

        Return True
    End Function

    Private Function cancelSODiscount() As Boolean
        Dim lsSQL As String

        lsSQL = "UPDATE Discount SET" & _
                    " cTranStat = " & strParm(xeTranStat.TRANS_CANCELLED) & _
                " WHERE sSourceNo = " & strParm(p_sSourceNo) & _
                    " AND sSourceCd = 'SOSp'"

        Try
            p_oAppDrvr.Execute(lsSQL, "Discount")
        Catch ex As Exception
            Throw ex
        End Try

        Return True
    End Function

    Private Function cancelSOComplementary() As Boolean
        Dim lsSQL As String

        lsSQL = "UPDATE Complementary SET" & _
                    " cTranStat = " & strParm(xeTranStat.TRANS_CANCELLED) & _
                " WHERE sSourceNo = " & strParm(p_sSourceNo) & _
                    " AND sSourceCd = 'SOSp'"

        Try
            p_oAppDrvr.Execute(lsSQL, "Complementary")
        Catch ex As Exception
            Throw ex
        End Try

        Return True
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createMasterTable()
        p_oDTMaster = New DataTable
        With p_oDTMaster
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("cPaymForm", GetType(String)).MaxLength = 1
            .Columns.Add("sReferNox", GetType(String)).MaxLength = 20
            .Columns.Add("nAmountxx", GetType(Decimal))
            .Columns.Add("cSplitTyp", GetType(String)).MaxLength = 1
            .Columns.Add("cPaidxxxx", GetType(String)).MaxLength = 1
            .Columns.Add("nTranTotl", GetType(Decimal))
            .Columns.Add("nDiscntbl", GetType(Decimal))
            .Columns.Add("nZeroRatd", GetType(Decimal))
            .Columns.Add("nVATSales", GetType(Decimal))
            .Columns.Add("nVATAmtxx", GetType(Decimal))
            .Columns.Add("nNonVatxx", GetType(Decimal))
            .Columns.Add("nVATDiscx", GetType(Decimal))
            .Columns.Add("nPWDDiscx", GetType(Decimal))
            .Columns.Add("nDiscount", GetType(Decimal))
            .Columns.Add("nVoidTotl", GetType(Decimal))
            .Columns.Add("nPrntBill", GetType(Integer))
            .Columns.Add("dPrntBill", GetType(Date))
            .Columns.Add("sBillNmbr", GetType(String)).MaxLength = 20
        End With
    End Sub

    Private Sub createDetailTable()
        Dim lsColumnGrpQty As String
        Dim lsColumnGrpAmt As String
        Dim lsColumnTransc As String

        p_oDTDetail = p_oDTOrder.Clone
        With p_oDTDetail
            For nCtr As Integer = 1 To p_nGroupNox
                lsColumnGrpQty = "nGrpQt" & Format(nCtr, "000")
                lsColumnGrpAmt = "nGrpAm" & Format(nCtr, "000")
                lsColumnTransc = "sTrans" & Format(nCtr, "000")
                .Columns.Add(lsColumnTransc, GetType(String)).MaxLength = 20
                .Columns.Add(lsColumnGrpQty, GetType(Integer))
                .Columns.Add(lsColumnGrpAmt, GetType(Decimal))
            Next nCtr
            .Columns.Add("nSrcEntry", GetType(Integer))
        End With
    End Sub

    Private Sub procDataTable(ByVal oDT As DataTable)
        Dim lnCtr As Integer
        Dim lnRow As Integer
        Dim loDt As New DataTable
        Dim lsTransNox As String = ""
        Dim lnSetNo As Integer
        Dim lnItem As Integer

        createDetailTable()
        With p_oDTDetail
            lnSetNo = 0
            For lnCtr = 0 To oDT.Rows.Count - 1
                If oDT.Rows(lnCtr)("cReversex") = "+" And
                    oDT.Rows(lnCtr)("cDetailxx") = "0" Then
                    .Rows.Add()
                    lnRow = .Rows.Count - 1
                    .Rows(lnRow)("nEntryNox") = oDT.Rows(lnCtr)("nEntryNox")
                    .Rows(lnRow)("sStockIDx") = oDT.Rows(lnCtr)("sStockIDx")
                    .Rows(lnRow)("nUnitPrce") = (oDT(lnCtr).Item("nUnitPrce") *
                                                (100 - oDT(lnCtr).Item("nDiscount")) / 100 -
                                                oDT(lnCtr).Item("nAddDiscx"))
                    .Rows(lnRow)("nDiscount") = 0.0
                    .Rows(lnRow)("nAddDiscx") = 0.0
                    .Rows(lnRow)("cReversex") = "+"
                    .Rows(lnRow)("nComplmnt") = 0
                    .Rows(lnRow)("cComboMlx") = oDT.Rows(lnCtr)("cComboMlx")
                    .Rows(lnRow)("cDetailxx") = oDT.Rows(lnCtr)("cDetailxx")
                    .Rows(lnRow)("nQuantity") = getRemQty(p_sSourceNo _
                                                               , oDT.Rows(lnCtr)("sStockIDx") _
                                                               , oDT.Rows(lnCtr)("nQuantity") _
                                                               , oDT.Rows(lnCtr)("nComplmnt") _
                                                               , lnCtr)

                    loDt = validateDetail(IIf(p_cSplitTyp = xeSplitType.xeSplitByMenu, oDT.Rows(lnCtr)("sStockIDx"), "") _
                                          , .Rows(lnRow)("nEntryNox")) 'lnCtr + 1


                    For nCtr As Integer = 1 To p_nGroupNox
                        .Rows(lnRow)("nGrpQt" & Format(nCtr, "000")) = 0
                        .Rows(lnRow)("nGrpAm" & Format(nCtr, "000")) = 0.0#
                        .Rows(lnRow)("sTrans" & Format(nCtr, "000")) = "x"

                        If loDt.Rows.Count > 0 Then
                            If p_cSplitTyp = xeSplitType.xeSplitByMenu Then
                                For lnItem = 0 To loDt.Rows.Count - 1
                                    'If loDt.Rows.Count >= nCtr Then
                                    'If lsTransNox <> loDt.Rows(lnSetNo)("sTransNox") Then
                                    '    lsTransNox = loDt.Rows(lnSetNo)("sTransNox")
                                    '    lnSetNo += 1
                                    'End If

                                    'If lnSetNo = nCtr Then
                                    '    .Rows(lnRow)("nGrpQt" & Format(lnSetNo, "000")) = loDt.Rows(lnSetNo - 1)("nQuantity")
                                    '    .Rows(lnRow)("nGrpAm" & Format(lnSetNo, "000")) = loDt.Rows(lnSetNo - 1)("nUnitPrce")
                                    '    .Rows(lnRow)("sTrans" & Format(lnSetNo, "000")) = loDt.Rows(lnSetNo - 1)("sTransNox")
                                    'End If
                                    'End If

                                    If lsTransNox <> loDt.Rows(lnItem)("sTransNox") Then
                                        lsTransNox = loDt.Rows(lnItem)("sTransNox")
                                        lnSetNo += 1
                                    End If
                                Next lnItem
                            Else
                                .Rows(lnRow)("nGrpQt" & Format(nCtr, "000")) = loDt.Rows(nCtr - 1)("nQuantity")
                                .Rows(lnRow)("nGrpAm" & Format(nCtr, "000")) = loDt.Rows(nCtr - 1)("nUnitPrce")
                                .Rows(lnRow)("sTrans" & Format(nCtr, "000")) = loDt.Rows(nCtr - 1)("sTransNox")
                            End If
                        End If
                    Next nCtr
                End If
            Next lnCtr
        End With

        'createMasterTable()
        'addMaster()
    End Sub

    Private Sub LoadDiscount()
        p_oDiscount = New Discount(p_oAppDrvr)
        p_oDiscount.POSNumbr = p_sTermnl
        p_oDiscount.SourceNo = p_oDTMaster.Rows(p_nSetNumbr - 1)("sTransNox")
        p_oDiscount.SourceCd = pxeSourceCde
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

    Private Sub computeMaster()
        Dim loDetail As DataTable
        Dim loDetailTemp As DataTable
        Dim loMaster As DataTable
        Dim lnRow As Integer

        loDetail = p_oDTDetail.Clone
        For nCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
            If p_cSplitTyp = xeSplitType.xeSplitByMenu Then

                Debug.Print(p_oDTDetail.Rows(nCtr)("nGrpQt" & Format(p_nSetNumbr, "000")))

                If p_oDTDetail.Rows(nCtr)("nGrpQt" & Format(p_nSetNumbr, "000")) > 0 Then
                    loDetail.Rows.Add()
                    lnRow = loDetail.Rows.Count - 1
                    loDetail.Rows(lnRow)("nUnitPrce") = p_oDTDetail.Rows(nCtr)("nUnitPrce")
                    loDetail.Rows(lnRow)("nQuantity") = p_oDTDetail.Rows(nCtr)("nGrpQt" & Format(p_nSetNumbr, "000"))
                    loDetail.Rows(lnRow)("nAddDiscx") = p_oDTDetail.Rows(nCtr)("nAddDiscx")
                    loDetail.Rows(lnRow)("nDiscount") = p_oDTDetail.Rows(nCtr)("nDiscount")
                    loDetail.Rows(lnRow)("cReversex") = "+"
                    loDetail.Rows(lnRow)("nComplmnt") = 0
                End If
            Else
                loDetail.Rows.Add()
                loDetail.Rows(nCtr)("nUnitPrce") = p_oDTDetail.Rows(nCtr)("nUnitPrce")
                loDetail.Rows(nCtr)("nQuantity") = p_oDTDetail.Rows(nCtr)("nQuantity")
                loDetail.Rows(nCtr)("nAddDiscx") = p_oDTDetail.Rows(nCtr)("nAddDiscx")
                loDetail.Rows(nCtr)("nDiscount") = p_oDTDetail.Rows(nCtr)("nDiscount")
                loDetail.Rows(nCtr)("cReversex") = "+"
                loDetail.Rows(nCtr)("nComplmnt") = 0
            End If
        Next nCtr

        If p_nGroupNox > p_oDTMaster.Rows.Count Then
            For nCtr As Integer = 1 To p_nGroupNox - p_oDTMaster.Rows.Count
                p_oDTMaster.Rows.Add()
                p_oDTMaster.Rows(p_oDTMaster.Rows.Count - 1)("sTransNox") = ""
                p_oDTMaster.Rows(p_oDTMaster.Rows.Count - 1)("nNonVATxx") = 0.0
                p_oDTMaster.Rows(p_oDTMaster.Rows.Count - 1)("nDiscount") = 0.0
                p_oDTMaster.Rows(p_oDTMaster.Rows.Count - 1)("nPWDDiscx") = 0.0
                p_oDTMaster.Rows(p_oDTMaster.Rows.Count - 1)("nVatDiscx") = 0.0
                p_oDTMaster.Rows(p_oDTMaster.Rows.Count - 1)("nTranTotl") = 0.0
                p_oDTMaster.Rows(p_oDTMaster.Rows.Count - 1)("nVATSales") = 0.0
                p_oDTMaster.Rows(p_oDTMaster.Rows.Count - 1)("nVATAmtxx") = 0.0
            Next nCtr
        Else
            'MsgBox(p_oDTMaster.Rows.Count)
        End If

        loMaster = p_oDTMaster.Clone
        loMaster.Rows.Add()
        loMaster.Rows(0)("nNonVATxx") = 0.0
        loMaster.Rows(0)("nDiscount") = 0.0
        loMaster.Rows(0)("nPWDDiscx") = 0.0
        loMaster.Rows(0)("nVatDiscx") = 0.0
        loMaster.Rows(0)("nTranTotl") = 0.0
        loMaster.Rows(0)("nVATSales") = 0.0
        loMaster.Rows(0)("nVATAmtxx") = 0.0
        Call LoadDiscount()

        'Recompute total
        If p_cSplitTyp <> xeSplitType.xeSplitByMenu Then
            loDetailTemp = loDetail.Clone
            With loDetailTemp
                .Rows.Add()
                .Rows(0)("nUnitPrce") = 0.0
                .Rows(0)("nQuantity") = 1
                .Rows(0)("nAddDiscx") = 0.0
                .Rows(0)("nDiscount") = 0.0
                .Rows(0)("cReversex") = "+"
                .Rows(0)("nComplmnt") = 0
            End With
            computeTotal(loMaster, loDetailTemp, p_oDtaDiscx, p_oDiscount)
        Else
            computeTotal(loMaster, loDetail, p_oDtaDiscx, p_oDiscount)
        End If

        p_oDTMaster.Rows(p_nSetNumbr - 1)("nNonVATxx") = loMaster.Rows(0)("nNonVATxx")
        p_oDTMaster.Rows(p_nSetNumbr - 1)("nDiscount") = loMaster.Rows(0)("nDiscount")
        p_oDTMaster.Rows(p_nSetNumbr - 1)("nPWDDiscx") = loMaster.Rows(0)("nPWDDiscx")
        p_oDTMaster.Rows(p_nSetNumbr - 1)("nVatDiscx") = loMaster.Rows(0)("nVatDiscx")
        p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") = loMaster.Rows(0)("nTranTotl")
        p_oDTMaster.Rows(p_nSetNumbr - 1)("nVATSales") = loMaster.Rows(0)("nVATSales")
        p_oDTMaster.Rows(p_nSetNumbr - 1)("nVATAmtxx") = loMaster.Rows(0)("nVATAmtxx")
        Dim lnVatSale As Decimal
        Dim lnServiceCharge As Decimal = 0
        If p_cSplitTyp <> xeSplitType.xeSplitByMenu Then
            p_oDTMaster.Rows(p_nSetNumbr - 1)("nDiscount") = Math.Round((((p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") / p_nSalesTotl) * 100) / 100) * IFNull(loMaster.Rows(0)("nDiscount"), 0), 2)

            p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") = Math.Round(p_oDTDetail.Rows(0)("nGrpAm" & Format(p_nSetNumbr, "000")), 2)
            Debug.Print("total = " & p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") & " group = " & p_oDTDetail.Rows(0)("nGrpAm" & Format(p_nSetNumbr, "000")), 2)

            If (p_bSChargexx) Then
                lnVatSale = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") / 1.17, 2)
                lnServiceCharge = Math.Round(lnVatSale * 0.05, 2)
                p_nSChargexx = lnServiceCharge
            Else
                lnVatSale = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") / 1.12, 2)
            End If

            Dim lnVatAmt As Decimal = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") - lnVatSale - lnServiceCharge, 2)
            'p_oDTMaster.Rows(p_nSetNumbr - 1)("nAmountxx") = Math.Round(lnVatSale + lnVatAmt)
            Dim lnSalesAmount As Decimal
            'Dim lnVatSale As Decimal = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") - p_oDTMaster.Rows(p_nSetNumbr - 1)("nNonVATxx"), 2)
            Debug.Print("vatofsale = " & lnVatSale)
            p_oDTMaster.Rows(p_nSetNumbr - 1)("nNonVATxx") = Math.Round(loMaster.Rows(0)("nNonVATxx"), 2)
            p_oDTMaster.Rows(p_nSetNumbr - 1)("nVATSales") = Math.Round(lnVatSale, 2)
            p_oDTMaster.Rows(p_nSetNumbr - 1)("nVATAmtxx") = Math.Round(lnVatAmt, 2)

            'Else
            '    lnVatSale = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") / 1.12, 2)
            '    lnServiceCharge = Math.Round(lnVatSale * 0.05, 2)
            '    p_nSChargexx = lnServiceCharge
            '    p_oDTMaster.Rows(p_nSetNumbr - 1)("nDiscount") = Math.Round((((p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") / p_nSalesTotl) * 100) / 100) * IFNull(loMaster.Rows(0)("nDiscount"), 0), 2)

            '    Debug.Print("total = " & p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") & " group = " & p_oDTDetail.Rows(0)("nGrpAm" & Format(p_nSetNumbr, "000")), 2)
            '    Dim lnVatSale As Decimal
            '    Dim lnServiceCharge As Decimal = 0
            '    If (p_bSChargexx) Then
            '        lnVatSale = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") / 1.12, 2)
            '        lnServiceCharge = Math.Round(lnVatSale * 0.05, 2)
            '        p_nSChargexx = lnServiceCharge
            '    Else
            '        lnVatSale = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") / 1.12, 2)
            '    End If

            '    Dim lnVatAmt As Decimal = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") - lnVatSale - lnServiceCharge, 2)
            '    'p_oDTMaster.Rows(p_nSetNumbr - 1)("nAmountxx") = Math.Round(lnVatSale + lnVatAmt)
            '    Dim lnSalesAmount As Decimal
            '    'Dim lnVatSale As Decimal = Math.Round(p_oDTMaster.Rows(p_nSetNumbr - 1)("nTranTotl") - p_oDTMaster.Rows(p_nSetNumbr - 1)("nNonVATxx"), 2)
            '    Debug.Print("vatofsale = " & lnVatSale)
            '    p_oDTMaster.Rows(p_nSetNumbr - 1)("nNonVATxx") = Math.Round(loMaster.Rows(0)("nNonVATxx"), 2)
            '    p_oDTMaster.Rows(p_nSetNumbr - 1)("nVATSales") = Math.Round(lnVatSale, 2)
            '    p_oDTMaster.Rows(p_nSetNumbr - 1)("nVATAmtxx") = Math.Round(lnVatAmt, 2)

        End If


    End Sub
#End Region

#Region "Public Procedures"
    Sub ShowSplitForm()
        p_oFormSplit = New frmOrderSplit
        With p_oFormSplit
            .SplitOrder = Me
            '.TopMost = True
            .ShowDialog()

            p_bCancelled = .Cancelled
        End With
    End Sub

    Sub ShowSplitPaymentForm()
        Dim loFormPaySplit As frmPaySplitted

        loFormPaySplit = New frmPaySplitted

        With loFormPaySplit
            .SplitOrder = Me

            .TopMost = True
            .ShowDialog()

            p_sSourceNo = .TransNo

            p_bCancelled = .Cancelled
        End With
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider

        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode

        p_oInventory = New clsInventory(p_oAppDrvr)

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub
End Class