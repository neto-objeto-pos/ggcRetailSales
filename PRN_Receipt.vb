'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     POS Receipt Printing
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
' Sample Receipt Printing
'1234567890123456789012345678901234567890
'
'             MONARK HOTEL
'   PEDRITO'S BAKESHOP AND RESTAURANT
'   Mc Arthur Highway, Tapuac District
'       Dagupan City, Pangasinan
'     VAT REG TIN: 941-184-389-000
'       MIN : 14121419321782091
'   Permit #: PR122014-004-D004507-000
'      Serial No. : L9GF261769
'****************************************
'QTY DESCRIPTION       UPRICE     AMOUNT 
'  2 123456789012345 2,500.00   5,000.00V
'  1 CLUBHSE SANDWCH   140.00     140.00V
'----------------------------------------
' No of Items: 3
'
' TOTAL                        5,140.00
' Less: Discount(s)              140.00
'       VAT                      500.00
'                         ------------- 
' Amount Due:                  4,500.00
' Cash                         1,000.00
' BDO                          1,000.00
' METROBANK                    1,000.00
' 12345-7890-12                1,500.00
'                         ------------- 
' CHANGE    :                      0.00
'///////////////////////////////////////
'Senior Citizen
'125-234561
'///////////////////////////////////////
'BDO 
'54697******4006
'SWIPED
'Approval Code:005273
'///////////////////////////////////////
'METROBANK
'552097******1519
'SWIPED
'Approval Code: 426235
'///////////////////////////////////////
'Check No: 12345-7890-12
'Bank    : Metrobank
'Date:   : 11/18/2016 
'Amount  : 1,500.00
'----------------------------------------
'
'  VAT Exempt Sales         2,500.00
'  Zero Rated Sales             0.00
'  VAT Sales                1,760.00 
'  VAT Amount                 240.00
' 
' Cust Name: ---------------------------- 
' Address  : ----------------------------
' TIN #    : ----------------------------
' Bus Style: ----------------------------
'
' Cashier: Marlon A. Sayson
' Terminal No.: 02       
' OR No.: 00172015
' Date: 11/18/2016 09:15 am
'****************************************
'       Have A Nice Day! Come Again.
'   This serves as an OFFICIAL RECEIPT
' Telephone (075)653-1347/48 or visit us
'     http://www.pedritosbakeshop.com
'
' ==========================================================================================
'  kalyptus [ 11/16/2016 09:37 am ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

Imports ADODB
Imports ggcAppDriver
Imports System.Drawing

Public Class PRN_Receipt
    Private p_oApp As GRider

    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sVATReg As String     'TIN:       941-184-389-000
    Private p_sCompny As String     'Company  : MONARK HOTEL

    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_sAccrdt As String     'Accrdt No: 038-227471337-000028
    Private p_sTermnl As String     'Termnl No: 02

    Private p_oDTMaster As DataTable
    Private p_oDTDetail As DataTable
    Private p_oDTGftChk As DataTable    'Gift Check
    Private p_oDTChkPym As DataTable    'Check Payment
    Private p_oDTCredit As DataTable    'Credit Card

    Private p_oDTHeader As DataTable
    Private p_oDTFooter As DataTable
    Private p_oDTDiscnt As DataTable

    'Transaction Master Info
    Private psCashrNme As String
    Private pdTransact As Date          'XXX
    Private psReferNox As String        'XXX

    Private pnTotalItm As Decimal
    Private pnTotalDue As Decimal
    Private pnDiscAmtV As Decimal
    Private pnDiscAmtN As Decimal

    'Total Payments
    Private pnCashTotl As Decimal       'XXX
    Private pnGiftTotl As Decimal
    Private pnChckTotl As Decimal
    Private pnCrdtTotl As Decimal

    'Sale Total Info
    Private pnVatblSle As Decimal
    Private pnVatExSle As Decimal       'XXX
    Private pnZroRtSle As Decimal
    Private pnVatAmntx As Decimal

    'Customer Information
    Private psCustName As String        'XXX
    Private psCustAddx As String        'XXX
    Private psCustTINx As String        'XXX    
    Private psCustBusx As String        'XXX

    Private Const pxeQTYLEN As Integer = 4  '+ 1
    Private Const pxeDSCLEN As Integer = 15 '+ 1
    Private Const pxePRCLEN As Integer = 8  '+ 1
    Private Const pxeTTLLEN As Integer = 10
    Private Const pxeREGLEN As Integer = 12
    Private Const pxeLFTMGN As Integer = 3

    Public Property Transaction_Date() As Date
        Get
            Return pdTransact
        End Get
        Set(ByVal value As Date)
            pdTransact = value
        End Set
    End Property

    Public Property ReferenceNo() As String
        Get
            Return psReferNox
        End Get
        Set(ByVal value As String)
            psReferNox = value
        End Set
    End Property

    Public Property CashPayment() As Decimal
        Get
            Return pnCashTotl
        End Get
        Set(ByVal value As Decimal)
            pnCashTotl = value
        End Set
    End Property

    Public Property NonVatSales() As Decimal
        Get
            Return pnVatExSle
        End Get
        Set(ByVal value As Decimal)
            pnVatExSle = value
        End Set
    End Property

    '+++++++++++++++++++++++++
    'InitMachine() As Boolean
    '   - Initializes and Validates the POS Machine
    '+++++++++++++++++++++++++
    Public Function InitMachine() As Boolean
        If p_sPOSNo = "" Then
            MsgBox("Invalid Machine Identification Info Detected...")
            Return False
        End If

        Dim lsSQL As String
        lsSQL = "SELECT" & _
                       "  sAccredtn" & _
                       ", sPermitNo" & _
                       ", sSerialNo" & _
                       ", nPOSNumbr" & _
               " FROM Cash_Reg_Machine" & _
               " WHERE sIDNumber = " & strParm(p_sPOSNo)

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

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddHeader(Header) As Boolean
    '   - Sets what are to be printed at the Header Section of Receipt
    '     Please exclude the MIN, Vat Reg, Permit No, Serial No, and Accredtn No
    '+++++++++++++++++++++++++
    Private Function AddHeader(ByVal Header As String) As Boolean
        With p_oDTHeader
            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sHeadName") = Left(Trim(Header), 40)
        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddDetail(Quantity, Description, UnitPrice, isVatable)
    '   - Sets the info of the ITEMS bought...
    '+++++++++++++++++++++++++
    Public Function AddDetail( _
            ByVal Quantity As Integer, _
            ByVal Description As String, _
            ByVal UnitPrice As Decimal, _
            ByVal isVatable As Boolean,
            Optional ByVal isDetail As Boolean = True) As Boolean

        With p_oDTDetail

            If .Rows.Count = 0 Then
                pnTotalDue = 0  'Initialize Total Amount Due
                pnZroRtSle = 0  'Initialize Zero Rated Sale
                pnTotalItm = 0  'Initialize Total Item Sold
            End If

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("nQuantity") = Quantity
            .Rows(.Rows.Count - 1).Item("sBriefDsc") = Left(Description, 14)
            .Rows(.Rows.Count - 1).Item("nUnitPrce") = UnitPrice
            .Rows(.Rows.Count - 1).Item("nTotlAmnt") = Quantity * UnitPrice
            .Rows(.Rows.Count - 1).Item("cVatablex") = IIf(isVatable = True, 1, 0)

            pnTotalDue = pnTotalDue + (Quantity * UnitPrice)

            If Not isDetail Then
                pnTotalItm = pnTotalItm + Quantity
            End If

            If Not isVatable Then
                pnZroRtSle = pnZroRtSle + (Quantity * UnitPrice)
            End If

        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddDiscount(IDNumber, DiscCard, Amount, isVatable)
    '   - Sets the info of the discounts for this sales...
    '+++++++++++++++++++++++++
    Public Function AddDiscount( _
            ByVal IDNumber As String, _
            ByVal DiscCard As String, _
            ByVal Amount As Decimal, _
            ByVal isVatable As Boolean) As Boolean

        With p_oDTDiscnt

            If .Rows.Count = 0 Then
                pnDiscAmtV = 0  'VATable Discount
                pnDiscAmtN = 0  'Non-VATable Discount
            End If

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sIDNumber") = IDNumber
            .Rows(.Rows.Count - 1).Item("sDiscCard") = DiscCard
            .Rows(.Rows.Count - 1).Item("nDiscAmnt") = Amount
            .Rows(.Rows.Count - 1).Item("cNoneVATx") = IIf(isVatable = True, 1, 0)

            If isVatable Then
                pnDiscAmtV = pnDiscAmtV + Amount
            Else
                pnDiscAmtN = pnDiscAmtN + Amount
            End If

        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddHeader(Header) As Boolean
    '   - Sets what are to be printed at the Footer Section of Receipt
    '     Could be greetings, remarks, and/or other info.
    '+++++++++++++++++++++++++
    Public Function AddFooter(ByVal Footer As String) As Boolean
        With p_oDTFooter
            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sFootName") = Left(Trim(Footer), 40)
        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddGiftCoupon(GiftSource, Amount)
    '   - Sets the info of Gift Coupon(s) used as payment
    '+++++++++++++++++++++++++
    Public Function AddGiftCoupon( _
            ByVal GiftSource As String, _
            ByVal Amount As Decimal) As Boolean

        With p_oDTGftChk

            If .Rows.Count = 0 Then pnGiftTotl = 0

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sGiftSrce") = GiftSource
            .Rows(.Rows.Count - 1).Item("nGiftAmnt") = Amount

            pnGiftTotl = pnGiftTotl + Amount

        End With

        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddCheck(Bank, CheckNo, CheckDate, Amount)
    '   - Sets the info of check(s) used as payment
    '+++++++++++++++++++++++++
    Public Function AddCheck( _
            ByVal Bank As String, _
            ByVal CheckNo As String, _
            ByVal CheckDate As Date, _
            ByVal Amount As Decimal) As Boolean

        With p_oDTChkPym

            If .Rows.Count = 0 Then pnChckTotl = 0

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sCheckBnk") = Bank
            .Rows(.Rows.Count - 1).Item("sCheckNox") = CheckNo
            .Rows(.Rows.Count - 1).Item("dCheckDte") = CheckDate
            .Rows(.Rows.Count - 1).Item("nCheckAmt") = Amount

            pnChckTotl = pnChckTotl + Amount

        End With


        Return True
    End Function

    '+++++++++++++++++++++++++
    'AddCreditCard(Bank, CardNumber, ApprNo, Amount)
    '   - Sets the info of credit card used as payment
    '+++++++++++++++++++++++++
    Public Function AddCreditCard( _
            ByVal Bank As String, _
            ByVal CardNumber As String, _
            ByVal ApprNo As String, _
            ByVal Amount As Decimal)

        With p_oDTCredit

            If .Rows.Count = 0 Then pnCrdtTotl = 0

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sCardBank") = Bank
            .Rows(.Rows.Count - 1).Item("sCardNoxx") = CardNumber
            .Rows(.Rows.Count - 1).Item("sApprovNo") = ApprNo
            .Rows(.Rows.Count - 1).Item("nCardAmnt") = Amount

            pnCrdtTotl = pnCrdtTotl + Amount

        End With

        Return True
    End Function

    Public Function PrintOR() As Boolean
        If Not AddHeader("The Monarch Hospitality and Tourism Corp") Then
            MsgBox("Invalid Company Name!")
            Return False
        End If

        If Not AddHeader("PEDRITOS PRIMA CAFE") Then
            MsgBox("Invalid Client Name!")
            Return False
        End If

        If Not AddHeader("Tapuac District") Then
            MsgBox("Invalid Client Address!")
            Return False
        End If

        If Not AddHeader("Dagupan City, Pangasinan") Then
            MsgBox("Invalid Town and Address!")
            Return False
        End If

        'If Not AddHeader(p_sCompny) Then
        '    MsgBox("Invalid Company Name!")
        '    Return False
        'End If

        'If Not AddHeader(p_oApp.BranchName) Then
        '    MsgBox("Invalid Client Name!")
        '    Return False
        'End If

        'If Not AddHeader(p_oApp.Address) Then
        '    MsgBox("Invalid Client Address!")
        '    Return False
        'End If

        'If Not AddHeader(p_oApp.TownCity & ", " & p_oApp.Province) Then
        '    MsgBox("Invalid Town and Address!")
        '    Return False
        'End If

        'Add Additional Info To the header
        '---------------------------------
        If Not AddHeader("VAT REG TIN: " & p_sVATReg) Then
            MsgBox("Invalid VAT REG TIN No!")
            Return False
        End If

        If Not AddHeader("MIN : " & p_sPOSNo) Then
            MsgBox("Invalid Machine Identification Number(MIN)!")
            Return False
        End If

        If Not AddHeader("Permit #: " & p_sPermit) Then
            MsgBox("Invalid Permit No!")
            Return False
        End If

        If Not AddHeader("Serial No.: " & p_sSerial) Then
            MsgBox("Invalid Serial No.!")
            Return False
        End If

        Dim Printer_Name As String = "\\192.168.10.14\EPSON LX-310 ESC/P"
        Dim builder As New System.Text.StringBuilder()


        builder.Append(RawPrint.pxePRINT_INIT)          'Initialize Printer
        builder.Append(RawPrint.pxePRINT_PSED)          'Double Strike + Condense + Emphasize

        builder.Append(Space(pxeLFTMGN) & PadCenter(p_oDTHeader(0).Item("sHeadName"), 40) & Environment.NewLine)

        Dim lnCtr As Integer

        builder.Append(RawPrint.pxePRINT_PSDF)          'Condense
        For lnCtr = 1 To p_oDTHeader.Rows.Count - 1
            builder.Append(Space(pxeLFTMGN) & PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40) & Environment.NewLine)
        Next

        'Print Asterisk(*)
        builder.Append(Space(pxeLFTMGN) & "*".PadLeft(40, "*") & Environment.NewLine)

        Dim ls4Print As String
        ls4Print = " QTY" & " " & "DESCRIPTION".PadRight(pxeDSCLEN) & " " & "UPRICE".PadLeft(pxePRCLEN) & " " & "AMOUNT".PadLeft(pxeTTLLEN)
        builder.Append(Space(pxeLFTMGN) & ls4Print & Environment.NewLine)

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1

            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                       UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadRight(pxeDSCLEN) + " "

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN) + " "
                If p_oDTDetail(lnCtr).Item("cVatablex") Then
                    ls4Print = ls4Print + "V"
                End If
            End If
            builder.Append(Space(pxeLFTMGN) & ls4Print & Environment.NewLine)
        Next

        'Print Dash Separator(-)
        builder.Append(Space(pxeLFTMGN) & "-".PadLeft(40, "-") & Environment.NewLine)

        builder.Append(Space(pxeLFTMGN) & " No of Items: " & pnTotalItm & Environment.NewLine & Environment.NewLine)

        'Print TOTAL Sales
        builder.Append(Space(pxeLFTMGN) & " TOTAL".PadLeft(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        'Print Discounts
        If pnDiscAmtV > 0 Then
            builder.Append(Space(pxeLFTMGN) & " Less: Discount(s)".PadLeft(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

            If pnDiscAmtN > 0 Then
                builder.Append(Space(pxeLFTMGN) & "               VAT".PadLeft(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
            End If
        ElseIf pnDiscAmtN > 0 Then
            builder.Append(Space(pxeLFTMGN) & "Less: VAT         ".PadLeft(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Line before Amount Due
        builder.Append(Space(pxeLFTMGN) & "                         -------------" & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_PSED)          'Double Strike + Condense + Emphasize
        'Print Amount Due By subracting the discounts
        builder.Append(Space(pxeLFTMGN) & " Amount Due:              " & Format(pnTotalDue - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)

        builder.Append(RawPrint.pxePRINT_PSDF)          'Condense

        'Print Cash Payments
        If pnCashTotl > 0 Then
            builder.Append(Space(pxeLFTMGN) & " Cash                     " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        End If

        'Print Credit Card Payments
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
                           Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(Space(pxeLFTMGN) & ls4Print & Environment.NewLine)
            Next
        End If

        'Print Check Payments
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
                           Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(Space(pxeLFTMGN) & ls4Print & Environment.NewLine)
            Next
        End If

        'Print Gift Coupon
        If p_oDTGftChk.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
                ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce")).PadRight(24) & " " & _
                           Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
                builder.Append(Space(pxeLFTMGN) & ls4Print & Environment.NewLine)
            Next
        End If

        'Print Line Before change....
        builder.Append(Space(pxeLFTMGN) & "                         -------------" & Environment.NewLine)

        'Print Change
        Dim lnChange As Decimal = pnTotalDue - (pnDiscAmtV + pnDiscAmtN)
        lnChange = (pnCashTotl + pnChckTotl + pnCrdtTotl + pnGiftTotl) - lnChange

        builder.Append(RawPrint.pxePRINT_PSED)          'Double Strike + Condense + Emphasize
        builder.Append(Space(pxeLFTMGN) & " CHANGE     :".PadRight(25) & " " & Format(lnChange, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)


        builder.Append(RawPrint.pxePRINT_PSDF)          'Condense

        'Print Discount Information
        If Not IsNothing(p_oDTDiscnt) Then
            If p_oDTDiscnt.Rows.Count > 0 Then
                If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then

                    builder.Append(Space(pxeLFTMGN) & "///////////////////////////////////////" & Environment.NewLine)

                    For lnCtr = 0 To p_oDTDiscnt.Rows.Count - 1
                        'Print Discount Description
                        builder.Append(Space(pxeLFTMGN) & p_oDTDiscnt(lnCtr).Item("sDiscCard") & Environment.NewLine)
                        'Print Card Number
                        builder.Append(Space(pxeLFTMGN) & p_oDTDiscnt(lnCtr).Item("sIDNumber") & Environment.NewLine)
                    Next
                End If
            End If
        End If

        'Print Credit Card Info
        If p_oDTCredit.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
                builder.Append(Space(pxeLFTMGN) & "///////////////////////////////////////" & Environment.NewLine)
                'Print Credit Card Bank
                builder.Append(Space(pxeLFTMGN) & p_oDTCredit(lnCtr).Item("sCardBank") & Environment.NewLine)

                'Print Card Number/Should hide entire card number
                ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
                ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
                builder.Append(Space(pxeLFTMGN) & ls4Print & Environment.NewLine)
                builder.Append(Space(pxeLFTMGN) & "SWIPED" & Environment.NewLine)
                builder.Append(Space(pxeLFTMGN) & "Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo") & Environment.NewLine)
            Next
        End If

        'Print Check Payment Info
        If p_oDTChkPym.Rows.Count > 0 Then
            For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
                builder.Append(Space(pxeLFTMGN) & "///////////////////////////////////////" & Environment.NewLine)
                builder.Append(Space(pxeLFTMGN) & "Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox") & Environment.NewLine)
                builder.Append(Space(pxeLFTMGN) & "Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk") & Environment.NewLine)
                builder.Append(Space(pxeLFTMGN) & "Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT) & Environment.NewLine)
                builder.Append(Space(pxeLFTMGN) & "Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL) & Environment.NewLine)
            Next
        End If

        'Print Dash Separator(-)
        builder.Append(Space(pxeLFTMGN) & "-".PadLeft(40, "-") & Environment.NewLine & Environment.NewLine)

        'Compute VAT & and other info
        '++++++++++++++++++++++++++++++++++++++
        'VAT is 12 % of sales
        'TODO: load VAT percent of sales from CONFIG
        Dim lnVatPerc As Double = 1.12
        pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
        pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

        'Print VAT Related info
        builder.Append(Space(pxeLFTMGN) & "  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append(Space(pxeLFTMGN) & "  Zero Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append(Space(pxeLFTMGN) & "  VAT Sales             " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine)
        builder.Append(Space(pxeLFTMGN) & "  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN) & Environment.NewLine & Environment.NewLine)

        If psCustName <> "" Then
            builder.Append(Space(pxeLFTMGN) & " Cust Name: " & psCustName & Environment.NewLine)
            builder.Append(Space(pxeLFTMGN) & " Address  : " & psCustAddx & Environment.NewLine)
            builder.Append(Space(pxeLFTMGN) & " TIN #    : " & psCustTINx & Environment.NewLine)
            builder.Append(Space(pxeLFTMGN) & " Bus Style: " & psCustBusx & Environment.NewLine & Environment.NewLine)
        Else
            builder.Append(Space(pxeLFTMGN) & " Cust Name: ----------------------------" & Environment.NewLine)
            builder.Append(Space(pxeLFTMGN) & " Address  : ----------------------------" & Environment.NewLine)
            builder.Append(Space(pxeLFTMGN) & " TIN #    : ----------------------------" & Environment.NewLine)
            builder.Append(Space(pxeLFTMGN) & " Bus Style: ----------------------------" & Environment.NewLine & Environment.NewLine)
        End If

        'Print Cashier
        builder.Append(Space(pxeLFTMGN) & " Cashier: " & "Michael Cuison" & Environment.NewLine)
        builder.Append(Space(pxeLFTMGN) & " Terminal No.: " & p_sTermnl & Environment.NewLine)
        builder.Append(Space(pxeLFTMGN) & " OR No.: " & psReferNox & Environment.NewLine)
        builder.Append(Space(pxeLFTMGN) & " Date : " & Format(pdTransact, xsDATE_TIME) & Environment.NewLine)

        'Print Asterisk(*)
        builder.Append(Space(pxeLFTMGN) & "*".PadLeft(40, "*") & Environment.NewLine)

        'Print the Footer
        For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
            builder.Append(Space(pxeLFTMGN) & PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40) & Environment.NewLine)
        Next

        builder.Append(Chr(&H1D) & "V" & Chr(66) & Chr(0))
        RawPrint.SendStringToPrinter(Printer_Name, builder.ToString())

        Return True
    End Function

    'Public Function PrintOR() As Boolean
    '    If Not AddHeader("The Monarch Hospitality and Tourism Corp") Then
    '        MsgBox("Invalid Company Name!")
    '        Return False
    '    End If

    '    If Not AddHeader("PEDRITOS PRIMA CAFE") Then
    '        MsgBox("Invalid Client Name!")
    '        Return False
    '    End If

    '    If Not AddHeader("Tapuac District") Then
    '        MsgBox("Invalid Client Address!")
    '        Return False
    '    End If

    '    If Not AddHeader("Dagupan City, Pangasinan") Then
    '        MsgBox("Invalid Town and Address!")
    '        Return False
    '    End If

    '    'If Not AddHeader(p_sCompny) Then
    '    '    MsgBox("Invalid Company Name!")
    '    '    Return False
    '    'End If

    '    'If Not AddHeader(p_oApp.BranchName) Then
    '    '    MsgBox("Invalid Client Name!")
    '    '    Return False
    '    'End If

    '    'If Not AddHeader(p_oApp.Address) Then
    '    '    MsgBox("Invalid Client Address!")
    '    '    Return False
    '    'End If

    '    'If Not AddHeader(p_oApp.TownCity & ", " & p_oApp.Province) Then
    '    '    MsgBox("Invalid Town and Address!")
    '    '    Return False
    '    'End If

    '    'Add Additional Info To the header
    '    '---------------------------------
    '    If Not AddHeader("VAT REG TIN: " & p_sVATReg) Then
    '        MsgBox("Invalid VAT REG TIN No!")
    '        Return False
    '    End If

    '    If Not AddHeader("MIN : " & p_sPOSNo) Then
    '        MsgBox("Invalid Machine Identification Number(MIN)!")
    '        Return False
    '    End If

    '    If Not AddHeader("Permit #: " & p_sPermit) Then
    '        MsgBox("Invalid Permit No!")
    '        Return False
    '    End If

    '    If Not AddHeader("Serial No.: " & p_sSerial) Then
    '        MsgBox("Invalid Serial No.!")
    '        Return False
    '    End If

    '    Dim loPrint As ggcLRReports.clsDirectPrintSF
    '    loPrint = New ggcLRReports.clsDirectPrintSF
    '    'loPrint.PrintFont = New Font("Courier New", 10)
    '    loPrint.PrintBegin()

    '    Dim lnCtr As Integer
    '    Dim lnRowCtr As Integer = 0
    '    Dim ls4Print As String

    '    'Print the header
    '    For lnCtr = 0 To p_oDTHeader.Rows.Count - 1
    '        loPrint.Print(lnRowCtr, 0, PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40))
    '        lnRowCtr = lnRowCtr + 1
    '    Next

    '    'Print Asterisk(*)
    '    loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print TITLE
    '    ls4Print = "QTY" + " " + "DESCRIPTION".PadLeft(pxeDSCLEN) + " " + "UPRICE".PadLeft(pxePRCLEN) + " " + "AMOUNT".PadLeft(pxeTTLLEN)
    '    loPrint.Print(lnRowCtr, 0, ls4Print)
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Detail of Sales
    '    For lnCtr = 0 To p_oDTDetail.Rows.Count - 1

    '        ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
    '                   UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadLeft(pxeDSCLEN) + " "

    '        If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
    '            ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) + " "
    '            ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN) + " "
    '            If p_oDTDetail(lnCtr).Item("cVatablex") Then
    '                ls4Print = ls4Print + "V"
    '            End If
    '        End If
    '        loPrint.Print(lnRowCtr, 0, ls4Print)

    '        lnRowCtr = lnRowCtr + 1
    '    Next

    '    'Print Dash Separator(-)
    '    loPrint.Print(lnRowCtr, 0, "-".PadLeft(40, "-"))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Dash Separator(*)
    '    loPrint.Print(lnRowCtr, 0, " No of Items: " & pnTotalItm)
    '    lnRowCtr = lnRowCtr + 2  'There should be space after this part...

    '    'Print TOTAL Sales
    '    loPrint.Print(lnRowCtr, 0, " TOTAL".PadLeft(25) & " " & Format(pnTotalDue, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Discounts
    '    If pnDiscAmtV > 0 Then
    '        loPrint.Print(lnRowCtr, 0, " Less: Discount(s)".PadLeft(25) & " " & Format(pnDiscAmtV, xsDECIMAL).PadLeft(pxeREGLEN))
    '        lnRowCtr = lnRowCtr + 1

    '        If pnDiscAmtN > 0 Then
    '            loPrint.Print(lnRowCtr, 0, "               VAT".PadLeft(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN))
    '            lnRowCtr = lnRowCtr + 1
    '        End If
    '    ElseIf pnDiscAmtN > 0 Then
    '        loPrint.Print(lnRowCtr, 0, "Less: VAT         ".PadLeft(25) & " " & Format(pnDiscAmtN, xsDECIMAL).PadLeft(pxeREGLEN))
    '        lnRowCtr = lnRowCtr + 1
    '    End If

    '    'Print Line before Amount Due
    '    loPrint.Print(lnRowCtr, 0, "                         -------------")
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Amount Due By subracting the discounts
    '    loPrint.Print(lnRowCtr, 0, " Amount Due:              " & Format(pnTotalDue - (pnDiscAmtV + pnDiscAmtN), xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Cash Payments
    '    If pnCashTotl > 0 Then
    '        loPrint.Print(lnRowCtr, 0, " Cash                     " & Format(pnCashTotl, xsDECIMAL).PadLeft(pxeREGLEN))
    '        lnRowCtr = lnRowCtr + 1
    '    End If

    '    'Print Credit Card Payments
    '    If p_oDTCredit.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
    '            ls4Print = " " & UCase(Left(p_oDTCredit(lnCtr).Item("sCardBank"), 17)).PadRight(24) & " " & _
    '                       Format(p_oDTCredit(lnCtr).Item("nCardAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
    '            loPrint.Print(lnRowCtr, 0, ls4Print)
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Check Payments
    '    If p_oDTChkPym.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
    '            ls4Print = " " & UCase(p_oDTChkPym(lnCtr).Item("sCheckNox")).PadRight(24) & " " & _
    '                       Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL).PadLeft(pxeREGLEN)
    '            loPrint.Print(lnRowCtr, 0, ls4Print)
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Gift Coupon
    '    If p_oDTGftChk.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTGftChk.Rows.Count - 1
    '            ls4Print = " " & UCase(p_oDTGftChk(lnCtr).Item("sGiftSrce")).PadRight(24) & " " & _
    '                       Format(p_oDTGftChk(lnCtr).Item("nGiftAmnt"), xsDECIMAL).PadLeft(pxeREGLEN)
    '            loPrint.Print(lnRowCtr, 0, ls4Print)
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Line Before change....
    '    loPrint.Print(lnRowCtr, 0, "                         -------------")
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Change
    '    Dim lnChange As Decimal = pnTotalDue - (pnDiscAmtV + pnDiscAmtN)
    '    lnChange = (pnCashTotl + pnChckTotl + pnCrdtTotl + pnGiftTotl) - lnChange
    '    loPrint.Print(lnRowCtr, 0, " CHANGE     :".PadLeft(25) & " " & Format(lnChange, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Discount Information
    '    If p_oDTDiscnt.Rows.Count > 0 Then
    '        If p_oDTDiscnt(0).Item("sDiscCard") <> "" Then
    '            loPrint.Print(lnRowCtr, 0, "///////////////////////////////////////")
    '            lnRowCtr = lnRowCtr + 1

    '            For lnCtr = 0 To p_oDTDiscnt.Rows.Count - 1
    '                'Print Discount Description
    '                loPrint.Print(lnRowCtr, 0, p_oDTDiscnt(lnCtr).Item("sDiscCard"))
    '                lnRowCtr = lnRowCtr + 1

    '                'Print Card Number
    '                loPrint.Print(lnRowCtr, 0, p_oDTDiscnt(lnCtr).Item("sIDNumber"))
    '                lnRowCtr = lnRowCtr + 1

    '            Next
    '        End If
    '    End If

    '    'Print Credit Card Info
    '    If p_oDTCredit.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTCredit.Rows.Count - 1
    '            loPrint.Print(lnRowCtr, 0, "///////////////////////////////////////")
    '            lnRowCtr = lnRowCtr + 1
    '            'Print Credit Card Bank
    '            loPrint.Print(lnRowCtr, 0, p_oDTCredit(lnCtr).Item("sCardBank"))
    '            lnRowCtr = lnRowCtr + 1

    '            'Print Card Number/Should hide entire card number
    '            ls4Print = p_oDTCredit(lnCtr).Item("sCardNoxx")
    '            ls4Print = Left(ls4Print, 5) & "".PadLeft(ls4Print.Length - 9, "*") & Right(ls4Print, 4)
    '            loPrint.Print(lnRowCtr, 0, ls4Print)
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "SWIPED")
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Approval Code: " & p_oDTCredit(lnCtr).Item("sApprovNo"))
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Check Payment Info
    '    If p_oDTChkPym.Rows.Count > 0 Then
    '        For lnCtr = 0 To p_oDTChkPym.Rows.Count - 1
    '            loPrint.Print(lnRowCtr, 0, "///////////////////////////////////////")
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Check No: " & p_oDTChkPym(lnCtr).Item("sCheckNox"))
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Bank    : " & p_oDTChkPym(lnCtr).Item("sCheckBnk"))
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Date:   : " & Format(p_oDTChkPym(lnCtr).Item("dCheckDte"), xsDATE_SHORT))
    '            lnRowCtr = lnRowCtr + 1

    '            loPrint.Print(lnRowCtr, 0, "Amount  : " & Format(p_oDTChkPym(lnCtr).Item("nCheckAmt"), xsDECIMAL))
    '            lnRowCtr = lnRowCtr + 1
    '        Next
    '    End If

    '    'Print Dash Separator(-)
    '    loPrint.Print(lnRowCtr, 0, "-".PadLeft(40, "-"))
    '    lnRowCtr = lnRowCtr + 2 'There should be space after this part..

    '    'Compute VAT & and other info
    '    '++++++++++++++++++++++++++++++++++++++
    '    'VAT is 12 % of sales
    '    'TODO: load VAT percent of sales from CONFIG
    '    Dim lnVatPerc As Double = 1.12
    '    pnVatblSle = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) / lnVatPerc
    '    pnVatAmntx = (pnTotalDue - (pnDiscAmtV + pnDiscAmtN + pnZroRtSle + pnVatExSle)) - pnVatblSle

    '    'Print VAT Related info
    '    loPrint.Print(lnRowCtr, 0, "  VAT Exempt Sales      " & Format(pnVatExSle, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "  Zero Rated Sales      " & Format(pnZroRtSle, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "  VAT Sales             " & Format(pnVatblSle, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, "  VAT Amount            " & Format(pnVatAmntx, xsDECIMAL).PadLeft(pxeREGLEN))
    '    lnRowCtr = lnRowCtr + 2 'There should be space after this part..

    '    If psCustName <> "" Then
    '        loPrint.Print(lnRowCtr, 0, " Cust Name: " & psCustName)
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " Address  : " & psCustAddx)
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " TIN #    : " & psCustTINx)
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " Bus Style:" & psCustBusx)
    '        lnRowCtr = lnRowCtr + 2 'There should be space after this part..
    '    Else
    '        loPrint.Print(lnRowCtr, 0, " Cust Name: ----------------------------")
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " Address  : ----------------------------")
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " TIN #    : ----------------------------")
    '        lnRowCtr = lnRowCtr + 1

    '        loPrint.Print(lnRowCtr, 0, " Bus Style: ----------------------------")
    '        lnRowCtr = lnRowCtr + 2 'There should be space after this part..
    '    End If

    '    'Print Cashier
    '    loPrint.Print(lnRowCtr, 0, " Cashier: " & "Juan Dela Cruz") 'psCashrNme
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, " Terminal No.: " & p_sTermnl)
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, " OR No.: " & psReferNox)
    '    lnRowCtr = lnRowCtr + 1

    '    loPrint.Print(lnRowCtr, 0, " Date : " & Format(pdTransact, xsDATE_TIME))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print Asterisk(*)
    '    loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
    '    lnRowCtr = lnRowCtr + 1

    '    'Print the Footer
    '    For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
    '        loPrint.Print(lnRowCtr, 0, PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40))
    '        lnRowCtr = lnRowCtr + 1
    '    Next

    '    loPrint.PrintEnd()

    '    Return True
    'End Function

    Private Function PadCenter(source As String, length As Integer) As String
        Dim spaces As Integer = length - source.Length
        Dim padLeft As Integer = spaces / 2 + source.Length
        Return source.PadLeft(padLeft, " ").PadRight(length, " ")
    End Function

    Private Sub createDetail()
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int16"))
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 14
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nTotlAmnt", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cDetailxx", System.Type.GetType("System.String")).MaxLength = 1

        'Consider All Sales to be VATABLE
        p_oDTDetail.Columns.Add("cVatablex", System.Type.GetType("System.String")).MaxLength = 1

        'Header Table
        p_oDTHeader = New DataTable("Header")
        p_oDTHeader.Columns.Add("sHeadName", System.Type.GetType("System.String")).MaxLength = 40

        'Footer Table
        p_oDTFooter = New DataTable("Footer")
        p_oDTFooter.Columns.Add("sFootName", System.Type.GetType("System.String")).MaxLength = 40

        p_oDTDiscnt = New DataTable("Discount")
        p_oDTDiscnt.Columns.Add("sIDNumber", System.Type.GetType("System.String")).MaxLength = 35
        p_oDTDiscnt.Columns.Add("sDiscCard", System.Type.GetType("System.String")).MaxLength = 35
        p_oDTDiscnt.Columns.Add("cNoneVATx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDiscnt.Columns.Add("nDiscAmnt", System.Type.GetType("System.Decimal"))
    End Sub

    Private Sub createGiftCheck()
        p_oDTGftChk = New DataTable("GiftChec")
        p_oDTGftChk.Columns.Add("nGiftAmnt", System.Type.GetType("System.Decimal"))
        p_oDTGftChk.Columns.Add("sGiftSrce", System.Type.GetType("System.String")).MaxLength = 23
    End Sub

    Private Sub createCheck()
        p_oDTChkPym = New DataTable("Check")
        p_oDTChkPym.Columns.Add("nCheckAmt", System.Type.GetType("System.Decimal"))
        p_oDTChkPym.Columns.Add("sCheckBnk", System.Type.GetType("System.String")).MaxLength = 32
        p_oDTChkPym.Columns.Add("sCheckNox", System.Type.GetType("System.String")).MaxLength = 23
        p_oDTChkPym.Columns.Add("dCheckDte", System.Type.GetType("System.DateTime"))
    End Sub

    Private Sub createCreditCard()
        p_oDTCredit = New DataTable("CreditCard")
        p_oDTCredit.Columns.Add("nCardAmnt", System.Type.GetType("System.Decimal"))
        p_oDTCredit.Columns.Add("sCardBank", System.Type.GetType("System.String")).MaxLength = 32
        p_oDTCredit.Columns.Add("sCardNoxx", System.Type.GetType("System.String")).MaxLength = 23
        p_oDTCredit.Columns.Add("sApprovNo", System.Type.GetType("System.String")).MaxLength = 10
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTMaster = Nothing
        p_oDTDetail = Nothing
        p_oDTChkPym = Nothing
        p_oDTCredit = Nothing
        p_oDTGftChk = Nothing

        p_oDTHeader = Nothing
        p_oDTFooter = Nothing
        p_oDTDiscnt = Nothing

        'Get Cashier Name from GRider
        psCashrNme = p_oApp.UserName

        Call createDetail()
        Call createCheck()
        Call createCreditCard()
        Call createGiftCheck()

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")      'MIN
        p_sVATReg = Environment.GetEnvironmentVariable("REG-TIN-No")     'VAT REG No.
        p_sCompny = Environment.GetEnvironmentVariable("RMS-CLT-NM")
    End Sub

    'Public Sub testOR()
    '    Dim loReceipt As ggcMiscParam.PRN_Receipt
    '    loReceipt = New ggcMiscParam.PRN_Receipt(p_oAppDriver)
    '    'If loReceipt.InitMachine() Then
    '    'Set Details
    '    loReceipt.AddDetail(2, "123456789012345", 2500, True)
    '    loReceipt.AddDetail(1, "CLUBHSE SANDWCH", 140, True)

    '    loReceipt.CashPayment = 4500
    '    loReceipt.ReferenceNo = "00172015"
    '    loReceipt.NonVatSales = 2500
    '    loReceipt.Transaction_Date = p_oAppDriver.SysDate

    '    If Not loReceipt.PrintOR Then
    '        MsgBox("Can't print OR")
    '        Exit Sub
    '    End If
    '    'End If
    'End Sub
End Class
