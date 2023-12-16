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

Public Class ChargeInvoice
    Private Const pxeMasterTble As String = "Charge_Invoice"
    Private Const xsSignature As String = "08220326"

    Private p_oApp As GRider
    Private p_oDTMstr As DataTable
    Private p_oOthersx As New Others

    Private p_oFormChargeInv As frmChargeInvoice
    Private p_nEditMode As xeEditMode

    Private p_sParent As String
    Private p_sPOSNo As String
    Private p_sBranchCd As String
    Private p_nSales As Decimal
    Private p_nDiscount As Decimal
    Private p_bCancelled As Boolean

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

    Private Const p_sMasTable As String = "Charge_Invoice"

    Private Const p_sMsgHeadr As String = "Charge Invoice Transaction"

    Public Event MasterRetrieved(ByVal Index As Integer, _
                                  ByVal Value As Object)

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

    WriteOnly Property HasParent As Boolean
        Set(ByVal Value As Boolean)
            pbHsParent = Value
        End Set
    End Property

    WriteOnly Property AccrdNumber As String
        Set(ByVal Value As String)
            p_sACCNox = Value
        End Set
    End Property

    WriteOnly Property AccrdFrom As Date
        Set(ByVal Value As Date)
            p_dACCFrm = Value
        End Set
    End Property

    WriteOnly Property AccrdThru As Date
        Set(ByVal Value As Date)
            p_dACCTru = Value
        End Set
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
                    Case 80
                        'If Trim(IFNull(p_oDTMstr(0).Item(1))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                        '    getClient(1, 80, p_oDTMstr(0).Item(1), True, False)
                        'End If
                        Return p_oOthersx.sClientNm
                    Case 81
                        'If Trim(IFNull(p_oDTMstr(0).Item(1))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                        '    getClient(1, 80, p_oDTMstr(0).Item(1), True, False)
                        'End If
                        Return p_oOthersx.sAddressX
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
                    Case 80         'sClientNm
                        'Call getClient(1, 80, value, False, False)
                        p_oOthersx.sClientNm = value
                    Case 81         'xAddressx
                        p_oOthersx.sAddressX = value
                    Case 9, 10
                        p_oDTMstr(0).Item(Index) = value
                    Case 0, 2 To 8 'All fiels except sClientNm and sClientID
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
                    Case "sclientnm"
                        'If Trim(IFNull(p_oDTMstr(0).Item(1))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                        '    getClient(1, 80, p_oDTMstr(0).Item(1), True, False)
                        'End If
                        Return p_oOthersx.sClientNm
                    Case "xaddressx"
                        'If Trim(IFNull(p_oDTMstr(0).Item(1))) <> "" And Trim(p_oOthersx.sClientNm) = "" Then
                        '    getClient(1, 80, p_oDTMstr(0).Item(1), True, False)
                        'End If
                        Return p_oOthersx.sAddressX
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
                    Case "sclientnm"
                        'Call getClient(1, 80, value, False, False)
                        p_oOthersx.sClientNm = value
                    Case "xaddressx"
                        p_oOthersx.sAddressX = value
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

        p_nEditMode = xeEditMode.MODE_ADDNEW

        Return True
    End Function

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

    'Public Function LoadBySource(String, String)
    Public Function LoadBySource(ByVal fsSourceNo As String, ByVal fsSourceCD As String) As Boolean
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Master, "a.sSourceCD = " & strParm(fsSourceCD) & " AND a.sSourceNo = " & strParm(fsSourceNo))
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
    Public Function SearchTransaction( _
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
                                        , "Trans No»Client Name»Branch", _
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


        p_oDTMstr(0)("sClientNm") = p_oOthersx.sClientNm
        p_oDTMstr(0)("sAddressx") = p_oOthersx.sAddressX

        p_oDTMstr(0)("nVATSales") = p_oDTMstr(0)("nAmountxx") / 1.12
        p_oDTMstr(0)("nVATAmtxx") = p_oDTMstr(0)("nAmountxx") - p_oDTMstr(0)("nVATSales")

        'Save master table 
        If p_nEditMode = xeEditMode.MODE_ADDNEW Then
            lsSQL = ADO2SQL(p_oDTMstr, p_sMasTable, , p_oApp.UserID, p_oApp.SysDate)
        Else
            lsSQL = ADO2SQL(p_oDTMstr, p_sMasTable, "sTransNox = " & strParm(p_oDTMstr(0).Item("sTransNox")), p_oApp.UserID, Format(p_oApp.SysDate, xsDATE_SHORT))
        End If

        If lsSQL <> "" Then
            p_oApp.Execute(lsSQL, p_sMasTable)
        End If

        If p_sParent = "" Then p_oApp.CommitTransaction()

        p_nEditMode = xeEditMode.MODE_READY

        'If Not pbHsParent Then Call printReciept()

        Return True
    End Function

    'Public Function CloseTransaction
    Public Function Bill(ByVal fsTransNox As String, ByVal fdTransact As Date) As Boolean
        If fsTransNox <> p_oDTMstr(0).Item("sTransNox") Then
            If Not OpenTransaction(fsTransNox) Then
                Return False
            End If
        End If

        Dim lsSQL As String

        If p_sParent = "" Then p_oApp.BeginTransaction()

        p_oDTMstr(0).Item("cTranStat") = "1"
        p_oDTMstr(0).Item("cBilledxx") = "1"
        p_oDTMstr(0).Item("dBilledxx") = fdTransact

        lsSQL = ADO2SQL(p_oDTMstr, p_sMasTable, "sTransNox = " & strParm(p_oDTMstr(0).Item("sTransNox")))
        p_oApp.Execute(lsSQL, p_sMasTable, p_oApp.BranchCode)

        If p_sParent = "" Then p_oApp.CommitTransaction()

        Return True
    End Function

    'Public Function PostTransaction
    Public Function Pay(ByVal fsTransNox As String, ByVal fdTransact As Date) As Boolean
        If fsTransNox <> p_oDTMstr(0).Item("sTransNox") Then
            If Not OpenTransaction(fsTransNox) Then
                Return False
            End If
        End If

        Dim lsSQL As String

        If p_sParent = "" Then p_oApp.BeginTransaction()

        Try
            p_oDTMstr(0).Item("cTranStat") = "2"
            p_oDTMstr(0).Item("cPaidxxxx") = "1"
            p_oDTMstr(0).Item("dPaidxxxx") = fdTransact
            p_oDTMstr(0).Item("nAmtPaidx") = p_oDTMstr(0).Item("nAmountxx")

            lsSQL = ADO2SQL(p_oDTMstr, p_sMasTable, "sTransNox = " & strParm(p_oDTMstr(0).Item("sTransNox")))
            p_oApp.Execute(lsSQL, p_sMasTable, p_oApp.BranchCode)
        Catch ex As MySqlException
            MsgBox(ex.Message)
            Throw ex
            If p_sParent = "" Then p_oApp.RollBackTransaction()
            Return False
        End Try

        If p_sParent = "" Then p_oApp.CommitTransaction()

        Return True
    End Function

    'Public Function VoidTransaction
    Public Function Waive(ByVal fsTransNox As String, ByVal fdTransact As Date, ByVal fsWaivexxx As String) As Boolean
        If fsTransNox <> p_oDTMstr(0).Item("sTransNox") Then
            If Not OpenTransaction(fsTransNox) Then
                Return False
            End If
        End If

        Dim lsSQL As String

        If p_sParent = "" Then p_oApp.BeginTransaction()

        Try
            p_oDTMstr(0).Item("cTranStat") = "4"
            p_oDTMstr(0).Item("cWaivexxx") = "1"
            p_oDTMstr(0).Item("dWaivexxx") = fdTransact
            p_oDTMstr(0).Item("sWaivexxx") = fsWaivexxx

            lsSQL = ADO2SQL(p_oDTMstr, p_sMasTable, "sTransNox = " & strParm(p_oDTMstr(0).Item("sTransNox")))
            p_oApp.Execute(lsSQL, p_sMasTable, p_oApp.BranchCode)
        Catch ex As MySqlException
            MsgBox(ex.Message)
            Throw ex
            If p_sParent = "" Then p_oApp.RollBackTransaction()
            Return False
        End Try

        If p_sParent = "" Then p_oApp.CommitTransaction()

        Return True
    End Function

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
        lsSQL = "SELECT" & _
                       "  a.sClientID" & _
                       ", a.sCompnyNm" & _
                       ", CONCAT(IF(IFNull(a.sHouseNox, '') = '', '', CONCAT(a.sHouseNox, ' ')), a.sAddressx, ', ', b.sTownName, ', ', c.sProvName, ' ', b.sZippCode) AS xAddressx" & _
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
                                             , "sClientID»sCompnyNm»xAddressX" _
                                             , "ID»Client Name»Address", _
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
                Case "dmodified", "smodified", "dbilledxx", "dpaidxxxx", "dwaivexxx"
                Case "crecdstat", "cbilledxx", "cpaidxxxx", "cwaivexxx"
                    p_oDTMstr(0).Item(lnCtr) = "0"
                Case "namountxx", "namtpaidx", "ndiscount", "nvatdiscx", "npwddiscx", "nvatsales", "nvatamtxx"
                    p_oDTMstr(0).Item(lnCtr) = 0.0
                Case "ctranstat"
                    p_oDTMstr(0).Item(lnCtr) = "0"
                Case "schargeno"
                    p_oDTMstr(0).Item(lnCtr) = Strings.Right("000000000000000" & getNextChargeNumber().ToString(), 15)
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

        lsSQL = "SELECT sTransNox" & _
                " FROM " & pxeMasterTble & _
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

    Private Function getNextChargeNumber() As String
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = "SELECT sChargeNo" & _
                " FROM " & pxeMasterTble & _
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sPOSNo & Format(p_oApp.getSysDate().ToString("yy") & "%")) & _
                " ORDER BY sChargeNo DESC" & _
                " LIMIT 1"

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            Return 1
        Else
            Return CInt(IIf(loDT(0).Item("sChargeNo") = "", 0, loDT(0).Item("sChargeNo"))) + 1
        End If
    End Function

    Private Sub initOthers()
        p_oOthersx.sClientNm = ""
        p_oOthersx.sAddressX = ""
    End Sub

    Private Function isEntryOk() As Boolean
        'Check for the information about the card
        If Trim(p_oDTMstr(0).Item("sSourceNo")) = "" Then
            MsgBox("Source No seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        'Check how much does he intends to borrow
        'If Trim(p_oDTMstr(0).Item("sClientID")) = "" Then
        '    MsgBox("Client seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
        '    Return False
        'End If

        If Not pbHsParent Then
            If p_oOthersx.sClientNm = "" Then
                MsgBox("Client seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
                Return False
            End If

            If p_oOthersx.sAddressX = "" Then
                MsgBox("Address seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
                Return False
            End If
        End If

        Return True
    End Function

    Private Function getSQ_Master() As String
        Return "SELECT a.sTransNox" & _
                    ", a.sClientID" & _
                    ", a.sChargeNo" & _
                    ", a.cBilledxx" & _
                    ", a.dBilledxx" & _
                    ", a.cPaidxxxx" & _
                    ", a.dPaidxxxx" & _
                    ", a.cWaivexxx" & _
                    ", a.dWaivexxx" & _
                    ", a.sWaivexxx" & _
                    ", a.sSourceCd" & _
                    ", a.sSourceNo" & _
                    ", a.nAmountxx" & _
                    ", a.nVATSales" & _
                    ", a.nVATAmtxx" & _
                    ", a.nDiscount" & _
                    ", a.nVatDiscx" & _
                    ", a.nPWDDiscx" & _
                    ", a.nAmtPaidx" & _
                    ", a.cTranStat" & _
                    ", a.sClientNm" & _
                    ", a.sAddressx" & _
                    ", a.dModified" & _
                " FROM " & p_sMasTable & " a"
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
        Return "SELECT a.sTransNox" & _
                    ", a.sClientID" & _
                    ", IFNULL(b.sBranchNm, '') sBranchNm" & _
              " FROM " & p_sMasTable & " a" & _
                " LEFT JOIN Branch b On a.sBranchCD = b.sBranchCD"
    End Function

    Sub ShowChargeInvoice()
        p_oDTMstr(0)("nAmountxx") = p_nSales

        p_oFormChargeInv = New frmChargeInvoice
        With p_oFormChargeInv
            .SalesTotal = p_nSales
            .Discounts = p_nDiscount
            .ChargeInvoice = Me
            .TopMost = True
            .ShowDialog()

            p_bCancelled = .Cancelled
        End With

    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oApp.BranchCode

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub

    Private Class Others
        Public sClientNm As String
        Public sAddressX As String
    End Class

    Function printReciept(Optional ByVal bReprint As Boolean = False) As Boolean
        Dim lnCtr As Integer
        Dim loPrint As PRN_Charge

        loPrint = New PRN_Charge(p_oApp)

        With loPrint

            If Not .InitMachine Then Return False
            .Transaction_Date = p_dTransact

            .Cashier = getCashier(p_sCashierx)
            .SerialNo = p_sSerial
            .TranMode = p_cTrnMde
            .AccrdNumber = p_sACCNox
            .ClientNo = p_nNoClient
            .WithDisc = p_nWithDisc
            .TableNo = p_nTableNo
            .LogName = p_sLogName

            .ReferenceNo = p_oDTMstr.Rows(0).Item("sChargeNo")
            .SourceNo = p_oDTMstr(0).Item("sSourceNo")
            .CustName = p_oDTMstr.Rows(0).Item("sClientNm")
            .CustAddress = Left(p_oDTMstr.Rows(0).Item("sAddressx"), 28)

            If Not IsNothing(p_oDtaOrder) Then
                Dim lnSlPrc As Double
                Dim lnComplmnt As Integer = 0
                Dim lnQuantity As Integer = 0
                For lnCtr = 0 To p_oDtaOrder.Rows.Count - 1
                    'Get compliment for the master item
                    If p_oDtaOrder(lnCtr)("cDetailxx") = xeLogical.NO Then
                        lnComplmnt = p_oDtaOrder(lnCtr)("nComplmnt")
                        lnQuantity = p_oDtaOrder(lnCtr)("nQuantity")
                    End If

                    'Do not include REVERSE(D) orders here...
                    If p_oDtaOrder(lnCtr)("cReversed") = xeLogical.NO Then
                        'Compute unit price here...
                        lnSlPrc = (p_oDtaOrder(lnCtr).Item("nUnitPrce") * _
                                    (100 - p_oDtaOrder(lnCtr).Item("nDiscount")) / 100 -
                                    p_oDtaOrder(lnCtr).Item("nAddDiscx"))

                        Dim lnDiv As Double
                        lnDiv = p_oDtaOrder(lnCtr)("nQuantity") / lnQuantity

                        If lnQuantity - lnComplmnt > 0 Then
                            .AddDetail(lnDiv * (lnQuantity - lnComplmnt), _
                                       p_oDtaOrder(lnCtr)("sBriefDsc"), _
                                       p_oDtaOrder(lnCtr)("nUnitPrce"), _
                                       True, _
                                       p_oDtaOrder(lnCtr)("cDetailxx") = "1", _
                                       IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                        End If

                        If lnComplmnt > 0 Then
                            .AddComplement(lnDiv * lnComplmnt, _
                                       p_oDtaOrder(lnCtr)("sBriefDsc"), _
                                       0, _
                                       True, IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                        End If
                    Else
                        'kalyptus - 2017.01.27 09:42am
                        'Print reverse items
                        If p_oDtaOrder(lnCtr)("cReversex") = "+" Then
                            .AddDetail(p_oDtaOrder(lnCtr)("nQuantity") * -1, _
                                       "Void-" & p_oDtaOrder(lnCtr)("sBriefDsc"), _
                                       0, _
                                       True, _
                                       p_oDtaOrder(lnCtr)("cDetailxx") = "1", _
                                       IIf(p_oDtaOrder(lnCtr)("cComboMlx") <> "1", True, False))
                        End If
                    End If
                Next
            End If
            'End If

            Dim loDR As DataRow
            Dim loDiscCard As clsDiscountCards
            loDiscCard = New clsDiscountCards(p_oApp)

            If Not IsNothing(p_oDtaDiscx) Then
                loDR = loDiscCard.SearchCard(p_oDtaDiscx(0)("sCardIDxx"), True)

                If Not IsNothing(loDR) Then
                    .AddDiscount(p_oDtaDiscx(0)("sIDNumber"), _
                                 loDR("sCardDesc"), _
                                 p_oDtaDiscx(0)("nDiscRate"), _
                                 p_oDtaDiscx(0)("nDiscAmtx"), _
                                 p_nDiscAmtx, _
                                 IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True), _
                                 p_oDtaDiscx(0)("nNoClient"), _
                                 p_oDtaDiscx(0)("nWithDisc"),
                                 p_oDtaDiscx(0)("sClientNm"))
                Else
                    .AddDiscount("", _
                                 "", _
                                 p_oDtaDiscx(0)("nDiscRate"), _
                                 p_oDtaDiscx(0)("nDiscAmtx"), _
                                 p_nDiscAmtx, _
                                 IIf(p_oDtaDiscx(0)("cNoneVatx") = "1", False, True))
                End If
            End If

            .NonVatSales = p_nNonVATxx

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
            .AddFooter("THIS DOCUMENT IS NOT VALID ")
            .AddFooter("FOR CLAIM OF INPUT TAX")

            Return .PrintOR
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

        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            lsCashierNm = ""
        Else
            lsCashierNm = Decrypt(loDta(0).Item("sUserName"), xsSignature)
        End If

        loDta = Nothing

        Return lsCashierNm
    End Function

End Class
