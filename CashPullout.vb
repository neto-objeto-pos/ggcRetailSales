'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     CashPullout Object
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
'  Note: Should allow supervisor to perform cash pullout....
'
'  kalyptus [ 11/23/2016 03:55 pm ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ADODB
Imports ggcAppDriver
Imports ggcReceipt

Public Class CashPullout
    Private p_oApp As GRider
    Private p_oDTMaster As DataTable
    Private p_oOthersx As New Others

    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sCompny As String     'Company  : MONARK HOTEL
    Private p_sCashierx As String

    Private Const p_sMasTable As String = "Cash_Pull_Out"
    Private Const p_sMsgHeadr As String = "Cash Pullout"

    Private Const pxeLFTMGN As Integer = 3

    Public Event MasterRetrieved(ByVal Index As Integer, _
                                  ByVal Value As Object)

    WriteOnly Property Cashier As String
        Set(ByVal Value As String)
            p_sCashierx = Value
        End Set
    End Property


    Public Property Master(ByVal Index As Integer) As Object
        Get
            Select Case Index
                Case 80
                    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sCashierN) = "" Then
                        getCashier(2, 80, p_oDTMaster(0).Item(2), True, False)
                    End If
                    Return p_oOthersx.sCashierN
                Case 81
                    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sCashierN) = "" Then
                        getCashier(2, 80, p_oDTMaster(0).Item(2), True, False)
                    End If
                    Return p_oOthersx.nOpenBalx
                Case 82
                    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sCashierN) = "" Then
                        getCashier(2, 80, p_oDTMaster(0).Item(2), True, False)
                    End If
                    Return p_oOthersx.nCPullOut
                Case 83
                    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sCashierN) = "" Then
                        getCashier(2, 80, p_oDTMaster(0).Item(2), True, False)
                    End If
                    Return p_oOthersx.nSalesAmt
                Case Else
                    Return p_oDTMaster(0).Item(Index)
            End Select
        End Get

        Set(ByVal value As Object)
            Select Case Index
                Case 80
                    getCashier(2, 80, value, False, False)
                Case 81, 82, 83
                Case Else
                    p_oDTMaster(0).Item(Index) = value

                    RaiseEvent MasterRetrieved(Index, p_oDTMaster(0).Item(Index))
            End Select
        End Set
    End Property

    'Property Master(String)
    Public Property Master(ByVal Index As String) As Object
        Get
            Select Case LCase(Index)
                Case "scashiern"
                    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sCashierN) = "" Then
                        getCashier(2, 80, p_oDTMaster(0).Item(2), True, False)
                    End If
                    Return p_oOthersx.sCashierN
                Case "nopenbalx"
                    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sCashierN) = "" Then
                        getCashier(2, 80, p_oDTMaster(0).Item(2), True, False)
                    End If
                    Return p_oOthersx.nOpenBalx
                Case "ncpullout"
                    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sCashierN) = "" Then
                        getCashier(2, 80, p_oDTMaster(0).Item(2), True, False)
                    End If
                    Return p_oOthersx.nCPullOut
                Case "nsalesamt"
                    If Trim(IFNull(p_oDTMaster(0).Item(2))) <> "" And Trim(p_oOthersx.sCashierN) = "" Then
                        getCashier(2, 80, p_oDTMaster(0).Item(2), True, False)
                    End If
                    Return p_oOthersx.nSalesAmt
                Case Else
                    Return p_oDTMaster(0).Item(Index)
            End Select
        End Get

        Set(ByVal value As Object)
            Select Case LCase(Index)
                Case "scashiern"
                    getCashier(2, 80, value, False, False)
                Case "nopenbalx", "ncpullout", "nsalesamt"
                Case Else
                    Master(p_oDTMaster.Columns(Index).Ordinal) = value
            End Select
        End Set
    End Property

    'Public Function NewTransaction()
    Public Function NewTransaction() As Boolean
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Master, "0=1")
        p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)
        p_oDTMaster.Rows.Add(p_oDTMaster.NewRow())

        Call initMaster()

        Return True
    End Function

    Public Sub SearchMaster(ByVal fnIndex As Integer, ByVal fsValue As String)
        Select Case fnIndex
            Case 80 ' sClientNm
                getCashier(2, 80, fsValue, False, True)
        End Select
    End Sub

    'Save CASHIER DAILY SALES 
    Public Function SaveTransaction() As Boolean

        Dim lsSQL As String

        If Not isEntryOk() Then Return False

        'Save master table 
        lsSQL = ADO2SQL(p_oDTMaster, p_sMasTable)

        p_oApp.BeginTransaction()

        If lsSQL <> "" Then
            p_oApp.Execute(lsSQL, p_sMasTable)
        End If

        If Not PrintTrans() Then Return False

        'UPDATE Daily_Summary for this CASH PULLOUT
        lsSQL = "UPDATE Daily_Summary" & _
               " SET nCPullOut = nCPullOut + " & p_oDTMaster(0).Item("nAmountxx") & _
               " WHERE sTranDate = " & strParm(Format(p_oDTMaster(0).Item("dTransact"), "yyyyMMdd")) & _
                 " AND sCRMNumbr = " & strParm(p_oDTMaster(0).Item("sCRMNumbr")) & _
                 " AND sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx"))
        p_oApp.Execute(lsSQL, "Daily_Summary")

        p_oApp.CommitTransaction()

        Return True
    End Function

    Private Function PrintTrans() As Boolean
        Dim Printer_Name As String = Environment.GetEnvironmentVariable("RMS_PRN_CS")
        Dim builder As New System.Text.StringBuilder()

        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(RawPrint.pxePRINT_CNTR)
        builder.Append(PadCenter(Trim(p_sCompny), 20) & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense
        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1 + RawPrint.pxeESC_DBLW + RawPrint.pxeESC_EMPH))
        builder.Append(PadCenter("CASH PULLOUT", 20) & Environment.NewLine & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_ESC & Chr(RawPrint.pxeESC_FNT1)) 'Condense

        builder.Append(RawPrint.pxePRINT_LEFT)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        'builder.Append(" Cashier: " & p_oOthersx.sCashierN & Environment.NewLine)
        builder.Append(" Sales Date: " & Format(p_oDTMaster(0).Item("dTransact"), xsDATE_SHORT) & Environment.NewLine)
        builder.Append(" Cashier   : " & p_oApp.UserName.PadRight(27) & Environment.NewLine)
        builder.Append(" Tran ID   : " & Strings.Right(p_oDTMaster(0).Item("sTransNox"), 6) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP1)          'Double Strike + Condense + Emphasize
        builder.Append(" Amount    : " & Format(p_oDTMaster(0).Item("nAmountxx"), xsDECIMAL) & Environment.NewLine)
        builder.Append(RawPrint.pxePRINT_EMP0)          'Double Strike + Condense + Emphasize

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))
        'RawPrint.SendStringToPrinter(Printer_Name, builder.ToString())

        Call WritePullout()

        Return True
    End Function

    Private Function WritePullout() As Boolean
        Dim builder As New System.Text.StringBuilder()

        builder.Append(Environment.NewLine)
        builder.Append(PadCenter(Trim(p_sCompny), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.BranchName), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.Address), 40) & Environment.NewLine)
        builder.Append(PadCenter(Trim(p_oApp.TownCity & ", " & p_oApp.Province), 40) & Environment.NewLine)
        builder.Append(PadCenter("MIN : " & p_sPOSNo, 40) & Environment.NewLine & Environment.NewLine)

        builder.Append(PadCenter("CASH PULLOUT", 40) & Environment.NewLine & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)

        'builder.Append(" Cashier: " & p_oOthersx.sCashierN & Environment.NewLine)
        builder.Append(" Sales Date: " & Format(p_oDTMaster(0).Item("dTransact"), xsDATE_SHORT) & Environment.NewLine)
        builder.Append(" Cashier   : " & p_oApp.UserName.PadRight(27) & Environment.NewLine)
        builder.Append(" Tran ID   : " & Strings.Right(p_oDTMaster(0).Item("sTransNox"), 6) & Environment.NewLine)
        builder.Append(" Amount    : " & Format(p_oDTMaster(0).Item("nAmountxx"), xsDECIMAL) & Environment.NewLine)

        builder.Append("-".PadLeft(40, "-") & Environment.NewLine)
        builder.Append("*".PadLeft(40, "*") & Environment.NewLine)
        builder.Append("/end-of-summary - " & Format(p_oApp.getSysDate, "dd/MMM/yyyy hh:mm:ss") & Environment.NewLine)

        RawPrint.writeToFile(p_sPOSNo & " " & Format(p_oApp.getSysDate(), "yyyyMMdd"), builder.ToString())

        Return True
    End Function

    'This method implements a search master where id and desc are not joined.
    Private Sub getCashier(ByVal fnColIdx As Integer _
                         , ByVal fnColDsc As Integer _
                         , ByVal fsValue As String _
                         , ByVal fbIsCode As Boolean _
                         , ByVal fbIsSrch As Boolean)

        'Compare the value to be search against the value in our column
        If fbIsCode Then
            If fsValue = p_oDTMaster(0).Item(fnColIdx) And fsValue <> "" And p_oOthersx.sCashierN <> "" Then Exit Sub
        Else
            If fsValue = p_oOthersx.sCashierN And fsValue <> "" Then Exit Sub
        End If

        'Load open CASHIER DAILY SALES only!!!
        Dim lsSQL As String
        lsSQL = "SELECT" & _
                       "  a.sCashierx" & _
                       ", b.sUserName" & _
                       ", a.nOpenBalx" & _
                       ", a.nCPullOut" & _
               " FROM Daily_Summary a" & _
                    " LEFT JOIN xxxSysUser b ON a.sCashierx = b.sUserIDxx" & _
               " WHERE a.sCRMNumbr = " & strParm(p_oDTMaster(0).Item("sCRMNumbr")) & _
                 " AND a.sTranDate = " & strParm(Format(p_oDTMaster(0).Item("dTransact"), "yyyyMMdd")) & _
                 " AND a.cTranStat = '0'"

        'Are we using like comparison or equality comparison
        If fbIsSrch Then
            Dim loRow As DataRow = KwikSearch(p_oApp _
                                             , lsSQL _
                                             , True _
                                             , fsValue _
                                             , "sCashierx»sUserName" _
                                             , "ID»Cashier", _
                                             , "a.sCashierx»b.sUserName" _
                                             , IIf(fbIsCode, 0, 1))
            If IsNothing(loRow) Then
                p_oDTMaster(0).Item(fnColIdx) = ""
                p_oOthersx.sCashierN = ""
                p_oOthersx.nCPullOut = 0
                p_oOthersx.nOpenBalx = 0
                p_oOthersx.nSalesAmt = 0
            Else
                p_oDTMaster(0).Item(fnColIdx) = loRow.Item("sCashierx")
                p_oOthersx.nCPullOut = loRow.Item("nCPullOut")
                p_oOthersx.nOpenBalx = loRow.Item("nOpenBalx")
                'TODO: create function that computes for the total SALES AMOUNT OF THE CASHIER
                p_oOthersx.nSalesAmt = getTotalSales(p_oDTMaster(0).Item("dTransact"))
            End If

            RaiseEvent MasterRetrieved(fnColDsc, p_oDTMaster(0).Item(fnColIdx))
            RaiseEvent MasterRetrieved(80, p_oOthersx.sCashierN)
            RaiseEvent MasterRetrieved(81, p_oOthersx.nOpenBalx)
            RaiseEvent MasterRetrieved(82, p_oOthersx.nCPullOut)
            RaiseEvent MasterRetrieved(83, (p_oOthersx.nSalesAmt + p_oOthersx.nOpenBalx) - p_oOthersx.nCPullOut)
            Exit Sub
        End If

        If fsValue <> "" Then
            If fbIsCode Then
                lsSQL = AddCondition(lsSQL, "a.sCashierx = " & strParm(fsValue))
            Else
                lsSQL = AddCondition(lsSQL, "b.sUserName = " & strParm(fsValue))
            End If
        End If

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count = 0 Then
            p_oDTMaster(0).Item(fnColIdx) = ""
            p_oOthersx.sCashierN = ""
            p_oOthersx.nCPullOut = 0
            p_oOthersx.nOpenBalx = 0
            p_oOthersx.nSalesAmt = 0
        ElseIf loDta.Rows.Count = 1 Then
            p_oDTMaster(0).Item(fnColIdx) = loDta(0).Item("sCashierx")
            p_oOthersx.nCPullOut = loDta(0).Item("nCPullOut")
            p_oOthersx.nOpenBalx = loDta(0).Item("nOpenBalx")
            p_oOthersx.nSalesAmt = getTotalSales(p_oDTMaster(0).Item("dTransact"))
        End If

        RaiseEvent MasterRetrieved(fnColDsc, p_oDTMaster(0).Item(fnColIdx))
        RaiseEvent MasterRetrieved(80, p_oOthersx.sCashierN)
        RaiseEvent MasterRetrieved(81, p_oOthersx.nOpenBalx)
        RaiseEvent MasterRetrieved(82, p_oOthersx.nCPullOut)
        RaiseEvent MasterRetrieved(83, (p_oOthersx.nSalesAmt + p_oOthersx.nOpenBalx) - p_oOthersx.nCPullOut)
    End Sub

    Private Function getTotalSales(sTranDate As Date) As Decimal
        Dim lsSQL As String

        'Get Terminal Number
        lsSQL = "SELECT nPOSNumbr" & _
               " FROM Cash_Reg_Machine" & _
               " WHERE sIDNumber = " & strParm(p_sPOSNo)

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        lsSQL = "SELECT a.sTransNox, a.nCashAmtx" & _
               " FROM Receipt_Master a" & _
                    " LEFT JOIN SO_Master b ON a.sSourceNo = b.sTransNox" & _
               " WHERE b.sTransNox LIKE " & strParm(p_oApp.BranchCode & loDta(0).Item("nPOSNumbr") & "%") & _
                 " AND b.sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) & _
                 " AND a.dTransact = " & dateParm(sTranDate) & _
                 " AND a.sSourceCd = 'SO'" & _
               " UNION " & _
               " SELECT a.sTransNox, a.nCashAmtx" & _
               " FROM Receipt_Master a" & _
                    " LEFT JOIN Order_Split b ON a.sSourceNo = b.sTransNox" & _
                    " LEFT JOIN SO_Master c ON b.sReferNox = c.sTransNox" & _
               " WHERE c.sTransNox LIKE " & strParm(p_oApp.BranchCode & loDta(0).Item("nPOSNumbr") & "%") & _
                 " AND c.sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) & _
                 " AND a.dTransact = " & dateParm(sTranDate) & _
                 " AND a.sSourceCd = 'SOSp'"

        loDta = p_oApp.ExecuteQuery(lsSQL)

        Dim lnSalesAmt As Decimal = 0

        Dim lnCtr As Integer
        For lnCtr = 0 To loDta.Rows.Count - 1
            lnSalesAmt = lnSalesAmt + loDta(lnCtr).Item("nCashAmtx")
        Next

        Return lnSalesAmt
    End Function

    Private Sub initMaster()
        p_oDTMaster(0).Item("sTransNox") = GetNextCode(p_sMasTable, "sTransNox", True, p_oApp.Connection, True, p_oApp.BranchCode)
        p_oDTMaster(0).Item("sCRMNumbr") = p_sPOSNo
        p_oDTMaster(0).Item("sCashierx") = p_sCashierx
        p_oDTMaster(0).Item("dTransact") = p_oApp.getSysDate

        'Load current user info
        Call getCashier(2, 80, p_sCashierx, True, False)

        If p_oOthersx.nSalesAmt <= 0 Then
            p_oDTMaster(0).Item("sCashierx") = ""
            p_oOthersx.sCashierN = 0
        End If

        p_oDTMaster(0).Item("nAmountxx") = 0
    End Sub

    Private Sub initOthers()
        p_oOthersx.sCashierN = ""
        p_oOthersx.nCPullOut = 0
        p_oOthersx.nOpenBalx = 0
        p_oOthersx.nSalesAmt = 0
    End Sub

    Private Function isEntryOk() As Boolean
        'Check for the information about the card
        If Trim(p_oDTMaster(0).Item("sCashierx")) = "" Then
            MsgBox("Cashier to pull out from is invalid! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        If (p_oOthersx.nSalesAmt + p_oOthersx.nOpenBalx - p_oOthersx.nCPullOut) < p_oDTMaster(0).Item("nAmountxx") Then
            MsgBox("Pull out is higher than the CASH ON HAND! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        'Check how much does he intends to pullout
        If p_oDTMaster(0).Item("nAmountxx") <= 0 Then
            MsgBox("Amount seems to have a problem! Please check your entry....", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, p_sMsgHeadr)
            Return False
        End If

        Return True
    End Function

    Private Function PadCenter(source As String, length As Integer) As String
        Dim spaces As Integer = length - source.Length
        Dim padLeft As Integer = spaces / 2 + source.Length
        Return source.PadLeft(padLeft, " ").PadRight(length, " ")
    End Function

    Private Function getSQ_Master() As String
        Return "SELECT a.sTransNox" & _
                    ", a.sCRMNumbr" & _
                    ", a.sCashierx" & _
                    ", a.dTransact" & _
                    ", a.nAmountxx" & _
                " FROM " & p_sMasTable & " a"
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider
        p_oDTMaster = Nothing

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")      'MIN
        p_sCompny = Environment.GetEnvironmentVariable("RMS-CLT-NM")
    End Sub

    Private Class Others
        Public sCashierN As String
        Public nOpenBalx As Decimal
        Public nCPullOut As Decimal
        Public nSalesAmt As Decimal
    End Class
End Class
