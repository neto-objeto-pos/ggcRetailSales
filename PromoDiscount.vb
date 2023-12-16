'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Promo Discount Transaction/Maintenance Object
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

Public Class PromoDiscount
    Private p_oApp As GRider
    Private p_oDTDetail As DataTable
    Private p_oDTMaster As DataTable
    Private p_nRow As Integer

    Private p_nEditMode As xeEditMode
    Private p_sParent As String

    Private Const p_sMasTable As String = "Promo_Discount"
    Private Const p_sDetTable As String = "Promo_Discount_Detail"

    Private Const p_sMsgHeadr As String = "Promo Discount Maintenance/Transaction"

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
                Select Case Index
                    Case 1
                        If p_oDTMaster(0).Item(Index) = "" And p_oDTMaster(0).Item("sBranchCd") <> "" Then
                            getBranch(p_oDTMaster, 0, 13, 1, p_oDTMaster(0).Item(13), 13, False)
                        End If
                    Case 2
                        If p_oDTMaster(0).Item(Index) = "" And p_oDTMaster(0).Item("sCategrCd") <> "" Then
                            getCategory(p_oDTMaster, 0, 14, 2, p_oDTMaster(0).Item(14), 14, False)
                        End If
                    Case 3, 4
                        If p_oDTMaster(0).Item(Index) = "" And p_oDTMaster(0).Item("sStockIDx") <> "" Then
                            getItem(p_oDTMaster, 0, 15, 3, 4, p_oDTMaster(0).Item(15), 15, False)
                        End If
                End Select

                Return p_oDTMaster(0).Item(Index)
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 1
                        Call getBranch(p_oDTMaster, 0, 13, 1, value, Index, False)
                    Case 2
                        Call getCategory(p_oDTMaster, 0, 14, 2, value, Index, False)
                    Case 3, 4
                        getItem(p_oDTMaster, 0, 15, 3, 4, value, Index, False)
                    Case 5, 6
                        If IsNumeric(value) Then
                            p_oDTMaster(0).Item(Index) = value
                        End If
                        RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
                    Case 7, 8
                        If IsNumeric(value) Then
                            p_oDTMaster(0).Item(Index) = value
                        End If
                        RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
                    Case 9, 10
                        If IsDate(value) Then
                            p_oDTMaster(0).Item(Index) = value
                        Else
                            p_oDTMaster(0).Item(Index) = vbNull
                        End If
                        RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
                    Case 11, 12
                        If IsDate(value) Then
                            p_oDTMaster(0).Item(Index) = value
                        Else
                            p_oDTMaster(0).Item(Index) = vbNull
                        End If
                        RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
                    Case 13
                        p_oDTMaster(0).Item(Index) = value
                        p_oDTMaster(0).Item("sBranchNm") = ""
                    Case 14
                        p_oDTMaster(0).Item(Index) = value
                        p_oDTMaster(0).Item("sCategrDs") = ""
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
                    Case 1
                        getCategory(p_oDTDetail, Row, 7, 1, p_oDTDetail(Row).Item(Index), Index, False)
                    Case 2, 3
                        getItem(p_oDTDetail, Row, 8, 2, 3, p_oDTDetail(Row).Item(Index), Index, False)
                End Select
                Return p_oDTDetail(Row).Item(Index)
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    Case 1
                        getCategory(p_oDTDetail, Row, 7, 1, value, Index, False)
                    Case 2, 3
                        getItem(p_oDTDetail, Row, 8, 2, 3, value, Index, False)
                    Case 4, 5
                        If IsNumeric(value) Then
                            p_oDTDetail(Row).Item(Index) = value
                        End If
                        RaiseEvent DetailRetrieved(Row, Index, p_oDTDetail(Row).Item(Index))
                    Case 6
                        If IsNumeric(value) Then
                            p_oDTDetail(Row).Item(Index) = value
                        End If
                        RaiseEvent DetailRetrieved(Row, Index, p_oDTDetail(Row).Item(Index))
                    Case 7
                        p_oDTDetail(Row).Item(Index) = value
                        p_oDTDetail(Row).Item(1) = ""
                    Case 8
                        p_oDTDetail(Row).Item(Index) = value
                        p_oDTDetail(Row).Item(2) = ""
                        p_oDTDetail(Row).Item(3) = ""
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
        p_oDTMaster(0).Item("sBranchCd") = loDta(0).Item("sBranchCd")
        p_oDTMaster(0).Item("sCategrCd") = loDta(0).Item("sCategrCd")
        p_oDTMaster(0).Item("sStockIDx") = loDta(0).Item("sStockIDx")
        p_oDTMaster(0).Item("nDiscRate") = loDta(0).Item("nDiscRate")
        p_oDTMaster(0).Item("nDiscAmtx") = loDta(0).Item("nDiscAmtx")
        p_oDTMaster(0).Item("nMinQtyxx") = loDta(0).Item("nMinQtyxx")
        p_oDTMaster(0).Item("nBaseQtyx") = loDta(0).Item("nBaseQtyx")
        p_oDTMaster(0).Item("dHappyHrF") = loDta(0).Item("dHappyHrF")
        p_oDTMaster(0).Item("dHappyHrT") = loDta(0).Item("dHappyHrT")
        p_oDTMaster(0).Item("dPromoFrm") = loDta(0).Item("dPromoFrm")
        p_oDTMaster(0).Item("dPromoTru") = loDta(0).Item("dPromoTru")
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
            p_oDTDetail(lnRow).Item("sCategrCd") = loDta(lnRow).Item("sCategrCd")
            p_oDTDetail(lnRow).Item("sStockIDx") = loDta(lnRow).Item("sStockIDx")
            p_oDTDetail(lnRow).Item("nDiscRate") = loDta(lnRow).Item("nDiscRate")
            p_oDTDetail(lnRow).Item("nDiscAmtx") = loDta(lnRow).Item("nDiscAmtx")
            p_oDTDetail(lnRow).Item("nMinQtyxx") = loDta(lnRow).Item("nMinQtyxx")
        Next

        p_nEditMode = xeEditMode.MODE_READY
        Return True
    End Function

    'Public Function SearchMaster
    Public Sub SearchMaster(ByVal fnIndex As Integer, _
                            ByVal fsValue As String)
        Select Case fnIndex
            Case 1  'sBranchNm
                If fsValue <> "" Then
                    getBranch(p_oDTMaster, 0, 13, 1, fsValue, fnIndex, True)
                End If
            Case 2  'sCategrDs
                If fsValue <> "" Then
                    getCategory(p_oDTMaster, 0, 14, 2, fsValue, fnIndex, True)
                End If
            Case 3, 4  'sBarrCode
                If fsValue <> "" Then
                    getItem(p_oDTMaster, 0, 15, 3, 4, fsValue, fnIndex, True)
                End If
        End Select
    End Sub

    Public Sub searchDetail(ByVal fnRow As Integer, _
                            ByVal fnIndex As Integer, _
                            ByVal fsValue As String)
        Select Case fnIndex
            Case 1  'sCategrDs
                If fsValue <> "" Then
                    getCategory(p_oDTDetail, fnRow, 7, 1, fsValue, fnIndex, True)
                End If
            Case 2, 3 'sBarrCode, sBriefDsc
                If fsValue <> "" Then
                    getItem(p_oDTDetail, fnRow, 8, 2, 3, fsValue, fnIndex, True)
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
            lsFilter = "b.sBriefDsc LIKE " & strParm(fsValue & "%") & " OR " & "c.sDescript LIKE " & strParm(fsValue & "%")
        End If

        Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , lsSQL _
                                        , False _
                                        , lsFilter _
                                        , "sTransNox»sDescript»dPromoFrm" _
                                        , "Trans No»Item»Date From", _
                                        , "a.sTransNox»IFNull(b.sBriefDsc, c.sDescript)»a.dPromoFrm" _
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
                  ", sBranchCd = " & strParm(p_oDTMaster(0).Item("sBranchCd")) & _
                  ", sCategrCd = " & strParm(p_oDTMaster(0).Item("sCategrCd")) & _
                  ", sStockIDx = " & strParm(p_oDTMaster(0).Item("sStockIDx")) & _
                  ", nDiscRate = " & p_oDTMaster(0).Item("nDiscRate") & _
                  ", nDiscAmtx = " & p_oDTMaster(0).Item("nDiscAmtx") & _
                  ", nMinQtyxx = " & p_oDTMaster(0).Item("nMinQtyxx") & _
                  ", nBaseQtyx = " & p_oDTMaster(0).Item("nBaseQtyx") & _
                  ", dHappyHrF = " & IIf(IsDate(p_oDTMaster(0).Item("dHappyHrF")), dateParm(p_oDTMaster(0).Item("dHappyHrF")), "NULL") & _
                  ", dHappyHrT = " & IIf(IsDate(p_oDTMaster(0).Item("dHappyHrT")), dateParm(p_oDTMaster(0).Item("dHappyHrT")), "NULL") & _
                  ", dPromoFrm = " & IIf(IsDate(p_oDTMaster(0).Item("dPromoFrm")), dateParm(p_oDTMaster(0).Item("dPromoFrm")), "NULL") & _
                  ", dPromoTru = " & IIf(IsDate(p_oDTMaster(0).Item("dPromoTru")), dateParm(p_oDTMaster(0).Item("dPromoTru")), "NULL") & _
                  ", cRecdStat = " & strParm(p_oDTMaster(0).Item("cRecdStat")) & _
                  ", sModified = " & strParm(p_oApp.UserID) & _
                  ", dModified = " & dateParm(p_oApp.getSysDate)
        p_oApp.Execute(lsSQL, p_sMasTable)

        'Save detail table
        Dim lnRow As Integer
        p_nRow = 0
        For lnRow = 0 To p_oDTDetail.Rows.Count - 1
            If Not (Trim(p_oDTDetail(p_nRow).Item("sCategrCd")) = "" And Trim(p_oDTDetail(p_nRow).Item("sStockIDx")) = "") Then
                p_nRow = p_nRow + 1
                lsSQL = "INSERT INTO " & p_sDetTable & _
                       " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                          ", nEntryNox = " & p_nRow & _
                          ", sCategrCd = " & strParm(p_oDTDetail(lnRow).Item("sCategrCd")) & _
                          ", sStockIDx = " & strParm(p_oDTDetail(lnRow).Item("sStockIDx")) & _
                          ", nDiscRate = " & p_oDTDetail(lnRow).Item("nDiscRate") & _
                          ", nDiscAmtx = " & p_oDTDetail(lnRow).Item("nDiscAmtx") & _
                          ", nMinQtyxx = " & p_oDTDetail(lnRow).Item("nMinQtyxx") & _
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

    Public Function searchDiscount(ByVal fsStockIDx As String, ByVal fdTransact As Date) As Boolean
        Dim lsSQL As String
        Dim loDta As DataTable

        'Extract the promo for the item before the category...
        lsSQL = "SELECT sTransNox" & _
               " FROM " & p_sMasTable & _
               " WHERE (sStockIDx = " & strParm(fsStockIDx) & " OR sCategrCd = " & strParm(fsStockIDx) & ")" & _
                 " AND " & dateParm(fdTransact) & " BETWEEN IFNULL(dPromoFrm, " & dateParm(fdTransact) & ") AND IFNULL(dPromoTru, NOW())" & _
                 " AND TIME(" & dateParm(fdTransact) & ") BETWEEN TIME(IFNULL(dPromoFrm, " & strParm(fdTransact) & ")) AND TIME(IFNULL(dPromoTru, NOW()))" & _
                 " AND cRecdStat = '1'" & _
               " ORDER BY sStockIDx DESC"

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
        If Trim(p_oDTMaster(0).Item("sCategrCd")) = "" And Trim(p_oDTMaster(0).Item("sStockIDx")) = "" Then
            MsgBox("Item for PROMO DISCOUNT seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        Return True
    End Function

    Private Sub getBranch(foData As DataTable, _
                        ByVal fnRow As Integer, _
                        ByVal fnItemCDE As Integer, _
                        ByVal fnItemDsc As Integer, _
                        ByVal fsValue As String, _
                        ByVal fnItemSrc As Integer, _
                        ByVal fbSearch As Boolean)

        'If there's no item to search then just exit
        If fsValue = "" Then Exit Sub

        'sStockIDxx has value so check first whether the item info was already loaded
        If foData(fnRow).Item(fnItemCDE) <> "" Then
            Select Case fnItemSrc
                Case fnItemDsc
                    If foData(fnRow).Item(fnItemDsc) = fsValue Then Exit Sub
            End Select
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" & _
                       "  a.sBranchCD" & _
                       ", a.sBranchNm" & _
               " FROM `Branch` a" & _
        IIf(fbSearch, " WHERE a.cRecdStat = '1'", "")

        'Are we using like comparison or equality comparison
        If fbSearch Then
            Dim loRow As DataRow = KwikSearch(p_oApp _
                                             , lsSQL _
                                             , True _
                                             , fsValue _
                                             , "sBranchCD»sBranchNm" _
                                             , "Code»Branch", _
                                             , "a.sBranchCD»a.sBranchNm" _
                                             , IIf(fnItemSrc = fnItemCDE, 0, 1))
            If IsNothing(loRow) Then
                foData(fnRow).Item(fnItemCDE) = ""
                foData(fnRow).Item(fnItemDsc) = ""
            Else
                foData(fnRow).Item(fnItemCDE) = loRow.Item("sBranchCD")
                foData(fnRow).Item(fnItemDsc) = loRow.Item("sBranchNm")
            End If

            If foData.TableName = p_sMasTable Then
                RaiseEvent MasterRetrieved(fnItemDsc, foData(fnRow).Item(fnItemDsc))
            Else
                RaiseEvent DetailRetrieved(fnRow, fnItemDsc, foData(fnRow).Item(fnItemDsc))
            End If

            Exit Sub

        End If

        Select Case fnItemSrc
            Case fnItemCDE
                lsSQL = AddCondition(lsSQL, "a.sBranchCD = " & strParm(fsValue))
            Case fnItemDsc
                lsSQL = AddCondition(lsSQL, "a.sBranchNm = " & strParm(fsValue))
        End Select

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            foData(fnRow).Item(fnItemCDE) = ""
            foData(fnRow).Item(fnItemDsc) = ""
        ElseIf loDta.Rows.Count = 1 Then
            foData(fnRow).Item(fnItemCDE) = loDta(0).Item("sBranchCD")
            foData(fnRow).Item(fnItemDsc) = loDta(0).Item("sBranchNm")
        End If

        If foData.TableName = p_sMasTable Then
            RaiseEvent MasterRetrieved(fnItemDsc, foData(fnRow).Item(fnItemDsc))
        Else
            RaiseEvent DetailRetrieved(fnRow, fnItemDsc, foData(fnRow).Item(fnItemDsc))
        End If
    End Sub

    Private Sub getCategory(foData As DataTable, _
                        ByVal fnRow As Integer, _
                        ByVal fnItemCDE As Integer, _
                        ByVal fnItemDsc As Integer, _
                        ByVal fsValue As String, _
                        ByVal fnItemSrc As Integer, _
                        ByVal fbSearch As Boolean)

        'If there's no item to search then just exit
        If fsValue = "" Then Exit Sub

        'sStockIDxx has value so check first whether the item info was already loaded
        If foData(fnRow).Item(fnItemCDE) <> "" Then
            Select Case fnItemSrc
                Case fnItemDsc
                    If foData(fnRow).Item(fnItemDsc) = fsValue Then Exit Sub
            End Select
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" & _
                       "  a.sCategrCd" & _
                       ", a.sDescript" & _
               " FROM `Product_Category` a" & _
        IIf(fbSearch, " WHERE a.cRecdStat = '1'", "")

        'Are we using like comparison or equality comparison
        If fbSearch Then
            Dim loRow As DataRow = KwikSearch(p_oApp _
                                             , lsSQL _
                                             , True _
                                             , fsValue _
                                             , "sCategrCd»sDescript" _
                                             , "Code»Category", _
                                             , "a.sCategrCd»a.sDescript" _
                                             , IIf(fnItemSrc = fnItemCDE, 0, 1))
            If IsNothing(loRow) Then
                foData(fnRow).Item(fnItemCDE) = ""
                foData(fnRow).Item(fnItemDsc) = ""
            Else
                foData(fnRow).Item(fnItemCDE) = loRow.Item("sCategrCd")
                foData(fnRow).Item(fnItemDsc) = loRow.Item("sDescript")
            End If

            If foData.TableName = p_sMasTable Then
                RaiseEvent MasterRetrieved(fnItemDsc, foData(fnRow).Item(fnItemDsc))
            Else
                RaiseEvent DetailRetrieved(fnRow, fnItemDsc, foData(fnRow).Item(fnItemDsc))
            End If

            Exit Sub

        End If

        Select Case fnItemSrc
            Case fnItemCDE
                lsSQL = AddCondition(lsSQL, "a.sCategrCd = " & strParm(fsValue))
            Case fnItemDsc
                lsSQL = AddCondition(lsSQL, "a.sDescript = " & strParm(fsValue))
        End Select

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            foData(fnRow).Item(fnItemCDE) = ""
            foData(fnRow).Item(fnItemDsc) = ""
        ElseIf loDta.Rows.Count = 1 Then
            foData(fnRow).Item(fnItemCDE) = loDta(0).Item("sCategrCd")
            foData(fnRow).Item(fnItemDsc) = loDta(0).Item("sDescript")
        End If

        If foData.TableName = p_sMasTable Then
            RaiseEvent MasterRetrieved(fnItemDsc, foData(fnRow).Item(fnItemDsc))
        Else
            RaiseEvent DetailRetrieved(fnRow, fnItemDsc, foData(fnRow).Item(fnItemDsc))
        End If
    End Sub

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
                    ", a.sBranchCd" & _
                    ", a.sCategrCd" & _
                    ", a.sStockIDx" & _
                    ", a.nDiscRate" & _
                    ", a.nDiscAmtx" & _
                    ", a.nMinQtyxx" & _
                    ", a.nBaseQtyx" & _
                    ", a.dHappyHrF" & _
                    ", a.dHappyHrT" & _
                    ", a.dPromoFrm" & _
                    ", a.dPromoTru" & _
                    ", a.cRecdStat" & _
                    ", a.sModified" & _
                    ", a.dModified" & _
                " FROM " & p_sMasTable & " a"
    End Function

    'Buy 1 combo item allow replacement of regular drinks to jumbo drinks
    Private Function getSQ_Detail() As String
        Return "SELECT a.sTransNox" & _
                    ", a.nEntryNox" & _
                    ", a.sCategrCd" & _
                    ", a.sStockIDx" & _
                    ", a.nDiscRate" & _
                    ", a.nDiscAmtx" & _
                    ", a.nMinQtyxx" & _
                " FROM " & p_sMasTable & " a" & _
                " ORDER BY a.nEntryNox"
    End Function

    Private Function getSQ_Browse() As String
        Return "SELECT a.sTransNox" & _
                    ", IFNull(b.sBriefDsc, c.sDescript) sDescript" & _
                    ", a.dPromoFrm" & _
              " FROM " & p_sMasTable & " a" & _
                " LEFT JOIN Inventory b On a.sStockIDx = b.sStockIDx" & _
                " LEFT JOIN Product_Category c ON a.sCategrCd = c.sCategrCd"
    End Function

    Private Function createMaster() As Boolean
        p_oDTMaster = Nothing
        p_oDTMaster = New DataTable(p_sMasTable)
        p_oDTMaster.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTMaster.Columns.Add("sBranchNm", System.Type.GetType("System.String")).MaxLength = 50
        p_oDTMaster.Columns.Add("sCategrDs", System.Type.GetType("System.String")).MaxLength = 32
        p_oDTMaster.Columns.Add("sBarcodex", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTMaster.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 16
        p_oDTMaster.Columns.Add("nDiscRate", System.Type.GetType("System.Decimal"))
        p_oDTMaster.Columns.Add("nDiscAmtx", System.Type.GetType("System.Decimal"))
        p_oDTMaster.Columns.Add("nMinQtyxx", System.Type.GetType("System.Int32"))
        p_oDTMaster.Columns.Add("nBaseQtyx", System.Type.GetType("System.Int32"))
        p_oDTMaster.Columns.Add("dHappyHrF", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("dHappyHrT", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("dPromoFrm", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("dPromoTru", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("sBranchCd", System.Type.GetType("System.String")).MaxLength = 4
        p_oDTMaster.Columns.Add("sCategrCd", System.Type.GetType("System.String")).MaxLength = 4
        p_oDTMaster.Columns.Add("sStockIDx", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTMaster.Columns.Add("cRecdStat", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTMaster.Columns.Add("sModified", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("dModified", System.Type.GetType("System.DateTime"))

        Return True
    End Function

    Private Function newMaster() As Boolean
        p_oDTMaster.Rows.Add()

        p_oDTMaster(0).Item("sTransNox") = GetNextCode(p_sMasTable, "sTransNox", True, p_oApp.Connection, True, p_oApp.BranchCode)
        p_oDTMaster(0).Item("sBranchNm") = ""
        p_oDTMaster(0).Item("sCategrDs") = ""
        p_oDTMaster(0).Item("sBarcodex") = ""
        p_oDTMaster(0).Item("sBriefDsc") = ""
        p_oDTMaster(0).Item("nDiscRate") = 0.0
        p_oDTMaster(0).Item("nDiscAmtx") = 0.0
        p_oDTMaster(0).Item("nMinQtyxx") = 0
        p_oDTMaster(0).Item("nBaseQtyx") = 0
        p_oDTMaster(0).Item("dHappyHrF") = Now
        p_oDTMaster(0).Item("dHappyHrT") = Now
        p_oDTMaster(0).Item("dPromoFrm") = Now
        p_oDTMaster(0).Item("dPromoTru") = Now
        p_oDTMaster(0).Item("sBranchCd") = ""
        p_oDTMaster(0).Item("sCategrCd") = ""
        p_oDTMaster(0).Item("sStockIDx") = ""
        p_oDTMaster(0).Item("cRecdStat") = "1"

        p_nEditMode = xeEditMode.MODE_ADDNEW

        Return True
    End Function

    Private Function createDetail() As Boolean
        p_oDTDetail = Nothing
        p_oDTDetail = New DataTable(p_sDetTable)
        p_oDTDetail.Columns.Add("nEntryNox", System.Type.GetType("System.Int16")).AutoIncrement = True
        p_oDTMaster.Columns.Add("sCategrDs", System.Type.GetType("System.String")).MaxLength = 32
        p_oDTDetail.Columns.Add("sBarcodex", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 16
        p_oDTDetail.Columns.Add("nDiscRate", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nDiscAmtx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nMinQtyxx", System.Type.GetType("System.Int32"))
        p_oDTDetail.Columns.Add("sCategrCd", System.Type.GetType("System.String")).MaxLength = 4
        p_oDTDetail.Columns.Add("sStockIDx", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("dModified", System.Type.GetType("System.DateTime"))
        p_oDTDetail.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 12

        Return True
    End Function

    Public Function newDetail() As Boolean
        If p_oDTDetail.Rows.Count > 0 Then
            Dim lnCtr As Integer
            For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
                If p_oDTDetail(lnCtr).Item("sCategrCd") = "" And p_oDTDetail(lnCtr).Item("sStockIDx") = "" Then
                    Return False
                End If
            Next
        End If

        p_oDTDetail.Rows.Add(p_oDTDetail.NewRow)
        p_nRow = p_oDTDetail.Rows.Count - 1

        p_oDTDetail(p_nRow).Item("sCategrDs") = ""
        p_oDTDetail(p_nRow).Item("sBarcodex") = ""
        p_oDTDetail(p_nRow).Item("sBriefDsc") = ""
        p_oDTDetail(p_nRow).Item("nDiscRate") = 0.0
        p_oDTDetail(p_nRow).Item("nDiscAmtx") = 0.0
        p_oDTDetail(p_nRow).Item("nMinQtyxx") = 0
        p_oDTDetail(p_nRow).Item("sCategrCd") = ""
        p_oDTDetail(p_nRow).Item("sStockIDx") = ""
        p_oDTDetail(p_nRow).Item("dModified") = Now()
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
