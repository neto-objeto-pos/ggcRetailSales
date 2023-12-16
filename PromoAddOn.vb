'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Add-On Promo Transaction/Maintenance Object
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
'  Kalyptus [ 12/20/2016 02:15 pm ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports MySql.Data.MySqlClient
Imports ggcAppDriver

Public Class PromoAddOn
    Private p_oApp As GRider
    Private p_oDTDetail As DataTable
    Private p_oDTMaster As DataTable
    Private p_nRow As Integer

    Private p_nEditMode As xeEditMode
    Private p_sParent As String

    Private Const p_sMasTable As String = "Promo_Add_On"
    Private Const p_sDetTable As String = "Promo_Add_On_Detail"

    Private Const p_sMsgHeadr As String = "Add-On Promo Maintenance/Transaction"

    Public Event MasterRetrieved(ByVal Index As Integer, _
                                 ByVal Value As Object)

    Public Event DetailRetrieved(ByVal Row As Integer, _
                                 ByVal Index As Integer, _
                                 ByVal Value As Object)

    Public Property Master(ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Return Master(p_oDTMaster.Columns(Index).Ordinal)
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Master(p_oDTMaster.Columns(Index).Ordinal) = value
            End If
        End Set
    End Property

    Public Property Master(ByVal Index As Integer) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                If Index = 1 Or Index = 2 Then
                    If p_oDTMaster(0).Item(Index) = "" And p_oDTMaster(0).Item("sStockIDx") <> "" Then
                        getItem(p_oDTMaster, 0, 5, 1, 2, p_oDTMaster(0).Item("sStockIDx"), 5, False)
                    End If
                End If
                Return p_oDTMaster(0).Item(Index)
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 1, 2
                        Call getItem(p_oDTMaster, 0, 5, 1, 2, value, Index, False)
                    Case 3, 4
                        If IsDate(value) Then
                            p_oDTMaster(0).Item(Index) = value
                        End If
                        RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
                    Case 5
                        p_oDTMaster(0).Item(Index) = value
                        p_oDTMaster(0).Item("sBriefDsc") = ""
                        p_oDTMaster(0).Item("sBarrCode") = ""
                End Select
            End If
        End Set
    End Property

    'Property Detail(Integer, String)
    Public Property Detail(ByVal Row As Integer, ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Return Detail(Row, p_oDTDetail(Row).Item(Index))
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Detail(Row, p_oDTDetail.Columns(Index).Ordinal) = value
            End If
        End Set
    End Property

    'Property Detail(Integer, Integer)
    Public Property Detail(ByVal Row As Integer, ByVal Index As Integer) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 1, 2
                        getItem(p_oDTDetail, Row, 7, 1, 2, p_oDTDetail(Row).Item(7), 7, False)
                    Case 5, 6
                        getItem(p_oDTDetail, Row, 8, 5, 6, p_oDTDetail(Row).Item(8), 8, False)
                End Select
                Return p_oDTDetail(Row).Item(Index)
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 1, 2
                        getItem(p_oDTDetail, Row, 7, 1, 2, value, Index, False)
                    Case 5, 6
                        getItem(p_oDTDetail, Row, 8, 5, 6, value, Index, False)
                    Case 7
                        p_oDTDetail(Row).Item(Index) = value
                        p_oDTDetail(Row).Item(1) = ""
                        p_oDTDetail(Row).Item(2) = ""
                    Case 8
                        p_oDTDetail(Row).Item(Index) = value
                        p_oDTDetail(Row).Item(5) = ""
                        p_oDTDetail(Row).Item(6) = ""
                End Select
            End If
        End Set
    End Property

    Public Property Parent() As String
        Get
            Return p_sParent
        End Get
        Set(ByVal value As String)
            p_sParent = value
        End Set
    End Property

    'Property RecordCount()
    Public ReadOnly Property ItemCount() As Integer
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Return p_oDTDetail.Rows.Count
            Else
                Return 0
            End If
        End Get
    End Property

    'Property EditMode()
    Public ReadOnly Property EditMode() As xeEditMode
        Get
            Return p_nEditMode
        End Get
    End Property

    'Public Function NewRecord()
    Public Function NewRecord() As Boolean
        Call createMaster()
        Call newMaster()

        Call createDetail()
        Call newDetail()

        p_nEditMode = xeEditMode.MODE_ADDNEW

        Return True
    End Function

    'Public Function OpenRecord(String)
    Public Function OpenRecord(ByVal fsRecdIDxx As String) As Boolean
        Dim lsSQL As String
        Dim loDta As DataTable
        lsSQL = AddCondition(getSQ_Master, "a.sTransNox = " & strParm(fsRecdIDxx))
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count <= 0 Then
            p_nEditMode = xeEditMode.MODE_UNKNOWN
            Return False
        End If

        'Load Master Record
        Call createMaster()
        Call newMaster()
        p_oDTMaster(0).Item("sTransNox") = loDta(0).Item("sTransNox")
        p_oDTMaster(0).Item("dPromoFrm") = loDta(0).Item("dPromoFrm")
        p_oDTMaster(0).Item("dPromoTru") = loDta(0).Item("dPromoTru")
        p_oDTMaster(0).Item("sStockIDx") = loDta(0).Item("sStockIDx")
        p_oDTMaster(0).Item("cRecdStat") = loDta(0).Item("cRecdStat")
        p_oDTMaster(0).Item("sModified") = loDta(0).Item("sModified")
        p_oDTMaster(0).Item("dModified") = loDta(0).Item("dModified")

        'Load Detail Table
        lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(fsRecdIDxx))
        loDta = p_oApp.ExecuteQuery(lsSQL)
        Dim lnRow As Integer
        Call createDetail()
        For lnRow = 0 To loDta.Rows.Count - 1
            Call newDetail()
            p_oDTDetail(lnRow).Item("sTransNox") = loDta(lnRow).Item("sTransNox")
            p_oDTDetail(lnRow).Item("nEntryNox") = loDta(lnRow).Item("nEntryNox")
            p_oDTDetail(lnRow).Item("sStockIDx") = loDta(lnRow).Item("sStockIDx")
            p_oDTDetail(lnRow).Item("nQuantity") = loDta(lnRow).Item("nQuantity")
            p_oDTDetail(lnRow).Item("nUnitPrce") = loDta(lnRow).Item("nUnitPrce")
            p_oDTDetail(lnRow).Item("sReplItem") = loDta(lnRow).Item("sReplItem")
            p_oDTDetail(lnRow).Item("dModified") = loDta(lnRow).Item("dModified")
        Next

        p_nEditMode = xeEditMode.MODE_READY
        Return True
    End Function

    'Public Function SearchMaster
    Public Sub SearchMaster(ByVal fnIndex As Integer, _
                            ByVal fsValue As String)
        Select Case fnIndex
            Case 1, 2  'sBarrCode
                If fsValue <> "" Then
                    getItem(p_oDTMaster, 0, 5, 1, 2, fsValue, fnIndex, True)
                End If
        End Select
    End Sub

    Public Sub searchDetail(ByVal fnRow As Integer, _
                            ByVal fnIndex As Integer, _
                            ByVal fsValue As String)
        Select Case fnIndex
            Case 1, 2  'sBarrCode, BriefDsc
                If fsValue <> "" Then
                    getItem(p_oDTDetail, fnRow, 7, 1, 2, fsValue, fnIndex, True)
                End If
            Case 5, 6 'xBarrCode, xBriefDsc
                If fsValue <> "" Then
                    getItem(p_oDTDetail, fnRow, 8, 5, 6, fsValue, fnIndex, True)
                End If
        End Select
    End Sub

    'Public Function SearchTransaction(String, Boolean, Boolean=False)
    Public Function SearchRecord( _
                        ByVal fsValue As String _
                      , Optional ByVal fbByCode As Boolean = False) As Boolean

        Dim lsSQL As String

        'Check if already loaded base on edit mode
        If p_nEditMode = xeEditMode.MODE_READY Or p_nEditMode = xeEditMode.MODE_UPDATE Then
            If fbByCode Then
                If fsValue = p_oDTMaster(0).Item("sTransNox") Then Return True
            Else
                If fsValue = p_oDTMaster(0).Item("sBriefDsc") Then Return True
            End If
        End If

        'Make sure that the parameter for search has value
        fsValue = Trim(fsValue)
        If fsValue = "" Or fsValue = "%" Then
            MsgBox("The search needs a value!", vbOKOnly + vbInformation, p_sMsgHeadr)
            Return False
        End If

        'Initialize SQL filter
        lsSQL = getSQ_Browse()

        'create Kwiksearch filter
        Dim lsFilter As String
        If fbByCode Then
            lsFilter = "a.sTransNox = " & strParm(fsValue)
        Else
            lsFilter = "b.sBriefDsc LIKE " & strParm(fsValue & "%")
        End If

        Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , lsSQL _
                                        , False _
                                        , lsFilter _
                                        , "sTransNox»sBriefDsc»dPromoFrm" _
                                        , "Trans No»Item»Date From", _
                                        , "sTransNox»sBriefDsc»dPromoFrm" _
                                        , IIf(fbByCode, 0, 1))
        If IsNothing(loDta) Then
            p_nEditMode = xeEditMode.MODE_UNKNOWN
            Return False
        Else
            Return OpenRecord(loDta.Item("sTransNox"))
        End If
    End Function

    'Public Function SaveTransaction
    Public Function SaveRecord() As Boolean
        If Not (p_nEditMode = xeEditMode.MODE_ADDNEW Or _
                p_nEditMode = xeEditMode.MODE_READY Or _
                p_nEditMode = xeEditMode.MODE_UPDATE) Then

            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If

        If Not isEntryOk() Then
            Return False
        End If

        Dim lsSQL As String = ""

        If p_sParent = "" Then p_oApp.BeginTransaction()

        'Save master table 
        lsSQL = "INSERT INTO " & p_sMasTable & _
               " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                  ", dPromoFrm = " & IIf(IsDate(p_oDTMaster(0).Item("dPromoFrm")), dateParm(p_oDTMaster(0).Item("dPromoFrm")), "NULL") & _
                  ", dPromoTru = " & IIf(IsDate(p_oDTMaster(0).Item("dPromoTru")), dateParm(p_oDTMaster(0).Item("dPromoTru")), "NULL") & _
                  ", sStockIDx = " & strParm(p_oDTMaster(0).Item("sStockIDx")) & _
                  ", cRecdStat = " & strParm(p_oDTMaster(0).Item("cRecdStat")) & _
                  ", sModified = " & strParm(p_oApp.UserID) & _
                  ", dModified = " & dateParm(p_oApp.getSysDate)
        p_oApp.Execute(lsSQL, p_sMasTable)

        'Save detail table
        Dim lnRow As Integer
        p_nRow = 0
        For lnRow = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(p_nRow).Item("sStockIDx") <> "" Then
                p_nRow = p_nRow + 1
                lsSQL = "INSERT INTO " & p_sDetTable & _
                       " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                          ", nEntryNox = " & p_nRow & _
                          ", sStockIDx = " & strParm(p_oDTDetail(lnRow).Item("sStockIDx")) & _
                          ", nQuantity = " & p_oDTDetail(lnRow).Item("nQuantity") & _
                          ", nUnitPrce = " & p_oDTDetail(lnRow).Item("nUnitPrce") & _
                          ", sReplItem = " & strParm(p_oDTDetail(lnRow).Item("sReplItem")) & _
                          ", dModified = " & dateParm(p_oApp.getSysDate)
                p_oApp.Execute(lsSQL, p_sDetTable)
            End If
        Next

        If p_sParent = "" Then p_oApp.CommitTransaction()

        p_nEditMode = xeEditMode.MODE_READY

        Return True
    End Function

    'Public Function CancelTransaction
    Public Function CancelRecord() As Boolean
        If Not (p_nEditMode = xeEditMode.MODE_READY Or _
                p_nEditMode = xeEditMode.MODE_UPDATE) Then

            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If

        Dim lsSQL As String

        If p_sParent = "" Then p_oApp.BeginTransaction()

        p_oDTMaster(0).Item("cRecdStat") = "3"
        lsSQL = ADO2SQL(p_oDTMaster, p_sMasTable, "sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        p_oApp.Execute(lsSQL, p_sMasTable, p_oApp.BranchCode)

        If p_sParent = "" Then p_oApp.CommitTransaction()

        Return True
    End Function

    Public Function searchAddOn(ByVal fsStockIDx As String, ByVal fdTransact As Date) As Boolean
        Dim lsSQL As String
        Dim loDta As DataTable

        lsSQL = "SELECT sTransNox" & _
               " FROM " & p_sMasTable & _
               " WHERE sStockIDx = " & strParm(fsStockIDx) & _
                 " AND " & dateParm(fdTransact) & " BETWEEN IFNULL(dPromoFrm, " & dateParm(fdTransact) & ") AND IFNULL(dPromoTru, NOW())" & _
                 " AND cRecdStat = '1'"
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then Return False

        Return OpenRecord(loDta(0).Item("sTransNox"))

        loDta = Nothing
    End Function

    Private Function isEntryOk() As Boolean
        'Check for the information about the card
        If Trim(p_oDTMaster(0).Item("sTransNox")) = "" Then
            MsgBox("Sales Promo No seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        'Check how much does he intends to borrow
        If Trim(p_oDTMaster(0).Item("sStockIDx")) = "" Then
            MsgBox("Item for ADD-ON PROMO seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        Dim lnRow As Integer
        For lnRow = 0 To p_oDTDetail.Rows.Count - 1
            'Check how much does he intends to borrow
            If Trim(p_oDTDetail(lnRow).Item("sStockIDx")) = "" Then
                MsgBox("Item for ADD-ON PROMO seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
                Return False
            End If
        Next

        Return True
    End Function

    Private Sub getItem(foData As DataTable, _
                        ByVal fnRow As Integer, _
                        ByVal fnItemIDx As Integer, _
                        ByVal fnItemCDE As Integer, _
                        ByVal fnItemDsc As Integer, _
                        ByVal fsValue As String, _
                        ByVal fnItemSrc As Integer, _
                        ByVal fbSearch As Boolean)

        'If there's no item to search then just exit
        If fsValue = "" Then Exit Sub

        'sStockIDxx has value so check first whether the item info was already loaded
        If foData(fnRow).Item(fnItemIDx) <> "" Then
            Select Case fnItemSrc
                Case fnItemCDE
                    If foData(fnRow).Item(fnItemCDE) = fsValue Then Exit Sub
                Case fnItemDsc
                    If foData(fnRow).Item(fnItemDsc) = fsValue Then Exit Sub
            End Select
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" & _
                       "  a.sBarCodex" & _
                       ", a.sBriefDsc" & _
                       ", a.sStockIDx" & _
               " FROM `Inventory` a" & _
        IIf(fbSearch, " WHERE a.cRecdStat = '1'", "")

        'Are we using like comparison or equality comparison
        If fbSearch Then
            Dim loRow As DataRow = KwikSearch(p_oApp _
                                             , lsSQL _
                                             , True _
                                             , fsValue _
                                             , "sBarCodex»sBriefDsc»sStockIDx" _
                                             , "Barcode»Description»Stock ID", _
                                             , "a.sBarCodex»a.sBriefDsc»a.sStockIDx" _
                                             , IIf(fnItemSrc = fnItemCDE, 0, 1))
            If IsNothing(loRow) Then
                foData(fnRow).Item(fnItemCDE) = ""
                foData(fnRow).Item(fnItemDsc) = ""
                foData(fnRow).Item(fnItemIDx) = ""
            Else
                foData(fnRow).Item(fnItemCDE) = loRow.Item("sBarCodex")
                foData(fnRow).Item(fnItemDsc) = loRow.Item("sBriefDsc")
                foData(fnRow).Item(fnItemIDx) = loRow.Item("sStockIDx")
            End If

            If foData.TableName = p_sMasTable Then
                RaiseEvent MasterRetrieved(fnItemCDE, foData(fnRow).Item(fnItemCDE))
                RaiseEvent MasterRetrieved(fnItemDsc, foData(fnRow).Item(fnItemDsc))
            Else
                RaiseEvent DetailRetrieved(fnRow, fnItemCDE, foData(fnRow).Item(fnItemCDE))
                RaiseEvent DetailRetrieved(fnRow, fnItemDsc, foData(fnRow).Item(fnItemDsc))
            End If

            Exit Sub

        End If

        Select Case fnItemSrc
            Case fnItemIDx
                lsSQL = AddCondition(lsSQL, "a.sStockIDx = " & strParm(fsValue))
            Case fnItemCDE
                lsSQL = AddCondition(lsSQL, "a.sBarCodex = " & strParm(fsValue))
            Case fnItemDsc
                lsSQL = AddCondition(lsSQL, "a.sBriefDsc = " & strParm(fsValue))
        End Select

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            foData(fnRow).Item(fnItemCDE) = ""
            foData(fnRow).Item(fnItemDsc) = ""
            foData(fnRow).Item(fnItemIDx) = ""
        ElseIf loDta.Rows.Count = 1 Then

            foData(fnRow).Item(fnItemIDx) = loDta(0).Item("sStockIDx")
            foData(fnRow).Item(fnItemCDE) = loDta(0).Item("sBarCodex")
            foData(fnRow).Item(fnItemDsc) = loDta(0).Item("sBriefDsc")
        End If

        If foData.TableName = p_sMasTable Then
            RaiseEvent MasterRetrieved(fnItemCDE, foData(fnRow).Item(fnItemCDE))
            RaiseEvent MasterRetrieved(fnItemDsc, foData(fnRow).Item(fnItemDsc))
        Else
            RaiseEvent DetailRetrieved(fnRow, fnItemCDE, foData(fnRow).Item(fnItemCDE))
            RaiseEvent DetailRetrieved(fnRow, fnItemDsc, foData(fnRow).Item(fnItemDsc))
        End If

    End Sub

    Private Function getSQ_Master() As String
        Return "SELECT a.sTransNox" & _
                    ", a.dPromoFrm" & _
                    ", a.dPromoTru" & _
                    ", a.sStockIDx" & _
                    ", a.cRecdStat" & _
                    ", a.sModified" & _
                    ", a.dModified" & _
                " FROM " & p_sMasTable & " a"
    End Function

    'Buy 1 combo item allow replacement of regular drinks to jumbo drinks
    Private Function getSQ_Detail() As String
        Return "SELECT a.sTransNox" & _
                    ", a.nEntryNox" & _
                    ", a.sStockIDx" & _
                    ", a.nQuantity" & _
                    ", a.nUnitPrce" & _
                    ", a.sReplItem" & _
                    ", a.dModified" & _
                " FROM " & p_sMasTable & " a" & _
                " ORDER BY a.nEntryNox"
    End Function

    Private Function getSQ_Browse() As String
        Return "SELECT a.sTransNox" & _
                    ", b.sBriefDsc" & _
                    ", a.dPromoFrm" & _
              " FROM " & p_sMasTable & " a" & _
                " LEFT JOIN Inventory b On a.sStockIDx = b.sStockIDx"
    End Function

    Private Function createMaster() As Boolean
        p_oDTMaster = Nothing
        p_oDTMaster = New DataTable(p_sMasTable)
        p_oDTMaster.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("sBarcodex", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 16
        p_oDTMaster.Columns.Add("dPromoFrm", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("dPromoTru", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("sStockIDx", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTMaster.Columns.Add("cRecdStat", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTMaster.Columns.Add("sModified", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("dModified", System.Type.GetType("System.DateTime"))

        Return True
    End Function

    Private Function newMaster() As Boolean
        p_oDTMaster.Rows.Add()

        p_oDTMaster(0).Item("sTransNox") = GetNextCode(p_sMasTable, "sTransNox", True, p_oApp.Connection, True, p_oApp.BranchCode)
        p_oDTMaster(0).Item("sBarcodex") = ""
        p_oDTMaster(0).Item("sBriefDsc") = ""
        p_oDTMaster(0).Item("dPromoFrm") = Now
        p_oDTMaster(0).Item("dPromoTru") = Now
        p_oDTMaster(0).Item("sStockIDx") = ""
        p_oDTMaster(0).Item("cRecdStat") = "1"

        p_nEditMode = xeEditMode.MODE_ADDNEW

        Return True
    End Function

    Private Function createDetail() As Boolean
        p_oDTDetail = Nothing
        p_oDTDetail = New DataTable(p_sDetTable)
        p_oDTDetail.Columns.Add("nEntryNox", System.Type.GetType("System.Int16")).AutoIncrement = True
        p_oDTDetail.Columns.Add("sBarcodex", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 16
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int32"))
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))

        p_oDTDetail.Columns.Add("xBarcodex", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("xBriefDsc", System.Type.GetType("System.String")).MaxLength = 16

        p_oDTDetail.Columns.Add("sStockIDx", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("sReplItem", System.Type.GetType("System.String")).MaxLength = 12

        p_oDTDetail.Columns.Add("dModified", System.Type.GetType("System.DateTime"))
        p_oDTDetail.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 12

        Return True
    End Function

    Public Function newDetail() As Boolean
        If p_oDTDetail.Rows.Count > 0 Then
            Dim lnCtr As Integer
            For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
                If p_oDTDetail(lnCtr).Item("sStockIDx") = "" Then
                    Return False
                End If
            Next
        End If

        p_oDTDetail.Rows.Add(p_oDTDetail.NewRow)
        p_nRow = p_oDTDetail.Rows.Count - 1

        p_oDTDetail(p_nRow).Item("sBarcodex") = ""
        p_oDTDetail(p_nRow).Item("sBriefDsc") = ""
        p_oDTDetail(p_nRow).Item("nQuantity") = 0
        p_oDTDetail(p_nRow).Item("nUnitPrce") = 0.0

        p_oDTDetail(p_nRow).Item("xBarcodex") = ""
        p_oDTDetail(p_nRow).Item("xBriefDsc") = ""

        p_oDTDetail(p_nRow).Item("sStockIDx") = ""
        p_oDTDetail(p_nRow).Item("sReplItem") = ""

        p_oDTDetail(p_nRow).Item("dModified") = ""
        p_oDTDetail(p_nRow).Item("sTransNox") = ""

        Return True
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTMaster = Nothing
        p_oDTDetail = Nothing

        p_sParent = ""

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub
End Class
