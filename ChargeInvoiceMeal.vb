'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Charge Invoice Transaction Object
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
'  Kalyptus [ 11/04/2016 02:15 am ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports MySql.Data.MySqlClient
Imports ggcAppDriver
Imports ggcRetailParams
Imports ggcReceipt
Imports ADODB

Public Class ChargeInvoiceMeal
    Private Const pxeMasterTble As String = "Employee_Meal_Master"
    Private Const pxeDetailTble As String = "Employee_Meal_Detail"
    Private Const pxeSummaryTble As String = "Employee_Meal_Summary"
    Private Const xsSignature As String = "08220326"

    Private p_oApp As GRider
    Private p_oDTMstr As DataTable

    Private p_oDTDetail As DataTable
    Private p_oOthersx As New Others

    Private p_oFormChargeInfo As frmQRResult
    Private p_nEditMode As xeEditMode

    Private p_sParent As String
    Private p_sPOSNo As String
    Private p_sBranchCd As String
    Private p_nSales As Decimal
    Private p_nDiscount As Decimal
    Private p_bCancelled As Boolean
    Private p_sCRMNmbr As String

    Private p_sVATReg As String
    Private p_sTermnl As String
    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_dPTUFrm As Date
    Private p_dPTUTru As Date
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_dAccFrm As Date
    Private p_dAccTru As Date
    Private p_cTrnMde As Char
    Private p_sACCNox As String
    Private p_sPTUNox As String


    Private p_sContrlNo As String

    Private p_oDtaOrder As DataTable
    Private p_oDtaDiscx As DataTable

    Private p_nNonVATxx As Decimal
    Private p_nDiscAmtx As Decimal
    Private p_dTransact As DateTime

    Private p_sCashierx As String
    Private p_sLogName As String

    'jovan added this global variable
    Private p_nNoClient As Integer
    Private p_nWithDisc As Integer
    Private p_nTableNo As Integer

    'Transaction Master Info
    Private psCashrNme As String
    Private pdTransact As Date          'XXX
    Private pbHsParent As Boolean       'XXX
    Private pbHsSubsidy As Boolean       'XXX
    Private pbHsComboMeal As Boolean       'XXX

    Private p_sChargeInfo() As String

    Private Const p_sMasTable As String = "Employee_Meal_Master"

    Private Const p_sMsgHeadr As String = "Charge Invoice Meal Transaction"

    Public Event MasterRetrieved(ByVal Index As Integer,
                                  ByVal Value As Object)

    Public Property ChargeInformation As String()
        Set(value As String())
            p_sChargeInfo = value
        End Set
        Get
            Return p_sChargeInfo
        End Get
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
    WriteOnly Property TranMode As Char
        Set(ByVal Value As Char)
            p_cTrnMde = Value
        End Set
    End Property

    WriteOnly Property CRMNumbr As String
        Set(ByVal Value As String)
            p_sCRMNmbr = Value
        End Set
    End Property

    WriteOnly Property HasParent As Boolean
        Set(ByVal Value As Boolean)
            pbHsParent = Value
        End Set
    End Property
    Public Property HasSubsidy As Boolean
        Set(ByVal Value As Boolean)
            pbHsSubsidy = Value
        End Set
        Get
            Return pbHsSubsidy
        End Get
    End Property

    Public Property HasComboMeal As Boolean
        Set(ByVal Value As Boolean)
            pbHsComboMeal = Value
        End Set
        Get
            Return pbHsComboMeal
        End Get
    End Property

    Property ClientNo As Integer
        Get
            Return p_nNoClient
        End Get
        Set(ByVal Value As Integer)
            p_nNoClient = Value
        End Set
    End Property

    Property WithDisc As Integer
        Get
            Return p_nWithDisc
        End Get
        Set(ByVal Value As Integer)
            p_nWithDisc = Value
        End Set
    End Property

    Public Property TableNo() As Integer
        Get
            Return p_nTableNo
        End Get
        Set(ByVal value As Integer)
            p_nTableNo = value
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



    Public Property WithDiscount() As Integer
        Get
            Return p_nWithDisc
        End Get
        Set(ByVal value As Integer)
            p_nWithDisc = value
        End Set
    End Property

    Public WriteOnly Property Cashier() As String
        Set(ByVal value As String)
            p_sCashierx = value
        End Set
    End Property

    WriteOnly Property DateTransact() As Date
        Set(ByVal value As Date)
            p_dTransact = value
        End Set
    End Property

    WriteOnly Property ControlNo As String
        Set(ByVal Value As String)
            p_sContrlNo = Value
        End Set
    End Property

    WriteOnly Property SalesOrder() As DataTable
        Set(ByVal oData As DataTable)
            p_oDtaOrder = oData
        End Set
    End Property

    WriteOnly Property Discount() As DataTable
        Set(ByVal oData As DataTable)
            p_oDtaDiscx = oData
        End Set
    End Property

    WriteOnly Property DiscAmount() As Decimal
        Set(ByVal value As Decimal)
            p_nDiscAmtx = value
        End Set
    End Property

    WriteOnly Property NonVAT As Decimal
        Set(ByVal value As Decimal)
            p_nNonVATxx = value
        End Set
    End Property

    WriteOnly Property SalesTotal As Decimal
        Set(ByVal value As Decimal)
            p_nSales = value
        End Set
    End Property

    WriteOnly Property Discounts As Decimal
        Set(ByVal value As Decimal)
            p_nDiscount = value
        End Set
    End Property

    WriteOnly Property POSNo As String
        Set(ByVal value As String)
            p_sPOSNo = value
        End Set
    End Property

    Public ReadOnly Property AppDriver() As ggcAppDriver.GRider
        Get
            Return p_oApp
        End Get
    End Property

    Public ReadOnly Property Cancelled As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    Public Property Master(ByVal Index As Integer) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    'Case 80
                    '    'If Trim(IFNull(p_oDTMstr(0).Item(1))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                    '    '    getClient(1, 80, p_oDTMstr(0).Item(1), True, False)
                    '    'End If
                    '    Return p_oOthersx.sClientNm
                    'Case 81
                    '    'If Trim(IFNull(p_oDTMstr(0).Item(1))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                    '    '    getClient(1, 80, p_oDTMstr(0).Item(1), True, False)
                    '    'End If
                    '    Return p_oOthersx.sAddressX
                    Case Else
                        Return p_oDTMstr(0).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    'Case 80         'sClientNm
                    '    'Call getClient(1, 80, value, False, False)
                    '    p_oOthersx.sClientNm = value
                    'Case 81         'xAddressx
                    '    p_oOthersx.sAddressX = value
                    'Case 9, 10
                    '    p_oDTMstr(0).Item(Index) = value
                    'Case 0, 2 To 8 'All fiels except sClientNm and sClientID
                    Case Else
                        p_oDTMstr(0).Item(Index) = value
                End Select
            End If
        End Set
    End Property

    'Property Master(String)
    Public Property Master(ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    'Case "sclientnm"
                    '    'If Trim(IFNull(p_oDTMstr(0).Item(1))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                    '    '    getClient(1, 80, p_oDTMstr(0).Item(1), True, False)
                    '    'End If
                    '    Return p_oOthersx.sClientNm
                    'Case "xaddressx"
                    '    'If Trim(IFNull(p_oDTMstr(0).Item(1))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                    '    '    getClient(1, 80, p_oDTMstr(0).Item(1), True, False)
                    '    'End If
                    '    Return p_oOthersx.sAddressX
                    Case Else
                        Return p_oDTMstr(0).Item(Index)
                End Select
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case LCase(Index)
                    'Case "sclientnm"
                    '    'Call getClient(1, 80, value, False, False)
                    '    p_oOthersx.sClientNm = value
                    'Case "xaddressx"
                    '    p_oOthersx.sAddressX = value
                    Case Else
                        Master(p_oDTMstr.Columns(Index).Ordinal) = value
                End Select
            End If
        End Set
    End Property

    'Property EditMode()
    Public ReadOnly Property EditMode() As xeEditMode
        Get
            Return p_nEditMode
        End Get
    End Property

    Public Property Parent() As String
        Get
            Return p_sParent
        End Get
        Set(ByVal value As String)
            p_sParent = value
        End Set
    End Property

    'Public Function NewTransaction()
    Public Function NewTransaction() As Boolean
        Dim lsSQL As String


        lsSQL = AddCondition(getSQ_Master, "0=1")
        p_oDTMstr = p_oApp.ExecuteQuery(lsSQL)
        p_oDTMstr.Rows.Add(p_oDTMstr.NewRow())
        Call initMaster()
        Call initOthers()


        lsSQL = AddCondition(getSQ_Detail, "0=1")
        p_oDTDetail = p_oApp.ExecuteQuery(lsSQL)

        Call initMaster()
        Call procDetail()

        p_nEditMode = xeEditMode.MODE_ADDNEW

        Return True
    End Function


    Private Sub procDetail()

        With p_oDTDetail
            For nCtr As Integer = 0 To p_oDtaOrder.Rows.Count - 1
                .Rows.Add()
                .Rows(nCtr)("sTransNox") = p_oDTMstr.Rows(0)("sTransNox")
                .Rows(nCtr)("nEntryNox") = nCtr + 1
                .Rows(nCtr)("sItemCode") = p_oDtaOrder.Rows(nCtr)("sBarcodex")
                .Rows(nCtr)("sItemDesc") = p_oDtaOrder.Rows(nCtr)("sDescript")
                .Rows(nCtr)("nUnitPrce") = p_oDtaOrder.Rows(nCtr)("nUnitPrce")
                .Rows(nCtr)("nQuantity") = p_oDtaOrder.Rows(nCtr)("nQuantity")
                .Rows(nCtr)("nDiscRate") = p_oDtaOrder.Rows(nCtr)("nDiscount")
                .Rows(nCtr)("nDiscAmnt") = p_oDtaOrder.Rows(nCtr)("nAddDiscx")
                .Rows(nCtr)("cReversex") = p_oDtaOrder.Rows(nCtr)("cReversex")
            Next nCtr
        End With
    End Sub

    'Public Function OpenTransaction(String)
    Public Function OpenTransaction(ByVal fsRecdIDxx As String) As Boolean
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Master, "a.sTransNox = " & strParm(fsRecdIDxx))
        p_oDTMstr = p_oApp.ExecuteQuery(lsSQL)

        If p_oDTMstr.Rows.Count <= 0 Then
            p_nEditMode = xeEditMode.MODE_UNKNOWN
            Return False
        End If

        Call initOthers()

        p_nEditMode = xeEditMode.MODE_READY
        Return True
    End Function


    Public Sub SearchMaster(ByVal fnIndex As Integer, ByVal fsValue As String)
        Select Case fnIndex
            Case 80
                Call getClient(1, 80, fsValue, False, True)
        End Select
    End Sub

    'Public Function SearchTransaction(String, Boolean, Boolean=False)
    Public Function SearchTransaction(
                        ByVal fsValue As String _
                      , Optional ByVal fbByCode As Boolean = False) As Boolean

        Dim lsSQL As String

        'Check if already loaded base on edit mode
        If p_nEditMode = xeEditMode.MODE_READY Or p_nEditMode = xeEditMode.MODE_UPDATE Then
            If fbByCode Then
                If fsValue = p_oDTMstr(0).Item("sTransNox") Then Return True
            Else
                If fsValue = p_oOthersx.sClientNm Then Return True
            End If
        End If

        'Initialize SQL filter
        lsSQL = getSQ_Browse()

        'create Kwiksearch filter
        Dim lsFilter As String
        If fbByCode Then
            lsFilter = "a.sTransNox = " & strParm(fsValue)
        Else
            lsFilter = "c.sCompnyNm like " & strParm(fsValue & "%")
        End If

        Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , lsSQL _
                                        , False _
                                        , lsFilter _
                                        , "sTransNox»sCompnyNm»sBranchNm" _
                                        , "Trans No»Client Name»Branch",
                                        , "a.sTransNox»c.sCompnyNm»IFNULL(b.sBranchNm, '')" _
                                        , IIf(fbByCode, 0, 1))
        If IsNothing(loDta) Then
            p_nEditMode = xeEditMode.MODE_UNKNOWN
            Return False
        Else
            Return OpenTransaction(loDta.Item("sTransNox"))
        End If
    End Function

    'Public Function SaveTransaction
    Public Function SaveTransaction() As Boolean
        Dim lnRow As Integer = 0
        If Not (p_nEditMode = xeEditMode.MODE_ADDNEW Or
                p_nEditMode = xeEditMode.MODE_READY Or
                p_nEditMode = xeEditMode.MODE_UPDATE) Then

            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If

        If Not isEntryOk() Then
            Return False
        End If

        Dim lsSQL As String = ""

        If p_sParent = "" Then p_oApp.BeginTransaction()
        'Save detail table

        With p_oDTDetail
            For nCtr As Integer = 0 To .Rows.Count - 1

                lsSQL = "INSERT INTO " & pxeDetailTble & " SET" &
                            "  sTransNox = " & strParm(p_oDTMstr(0).Item("sTransNox")) &
                            ", nEntryNox = " & nCtr + 1 &
                            ", sItemCode = " & strParm(.Rows(nCtr)("sItemCode")) &
                            ", sItemDesc = " & strParm(.Rows(nCtr)("sItemDesc")) &
                            ", nUnitPrce = " & .Rows(nCtr)("nUnitPrce") &
                            ", nQuantity = " & .Rows(nCtr)("nQuantity") &
                            ", nDiscRate = " & .Rows(nCtr)("nDiscRate") &
                            ", nDiscAmnt = " & .Rows(nCtr)("nDiscAmnt") &
                            ", cReversex = " & strParm(.Rows(nCtr)("cReversex"))

                Try
                    lnRow = p_oApp.Execute(lsSQL, pxeDetailTble)
                    If lnRow <= 0 Then
                        MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                        p_oApp.RollBackTransaction()
                        Return False
                    End If
                Catch ex As Exception
                    Throw ex
                    p_oApp.RollBackTransaction()
                    Return False
                End Try
                lnRow = lnRow + 1
            Next nCtr
        End With

        'Save master table 
        If p_nEditMode = xeEditMode.MODE_ADDNEW Then


            lsSQL = "INSERT INTO " & pxeMasterTble & " SET" &
                                    "  sTransNox = " & strParm(p_oDTMstr(0).Item("sTransNox")) &
                                    ", dTransact = " & dateParm(p_oDTMstr(0).Item("dTransact")) &
                                    ", sEmployID = " & strParm(p_oDTMstr(0).Item("sEmployID")) &
                                    ", nEntryNox = " & lnRow - 1 &
                                    ", nTotalAmt = " & CDec(p_oDTMstr(0).Item("nTotalAmt")) &
                                    ", dPlacedxx = " & datetimeParm(p_oApp.getSysDate) &
                                    ", dTimeStmp = " & datetimeParm(p_oApp.getSysDate)


            Try
                lnRow = p_oApp.Execute(lsSQL, pxeMasterTble)
                If lnRow <= 0 Then
                    MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                    p_oApp.RollBackTransaction()
                    Return False
                End If
            Catch ex As Exception
                Throw ex
                p_oApp.RollBackTransaction()
                Return False
            End Try
        End If

        lsSQL = "INSERT INTO " & pxeSummaryTble & " SET" &
                                    "  sEmployID = " & strParm(p_oDTMstr(0).Item("sEmployID")) &
                                    ", dTransact = " & dateParm(p_oDTMstr(0).Item("dTransact")) &
                                    ", nEntryNox = " & xeLogical.YES &
                                    ", nTranTotl = " & p_oDTMstr(0).Item("nTotalAmt") &
                                    ", cTranStat = " & xeTranStat.TRANS_OPEN &
                                    ", cSendStat = " & xeLogical.NO &
                                    ", sModified = " & strParm(p_oApp.UserID) &
                                    ", dModified = " & datetimeParm(p_oApp.getSysDate) &
                                " ON DUPLICATE KEY UPDATE " &
                                    " nEntryNox = nEntryNox + 1 " &
                                    ", nTranTotl = nTranTotl + " & p_oDTMstr(0).Item("nTotalAmt")

        Try
            lnRow = p_oApp.Execute(lsSQL, pxeSummaryTble)
            If lnRow <= 0 Then
                MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                        "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                p_oApp.RollBackTransaction()
                Return False
            End If
        Catch ex As Exception
                Throw ex
                p_oApp.RollBackTransaction()
                Return False
            End Try

        If p_sParent = "" Then p_oApp.CommitTransaction()

        p_nEditMode = xeEditMode.MODE_READY

        'If Not pbHsParent Then Call printReciept()

        Return True
    End Function


    'This method implements a search master where id and desc are not joined.
    'This method implements a search master where id and desc are not joined.
    Private Sub getClient(ByVal fnColIdx As Integer _
                          , ByVal fnColDsc As Integer _
                          , ByVal fsValue As String _
                          , ByVal fbIsCode As Boolean _
                          , ByVal fbIsSrch As Boolean)

        'Compare the value to be search against the value in our column
        If fbIsCode Then
            If fsValue = p_oDTMstr(0).Item(fnColIdx) And fsValue <> "" And p_oOthersx.sClientNm <> "" Then Exit Sub
        Else
            If fsValue = p_oOthersx.sClientNm And fsValue <> "" Then Exit Sub
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" &
                       "  a.sClientID" &
                       ", a.sCompnyNm" &
                       ", CONCAT(IF(IFNull(a.sHouseNox, '') = '', '', CONCAT(a.sHouseNox, ' ')), a.sAddressx, ', ', b.sTownName, ', ', c.sProvName, ' ', b.sZippCode) AS xAddressx" &
               " FROM `Client_Master` a" &
                " LEFT JOIN TownCity b ON a.sTownIDxx = b.sTownIDxx" &
                " LEFT JOIN Province c ON b.sProvIDxx = c.sProvIDxx" &
        IIf(fbIsCode = False, " WHERE a.cRecdStat = '1'", "")

        'Are we using like comparison or equality comparison
        If fbIsSrch Then
            Dim loRow As DataRow = KwikSearch(p_oApp _
                                             , lsSQL _
                                             , True _
                                             , fsValue _
                                             , "sClientID»sCompnyNm»xAddressX" _
                                             , "ID»Client Name»Address",
                                             , "a.sClientID»a.sCompnyNm»CONCAT(IF(IFNull(a.sHouseNox, '') = '', '', CONCAT(a.sHouseNox, ' ')), a.sAddressx, ', ', b.sTownName, ', ', c.sProvName, ' ', b.sZippCode)" _
                                             , IIf(fbIsCode, 0, 1))
            If IsNothing(loRow) Then
                p_oDTMstr(0).Item(fnColIdx) = ""
                p_oOthersx.sClientNm = ""
                p_oOthersx.sAddressX = ""
            Else
                p_oDTMstr(0).Item(fnColIdx) = loRow.Item("sClientID")
                p_oOthersx.sClientNm = loRow.Item("sCompnyNm")
                p_oOthersx.sAddressX = loRow.Item("xAddressX")
            End If

            RaiseEvent MasterRetrieved(fnColDsc, p_oOthersx.sClientNm)
            Exit Sub

        End If

        If fsValue <> "" Then
            If fbIsCode Then
                lsSQL = AddCondition(lsSQL, "a.sClientID = " & strParm(fsValue))
            Else
                lsSQL = AddCondition(lsSQL, "a.sCompnyNm = " & strParm(fsValue))
            End If
        End If

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            p_oDTMstr(0).Item(fnColIdx) = ""
            p_oOthersx.sClientNm = ""
            p_oOthersx.sAddressX = ""
        ElseIf loDta.Rows.Count = 1 Then
            p_oDTMstr(0).Item(fnColIdx) = loDta(0).Item("sClientID")
            p_oOthersx.sClientNm = loDta(0).Item("sClientNm")
            p_oOthersx.sAddressX = loDta(0).Item("xAddressX")
        End If

        RaiseEvent MasterRetrieved(fnColDsc, p_oOthersx.sClientNm)
    End Sub

    Private Sub initMaster()
        Dim lnCtr As Integer
        For lnCtr = 0 To p_oDTMstr.Columns.Count - 1
            Select Case LCase(p_oDTMstr.Columns(lnCtr).ColumnName)
                Case "stransnox"
                    p_oDTMstr(0).Item(lnCtr) = getNextTransNo()
                Case "dtransact", "dplacedxx", "dtimestmp"
                Case "ntotalamt"
                    p_oDTMstr(0).Item(lnCtr) = 0.0
                Case "nentrynox"
                    p_oDTMstr(0).Item(lnCtr) = 0
                Case Else
                    p_oDTMstr(0).Item(lnCtr) = ""
            End Select
        Next
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

        lsSQL = "SELECT sTransNox" &
                " FROM " & pxeMasterTble &
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oApp.getSysDate(), "yy") & "%") &
                " ORDER BY sTransNox DESC" &
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


    Private Sub initOthers()
        p_oOthersx.sClientNm = ""
        p_oOthersx.sAddressX = ""
    End Sub

    Private Function isEntryOk() As Boolean

        'Check how much does he intends to borrow
        Debug.Print("employeeid" & p_oDTMstr(0).Item("sEmployID"))
        If Trim(p_oDTMstr(0).Item("sEmployID")) = "" Then
            MsgBox("Client seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        Return True
    End Function

    Private Function getSQ_Master() As String
        Return "SELECT a.sTransNox" &
                    ", a.dTransact" &
                    ", a.sEmployID" &
                    ", a.nEntryNox" &
                    ", a.nTotalAmt" &
                    ", a.dPlacedxx" &
                    ", a.dTimeStmp" &
                " FROM " & p_sMasTable & " a"
    End Function

    Private Function getSQ_Detail() As String
        Return "SELECT a.sTransNox" &
                    ", a.nEntryNox" &
                    ", a.sItemCode" &
                    ", a.sItemDesc" &
                    ", a.nUnitPrce" &
                    ", a.nQuantity" &
                    ", a.nDiscRate" &
                    ", a.nDiscAmnt" &
                    ", a.cReversex" &
                    ", a.dTimeStmp" &
                " FROM " & pxeDetailTble & " a"
    End Function

    'Note: Activate this commented function once we will begin using Client_Master instead of using free text
    'Private Function getSQ_Browse() As String
    '    Return "SELECT a.sTransNox" & _
    '                ", c.sCompnyNm" & _
    '                ", IFNULL(b.sBranchNm, '') sBranchNm" & _
    '          " FROM " & p_sMasTable & " a" & _
    '            " LEFT JOIN Branch b On a.sBranchCD = b.sBranchCD" & _
    '            " LEFT JOIN Client_Master c ON a.sClientID = c.sClientID"
    'End Function

    Private Function getSQ_Browse() As String
        Return "SELECT a.sTransNox" &
                    ", a.sClientID" &
                    ", IFNULL(b.sBranchNm, '') sBranchNm" &
              " FROM " & p_sMasTable & " a" &
                " LEFT JOIN Branch b On a.sBranchCD = b.sBranchCD"
    End Function

    Sub ShowChargeInvoiceMeal()
        'p_oDTMstr(0)("nAmountxx") = p_nSales

        p_oFormChargeInfo = New frmQRResult(p_oApp)
        With p_oFormChargeInfo
            .TopMost = True
            .ChargeInvoiceMeal = Me
            .ChargeInformation = p_sChargeInfo
            .SummaryTotal = getRunningTotal()
            .ShowDialog()

            p_bCancelled = .Cancelled

        End With

    End Sub



    Public Function getRunningTotal() As String
        Dim loDT As DataTable
        Dim lsSQL As String
        lsSQL = "SELECT IFNULL(SUM(nTranTotl), 0.0) nTranTotl " &
                 " FROM Employee_Meal_Summary " &
                    " WHERE sEmployID = " & strParm(p_oDTMstr.Rows(0)("sEmployID")) &
                    " AND cTranStat = " & strParm(xeTranStat.TRANS_OPEN)

        'Debug.Print(lsSQL)

        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count > 0 Then
            Return CDec(loDT(0).Item("nTranTotl"))
        Else
            Return "0.00"
        End If

    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oApp.BranchCode

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub

    Private Class Others
        Public sClientNm As String
        Public sAddressX As String
    End Class



End Class
