'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Complementary
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


Public Class Complementary

#Region "Constant"
    Private Const xsSignature As String = "08220326"
    Private Const pxeMODULENAME As String = "Complementary"
    Private Const pxeMasterTble As String = "Complementary"
    Private Const pxeDetailTble As String = "SO_Detail"
#End Region

#Region "xeCompType"
    Enum xeCompType
        xeCompAmount = 0
        xecompItem = 1
    End Enum
#End Region

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oDTMaster As DataTable
    Protected p_oDTDetail As DataTable
    Protected p_oDTOrder As DataTable
    Protected p_oSpit As SplitOrder
    Protected p_nEditMode As xeEditMode
    Protected p_oFormComplementary As frmComplementary
    Protected p_oSC As New MySqlCommand
    Protected p_oInventory As clsInventory
    Protected p_oDT As DataTable

    Protected p_sBranchCd As String
    Protected p_sSourceNo As String
    Protected p_sSourceCd As String
    Protected p_nSalesTot As Decimal
    Protected p_nCompType As xeCompType
    Protected p_bCancelled As Boolean
    Protected p_nGroupNox As Integer = 1
    Protected p_sPOSNo As String
    Protected p_bHasComplementary As Boolean
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

    ReadOnly Property HasComplementary
        Get
            Return p_bHasComplementary
        End Get
    End Property

    WriteOnly Property OrderDetail As DataTable
        Set(ByVal Value As DataTable)
            p_oDTOrder = Value
        End Set
    End Property

    WriteOnly Property ComplemtaryType As xeCompType
        Set(ByVal Value As xeCompType)
            p_nCompType = Value
        End Set
    End Property

    Property SalesTotal As Decimal
        Get
            Return p_nSalesTot
        End Get
        Set(value As Decimal)
            p_nSalesTot = value
        End Set
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

    Property Detail(ByVal Row As Integer, _
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "nentrynox"
                    Case "sbarcodex"
                    Case "sdescript"
                    Case "nunitprce"
                    Case "creversex"
                    Case "nquantity"
                    Case "ndiscount"
                    Case "nadddiscx"
                    Case "cprintedx"
                    Case "ncomplmnt"
                    Case "cdetailxx"
                    Case "creversed"
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
                    Case "sbarcodex"
                    Case "sdescript"
                    Case "nunitprce"
                    Case "creversex"
                    Case "nquantity"
                    Case "ndiscount"
                    Case "nadddiscx"
                    Case "cprintedx"
                    Case "ncomplmnt"
                        If Value > validateDetail(p_oDTMaster.Rows(0)("sTransNox") _
                                            , p_oDTDetail.Rows(Row)("sStockIDx") _
                                            , p_oDTDetail.Rows(Row)("nQuantity")) Then Value = 0
                    Case "cdetailxx"
                    Case "creversed"
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

    Private Function loadOrder() As Boolean
        Dim lnCtr As Integer

        Dim lsSourceNo As String = ""
        Dim lsSourceCd As String = ""
        Dim lsSplitType As String = ""
        Dim lbCancelled As Boolean = False
        Dim lbSplitted As Boolean = False

        p_oSpit = New SplitOrder(p_oAppDrvr)
        p_oSpit.SourceNo = p_sSourceNo
        p_oSpit.OrderDetail = p_oDTOrder

        p_oDTDetail = p_oDTOrder.Clone
        p_oDTDetail.Columns.Add("nOrigValx", Type.GetType("System.Int32"))

        'If Not p_oSpit.OpenBySource Then
        '    p_oDTDetail = p_oDTOrder.Clone
        'Else
        '    p_oSpit.GroupNo = p_nGroupNox
        'End If

        p_oSpit.OpenBySource()
        If p_oSpit.WasSplitted Then
            MsgBox("Unable to complement splitted order.", MsgBoxStyle.Information, "Notice")
            Return False
            p_oSpit.GroupNo = p_nGroupNox
        End If

        With p_oDTDetail
            For lnCtr = 0 To p_oDTOrder.Rows.Count - 1
                If p_oDTOrder.Rows(lnCtr)("cReversex") = "+" And
                    p_oDTOrder.Rows(lnCtr)("cDetailxx") = "0" Then
                    .Rows.Add()
                    .Rows(lnCtr)("sTransNox") = IFNull(p_oDTOrder.Rows(lnCtr)("sTransNox"), p_sSourceNo)
                    .Rows(lnCtr)("nEntryNox") = p_oDTOrder.Rows(lnCtr)("nEntryNox")
                    .Rows(lnCtr)("sStockIDx") = p_oDTOrder.Rows(lnCtr)("sStockIDx")
                    .Rows(lnCtr)("nQuantity") = p_oDTOrder.Rows(lnCtr)("nQuantity")
                    .Rows(lnCtr)("nComplmnt") = p_oDTOrder.Rows(lnCtr)("nComplmnt")
                    .Rows(lnCtr)("nOrigValx") = p_oDTOrder.Rows(lnCtr)("nComplmnt")
                    .Rows(lnCtr)("nUnitPrce") = p_oDTOrder.Rows(lnCtr)("nUnitPrce")
                    .Rows(lnCtr)("sBarcodex") = p_oDTOrder.Rows(lnCtr)("sBarcodex")
                    .Rows(lnCtr)("sDescript") = p_oDTOrder.Rows(lnCtr)("sDescript")
                End If
            Next lnCtr
        End With

        Return True
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

#Region "Public function"
    Function SaveTransaction() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim lnCtr As Integer
        Dim lnTotal As Integer

        With p_oDTMaster
            If p_bCancelled Then Return False

            For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
                If p_nCompType = xeCompType.xeCompAmount Then
                    p_oDTDetail.Rows(lnCtr)("nComplmnt") = p_oDTDetail.Rows(lnCtr)("nQuantity")
                End If

                If p_oDTDetail.Rows(lnCtr)("nOrigValx") <> p_oDTDetail.Rows(lnCtr)("nComplmnt") Then
                    lnTotal += p_oDTDetail.Rows(lnCtr)("nComplmnt") * p_oDTDetail.Rows(lnCtr)("nUnitPrce")
                    lsSQL = "UPDATE " & pxeDetailTble & " SET" & _
                                " nComplmnt = " & CDbl(p_oDTDetail.Rows(lnCtr)("nComplmnt")) & _
                            " WHERE sTransNox = " & strParm(p_oDTDetail.Rows(lnCtr)("sTransNox")) & _
                                " AND sStockIDx = " & strParm(p_oDTDetail.Rows(lnCtr)("sStockIDx")) & _
                                " AND nEntryNox = " & CDbl(p_oDTDetail.Rows(lnCtr)("nEntryNox"))

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
            Next

            lsSQL = "INSERT INTO " & pxeMasterTble & " SET" & _
                        "  sTransNox = " & strParm(.Rows(0)("sTransNox")) & _
                        ", dTransact = " & dateParm(.Rows(0)("dTransact")) & _
                        ", sRemarksx = " & strParm(.Rows(0)("sRemarksx")) & _
                        ", sRequestd = " & strParm(.Rows(0)("sRequestd")) & _
                        ", sApproved = " & strParm(.Rows(0)("sApproved")) & _
                        ", nAmountxx = " & CDbl(lnTotal) & _
                        ", sSourceNo = " & strParm(p_sSourceNo) & _
                        ", sSourceCd = " & strParm(p_sSourceCd) & _
                    " ON DUPLICATE KEY UPDATE" & _
                        "  sRemarksx = " & strParm(.Rows(0)("sRemarksx")) & _
                        ", sRequestd = " & strParm(.Rows(0)("sRequestd")) & _
                        ", sApproved = " & strParm(.Rows(0)("sApproved")) & _
                        ", nAmountxx = " & CDbl(lnTotal)

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

        p_oDTMaster(0)("nAmountxx") = lnTotal

        Return True
    End Function

    Function OpenTransaction(ByVal fsTransNox As String) As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String

        lsSQL = AddCondition("SELECT * FROM " & pxeMasterTble, "sTransNox = " & strParm(fsTransNox))
        
        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If Not loadOrder() Then Return False

        Call createMasterTable()

        If loDT.Rows.Count > 0 Then
            With p_oDTMaster
                .Rows.Add()
                .Rows(0)("sTransNox") = loDT.Rows(0)("sTransNox")
                .Rows(0)("dTransact") = loDT.Rows(0)("dTransact")
                .Rows(0)("sRemarksx") = loDT.Rows(0)("sRemarksx")
                .Rows(0)("sRequestd") = loDT.Rows(0)("sRequestd")
                .Rows(0)("sApproved") = loDT.Rows(0)("sApproved")
                .Rows(0)("nAmountxx") = loDT.Rows(0)("nAmountxx")
            End With

            p_bHasComplementary = True
        Else
            Call initMaster()
            p_bHasComplementary = False
        End If

        Return True
    End Function

    Function OpenBySource() As Boolean
        Dim loDT As New DataTable
        Dim lsSQL As String

        lsSQL = AddCondition("SELECT * FROM SO_Master", "sTransNox = " & strParm(p_sSourceNo))
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)
        If IFNull(loDT.Rows(0)("sMergeIDx"), "") = "" Then
            lsSQL = AddCondition("SELECT * FROM " & pxeMasterTble, "sSourceNo = " & strParm(p_sSourceNo) & _
                                                                    " AND sSourceCd = " & strParm(p_sSourceCd))
        Else
            lsSQL = "SELECT a.* FROM " & pxeMasterTble & " a" & _
                        ", SO_Master b" & _
                    " WHERE a.sTransNox = b.sTransNox" & _
                        " AND b.sMergeIDx = " & strParm(loDT.Rows(0)("sMergeIDx")) & _
                        " AND a.sSourceCd = " & strParm(p_sSourceCd)
        End If

        loDT = New DataTable
        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If Not loadOrder() Then Return False

        Call createMasterTable()

        If loDT.Rows.Count > 0 Then
            With p_oDTMaster
                .Rows.Add()
                .Rows(0)("sTransNox") = loDT.Rows(0)("sTransNox")
                .Rows(0)("dTransact") = loDT.Rows(0)("dTransact")
                .Rows(0)("sRemarksx") = loDT.Rows(0)("sRemarksx")
                .Rows(0)("sRequestd") = loDT.Rows(0)("sRequestd")
                .Rows(0)("sApproved") = loDT.Rows(0)("sApproved")
                .Rows(0)("nAmountxx") = loDT.Rows(0)("nAmountxx")
            End With

            p_bHasComplementary = True
        Else
            Call initMaster()
            p_bHasComplementary = False
        End If

        Return True
    End Function
#End Region

#Region "Private Procedures"
    Private Sub createMasterTable()
        p_oDTMaster = New DataTable
        With p_oDTMaster
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
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

    Sub ShowComplementary()
        p_oFormComplementary = New frmComplementary
        With p_oFormComplementary
            .Complementary = Me
            .TopMost = True
            .ShowDialog()

            p_bCancelled = .CloseForm
        End With
    End Sub

    Private Sub createTempTable()
        p_oDT = New DataTable

        With p_oDT
            .Columns.Add("sTransNox", GetType(String)).MaxLength = 20
            .Columns.Add("sStockIDx", GetType(String)).MaxLength = 12
            .Columns.Add("nQuantity", GetType(Integer))
            .Columns.Add("nPaidQtyx", GetType(Integer))
            .Columns.Add("nRemQtyxx", GetType(Integer))
            .Columns.Add("nEtrnyNox", GetType(Integer))
        End With
    End Sub

    Private Function validateDetail(ByVal fsReferNox As String, _
                                ByVal fsStockIDx As String, _
                                ByVal fnQuantity As Integer) As Integer
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
                " HAVING NOT ISNULL(sTransNox)"

        loDt = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDt.Rows.Count = 0 Then
            Return fnQuantity
        Else
            If Not IsNothing(p_oDT) Then createTempTable()
            With p_oDT
                If .Rows.Count = 0 Then
                    .Rows.Add()
                    .Rows(0)("sTransNox") = fsReferNox
                    .Rows(0)("nEntryNox") = .Rows.Count
                    .Rows(0)("sStockIDx") = fsStockIDx
                    .Rows(0)("nQuantity") = fnQuantity
                    .Rows(0)("nPaidQtyx") = loDt.Rows(0)("nQuantity")
                    .Rows(0)("nRemQtyxx") = fnQuantity - .Rows(0)("nPaidQtyx")
                    Return .Rows(0)("nRemQtyxx")
                Else
                    Dim foundRows() As DataRow

                    ' Use the Select method to find all rows matching the filter.
                    foundRows = .Select("sTransNox = " & strParm(fsReferNox) & _
                                            " AND sStockIDx = " & strParm(fsStockIDx), "nEntryNox ASC")

                    .Rows.Add()
                    If foundRows.GetUpperBound(0) < 0 Then
                        .Rows(0)("sTransNox") = fsReferNox
                        .Rows(0)("nEntryNox") = .Rows.Count
                        .Rows(0)("sStockIDx") = fsStockIDx
                        .Rows(0)("nQuantity") = fnQuantity
                        .Rows(0)("nPaidQtyx") = loDt.Rows(0)("nQuantity")
                        .Rows(0)("nRemQtyxx") = fnQuantity - .Rows(0)("nPaidQtyx")
                        Return .Rows(0)("nRemQtyxx")
                    Else
                        Dim lnQuantity As Integer
                        lnQuantity = ((.Rows(foundRows.GetUpperBound(0))("nRemQtyxx") - .Rows(foundRows.GetUpperBound(0))("nPaidQtyx")) + fnQuantity) _
                                     - loDt.Rows(0)("nQuantity")
                        .Rows(0)("sTransNox") = fsReferNox
                        .Rows(0)("nEntryNox") = .Rows.Count
                        .Rows(0)("sStockIDx") = fsStockIDx
                        .Rows(0)("nQuantity") = fnQuantity
                        .Rows(0)("nPaidQtyx") = loDt.Rows(0)("nQuantity")
                        .Rows(0)("nRemQtyxx") = lnQuantity - .Rows(0)("nPaidQtyx")
                        Return .Rows(0)("nRemQtyxx")
                    End If
                End If
            End With
        End If
    End Function
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