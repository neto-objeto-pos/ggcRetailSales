'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Sales Order Printing
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
' Sales Order Printing Sample
'1234567890123456789012345678901234567890
'
'             MONARK HOTEL
'   PEDRITO'S BAKESHOP AND RESTAURANT
'   Mc Arthur Highway, Tapuac District
'       Dagupan City, Pangasinan
'****************************************
'QTY DESCRIPTION       UPRICE     AMOUNT 
'  2 123456789012345 2,500.00   5,000.00
'  1 CLUBHSE SANDWCH   140.00     140.00
'----------------------------------------
'  3 item(s)                   
'
' TABLE NO: XXX
' CTRL NO : XXXXXXXX
' WAITER  : Marlon A. Sayson
' Date    : 11/18/2016 09:15 am
'****************************************
'
' ==========================================================================================
'  kalyptus [ 11/21/2016 09:07 am ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€

Imports ADODB
Imports ggcAppDriver
Imports System.Drawing

Public Class PRN_Order
    Private p_oApp As GRider
    Private p_sCompny As String     'Company  : MONARK HOTEL

    Private p_oDTDetail As DataTable
    Private p_oDTHeader As DataTable
    Private p_oDTFooter As DataTable

    Private pnTotalItm As Decimal
    Private psContrlNo As String
    Private psWaiterxx As String
    Private psTableNox As String
    Private pdTransact As Date

    Private Const pxeQTYLEN As Integer = 3  '+ 1
    Private Const pxeDSCLEN As Integer = 15 '+ 1
    Private Const pxePRCLEN As Integer = 8  '+ 1
    Private Const pxeTTLLEN As Integer = 10
    Private Const pxeREGLEN As Integer = 12

    Public Property Transaction_Date() As Date
        Get
            Return pdTransact
        End Get
        Set(ByVal value As Date)
            pdTransact = value
        End Set
    End Property

    Public Property ControlNo() As String
        Get
            Return psContrlNo
        End Get
        Set(ByVal value As String)
            psContrlNo = value
        End Set
    End Property

    Public Property Waiter() As String
        Get
            Return psWaiterxx
        End Get
        Set(ByVal value As String)
            psWaiterxx = value
        End Set
    End Property

    Public Property TableNo() As String
        Get
            Return psTableNox
        End Get
        Set(ByVal value As String)
            psTableNox = value
        End Set
    End Property

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
            ByVal UnitPrice As Decimal) As Boolean

        With p_oDTDetail

            If .Rows.Count = 0 Then
                pnTotalItm = 0  'Initialize Total Item Sold
            End If

            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("nQuantity") = Quantity
            .Rows(.Rows.Count - 1).Item("sBriefDsc") = Left(Description, 15)
            .Rows(.Rows.Count - 1).Item("nUnitPrce") = UnitPrice
            .Rows(.Rows.Count - 1).Item("nTotlAmnt") = Quantity * UnitPrice

            pnTotalItm = pnTotalItm + Quantity

        End With

        Return True
    End Function

    Public Function AddFooter(ByVal Footer As String) As Boolean
        With p_oDTFooter
            .Rows.Add(.NewRow)
            .Rows(.Rows.Count - 1).Item("sFootName") = Left(Trim(Footer), 40)
        End With
        Return True
    End Function

    Public Function PrintOrder() As Boolean
        Dim loPrint As ggcLRReports.clsDirectPrintSF
        loPrint = New ggcLRReports.clsDirectPrintSF
        loPrint.PrintFont = New Font("Courier New", 8)
        loPrint.PrintBegin()

        Dim lnCtr As Integer
        Dim lnRowCtr As Integer = 0
        Dim ls4Print As String

        If Not AddHeader(p_sCompny) Then
            MsgBox("Invalid Company Name!")
            Return False
        End If

        If Not AddHeader(p_oApp.BranchName) Then
            MsgBox("Invalid Client Name!")
            Return False
        End If

        If Not AddHeader(p_oApp.Address) Then
            MsgBox("Invalid Client Address!")
            Return False
        End If

        If Not AddHeader(p_oApp.TownCity & ", " & p_oApp.Province) Then
            MsgBox("Invalid Town and Address!")
            Return False
        End If

        'Print the header
        For lnCtr = 0 To p_oDTHeader.Rows.Count - 1
            loPrint.Print(lnRowCtr, 0, PadCenter(p_oDTHeader(lnCtr).Item("sHeadName"), 40))
            lnRowCtr = lnRowCtr + 1
        Next

        'Print Asterisk(*)
        loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
        lnRowCtr = lnRowCtr + 1

        'Print TITLE
        ls4Print = "QTY" + " " + "DESCRIPTION".PadLeft(pxeDSCLEN) + " " + "UPRICE".PadLeft(pxePRCLEN) + " " + "AMOUNT".PadLeft(pxeTTLLEN)
        loPrint.Print(lnRowCtr, 0, ls4Print)
        lnRowCtr = lnRowCtr + 1

        'Print Detail of Sales
        For lnCtr = 0 To p_oDTDetail.Rows.Count - 1

            ls4Print = Format(p_oDTDetail(lnCtr).Item("nQuantity"), "0").PadLeft(pxeQTYLEN) + " " + _
                       UCase(p_oDTDetail(lnCtr).Item("sBriefDsc")).PadLeft(pxeDSCLEN) + " "

            If p_oDTDetail(lnCtr).Item("nUnitPrce") > 0 Then
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nUnitPrce"), xsDECIMAL).PadLeft(pxePRCLEN) & " "
                ls4Print = ls4Print + Format(p_oDTDetail(lnCtr).Item("nTotlAmnt"), xsDECIMAL).PadLeft(pxeTTLLEN) & " "
            End If
            loPrint.Print(lnRowCtr, 0, ls4Print)

            lnRowCtr = lnRowCtr + 1
        Next

        'Print Dash Separator(-)
        loPrint.Print(lnRowCtr, 0, "-".PadLeft(40, "-"))
        lnRowCtr = lnRowCtr + 1

        'Print Dash Separator(*)
        loPrint.Print(lnRowCtr, 0, " No of Items: " & Double.Parse(pnTotalItm))
        lnRowCtr = lnRowCtr + 2  'There should be space after this part...


        loPrint.Print(lnRowCtr, 0, " TABLE NO: " & psTableNox)
        lnRowCtr = lnRowCtr + 1

        loPrint.Print(lnRowCtr, 0, " CTRL NO : " & psContrlNo)
        lnRowCtr = lnRowCtr + 1

        loPrint.Print(lnRowCtr, 0, " WAITER  : " & psWaiterxx)
        lnRowCtr = lnRowCtr + 1

        loPrint.Print(lnRowCtr, 0, " Date    : " & Format(pdTransact, xsDATE_TIME))
        lnRowCtr = lnRowCtr + 1

        'Print Asterisk(*)
        loPrint.Print(lnRowCtr, 0, "*".PadLeft(40, "*"))
        lnRowCtr = lnRowCtr + 1

        'Print the Footer
        For lnCtr = 0 To p_oDTFooter.Rows.Count - 1
            loPrint.Print(lnRowCtr, 0, PadCenter(p_oDTFooter(lnCtr).Item("sFootName"), 40))
            lnRowCtr = lnRowCtr + 1
        Next

        loPrint.PrintEnd()

        Return True
    End Function


    Private Sub createDetail()
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int16"))
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 15
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nTotlAmnt", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cDetailxx", System.Type.GetType("System.String")).MaxLength = 1

        'Header Table
        p_oDTHeader = New DataTable("Header")
        p_oDTHeader.Columns.Add("sHeadName", System.Type.GetType("System.String")).MaxLength = 40

        'Footer Table
        p_oDTFooter = New DataTable("Footer")
        p_oDTFooter.Columns.Add("sFootName", System.Type.GetType("System.String")).MaxLength = 40
    End Sub

    Private Function PadCenter(source As String, length As Integer) As String
        Dim spaces As Integer = length - source.Length
        Dim padLeft As Integer = spaces / 2 + source.Length
        Return source.PadLeft(padLeft, " ").PadRight(length, " ")
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTDetail = Nothing
        p_oDTHeader = Nothing
        p_oDTFooter = Nothing

        p_sCompny = Environment.GetEnvironmentVariable("RMS-CLT-NM")

        Call createDetail()
    End Sub

    'Public Sub testOrder()
    '    Dim loReceipt As ggcMiscParam.PRN_Order

    '    loReceipt = New ggcMiscParam.PRN_Order(p_oApp)
    '    'If loReceipt.InitMachine() Then
    '    'Set Header
    '    loReceipt.AddHeader("MONARK HOTEL")
    '    loReceipt.AddHeader("PEDRITO'S BAKESHOP AND RESTAURANT")
    '    loReceipt.AddHeader("Mc Arthur Highway, Tapuac District")
    '    loReceipt.AddHeader("Dagupan City, Pangasinan")

    '    'Set Details
    '    loReceipt.AddDetail(2, "123456789012345", 2500)
    '    loReceipt.AddDetail(1, "CLUBHSE SANDWCH", 140)

    '    loReceipt.Transaction_Date = Now()
    '    loReceipt.TableNo = "5"
    '    loReceipt.Waiter = "Marlon A. Sayson"
    '    loReceipt.ControlNo = "250014"

    '    If Not loReceipt.PrintOrder Then
    '        MsgBox("Can't print Order")
    '        Exit Sub
    '    End If
    '    'End If
    'End Sub

End Class
