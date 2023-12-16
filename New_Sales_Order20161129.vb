'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Sales Order Object
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
'  XerSys [ 10/13/2016 09:37 am ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports MySql.Data.MySqlClient
Imports ggcAppDriver
Imports ggcReceipt

Public Class New_Sales_Order
    Private p_oApp As GRider
    Private p_oDTDetail As DataTable
    Private p_oDTMaster As DataTable

    Private p_sPOSNo As String
    Private p_sAccreditNo As String

    Private p_nQuantity As Integer
    Private p_sWaiterID As String
    Private p_nTranTotl As Double

    Private p_bExisting As xeLogical
    Private p_nRow As Integer

    Private p_bShowMsg As Boolean
    Private p_sParent As String
    Private p_bNotify As Boolean ' this must be set on configuration table for notifications eg promos

    Private p_sBranchCd As String 'Branch code of the transaction to retrieve
    Private p_sBranchNm As String 'Branch Name of the transaction to retrieve
    
    Private Const pxeMasTable As String = "SO_Master"
    Private Const pxeDetTable As String = "SO_Detail"
    Private Const pxeFinGoods As String = "FsGd"
    Private Const pxeDelimtr As String = "»"
    Private Const pxeCtrFormat As String = "000000"
    Private Const pxeSourceCde As String = "SO"

    Public Event DetailRetrieved(ByVal Row As Integer, _
                                 ByVal Index As Integer, _
                                 ByVal Value As Object)    'Property Detail(Integer)

    Public ReadOnly Property POSNumber()
        Get
            Return p_sPOSNo
        End Get
    End Property

    Public ReadOnly Property Accreditation()
        Get
            Return p_sAccreditNo
        End Get
    End Property

    Public ReadOnly Property Master(ByVal Index As String) As Object
        Get
            Select Case LCase(Index)
                Case "stransnox"
                    Return p_oDTMaster(0).Item("sTransNox")
                Case "dtransact"
                    Return p_oDTMaster(0).Item("dTransact")
                Case "ncontrlno"
                    Return p_oDTMaster(0).Item("nContrlNo")
                Case "ntrantotl"
                    Return p_oDTMaster(0).Item("nTranTotl")
                Case "scashrnme"
                    If p_oDTMaster(0).Item("sCashrNme") = "" Then
                        If p_oDTMaster(0).Item("sCashierx") <> "" Then
                            Call getClient(7, 5, p_oDTMaster(0).Item("sWaiterID"), True, False)
                        End If
                    End If
                    Return p_oDTMaster(0).Item("sCashrNme")
                Case "sidnumber"
                    Return p_oDTMaster(0).Item("sTableNox")
                Case "swaiternm"
                    If p_oDTMaster(0).Item("sWaiterNm") = vbEmpty Then
                        If p_oDTMaster(0).Item("sWaiterID") <> vbEmpty Then
                            Call getClient(8, 6, p_oDTMaster(0).Item("sWaiterID"), True, False)
                        End If
                    End If
                    Return p_oDTMaster(0).Item("sWaiterNm")
                Case Else
                    Return vbEmpty
            End Select
        End Get
    End Property

    Public ReadOnly Property Master(ByVal Index As Integer) As Object
        Get
            Select Case Index
                Case 5
                    If p_oDTMaster(0).Item("sCashrNme") = vbEmpty Then
                        If p_oDTMaster(0).Item("sCashierx") <> vbEmpty Then
                            Call getClient(7, 5, p_oDTMaster(0).Item("sWaiterID"), True, False)
                        End If
                    End If
                    Return p_oDTMaster(0).Item("sCashrNme")
                Case 6
                    If p_oDTMaster(0).Item("sWaiterNm") = vbEmpty Then
                        If p_oDTMaster(0).Item("sWaiterID") <> vbEmpty Then
                            Call getClient(8, 6, p_oDTMaster(0).Item("sWaiterID"), True, False)
                        End If
                    End If
                    Return p_oDTMaster(0).Item("sWaiterNm")
                Case 0 To 4, 7 To 10
                    Return p_oDTMaster(0).Item(Index)
                Case Else
                    Return vbEmpty
            End Select
        End Get
    End Property

    'Property Detail(Integer, String)
    Public ReadOnly Property Detail(ByVal Row As Integer, ByVal Index As String) As Object
        Get
            If Row + 1 > ItemCount Then
                Return vbEmpty
            End If

            Select Case LCase(Index)
                Case "nentrynox"
                Case "sbarcodex"
                Case "sdescript"
                Case "nunitprce"
                Case "creversex"
                Case "nquantity"
                Case "ndiscount"
                Case "nadddiscx"
                Case "cprintedx"
                Case "ccomplmnt"
                Case "cdetailxx"
                Case "creversed"
                Case Else
                    Return vbEmpty
            End Select
            Return p_oDTDetail(Row).Item(Index)
        End Get
    End Property

    'Property Detail(Integer, Integer)
    Public ReadOnly Property Detail(ByVal Row As Integer, ByVal Index As Integer) As Object
        Get
            If Row + 1 > ItemCount Then
                Return vbEmpty
            End If

            Select Case LCase(Index)
                Case 0
                Case 1
                Case 2
                Case 3
                Case 4
                Case 5
                Case 6
                Case 7
                Case 8
                Case 9
                Case 18
                Case 24
                Case Else
                    Return vbEmpty
            End Select
            Return p_oDTDetail(Row).Item(Index)
        End Get
    End Property

    Public Property Quantity() As Integer
        Get
            Return p_nQuantity
        End Get
        Set(ByVal value As Integer)
            p_nQuantity = value
        End Set
    End Property

    'Property RecordCount()
    Public ReadOnly Property ItemCount() As Integer
        Get
            Return p_oDTDetail.Rows.Count
        End Get
    End Property

    'Property ShowMessage()
    Public Property ShowMessage() As Boolean
        Get
            Return p_bShowMsg
        End Get
        Set(ByVal value As Boolean)
            p_bShowMsg = value
        End Set
    End Property

    'Property ()
    Public ReadOnly Property BranchCode() As String
        Get
            Return p_sBranchCd
        End Get
    End Property

    Public ReadOnly Property BranchName() As String
        Get
            Return p_sBranchNm
        End Get
    End Property

    'Public Function OpenTransaction(String)
    Public Function LoadOrder(ByVal fnTableNo As Integer) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = AddCondition(getSQ_Order, "sTableNox LIKE " & strParm("%" & fnTableNo & "%"))

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            p_bExisting = False
            Return False
        End If

        p_oDTMaster(0).Item("sTransNox") = loDT(0).Item("sTransNox")
        p_oDTMaster(0).Item("dTransact") = loDT(0).Item("dTransact")
        p_oDTMaster(0).Item("nContrlNo") = loDT(0).Item("nContrlNo")
        p_oDTMaster(0).Item("nTranTotl") = loDT(0).Item("nTranTotl")
        p_oDTMaster(0).Item("sCashrNme") = ""
        p_oDTMaster(0).Item("sWaiterNm") = ""
        p_oDTMaster(0).Item("sCashierx") = loDT(0).Item("sCashierx")
        p_oDTMaster(0).Item("sWaiterID") = loDT(0).Item("sWaiterID")
        p_oDTMaster(0).Item("sReceiptx") = loDT(0).Item("sReceiptx")
        p_oDTMaster(0).Item("sMergeIDx") = loDT(0).Item("sMergeIDx")
        p_oDTMaster(0).Item("sTableNox") = loDT(0).Item("sTableNox")

        lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        loDT = p_oApp.ExecuteQuery(lsSQL)

        Dim lnCtr As Integer
        Call createDetail()

        For lnCtr = 0 To loDT.Rows.Count - 1
            newDetail()

            p_oDTDetail(lnCtr).Item("sBarcodex") = loDT(lnCtr).Item("sBarcodex")
            p_oDTDetail(lnCtr).Item("sDescript") = loDT(lnCtr).Item("sDescript")
            p_oDTDetail(lnCtr).Item("sBriefDsc") = loDT(lnCtr).Item("sBriefDsc")
            p_oDTDetail(lnCtr).Item("nUnitPrce") = loDT(lnCtr).Item("nUnitPrce")
            p_oDTDetail(lnCtr).Item("cReversex") = loDT(lnCtr).Item("cReversex")
            p_oDTDetail(lnCtr).Item("nQuantity") = loDT(lnCtr).Item("nQuantity")
            p_oDTDetail(lnCtr).Item("nDiscount") = loDT(lnCtr).Item("nDiscount")
            p_oDTDetail(lnCtr).Item("nAddDiscx") = loDT(lnCtr).Item("nAddDiscx")
            p_oDTDetail(lnCtr).Item("nDiscLev1") = loDT(lnCtr).Item("nDiscLev1")
            p_oDTDetail(lnCtr).Item("nDiscLev2") = loDT(lnCtr).Item("nDiscLev2")
            p_oDTDetail(lnCtr).Item("nDiscLev3") = loDT(lnCtr).Item("nDiscLev3")
            p_oDTDetail(lnCtr).Item("cDetSaved") = 1
            'p_oDTDetail(lnCtr).Item("nDiscAmtx") = loDT(lnCtr).Item("nDiscAmtx")
            'p_oDTDetail(lnCtr).Item("nDealrDsc") = loDT(lnCtr).Item("nDealrDsc")
        Next

        p_nTranTotl = p_oDTMaster(0).Item("nTranTotl")
        p_sWaiterID = p_oDTMaster(0).Item("sWaiterID")

        p_bExisting = True
        Return True
    End Function

    Public Function SearchItem( _
                              ByVal fsValue As String _
                            , Optional ByVal fbByCode As Boolean = False) As Boolean

        Dim lsFilter As String

        If fbByCode Then
            lsFilter = "sBarcodex LIKE " & strParm(fsValue & "%")
        Else
            lsFilter = "sDescript LIKE " & strParm(fsValue & "%")
        End If

        Debug.Print(AddCondition(getSQ_Search, lsFilter))
        Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , getSQ_Search _
                                        , False _
                                        , lsFilter _
                                        , "sBarcodex»sDescript»nUnitPrce" _
                                        , "Bar Code»Description»Unit Price", _
                                        , "sBarcodex»sDescript»nUnitPrce" _
                                        , IIf(fbByCode, 0, 1))
        If IsNothing(loDta) Then
            Return False
        Else
            Return AddOrder(loDta("sBarcodex"), p_nQuantity)
        End If
    End Function

    Public Function AddOrder(ByVal fsBarrCode As String, Optional ByVal fnQuantity As Integer = 1) As Boolean
        Dim lsSQL As String
        Dim loDT As New DataTable
        Dim lnRow As Integer

        ' first retrieve the detail
        lsSQL = AddCondition(getSQ_Search, "sBarcodex = " & strParm(fsBarrCode))
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            Return False
        End If

        ' Now add the retrieve info to the detail
        Call newDetail(loDT)

        p_oDTDetail(p_nRow).Item("nQuantity") = fnQuantity
        ' store this row coz it might be incremented on the following procedure

        lnRow = p_nRow
        ' check if inventory has promo
        If loDT(0).Item("cWthPromo") = xeLogical.YES Then
            Call procPromo()
        End If

        If loDT(0).Item("cComboMlx") = xeLogical.YES Then
            Call procCombo()
        End If

        For lnRow = lnRow To p_nRow
            Call saveDetail(lnRow)
        Next

        ' update tran totals
        Call computeTotal()
        Return True
    End Function

    Public Function ReverseOrder(ByVal fsBarrCode As String, Optional ByVal fnQuantity As Integer = 1) As Boolean
        Dim lnRow As Integer

        ' check for the info that is being reverse
        lnRow = -1
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnCtr).Item("sBarcodex") = fsBarrCode Then
                lnRow = lnCtr
            End If
        Next

        If lnRow < 0 Then
            Return False
        End If

        Call newDetail()

        p_oDTDetail(p_nRow).Item("sBarcodex") = p_oDTDetail(lnRow).Item("sBarcodex")
        p_oDTDetail(p_nRow).Item("sDescript") = p_oDTDetail(lnRow).Item("sDescript")
        p_oDTDetail(p_nRow).Item("sBriefDsc") = p_oDTDetail(lnRow).Item("sBriefDsc")
        p_oDTDetail(p_nRow).Item("nUnitPrce") = p_oDTDetail(lnRow).Item("nUnitPrce")
        p_oDTDetail(p_nRow).Item("sStockIDx") = p_oDTDetail(lnRow).Item("sStockIDx")
        p_oDTDetail(p_nRow).Item("nDiscLev1") = p_oDTDetail(lnRow).Item("nDiscLev1")
        p_oDTDetail(p_nRow).Item("nDiscLev2") = p_oDTDetail(lnRow).Item("nDiscLev2")
        p_oDTDetail(p_nRow).Item("nDiscLev3") = p_oDTDetail(lnRow).Item("nDiscLev3")
        p_oDTDetail(p_nRow).Item("nDiscAmtx") = p_oDTDetail(lnRow).Item("nDiscAmtx")
        p_oDTDetail(p_nRow).Item("nDealrDsc") = p_oDTDetail(lnRow).Item("nDealrDsc")
        p_oDTDetail(p_nRow).Item("cComboMlx") = p_oDTDetail(lnRow).Item("cComboMlx")
        p_oDTDetail(p_nRow).Item("cWthPromo") = p_oDTDetail(lnRow).Item("cWthPromo")
        p_oDTDetail(p_nRow).Item("sCategrID") = p_oDTDetail(lnRow).Item("sCategrID")
        p_oDTDetail(p_nRow).Item("nQuantity") = fnQuantity
        p_oDTDetail(p_nRow).Item("cReversex") = "-"
        p_oDTDetail(p_nRow).Item("dModified") = Now()

        'save detail
        Call saveDetail(p_nRow)

        'reverse items
        Call reverseCombo(p_oDTDetail(p_nRow).Item("sStockIDx"), p_oDTDetail(p_nRow).Item("nQuantity"))

        'set tag detail as reversed
        p_oDTDetail(lnRow).Item("cReversed") = "1"

        ' update tran totals
        Call computeTotal()
        Return True
    End Function

    Private Function reverseCombo(ByVal fsStockIDx As String, Optional ByVal fnQuantity As Integer = 1) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lnRow As Integer

        lsSQL = getSQ_Combo(fsStockIDx)
        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            Return True
        End If

        ' add the detail in the order
        For lnRow = 0 To loDT.Rows.Count - 1
            Call newDetail()

            ' assign item details
            p_oDTDetail(p_nRow).Item("sStockIDx") = loDT(lnRow).Item("sStockIDx")
            p_oDTDetail(p_nRow).Item("sBarcodex") = loDT(lnRow).Item("sBarcodex")
            p_oDTDetail(p_nRow).Item("sDescript") = loDT(lnRow).Item("sDescript")
            p_oDTDetail(p_nRow).Item("sBriefDsc") = loDT(lnRow).Item("sBriefDsc")
            p_oDTDetail(p_nRow).Item("nQuantity") = fnQuantity
            p_oDTDetail(p_nRow).Item("cReversex") = "-"
            p_oDTDetail(p_nRow).Item("dModified") = Now
            p_oDTDetail(p_nRow).Item("cDetailxx") = 1

            Call saveDetail(p_nRow)
        Next

        Return True
    End Function

    Public Function SaveTransaction() As Boolean
        Dim lsSQL As String = ""
        Dim lsTableNo As String = ""

        Dim loForm As frmPickTable

        loForm = New frmPickTable
        With loForm
            .TableNo = p_oDTMaster(0).Item("sTableNox")
            .ShowDialog()
            If .Cancel Then Return False

            lsTableNo = .TableNo
        End With

        p_oApp.BeginTransaction()
        If p_bExisting Then
            ' check the following field
            If p_nTranTotl <> p_oDTMaster(0).Item("nTranTotl") Then
                lsSQL = ", nTranTotl = " & p_oDTMaster(0).Item("nTranTotl")
            End If

            If p_sWaiterID <> p_oDTMaster(0).Item("sWaiterID") Then
                lsSQL = lsSQL & ", sWaiterID = " & strParm(p_oDTMaster(0).Item("sWaiterID"))
            End If

            If lsTableNo <> p_oDTMaster(0).Item("sTableNox") Then
                Dim lasDetail() As String
                Dim lasSpitted() As String = Split(lsTableNo, ",")
                Dim lnCtr As Integer
                Dim lnNox As Integer

                lsTableNo = ""
                For lnCtr = 0 To UBound(lasSpitted)
                    If lasSpitted(lnCtr) <> "" And lasSpitted(lnCtr) <> "-" Then
                        If lasSpitted(lnCtr).Contains("-") Then
                            'number range
                            lasDetail = Split(lasSpitted(lnCtr), "-")

                            For lnNox = lasDetail(0) To lasDetail(1)
                                lsTableNo = lnNox & "," & lsTableNo
                            Next
                        Else
                            'separated by ,
                            lsTableNo = lasSpitted(lnCtr) & "," & lsTableNo
                        End If
                    End If
                Next
                lsTableNo = Left(lsTableNo, Len(lsTableNo) - 1)

                lsSQL = lsSQL & ", sTableNox = " & strParm(Replace(lsTableNo, ",", pxeDelimtr))

                'update table status to occupied
                Call updateTable(lsTableNo)
            End If

            If lsSQL <> "" Then
                lsSQL = "UPDATE " & pxeMasTable & _
                        " SET " & Mid(lsSQL, 3) & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox"))

                p_oApp.Execute(lsSQL, pxeMasTable)
            End If
        Else
            p_oDTMaster(0).Item("sTableNox") = Replace(lsTableNo, ",", pxeDelimtr)

            Call saveMaster()
        End If

        'after saving the master, loop through the detail and check for pending detail
        Dim lnRow As Integer

        For lnctr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnRow).Item("cDetSaved") = xeLogical.NO Then
                Call saveDetail(lnRow)

                p_oDTDetail(lnRow).Item("cDetSaved") = xeLogical.YES
            End If
        Next

        p_oApp.CommitTransaction()

        Call createMaster()
        Call initMaster()

        Return True
    End Function

    Public Function updateTable(lsTableNox As String) As Boolean
        Dim lsSQL As String = ""
        Dim lnCtr As Integer
        Dim lasTable() As String

        If lsTableNox = "" Then
            MsgBox("Invalid Table Number Detected!!!", MsgBoxStyle.Critical, "Warning")
            Return False
        End If

        lasTable = Split(lsTableNox, ",")
        For lnCtr = 0 To UBound(lasTable)
            lsSQL = lsSQL & lasTable(lnCtr) & ", "
        Next
        lsSQL = "nTableNox IN (" & Strings.Left(lsSQL, Strings.Len(lsSQL) - 2) & ")"

        lsSQL = AddCondition("UPDATE Table_Master SET cStatusxx = '1'", lsSQL)

        If lsSQL <> "" Then p_oApp.Execute(lsSQL, "Table_Master")

        Return True
    End Function

    Public Function PayOrder() As Boolean
        Dim lbSuccess As Boolean

        'enter payment
        Dim loPayment As Receipt
        loPayment = New Receipt(p_oApp)

        With loPayment
            .SourceCd = pxeSourceCde
            .SourceNo = p_oDTMaster(0)("sTransNox")
            .Master("nSalesAmt") = Master("nTranTotl")
            lbSuccess = .payTransaction()
        End With

        Return lbSuccess
    End Function

    Public Function MergeOrder() As Boolean
        Dim loMerge As MergeOrder

        loMerge = New MergeOrder(p_oApp)

        With loMerge
            .Branch = p_oApp.BranchCode
            .ShowMergeTable()
        End With

        'Dim lasTable() As String
        'Dim lsTable As String = "" ' Replace this later on when the form is available
        'Dim lsSQL As String = ""
        'Dim lnCtr As Integer
        'Dim loDT As DataTable

        'lasTable = Split(lsTable, ",")
        'For lnCtr = 0 To UBound(lasTable)
        '    lsSQL = ", " & lasTable(lnCtr)
        'Next

        'lsSQL = "SELECT sTransNox" & _
        '        " FROM " & pxeMasTable & _
        '        " WHERE sTableNox IN (" & Mid(lsSQL, 3) & ")"

        'loDT = p_oApp.ExecuteQuery(lsSQL)
        'If loDT.Rows.Count = 0 Then Return False

        'lsSQL = ""
        'For lnCtr = 0 To loDT.Rows.Count - 1
        '    lsSQL = lsSQL & ", " & strParm(loDT(0).Item("sTransNox"))
        'Next

        'lsSQL = "UPDATE " & pxeMasTable & _
        '        " SET sMergeIDx = " & strParm(getNextMergeID()) & _
        '        " WHERE sTransNox IN (" & Mid(lsSQL, 3) & ")"
        'p_oApp.Execute(lsSQL, pxeMasTable)

        Return True
    End Function

    Public Function SplitOrder() As Boolean
        'TODO: insert form entry for splitting of order
        '      form must return data table

        Dim lsSQL As String = ""
        Dim lnCtr As Integer
        Dim loDT As DataTable

        ' put the object call here
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then Return False

        lsSQL = ""
        For lnCtr = 0 To loDT.Rows.Count - 1
            lsSQL = lsSQL & ", " & strParm(loDT(0).Item("sTransNox"))
        Next

        lsSQL = "UPDATE " & pxeMasTable & _
                " SET sMergeIDx = " & strParm(getNextMergeID()) & _
                " WHERE sTransNox IN (" & Mid(lsSQL, 3) & ")"
        p_oApp.Execute(lsSQL, pxeMasTable)

        Return True
    End Function

    Public Function ChangeQty(ByVal fnRowNo As Integer) As Boolean
        Dim loForm As frmChangeQty

        loForm = New frmChangeQty
        With loForm
            .Description = p_oDTDetail(fnRowNo)("sDescript")
            .Quantity = p_oDTDetail(fnRowNo)("nQuantity")
            .ShowDialog()

            If .Cancel Then Return False

            p_nQuantity = .Quantity

            If p_nQuantity = p_oDTDetail(fnRowNo)("nQuantity") Then Return True
        End With

        If p_bExisting Then
            ' new transaction
            If fnRowNo > p_oDTDetail.Rows.Count - 1 Then
                Return False
            ElseIf p_oDTDetail(fnRowNo).Item("cPrintedx") = xeLogical.YES Then
                Return False
            ElseIf p_oDTDetail(fnRowNo).Item("cDetSaved") = xeLogical.YES Then
                Return updateQty(fnRowNo)
            Else
                p_oDTDetail(fnRowNo).Item("nQuantity") = p_nQuantity
            End If
        Else
            If p_oDTDetail(fnRowNo).Item("cDetSaved") = xeLogical.YES Then
                Return updateQty(fnRowNo)
            Else
                p_oDTDetail(fnRowNo).Item("nQuantity") = p_nQuantity
            End If
        End If

        Call computeTotal()
        Return True
    End Function

    Public Function ChangePrice(ByVal fnRowNo As Integer) As Boolean
        Dim lsSQL As String
        Dim lnNewPrice As Double

        Dim loForm As frmChangePrice

        loForm = New frmChangePrice
        With loForm
            .Description = p_oDTDetail(fnRowNo)("sDescript")
            .UnitPrice = p_oDTDetail(fnRowNo)("nUnitPrce")
            .ShowDialog()

            If .Cancel Then Return False

            lnNewPrice = .UnitPrice

            If lnNewPrice = p_oDTDetail(fnRowNo)("nUnitPrce") Then Return True
        End With

        Try
            If p_sParent = "" Then p_oApp.BeginTransaction()

            lsSQL = "UPDATE " & pxeDetTable & _
                    " SET nUnitPrce = " & lnNewPrice & _
                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                        " AND nEntryNox = " & fnRowNo

            p_oApp.Execute(lsSQL, pxeDetTable)
        Catch ex As Exception
            If p_sParent = "" Then p_oApp.RollBackTransaction()
            Return False
        Finally
            If p_sParent = "" Then p_oApp.CommitTransaction()
        End Try

        p_oDTDetail(fnRowNo).Item("nUnitPrce") = lnNewPrice

        Call computeTotal()

        Return True
    End Function

    Public Function ChargeOrder() As Boolean
        Dim lsSQL As String
        ' TODO: put charge invoice


        p_oApp.BeginTransaction()
        lsSQL = "UPDATE " & pxeMasTable & _
                " SET cTranStat = " & strParm(xeTranStat.TRANS_CLOSED) & _
                " WHERE sTransnox = " & strParm(p_oDTMaster(0).Item("sTransNox"))
        p_oApp.CommitTransaction()
        Return True
    End Function

    Public Function AddItem(ByVal fnRowNo As Integer) As Boolean
        If p_bExisting Then
            ' new transaction
            If fnRowNo > p_oDTDetail.Rows.Count - 1 Then
                Return False
            ElseIf p_oDTDetail(fnRowNo).Item("cPrintedx") = xeLogical.YES Then
                Return AddOrder(p_oDTDetail(fnRowNo).Item("sBarcodex"), p_nQuantity)
            ElseIf p_oDTDetail(fnRowNo).Item("cDetSaved") = xeLogical.YES Then
                Return addQty(fnRowNo)
            Else
                p_oDTDetail(fnRowNo).Item("nQuantity") += p_nQuantity '1
            End If
        Else
            If p_oDTDetail(fnRowNo).Item("cDetSaved") = xeLogical.YES Then
                Return addQty(fnRowNo)
            Else
                p_oDTDetail(fnRowNo).Item("nQuantity") += p_nQuantity '1
            End If
        End If

        Call computeTotal()
        Return True
    End Function

    Public Function DeductItem(ByVal fnRowNo As Integer) As Boolean
        If p_bExisting Then
            ' new transaction
            If fnRowNo > p_oDTDetail.Rows.Count - 1 Then
                Return False
            ElseIf p_oDTDetail(fnRowNo).Item("cPrintedx") = xeLogical.YES Then
                Call ReverseOrder(p_oDTDetail(fnRowNo).Item("sBarcodex"), p_nQuantity)
            ElseIf p_oDTDetail(fnRowNo).Item("cDetSaved") = xeLogical.YES Then
                Return deductQty(fnRowNo)
            Else
                p_oDTDetail(fnRowNo).Item("nQuantity") -= p_nQuantity
            End If
        Else
            If p_oDTDetail(fnRowNo).Item("cDetSaved") = xeLogical.YES Then
                Return deductQty(fnRowNo)
            Else
                p_oDTDetail(fnRowNo).Item("nQuantity") -= p_nQuantity
            End If
        End If

        Call computeTotal()
        Return True
    End Function

    Private Function saveDetail(ByVal fnRow As Integer) As Boolean
        Dim lsSQL As String

        If p_oDTMaster(0).Item("sTransNox") = "" Then
            p_oDTMaster(0).Item("sTransNox") = getNextTransNo()
            saveMaster()
            p_bExisting = True
        End If

        lsSQL = "INSERT INTO " & pxeDetTable & _
                " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                    ", nEntryNox = " & p_oDTDetail(fnRow).Item("nEntryNox") & _
                    ", sStockIDx = " & strParm(p_oDTDetail(fnRow).Item("sStockIDx")) & _
                    ", cReversex = " & strParm(p_oDTDetail(fnRow).Item("cReversex")) & _
                    ", nQuantity = " & p_oDTDetail(fnRow).Item("nQuantity") & _
                    ", nUnitPrce = " & p_oDTDetail(fnRow).Item("nUnitPrce") & _
                    ", nDiscount = " & p_oDTDetail(fnRow).Item("nDiscount") & _
                    ", nAddDiscx = " & p_oDTDetail(fnRow).Item("nAddDiscx") & _
                    ", cPrintedx = " & p_oDTDetail(fnRow).Item("cPrintedx") & _
                    ", cComplmnt = " & p_oDTDetail(fnRow).Item("cComplmnt") & _
                    ", dModified = " & dateParm(p_oDTDetail(fnRow).Item("dModified"))

        Try
            p_oApp.Execute(lsSQL, pxeDetTable)
        Catch ex As Exception
            MsgBox(ex.Message)
            Throw ex
        End Try

        p_oDTDetail(fnRow).Item("cDetSaved") = 1

        Return True
    End Function

    Private Function addQty(ByVal fnRowNo As Integer) As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim loDT As DataTable

        Try
            If p_sParent = "" Then p_oApp.BeginTransaction()

            lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo

            loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

            If loDT.Rows.Count <> 0 Then
                lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

                loDT = p_oApp.ExecuteQuery(lsSQL)

                For lnRow = 1 To loDT.Rows.Count
                    lsSQL = "UPDATE " & pxeDetTable & _
                            " SET nQuantity = nQuantity + " & p_nQuantity & _
                            " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                                " AND nEntryNox = " & fnRowNo + lnRow

                    'update detail
                    p_oApp.Execute(lsSQL, pxeDetTable)
                    p_oDTDetail(fnRowNo + lnRow).Item("nQuantity") += p_nQuantity
                Next
            End If

            lsSQL = "UPDATE " & pxeDetTable & _
                    " SET nQuantity = nQuantity + " & p_nQuantity & _
                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                        " AND nEntryNox = " & fnRowNo

            p_oApp.Execute(lsSQL, pxeDetTable)
        Catch ex As Exception
            If p_sParent = "" Then p_oApp.RollBackTransaction()
            Return False
        Finally
            If p_sParent = "" Then p_oApp.CommitTransaction()
        End Try

        p_oDTDetail(fnRowNo).Item("nQuantity") += p_nQuantity

        Call computeTotal()
        Return True
    End Function

    Private Function deductQty(ByVal fnRowNo As Integer) As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim loDT As DataTable

        If p_oDTDetail(fnRowNo).Item("nQuantity") = 1 Then
            Try
                If p_sParent = "" Then p_oApp.BeginTransaction()

                lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo
                loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

                If loDT.Rows.Count <> 0 Then
                    lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

                    loDT = p_oApp.ExecuteQuery(lsSQL)

                    For lnRow = loDT.Rows.Count To 1 Step -1
                        lsSQL = "DELETE FROM " & pxeDetTable & _
                                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                                    " AND nEntryNox = " & fnRowNo + lnRow

                        'delete detail
                        p_oApp.Execute(lsSQL, pxeDetTable)
                        p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo + lnRow))
                    Next
                End If

                lsSQL = "DELETE FROM " & pxeDetTable & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo

                'delete combo item
                p_oApp.Execute(lsSQL, pxeDetTable)
                p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo))

                'update entry no
                If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
                    lsSQL = "UPDATE " & pxeDetTable & _
                            " SET nEntryNox = nEntryNox - 1" & _
                            " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                                " AND nEntryNox > " & fnRowNo

                    p_oApp.Execute(lsSQL, pxeDetTable)
                End If
            Catch ex As Exception
                If p_sParent = "" Then p_oApp.RollBackTransaction()
                Return False
            Finally
                If p_sParent = "" Then p_oApp.CommitTransaction()
            End Try
        Else
            Try
                If p_sParent = "" Then p_oApp.BeginTransaction()

                lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo

                loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

                If loDT.Rows.Count <> 0 Then
                    lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

                    loDT = p_oApp.ExecuteQuery(lsSQL)

                    For lnRow = 1 To loDT.Rows.Count
                        lsSQL = "UPDATE " & pxeDetTable & _
                                " SET nQuantity = nQuantity - " & p_nQuantity & _
                                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                                    " AND nEntryNox = " & fnRowNo + lnRow

                        p_oApp.Execute(lsSQL, pxeDetTable) 'update detail

                        p_oDTDetail(fnRowNo + lnRow).Item("nQuantity") -= p_nQuantity
                    Next
                End If

                lsSQL = "UPDATE " & pxeDetTable & _
                        " SET nQuantity = nQuantity -" & p_nQuantity & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo

                p_oApp.Execute(lsSQL, pxeDetTable)
            Catch ex As Exception
                If p_sParent = "" Then p_oApp.RollBackTransaction()
                Return False
            Finally
                If p_sParent = "" Then p_oApp.CommitTransaction()
            End Try

            p_oDTDetail(fnRowNo).Item("nQuantity") -= p_nQuantity
        End If

        Call computeTotal()
        Return True
    End Function

    Private Function updateQty(fnRowNo As Integer) As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim loDT As DataTable

        Try
            If p_sParent = "" Then p_oApp.BeginTransaction()

            If p_nQuantity = 0 Then
                lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo
                loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

                If loDT.Rows.Count <> 0 Then
                    lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

                    loDT = p_oApp.ExecuteQuery(lsSQL)

                    For lnRow = loDT.Rows.Count To 1 Step -1
                        lsSQL = "DELETE FROM " & pxeDetTable & _
                                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                                    " AND nEntryNox = " & fnRowNo + lnRow

                        'delete detail
                        p_oApp.Execute(lsSQL, pxeDetTable)
                        p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo + lnRow))
                    Next
                End If

                lsSQL = "DELETE FROM " & pxeDetTable & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo

                'delete combo item
                p_oApp.Execute(lsSQL, pxeDetTable)
                p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo))

                'update entry no
                If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
                    lsSQL = "UPDATE " & pxeDetTable & _
                            " SET nEntryNox = nEntryNox - 1" & _
                            " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                                " AND nEntryNox > " & fnRowNo

                    p_oApp.Execute(lsSQL, pxeDetTable)
                End If
            Else
                lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo

                loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

                If loDT.Rows.Count <> 0 Then
                    lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

                    loDT = p_oApp.ExecuteQuery(lsSQL)

                    For lnRow = 1 To loDT.Rows.Count
                        lsSQL = "UPDATE " & pxeDetTable & _
                                " SET nQuantity = " & p_nQuantity & _
                                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                                    " AND nEntryNox = " & fnRowNo + lnRow

                        'update detail
                        p_oApp.Execute(lsSQL, pxeDetTable)
                        p_oDTDetail(fnRowNo + lnRow).Item("nQuantity") = p_nQuantity
                    Next
                End If

                lsSQL = "UPDATE " & pxeDetTable & _
                        " SET nQuantity = " & p_nQuantity & _
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                            " AND nEntryNox = " & fnRowNo

                p_oApp.Execute(lsSQL, pxeDetTable)

                p_oDTDetail(fnRowNo).Item("nQuantity") = p_nQuantity
            End If
        Catch ex As Exception
            If p_sParent = "" Then p_oApp.RollBackTransaction()
            Return False
        Finally
            If p_sParent = "" Then p_oApp.CommitTransaction()
        End Try

        Call computeTotal()

        Return True
    End Function

    Private Function saveMaster() As Boolean
        Dim lsSQL As String

        lsSQL = "INSERT INTO " & pxeMasTable & _
                " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                    ", dTransact = " & dateParm(p_oDTMaster(0).Item("dTransact")) & _
                    ", nContrlNo = " & getNextControl() & _
                    ", nTranTotl = " & p_oDTMaster(0).Item("nTranTotl") & _
                    ", sCashierx = " & strParm(p_oApp.UserID) & _
                    ", sWaiterID = " & strParm(p_oDTMaster(0).Item("sWaiterID")) & _
                    ", sTableNox = " & strParm(p_oDTMaster(0).Item("sTableNox")) & _
                    ", cTranStat = " & p_oDTMaster(0).Item("cTranStat") & _
                    ", sModified = " & strParm(p_oApp.UserID) & _
                    ", dModified = " & dateParm(p_oApp.getSysDate)

        p_oApp.Execute(lsSQL, pxeMasTable)

        Return True
    End Function

    Private Function computeTotal() As Boolean
        Dim lnRow As Integer
        Dim lnTotal As Double = 0.0

        For lnRow = 0 To p_oDTDetail.Rows.Count - 1
            lnTotal += (p_oDTDetail(lnRow).Item("nUnitPrce") * _
                        (100 - p_oDTDetail(lnRow).Item("nDiscount")) / 100 -
                        p_oDTDetail(lnRow).Item("nAddDiscx")) * _
                    IIf(p_oDTDetail(lnRow).Item("cReversex") = "+", 1, -1) * _
                    p_oDTDetail(lnRow).Item("nQuantity")

        Next
        p_oDTMaster(0).Item("nTranTotl") = lnTotal

        Return True
    End Function

    Private Function createMaster() As Boolean
        p_oDTMaster = New DataTable("Master")
        p_oDTMaster.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 14
        p_oDTMaster.Columns.Add("dTransact", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("nContrlNo", System.Type.GetType("System.Int32"))
        p_oDTMaster.Columns.Add("sIDNumber", System.Type.GetType("System.Int32"))
        p_oDTMaster.Columns.Add("nTranTotl", System.Type.GetType("System.Decimal"))
        p_oDTMaster.Columns.Add("sCashrNme", System.Type.GetType("System.String")).MaxLength = 64
        p_oDTMaster.Columns.Add("sWaiterNm", System.Type.GetType("System.String")).MaxLength = 64
        p_oDTMaster.Columns.Add("sCashierx", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("sWaiterID", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("sReceiptx", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("sMergeIDx", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("sTableNox", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTMaster.Columns.Add("cTranStat", System.Type.GetType("System.String")).MaxLength = 1

        Return True
    End Function

    Private Function initMaster() As Boolean
        p_oDTMaster.Rows.Add()
        p_oDTMaster(0).Item("sTransNox") = ""
        p_oDTMaster(0).Item("dTransact") = Now
        p_oDTMaster(0).Item("nContrlNo") = 0
        p_oDTMaster(0).Item("sIDNumber") = 0
        p_oDTMaster(0).Item("nTranTotl") = 0.0
        p_oDTMaster(0).Item("sCashrNme") = "To Follow"
        p_oDTMaster(0).Item("sWaiterNm") = ""
        p_oDTMaster(0).Item("sCashierx") = p_oApp.UserID
        p_oDTMaster(0).Item("sWaiterID") = ""
        p_oDTMaster(0).Item("sReceiptx") = ""
        p_oDTMaster(0).Item("sMergeIDx") = ""
        p_oDTMaster(0).Item("sTableNox") = ""
        p_oDTMaster(0).Item("cTranStat") = "0"

        Return True
    End Function

    Private Function createDetail() As Boolean
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nEntryNox", System.Type.GetType("System.Int16")).AutoIncrement = True
        p_oDTDetail.Columns.Add("sBarcodex", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("sDescript", System.Type.GetType("System.String")).MaxLength = 64
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 14
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cReversex", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int32"))
        p_oDTDetail.Columns.Add("nDiscount", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nAddDiscx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cPrintedx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("cComplmnt", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("nDiscLev1", System.Type.GetType("System.Decimal")) ' the following field will be only used for 
        p_oDTDetail.Columns.Add("nDiscLev2", System.Type.GetType("System.Decimal")) ' validating discount given to customer
        p_oDTDetail.Columns.Add("nDiscLev3", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nDiscAmtx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nDealrDsc", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cWthPromo", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("cComboMlx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("cDetailxx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("sStockIDx", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("sCategrID", System.Type.GetType("System.String")).MaxLength = 4
        p_oDTDetail.Columns.Add("cForwardx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("cDetSaved", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("cReversed", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("dModified", System.Type.GetType("System.DateTime"))

        Return True
    End Function

    Private Function newDetail() As Boolean
        p_oDTDetail.Rows.Add(p_oDTDetail.NewRow)
        p_nRow = p_oDTDetail.Rows.Count - 1

        p_oDTDetail(p_nRow).Item("sBarcodex") = ""
        p_oDTDetail(p_nRow).Item("sDescript") = ""
        p_oDTDetail(p_nRow).Item("sBriefDsc") = ""
        p_oDTDetail(p_nRow).Item("nUnitPrce") = 0.0
        p_oDTDetail(p_nRow).Item("cReversex") = "+"
        p_oDTDetail(p_nRow).Item("nQuantity") = 0
        p_oDTDetail(p_nRow).Item("nDiscount") = 0.0
        p_oDTDetail(p_nRow).Item("nAddDiscx") = 0.0
        p_oDTDetail(p_nRow).Item("nDiscLev1") = 0.0
        p_oDTDetail(p_nRow).Item("nDiscLev2") = 0.0
        p_oDTDetail(p_nRow).Item("nDiscLev3") = 0.0
        p_oDTDetail(p_nRow).Item("nDiscAmtx") = 0.0
        p_oDTDetail(p_nRow).Item("nDealrDsc") = 0.0
        p_oDTDetail(p_nRow).Item("cComboMlx") = 0
        p_oDTDetail(p_nRow).Item("cDetailxx") = 0
        p_oDTDetail(p_nRow).Item("cWthPromo") = 0
        p_oDTDetail(p_nRow).Item("sStockIDx") = ""
        p_oDTDetail(p_nRow).Item("sCategrID") = ""
        p_oDTDetail(p_nRow).Item("cPrintedx") = 0
        p_oDTDetail(p_nRow).Item("cForwardx") = 0
        p_oDTDetail(p_nRow).Item("cComplmnt") = 0
        p_oDTDetail(p_nRow).Item("cDetSaved") = 0
        p_oDTDetail(p_nRow).Item("cReversed") = "0"

        Return True
    End Function

    Private Function newDetail(ByVal foDT As DataTable) As Boolean
        Call newDetail()

        p_oDTDetail(p_nRow).Item("sBarcodex") = foDT(0).Item("sBarcodex")
        p_oDTDetail(p_nRow).Item("sDescript") = foDT(0).Item("sDescript")
        p_oDTDetail(p_nRow).Item("sBriefDsc") = foDT(0).Item("sBriefDsc")
        p_oDTDetail(p_nRow).Item("nUnitPrce") = foDT(0).Item("nUnitPrce")
        p_oDTDetail(p_nRow).Item("sStockIDx") = foDT(0).Item("sStockIDx")
        p_oDTDetail(p_nRow).Item("nDiscLev1") = foDT(0).Item("nDiscLev1")
        p_oDTDetail(p_nRow).Item("nDiscLev2") = foDT(0).Item("nDiscLev2")
        p_oDTDetail(p_nRow).Item("nDiscLev3") = foDT(0).Item("nDiscLev3")
        p_oDTDetail(p_nRow).Item("nDiscAmtx") = foDT(0).Item("nDiscAmtx")
        p_oDTDetail(p_nRow).Item("nDealrDsc") = foDT(0).Item("nDealrDsc")
        p_oDTDetail(p_nRow).Item("cComboMlx") = foDT(0).Item("cComboMlx")
        p_oDTDetail(p_nRow).Item("cWthPromo") = foDT(0).Item("cWthPromo")
        p_oDTDetail(p_nRow).Item("sCategrID") = foDT(0).Item("sCategrID")
        p_oDTDetail(p_nRow).Item("dModified") = Now()

        Return True
    End Function

    Private Function procPromo() As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = getSQ_Promo(p_oDTDetail(p_nRow).Item("sStockIDx"), p_oDTDetail(p_nRow).Item("sCategrID"))
        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            Return True
        End If

        ' check first the quantity 
        If p_oDTDetail(p_nRow).Item("nQuantity") < loDT(0).Item("nMinQtyxx") Then
            ' sales did not reach the minimum quantity
            If p_bNotify Then
                If MsgBox("Item is on Promo." & vbCrLf & _
                        "However the minimum quantity of " & loDT(0).Item("nMinQtyxx") & " is not reach!" & vbCrLf & _
                        "Discount is " & Format(loDT(0).Item("nDiscRate"), "#0.00") & "% and/or " & Format(loDT(0).Item("nDiscAmtx"), "#,##0.00") & "PHP" & vbCrLf & _
                        "Would you like to avail the discount?", vbCritical + vbYesNo) <> MsgBoxResult.Yes Then
                    Return True
                End If

                ' customer aggreed to avail promo
                p_oDTDetail(p_nRow).Item("nQuantity") = loDT(0).Item("nMinQtyxx")
            Else
                Return True
            End If
        End If

        ' assign discounts
        p_oDTDetail(p_nRow).Item("nDiscount") = loDT(0).Item("nDiscRate")
        p_oDTDetail(p_nRow).Item("nAddDiscx") = loDT(0).Item("nDiscAmtx")
        p_oDTDetail(p_nRow).Item("nDiscLev1") = 0 ' remove any discount coz promo items overrides all regulary discounting
        p_oDTDetail(p_nRow).Item("nDiscLev2") = 0
        p_oDTDetail(p_nRow).Item("nDiscLev3") = 0
        p_oDTDetail(p_nRow).Item("nDiscAmtx") = 0
        p_oDTDetail(p_nRow).Item("nDealrDsc") = 0

        If loDT(0).Item("nExtQtyxx") > 0 Then
            If loDT(0).Item("nExtQtyxx") <= p_oDTDetail(p_nRow).Item("nQuantity") - loDT(0).Item("nBaseQtyx") Then
                ' promo is extended to additional purchase
                Dim loDT2 As New DataTable

                ' copy the current row as new row
                loDT2 = p_oDTDetail.Clone
                loDT2.ImportRow(p_oDTDetail(p_nRow))

                Call newDetail(loDT2)

                ' set the quantity to the base qty
                p_oDTDetail(p_nRow - 1).Item("nQuantity") = loDT(0).Item("nBaseQtyx")

                ' now assign the discount for the extended
                p_oDTDetail(p_nRow).Item("nQuantity") -= loDT(0).Item("nBaseQtyx")
                p_oDTDetail(p_nRow).Item("nDiscount") = loDT(0).Item("nExtDRate")
                p_oDTDetail(p_nRow).Item("nAddDiscx") = loDT(0).Item("nExtDAmtx")
            End If
        End If

        Return True
    End Function

    Private Function procCombo() As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lnRow As Integer

        lsSQL = getSQ_Combo(p_oDTDetail(p_nRow).Item("sStockIDx"))
        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            Return True
        End If

        ' add the detail in the order
        For lnRow = 0 To loDT.Rows.Count - 1
            Call newDetail()

            ' assign item details
            p_oDTDetail(p_nRow).Item("sStockIDx") = loDT(lnRow).Item("sStockIDx")
            p_oDTDetail(p_nRow).Item("sBarcodex") = loDT(lnRow).Item("sBarcodex")
            p_oDTDetail(p_nRow).Item("sDescript") = loDT(lnRow).Item("sDescript")
            p_oDTDetail(p_nRow).Item("sBriefDsc") = loDT(lnRow).Item("sBriefDsc")
            p_oDTDetail(p_nRow).Item("nQuantity") = p_nQuantity 'loDT(lnRow).Item("nQuantity")
            p_oDTDetail(p_nRow).Item("dModified") = Now
            p_oDTDetail(p_nRow).Item("cDetailxx") = 1
        Next

        Return True
    End Function

    Public Function GetTables() As DataTable
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = "SELECT nTableNox" & _
                    ", cStatusxx" & _
                    ", dReserved" & _
                    ", nCapacity" & _
                    ", nOccupnts" & _
                " FROM Table_Master"

        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then MsgBox("No tables on record!", MsgBoxStyle.Critical, "Warning")

        Return loDT
    End Function

    'This method implements a search master where id and desc are not joined.
    Private Sub getClient(ByVal fnColIdx As Integer _
                          , ByVal fnColDsc As Integer _
                          , ByVal fsValue As String _
                          , ByVal fbIsCode As Boolean _
                          , ByVal fbIsSrch As Boolean)

        'Compare the value to be search against the value in our column
        If fbIsCode Then
            If fsValue = p_oDTMaster(0).Item(fnColIdx) And fsValue <> "" And p_oDTMaster(0).Item(fnColDsc) <> "" Then Exit Sub
        Else
            If fsValue = p_oDTMaster(0).Item(fnColDsc) And fsValue <> "" Then Exit Sub
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" & _
                       "  a.sClientID" & _
                       ", a.sCompnyNm" & _
                       ", CONCAT(IF(IFNull(a.sHouseNox, '') = '', '', CONCAT(a.sHouseNox, ' ')), a.sAddressx, ', ', b.sTownName, ', ', c.sProvName, ' ', b.sZippCode) xAddressx " & _
               " FROM `Client_Master` a" & _
                " LEFT JOIN TownCity b ON a.sTownIDxx = b.sTownIDxx" & _
                " LEFT JOIN Province c ON b.sProvIDxx = c.sProvIDxx" & _
        IIf(fbIsCode = False, " WHERE a.cRecdStat = '1'", "")

        'Are we using like comparison or equality comparison
        If fbIsSrch Then
            Dim loRow As DataRow = KwikSearch(p_oApp _
                                             , lsSQL _
                                             , True _
                                             , fsValue _
                                             , "sClientID»sCompnyNm»xAddressx" _
                                             , "ID»Client Name»Address", _
                                             , "a.sClientID»a.sCompnyNm»CONCAT(IF(IFNull(a.sHouseNox, '') = '', '', CONCAT(a.sHouseNox, ' ')), a.sAddressx, ', ', b.sTownName, ', ', c.sProvName, ' ', b.sZippCode)" _
                                             , IIf(fbIsCode, 0, 1))
            If IsNothing(loRow) Then
                p_oDTMaster(0).Item(fnColIdx) = ""
                p_oDTMaster(0).Item(fnColDsc) = ""
                'p_oOthersx.sAddressX = ""
            Else
                p_oDTMaster(0).Item(fnColIdx) = loRow.Item("sClientID")
                p_oDTMaster(0).Item(fnColDsc) = loRow.Item("sClientNm")
                'p_oOthersx.sAddressX = loRow.Item("xAddressX")
            End If

            'RaiseEvent MasterRetrieved(fnColDsc, p_oOthersx.sClientNm)
            Exit Sub
        End If

        If fsValue <> "" Then
            If fbIsCode Then
                lsSQL = AddCondition(lsSQL, "a.sClientID = " & strParm(fsValue))
            Else
                lsSQL = AddCondition(lsSQL, "a.sClientNm = " & strParm(fsValue))
            End If
        End If

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            p_oDTMaster(0).Item(fnColIdx) = ""
            p_oDTMaster(0).Item(fnColDsc) = ""
            'p_oOthersx.sAddressX = ""
        ElseIf loDta.Rows.Count = 1 Then
            p_oDTMaster(0).Item(fnColIdx) = loDta(0).Item("sClientID")
            p_oDTMaster(0).Item(fnColDsc) = loDta(0).Item("sClientNm")
            'p_oOthersx.sAddressX = loDta(0).Item("xAddressX")
        End If

        'RaiseEvent MasterRetrieved(fnColDsc, p_oOthersx.sClientNm)
    End Sub

    Private Function isEntryOk() As Boolean
        'We are not filtering any
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
                " FROM " & pxeMasTable & _
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oApp.getSysDate(), "yy") & "%") & _
                " ORDER BY sTransNox DESC" & _
                " LIMIT 1"

        Try
            loDA.SelectCommand = New MySqlCommand(lsSQL, p_oApp.Connection)
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

            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)

            lsSQL = loDT.Rows(0).Item("sTransNox")
            lnLen = Len(lsSQL)

            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sPOSNo & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function

    Private Function getNextControl() As String
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = "SELECT nContrlNo" & _
                " FROM " & pxeMasTable & _
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oApp.getSysDate().ToString("yy") & "%")) & _
                    " AND dTransact LIKE " & strParm(Format(p_oApp.getSysDate().ToString("yyyy-MM") & "%")) & _
                " ORDER BY nContrlNo DESC" & _
                " LIMIT 1"

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            Return 1
        Else
            Return loDT(0).Item("nContrlNo") + 1
        End If
    End Function

    Private Function getNextMergeID() As String
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lnCtr As Integer

        lsSQL = "SELECT sMergeIDx" & _
                " FROM " & pxeMasTable & _
                " WHERE sMergeIDx LIKE " & strParm(p_sBranchCd & Format(p_oApp.getSysDate(), "yy") & "%") & _
                " ORDER BY sMergeIDx DESC" & _
                " LIMIT 1"

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnCtr = 1
        Else
            lnCtr = Mid(loDT(0).Item(0), Len(p_sBranchCd & "yy") + 1)
        End If

        Return p_sBranchCd & Format(p_oApp.getSysDate(), "yy") & Format(lnCtr, pxeCtrFormat)
    End Function

    'Returns the SQL for the Management of the Master Part of the Transaction
    Private Function getSQ_Order() As String
        Return _
            "SELECT sTransNox" & _
                ", dTransact" & _
                ", nContrlNo" & _
                ", sReceiptx" & _
                ", nTranTotl" & _
                ", sCashierx" & _
                ", sTableNox" & _
                ", sWaiterID" & _
                ", sMergeIDx" & _
                ", cTranStat" & _
                " FROM " & pxeMasTable & _
                " WHERE cTranStat = " & strParm(xeTranStat.TRANS_OPEN)
    End Function

    'Returns the SQL for the Management of the Master Part of the Transaction
    Private Function getSQ_Detail() As String
        Return _
            "SELECT b.sBarcodex" & _
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
                " FROM " & pxeDetTable & " a" & _
                ", Inventory b" & _
                " WHERE a.sStockIDx = b.sStockIDx" & _
                " ORDER BY a.nEntryNox"
    End Function

    Private Function getSQ_Search() As String
        'Kalyptus - 2016.11.04 09:33am
        'Added the fields cWthPromo and cComboMlx to the SQL statement

        Return _
            "SELECT sBarcodex" & _
                ", sDescript" & _
                ", nUnitPrce" & _
                ", cWthPromo" & _
                ", cComboMlx" & _
                ", sBriefDsc" & _
                ", sStockIDx" & _
                ", nDiscLev1" & _
                ", nDiscLev2" & _
                ", nDiscLev3" & _
                ", nDealrDsc" & _
                ", sCategrID" & _
                ", 0 nDiscAmtx" & _
            " FROM Inventory" & _
            " WHERE sInvTypID = " & strParm(pxeFinGoods)
    End Function

    Private Function getSQ_Promo(ByVal fsStockIDx As String, ByVal fsCategrID As String) As String
        Dim lsSQL As String

        lsSQL = _
            "SELECT a.sTransNox" & _
                ", a.nDiscRate" & _
                ", a.nDiscAmtx" & _
                ", a.nMinQtyxx" & _
                ", a.nExtDRate" & _
                ", a.nExtDAmtx" & _
                ", a.nExtQtyxx" & _
                ", a.dHappyHrF" & _
                ", a.dHappyHrT" & _
                ", a.dPromoFrm" & _
                ", a.dPromoTru" & _
            " FROM Sales_Promo a"

        If fsCategrID = vbEmpty Then
            lsSQL = lsSQL & _
                " WHERE a.sStockIDx = " & strParm(fsStockIDx)
        Else
            lsSQL = lsSQL & _
                " WHERE ( a.sStockIDx = " & strParm(fsStockIDx) & _
                    " OR a.sCategrID = " & strParm(fsCategrID) & " )"
        End If

        lsSQL = lsSQL & _
                    " AND SYSDATE() BETWEEN a.dPromoFrm AND a.dPromoTru" & _
                    " AND ( a.tHappyHrF IS NULL" & _
                        " OR TIME(SYSDATE()) BETWEEN a.tHappyHrF AND a.tHappyHrT )" & _
                    " AND a.cRecdStat = " & xeRecordStat.RECORD_NEW & _
                " ORDER BY a.dPromoFrm DESC LIMIT 1"
        Return lsSQL
    End Function

    Private Function getSQ_Combo(ByVal fsStockIDx As String) As String
        Return _
            "SELECT a.sComboIDx" & _
                ", b.sBarcodex" & _
                ", b.sDescript" & _
                ", a.nQuantity" & _
                ", b.sCategrID" & _
                ", b.sStockIDx" & _
                ", b.sBriefDsc" & _
            " FROM Combo_Meals a" & _
                ", Inventory b" & _
            " WHERE a.sStockIDx = b.sStockIDx" & _
                " AND a.sComboIDx = " & strParm(fsStockIDx) & _
            " ORDER BY a.nEntryNox"
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTMaster = Nothing
        p_oDTDetail = Nothing
        p_bShowMsg = False

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")
        p_sAccreditNo = "To Follow"
        p_sBranchCd = p_oApp.BranchCode

        Call createMaster()
        Call initMaster()

        Call createDetail()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class