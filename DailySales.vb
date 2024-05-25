'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Daily Sales Report/Summary Object
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
' ==========================================================================================
'  kalyptus [ 11/21/2016 03:45 pm ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ADODB
Imports ggcAppDriver
Imports ggcReceipt
Imports System.IO

Public Class DailySales
    Private p_oApp As GRider
    Private p_nEditMode As xeEditMode
    Private p_sParent As String

    Private p_oDTMaster As DataTable

    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sVATReg As String     'TIN:       941-184-389-000
    Private p_sCompny As String     'Company  : MONARK HOTEL

    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_sAccrdt As String     'Accrdt No: 038-227471337-000028
    Private p_sTermnl As String     'Termnl No: 02
    Private p_nZRdCtr As Integer

    Private p_sCashier As String = ""

    '0->With Open Sales Order From Previous Sale;
    '1->Sales for the Day was already closed;
    '2->Sales for the Day is Ok;
    '3->Error Printing TXReading 
    '4->User is not allowed to enter Sales Transaction
    Private p_nSaleStat As Integer
    Private p_dPOSDatex As Date
    Private p_bWasRLCClt As Boolean

    Private Const p_sMasTable As String = "Daily_Summary"
    Private Const p_sMsgHeadr As String = "Daily Summary"
    Private Const xsSignature As String = "08220326"

    Private Const pxeLFTMGN As Integer = 3

    Public Event MasterRetrieved(ByVal Index As Integer, _
                                  ByVal Value As Object)

    'Property EditMode()
    Public ReadOnly Property EditMode() As xeEditMode
        Get
            Return p_nEditMode
        End Get
    End Property

    'We should access the value of this property after creating a new Daily_Summary record...
    Public ReadOnly Property SalesStatus() As Integer
        Get
            Return p_nSaleStat
        End Get
    End Property

    Public ReadOnly Property POSDate() As Date
        Get
            Return p_dPOSDatex
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

    Public Property Cashier() As String
        Get
            Return p_sCashier
        End Get
        Set(ByVal value As String)
            p_sCashier = value
        End Set
    End Property

    Public Property Master(ByVal Index As Integer) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Return p_oDTMaster(0).Item(Index)
            Else
                Return vbEmpty
            End If
        End Get

        Set(ByVal value As Object)
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Select Case Index
                    'On nOpenBalx and nCPullOut are allowed to be edited here...
                    Case 3, 4, 25, 26
                        p_oDTMaster(0).Item(Index) = value
                    Case Else
                        'Nothing to do
                End Select

                RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
            End If
        End Set
    End Property

    'Property Master(String)
    Public Property Master(ByVal Index As String) As Object
        Get
            If p_nEditMode <> xeEditMode.MODE_UNKNOWN Then
                Return p_oDTMaster(0).Item(Index)
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

    'Create CASHIER DAILY SALES in the Daily_Summary table
    Public Function NewTransaction() As Boolean
        'Set default as Sales for today is okey...
        p_nSaleStat = 2
        p_dPOSDatex = p_oApp.getSysDate

        'Check for open daily sales 
        If Not validSummary(False) Then
            MsgBox("There are open SALES ORDER!  Please close them first....")
            p_nSaleStat = 0
            Return False
        End If

        Dim lsSQL As String
        Dim loDta As DataTable
        Dim lsCashierx As String = p_oApp.UserID
        Dim lbSupervisor As Boolean = p_oApp.UserLevel >= xeUserRights.SUPERVISOR

        'check unclosed previous day summary
        lsSQL = "SELECT sTranDate, sCRMNumbr, sCashierx" &
               " FROM Daily_Summary" &
               " WHERE sCRMNumbr = " & strParm(p_sPOSNo) &
                 " AND sTranDate < " & Format(p_oApp.getSysDate, "yyyyMMdd") &
                 " AND cTranStat = '0'"

        Debug.Print(lsSQL)
        loDta = p_oApp.ExecuteQuery(lsSQL)
        If loDta.Rows.Count > 0 Then
            MsgBox("Previous' day's EOD was not performed.", MsgBoxStyle.Information, "Notice")

            lsCashierx = loDta(0)("sCashierx")
            p_sCashier = lsCashierx
            If lsCashierx = p_oApp.UserID Then 'own account
                If PrintTXReading(loDta(0)("sTranDate"), p_sPOSNo) Then
                    MsgBox("X-Reading for " & loDta(0)("sTranDate") & " printed successfuly." & vbCrLf &
                            "Please re-login to continue use of POS.", MsgBoxStyle.Information, "Notice")
                End If
            Else
                If lbSupervisor Then
                    If MsgBox(getCashier(lsCashierx) & " is the Cashier-In-Charge for the date " & loDta(0)("sTranDate") & "." & vbCrLf &
                           "Do you want to continue using your account to close her transaction?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Notice") = vbYes Then
                        If PrintTXReading(loDta(0)("sTranDate"), p_sPOSNo) Then
                            MsgBox("X-Reading for " & loDta(0)("sTranDate") & " printed successfuly." & vbCrLf &
                                    "Please re-login to continue use of POS.", MsgBoxStyle.Information, "Notice")
                        End If
                    End If
                Else
                    MsgBox(getCashier(lsCashierx) & " is the Cashier-In-Charge for the date " & loDta(0)("sTranDate") & "." & vbCrLf &
                           "Please ask to login her account to close the transaction.", MsgBoxStyle.Information, "Notice")
                End If
            End If

            p_nSaleStat = xeTranStat.TRANS_UNKNOWN
            p_dPOSDatex = p_oApp.getSysDate
            Return False
        Else
            'check unposted previous day summary
            lsSQL = "SELECT sTranDate, sCRMNumbr, sCashierx" &
                   " FROM Daily_Summary" &
                   " WHERE sCRMNumbr = " & strParm(p_sPOSNo) &
                     " AND sTranDate < " & Format(p_oApp.getSysDate, "yyyyMMdd") &
                     " AND cTranStat < '2'"
            loDta = p_oApp.ExecuteQuery(lsSQL)
            If loDta.Rows.Count > 0 Then
                Dim loZReading As PRN_TZ_Reading

                loZReading = New PRN_TZ_Reading(p_oApp)
                loZReading.isBackend = False

                If loZReading.PrintTZReading(loDta(0)("sTranDate"),
                                             loDta(0)("sTranDate"),
                                                    p_sPOSNo, False) Then

                    MsgBox("End of Day Transaction Summary successfully printed." & vbCrLf &
                            "Please re-login to continue use of POS.", MsgBoxStyle.Information, "Notice")
                    p_nSaleStat = 4

                    Return False
                End If
            End If
        End If

        'Prepare query for checking of unprocess Terminal X Ready
        lsSQL = "SELECT sTranDate, sCRMNumbr, sCashierx" &
               " FROM Daily_Summary" &
               " WHERE sCRMNumbr = " & strParm(p_sPOSNo) &
                 " AND sTranDate = " & Format(p_oApp.getSysDate, "yyyyMMdd") &
                 " AND cTranStat = '0'"
        loDta = p_oApp.ExecuteQuery(lsSQL)
9988577211:
        'Is there an unprocess Terminal X Reading
        If loDta.Rows.Count > 0 Then
            lsCashierx = loDta(0)("sCashierx")

            If lsCashierx = p_oApp.UserID Then 'own account
                p_nSaleStat = 0
            Else
                If lbSupervisor Then
                    If MsgBox("Someone is still logged as cashier. Do you continue with the account?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Confirm") = vbYes Then
                        p_sCashier = lsCashierx
                        p_nSaleStat = 0
                    Else
                        p_nSaleStat = 4
                        Return False
                    End If
                Else
                    MsgBox("Someone is still logged as cashier. Kinldy end the shift first.", MsgBoxStyle.Information, "Notice")
                    p_nSaleStat = 4
                    Return False
                End If
            End If
        End If

        'Prepare query that will check for the existence of CASHIER DAILY SALES
        lsSQL = Format(p_oApp.getSysDate, "yyyyMMdd")
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(lsSQL) &
                                      " AND sCRMNumbr = " & strParm(p_sPOSNo))
        loDta = Nothing
        loDta = p_oApp.ExecuteQuery(lsSQL)

        Dim lsCondition As String = "0=1"

        'Is there an existing CASHIER DAILY SALES
        If loDta.Rows.Count > 0 Then
            Dim lnCtr As Integer
            For lnCtr = 0 To loDta.Rows.Count - 1
                If loDta(lnCtr).Item("cTranStat") = xeTranStat.TRANS_OPEN Then

                    lsCondition = "sTranDate = " & strParm(Format(p_oApp.getSysDate, "yyyyMMdd")) &
                                " AND sCRMNumbr = " & strParm(p_sPOSNo) &
                                " AND sCashierx = " & strParm(lsCashierx) &
                                " AND cTranStat = " & strParm(xeTranStat.TRANS_OPEN)
                    Exit For
                ElseIf loDta(lnCtr).Item("cTranStat") = xeTranStat.TRANS_POSTED Then
                    MsgBox("This transaction date was already posted!", , p_sMsgHeadr)
                    p_nSaleStat = 1
                    'Return False
                End If
            Next
        End If

        lsSQL = AddCondition(getSQ_Master, lsCondition)
        p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)

        p_oApp.SaveEvent("0001", "", p_sSerial)

        If p_oDTMaster.Rows.Count = 0 Then
            p_oDTMaster.Rows.Add(p_oDTMaster.NewRow())
            Call initMaster()

            p_sCashier = p_oDTMaster(0)("sCashierx")
            p_nEditMode = xeEditMode.MODE_ADDNEW

            p_oApp.SaveEvent("0003", "", p_sSerial)
        Else
            p_nEditMode = xeEditMode.MODE_READY
            p_sCashier = lsCashierx
            p_nSaleStat = 2

            p_oApp.SaveEvent("0019", "", p_sSerial)
        End If

        Return True
    End Function

    ''Create CASHIER DAILY SALES in the Daily_Summary table
    'Public Function NewTransaction() As Boolean
    '    'Set default as Sales for today is okey...
    '    p_nSaleStat = 2

    '    'Check for open daily sales 
    '    If Not validSummary(False) Then
    '        MsgBox("There are open SALES ORDER!  Please close them first....")
    '        p_nSaleStat = 0
    '        Return False
    '    End If

    '    'Prepare query for checking of unprocess Terminal X Ready
    '    Dim lsSQL As String
    '    lsSQL = "SELECT sTranDate, sCRMNumbr" & _
    '           " FROM Daily_Summary" & _
    '           " WHERE sCRMNumbr = " & strParm(p_sPOSNo) & _
    '             " AND sTranDate < " & Format(p_oApp.getSysDate, "yyyyMMdd") & _
    '             " AND cTranStat = '0'"
    '    Dim loDta As DataTable
    '    loDta = p_oApp.ExecuteQuery(lsSQL)

    '    'Is there an unprocess Terminal X Reading
    '    If loDta.Rows.Count > 0 Then
    '        'Perform Termianl X Reading
    '        If Not PrintTXReading(loDta(0).Item("sTranDate"), loDta(0).Item("sCRMNumbr")) Then
    '            MsgBox("Terminal X Reading failed!!", , p_sMsgHeadr)
    '            p_nSaleStat = 3
    '            Return False
    '        Else
    '            MsgBox("Terminal X Reading was perform successfully!!", , p_sMsgHeadr)
    '        End If
    '    End If

    '    'Prepare query that will check for the existence of CASHIER DAILY SALES
    '    lsSQL = Format(p_oApp.getSysDate, "yyyyMMdd")
    '    lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(lsSQL) & _
    '                                  " AND sCRMNumbr = " & strParm(p_sPOSNo))
    '    loDta = Nothing
    '    loDta = p_oApp.ExecuteQuery(lsSQL)

    '    Dim lsCondition As String = "0=1"

    '    'Is there an existing CASHIER DAILY SALES
    '    If loDta.Rows.Count > 0 Then
    '        'Are/Is existing DAILY SALES POSTED/Assume once a CDS is posted all CDS for that day will be posted...
    '        If loDta(0).Item("cTranStat") = xeTranStat.TRANS_CLOSED Then
    '            MsgBox("This transaction date was already posted!", , p_sMsgHeadr)
    '            p_nSaleStat = 1
    '            Return False
    '        End If

    '        Dim lnCtr As Integer
    '        For lnCtr = 0 To loDta.Rows.Count - 1
    '            'Does the currently login CASHIER has an existing CASHIER DAILY SALES 
    '            If loDta(lnCtr).Item("sCashierx") = p_oApp.UserID Then
    '                'If loDta(0).Item("cTranStat") = xeTranStat.TRANS_OPEN Then
    '                '    lsCondition = "sTranDate = " & strParm(lsSQL) & _
    '                '             " AND sCRMNumbr = " & strParm(p_sPOSNo) & _
    '                '             " AND sCashierx = " & strParm(p_oApp.UserID)
    '                'ElseIf loDta(0).Item("cTranStat") = xeTranStat.TRANS_CLOSED Then
    '                '    'A cashier with closed CASHIER DAILY SALES will not be allowed to create another
    '                '    MsgBox("The cashier was already out!", , p_sMsgHeadr)
    '                '    Return False
    '                'End If

    '                lsCondition = "sTranDate = " & strParm(Format(p_oApp.getSysDate, "yyyyMMdd")) & _
    '                         " AND sCRMNumbr = " & strParm(p_sPOSNo) & _
    '                         " AND sCashierx = " & strParm(p_oApp.UserID)
    '                Exit For
    '            End If
    '        Next
    '    End If

    '    lsSQL = AddCondition(getSQ_Master, lsCondition)
    '    p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)

    '    If p_oDTMaster.Rows.Count = 0 Then
    '        p_oDTMaster.Rows.Add(p_oDTMaster.NewRow())
    '        Call initMaster()
    '        p_nEditMode = xeEditMode.MODE_ADDNEW
    '    Else
    '        p_nEditMode = xeEditMode.MODE_READY
    '    End If

    '    Return True
    'End Function

    'Save CASHIER DAILY SALES 
    Public Function SaveTransaction() As Boolean
        If Not (p_nEditMode = xeEditMode.MODE_ADDNEW Or
        p_nEditMode = xeEditMode.MODE_READY Or
        p_nEditMode = xeEditMode.MODE_UPDATE) Then

            MsgBox("Invalid Edit Mode detected!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End If

        Try
            Dim lsSQL As String

            'Save master table 
            If p_nEditMode = xeEditMode.MODE_ADDNEW Then
                lsSQL = ADO2SQL(p_oDTMaster, p_sMasTable)
            Else
                lsSQL = "sTranDate = " & strParm(p_oDTMaster(0).Item("sTranDate")) &
                            " AND sCRMNumbr = " & strParm(p_oDTMaster(0).Item("sCRMNumbr")) &
                            " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx"))
                lsSQL = ADO2SQL(p_oDTMaster, p_sMasTable, lsSQL)
            End If

            If p_sParent = "" Then p_oApp.BeginTransaction()

            If lsSQL <> "" Then
                p_oApp.Execute(lsSQL, p_sMasTable)
            Else
                MsgBox("Unable to Save Daily_Summary!!!" & vbCrLf &
                       "Pleased contact MIS SEG for assistance!!!", MsgBoxStyle.Critical, p_sMsgHeadr)
                Return False
            End If

            If p_sParent = "" Then p_oApp.CommitTransaction()

            p_nEditMode = xeEditMode.MODE_READY
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try

        Return True
    End Function

    'Open/Load CASHIER DAILY SALES 
    Public Function OpenTransaction(ByVal sTrandate As String, ByVal sCRMNumbr As String, ByVal sCashierx As String) As Boolean
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) & _
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) & _
                                      " AND sCashierx = " & strParm(sCashierx))
        p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)

        If p_oDTMaster.Rows.Count <= 0 Then
            p_nEditMode = xeEditMode.MODE_UNKNOWN
            Return False
        End If

        p_nEditMode = xeEditMode.MODE_READY
        Return True
    End Function

    'Cashier LogOut
    Public Function PrintCashierSales(ByVal sTrandate As String, ByVal sCRMNumbr As String, ByVal sCashierx As String) As Boolean
        'Open/Load the right daily summary
        If p_nEditMode = xeEditMode.MODE_UNKNOWN Then
            If Not OpenTransaction(sTrandate, sCRMNumbr, sCashierx) Then
                MsgBox("Cannot open the daily transaction", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, p_sMsgHeadr)
                Return False
            End If
        Else
            'If Not (p_oDTMaster(0).Item("sTrandate") = sTrandate And p_oDTMaster(0).Item("sCRMNumbr") = sCRMNumbr And p_oDTMaster(0).Item("sCashierx") = sCashierx) Then
            If Not OpenTransaction(sTrandate, sCRMNumbr, sCashierx) Then
                MsgBox("Cannot open the daily transaction", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, p_sMsgHeadr)
                Return False
            End If
            'End If
        End If

        Call doPrintCashierSales()

        p_oApp.SaveEvent("0021", "Date: " & sTrandate, p_sSerial)
        Return True

    End Function

    'EZ Reading
    Public Function PrintTXReading(ByVal sTrandate As String, ByVal sCRMNumbr As String, Optional bValidate As Boolean = True) As Boolean
        'Get configuration of machine
        If Not initMachine() Then
            Return False
        End If

        Dim bCurrent As Boolean = False
        If Format(p_oApp.getSysDate, "yyyyMMdd") = sTrandate Then bCurrent = True
        'check if there are open daily sales 
        If bValidate Then
            If Not validSummary(bCurrent) Then
                MsgBox("There are open SALES ORDER!  Please save them first....")
                Return False
            End If
        Else
            If Not bCurrent Then
                If Not validSummary(bCurrent) Then
                    Dim lsSQL As String
                    lsSQL = "SELECT a.sTransNox" &
                                    ", b.sTransNox xTransNox" &
                            " FROM SO_Master a" &
                                " LEFT JOIN SO_Detail b" &
                                    " ON a.sTransNox = b.sTransNox" &
                            " WHERE a.sTransNox Like " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                                    " And a.dTransact < " & dateParm(Format(p_oApp.SysDate, xsDATE_SHORT)) &
                                    " And a.cTranStat = '0'" &
                            " HAVING xTransNox IS NULL"

                    Dim loDta As DataTable
                    Dim lnCtr As Integer

                    loDta = p_oApp.ExecuteQuery(lsSQL)

                    If loDta.Rows.Count > 0 Then
                        For lnCtr = 0 To loDta.Rows.Count - 1
                            lsSQL = "UPDATE SO_Master SET" &
                                        " cTranStat = '3'" &
                                    " WHERE sTransNox = " & strParm(loDta.Rows(lnCtr)("sTransNox"))

                            p_oApp.Execute(lsSQL, "SO_Master")
                        Next
                    Else
                        MsgBox("There are open SALES ORDER!  Please save them first....")
                        Return False
                    End If
                End If
            End If
        End If

        If p_sParent = "" Then p_oApp.BeginTransaction()

        'print daily sales
        If Not doPrintTXReading(sTrandate, sCRMNumbr) Then
            MsgBox("Unable to perform Terminal X Reading!!", , p_sMsgHeadr)
            Return False
        End If

        Call doWriteTXReading(sTrandate, sCRMNumbr)

        'create query to post the daily sales
        If p_oDTMaster(0).Item("cTranStat") = "0" Then
            Dim lsSQL As String
            lsSQL = "UPDATE " & p_sMasTable & _
                    " SET cTranStat = " & strParm(xeTranStat.TRANS_CLOSED) & _
                    " WHERE sTranDate = " & strParm(sTrandate) & _
                        " AND sCRMNumbr = " & strParm(sCRMNumbr) & _
                        " AND cTranStat = '0'"
            'post daily sales
            p_oApp.Execute(lsSQL, p_sMasTable)
        End If

        p_oApp.SaveEvent("0004", "", p_sSerial)
        p_oApp.SaveEvent("0020", "Date: " & sTrandate, p_sSerial)
        If p_sParent = "" Then p_oApp.CommitTransaction()

        MsgBox("X-Reading was perform successfully!!", , p_sMsgHeadr)

        Return True
    End Function

    Private Function doWriteTXReading(ByVal sTrandate As String, ByVal sCRMNumbr As String) As Boolean
        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND sCashierx = " & strParm(p_sCashier) &
                                      " AND cTranStat = '0'")

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            MsgBox("There are no transaction for this date(" & sTrandate & ").", , p_sMsgHeadr)
            Return False
        ElseIf loDta.Rows.Count > 1 Then
            MsgBox("Unclosed cashier shifts detected for the date(" & sTrandate & ").", , p_sMsgHeadr)
            Return False
        End If

        If Not OpenTransaction(sTrandate, sCRMNumbr, loDta(0).Item("sCashierx")) Then
            MsgBox("Can't open transaction of " & getCashier(loDta(0).Item("sCashierx")))
            Return False
        End If

        'If p_oDTMaster(0).Item("cTranStat") = "0" Then
        '    'computeTotalCashierSales()

        '    lsSQL = "UPDATE Daily_Summary" &
        '           " SET nSalesAmt = " & p_oDTMaster(0).Item("nSalesAmt") &
        '              ", nVATSales = " & p_oDTMaster(0).Item("nVATSales") &
        '              ", nVATAmtxx = " & p_oDTMaster(0).Item("nVATAmtxx") &
        '              ", nNonVATxx = " & p_oDTMaster(0).Item("nNonVATxx") &
        '              ", nZeroRatd = " & p_oDTMaster(0).Item("nZeroRatd") &
        '              ", nDiscount = " & p_oDTMaster(0).Item("nDiscount") &
        '              ", nVatDiscx = " & p_oDTMaster(0).Item("nVatDiscx") &
        '              ", nPWDDiscx = " & p_oDTMaster(0).Item("nPWDDiscx") &
        '              ", nReturnsx = " & p_oDTMaster(0).Item("nReturnsx") &
        '              ", nVoidAmnt = " & p_oDTMaster(0).Item("nVoidAmnt") &
        '              ", nCancelld = " & p_oDTMaster(0).Item("nCancelld") &
        '              ", nAccuSale = " & p_oDTMaster(0).Item("nAccuSale") &
        '              ", nCashAmnt = " & p_oDTMaster(0).Item("nCashAmnt") &
        '              ", nSChargex = " & p_oDTMaster(0).Item("nSChargex") &
        '              ", nChckAmnt = " & p_oDTMaster(0).Item("nChckAmnt") &
        '              ", nCrdtAmnt = " & p_oDTMaster(0).Item("nCrdtAmnt") &
        '              ", nChrgAmnt = " & p_oDTMaster(0).Item("nChrgAmnt") &
        '              ", nGiftAmnt = " & p_oDTMaster(0).Item("nGiftAmnt") &
        '              ", sORNoFrom = " & strParm(p_oDTMaster(0).Item("sORNoFrom")) &
        '              ", sORNoThru = " & strParm(p_oDTMaster(0).Item("sORNoThru")) &
        '              ", nZReadCtr = " & p_nZRdCtr &
        '              ", dClosedxx = " & datetimeParm(p_oApp.SysDate) &
        '              ", nVoidCntx = " & p_oDTMaster(0).Item("nVoidCntx") &
        '           " WHERE sTranDate = " & strParm(p_oDTMaster(0).Item("sTranDate")) &
        '             " AND sCRMNumbr = " & strParm(p_oDTMaster(0).Item("sCRMNumbr")) &
        '             " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx"))

        '    p_oApp.Execute(lsSQL, p_sMasTable)
        'End If

        'Reload data after recomputation of total sales
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) &
                                      " AND cTranStat <> '3'")
        loDta = p_oApp.ExecuteQuery(lsSQL)

        'iMac 2018.02.10
        'get previous day accumulated sale
        lsSQL = "SELECT nAccuSale FROM Daily_Summary" &
                " WHERE sTranDate < " & strParm(sTrandate) &
                    " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                    " AND cTranStat IN ('1', '2')" &
                " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
        End If

        '        Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter(Trim(p_sCompny), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)

        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(PadCenter("X-READING", 40) & Environment.NewLine)

        'Get the transaction date thru reverse formatting the sTrandate field
        Dim lsTranDate As String
        lsTranDate = p_oDTMaster(0).Item("sTranDate")
        lsTranDate = Left(lsTranDate, 4) & "-" & Mid(lsTranDate, 5, 2) & "-" & Right(lsTranDate, 2)

        builder.Append(Environment.NewLine)
        builder.Append("DATE      :" & Format(CDate(lsTranDate), "dd-MMM-yyyy") & Environment.NewLine)
        builder.Append("CASHIER   :" & getCashier(p_sCashier) & Environment.NewLine)
        builder.Append("TERMINAL  :" & p_sTermnl & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)

        'builder.Append(Environment.NewLine)
        'builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        'builder.Append("DATE      :" & Format(CDate(lsTranDate), "dd-MMM-yyyy") & Environment.NewLine)
        'builder.Append("CASHIER   :" & getCashier(p_sCashier) & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)
        'builder.Append(RawPrint.pxePRINT_EMP0)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim lnOpenBalx As Decimal = 0
        Dim lnCPullOut As Decimal = 0

        Dim lnCashAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0   'Non-Vat means Vat Exempt
        Dim lnDiscount As Decimal = 0   'Regular Discount
        Dim lnVatDiscx As Decimal = 0   '12% VAT Discount
        Dim lnPWDDiscx As Decimal = 0   'Senior/PWD Discount

        Dim lnReturnsx As Decimal = 0   'Returns
        Dim lnVoidAmnt As Decimal = 0   'Void Transactions
        Dim lnVoidCntx As Integer = 0

        Dim lsORNoFrom As String = loDta(0).Item("sORNoFrom")
        Dim lsORNoThru As String = loDta(0).Item("sORNoThru")

        Dim ldStartShft As Date = loDta(0).Item("dOpenedxx")
        Dim ldEndedSfht As Date = loDta(0).Item("dClosedxx")

        'Compute Gross Sales
        lnSalesAmt = loDta(0).Item("nSalesAmt")

        'Compute VAT Related Sales
        lnVATSales = loDta(0).Item("nVATSales")
        lnVATAmtxx = loDta(0).Item("nVATAmtxx")
        lnZeroRatd = loDta(0).Item("nZeroRatd")

        'Compute Discounts
        lnDiscount = loDta(0).Item("nDiscount")
        lnVatDiscx = loDta(0).Item("nVatDiscx")
        lnPWDDiscx = loDta(0).Item("nPWDDiscx")

        'Compute Returns/Refunds/Void Transactions
        lnReturnsx = loDta(0).Item("nReturnsx")
        lnVoidAmnt = loDta(0).Item("nVoidAmnt")
        lnVoidCntx = loDta(0).Item("nVoidCntx")

        'Compute Cashier Collection Info
        lnOpenBalx = loDta(0).Item("nOpenBalx")
        lnCPullOut = loDta(0).Item("nCPullOut")

        lnCashAmnt = loDta(0).Item("nCashAmnt")
        lnSChargex = loDta(0).Item("nSChargex")
        lnChckAmnt = loDta(0).Item("nChckAmnt")
        lnCrdtAmnt = loDta(0).Item("nCrdtAmnt")
        lnChrgAmnt = loDta(0).Item("nChrgAmnt")
        lnGiftAmnt = loDta(0).Item("nGiftAmnt")

        'Compute for VAT Exempt Sales
        'lnNonVATxx = (lnSalesAmt + lnSChargex) - (lnVATSales + lnZeroRatd + lnVATAmtxx + lnPWDDiscx + lnVatDiscx + lnDiscount)
        'lnNonVATxx = loDta(0).Item("nNonVATxx") + lnPWDDiscx
        lnNonVATxx = loDta(0).Item("nNonVATxx")

        builder.Append(Environment.NewLine)
        builder.Append(" Shift Start   :  " & Format(ldStartShft, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)
        builder.Append(" Shift End     :  " & Format(ldEndedSfht, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine & Environment.NewLine)

        'Print the begging and ending OR
        builder.Append(" Beginning SI No.:  " & lsORNoFrom & Environment.NewLine)
        builder.Append(" Ending SI No.   :  " & lsORNoThru & Environment.NewLine)

        'MAC 2018.01.30
        '   Add Beginning and ending balance on report
        'builder.Append(Environment.NewLine)
        'builder.Append(" Beginning Balance  : ".PadRight(24) & Format(lnPrevSale, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Print the Computation of NET Sales
        builder.Append(Environment.NewLine)
        builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex + lnDiscount + lnVatDiscx + lnPWDDiscx + lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        If lnSChargex > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If

        builder.Append("        VAT SC/PWD".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)

        builder.Append(" NET SALES".PadRight(24) & Format(lnSalesAmt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" NET SALES".PadRight(24) & Format(lnSalesAmt - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(Environment.NewLine)

        builder.Append(" VATABLE Sales".PadRight(24) & Format(lnVATSales, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(lnVATAmtxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        '        builder.Append(Space(pxeLFTMGN) & " VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ZERO Rated Sales".PadRight(24) & Format(lnZeroRatd, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ' ''Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        ''builder.Append(Environment.NewLine)

        ' ''builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ' ''builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx + lnVatDiscx + lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("     Less: 20% Discount:".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("           Less 12% VAT:".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ' ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        builder.Append(Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Petty Cash".PadRight(24) & Format(lnOpenBalx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(lnCPullOut, xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        'builder.Append("  Cashbox Amount".PadRight(24) & Format(lnOpenBalx + (lnCashAmnt + lnSChargex) - lnCPullOut - lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cash".PadRight(24) & Format((lnOpenBalx + lnCashAmnt) - (lnCPullOut + lnReturnsx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(lnChckAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(lnCrdtAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(lnGiftAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(lnChrgAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        'builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" Void SI Count: ".PadRight(24) & Format(lnVoidCntx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" Void SI Amount: ".PadRight(24) & Format(lnVoidAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate(), "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)
        'builder.Append("/end-of-summary - " & Mid(sTrandate, 7, 2) + "/" + Mid(sTrandate, 5, 2) + "/" + Mid(sTrandate, 1, 4) + " " + Format(p_oApp.getSysDate(), "hh:mm:ss") & Environment.NewLine)

        RawPrint.writeToFile(p_sPOSNo, builder.ToString())
        RawPrint.writeToFile(p_sPOSNo & " " & sTrandate, builder.ToString())
        RawPrint.writeToFile(p_sPOSNo & " X-READING" & " " & sTrandate, builder.ToString())

        Return True
    End Function

    Public Function doWriteTXReadingReg(ByVal sTrandate As String,
                                         ByVal sCRMNumbr As String,
                                         ByVal sCashierx As String) As Boolean
        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND sCashierx = " & strParm(sCashierx))

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If Not OpenTransaction(sTrandate, sCRMNumbr, loDta(0).Item("sCashierx")) Then
            MsgBox("Can't open transaction of " & getCashier(loDta(0).Item("sCashierx")))
            Return False
        End If

        'Reload data after recomputation of total sales
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND sCashierx = " & strParm(sCashierx))
        loDta = p_oApp.ExecuteQuery(lsSQL)

        'iMac 2018.02.10
        'get previous day accumulated sale
        lsSQL = "SELECT nAccuSale FROM Daily_Summary" &
                " WHERE sTranDate < " & strParm(sTrandate) &
                    " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                    " AND cTranStat IN ('1', '2')" &
                " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
        End If

        '        Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)
        p_sCompny = "The Monarch Hospitality & Tourism Corp."
        builder.Append(PadCenter(Trim(p_sCompny), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)

        p_sVATReg = "469-083-682-002"
        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        p_sPOSNo = "22010313392685364"
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        p_sSerial = "WCC6Y4VEA7V0"
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(PadCenter("X-READING", 40) & Environment.NewLine)

        'Get the transaction date thru reverse formatting the sTrandate field
        Dim lsTranDate As String
        lsTranDate = p_oDTMaster(0).Item("sTranDate")
        lsTranDate = Left(lsTranDate, 4) & "-" & Mid(lsTranDate, 5, 2) & "-" & Right(lsTranDate, 2)

        builder.Append(Environment.NewLine)
        builder.Append("DATE      :" & Format(CDate(lsTranDate), "dd-MMM-yyyy") & Environment.NewLine)
        builder.Append("CASHIER   :" & getCashier(sCashierx) & Environment.NewLine)
        builder.Append("TERMINAL  :" & p_sTermnl & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)

        'builder.Append(Environment.NewLine)
        'builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        'builder.Append("DATE      :" & Format(CDate(lsTranDate), "dd-MMM-yyyy") & Environment.NewLine)
        'builder.Append("CASHIER   :" & getCashier(p_sCashier) & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)
        'builder.Append(RawPrint.pxePRINT_EMP0)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim lnOpenBalx As Decimal = 0
        Dim lnCPullOut As Decimal = 0

        Dim lnCashAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0   'Non-Vat means Vat Exempt
        Dim lnDiscount As Decimal = 0   'Regular Discount
        Dim lnVatDiscx As Decimal = 0   '12% VAT Discount
        Dim lnPWDDiscx As Decimal = 0   'Senior/PWD Discount

        Dim lnReturnsx As Decimal = 0   'Returns
        Dim lnVoidAmnt As Decimal = 0   'Void Transactions
        Dim lnVoidCntx As Integer = 0

        Dim lsORNoFrom As String = loDta(0).Item("sORNoFrom")
        Dim lsORNoThru As String = loDta(0).Item("sORNoThru")

        Dim ldStartShft As Date = loDta(0).Item("dOpenedxx")
        Dim ldEndedSfht As Date = loDta(0).Item("dClosedxx")

        'Compute Gross Sales
        lnSalesAmt = loDta(0).Item("nSalesAmt")

        'Compute VAT Related Sales
        lnVATSales = loDta(0).Item("nVATSales")
        lnVATAmtxx = loDta(0).Item("nVATAmtxx")
        lnZeroRatd = loDta(0).Item("nZeroRatd")

        'Compute Discounts
        lnDiscount = loDta(0).Item("nDiscount")
        lnVatDiscx = loDta(0).Item("nVatDiscx")
        lnPWDDiscx = loDta(0).Item("nPWDDiscx")

        'Compute Returns/Refunds/Void Transactions
        lnReturnsx = loDta(0).Item("nReturnsx")
        lnVoidAmnt = loDta(0).Item("nVoidAmnt")
        lnVoidCntx = loDta(0).Item("nVoidCntx")

        'Compute Cashier Collection Info
        lnOpenBalx = loDta(0).Item("nOpenBalx")
        lnCPullOut = loDta(0).Item("nCPullOut")

        lnCashAmnt = loDta(0).Item("nCashAmnt")
        lnSChargex = loDta(0).Item("nSChargex")
        lnChckAmnt = loDta(0).Item("nChckAmnt")
        lnCrdtAmnt = loDta(0).Item("nCrdtAmnt")
        lnChrgAmnt = loDta(0).Item("nChrgAmnt")
        lnGiftAmnt = loDta(0).Item("nGiftAmnt")

        'Compute for VAT Exempt Sales
        'lnNonVATxx = (lnSalesAmt + lnSChargex) - (lnVATSales + lnZeroRatd + lnVATAmtxx + lnPWDDiscx + lnVatDiscx + lnDiscount)
        'lnNonVATxx = loDta(0).Item("nNonVATxx") + lnPWDDiscx
        lnNonVATxx = loDta(0).Item("nNonVATxx")

        builder.Append(Environment.NewLine)
        builder.Append(" Shift Start   :  " & Format(ldStartShft, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)
        builder.Append(" Shift End     :  " & Format(ldEndedSfht, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine & Environment.NewLine)

        'Print the begging and ending OR
        builder.Append(" Beginning SI No.:  " & lsORNoFrom & Environment.NewLine)
        builder.Append(" Ending SI No.   :  " & lsORNoThru & Environment.NewLine)

        'MAC 2018.01.30
        '   Add Beginning and ending balance on report
        'builder.Append(Environment.NewLine)
        'builder.Append(" Beginning Balance  : ".PadRight(24) & Format(lnPrevSale, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Print the Computation of NET Sales
        builder.Append(Environment.NewLine)
        builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        If lnSChargex > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If

        builder.Append("        VAT SC/PWD".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)

        'builder.Append(" NET SALES".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx + lnSChargex), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" NET SALES".PadRight(24) & Format(lnSalesAmt - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(Environment.NewLine)

        builder.Append(" VATABLE Sales".PadRight(24) & Format(lnVATSales, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(lnVATAmtxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        '        builder.Append(Space(pxeLFTMGN) & " VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ZERO Rated Sales".PadRight(24) & Format(lnZeroRatd, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ' ''Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        ''builder.Append(Environment.NewLine)

        ' ''builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ' ''builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx + lnVatDiscx + lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("     Less: 20% Discount:".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("           Less 12% VAT:".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ' ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        builder.Append(Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Petty Cash".PadRight(24) & Format(lnOpenBalx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(lnCPullOut, xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        'builder.Append("  Cashbox Amount".PadRight(24) & Format(lnOpenBalx + (lnCashAmnt + lnSChargex) - lnCPullOut - lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cash".PadRight(24) & Format((lnOpenBalx + lnCashAmnt) - (lnCPullOut + lnReturnsx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(lnChckAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(lnCrdtAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(lnGiftAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(lnChrgAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        'builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" Void SI Count: ".PadRight(24) & Format(lnVoidCntx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" Void SI Amount: ".PadRight(24) & Format(lnVoidAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate(), "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)
        'builder.Append("/end-of-summary - " & Mid(sTrandate, 7, 2) + "/" + Mid(sTrandate, 5, 2) + "/" + Mid(sTrandate, 1, 4) + " " + Format(p_oApp.getSysDate(), "hh:mm:ss") & Environment.NewLine)

        RawPrint.writeToFile(p_sPOSNo & " X-READING" & " " & sTrandate, builder.ToString())

        Return True
    End Function

    'Prints the result of Terminal Reading/DAILY SALES SUMMARY
    Private Function doPrintTXReading(ByVal sTrandate As String, ByVal sCRMNumbr As String) As Boolean
        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND sCashierx =  " & strParm(p_sCashier) &
                                      " AND cTranStat = '0'")

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            MsgBox("There are no transaction for this date(" & sTrandate & ").", , p_sMsgHeadr)
            Return False
        ElseIf loDta.Rows.Count > 1 Then
            MsgBox("Unclosed cashier shifts detected for the date(" & sTrandate & ").", , p_sMsgHeadr)
            Return False
        End If

        If Not OpenTransaction(sTrandate, sCRMNumbr, loDta(0).Item("sCashierx")) Then
            MsgBox("Can't open transaction of " & getCashier(loDta(0).Item("sCashierx")))
            Return False
        End If

        computeTotalCashierSales()

        If p_oDTMaster(0).Item("cTranStat") = "0" Then
            lsSQL = "SELECT dOpenedxx, dClosedxx, nAccuSale, nSChargex FROM Daily_Summary" &
                    " WHERE dClosedxx < " & datetimeParm(p_oDTMaster(0).Item("dOpenedxx")) &
                    " ORDER BY dOpenedxx DESC LIMIT 1"
            loDta = p_oApp.ExecuteQuery(lsSQL)

            Dim lnNetAmnt As Decimal = p_oDTMaster(0).Item("nSalesAmt") - (p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nPWDDiscx") + p_oDTMaster(0).Item("nVatDiscx"))
            If loDta.Rows.Count = 0 Then
                p_oDTMaster(0).Item("nAccuSale") = lnNetAmnt
            Else
                p_oDTMaster(0).Item("nAccuSale") = lnNetAmnt + loDta(0)("nAccuSale")
                'p_oDTMaster(0).Item("nSChargex") = p_oDTMaster(0).Item("nSChargex") + loDta(0)("nSChargex")
            End If
            loDta = Nothing

            lsSQL = "UPDATE Daily_Summary" &
                   " SET nSalesAmt = " & p_oDTMaster(0).Item("nSalesAmt") &
                      ", nVATSales = " & p_oDTMaster(0).Item("nVATSales") &
                      ", nVATAmtxx = " & p_oDTMaster(0).Item("nVATAmtxx") &
                      ", nNonVATxx = " & p_oDTMaster(0).Item("nNonVATxx") &
                      ", nZeroRatd = " & p_oDTMaster(0).Item("nZeroRatd") &
                      ", nDiscount = " & p_oDTMaster(0).Item("nDiscount") &
                      ", nVatDiscx = " & p_oDTMaster(0).Item("nVatDiscx") &
                      ", nPWDDiscx = " & p_oDTMaster(0).Item("nPWDDiscx") &
                      ", nReturnsx = " & p_oDTMaster(0).Item("nReturnsx") &
                      ", nVoidAmnt = " & p_oDTMaster(0).Item("nVoidAmnt") &
                      ", nCancelld = " & p_oDTMaster(0).Item("nCancelld") &
                      ", nAccuSale = " & p_oDTMaster(0).Item("nAccuSale") &
                      ", nCashAmnt = " & p_oDTMaster(0).Item("nCashAmnt") &
                      ", nSChargex = " & p_oDTMaster(0).Item("nSChargex") &
                      ", nChckAmnt = " & p_oDTMaster(0).Item("nChckAmnt") &
                      ", nCrdtAmnt = " & p_oDTMaster(0).Item("nCrdtAmnt") &
                      ", nChrgAmnt = " & p_oDTMaster(0).Item("nChrgAmnt") &
                      ", nGiftAmnt = " & p_oDTMaster(0).Item("nGiftAmnt") &
                      ", sORNoFrom = " & strParm(p_oDTMaster(0).Item("sORNoFrom")) &
                      ", sORNoThru = " & strParm(p_oDTMaster(0).Item("sORNoThru")) &
                      ", nZReadCtr = " & p_nZRdCtr &
                      ", dClosedxx = " & datetimeParm(p_oApp.SysDate) &
                      ", nVoidCntx = " & p_oDTMaster(0).Item("nVoidCntx") &
                   " WHERE sTranDate = " & strParm(p_oDTMaster(0).Item("sTranDate")) &
                     " AND sCRMNumbr = " & strParm(p_oDTMaster(0).Item("sCRMNumbr")) &
                     " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx"))

            p_oApp.Execute(lsSQL, p_sMasTable)
        End If

        'Reload data after recomputation of total sales
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) &
                                      " AND cTranStat <> '3'")

        loDta = p_oApp.ExecuteQuery(lsSQL)

        'iMac 2018.02.10
        'get previous day accumulated sale
        lsSQL = "SELECT nAccuSale FROM Daily_Summary" &
                " WHERE sTranDate < " & strParm(sTrandate) &
                    " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                    " AND cTranStat IN ('1', '2')" &
                " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
        End If


        '        Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        'builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        'p_sCompny = "The Monarch Hospitality & Tourism Corp."
        builder.Append(PadCenter(Trim(p_sCompny), 20) & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense

        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)

        'p_sVATReg = "469-083-682-002"
        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        'p_sPOSNo = "22010313392685363"
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        'p_sSerial = "WCC6Y5NUS72X"
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("X-READING" & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Get the transaction date thru reverse formatting the sTrandate field
        Dim lsTranDate As String
        lsTranDate = p_oDTMaster(0).Item("sTranDate")
        lsTranDate = Left(lsTranDate, 4) & "-" & Mid(lsTranDate, 5, 2) & "-" & Right(lsTranDate, 2)

        builder.Append(Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append("DATE      :" & Format(CDate(lsTranDate), "dd-MMM-yyyy") & Environment.NewLine)
        builder.Append("CASHIER   :" & getCashier(p_sCashier) & Environment.NewLine)
        builder.Append("TERMINAL  :" & p_sTermnl & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim lnOpenBalx As Decimal = 0
        Dim lnCPullOut As Decimal = 0

        Dim lnCashAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0   'Non-Vat means Vat Exempt
        Dim lnDiscount As Decimal = 0   'Regular Discount
        Dim lnVatDiscx As Decimal = 0   '12% VAT Discount
        Dim lnPWDDiscx As Decimal = 0   'Senior/PWD Discount

        Dim lnReturnsx As Decimal = 0   'Returns
        Dim lnVoidAmnt As Decimal = 0   'Void Transactions
        Dim lnVoidCntx As Integer = 0

        Dim lsORNoFrom As String = loDta(0).Item("sORNoFrom")
        Dim lsORNoThru As String = loDta(0).Item("sORNoThru")

        Dim ldStartShft As Date = loDta(0).Item("dOpenedxx")
        Dim ldEndedSfht As Date = loDta(0).Item("dClosedxx")

        'Compute Gross Sales
        lnSalesAmt = loDta(0).Item("nSalesAmt")

        'Compute VAT Related Sales
        lnVATSales = loDta(0).Item("nVATSales")
        lnVATAmtxx = loDta(0).Item("nVATAmtxx")
        lnZeroRatd = loDta(0).Item("nZeroRatd")

        'Compute Discounts
        lnDiscount = loDta(0).Item("nDiscount")
        lnVatDiscx = loDta(0).Item("nVatDiscx")
        lnPWDDiscx = loDta(0).Item("nPWDDiscx")

        'Compute Returns/Refunds/Void Transactions
        lnReturnsx = loDta(0).Item("nReturnsx")
        lnVoidAmnt = loDta(0).Item("nVoidAmnt")
        lnVoidCntx = loDta(0).Item("nVoidCntx")

        'Compute Cashier Collection Info
        lnOpenBalx = loDta(0).Item("nOpenBalx")
        lnCPullOut = loDta(0).Item("nCPullOut")

        lnCashAmnt = loDta(0).Item("nCashAmnt")
        lnSChargex = loDta(0).Item("nSChargex")
        lnChckAmnt = loDta(0).Item("nChckAmnt")
        lnCrdtAmnt = loDta(0).Item("nCrdtAmnt")
        lnChrgAmnt = loDta(0).Item("nChrgAmnt")
        lnGiftAmnt = loDta(0).Item("nGiftAmnt")

        'Compute for VAT Exempt Sales
        'lnNonVATxx = (lnSalesAmt + lnSChargex) - (lnVATSales + lnZeroRatd + lnVATAmtxx + lnPWDDiscx + lnVatDiscx + lnDiscount)
        'lnNonVATxx = loDta(0).Item("nNonVATxx") + lnPWDDiscx
        lnNonVATxx = loDta(0).Item("nNonVATxx")

        builder.Append(Environment.NewLine)
        builder.Append(" Shift Start   :  " & Format(ldStartShft, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)
        builder.Append(" Shift End     :  " & Format(ldEndedSfht, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine & Environment.NewLine)

        'Print the begging and ending OR
        builder.Append(" Beginning SI No.:  " & lsORNoFrom & Environment.NewLine)
        builder.Append(" Ending SI No.   :  " & lsORNoThru & Environment.NewLine)

        'builder.Append(" Beginning Balance  : ".PadRight(24) & Format(lnPrevSale, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Print the Computation of NET Sales
        'builder.Append(Environment.NewLine)
        'If lnSChargex > 0 Then
        '    builder.Append(" SERVICE CHARGE".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'End If

        builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex + lnDiscount + lnVatDiscx + lnPWDDiscx + lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        If lnSChargex > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If

        builder.Append("        VAT SC/PWD".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_EMP1)
        builder.Append(" NET SALES".PadRight(24) & Format(lnSalesAmt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" NET SALES".PadRight(24) & Format(lnSalesAmt - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(Environment.NewLine)

        builder.Append(" VATABLE Sales".PadRight(24) & Format(lnVATSales, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(lnVATAmtxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        '        builder.Append(Space(pxeLFTMGN) & " VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ZERO Rated Sales".PadRight(24) & Format(lnZeroRatd, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        builder.Append(Environment.NewLine)

        ''builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx + lnVatDiscx + lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("     Less: 20% Discount:".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("           Less 12% VAT:".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        'builder.Append(Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Petty Cash".PadRight(24) & Format(lnOpenBalx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(lnCPullOut, xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        'builder.Append("  Cashbox Amount".PadRight(24) & Format(lnOpenBalx + (lnCashAmnt + lnSChargex) - lnCPullOut - lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cash".PadRight(24) & Format((lnOpenBalx + lnCashAmnt) - (lnCPullOut + lnReturnsx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(lnChckAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(lnCrdtAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(lnGiftAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(lnChrgAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP1)
        'builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)
        'builder.Append("        Z-Counter  : ".PadRight(24) & loDta(0).Item("nZReadCtr").ToString.PadLeft(13) & Environment.NewLine)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" Void SI Count: ".PadRight(24) & Format(lnVoidCntx, xsINTEGER).PadLeft(13) & Environment.NewLine)
        builder.Append(" Void SI Amount: ".PadRight(24) & Format(lnVoidAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Extract accumulated total sales...
        lsSQL = "SELECT" &
                       "  nVATSales" &
                       ", nVATAmtxx" &
                       ", nNonVATxx" &
                       ", nZeroRatd" &
                       ", nSalesTot" &
                       ", sIDNumber" &
                       ", nSChargex" &
                       ", nSChrgAmt" &
               " FROM Cash_Reg_Machine" &
               " WHERE sIDNumber = " & strParm(p_sPOSNo)
        loDta = p_oApp.ExecuteQuery(lsSQL)

        'kalyptus - 2016.12.14 11:09am 
        'UPDATE Cash Register machine info if the daily cash register is not yet posted...
        If p_oDTMaster(0).Item("cTranStat") = "0" Then
            loDta(0).Item("nSalesTot") += lnSalesAmt
            loDta(0).Item("nVATSales") += lnVATSales
            loDta(0).Item("nVATAmtxx") += lnVATAmtxx
            loDta(0).Item("nNonVATxx") += lnNonVATxx
            loDta(0).Item("nZeroRatd") += lnZeroRatd
            loDta(0).Item("nSChrgAmt") += lnSChargex

            lsSQL = "UPDATE Cash_Reg_Machine" &
                   " SET nVATSales = nVATSales + " & lnVATSales &
                      ", nVATAmtxx = nVATAmtxx + " & lnVATAmtxx &
                      ", nNonVATxx = nNonVATxx + " & lnNonVATxx &
                      ", nZeroRatd = nZeroRatd + " & lnZeroRatd &
                      ", nSalesTot = nSalesTot + " & lnSalesAmt &
                      ", nSChrgAmt = nSChrgAmt + " & lnSChargex &
                      ", sORNoxxxx = " & strParm(lsORNoThru) &
                   " WHERE sIDNumber = " & strParm(p_sPOSNo)
            p_oApp.Execute(lsSQL, "Cash_Reg_Machine")
        End If

        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        Debug.Print(Format(Convert.ToDateTime(Mid(sTrandate, 5, 2) + "/" + Mid(sTrandate, 1, 4) + "/" + Mid(sTrandate, 7, 2) + " " + Format(p_oApp.getSysDate(), "hh:mm:ss")), "dd/MMM/yyyy hh:mm:ss"))
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate(), "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)
        'builder.Append("/end-of-summary - " & Format(Convert.ToDateTime(Mid(sTrandate, 5, 2) + "/" + Mid(sTrandate, 1, 4) + "/" + Mid(sTrandate, 7, 2) + " " + Format(p_oApp.getSysDate(), "hh:mm:ss")), "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")
        'Dim cashier_printer As String = "\\192.168.10.12\EPSON TM-U220 Receipt"

        'Print the designation printer location...
        RawPrint.SendStringToPrinter(cashier_printer, builder.ToString())

        Return True
    End Function

    Public Function doPrintTXReadingReg(ByVal sTrandate As String,
                                         ByVal sCRMNumbr As String,
                                         ByVal sCashierx As String) As Boolean
        Dim lsSQL As String
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) &
                                      " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                      " AND sCashierx =  " & strParm(sCashierx))
        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If Not OpenTransaction(sTrandate, sCRMNumbr, loDta(0).Item("sCashierx")) Then
            MsgBox("Can't open transaction of " & getCashier(loDta(0).Item("sCashierx")))
            Return False
        End If

        computeTotalCashierSales()

        lsSQL = "SELECT dOpenedxx, dClosedxx, nAccuSale, nSChargex FROM Daily_Summary" &
                " WHERE dClosedxx < " & datetimeParm(p_oDTMaster(0).Item("dOpenedxx")) &
                " ORDER BY dOpenedxx DESC LIMIT 1"
        loDta = p_oApp.ExecuteQuery(lsSQL)

        Dim lnNetAmnt As Decimal = p_oDTMaster(0).Item("nSalesAmt") - (p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nPWDDiscx") + p_oDTMaster(0).Item("nVatDiscx"))
        If loDta.Rows.Count = 0 Then
            p_oDTMaster(0).Item("nAccuSale") = lnNetAmnt
        Else
            p_oDTMaster(0).Item("nAccuSale") = lnNetAmnt + loDta(0)("nAccuSale")
            'p_oDTMaster(0).Item("nSChargex") = p_oDTMaster(0).Item("nSChargex") + loDta(0)("nSChargex")
        End If
        loDta = Nothing

        lsSQL = "UPDATE Daily_Summary" &
                " SET nSalesAmt = " & p_oDTMaster(0).Item("nSalesAmt") &
                    ", nVATSales = " & p_oDTMaster(0).Item("nVATSales") &
                    ", nVATAmtxx = " & p_oDTMaster(0).Item("nVATAmtxx") &
                    ", nNonVATxx = " & p_oDTMaster(0).Item("nNonVATxx") &
                    ", nZeroRatd = " & p_oDTMaster(0).Item("nZeroRatd") &
                    ", nDiscount = " & p_oDTMaster(0).Item("nDiscount") &
                    ", nVatDiscx = " & p_oDTMaster(0).Item("nVatDiscx") &
                    ", nPWDDiscx = " & p_oDTMaster(0).Item("nPWDDiscx") &
                    ", nReturnsx = " & p_oDTMaster(0).Item("nReturnsx") &
                    ", nVoidAmnt = " & p_oDTMaster(0).Item("nVoidAmnt") &
                    ", nCancelld = " & p_oDTMaster(0).Item("nCancelld") &
                    ", nAccuSale = " & p_oDTMaster(0).Item("nAccuSale") &
                    ", nCashAmnt = " & p_oDTMaster(0).Item("nCashAmnt") &
                    ", nSChargex = " & p_oDTMaster(0).Item("nSChargex") &
                    ", nChckAmnt = " & p_oDTMaster(0).Item("nChckAmnt") &
                    ", nCrdtAmnt = " & p_oDTMaster(0).Item("nCrdtAmnt") &
                    ", nChrgAmnt = " & p_oDTMaster(0).Item("nChrgAmnt") &
                    ", nGiftAmnt = " & p_oDTMaster(0).Item("nGiftAmnt") &
                    ", nVoidCntx = " & p_oDTMaster(0).Item("nVoidCntx") &
                " WHERE sTranDate = " & strParm(p_oDTMaster(0).Item("sTranDate")) &
                    " AND sCRMNumbr = " & strParm(p_oDTMaster(0).Item("sCRMNumbr")) &
                    " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx"))

        p_oApp.Execute(lsSQL, p_sMasTable)

        'Reload data after recomputation of total sales
        lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) &
                                    " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                                    " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) &
                                    " AND cTranStat <> '3'")
        loDta = p_oApp.ExecuteQuery(lsSQL)

        'iMac 2018.02.10
        'get previous day accumulated sale
        lsSQL = "SELECT nAccuSale FROM Daily_Summary" &
                " WHERE sTranDate < " & strParm(sTrandate) &
                    " AND sCRMNumbr = " & strParm(sCRMNumbr) &
                    " AND cTranStat IN ('1', '2')" &
                " ORDER BY dClosedxx DESC LIMIT 1"

        Dim loDT As DataTable
        Dim lnPrevSale As Decimal
        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnPrevSale = 0
        Else
            lnPrevSale = loDT(0)("nAccuSale")
        End If

        '        Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        'builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        p_sCompny = "The Monarch Hospitality & Tourism Corp."
        builder.Append(PadCenter(Trim(p_sCompny), 20) & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense

        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)

        p_sVATReg = "469-083-682-002"
        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        p_sPOSNo = "21072616181662943"
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        p_sSerial = "WCC6Y4VEA7V0"
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLH + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("X-READING" & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(RawPrint.pxePRINT_LEFT)

        'Get the transaction date thru reverse formatting the sTrandate field
        Dim lsTranDate As String
        lsTranDate = p_oDTMaster(0).Item("sTranDate")
        lsTranDate = Left(lsTranDate, 4) & "-" & Mid(lsTranDate, 5, 2) & "-" & Right(lsTranDate, 2)

        builder.Append(Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append("DATE      :" & Format(CDate(lsTranDate), "dd-MMM-yyyy") & Environment.NewLine)
        builder.Append("CASHIER   :" & getCashier(p_oDTMaster(0).Item("sCashierx")) & Environment.NewLine)
        builder.Append("TERMINAL  :" & p_sTermnl & Environment.NewLine)
        'builder.Append("TERMINAL #:" & p_sSerial & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        'Print Asterisk(*)
        builder.Append(Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        Dim lnOpenBalx As Decimal = 0
        Dim lnCPullOut As Decimal = 0

        Dim lnCashAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0   'Non-Vat means Vat Exempt
        Dim lnDiscount As Decimal = 0   'Regular Discount
        Dim lnVatDiscx As Decimal = 0   '12% VAT Discount
        Dim lnPWDDiscx As Decimal = 0   'Senior/PWD Discount

        Dim lnReturnsx As Decimal = 0   'Returns
        Dim lnVoidAmnt As Decimal = 0   'Void Transactions
        Dim lnVoidCntx As Integer = 0

        Dim lsORNoFrom As String = loDta(0).Item("sORNoFrom")
        Dim lsORNoThru As String = loDta(0).Item("sORNoThru")

        Dim ldStartShft As Date = loDta(0).Item("dOpenedxx")
        Dim ldEndedSfht As Date = loDta(0).Item("dClosedxx")

        'Compute Gross Sales
        lnSalesAmt = loDta(0).Item("nSalesAmt")

        'Compute VAT Related Sales
        lnVATSales = loDta(0).Item("nVATSales")
        lnVATAmtxx = loDta(0).Item("nVATAmtxx")
        lnZeroRatd = loDta(0).Item("nZeroRatd")

        'Compute Discounts
        lnDiscount = loDta(0).Item("nDiscount")
        lnVatDiscx = loDta(0).Item("nVatDiscx")
        lnPWDDiscx = loDta(0).Item("nPWDDiscx")

        'Compute Returns/Refunds/Void Transactions
        lnReturnsx = loDta(0).Item("nReturnsx")
        lnVoidAmnt = loDta(0).Item("nVoidAmnt")
        lnVoidCntx = loDta(0).Item("nVoidCntx")

        'Compute Cashier Collection Info
        lnOpenBalx = loDta(0).Item("nOpenBalx")
        lnCPullOut = loDta(0).Item("nCPullOut")

        lnCashAmnt = loDta(0).Item("nCashAmnt")
        lnSChargex = loDta(0).Item("nSChargex")
        lnChckAmnt = loDta(0).Item("nChckAmnt")
        lnCrdtAmnt = loDta(0).Item("nCrdtAmnt")
        lnChrgAmnt = loDta(0).Item("nChrgAmnt")
        lnGiftAmnt = loDta(0).Item("nGiftAmnt")

        'Compute for VAT Exempt Sales
        'lnNonVATxx = (lnSalesAmt + lnSChargex) - (lnVATSales + lnZeroRatd + lnVATAmtxx + lnPWDDiscx + lnVatDiscx + lnDiscount)
        'lnNonVATxx = loDta(0).Item("nNonVATxx") + lnPWDDiscx
        lnNonVATxx = loDta(0).Item("nNonVATxx")

        builder.Append(Environment.NewLine)
        builder.Append(" Shift Start   :  " & Format(ldStartShft, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)
        builder.Append(" Shift End     :  " & Format(ldEndedSfht, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine & Environment.NewLine)

        'Print the begging and ending OR
        builder.Append(" Beginning SI No.:  " & lsORNoFrom & Environment.NewLine)
        builder.Append(" Ending SI No.   :  " & lsORNoThru & Environment.NewLine)

        'builder.Append(" Beginning Balance  : ".PadRight(24) & Format(lnPrevSale, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("    Ending Balance  : ".PadRight(24) & Format(lnPrevSale + ((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx)), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Print the Computation of NET Sales
        'builder.Append(Environment.NewLine)
        'If lnSChargex > 0 Then
        '    builder.Append(" SERVICE CHARGE".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'End If

        builder.Append(" GROSS SALES".PadRight(24) & Format(lnSalesAmt + lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        If lnSChargex > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(lnSChargex, xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(lnDiscount, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If

        builder.Append("        VAT SC/PWD".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_EMP1)
        'builder.Append(" NET SALES".PadRight(24) & Format((lnSalesAmt + lnSCharge) - (lnDiscount + lnPWDDiscx + lnVatDiscx + lnSChargex), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" NET SALES".PadRight(24) & Format(lnSalesAmt - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(Environment.NewLine)

        builder.Append(" VATABLE Sales".PadRight(24) & Format(lnVATSales, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(lnVATAmtxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        '        builder.Append(Space(pxeLFTMGN) & " VAT Exempt Sales".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" ZERO Rated Sales".PadRight(24) & Format(lnZeroRatd, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        builder.Append(Environment.NewLine)

        ''builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        ''builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx - (lnVatDiscx + lnPWDDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(lnNonVATxx + lnVatDiscx + lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(lnNonVATxx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("     Less: 20% Discount:".PadRight(24) & Format(lnPWDDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("           Less 12% VAT:".PadRight(24) & Format(lnVatDiscx, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        'builder.Append(Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Petty Cash".PadRight(24) & Format(lnOpenBalx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(lnCPullOut, xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        'builder.Append("  Cashbox Amount".PadRight(24) & Format(lnOpenBalx + (lnCashAmnt + lnSChargex) - lnCPullOut - lnReturnsx, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cash".PadRight(24) & Format((lnOpenBalx + lnCashAmnt) - (lnCPullOut + lnReturnsx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(lnChckAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(lnCrdtAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(lnGiftAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(lnChrgAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP1)
        'builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format((lnSalesAmt + lnSChargex) - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format((lnSalesAmt) - (lnDiscount + lnPWDDiscx + lnVatDiscx), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)
        'builder.Append("        Z-Counter  : ".PadRight(24) & loDta(0).Item("nZReadCtr").ToString.PadLeft(13) & Environment.NewLine)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append(" Void SI Count: ".PadRight(24) & Format(lnVoidCntx, xsINTEGER).PadLeft(13) & Environment.NewLine)
        builder.Append(" Void SI Amount: ".PadRight(24) & Format(lnVoidAmnt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Extract accumulated total sales...
        lsSQL = "SELECT" &
                       "  nVATSales" &
                       ", nVATAmtxx" &
                       ", nNonVATxx" &
                       ", nZeroRatd" &
                       ", nSalesTot" &
                       ", sIDNumber" &
                       ", nSChargex" &
                       ", nSChrgAmt" &
               " FROM Cash_Reg_Machine" &
               " WHERE sIDNumber = " & strParm(p_sPOSNo)

        loDta = p_oApp.ExecuteQuery(lsSQL)

        'kalyptus - 2016.12.14 11:09am 
        'UPDATE Cash Register machine info if the daily cash register is not yet posted...
        'If p_oDTMaster(0).Item("cTranStat") = "0" Then
            loDta(0).Item("nSalesTot") += lnSalesAmt
            loDta(0).Item("nVATSales") += lnVATSales
            loDta(0).Item("nVATAmtxx") += lnVATAmtxx
            loDta(0).Item("nNonVATxx") += lnNonVATxx
            loDta(0).Item("nZeroRatd") += lnZeroRatd
            loDta(0).Item("nSChrgAmt") += lnSChargex

            lsSQL = "UPDATE Cash_Reg_Machine" &
                   " SET nVATSales = nVATSales + " & lnVATSales &
                      ", nVATAmtxx = nVATAmtxx + " & lnVATAmtxx &
                      ", nNonVATxx = nNonVATxx + " & lnNonVATxx &
                      ", nZeroRatd = nZeroRatd + " & lnZeroRatd &
                      ", nSalesTot = nSalesTot + " & lnSalesAmt &
                      ", nSChrgAmt = nSChrgAmt + " & lnSChargex &
                      ", sORNoxxxx = " & strParm(lsORNoThru) &
                   " WHERE sIDNumber = " & strParm(p_sPOSNo)
            p_oApp.Execute(lsSQL, "Cash_Reg_Machine")
        'End If

        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        Debug.Print(Format(Convert.ToDateTime(Mid(sTrandate, 5, 2) + "/" + Mid(sTrandate, 1, 4) + "/" + Mid(sTrandate, 7, 2) + " " + Format(p_oApp.getSysDate(), "hh:mm:ss")), "dd/MMM/yyyy hh:mm:ss"))
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate(), "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)
        'builder.Append("/end-of-summary - " & Format(Convert.ToDateTime(Mid(sTrandate, 5, 2) + "/" + Mid(sTrandate, 1, 4) + "/" + Mid(sTrandate, 7, 2) + " " + Format(p_oApp.getSysDate(), "hh:mm:ss")), "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")
        'Dim cashier_printer As String = "\\192.168.10.12\EPSON TM-U220 Receipt"


        'Print the designation printer location...
        RawPrint.SendStringToPrinter(cashier_printer, builder.ToString())

        Return True
    End Function

    'Private Function doPrintEZReading(ByVal sTrandate As String, ByVal sCRMNumbr As String) As Boolean

    '    Dim lsSQL As String
    '    lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) & _
    '                                  " AND sCRMNumbr = " & strParm(sCRMNumbr) & _
    '                                  " AND cTranStat = '0'")

    '    Dim loDta As DataTable

    '    loDta = p_oApp.ExecuteQuery(lsSQL)

    '    If loDta.Rows.Count = 0 Then
    '        MsgBox("There are no transaction for this date....", , p_sMsgHeadr)
    '        Return False
    '    End If

    '    'Recompute the TOTAL CASHIER SALES of each unposted EZ Reading...
    '    Dim lnCtr As Integer
    '    For lnCtr = 0 To loDta.Rows.Count - 1
    '        If Not OpenTransaction(sTrandate, sCRMNumbr, loDta(lnCtr).Item("sCashierx")) Then
    '            MsgBox("Can't open transaction of " & getCashier(loDta(lnCtr).Item("sCashierx")))
    '            Return False
    '        End If

    '        computeTotalCashierSales()

    '        lsSQL = "UPDATE Daily_Summary" & _
    '               " SET nSalesAmt = " & p_oDTMaster(0).Item("nSalesAmt") & _
    '                  ", nVATSales = " & p_oDTMaster(0).Item("nVATSales") & _
    '                  ", nVATAmtxx = " & p_oDTMaster(0).Item("nVATAmtxx") & _
    '                  ", nNonVATxx = " & p_oDTMaster(0).Item("nNonVATxx") & _
    '                  ", nZeroRatd = " & p_oDTMaster(0).Item("nZeroRatd") & _
    '                  ", nDiscount = " & p_oDTMaster(0).Item("nDiscount") & _
    '               " WHERE sTranDate = " & strParm(p_oDTMaster(0).Item("sTranDate")) & _
    '                 " AND sCRMNumbr = " & strParm(p_oDTMaster(0).Item("sCRMNumbr")) & _
    '                 " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx"))
    '        p_oApp.Execute(lsSQL, p_sMasTable)

    '    Next

    '    'Reload data after recomputation of total sales
    '    lsSQL = AddCondition(getSQ_Master, "sTranDate = " & strParm(sTrandate) & _
    '                                  " AND sCRMNumbr = " & strParm(sCRMNumbr) & _
    '                                  " AND cTranStat <> '3'")
    '    loDta = p_oApp.ExecuteQuery(lsSQL)

    '    Dim loPrint As ggcLRReports.clsDirectPrintSF
    '    loPrint = New ggcLRReports.clsDirectPrintSF

    '    'loPrint.PrintFont = New Font("Courier New", 10)
    '    loPrint.PrintBegin()

    '    Dim lnRowCtr As Integer = 0

    '    'Print the header
    '    loPrint.Print(lnRowCtr, 0, PadCenter("MONARK HOTEL", 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("PEDRITO'S BAKESHOP AND RESTAURANT", 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("Mc Arthur Highway, Tapuac District", 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("Dagupan City, Pangasinan", 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("VAT REG TIN: " & p_sVATReg, 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("MIN : " & p_sPOSNo, 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("Permit #: " & p_sPermit, 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("Serial No. : " & p_sSerial, 40))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Dash Separator(-)
    '    loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, PadCenter("DAILY SALES SUMMARY", 40))
    '    lnRowCtr = lnRowCtr + 2

    '    loPrint.Print(lnRowCtr, 0, "VAT SALES".PadLeft(11) & "VAT AMT".PadLeft(11) & "NON VAT".PadLeft(9) & "ZERO RATD".PadLeft(9))
    '    lnRowCtr = lnRowCtr + 1

    '    Dim lnSalesAmt As Decimal = 0
    '    Dim lnVATSales As Decimal = 0
    '    Dim lnVATAmtxx As Decimal = 0
    '    Dim lnZeroRatd As Decimal = 0
    '    Dim lnNonVATxx As Decimal = 0
    '    Dim lnDiscount As Decimal = 0
    '    Dim lnNonVatxy As Decimal = 0

    '    For lnCtr = 0 To loDta.Rows.Count - 1
    '        loPrint.Print(lnRowCtr, 0, getCashier(loDta(lnCtr).Item("sCashierx")))
    '        lnRowCtr = lnRowCtr + 1

    '        lnNonVatxy = loDta(lnCtr).Item("nSalesAmt") - (loDta(lnCtr).Item("nVATSales") + loDta(lnCtr).Item("nVATAmtxx") + loDta(lnCtr).Item("nZeroRatd"))

    '        loPrint.Print(lnRowCtr, 0, Format(loDta(lnCtr).Item("nVATSales"), xsDECIMAL).PadLeft(11) & _
    '                                   Format(loDta(lnCtr).Item("nVATAmtxx"), xsDECIMAL).PadLeft(11) & _
    '                                   Format(lnNonVatxy, xsDECIMAL).PadLeft(9) & _
    '                                   Format(loDta(lnCtr).Item("nZeroRatd"), xsDECIMAL).PadLeft(9))
    '        lnRowCtr = lnRowCtr + 1

    '        lnSalesAmt = lnSalesAmt + loDta(lnCtr).Item("nSalesAmt")
    '        lnVATSales = lnVATSales + loDta(lnCtr).Item("nVATSales")
    '        lnVATAmtxx = lnVATAmtxx + loDta(lnCtr).Item("nVATAmtxx")
    '        lnZeroRatd = lnZeroRatd + loDta(lnCtr).Item("nZeroRatd")
    '        lnDiscount = lnDiscount + loDta(lnCtr).Item("nDiscount")
    '    Next

    '    lnNonVATxx = lnSalesAmt - (lnVATSales + lnZeroRatd + lnVATAmtxx)

    '    loPrint.Print(lnRowCtr, 0, "-".PadLeft(40, "-"))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, Format(lnVATSales, xsDECIMAL).PadLeft(11) & _
    '                               Format(lnVATAmtxx, xsDECIMAL).PadLeft(11) & _
    '                               Format(lnNonVatxy, xsDECIMAL).PadLeft(9) & _
    '                               Format(lnZeroRatd, xsDECIMAL).PadLeft(9))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
    '    lnRowCtr = lnRowCtr + 1

    '    Return True
    'End Function


    'Prints CASHIER DAILY SALES/DAILY SALES REPORT 
    'MAC 2018.01.29
    Private Function doPrintCashierSales() As Boolean
        If Not initMachine() Then
            Return False
        End If

        Dim lsTranDate As String
        lsTranDate = p_oDTMaster(0).Item("sTranDate")
        lsTranDate = Left(lsTranDate, 4) & "-" & Mid(lsTranDate, 5, 2) & "-" & Right(lsTranDate, 2)

        If p_oDTMaster(0).Item("cTranStat") = "0" Then
            Call computeTotalCashierSales()
        End If

        'Compute for total 

        '        Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        'builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append(PadCenter(Trim(p_sCompny), 20) & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)
        builder.Append(PadCenter("VAT REG TIN: " & p_sVATReg, 40) & Environment.NewLine)
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine)
        builder.Append(PadCenter("PTU No.: " & p_sPermit, 40) & Environment.NewLine)
        builder.Append(PadCenter("Serial No. : " & p_sSerial, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append("Y-READING" & Environment.NewLine & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense

        builder.Append(RawPrint.pxePRINT_LEFT)
        builder.Append("DATE: " & Format(CDate(lsTranDate), "dd/MMM/yyyy") & Environment.NewLine)
        builder.Append("CASHIER: " & getCashier(Decrypt(p_oApp.UserName, "08220326")) & Environment.NewLine & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        builder.Append("TERMINAL #: " & p_sTermnl & Environment.NewLine & Environment.NewLine)

        builder.Append(" Beginning SI  :  " & p_oDTMaster(0).Item("sORNoFrom") & Environment.NewLine)
        builder.Append(" Ending SI     :  " & p_oDTMaster(0).Item("sORNoThru") & Environment.NewLine & Environment.NewLine)

        'Print the Computation of NET Sales
        builder.Append(" GROSS SALES".PadRight(24) & Format(p_oDTMaster(0).Item("nSalesAmt") +
                                                            p_oDTMaster(0).Item("nSChargex") +
                                                            p_oDTMaster(0).Item("nDiscount") +
                                                            p_oDTMaster(0).Item("nVatDiscx") +
                                                            p_oDTMaster(0).Item("nPWDDiscx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        If p_oDTMaster(0).Item("nSChargex") > 0 Then
            builder.Append(" Less : Service Charge".PadRight(24) & Format(p_oDTMaster(0).Item("nSChargex"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
            builder.Append("        Regular Discnt".PadRight(24) & Format(p_oDTMaster(0).Item("nDiscount"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        Else
            builder.Append(" Less : Regular Discnt".PadRight(24) & Format(p_oDTMaster(0).Item("nDiscount"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        End If
        builder.Append("        VAT SC/PWD".PadRight(24) & Format(p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        20% SC/PWD Disc.".PadRight(24) & Format(p_oDTMaster(0).Item("nPWDDiscx") + p_oDTMaster(0).Item("nPWDDiscx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Returns".PadRight(24) & Format(p_oDTMaster(0).Item("nReturnsx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("        Void".PadRight(24) & Format(p_oDTMaster(0).Item("nVoidAmnt"), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append(" ".PadRight(24) & "-".PadLeft(13, "-") & Environment.NewLine)
        'builder.Append(Space(pxeLFTMGN) & " NET SALES".PadRight(24) & Format(p_oDTMaster(0).Item("nSalesAmt") - (p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nPWDDiscx") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nReturnsx") + p_oDTMaster(0).Item("nVoidAmnt")), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" NET SALES".PadRight(24) & Format(p_oDTMaster(0).Item("nSalesAmt"), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'Display a space in between NEW Sales and VAT Related Info
        builder.Append(Environment.NewLine)

        builder.Append(" VATable Sales".PadRight(24) & Format(p_oDTMaster(0).Item("nVATSales"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT Amount".PadRight(24) & Format(p_oDTMaster(0).Item("nVATAmtxx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(Space(pxeLFTMGN) & " VAT Exempt Sales".PadRight(24) & Format(p_oDTMaster(0).Item("nNonVATxx") - (p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" VAT-Exempt Sales".PadRight(24) & Format(p_oDTMaster(0).Item("nNonVATxx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" Zero-Rated Sales".PadRight(24) & Format(p_oDTMaster(0).Item("nZeroRatd"), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between VAT Related Info and SENIOR/PWD Discount Info
        'builder.Append(Environment.NewLine)

        'builder.Append(" Senior/PWD Gross Sales:".PadRight(24) & Format(p_oDTMaster(0).Item("nNonVATxx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("   Senior/PWD Net Sales:".PadRight(24) & Format(p_oDTMaster(0).Item("nNonVATxx") - (p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("     Less: 20% Discount:".PadRight(24) & Format(p_oDTMaster(0).Item("nPWDDiscx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("           Less 12% VAT:".PadRight(24) & Format(p_oDTMaster(0).Item("nVatDiscx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        ''Display a space in between SENIOR/PWD Discount Info & Collection Info
        'builder.Append(Environment.NewLine)

        builder.Append(" Collection Info:" & Environment.NewLine)
        builder.Append("  Deposit".PadRight(24) & Format(p_oDTMaster(0).Item("nOpenBalx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Withdrawal".PadRight(24) & Format(p_oDTMaster(0).Item("nCPullOut"), xsDECIMAL).PadLeft(13) & Environment.NewLine & Environment.NewLine)

        builder.Append("  Cashbox Amount".PadRight(24) & Format(p_oDTMaster(0).Item("nOpenBalx") + p_oDTMaster(0).Item("nCashAmnt") - p_oDTMaster(0).Item("nCPullOut") - p_oDTMaster(0).Item("nReturnsx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Cheque".PadRight(24) & Format(p_oDTMaster(0).Item("nChckAmnt"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Credit Card".PadRight(24) & Format(p_oDTMaster(0).Item("nCrdtAmnt"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append("  Gift Cheque".PadRight(24) & Format(p_oDTMaster(0).Item("nGiftAmnt"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append("  Company Accounts".PadRight(24) & Format(p_oDTMaster(0).Item("nChrgAmnt"), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        'builder.Append(Space(pxeLFTMGN) & " Curr. Sales Amt.  : ".PadRight(24) & Format(p_oDTMaster(0).Item("nSalesAmt") - (p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nPWDDiscx") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nReturnsx") + p_oDTMaster(0).Item("nVoidAmnt")), xsDECIMAL).PadLeft(13) & Environment.NewLine)

        Dim lnSalesAmt As Decimal = p_oDTMaster(0).Item("nSalesAmt") - (p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nPWDDiscx") + p_oDTMaster(0).Item("nVatDiscx"))

        builder.Append(" Beginning Balance : ".PadRight(24) & Format(p_oDTMaster(0).Item("nOpenBalx"), xsDECIMAL).PadLeft(13) & Environment.NewLine)
        builder.Append(" Ending Balance    : ".PadRight(24) & Format(p_oDTMaster(0).Item("nOpenBalx") + lnSalesAmt, xsDECIMAL).PadLeft(13) & Environment.NewLine)

        'builder.Append(" Curr. Sales Amt.  : ".PadRight(24) & Format(lnSalesAmt, xsDECIMAL).PadLeft(13) & Environment.NewLine)
        'builder.Append(" Reset Counter.    : ".PadRight(24) & p_oDTMaster(0).Item("nZReadCtr") & Environment.NewLine)
        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)

        'Print Dash Separator(-)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))

        'Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim cashier_printer As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")

        'Print the designation printer location...
        RawPrint.SendStringToPrinter(cashier_printer, builder.ToString())

        Return True
    End Function

    'Private Function doPrintCashierSales() As Boolean
    '    If Not initMachine() Then
    '        Return False
    '    End If

    '    Dim lsTranDate As String
    '    lsTranDate = p_oDTMaster(0).Item("sTranDate")
    '    lsTranDate = Left(lsTranDate, 4) & "-" & Mid(lsTranDate, 5, 2) & "-" & Right(lsTranDate, 2)

    '    Call computeTotalCashierSales()

    '    Dim loPrint As ggcLRReports.clsDirectPrintSF
    '    loPrint = New ggcLRReports.clsDirectPrintSF
    '    'loPrint.PrintFont = New Font("Courier New", 10)
    '    loPrint.PrintBegin()

    '    Dim lnRowCtr As Integer = 0

    '    'Print the header
    '    loPrint.Print(lnRowCtr, 0, PadCenter("MONARK HOTEL", 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("PEDRITO'S BAKESHOP AND RESTAURANT", 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("Mc Arthur Highway, Tapuac District", 40))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, PadCenter("Dagupan City, Pangasinan", 40))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Dash Separator(-)
    '    loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, PadCenter("DAILY CASHIER SALES REPORT", 40))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "CASHIER: " & getCashier(p_oDTMaster(0).Item("sCashierx")))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "DATE: " & lsTranDate)
    '    lnRowCtr = lnRowCtr + 2

    '    loPrint.Print(lnRowCtr, 0, "GROSS SALES       : " & Format(p_oDTMaster(0).Item("nSalesAmt") + p_oDTMaster(0).Item("nDiscount"), xsDECIMAL).PadLeft(11))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "LESS: DISCOUNT    : " & Format(p_oDTMaster(0).Item("nDiscount"), xsDECIMAL).PadLeft(11))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "                    " & "-----------")
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "NET SALES         : " & Format(p_oDTMaster(0).Item("nSalesAmt"), xsDECIMAL).PadLeft(11))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "ADD : OPEN BAL    : " & Format(p_oDTMaster(0).Item("nOpenBalx"), xsDECIMAL).PadLeft(11))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "LESS: PULL OUT    : " & Format(p_oDTMaster(0).Item("nCPullOut"), xsDECIMAL).PadLeft(11))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "                    " & "-----------")
    '    loPrint.Print(lnRowCtr, 0, "CASH ON HAND      : " & Format(p_oDTMaster(0).Item("nSalesAmt") + p_oDTMaster(0).Item("nOpenBalx") - p_oDTMaster(0).Item("nCPullOut"), xsDECIMAL).PadLeft(11))
    '    lnRowCtr = lnRowCtr + 2

    '    loPrint.Print(lnRowCtr, 0, "VAT SALES         : " & p_oDTMaster(0).Item("nVATSales"))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "VAT AMOUNT        : " & p_oDTMaster(0).Item("nVATAmtxx"))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "NO VAT SALES      : " & p_oDTMaster(0).Item("nNonVATxx"))
    '    lnRowCtr = lnRowCtr + 1
    '    loPrint.Print(lnRowCtr, 0, "ZERO RATED SALES  : " & p_oDTMaster(0).Item("nZeroRatd"))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Dash Separator(-)
    '    loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
    '    lnRowCtr = lnRowCtr + 1

    '    Return True

    'End Function

    'Private Function computeTotalCashierSales() As Boolean
    '    Dim lsTranDate As String
    '    lsTranDate = p_oDTMaster(0).Item("sTranDate")
    '    lsTranDate = Left(lsTranDate, 4) & "-" & Mid(lsTranDate, 5, 2) & "-" & Right(lsTranDate, 2)

    '    Dim lsSQL As String
    '    lsSQL = "SELECT a.nSalesAmt" & _
    '                 ", a.nVATSales" & _
    '                 ", a.nVATAmtxx" & _
    '                 ", a.nZeroRatd" & _
    '                 ", a.nDiscount" & _
    '                 ", a.sSourceCd" & _
    '                 ", a.sSourceNo" & _
    '           " FROM Receipt_Master a" & _
    '                " LEFT JOIN SO_Master b ON a.sSourceNo = b.sTransNox" & _
    '           " WHERE a.sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") & _
    '             " AND b.sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) & _
    '             " AND a.dTransact = " & dateParm(lsTranDate) & _
    '             " AND a.sSourceCd = 'SO'" & _
    '           " UNION " & _
    '           " SELECT a.nSalesAmt" & _
    '                 ", a.nVATSales" & _
    '                 ", a.nVATAmtxx" & _
    '                 ", a.nZeroRatd" & _
    '                 ", a.nDiscount" & _
    '                 ", a.sSourceCd" & _
    '                 ", a.sSourceNo" & _
    '           " FROM Receipt_Master a" & _
    '                " LEFT JOIN Order_Split b ON a.sSourceNo = b.sTransNox" & _
    '                " LEFT JOIN SO_Master c ON b.sReferNox = c.sTransNox" & _
    '           " WHERE a.sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") & _
    '             " AND c.sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) & _
    '             " AND a.dTransact = " & dateParm(lsTranDate) & _
    '             " AND a.sSourceCd = 'SOSp'"

    '    Dim loDta As DataTable
    '    loDta = p_oApp.ExecuteQuery(lsSQL)

    '    Dim lnSalesAmt As Decimal = 0
    '    Dim lnVATSales As Decimal = 0
    '    Dim lnVATAmtxx As Decimal = 0
    '    Dim lnZeroRatd As Decimal = 0
    '    Dim lnNonVATxx As Decimal = 0
    '    Dim lnDiscount As Decimal = 0

    '    Dim lnCtr As Integer
    '    For lnCtr = 0 To loDta.Rows.Count - 1
    '        lnSalesAmt = lnSalesAmt + loDta(lnCtr).Item("nSalesAmt")
    '        lnVATSales = lnVATSales + loDta(lnCtr).Item("nVATSales")
    '        lnVATAmtxx = lnVATAmtxx + loDta(lnCtr).Item("nVATAmtxx")
    '        lnZeroRatd = lnZeroRatd + loDta(lnCtr).Item("nZeroRatd")
    '        lnDiscount = lnDiscount + loDta(lnCtr).Item("nDiscount")
    '    Next

    '    lnNonVATxx = lnSalesAmt - (lnVATSales + lnZeroRatd + lnVATAmtxx)

    '    p_oDTMaster(0).Item("nSalesAmt") = lnSalesAmt
    '    p_oDTMaster(0).Item("nVATSales") = lnVATSales
    '    p_oDTMaster(0).Item("nVATAmtxx") = lnVATAmtxx
    '    p_oDTMaster(0).Item("nNonVATxx") = lnNonVATxx
    '    p_oDTMaster(0).Item("nZeroRatd") = lnZeroRatd
    '    p_oDTMaster(0).Item("nDiscount") = lnDiscount

    '    Return True
    'End Function

    Private Function computeTotalCashierSales() As Boolean
        Dim loDSC As New DataTable
        Dim lsSQL As String
        Dim lsTranDate As String
        Dim lnCtr As Integer

        lsTranDate = p_oDTMaster(0).Item("sTranDate")
        lsTranDate = Left(lsTranDate, 4) & "-" & Mid(lsTranDate, 5, 2) & "-" & Right(lsTranDate, 2)

        lsSQL = "SELECT" &
                    "  a.sTransNox" &
                    ", b.sTransNox" &
                " FROM Receipt_Master a" &
                    " LEFT JOIN Discount b" &
                        " ON a.sSourceNo = b.sSourceNo" &
                " WHERE a.dTransact = " & dateParm(lsTranDate) &
                    " AND (a.nPWDDiscx > 0 OR a.nDiscount > 0)" &
                    " AND a.cTranStat <> '3'" &
                " HAVING b.sTransNox IS NULL"
        loDSC = p_oApp.ExecuteQuery(lsSQL)

        For lnCtr = 0 To loDSC.Rows.Count - 1
            p_oApp.Execute("UPDATE Receipt_Master SET" &
                                      "  nVatDiscx = 0" &
                                      ", nPWDDiscx = 0" &
                                      ", nDiscount = 0" &
                                  " WHERE sTransNox = " & strParm(loDSC.Rows(lnCtr)("sTransNox")), "Receipt_Master")
        Next

        lsSQL = "Select a.nSalesAmt" &
                     ", a.nVATSales" &
                     ", a.nVATAmtxx" &
                     ", a.nZeroRatd" &
                     ", a.nDiscount" &
                     ", a.nVatDiscx" &
                     ", a.nPWDDiscx" &
                     ", a.nCashAmtx" &
                     ", a.sSourceCd" &
                     ", a.sSourceNo" &
                     ", a.sORNumber" &
                     ", a.nSChargex" &
                     ", a.cTranStat" &
               " FROM Receipt_Master a" &
               " WHERE a.sTransNox Like " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " And a.sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) &
                 " And a.dTransact = " & dateParm(lsTranDate) &
               " ORDER BY a.sORNumber ASC"

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        Dim lnSalesAmt As Decimal = 0
        Dim lnVATSales As Decimal = 0
        Dim lnVATAmtxx As Decimal = 0
        Dim lnZeroRatd As Decimal = 0
        Dim lnNonVATxx As Decimal = 0
        Dim lnDiscount As Decimal = 0
        Dim lnPWDDiscx As Decimal = 0
        Dim lnVatDiscx As Decimal = 0
        Dim lnVoidCntx As Integer = 0

        'Add payment form computation
        Dim lnCashAmtx As Decimal = 0
        Dim lnChckAmnt As Decimal = 0
        Dim lnCrdtAmnt As Decimal = 0
        Dim lnChrgAmnt As Decimal = 0
        Dim lnGiftAmnt As Decimal = 0
        Dim lnSChargex As Decimal = 0

        For lnCtr = 0 To loDta.Rows.Count - 1
            If loDta(lnCtr).Item("cTranStat") <> 3 Then
                lnSalesAmt = lnSalesAmt + loDta(lnCtr).Item("nSalesAmt")
                lnVATSales = lnVATSales + loDta(lnCtr).Item("nVATSales")
                lnVATAmtxx = lnVATAmtxx + loDta(lnCtr).Item("nVATAmtxx")
                lnSChargex = lnSChargex + loDta(lnCtr).Item("nSChargex")
                lnZeroRatd = lnZeroRatd + IFNull(loDta(lnCtr).Item("nZeroRatd"), 0)

                lnPWDDiscx = lnPWDDiscx + loDta(lnCtr).Item("nPWDDiscx")
                lnVatDiscx = lnVatDiscx + loDta(lnCtr).Item("nVatDiscx")
                lnDiscount = lnDiscount + loDta(lnCtr).Item("nDiscount")

                'Add payment form computation
                lnCashAmtx = lnCashAmtx + loDta(lnCtr).Item("nCashAmtx")

                lsSQL = "Select cPaymForm, nAmountxx" &
                       " FROM Payment" &
                       " WHERE sSourceCd = " & strParm(loDta(lnCtr).Item("sSourceCd")) &
                         " And sSourceNo = " & strParm(loDta(lnCtr).Item("sSourceNo"))
                Dim loDtaX As DataTable = p_oApp.ExecuteQuery(lsSQL)

                If loDtaX.Rows.Count > 0 Then
                    Dim lnCtrX As Integer
                    For lnCtrX = 0 To loDtaX.Rows.Count - 1
                        Select Case loDtaX(lnCtrX).Item("cPaymForm")
                            Case "1" 'Credit Card
                                lnCrdtAmnt = lnCrdtAmnt + loDtaX(lnCtrX).Item("nAmountxx")
                            Case "2" 'Check Payment
                                lnChckAmnt = lnChckAmnt + loDtaX(lnCtrX).Item("nAmountxx")
                            Case "3" 'Gift Certificate
                                lnGiftAmnt = lnGiftAmnt + loDtaX(lnCtrX).Item("nAmountxx")
                        End Select
                    Next
                End If
            ElseIf loDta(lnCtr).Item("cTranStat") = 3 Then
                lnVoidCntx = lnVoidCntx + 1
            End If
        Next

        'Cash Invoice is always set to 0

        p_oDTMaster(0).Item("nVoidCntx") = lnVoidCntx
        If loDta.Rows.Count > 0 Then
            p_oDTMaster(0).Item("sORNoFrom") = loDta(0).Item("sORNumber")
            p_oDTMaster(0).Item("sORNoThru") = loDta(loDta.Rows.Count - 1).Item("sORNumber")
        Else
            p_oDTMaster(0).Item("sORNoFrom") = ""
            p_oDTMaster(0).Item("sORNoThru") = ""
        End If

        'Compute for VOID Sales
        Dim loDtaCancelled As DataTable
        'lsSQL = "Select sTransNox, nTranTotl" & _
        '       " FROM SO_Master" & _
        '       " WHERE sTransNox Like " & strParm(p_oApp.BranchCode & p_sTermnl & "%") & _
        '         " And dTransact = " & dateParm(lsTranDate) & _
        '         " And IFNULL(sMergeIDx, '') = ''" & _
        '         " AND cTranStat = '3'" & _
        '         " AND sModified = " & strParm(p_oDTMaster(0).Item("sCashierx")) & _
        '       " UNION " & _
        '        "SELECT DISTINCT sMergeIDx, nTranTotl" & _
        '       " FROM SO_Master" & _
        '       " WHERE sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") & _
        '         " AND dTransact = " & dateParm(lsTranDate) & _
        '         " AND IFNULL(sMergeIDx, '') <> ''" & _
        '         " AND cTranStat = '3'" & _
        '         " AND sModified = " & strParm(p_oDTMaster(0).Item("sCashierx"))
        lsSQL = "SELECT" &
                       " (a.nUnitPrce - (a.nUnitPrce * (a.nDiscount / 100))) - a.nAddDiscx nTranTotl" &
               " FROM SO_Detail a" &
                    " LEFT JOIN SO_Master b ON a.sTransNox = b.sTransNox" &
               " WHERE a.sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " AND b.dTransact = " & dateParm(lsTranDate) &
                 " AND b.sModified = " & strParm(p_oDTMaster(0).Item("sCashierx")) &
                 " AND b.cTranStat = '3'"

        loDtaCancelled = p_oApp.ExecuteQuery(lsSQL)
        Dim lnCancelledAmnt As Decimal = 0

        For lnCtr = 0 To loDtaCancelled.Rows.Count - 1
            lnCancelledAmnt += IFNull(loDtaCancelled(lnCtr).Item("nTranTotl"), 0)
        Next

        'Compute for Reversed Orders as VOID Sales
        lsSQL = "SELECT dTransact, a.nQuantity, a.nUnitPrce, a.nDiscount, a.nAddDiscx" &
               " FROM SO_Detail a" &
                    " LEFT JOIN SO_Master b ON a.sTransNox = b.sTransNox" &
               " WHERE b.sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " AND b.dTransact = " & dateParm(lsTranDate) &
                 " AND b.cTranStat <> '3'" &
                 " AND a.cReversex = '-'" &
                 " AND b.sModified = " & strParm(p_oDTMaster(0).Item("sCashierx"))
        loDtaCancelled = p_oApp.ExecuteQuery(lsSQL)
        Dim lnSlPrc As Decimal = 0
        For lnCtr = 0 To loDtaCancelled.Rows.Count - 1
            lnSlPrc = (loDtaCancelled(lnCtr).Item("nUnitPrce") - (loDtaCancelled(lnCtr).Item("nUnitPrce") * (loDtaCancelled(lnCtr).Item("nDiscount") / 100))) - loDtaCancelled(lnCtr).Item("nAddDiscx")
            lnCancelledAmnt += (lnSlPrc * loDtaCancelled(lnCtr).Item("nQuantity"))
        Next
        p_oDTMaster(0).Item("nCancelld") = lnCancelledAmnt

        'iMac 2018-02-09
        'add cancelled invoice to void amount
        lsSQL = "SELECT nSalesAmt" &
                " FROM Receipt_Master" &
                " WHERE cTranStat = '3'" &
                    " AND dTransact = " & dateParm(lsTranDate) &
                    " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx"))

        Dim loDtaVoid As DataTable
        Dim lnVoidAmnt As Decimal = 0

        loDtaVoid = p_oApp.ExecuteQuery(lsSQL)
        For lnCtr = 0 To loDtaVoid.Rows.Count - 1
            lnVoidAmnt += loDtaVoid(lnCtr).Item("nSalesAmt")
        Next

        'Save the computed void amount to the field
        p_oDTMaster(0).Item("nVoidAmnt") = lnVoidAmnt

        'Compute for the Returns of customers
        lsSQL = "SELECT SUM(nTranAmtx) nTranAmnt" &
               " FROM Return_Master" &
               " WHERE sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " AND dTransact = " & dateParm(lsTranDate) &
                 " AND sModified = " & strParm(p_oDTMaster(0).Item("sCashierx"))
        loDtaVoid = p_oApp.ExecuteQuery(lsSQL)
        If loDtaVoid.Rows.Count > 0 Then
            p_oDTMaster(0).Item("nReturnsx") = IFNull(loDtaVoid(0).Item("nTranAmnt"), 0)
        End If

        'Compute for charge Invoice
        lsSQL = "SELECT" &
                    "  IFNULL(SUM(a.nAmountxx), 0) nTranTotl" &
                    ", IFNULL(SUM(a.nPWDDiscx), 0) nPWDDiscx" &
                    ", IFNULL(SUM(a.nVATDiscx), 0) nVATDiscx" &
                    ", IFNULL(SUM(a.nDiscount), 0) nDiscount" &
                    ", IFNULL(SUM(a.nVATSales), 0) nVATSales" &
                    ", IFNULL(SUM(a.nVATAmtxx), 0) nVATAmtxx" &
               " FROM Charge_Invoice a" &
                    " LEFT JOIN SO_Master b ON a.sSourceNo = b.sTransNox" &
               " WHERE a.sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " AND a.sSourceCD = 'SO'" &
                 " AND b.dTransact = " & dateParm(lsTranDate) &
                 " AND b.sMergeIDx IS NULL" &
                 " AND a.cTranStat = '0'" &
               " UNION" &
               " SELECT" &
                    "  IFNULL(SUM(a.nAmountxx), 0) nTranTotl" &
                    ", IFNULL(SUM(a.nPWDDiscx), 0) nPWDDiscx" &
                    ", IFNULL(SUM(a.nVATDiscx), 0) nVATDiscx" &
                    ", IFNULL(SUM(a.nDiscount), 0) nDiscount" &
                    ", IFNULL(SUM(a.nVATSales), 0) nVATSales" &
                    ", IFNULL(SUM(a.nVATAmtxx), 0) nVATAmtxx" &
               " FROM Charge_Invoice a" &
                    " LEFT JOIN SO_Master b ON a.sSourceNo = b.sMergeIDx" &
               " WHERE a.sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " AND a.sSourceCD = 'SO'" &
                 " AND b.dTransact = " & dateParm(lsTranDate) &
                 " AND b.sMergeIDx IS NOT NULL" &
                 " AND a.cTranStat = '0'" &
               " UNION" &
               " SELECT" &
                    "  IFNULL(SUM(a.nAmountxx), 0) nTranTotl" &
                    ", IFNULL(SUM(a.nPWDDiscx), 0) nPWDDiscx" &
                    ", IFNULL(SUM(a.nVATDiscx), 0) nVATDiscx" &
                    ", IFNULL(SUM(a.nDiscount), 0) nDiscount" &
                    ", IFNULL(SUM(a.nVATSales), 0) nVATSales" &
                    ", IFNULL(SUM(a.nVATAmtxx), 0) nVATAmtxx" &
               " FROM Charge_Invoice a" &
                    " LEFT JOIN Order_Split b ON a.sSourceNo = b.sTransNox" &
                    " LEFT JOIN SO_Master c ON b.sReferNox = c.sTransNox" &
               " WHERE a.sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " AND a.sSourceCD = 'SOSp'" &
                 " AND c.dTransact = " & dateParm(lsTranDate) &
                 " AND a.cTranStat = '0'"

        Dim loDtaChrg As DataTable
        loDtaChrg = p_oApp.ExecuteQuery(lsSQL)
        lnChrgAmnt = 0

        If loDtaChrg.Rows.Count > 0 Then
            For lnCtr = 0 To loDtaChrg.Rows.Count - 1
                'lnSalesAmt = lnSalesAmt + loDtaChrg(lnCtr).Item("nTranTotl")

                lnChrgAmnt += loDtaChrg(lnCtr).Item("nTranTotl")
                'lnPWDDiscx += loDtaChrg(lnCtr).Item("nPWDDiscx")
                'lnVatDiscx += loDtaChrg(lnCtr).Item("nVatDiscx")
                'lnDiscount += loDtaChrg(lnCtr).Item("nDiscount")
                'lnVATSales += loDtaChrg(lnCtr).Item("nVATSales")
                'lnVATAmtxx += loDtaChrg(lnCtr).Item("nVATAmtxx")
            Next
        End If

        'lnNonVATxx = lnSalesAmt - (lnVATSales + lnZeroRatd + lnVATAmtxx)

        lnNonVATxx = lnSalesAmt + lnVatDiscx + lnPWDDiscx
        lnNonVATxx = lnNonVATxx - (lnVATSales + lnVATAmtxx)
        lnNonVATxx = lnNonVATxx / 1.12

        'p_oDTMaster(0).Item("nSalesAmt") = (lnSalesAmt + lnDiscount + lnVatDiscx + lnPWDDiscx) - p_oDTMaster(0).Item("nReturnsx")
        p_oDTMaster(0).Item("nSalesAmt") = lnSalesAmt - p_oDTMaster(0).Item("nReturnsx")
        'lnSalesAmt = lnSalesAmt - (lnNonVATxx + lnZeroRatd)
        'p_oDTMaster(0).Item("nVATSales") = Math.Round(((lnSalesAmt) - p_oDTMaster(0).Item("nReturnsx")) / 1.12, 2)
        'p_oDTMaster(0).Item("nVATAmtxx") = lnSalesAmt - (p_oDTMaster(0).Item("nReturnsx") + p_oDTMaster(0).Item("nVATSales"))

        'p_oDTMaster(0).Item("nVATSales") = Math.Round(((lnSalesAmt + lnChrgAmnt) - p_oDTMaster(0).Item("nReturnsx")) / 1.12, 2)
        'p_oDTMaster(0).Item("nVATAmtxx") = Math.Round(p_oDTMaster(0).Item("nVATSales") * 0.12, 2)
        'p_oDTMaster(0).Item("nVATAmtxx") = lnSalesAmt - (p_oDTMaster(0).Item("nReturnsx") + p_oDTMaster(0).Item("nVATSales"))

        p_oDTMaster(0).Item("nVATSales") = lnVATSales
        p_oDTMaster(0).Item("nVATAmtxx") = lnVATAmtxx

        p_oDTMaster(0).Item("nNonVATxx") = lnNonVATxx
        p_oDTMaster(0).Item("nZeroRatd") = lnZeroRatd
        p_oDTMaster(0).Item("nDiscount") = lnDiscount
        p_oDTMaster(0).Item("nPWDDiscx") = lnPWDDiscx
        p_oDTMaster(0).Item("nVatDiscx") = lnVatDiscx

        p_oDTMaster(0).Item("nCashAmnt") = lnCashAmtx
        p_oDTMaster(0).Item("nChckAmnt") = lnChckAmnt
        p_oDTMaster(0).Item("nCrdtAmnt") = lnCrdtAmnt
        p_oDTMaster(0).Item("nGiftAmnt") = lnGiftAmnt
        p_oDTMaster(0).Item("nChrgAmnt") = lnChrgAmnt
        p_oDTMaster(0).Item("nSChargex") = lnSChargex
        Return True
    End Function

    Private Function validSummary(ByVal bCurrent As Boolean) As Boolean
        'Load Transactions with OPEN STATUS prior to the current day....
        Dim lsSQL As String
        lsSQL = "SELECT *" & _
               " FROM SO_Master" & _
               " WHERE sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") & _
                 " AND dTransact " & IIf(bCurrent, " = ", " < ") & dateParm(Format(p_oApp.SysDate, xsDATE_SHORT)) & _
                 " AND cTranStat = '0'"

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count > 0 Then p_dPOSDatex = loDta.Rows(0).Item("dTransact")

        Return loDta.Rows.Count = 0
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

    Public Function initMachine() As Boolean
        If p_sPOSNo = "" Then
            MsgBox("Invalid Machine Identification Info Detected...")
            Return False
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" &
                       "  sAccredtn" &
                       ", sPermitNo" &
                       ", sSerialNo" &
                       ", nPOSNumbr" &
                       ", nZReadCtr" &
                       ", cRLCPOSxx" &
               " FROM Cash_Reg_Machine" &
               " WHERE sIDNumber = " & strParm(p_sPOSNo)

        Debug.Print(p_sPOSNo)

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count <> 1 Then
            MsgBox("Invalid Config for MIN Detected...")
            Return False
        End If

        p_sAccrdt = loDta(0).Item("sAccredtn")
        p_sPermit = loDta(0).Item("sPermitNo")
        p_sSerial = loDta(0).Item("sSerialNo")
        p_sTermnl = loDta(0).Item("nPOSNumbr")
        p_nZRdCtr = loDta(0).Item("nZReadCtr")
        p_bWasRLCClt = IIf(loDta(0).Item("cRLCPOSxx") = 0, False, True)

        Return True
    End Function

    Private Sub initMaster()
        Dim lnCtr As Integer
        For lnCtr = 0 To p_oDTMaster.Columns.Count - 1
            Select Case LCase(p_oDTMaster.Columns(lnCtr).ColumnName)
                Case "strandate"
                    p_oDTMaster(0).Item(lnCtr) = Format(p_oApp.getSysDate, "yyyyMMdd")
                Case "ctranstat"
                    p_oDTMaster(0).Item(lnCtr) = "0"
                Case "scrmnumbr"
                    p_oDTMaster(0).Item(lnCtr) = p_sPOSNo
                Case "scashierx"
                    p_oDTMaster(0).Item(lnCtr) = p_oApp.UserID
                Case "sornofrom", "sornothru"
                    p_oDTMaster(0).Item(lnCtr) = ""
                Case "dopenedxx", "dclosedxx"
                Case Else
                    p_oDTMaster(0).Item(lnCtr) = 0.0
            End Select
        Next

        Dim lsSQL As String
        Dim loDta As DataTable

        lsSQL = "SELECT nAccuSale, nSalesAmt" & _
               " FROM Daily_Summary" & _
               " WHERE sTranDate = " & Format(p_oApp.getSysDate, "yyyyMMdd") & _
                 " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) & _
               " ORDER BY sTranDate DESC LIMIT 1"
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count > 0 Then
            p_oDTMaster(0).Item("nAccuSale") = loDta(0).Item("nAccuSale") + loDta(0).Item("nSalesAmt")
        End If
    End Sub

    Private Function PadCenter(ByVal source As String, ByVal length As Integer) As String
        Dim spaces As Integer = length - source.Length
        Dim padLeft As Integer = spaces / 2 + source.Length
        Return source.PadLeft(padLeft, " ").PadRight(length, " ")
    End Function

    Private Function getSQ_Master() As String
        Return "SELECT a.sTranDate" &
                    ", a.sCRMNumbr" &
                    ", a.sCashierx" &
                    ", a.nOpenBalx" &
                    ", a.nCPullOut" &
                    ", a.nSalesAmt" &
                    ", a.nVATSales" &
                    ", a.nVATAmtxx" &
                    ", a.nNonVATxx" &
                    ", a.nZeroRatd" &
                    ", a.nDiscount" &
                    ", a.nPWDDiscx" &
                    ", a.nVatDiscx" &
                    ", a.nReturnsx" &
                    ", a.nVoidAmnt" &
                    ", a.nAccuSale" &
                    ", a.nCashAmnt" &
                    ", a.nSChargex" &
                    ", a.nChckAmnt" &
                    ", a.nCrdtAmnt" &
                    ", a.nGiftAmnt" &
                    ", a.nChrgAmnt" &
                    ", a.sORNoFrom" &
                    ", a.sORNoThru" &
                    ", a.nZReadCtr" &
                    ", a.cTranStat" &
                    ", a.dOpenedxx" &
                    ", a.dClosedxx" &
                    ", a.nVoidCntx" &
                    ", a.nCancelld" &
                " FROM " & p_sMasTable & " a"
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider
        p_nEditMode = xeEditMode.MODE_UNKNOWN

        p_oDTMaster = Nothing

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")      'MIN
        p_sVATReg = Environment.GetEnvironmentVariable("REG-TIN-No")     'VAT REG No.
        p_sCompny = Environment.GetEnvironmentVariable("RMS-CLT-NM")
    End Sub
End Class
