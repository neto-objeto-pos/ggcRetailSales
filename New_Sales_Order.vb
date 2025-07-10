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
Imports ggcRetailParams
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Win32

Public Class New_Sales_Order
    Private Const xsSignature As String = "08220326"
    Private p_oApp As GRider
    Private p_oDTDetail As DataTable
    Private p_oDTMaster As DataTable

    Private p_oDiscount As Discount
    Private p_oDtaDiscx As DataTable
    Private p_oTable As TableMaster
    Private p_oSC As New MySqlCommand

    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sVATReg As String
    Private p_sTermnl As String
    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_dPTUFrm As Date
    Private p_dPTUTru As Date
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_sAccrdt As String     'Accrdt No: 038-227471337-000028
    Private p_dAccFrm As Date
    Private p_dAccTru As Date
    Private p_cTrnMde As Char

    Private p_nQuantity As Integer
    Private p_sWaiterID As String
    Private p_nTranTotl As Double
    Private p_nComplmnt As Double
    Public p_bSChargex As Boolean
    Public p_nTotalSales As Double
    Private p_nSChargex As Decimal
    Private p_sTrantype As String

    Private p_oFormChargeCriteria As frmChargeCriteria

    Public p_nBill As Double
    Public p_nCharge As Double

    Private p_bExisting As xeLogical
    Private p_nRow As Integer
    Private p_bHasDisct As Boolean
    Private p_bWasSplitted As Boolean

    Private p_bShowMsg As Boolean
    Private p_sParent As String
    Private p_bNotify As Boolean ' this must be set on configuration table for notifications eg promos

    Private p_sBranchCd As String 'Branch code of the transaction to retrieve
    Private p_sBranchNm As String 'Branch Name of the transaction to retrieve
    Private p_sAddressx As String

    Private p_sUserIDxx As String
    Private p_sUserName As String
    Private p_sLogNamex As String
    Private p_nUserLevl As Integer

    Private p_sCashierx As String
    Private p_sLogName As String


    Public Const pxeJavaPath As String = "D:\GGC_Java_Systems\"

    Public Const pxeJavaPathTemp As String = "D:\GGC_Java_Systems\temp\"

    '0->With Open Sales Order From Previous Sale;
    '1->Sales for the Day was already closed;
    '2->Sales for the Day is Ok;
    '3->Error Printing TXReading 
    '4->User is not allowed to enter Sales Transaction
    Private p_nSaleStat As Integer

    Private p_bValidDailySales As Boolean
    Private p_dPOSDatex As Date

    Private Const pxeMasTable As String = "SO_Master"
    Private Const pxeDetTable As String = "SO_Detail"
    Private Const pxeFinGoods As String = "FsGd"
    Private Const pxeDelimtr As String = "»"
    Private Const pxeCtrFormat As String = "000000"
    Private Const pxeSourceCde As String = "SO"

    'jovan added this global variable
    Private p_nNoClient As Integer
    Private p_nWithDisc As Integer
    Private p_nTableNo As Integer
    Private p_sMergeTb As String
    Private pnCharge As Integer



    Protected p_bCancelled As Boolean
    Protected p_sQRCode As String
    'Protected p_oFormQRScanner As frmQRCode
    'Protected p_oFormQRResult As frmQRResult

    Public Event MasterRetreived(ByVal Index As Integer,
                                 ByVal Value As Object)
    Public Event DetailRetrieved(ByVal Row As Integer,
                                 ByVal Index As Integer,
                                 ByVal Value As Object)    'Property Detail(Integer)

    Public Event DisplayFromRow(ByVal Row As Integer)
    Public Event DisplayRow(ByVal Row As Integer)

    Public WriteOnly Property ValidDailySales() As Boolean
        Set(ByVal value As Boolean)
            p_bValidDailySales = value
        End Set
    End Property

    Public Property cTranMode() As String
        Get
            Return p_cTrnMde
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Public Property WasSplitted() As Boolean
        Get
            Return p_bWasSplitted
        End Get
        Set(ByVal value As Boolean)
            p_bWasSplitted = value
        End Set
    End Property


    Public Property HasDiscount() As Boolean
        Get
            Return p_bHasDisct
        End Get
        Set(ByVal value As Boolean)
            p_bHasDisct = value
        End Set
    End Property

    Public WriteOnly Property PosDate() As Date
        Set(ByVal Value As Date)
            p_dPOSDatex = Value
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
    Public Property TranType() As Integer
        Get
            Return p_sTrantype
        End Get
        Set(ByVal value As Integer)
            p_sTrantype = value
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
    Public Property setSChargex As Double
        Get
            Return pnCharge
        End Get
        Set(ByVal value As Double)
            pnCharge = value
        End Set
    End Property
    Public WriteOnly Property setTranTotal As Double
        Set(ByVal value As Double)
            p_nTotalSales = value
        End Set
    End Property
    Public WriteOnly Property setBill As Double
        Set(ByVal value As Double)
            p_nBill = value
        End Set
    End Property
    Public WriteOnly Property servicecharge As Double
        Set(ByVal value As Double)
            p_nCharge = value
        End Set
    End Property

    Public ReadOnly Property POSNumber()
        Get
            Return p_sTermnl
        End Get
    End Property

    Public ReadOnly Property Accreditation()
        Get
            Return p_sAccrdt
        End Get
    End Property

    Public ReadOnly Property SerialNo()
        Get
            Return p_sSerial
        End Get
    End Property

    'We should access the value of this property after creating a new Daily_Summary record...
    Public WriteOnly Property SalesStatus() As Integer
        Set(ByVal value As Integer)
            p_nSaleStat = value
        End Set
    End Property
    Public ReadOnly Property Address()
        Get
            Return p_sAddressx
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
                Case "nvatsales"
                    Return p_oDTMaster(0).Item("nVATSales")
                Case "nvatamtxx"
                    Return p_oDTMaster(0).Item("nVATAmtxx")
                Case "ndiscount" 'Regular Discount
                    Return p_oDTMaster(0).Item("nDiscount")
                Case "nvatdiscx" '12% Vat Discount
                    Return p_oDTMaster(0).Item("nVatDiscx")
                Case "npwddiscx" 'Senior/PWD Discount
                    Return p_oDTMaster(0).Item("nPWDDiscx")
                Case "ndiscntbl" 'discountable
                    Return p_oDTMaster(0).Item("nDiscntbl")
                Case "nnonvatxx"
                    Return p_oDTMaster(0).Item("nNonVATxx")
                Case "nvoidtotl"
                    Return p_oDTMaster(0).Item("nVoidTotl")
                Case "ctrantype"
                    Return p_oDTMaster(0).Item("cTranType")
                Case "scashrnme"
                    If p_oDTMaster(0).Item("sCashrNme") = "" Then
                        If p_oDTMaster(0).Item("sCashierx") <> "" Then
                            p_oDTMaster(0).Item("sCashrNme") = getUserName(p_oDTMaster(0).Item("sCashierx"))
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
                Case "nschargex"
                    Return p_oDTMaster(0).Item("nSChargex")
                Case "noccupnts"
                    Return p_oDTMaster(0).Item("noccupnts")
                Case "stablenox"
                    Return p_oDTMaster(0).Item("sTableNox")
                Case "smergeidx"
                    Return p_oDTMaster(0).Item("sMergeIDx")
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
                            p_oDTMaster(0).Item("sCashrNme") = getUserName(p_oDTMaster(0).Item("sCashierx"))
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
                Case 0 To 4, 7 To 22
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
                Case "sbriefdsc"
                Case "nunitprce"
                Case "creversex"
                Case "nquantity"
                Case "ndiscount"
                Case "nadddiscx"
                Case "cprintedx"
                Case "ncomplmnt"
                Case "cdetailxx"
                Case "creversed"
                Case "sstockidx"
                Case "cwthpromo"
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

    Public ReadOnly Property Complement() As Double
        Get
            Return p_nComplmnt
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

    Function LoadOrder() As Boolean
        Dim lsTransNox As String

        'iMac 2017-01-19
        If p_bValidDailySales = False Then
            If hasOpenTransaction() = False Then
                MsgBox("Application needs to close. Please restart the system.", MsgBoxStyle.Information, "Notice")
            End If

            Return True
        End If

        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = AddCondition(getSQ_Order, "sTableNox = ''")

        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_bExisting = False
            Return False
        End If

        p_oDTMaster(0).Item("sTransNox") = loDT(0).Item("sTransNox")
        p_oDTMaster(0).Item("dTransact") = loDT(0).Item("dTransact")
        p_oDTMaster(0).Item("nContrlNo") = loDT(0).Item("nContrlNo")
        p_oDTMaster(0).Item("nTranTotl") = loDT(0).Item("nTranTotl")
        p_oDTMaster(0).Item("sCashierx") = loDT(0).Item("sCashierx")
        p_oDTMaster(0).Item("sCashrNme") = getUserName(loDT(0).Item("sCashierx"))
        p_oDTMaster(0).Item("sWaiterID") = loDT(0).Item("sWaiterID")
        p_oDTMaster(0).Item("sWaiterNm") = getWaiter(p_oDTMaster(0).Item("sWaiterID"))
        p_oDTMaster(0).Item("sReceiptx") = loDT(0).Item("sReceiptx")
        p_oDTMaster(0).Item("sMergeIDx") = loDT(0).Item("sMergeIDx")
        p_oDTMaster(0).Item("sTableNox") = IIf(loDT(0).Item("sTableNox") = "", 0, loDT(0).Item("sTableNox"))
        p_oDTMaster(0).Item("sBillNmbr") = loDT(0).Item("sBillNmbr")
        p_oDTMaster(0).Item("nPrntBill") = loDT(0).Item("nPrntBill")
        p_oDTMaster(0).Item("dPrntBill") = loDT(0).Item("dPrntBill")

        p_oDTMaster(0).Item("nSChargex") = 0
        p_oDTMaster(0).Item("cSChargex") = loDT(0).Item("cSChargex")
        p_bSChargex = IIf(loDT(0).Item("cSChargex") = "1", True, False)

        p_sTrantype = IIf(p_oDTMaster(0).Item("cTranType") = "", "0", p_oDTMaster(0).Item("cTranType"))
        p_oDTMaster(0).Item("cTranType") = loDT(0).Item("cTranType")
        p_nTableNo = IIf(p_oDTMaster(0).Item("sTableNox") = "", 0, p_oDTMaster(0).Item("sTableNox"))

        lsSQL = AddCondition(getSQ_Discount, "sSourceNo = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        loDT = p_oApp.ExecuteQuery(lsSQL)

        p_oDTMaster(0).Item("nDiscount") = 0.00
        p_oDTMaster(0).Item("nVatDiscx") = 0.00
        p_oDTMaster(0).Item("nPWDDiscx") = 0.00

        p_oDTMaster(0).Item("nVATSales") = 0.00
        p_oDTMaster(0).Item("nVATAmtxx") = 0.00
        p_oDTMaster(0).Item("nNonVATxx") = 0.00

        If loDT.Rows.Count > 0 Then
            p_nNoClient = loDT(0).Item("nNoClient")
            p_nWithDisc = loDT(0).Item("nWithDisc")
        End If

        If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
            lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        Else
            lsSQL = AddCondition(getSQ_Detail_WithMerge, "c.sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx")))
        End If

        loDT = p_oApp.ExecuteQuery(lsSQL)

        Dim lnCtr As Integer
        Call createDetail()

        p_sMergeTb = ""
        lsTransNox = ""
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
            p_oDTDetail(lnCtr).Item("cPrintedx") = loDT(lnCtr).Item("cPrintedx")
            p_oDTDetail(lnCtr).Item("sCategrID") = loDT(lnCtr).Item("sCategrID")

            p_oDTDetail(lnCtr).Item("sTransNox") = loDT(lnCtr).Item("sTransNox")
            p_oDTDetail(lnCtr).Item("sStockIDx") = loDT(lnCtr).Item("sStockIDx")
            p_oDTDetail(lnCtr).Item("nComplmnt") = loDT(lnCtr).Item("nComplmnt")
            p_oDTDetail(lnCtr).Item("nEntryNox") = loDT(lnCtr).Item("nEntryNox")
            p_oDTDetail(lnCtr).Item("cServedxx") = loDT(lnCtr).Item("cServedxx")
            p_oDTDetail(lnCtr).Item("cDetailxx") = loDT(lnCtr).Item("cDetailxx")
            p_oDTDetail(lnCtr).Item("sReplItem") = loDT(lnCtr).Item("sReplItem")
            p_oDTDetail(lnCtr).Item("cReversed") = loDT(lnCtr).Item("cReversed")
            p_oDTDetail(lnCtr).Item("cComboMlx") = loDT(lnCtr).Item("cComboMlx")
            p_oDTDetail(lnCtr).Item("cWthPromo") = loDT(lnCtr).Item("cWthPromo")
            p_oDTDetail(lnCtr).Item("sPrntPath") = IFNull(loDT(lnCtr).Item("sPrntPath"),)

            'p_oDTDetail(lnCtr).Item("nDiscAmtx") = loDT(lnCtr).Item("nDiscAmtx") 
            'p_oDTDetail(lnCtr).Item("nDealrDsc") = loDT(lnCtr).Item("nDealrDsc")
            If IFNull(p_oDTMaster(0).Item("sMergeIDx"), "") <> "" Then
                If lsTransNox <> p_oDTDetail(lnCtr).Item("sTransNox") Then
                    p_sMergeTb = p_sMergeTb & loDT(lnCtr).Item("sTableNox").ToString.PadLeft(2, "0") & ","
                End If
            End If
            lsTransNox = p_oDTDetail(lnCtr).Item("sTransNox")
        Next


        'Load discount and recompute total based on discount...
        p_oDtaDiscx = LoadDiscount(pxeSourceCde, p_oDTMaster(0).Item("sTransNox"))

        If Not IsNothing(p_oDtaDiscx) Then p_bHasDisct = True
        'Recompute total

        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        If p_oDTMaster(0).Item("nPWDDiscx") > 0 Then
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + (p_oDTMaster(0).Item("nDiscount") - p_oDTMaster(0).Item("nPWDDiscx")) + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2))) * (p_nSChargex / 100), 0)
        Else
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + (p_oDTMaster(0).Item("nDiscount") - p_oDTMaster(0).Item("nPWDDiscx")) + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2))) * (p_nSChargex / 100), 0)

            'p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + (p_oDTMaster(0).Item("nDiscount") - p_oDTMaster(0).Item("nPWDDiscx")) + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)) / 1.12) * (p_nSChargex / 100), 0)
        End If

        p_nTranTotl = p_oDTMaster(0).Item("nTranTotl")
        p_sWaiterID = p_oDTMaster(0).Item("sWaiterID")

        p_bExisting = True
        p_bWasSplitted = checkIfSplitted(p_oDTMaster(0)("sTransNox"))

        Return True
    End Function

    Private Function openOrder(ByVal fsTransNox As String) As Boolean
        Dim lsTransNox As String
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = AddCondition(getSQ_Order, "sTransNox = " & strParm(fsTransNox))

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            p_bExisting = False
            Return False
        End If

        p_oDTMaster(0).Item("sTransNox") = loDT(0).Item("sTransNox")
        p_oDTMaster(0).Item("dTransact") = loDT(0).Item("dTransact")
        p_oDTMaster(0).Item("nContrlNo") = loDT(0).Item("nContrlNo")
        p_oDTMaster(0).Item("nTranTotl") = loDT(0).Item("nTranTotl")
        p_oDTMaster(0).Item("sCashierx") = loDT(0).Item("sCashierx")
        p_oDTMaster(0).Item("sCashrNme") = getUserName(loDT(0).Item("sCashierx"))
        p_oDTMaster(0).Item("sWaiterID") = loDT(0).Item("sWaiterID")
        p_oDTMaster(0).Item("sWaiterNm") = getWaiter(p_oDTMaster(0).Item("sWaiterID"))
        p_oDTMaster(0).Item("sReceiptx") = loDT(0).Item("sReceiptx")
        p_oDTMaster(0).Item("sMergeIDx") = loDT(0).Item("sMergeIDx")
        p_oDTMaster(0).Item("sTableNox") = IIf(loDT(0).Item("sTableNox") = "", 0, loDT(0).Item("sTableNox"))
        p_oDTMaster(0).Item("sBillNmbr") = loDT(0).Item("sBillNmbr")
        p_oDTMaster(0).Item("nPrntBill") = loDT(0).Item("nPrntBill")
        p_oDTMaster(0).Item("dPrntBill") = loDT(0).Item("dPrntBill")

        p_oDTMaster(0).Item("nSChargex") = 0
        p_oDTMaster(0).Item("cSChargex") = loDT(0).Item("cSChargex")
        p_bSChargex = IIf(loDT(0).Item("cSChargex") = "1", True, False)
        p_nTableNo = IIf(p_oDTMaster(0).Item("sTableNox") = "", 0, p_oDTMaster(0).Item("sTableNox"))

        lsSQL = AddCondition(getSQ_Discount, "sSourceNo = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count > 0 Then
            p_nNoClient = loDT(0).Item("nNoClient")
            p_nWithDisc = loDT(0).Item("nWithDisc")
        End If

        If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
            lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        Else
            lsSQL = AddCondition(getSQ_Detail_WithMerge, "c.sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx")))
        End If

        loDT = p_oApp.ExecuteQuery(lsSQL)

        Dim lnCtr As Integer
        Call createDetail()

        p_sMergeTb = ""
        lsTransNox = ""
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
            p_oDTDetail(lnCtr).Item("cPrintedx") = loDT(lnCtr).Item("cPrintedx")
            p_oDTDetail(lnCtr).Item("sCategrID") = loDT(lnCtr).Item("sCategrID")

            p_oDTDetail(lnCtr).Item("sTransNox") = loDT(lnCtr).Item("sTransNox")
            p_oDTDetail(lnCtr).Item("sStockIDx") = loDT(lnCtr).Item("sStockIDx")
            p_oDTDetail(lnCtr).Item("nComplmnt") = loDT(lnCtr).Item("nComplmnt")
            p_oDTDetail(lnCtr).Item("nEntryNox") = loDT(lnCtr).Item("nEntryNox")
            p_oDTDetail(lnCtr).Item("cServedxx") = loDT(lnCtr).Item("cServedxx")
            p_oDTDetail(lnCtr).Item("cDetailxx") = loDT(lnCtr).Item("cDetailxx")
            p_oDTDetail(lnCtr).Item("sReplItem") = loDT(lnCtr).Item("sReplItem")
            p_oDTDetail(lnCtr).Item("cReversed") = loDT(lnCtr).Item("cReversed")
            p_oDTDetail(lnCtr).Item("cComboMlx") = loDT(lnCtr).Item("cComboMlx")
            p_oDTDetail(lnCtr).Item("cWthPromo") = loDT(lnCtr).Item("cWthPromo")
            p_oDTDetail(lnCtr).Item("sPrntPath") = IFNull(loDT(lnCtr).Item("sPrntPath"),)

            'p_oDTDetail(lnCtr).Item("nDiscAmtx") = loDT(lnCtr).Item("nDiscAmtx") 
            'p_oDTDetail(lnCtr).Item("nDealrDsc") = loDT(lnCtr).Item("nDealrDsc")
            If IFNull(p_oDTMaster(0).Item("sMergeIDx"), "") <> "" Then
                If lsTransNox <> p_oDTDetail(lnCtr).Item("sTransNox") Then
                    p_sMergeTb = p_sMergeTb & loDT(lnCtr).Item("sTableNox").ToString.PadLeft(2, "0") & ","
                End If
            End If
            lsTransNox = p_oDTDetail(lnCtr).Item("sTransNox")
        Next

        'Load discount and recompute total based on discount...
        p_oDtaDiscx = LoadDiscount(pxeSourceCde, p_oDTMaster(0).Item("sTransNox"))

        If Not IsNothing(p_oDtaDiscx) Then p_bHasDisct = True
        'Recompute total
        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        If p_oDTMaster(0).Item("nPWDDiscx") > 0 Then
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2))) * (p_nSChargex / 100), 0)
        Else
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)) / 1.12) * (p_nSChargex / 100), 0)
        End If

        p_nTranTotl = p_oDTMaster(0).Item("nTranTotl")
        p_sWaiterID = p_oDTMaster(0).Item("sWaiterID")

        p_bExisting = True
        p_bWasSplitted = checkIfSplitted(p_oDTMaster(0)("sTransNox"))

        Return True
    End Function

    Private Function getWaiter(ByVal sWaiterID As String) As String
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = "SELECT sClientID, sCompnyNm FROM Client_Master WHERE sClientID = " & strParm(sWaiterID)
        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            Return ""
        Else
            Return loDT(0)("sCompnyNm")
        End If
    End Function

    Public Function hasOpenTransaction() As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = "SELECT sTransNox" &
                   " FROM SO_Master" &
                   " WHERE sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                     " AND dTransact  < " & dateParm(Format(p_oApp.SysDate, xsDATE_SHORT)) &
                     " AND cTranStat = '0'" &
                " ORDER BY sTransNox"

        loDT = p_oApp.ExecuteQuery(lsSQL)

        With loDT
            If .Rows.Count = 0 Then
                hasOpenTransaction = False
            Else
                hasOpenTransaction = LoadOrder(loDT(0)("sTransNox"))
            End If
        End With
    End Function

    Public Function LoadChargeOrder(ByVal fsTransNo As String) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = AddCondition(getSQ_ChargedOrder, "sTransNox LIKE " & strParm("%" & fsTransNo & "%"))

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            p_bExisting = False
            Return False
        End If

        p_oDTMaster(0).Item("sTransNox") = loDT(0).Item("sTransNox")
        p_oDTMaster(0).Item("dTransact") = loDT(0).Item("dTransact")
        p_oDTMaster(0).Item("nContrlNo") = loDT(0).Item("nContrlNo")
        p_oDTMaster(0).Item("nTranTotl") = loDT(0).Item("nTranTotl")
        p_oDTMaster(0).Item("sCashierx") = loDT(0).Item("sCashierx")
        p_oDTMaster(0).Item("sCashrNme") = getUserName(loDT(0).Item("sCashierx"))
        p_oDTMaster(0).Item("sWaiterID") = loDT(0).Item("sWaiterID")
        p_oDTMaster(0).Item("sWaiterNm") = getWaiter(p_oDTMaster(0).Item("sWaiterID"))
        p_oDTMaster(0).Item("sReceiptx") = loDT(0).Item("sReceiptx")
        p_oDTMaster(0).Item("sMergeIDx") = loDT(0).Item("sMergeIDx")
        p_oDTMaster(0).Item("sTableNox") = loDT(0).Item("sTableNox")

        If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
            lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        Else
            lsSQL = AddCondition(getSQ_Detail_WithMerge, "c.sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx")))
        End If

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
            p_oDTDetail(lnCtr).Item("cPrintedx") = loDT(lnCtr).Item("cPrintedx")
            p_oDTDetail(lnCtr).Item("sCategrID") = loDT(lnCtr).Item("sCategrID")

            p_oDTDetail(lnCtr).Item("sTransNox") = loDT(lnCtr).Item("sTransNox")
            p_oDTDetail(lnCtr).Item("sStockIDx") = loDT(lnCtr).Item("sStockIDx")
            p_oDTDetail(lnCtr).Item("nComplmnt") = loDT(lnCtr).Item("nComplmnt")

            p_oDTDetail(lnCtr).Item("cServedxx") = loDT(lnCtr).Item("cServedxx")
            p_oDTDetail(lnCtr).Item("cDetailxx") = loDT(lnCtr).Item("cDetailxx")
            p_oDTDetail(lnCtr).Item("sReplItem") = loDT(lnCtr).Item("sReplItem")
            p_oDTDetail(lnCtr).Item("cReversed") = loDT(lnCtr).Item("cReversed")

            p_oDTDetail(lnCtr).Item("cComboMlx") = loDT(lnCtr).Item("cComboMlx")

            p_oDTDetail(lnCtr).Item("cWthPromo") = loDT(lnCtr).Item("cWthPromo")

            p_oDTDetail(lnCtr).Item("sPrntPath") = IFNull(loDT(lnCtr).Item("sPrntPath"),)
            p_oDTDetail(lnCtr).Item("dModified") = loDT(lnCtr).Item("dModified")
            'p_oDTDetail(lnCtr).Item("nDiscAmtx") = loDT(lnCtr).Item("nDiscAmtx")
            'p_oDTDetail(lnCtr).Item("nDealrDsc") = loDT(lnCtr).Item("nDealrDsc")
        Next

        'Load discount and recompute total based on discount...
        p_oDtaDiscx = LoadDiscount(pxeSourceCde, p_oDTMaster(0).Item("sTransNox"))
        'Recompute total
        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

        p_nTranTotl = p_oDTMaster(0).Item("nTranTotl")
        p_sWaiterID = p_oDTMaster(0).Item("sWaiterID")




        p_bExisting = True
        Return True
    End Function

    Public Function LoadOrder(ByVal fsTransNo As String, Optional ByVal fsOpenTrans As Boolean = False) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lsTransNox As String

        If fsOpenTrans Then
            lsSQL = AddCondition(getSQ_OpenOrder, "a.sTransNox = " & strParm(fsTransNo))
        Else
            lsSQL = AddCondition(getSQ_Order, "sTransNox LIKE " & strParm("%" & fsTransNo & "%"))
        End If
        Debug.Print(lsSQL)
        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_bExisting = False
            Return False
        End If

        p_oDTMaster(0).Item("sTransNox") = loDT(0).Item("sTransNox")
        p_oDTMaster(0).Item("dTransact") = loDT(0).Item("dTransact")
        p_oDTMaster(0).Item("nContrlNo") = loDT(0).Item("nContrlNo")
        p_oDTMaster(0).Item("nTranTotl") = loDT(0).Item("nTranTotl")
        p_oDTMaster(0).Item("sCashierx") = loDT(0).Item("sCashierx")
        p_oDTMaster(0).Item("sCashrNme") = getUserName(loDT(0).Item("sCashierx"))
        p_oDTMaster(0).Item("sWaiterID") = loDT(0).Item("sWaiterID")
        p_oDTMaster(0).Item("sWaiterNm") = getWaiter(p_oDTMaster(0).Item("sWaiterID"))
        p_oDTMaster(0).Item("sReceiptx") = loDT(0).Item("sReceiptx")
        p_oDTMaster(0).Item("sMergeIDx") = loDT(0).Item("sMergeIDx")
        p_oDTMaster(0).Item("sTableNox") = IIf(loDT(0).Item("sTableNox") = "", 0, loDT(0).Item("sTableNox"))
        p_oDTMaster(0).Item("sBillNmbr") = loDT(0).Item("sBillNmbr")
        p_oDTMaster(0).Item("nPrntBill") = loDT(0).Item("nPrntBill")
        p_oDTMaster(0).Item("dPrntBill") = loDT(0).Item("dPrntBill")
        p_oDTMaster(0).Item("cSChargex") = loDT(0).Item("cSChargex")
        p_bSChargex = IIf(IIf(loDT(0).Item("cSChargex") = "x", 0, loDT(0).Item("cSChargex")) = 1, True, False)
        p_nTableNo = IIf(p_oDTMaster(0).Item("sTableNox") = "", 0, p_oDTMaster(0).Item("sTableNox"))
        p_sTrantype = IFNull(loDT(0).Item("cTranType"), "")

        lsSQL = AddCondition(getSQ_Discount, "sSourceNo = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        loDT = p_oApp.ExecuteQuery(lsSQL)

        p_oDTMaster(0).Item("nDiscount") = 0.00
        p_oDTMaster(0).Item("nVatDiscx") = 0.00
        p_oDTMaster(0).Item("nPWDDiscx") = 0.00

        p_oDTMaster(0).Item("nVATSales") = 0.00
        p_oDTMaster(0).Item("nVATAmtxx") = 0.00
        p_oDTMaster(0).Item("nNonVATxx") = 0.00

        If loDT.Rows.Count > 0 Then
            p_nNoClient = loDT(0).Item("nNoClient")
            p_nWithDisc = loDT(0).Item("nWithDisc")
        End If

        If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
            lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        Else
            lsSQL = AddCondition(getSQ_Detail_WithMerge, "c.sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx")))
        End If

        loDT = p_oApp.ExecuteQuery(lsSQL)

        Dim lnCtr As Integer
        Call createDetail()

        p_sMergeTb = ""
        lsTransNox = ""
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
            p_oDTDetail(lnCtr).Item("cPrintedx") = loDT(lnCtr).Item("cPrintedx")
            p_oDTDetail(lnCtr).Item("sCategrID") = loDT(lnCtr).Item("sCategrID")

            p_oDTDetail(lnCtr).Item("sTransNox") = loDT(lnCtr).Item("sTransNox")
            p_oDTDetail(lnCtr).Item("sStockIDx") = loDT(lnCtr).Item("sStockIDx")
            p_oDTDetail(lnCtr).Item("nComplmnt") = loDT(lnCtr).Item("nComplmnt")

            p_oDTDetail(lnCtr).Item("cServedxx") = loDT(lnCtr).Item("cServedxx")
            p_oDTDetail(lnCtr).Item("cDetailxx") = loDT(lnCtr).Item("cDetailxx")
            p_oDTDetail(lnCtr).Item("sReplItem") = loDT(lnCtr).Item("sReplItem")
            p_oDTDetail(lnCtr).Item("cReversed") = loDT(lnCtr).Item("cReversed")

            p_oDTDetail(lnCtr).Item("cComboMlx") = loDT(lnCtr).Item("cComboMlx")
            p_oDTDetail(lnCtr).Item("sPrntPath") = IFNull(loDT(lnCtr).Item("sPrntPath"),)

            p_oDTDetail(lnCtr).Item("cWthPromo") = loDT(lnCtr).Item("cWthPromo")

            p_oDTDetail(lnCtr).Item("dModified") = loDT(lnCtr).Item("dModified")
            'p_oDTDetail(lnCtr).Item("nDiscAmtx") = loDT(lnCtr).Item("nDiscAmtx")
            'p_oDTDetail(lnCtr).Item("nDealrDsc") = loDT(lnCtr).Item("nDealrDsc")

            If IFNull(p_oDTMaster(0).Item("sMergeIDx"), "") <> "" Then
                If lsTransNox <> p_oDTDetail(lnCtr).Item("sTransNox") Then
                    p_sMergeTb = p_sMergeTb & loDT(lnCtr).Item("sTableNox").ToString.PadLeft(2, "0") & ","
                End If
            End If
            lsTransNox = p_oDTDetail(lnCtr).Item("sTransNox")
        Next

        'Load discount and recompute total based on discount...
        p_oDtaDiscx = LoadDiscount(pxeSourceCde, p_oDTMaster(0).Item("sTransNox"))
        If Not IsNothing(p_oDtaDiscx) Then p_bHasDisct = True

        'Recompute total
        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        If p_oDTMaster(0).Item("nPWDDiscx") > 0 Then
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2))) * (p_nSChargex / 100), 0)
        Else
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)) / 1.12) * (p_nSChargex / 100), 0)
        End If

        p_nTranTotl = p_oDTMaster(0).Item("nTranTotl")
        p_sWaiterID = p_oDTMaster(0).Item("sWaiterID")

        p_bExisting = True
        'added to check for splitted transaction for ui validation
        p_bWasSplitted = checkIfSplitted(p_oDTMaster(0)("sTransNox"))

        Return True
    End Function

    Public Function LoadTable(ByVal fnTableNo As Integer) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lsTransNox As String

        lsSQL = AddCondition(getSQ_Order, "sTableNox = " & strParm(fnTableNo))
        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then
            p_bExisting = False
            Return False
        ElseIf loDT.Rows.Count = 1 Then
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
            p_oDTMaster(0).Item("nPrntBill") = loDT(0).Item("nPrntBill")
            p_oDTMaster(0).Item("dPrntBill") = loDT(0).Item("dPrntBill")
            p_oDTMaster(0).Item("sBillNmbr") = loDT(0).Item("sBillNmbr")
            p_oDTMaster(0).Item("cSChargex") = loDT(0).Item("cSChargex")
            p_bSChargex = IIf(loDT(0).Item("cSChargex") = 1, True, False)
            p_oDTMaster(0).Item("cTranType") = loDT(0).Item("cTranType")
            p_nTableNo = loDT(0).Item("sTableNox")
            p_sTrantype = IFNull(loDT(0).Item("cTranType"), "0")
        Else
            'more than 1 order in a table
            Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , lsSQL _
                                        , False _
                                        , "" _
                                        , "sTransNox»nContrlNo»dTransact»nTranTotl" _
                                        , "TransNox»Control»Date»Bill",
                                        , "sTransNox»nContrlNo»dTransact»nTranTotl" _
                                        , 1)

            If IsNothing(loDta) Then
                Return False
            Else
                p_oDTMaster(0).Item("sTransNox") = loDta("sTransNox")
                p_oDTMaster(0).Item("dTransact") = loDta("dTransact")
                p_oDTMaster(0).Item("nContrlNo") = loDta("nContrlNo")
                p_oDTMaster(0).Item("nTranTotl") = loDta("nTranTotl")
                p_oDTMaster(0).Item("sCashrNme") = ""
                p_oDTMaster(0).Item("sWaiterNm") = ""
                p_oDTMaster(0).Item("sCashierx") = loDta("sCashierx")
                p_oDTMaster(0).Item("sWaiterID") = loDta("sWaiterID")
                p_oDTMaster(0).Item("sReceiptx") = loDta("sReceiptx")
                p_oDTMaster(0).Item("sMergeIDx") = loDta("sMergeIDx")
                p_oDTMaster(0).Item("sTableNox") = loDta("sTableNox")
                p_oDTMaster(0).Item("nPrntBill") = loDta("nPrntBill")
                p_oDTMaster(0).Item("dPrntBill") = loDta("dPrntBill")
                p_oDTMaster(0).Item("sBillNmbr") = loDta("sBillNmbr")
                p_nTableNo = loDT(0).Item("sTableNox")
                p_sTrantype = IFNull(loDT(0).Item("cTranType"), "")
            End If
        End If

        'Added this function to check for discount information

        lsSQL = AddCondition(getSQ_Discount, "sSourceNo = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        loDT = p_oApp.ExecuteQuery(lsSQL)

        p_oDTMaster(0).Item("nDiscount") = 0.00
        p_oDTMaster(0).Item("nVatDiscx") = 0.00
        p_oDTMaster(0).Item("nPWDDiscx") = 0.00

        p_oDTMaster(0).Item("nVATSales") = 0.00
        p_oDTMaster(0).Item("nVATAmtxx") = 0.00
        p_oDTMaster(0).Item("nNonVATxx") = 0.00

        If loDT.Rows.Count > 0 Then
            p_nNoClient = loDT(0).Item("nNoClient")
            p_nWithDisc = loDT(0).Item("nWithDisc")
        End If
        '******************************************
        If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
            lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")))
        Else
            lsSQL = AddCondition(getSQ_Detail_WithMerge, "c.sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx")))
        End If

        loDT = p_oApp.ExecuteQuery(lsSQL)

        Dim lnCtr As Integer
        Call createDetail()

        lsTransNox = ""
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
            p_oDTDetail(lnCtr).Item("cPrintedx") = loDT(lnCtr).Item("cPrintedx")
            p_oDTDetail(lnCtr).Item("sCategrID") = loDT(lnCtr).Item("sCategrID")

            p_oDTDetail(lnCtr).Item("sTransNox") = loDT(lnCtr).Item("sTransNox")
            p_oDTDetail(lnCtr).Item("sStockIDx") = loDT(lnCtr).Item("sStockIDx")
            p_oDTDetail(lnCtr).Item("nComplmnt") = loDT(lnCtr).Item("nComplmnt")

            p_oDTDetail(lnCtr).Item("cServedxx") = loDT(lnCtr).Item("cServedxx")
            p_oDTDetail(lnCtr).Item("cDetailxx") = loDT(lnCtr).Item("cDetailxx")
            p_oDTDetail(lnCtr).Item("sReplItem") = loDT(lnCtr).Item("sReplItem")
            p_oDTDetail(lnCtr).Item("cReversed") = loDT(lnCtr).Item("cReversed")

            p_oDTDetail(lnCtr).Item("cComboMlx") = loDT(lnCtr).Item("cComboMlx")

            p_oDTDetail(lnCtr).Item("sPrntPath") = IFNull(loDT(lnCtr).Item("sPrntPath"),)
            p_oDTDetail(lnCtr).Item("cWthPromo") = loDT(lnCtr).Item("cWthPromo")

            p_oDTDetail(lnCtr).Item("dModified") = loDT(lnCtr).Item("dModified")
            'p_oDTDetail(lnCtr).Item("nDiscAmtx") = loDT(lnCtr).Item("nDiscAmtx")
            'p_oDTDetail(lnCtr).Item("nDealrDsc") = loDT(lnCtr).Item("nDealrDsc")
            If IFNull(p_oDTMaster(0).Item("sMergeIDx"), "") <> "" Then
                If lsTransNox <> p_oDTDetail(lnCtr).Item("sTransNox") Then
                    p_sMergeTb = p_sMergeTb & loDT(lnCtr).Item("sTableNox").ToString.PadLeft(2, "0") & ","
                End If
            End If
            lsTransNox = p_oDTDetail(lnCtr).Item("sTransNox")
        Next

        'Load discount and recompute total based on discount...
        p_oDtaDiscx = LoadDiscount(pxeSourceCde, p_oDTMaster(0).Item("sTransNox"))
        'Recompute total
        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        If p_oDTMaster(0).Item("nPWDDiscx") > 0 Then
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2))) * (p_nSChargex / 100), 0)
        Else
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)) / 1.12) * (p_nSChargex / 100), 0)
        End If

        p_nTranTotl = p_oDTMaster(0).Item("nTranTotl")
        p_sWaiterID = p_oDTMaster(0).Item("sWaiterID")

        p_bExisting = True
        Return True
    End Function

    'Public Function LoadTable(ByVal fnTableNo As Integer) As Boolean
    '    Dim lsSQL As String
    '    Dim loDT As DataTable

    '    lsSQL = AddCondition(getSQ_Order, "sTableNox LIKE " & strParm("%" & fnTableNo & "%"))

    '    loDT = p_oApp.ExecuteQuery(lsSQL)
    '    If loDT.Rows.Count = 0 Then
    '        p_bExisting = False
    '        Return False
    '    End If

    '    p_oDTMaster(0).Item("sTransNox") = loDT(0).Item("sTransNox")
    '    p_oDTMaster(0).Item("dTransact") = loDT(0).Item("dTransact")
    '    p_oDTMaster(0).Item("nContrlNo") = loDT(0).Item("nContrlNo")
    '    p_oDTMaster(0).Item("nTranTotl") = loDT(0).Item("nTranTotl")
    '    p_oDTMaster(0).Item("sCashrNme") = ""
    '    p_oDTMaster(0).Item("sWaiterNm") = ""
    '    p_oDTMaster(0).Item("sCashierx") = loDT(0).Item("sCashierx")
    '    p_oDTMaster(0).Item("sWaiterID") = loDT(0).Item("sWaiterID")
    '    p_oDTMaster(0).Item("sReceiptx") = loDT(0).Item("sReceiptx")
    '    p_oDTMaster(0).Item("sMergeIDx") = loDT(0).Item("sMergeIDx")
    '    p_oDTMaster(0).Item("sTableNox") = loDT(0).Item("sTableNox")

    '    If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
    '        lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")))
    '    Else
    '        lsSQL = AddCondition(getSQ_Detail_WithMerge, "c.sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx")))
    '    End If

    '    loDT = p_oApp.ExecuteQuery(lsSQL)

    '    Dim lnCtr As Integer
    '    Call createDetail()

    '    For lnCtr = 0 To loDT.Rows.Count - 1
    '        newDetail()

    '        p_oDTDetail(lnCtr).Item("sBarcodex") = loDT(lnCtr).Item("sBarcodex")
    '        p_oDTDetail(lnCtr).Item("sDescript") = loDT(lnCtr).Item("sDescript")
    '        p_oDTDetail(lnCtr).Item("sBriefDsc") = loDT(lnCtr).Item("sBriefDsc")
    '        p_oDTDetail(lnCtr).Item("nUnitPrce") = loDT(lnCtr).Item("nUnitPrce")
    '        p_oDTDetail(lnCtr).Item("cReversex") = loDT(lnCtr).Item("cReversex")
    '        p_oDTDetail(lnCtr).Item("nQuantity") = loDT(lnCtr).Item("nQuantity")
    '        p_oDTDetail(lnCtr).Item("nDiscount") = loDT(lnCtr).Item("nDiscount")
    '        p_oDTDetail(lnCtr).Item("nAddDiscx") = loDT(lnCtr).Item("nAddDiscx")
    '        p_oDTDetail(lnCtr).Item("nDiscLev1") = loDT(lnCtr).Item("nDiscLev1")
    '        p_oDTDetail(lnCtr).Item("nDiscLev2") = loDT(lnCtr).Item("nDiscLev2")
    '        p_oDTDetail(lnCtr).Item("nDiscLev3") = loDT(lnCtr).Item("nDiscLev3")
    '        p_oDTDetail(lnCtr).Item("cDetSaved") = 1
    '        p_oDTDetail(lnCtr).Item("cPrintedx") = loDT(lnCtr).Item("cPrintedx")
    '        p_oDTDetail(lnCtr).Item("sCategrID") = loDT(lnCtr).Item("sCategrID")

    '        p_oDTDetail(lnCtr).Item("sTransNox") = loDT(lnCtr).Item("sTransNox")
    '        p_oDTDetail(lnCtr).Item("sStockIDx") = loDT(lnCtr).Item("sStockIDx")
    '        p_oDTDetail(lnCtr).Item("nComplmnt") = loDT(lnCtr).Item("nComplmnt")

    '        p_oDTDetail(lnCtr).Item("cServedxx") = loDT(lnCtr).Item("cServedxx")
    '        p_oDTDetail(lnCtr).Item("cDetailxx") = loDT(lnCtr).Item("cDetailxx")
    '        p_oDTDetail(lnCtr).Item("sReplItem") = loDT(lnCtr).Item("sReplItem")
    '        p_oDTDetail(lnCtr).Item("cReversed") = loDT(lnCtr).Item("cReversed")

    '        p_oDTDetail(lnCtr).Item("cComboMlx") = loDT(lnCtr).Item("cComboMlx")

    '        p_oDTDetail(lnCtr).Item("cWthPromo") = loDT(lnCtr).Item("cWthPromo")

    '        p_oDTDetail(lnCtr).Item("dModified") = loDT(lnCtr).Item("dModified")
    '        'p_oDTDetail(lnCtr).Item("nDiscAmtx") = loDT(lnCtr).Item("nDiscAmtx")
    '        'p_oDTDetail(lnCtr).Item("nDealrDsc") = loDT(lnCtr).Item("nDealrDsc")
    '    Next

    '    'Load discount and recompute total based on discount...
    '    p_oDtaDiscx = LoadDiscount(pxeSourceCde, p_oDTMaster(0).Item("sTransNox"))
    '    'Recompute total
    '    Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

    '    p_nTranTotl = p_oDTMaster(0).Item("nTranTotl")
    '    p_sWaiterID = p_oDTMaster(0).Item("sWaiterID")

    '    p_bExisting = True
    '    Return True
    'End Function

    Private Function loadSplit(ByVal fsSourceNo As String,
                               ByVal fcSplitTyp As Integer) As DataTable
        Dim lsSQL As String
        Dim loDT As New DataTable

        If fcSplitTyp = 2 Then
            lsSQL = "SELECT b.sBarcodex" &
                        ", b.sDescript" &
                        ", b.sBriefDsc" &
                        ", a.nUnitPrce" &
                        ", a.cReversex" &
                        ", d.nQuantity" &
                        ", a.nDiscount" &
                        ", a.nAddDiscx" &
                        ", b.nDiscLev1" &
                        ", b.nDiscLev2" &
                        ", b.nDiscLev3" &
                        ", b.nDealrDsc" &
                        ", a.sStockIDx" &
                        ", b.sCategrID" &
                        ", a.cPrintedx" &
                        ", a.sTransNox" &
                        ", a.nComplmnt" &
                        ", a.nEntryNox" &
                        ", a.cServedxx" &
                        ", a.cDetailxx" &
                        ", a.sReplItem" &
                        ", a.cReversed" &
                        ", a.cComboMlx" &
                        ", a.cWthPromo" &
                        ", a.dModified" &
                        ", c.nAmountxx" &
                    " FROM SO_Detail a" &
                        ", Inventory b" &
                        ", Order_Split c" &
                        ", Order_Split_Detail d" &
                    " WHERE a.sStockIDx = b.sStockIDx" &
                        " AND a.sTransNox = c.sReferNox" &
                        " AND c.sTransNox = d.sTransNox" &
                        " AND a.sStockIDx = d.sStockIDx" &
                        " AND d.sTransNox = " & strParm(fsSourceNo) &
                    " ORDER BY a.nEntryNox"
        Else
            lsSQL = "SELECT b.sBarcodex" &
                        ", b.sDescript" &
                        ", b.sBriefDsc" &
                        ", a.nUnitPrce" &
                        ", a.cReversex" &
                        ", a.nQuantity" &
                        ", a.nDiscount" &
                        ", a.nAddDiscx" &
                        ", b.nDiscLev1" &
                        ", b.nDiscLev2" &
                        ", b.nDiscLev3" &
                        ", b.nDealrDsc" &
                        ", a.sStockIDx" &
                        ", b.sCategrID" &
                        ", a.cPrintedx" &
                        ", a.sTransNox" &
                        ", a.nComplmnt" &
                        ", a.nEntryNox" &
                        ", a.cServedxx" &
                        ", a.cDetailxx" &
                        ", a.sReplItem" &
                        ", a.cReversed" &
                        ", a.cComboMlx" &
                        ", a.cWthPromo" &
                        ", a.dModified" &
                        ", c.nAmountxx" &
                        ", d.nTranTotl" &
                    " FROM SO_Detail a" &
                        ", Inventory b" &
                        ", Order_Split c" &
                        ", SO_Master d" &
                    " WHERE a.sStockIDx = b.sStockIDx" &
                        " AND a.sTransNox = c.sReferNox" &
                        " AND a.sTransNox = d.sTransNox" &
                        " AND c.sTransNox = " & strParm(fsSourceNo) &
                    " ORDER BY a.nEntryNox"
            'lsSQL = "SELECT" & _
            '           "  nUnitPrce" & _
            '           ", 'MEAL(S)' sDescript" & _
            '           ", 'MEAL(S)' sBriefDsc" & _
            '           ", '0' cReversed" & _
            '           ", '0' cComboMlx" & _
            '           ", 1 nQuantity" & _
            '           ", 0.00 nDiscount" & _
            '           ", 0.00 nAddDiscx" & _
            '           ", '0' cDetailxx" & _
            '           ", 0 nComplmnt" & _
            '           ", 0 cWthPromo" & _
            '       " FROM Order_Split_Detail" & _
            '       " WHERE sTransNox = " & strParm(fsSourceNo)
        End If

        loDT = p_oApp.ExecuteQuery(lsSQL)

        Return loDT
    End Function

    'iMac 2017-01-06
    Public Function SearchItem() As DataTable
        Dim loDT As DataTable

        loDT = p_oApp.ExecuteQuery(getSQ_Search)

        Return loDT
    End Function

    Public Function SearchItem(
                              ByVal fsValue As String _
                                , Optional ByVal fsCategr As String = "" _
                                , Optional ByVal fbByCode As Boolean = False) As Boolean

        Dim lsFilter As String
        Dim lsSQL As String

        If fbByCode Then
            lsFilter = "a.sBarcodex = " & strParm(fsValue)
        Else
            lsFilter = "a.sDescript LIKE " & strParm("%" & fsValue & "%")
        End If
        lsSQL = AddCondition(getSQ_Search, lsFilter)

        Dim loDT As DataTable
        loDT = p_oApp.ExecuteQuery(AddCondition(lsSQL, IIf(fsCategr = "", "0=0", "a.sCategrID = " & strParm(fsCategr))))

        If loDT.Rows.Count = 1 Then
            Return AddOrder(loDT(0)("sBarcodex"), p_nQuantity)
        Else
            Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , getSQ_Search _
                                        , False _
                                        , lsFilter _
                                        , "sBarcodex»sDescript»nSelPrice" _
                                        , "Bar Code»Description»Unit Price",
                                        , "a.sBarcodex»a.sDescript»a.nSelPrice" _
                                        , IIf(fbByCode, 0, 1))
            If IsNothing(loDta) Then
                Return False
            Else
                Return AddOrder(loDta("sBarcodex"), p_nQuantity)
            End If
        End If
    End Function

    Public Function ItemLookUp(
                              ByVal fsValue As String _
                                , Optional ByVal fsCategr As String = "" _
                                , Optional ByVal fbByCode As Boolean = False) As Boolean

        Dim lsFilter As String
        Dim lsSQL As String

        If fbByCode Then
            lsFilter = "sBarcodex = " & strParm(fsValue)
        Else
            lsFilter = "sDescript LIKE " & strParm("%" & fsValue & "%")
        End If
        lsSQL = AddCondition(getSQ_Search, lsFilter)

        Dim loDT As DataTable
        loDT = p_oApp.ExecuteQuery(AddCondition(lsSQL, IIf(fsCategr = "", "0=0", "sCategrID = " & strParm(fsCategr))))

        If loDT.Rows.Count = 1 Then
            Return AddOrder(loDT(0)("sBarcodex"), 1)
        Else
            Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , getSQ_Search _
                                        , False _
                                        , lsFilter _
                                        , "sDescript»nSelPrice" _
                                        , "Description»Unit Price",
                                        , "sDescript»nSelPrice" _
                                        , 0)
            If IsNothing(loDta) Then
                Return False
            Else
                Return AddOrder(loDta("sBarcodex"), 1)
            End If
        End If
    End Function

    Public Function AddOrder(ByVal fsBarrCode As String, Optional ByVal fnQuantity As Integer = 1) As Boolean
        Dim lsSQL As String
        Dim loDT As New DataTable
        Dim lnRow As Integer
        Dim lnQty As Integer

        If p_bValidDailySales = False Then
            Return False
        End If

        ' first retrieve the detail
        lsSQL = AddCondition(getSQ_Search, "sBarcodex = " & strParm(fsBarrCode))

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            Return False
        End If

        ' Now add the retrieve info to the detail
        Call newDetail(loDT, lnQty)
        p_oDTDetail(p_nRow).Item("nQuantity") = fnQuantity + lnQty
        ' store this row coz it might be incremented on the following procedure

        lnRow = p_nRow
        If loDT(0).Item("cComboMlx") = xeLogical.YES Then
            Call procCombo()
        End If

        'check if inventory has promo
        Dim lbWAddOn As Boolean = False
        Dim lbWDiscn As Boolean = False
        Dim lbWCombo As Boolean = False

        If loDT(0).Item("cWthPromo") = xeLogical.YES Then
            lbWAddOn = procAddOn(lnRow)
            lbWDiscn = procDisc(lnRow)

            If Not lbWAddOn And Not lbWDiscn Then clearDetail(lnRow)
        End If

        If IIf(Master("sTableNox") = "", 0, Master("sTableNox")) <> "0" Then
            If p_oDTDetail(p_nRow).Item("cPrintedx") = xeLogical.YES Then
                If Not p_oApp.getUserApproval() Then Return False
                'MsgBox("Update on QTY detected!" & vbCrLf &
                '    "You must need to save order to save transaction", vbInformation)
            End If
            If Not saveDetail(p_nRow) Then Return False
        End If

        'For lnRow = lnRow To p_oDTDetail.Rows.Count - 1
        'Call saveDetail(lnRow)
        'Next
        'RaiseEvent DisplayFromRow(lnRow)

        ' update tran totals
        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

        For lnRow = lnRow To p_oDTDetail.Rows.Count - 1
            Call saveDetail(lnRow)
        Next
        RaiseEvent DisplayFromRow(lnRow)

        'ask sir marlon
        'If Not p_bExisting Then
        '    Call saveMaster()

        '    p_bExisting = True
        'End If

        Return True
    End Function

    Public Function ReverseOrder(ByVal fnRow As Integer, Optional ByVal fnQuantity As Integer = 1) As Boolean
        If p_oDTDetail(fnRow).Item("cReversed") = "1" Or p_oDTDetail(fnRow).Item("cDetailxx") = "1" Then Return False

        Call newDetail()

        p_oDTDetail(p_nRow).Item("sBarcodex") = p_oDTDetail(fnRow).Item("sBarcodex")
        p_oDTDetail(p_nRow).Item("sDescript") = p_oDTDetail(fnRow).Item("sDescript")
        p_oDTDetail(p_nRow).Item("sBriefDsc") = p_oDTDetail(fnRow).Item("sBriefDsc")
        p_oDTDetail(p_nRow).Item("nUnitPrce") = p_oDTDetail(fnRow).Item("nUnitPrce")

        p_oDTDetail(p_nRow).Item("nDiscount") = p_oDTDetail(fnRow).Item("nDiscount")
        p_oDTDetail(p_nRow).Item("nAddDiscx") = p_oDTDetail(fnRow).Item("nAddDiscx")

        p_oDTDetail(p_nRow).Item("sStockIDx") = p_oDTDetail(fnRow).Item("sStockIDx")
        p_oDTDetail(p_nRow).Item("nDiscLev1") = p_oDTDetail(fnRow).Item("nDiscLev1")
        p_oDTDetail(p_nRow).Item("nDiscLev2") = p_oDTDetail(fnRow).Item("nDiscLev2")
        p_oDTDetail(p_nRow).Item("nDiscLev3") = p_oDTDetail(fnRow).Item("nDiscLev3")
        p_oDTDetail(p_nRow).Item("nDiscAmtx") = p_oDTDetail(fnRow).Item("nDiscAmtx")
        p_oDTDetail(p_nRow).Item("nDealrDsc") = p_oDTDetail(fnRow).Item("nDealrDsc")
        p_oDTDetail(p_nRow).Item("cComboMlx") = p_oDTDetail(fnRow).Item("cComboMlx")
        p_oDTDetail(p_nRow).Item("cWthPromo") = p_oDTDetail(fnRow).Item("cWthPromo")
        p_oDTDetail(p_nRow).Item("sCategrID") = p_oDTDetail(fnRow).Item("sCategrID")
        'Debug.Print(p_oDTDetail(fnRow).Item("sPrntPath"))
        p_oDTDetail(p_nRow).Item("sPrntPath") = p_oDTDetail(fnRow).Item("sPrntPath")
        p_oDTDetail(p_nRow).Item("nQuantity") = fnQuantity
        p_oDTDetail(p_nRow).Item("cReversex") = "-"
        p_oDTDetail(p_nRow).Item("cReversed") = "1"
        p_oDTDetail(p_nRow).Item("dModified") = Now()

        'save detail
        Call saveDetail(p_nRow)

        'RaiseEvent DisplayRow(p_nRow + 1)
        RaiseEvent DisplayRow(p_nRow)


        Dim lnDetlRow As Integer
        Dim lnOldQTY As Integer = p_oDTDetail(fnRow).Item("nQuantity")    'Get the old value

        For lnDetlRow = fnRow + 1 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnDetlRow).Item("cDetailxx") = "1" Then 'iMac 2017-01-11
                Call newDetail()

                Dim lnQuantity As Integer = Math.DivRem(p_oDTDetail(lnDetlRow).Item("nQuantity"), lnOldQTY, 0)

                p_oDTDetail(p_nRow).Item("sStockIDx") = p_oDTDetail(lnDetlRow).Item("sStockIDx")
                p_oDTDetail(p_nRow).Item("sBarcodex") = p_oDTDetail(lnDetlRow).Item("sBarcodex")
                p_oDTDetail(p_nRow).Item("sDescript") = p_oDTDetail(lnDetlRow).Item("sDescript")
                p_oDTDetail(p_nRow).Item("sBriefDsc") = p_oDTDetail(lnDetlRow).Item("sBriefDsc")
                p_oDTDetail(p_nRow).Item("nUnitPrce") = p_oDTDetail(lnDetlRow).Item("nUnitPrce")
                p_oDTDetail(p_nRow).Item("nDiscount") = p_oDTDetail(lnDetlRow).Item("nDiscount")
                p_oDTDetail(p_nRow).Item("nAddDiscx") = p_oDTDetail(lnDetlRow).Item("nAddDiscx")

                p_oDTDetail(p_nRow).Item("nQuantity") = p_oDTDetail(lnDetlRow).Item("nQuantity")

                p_oDTDetail(p_nRow).Item("dModified") = Now
                p_oDTDetail(p_nRow).Item("cReversex") = "-"
                p_oDTDetail(p_nRow).Item("cDetailxx") = 1
                p_oDTDetail(p_nRow).Item("cReversed") = "1"

                Call saveDetail(p_nRow)

                'RaiseEvent DisplayRow(p_nRow + 1)
                RaiseEvent DisplayRow(p_nRow)
            Else
                Exit For
            End If
        Next

        ''reverse items
        'Call reverseCombo(p_oDTDetail(p_nRow).Item("sStockIDx"), p_oDTDetail(p_nRow).Item("nQuantity"))

        'set tag detail as reversed
        p_oDTDetail(fnRow).Item("cReversed") = "1"
        Dim lsSQL As String
        lsSQL = "UPDATE " & pxeDetTable &
               " SET cReversed = '1'" &
               " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                 " AND nEntryNox = " & fnRow + 1
        p_oApp.Execute(lsSQL, pxeDetTable)

        ' update tran totals
        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        Return True
    End Function

    'Private Function reverseCombo(ByVal fsStockIDx As String, Optional ByVal fnQuantity As Integer = 1) As Boolean
    '    Dim lsSQL As String
    '    Dim loDT As DataTable
    '    Dim lnRow As Integer

    '    lsSQL = getSQ_Combo(fsStockIDx)
    '    loDT = p_oApp.ExecuteQuery(lsSQL)

    '    If loDT.Rows.Count = 0 Then
    '        Return True
    '    End If

    '    ' add the detail in the order
    '    For lnRow = 0 To loDT.Rows.Count - 1
    '        Call newDetail()

    '        ' assign item details
    '        p_oDTDetail(p_nRow).Item("sStockIDx") = loDT(lnRow).Item("sStockIDx")
    '        p_oDTDetail(p_nRow).Item("sBarcodex") = loDT(lnRow).Item("sBarcodex")
    '        p_oDTDetail(p_nRow).Item("sDescript") = loDT(lnRow).Item("sDescript")
    '        p_oDTDetail(p_nRow).Item("sBriefDsc") = loDT(lnRow).Item("sBriefDsc")
    '        p_oDTDetail(p_nRow).Item("nQuantity") = fnQuantity
    '        p_oDTDetail(p_nRow).Item("cReversex") = "-"
    '        p_oDTDetail(p_nRow).Item("dModified") = Now
    '        p_oDTDetail(p_nRow).Item("cDetailxx") = 1

    '        Call saveDetail(p_nRow)
    '    Next

    '    Return True
    'End Function

    Public Function SaveTransaction() As Boolean
        Dim lsSQL As String = ""
        Dim lsTableNo As String = ""

        'iMac 2017.01.26
        'dont save if detail is empty
        If ItemCount = 0 Then Return False

        Dim loForm As frmSelectTable

        loForm = New frmSelectTable
        With loForm
            .AppDriver = p_oApp
            .Waiter = p_oDTMaster(0).Item("sWaiterID")
            .TableNo = p_oDTMaster(0).Item("sTableNox")
            .TransNo = p_oDTMaster(0).Item("sTransNox")
            .TranType = If(Convert.IsDBNull(p_oDTMaster(0).Item("cTranType")), "0", p_oDTMaster(0).Item("cTranType").ToString())
            If (p_oApp.BranchCode = "P013") Then
                .isWSCharge = IIf(p_oDTMaster(0).Item("cSChargex") = "x", False, p_oDTMaster(0).Item("cSChargex"))
            Else

                .isWSCharge = IIf(p_oDTMaster(0).Item("cSChargex") = "x", True, p_oDTMaster(0).Item("cSChargex"))
            End If
            .Occupants = IFNull(p_oDTMaster(0).Item("nOccupnts"), 0)
            .ShowDialog()
            If .Cancel Then Return False

            lsTableNo = .TableNo
            p_oDTMaster(0).Item("sWaiterID") = IFNull(.Waiter, "")
            p_oDTMaster(0).Item("sWaiterNm") = getWaiter(p_oDTMaster(0).Item("sWaiterID"))
            p_oDTMaster(0).Item("nOccupnts") = .Occupants
            p_oDTMaster(0).Item("cSChargex") = IIf(.isWSCharge, 1, 0)
            p_oDTMaster(0).Item("cTranType") = .TranType
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

            lsSQL = lsSQL & ", nOccupnts = " & p_oDTMaster(0).Item("nOccupnts")
            lsSQL = lsSQL & ", cSChargex = " & p_oDTMaster(0).Item("cSChargex")
            lsSQL = lsSQL & ", cTranType = " & p_oDTMaster(0).Item("cTranType")

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

                p_oDTMaster(0).Item("sTableNox") = Replace(lsTableNo, ",", pxeDelimtr)
                'update table status to occupied
                Call updateTable(lsTableNo)
            End If

            If lsSQL <> "" Then
                lsSQL = "UPDATE " & pxeMasTable &
                        " SET " & Mid(lsSQL, 3) &
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox"))

                p_oApp.Execute(lsSQL, pxeMasTable)
            End If
        Else
            p_oDTMaster(0).Item("sTableNox") = Replace(lsTableNo, ",", pxeDelimtr)
            p_oDTMaster(0).Item("nTranTotl") = p_nTranTotl

            p_oDTMaster(0).Item("sTransNox") = getSOTransNo()
            Call saveMaster()
        End If

        'after saving the master, loop through the detail and check for pending detail
        Dim lnRow As Integer

        For lnctr = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(lnRow).Item("cDetSaved") = xeLogical.NO Then
                Call saveDetail(lnRow)

                p_oDTDetail(lnRow).Item("cDetSaved") = 1
            End If
        Next

        p_oApp.CommitTransaction()

        p_oApp.SaveEvent("0028", "Order No. " & p_oDTMaster(0).Item("nContrlNo") &
                                "/Table No. " & IFNull(p_oDTMaster(0).Item("sTableNox"), ""), p_sSerial)


        Call printOrder()
        Call createMaster()
        Call initMaster()

        Return True
    End Function

    Public Function BrowseOrder() As Boolean
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Order, "(sTableNox IS NULL OR sTableNox = '')")
        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 1 Then
            Return LoadOrder(loDT.Rows(0)("sTransNox"))
        Else
            Dim loDta As DataRow = KwikSearch(p_oApp _
                                        , lsSQL _
                                        , False _
                                        , "" _
                                        , "sTransNox»nContrlNo" _
                                        , "TransNox»Control",
                                        , "sTransNox»nContrlNo" _
                                        , 1)
            If IsNothing(loDta) Then
                Return False
            Else
                Return LoadOrder(loDta("sTransNox"))
            End If
        End If
    End Function

    Public Function BrowseOpenOrder() As Boolean
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_neoOpenOrder, "a.cTranStat = '0'")

        loDT = p_oApp.ExecuteQuery(lsSQL)
        Debug.Print(lsSQL)

        If loDT.Rows.Count = 1 Then
            Return LoadOrder(loDT.Rows(0)("sTransNox"))
        Else
            Dim loDta As DataRow = KwikSearch(p_oApp _
                                         , lsSQL _
                                         , True _
                                         , "" _
                                         , "sTableNox»sCustName»sTransNox»nTranTotl" _
                                         , "TableNo»Customer»Trans No»Amt" _
                                         , "" _
                                         , "sTableNox»sCustNames»sTransNox»nTranTotl" _
                                         , 0)

            If IsNothing(loDta) Then
                Return False
            Else
                Return LoadOrder(loDta("sTransNox"))
            End If
        End If
    End Function

    Private Sub printOrder()
        Dim loPrint As PRN_Order
        Dim lnCtr As Integer
        Dim lnRow As Integer
        Dim lsSQL As String
        Dim lbPrint As Boolean

        loPrint = New PRN_Order(p_oApp)
        With loPrint
            .Transaction_Date = p_oApp.getSysDate
            .TableNo = p_oDTMaster(0)("sTableNox")
            .Waiter = p_oDTMaster(0)("sWaiterNm")
            .Dservice = p_oDTMaster(0).Item("cTranType")
            .Cashier = getCashier(p_oApp.UserID)
            .Terminal = p_sTermnl
            .LogName = p_sLogName

            lnRow = 0
            lbPrint = False
            For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
                If p_oDTDetail(lnCtr).Item("cPrintedx") = "0" Then
                    If p_oDTDetail(lnCtr).Item("cReversed") = xeLogical.NO Then
                        If p_oDTDetail(lnCtr).Item("cDetailxx") = 1 Or p_oDTDetail(lnCtr).Item("cWthPromo") = 1 Then
                            .AddDetail(p_oDTDetail(lnCtr).Item("nQuantity"),
                               p_oDTDetail(lnCtr).Item("sDescript"),
                               (p_oDTDetail(lnCtr).Item("nUnitPrce") - (p_oDTDetail(lnCtr).Item("nUnitPrce") * (p_oDTDetail(lnCtr).Item("nDiscount") / 100))) - p_oDTDetail(lnCtr).Item("nAddDiscx"), True, IFNull(p_oDTDetail(lnCtr).Item("sPrntPath"), ""), p_oDTDetail(lnCtr).Item("sDescript"))
                        Else
                            .AddDetail(p_oDTDetail(lnCtr).Item("nQuantity"),
                               p_oDTDetail(lnCtr).Item("sDescript"),
                               p_oDTDetail(lnCtr).Item("nUnitPrce"), True, IFNull(p_oDTDetail(lnCtr).Item("sPrntPath"), ""), p_oDTDetail(lnCtr).Item("sDescript"))
                        End If
                    Else
                        If p_oDTDetail(lnCtr).Item("cReversex") = "+" Then
                            .AddDetail(p_oDTDetail(lnCtr).Item("nQuantity"),
                               p_oDTDetail(lnCtr).Item("sDescript"),
                               p_oDTDetail(lnCtr).Item("nUnitPrce"), False, IFNull(p_oDTDetail(lnCtr).Item("sPrntPath"), ""), p_oDTDetail(lnCtr).Item("sDescript"))
                        ElseIf p_oDTDetail(lnCtr).Item("cReversex") = "-" Then
                            .AddDetail(p_oDTDetail(lnCtr).Item("nQuantity"),
                               "Void-" & p_oDTDetail(lnCtr).Item("sDescript"),
                               p_oDTDetail(lnCtr).Item("nUnitPrce"), False, IFNull(p_oDTDetail(lnCtr).Item("sPrntPath"), ""), p_oDTDetail(lnCtr).Item("sDescript"))
                        End If
                    End If

                    lsSQL = "UPDATE " & pxeDetTable &
                            " SET cPrintedx = 1" &
                            " WHERE sTransNox = " & strParm(p_oDTMaster(0)("sTransNox")) &
                                " AND nEntryNox = " & lnCtr + 1

                    Try
                        p_oApp.Execute(lsSQL, pxeDetTable)
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        Throw ex
                    End Try

                    p_oDTDetail(lnCtr).Item("cPrintedx") = "1"
                    lnRow += 1
                    If Not lbPrint Then lbPrint = True
                End If
            Next

            'TODO: add footer
            If lbPrint Then
                '.ControlNo = p_oDTMaster(0)("nContrlNo")
                '.ReferNox = p_oDTMaster(0)("sTransNox")

                Dim lsControl As String = getNextOrderNo()
                Dim lsReferNo As String = getNextMasterNo()

                .ControlNo = lsControl
                .ReferNox = lsReferNo

                lsSQL = "UPDATE " & pxeMasTable &
                            " SET sOrderNox = " & strParm(lsControl) &
                            " WHERE sTransNox = " & strParm(p_oDTMaster(0)("sTransNox"))

                Try
                    p_oApp.Execute(lsSQL, pxeMasTable)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    Throw ex
                End Try

                lsSQL = "UPDATE Cash_Reg_Machine SET" &
                            "  sMasterNo = " & strParm(lsReferNo) &
                            ", sOrderNox = " & strParm(lsControl) &
                        " WHERE sIDNumber = " & strParm(p_sPOSNo)

                Try
                    p_oApp.Execute(lsSQL, "Cash_Reg_Machine")
                Catch ex As Exception
                    MsgBox(ex.Message)
                    Throw ex
                End Try

                If lnRow > 0 Then .PrintOrder()
            End If
        End With
    End Sub

    Public Function updateTable(ByVal lsTableNox As String) As Boolean
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

        lsSQL = AddCondition("UPDATE Table_Master SET cStatusxx = '1', nOccupnts =" & IFNull(p_oDTMaster(0).Item("nOccupnts"), 0), lsSQL)
        If lsSQL <> "" Then p_oApp.Execute(lsSQL, "Table_Master")

        Return True
    End Function

    Public Function PrintBill() As Boolean
        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        Dim lsSourceNo As String = ""
        Dim lsSourceCd As String = ""
        Dim lsSplitType As String = ""
        Dim lbCancelled As Boolean = False
        Dim lbSplitted As Boolean = False
        Dim lbReprint As Boolean
        Dim lsReferNo As String = getNextMasterNo()
        Dim lsBillNox As String = getNextBillNo()
        Dim lsSQL As String = ""

        Dim loPayment As Receipt
        loPayment = New Receipt(p_oApp)

        'Recompute total
        p_oDTMaster(0).Item("nDiscount") = 0.00
        p_oDTMaster(0).Item("nVatDiscx") = 0.00
        p_oDTMaster(0).Item("nPWDDiscx") = 0.00

        p_oDTMaster(0).Item("nVATSales") = 0.00
        p_oDTMaster(0).Item("nVATAmtxx") = 0.00
        p_oDTMaster(0).Item("nNonVATxx") = 0.00

        If p_oDTMaster(0).Item("nPrntBill") > 0 Then
            Dim lnRep As Integer

            lnRep = MsgBox("Are you sure you want to reprint bill?", vbQuestion & vbYesNo, "CONFIRMATION")
            If lnRep = vbNo Then Return False
        End If

        With loPayment
            .Cashier = p_sCashierx
            .POSNumbr = p_sTermnl
            .CRMNumbr = p_sPOSNo
            .ControlNo = p_oDTMaster(0)("nContrlNo")
            .LogName = p_sLogName
            .PosDate = p_oApp.getSysDate
            .PosDate = p_dPOSDatex
            .SplitType = 2


            If SplitOrder(lsSourceNo, lsSourceCd, lbCancelled, lsSplitType) Then
                If lbCancelled Then Return False

                'get datatable of splitted detail
                .SalesOrder = loadSplit(lsSourceNo, lsSplitType)

                .SourceCd = lsSourceCd
                .SourceNo = lsSourceNo
                .MasterNo = lsReferNo
                .BillingNo = lsBillNox
                .SplitType = lsSplitType
                lbSplitted = True

                lsSQL = "UPDATE Order_Split SET" &
                            "  nPrntBill = nPrntBill + 1" &
                            ", dPrntBill = " & dateParm(p_oApp.getSysDate) &
                            ", sBillNmbr = " & strParm(lsBillNox) &
                        " WHERE sTransNox = " & strParm(lsSourceNo)

                Try
                    p_oApp.Execute(lsSQL, "Order_Split")
                Catch ex As Exception
                    MsgBox(ex.Message)
                    Throw ex
                End Try
            Else
                'assign the actual detail since in was not splitted
                .SalesOrder = p_oDTDetail

                .SourceCd = pxeSourceCde
                .SourceNo = p_oDTMaster(0).Item("sTransNox")
                p_oDtaDiscx = LoadDiscount(.SourceCd, .SourceNo)
                .Discounts = p_oDtaDiscx
                Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

                .MasterNo = lsReferNo
                p_oDTMaster.Rows(0).Item("sBillNmbr") = lsBillNox
            End If

            If p_oDTMaster(0)("nPrntBill") = 0 Then
                lsSQL = "UPDATE " & pxeMasTable &
                        " SET  sBillNmbr = " & strParm(lsBillNox) &
                            ", nPrntBill = nPrntBill + 1" &
                            ", dPrntBill = " & dateParm(p_oApp.getSysDate) &
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0)("sTransNox"))

                .BillingNo = lsBillNox
                lbReprint = False
            Else
                lsSQL = "UPDATE " & pxeMasTable &
                        " SET  nPrntBill = nPrntBill + 1" &
                            ", dPrntBill = " & dateParm(p_oApp.getSysDate) &
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0)("sTransNox"))

                .BillingNo = IFNull(p_oDTMaster(0)("sBillNmbr"), "")
                lbReprint = True
            End If

            Try
                p_oApp.Execute(lsSQL, pxeMasTable)
            Catch ex As Exception
                MsgBox(ex.Message)
                Throw ex
            End Try

            If p_oDTMaster(0)("nPrntBill") = 0 Then
                lsSQL = "UPDATE Cash_Reg_Machine SET" &
                            "  sMasterNo = " & strParm(lsReferNo) &
                            ", sBillNmbr = " & strParm(lsBillNox) &
                        " WHERE sIDNumber = " & strParm(p_sPOSNo)
            Else
                lsSQL = "UPDATE Cash_Reg_Machine SET" &
                            "  sMasterNo = " & strParm(lsReferNo) &
                        " WHERE sIDNumber = " & strParm(p_sPOSNo)
            End If

            Try
                p_oApp.Execute(lsSQL, "Cash_Reg_Machine")
            Catch ex As Exception
                MsgBox(ex.Message)
                Throw ex
            End Try

            .NonVAT = p_oDTMaster(0).Item("nNonVATxx") - p_oDTMaster(0).Item("nVatDiscx")

            If p_oDiscount.HasDiscount Then
                '.Discounts = p_oDiscount.Discounts

                If p_oDiscount.Master("cNoneVATx") = "1" Then
                    .DiscAmount = p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")
                Else
                    .DiscAmount = p_oDTMaster(0).Item("nDiscount")
                End If
            End If

            .NewTransaction()

            'kalyptus - 2017.01.20 3:10pm
            'If sales status is a sales order from the previous day is still open then 
            'use the transaction date of that sales order
            If p_nSaleStat = 0 Then .Master("dTransact") = p_oDTMaster(0).Item("dTransact")

            .Master("nDiscount") = Math.Round(p_oDTMaster(0).Item("nDiscount"), 2)
            .Master("nVatDiscx") = Math.Round(p_oDTMaster(0).Item("nVatDiscx"), 2)
            .Master("nPWDDiscx") = Math.Round(p_oDTMaster(0).Item("nPWDDiscx"), 2)

            '.Master("nSalesAmt") = Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)
            .Master("nSalesAmt") = Math.Round((p_oDTMaster(0).Item("nTranTotl") - p_oDTMaster(0).Item("nVoidTotl")) / 1.12, 2)
            Debug.Print("totasales amt= " & .Master("nSalesAmt"))
            .Master("nVATSales") = Math.Round(p_oDTMaster(0).Item("nVATSales"), 2)
            .Master("nVATAmtxx") = Math.Round(p_oDTMaster(0).Item("nVATAmtxx"), 2)
            If p_oDTMaster(0).Item("nPWDDiscx") > 0 Then
                .Master("nSChargex") = IIf(p_bSChargex, (.Master("nSalesAmt") - (.Master("nDiscount") + .Master("nVatDiscx") + .Master("nPWDDiscx"))) * (p_nSChargex / 100), 0)

                Debug.Print("service charge amt= " & .Master("nSChargex"))
            ElseIf Not (lbSplitted) Then 'regular order
                .Master("nSChargex") = IIf(p_bSChargex, (.Master("nSalesAmt") - (.Master("nDiscount") + .Master("nVatDiscx") + .Master("nPWDDiscx"))) * (p_nSChargex / 100), 0)
                Debug.Print("totasales amt= " & .Master("nSalesAmt"))
                Debug.Print("service charge amt= " & .Master("nSChargex"))
            ElseIf (lsSplitType <> 2) Then
                .Master("nSChargex") = IIf(p_bSChargex, ((p_nTotalSales / 1.17) - (.Master("nDiscount") + .Master("nVatDiscx") + .Master("nPWDDiscx"))) * (p_nSChargex / 100), 0)
                Debug.Print("totalsales amt= " & p_nTotalSales)
                Debug.Print("service charge amt= " & .Master("nSChargex"))
            Else
                .Master("nSChargex") = IIf(p_bSChargex, ((.Master("nSalesAmt") / 1.17) - (.Master("nDiscount") + .Master("nVatDiscx") + .Master("nPWDDiscx"))) * (p_nSChargex / 100), 0)
                Debug.Print("sales amt= " & .Master("nSalesAmt"))
                Debug.Print("service charge amt= " & .Master("nSChargex"))
            End If

            p_oDTMaster(0).Item("nPrntBill") = p_oDTMaster(0).Item("nPrntBill") + 1

            p_oApp.SaveEvent("0013", "Order No. " & p_oDTMaster(0)("nContrlNo"), p_sSerial)
            If (p_oDTMaster(0).Item("nPrntBill") = 0) Then
                lbReprint = False
            End If
            Return .printBilling(lbReprint)
        End With
    End Function

    Public Function PayOrder() As Boolean
        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        Dim lbSuccess As Boolean
        Dim lsSourceNo As String = ""
        Dim lsSourceCd As String = ""
        Dim lsSplitType As String = ""
        Dim lsBillNmbrx As String = ""
        Dim lbCancelled As Boolean = False
        Dim lbSplitted As Boolean = False
        Dim lsReferNo As String = getNextMasterNo()

        Dim loPayment As Receipt
        loPayment = New Receipt(p_oApp)
        loPayment.myBill = p_nBill

        loPayment.myCharge = p_nCharge
        'Recompute total
        p_oDTMaster(0).Item("nDiscount") = 0.00
        p_oDTMaster(0).Item("nVatDiscx") = 0.00
        p_oDTMaster(0).Item("nPWDDiscx") = 0.00

        p_oDTMaster(0).Item("nVATSales") = 0.00
        p_oDTMaster(0).Item("nVATAmtxx") = 0.00
        p_oDTMaster(0).Item("nNonVATxx") = 0.00
        'p_sTrantype = p_oDTMaster(0).Item("cTranType")
        With loPayment
            .Cashier = p_sCashierx
            .POSNumbr = p_sTermnl
            .CRMNumbr = p_sPOSNo
            .SerialNo = p_sSerial
            .TranMode = p_cTrnMde
            .AccrdNumber = p_sAccrdt
            .ClientNo = p_nNoClient
            .WithDisc = p_nWithDisc
            .TableNo = p_nTableNo
            .TranType = p_sTrantype
            .LogName = p_sLogName
            .PosDate = p_dPOSDatex
            .MergeTable = p_sMergeTb


            If SplitOrder(lsSourceNo, lsSourceCd, lbCancelled, lsSplitType, lsBillNmbrx) Then
                If lbCancelled Then
                    Return False
                End If

                'get datatable of splitted detail
                .SalesOrder = loadSplit(lsSourceNo, lsSplitType)
                .SplitType = lsSplitType
                .SourceCd = "SOSp"
                .SourceNo = lsSourceNo
                .MasterNo = lsReferNo
                If (lsSplitType <> ggcRetailSales.SplitOrder.xeSplitType.xeSplitByMenu) Then
                    loPayment.myBill = p_nBill
                    If (p_bSChargex) Then
                        loPayment.myCharge = p_nCharge
                    End If
                Else
                    '1.17 with scharge / 1.2 w/o servicecharge
                    Dim lnSales As Decimal
                    Dim lnVatSales As Decimal
                    Dim lnSalesAmt As Decimal
                    Dim lnServiceCharge As Decimal
                    lnSales = Math.Round(p_nBill / 0.05, 2)
                    lnVatSales = Math.Round(p_nBill / 1.17, 2)
                    lnSalesAmt = Math.Round(lnVatSales * 1.12, 2)

                    lnServiceCharge = Math.Round(lnVatSales * 0.05, 2)
                    loPayment.myBill = Math.Round(lnSalesAmt, 2)
                    loPayment.myCharge = lnServiceCharge

                End If
                .SplitSource = p_oDTMaster(0)("sTransNox")
                .BillingNo = lsBillNmbrx

                p_oDtaDiscx = LoadDiscount(.SourceCd, .SourceNo)
                .Discounts = p_oDtaDiscx

                lbSplitted = True
            Else
                'assign the actual detail since in was not splitted
                .SalesOrder = p_oDTDetail
                .SplitType = 2
                .SourceCd = pxeSourceCde
                .SourceNo = p_oDTMaster(0)("sTransNox")
                .MasterNo = lsReferNo
                .BillingNo = IFNull(p_oDTMaster.Rows(0).Item("sBillNmbr"), "")

                p_oDtaDiscx = LoadDiscount(.SourceCd, .SourceNo)
                .Discounts = p_oDtaDiscx

                ''**************************************
                'For Each row As DataRow In p_oDtaDiscx.Rows
                '    For Each column As DataColumn In p_oDtaDiscx.Columns
                '        rowData = rowData & column.ColumnName & "=" & row(column) & " "
                '    Next
                '    rowData = rowData & vbNewLine & vbNewLine
                'Next

                'MsgBox(rowData)
                ''**************************************
                'Recompute total
                Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
            End If

            Dim lsSQL As String

            lsSQL = "UPDATE Cash_Reg_Machine SET" &
                        "  sMasterNo = " & strParm(lsReferNo) &
                    " WHERE sIDNumber = " & strParm(p_sPOSNo)

            Try
                p_oApp.Execute(lsSQL, "Cash_Reg_Machine")
            Catch ex As Exception
                MsgBox(ex.Message)
                Throw ex
            End Try
            'MsgBox("1")
            .NonVAT = p_oDTMaster(0).Item("nNonVATxx") - p_oDTMaster(0).Item("nVatDiscx")

            If p_oDiscount.HasDiscount Then
                '.Discounts = p_oDiscount.Discounts

                If p_oDiscount.Master("cNoneVATx") = "1" Then
                    .DiscAmount = p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")
                Else
                    .DiscAmount = p_oDTMaster(0).Item("nDiscount")
                End If
            End If

            .NewTransaction()

            'kalyptus - 2017.01.20 3:10pm
            'If sales status is a sales order from the previous day is still open then 
            'use the transaction date of that sales order
            If p_nSaleStat = 0 Then .Master("dTransact") = p_oDTMaster(0).Item("dTransact")

            .Master("nDiscount") = Math.Round(p_oDTMaster(0).Item("nDiscount") + 0.00001, 2)
            .Master("nVatDiscx") = Math.Round(p_oDTMaster(0).Item("nVatDiscx") + 0.00001, 2)
            .Master("nPWDDiscx") = Math.Round(p_oDTMaster(0).Item("nPWDDiscx") + 0.00001, 2)
            'MsgBox(pnCharge.ToString)
            .Master("nSalesAmt") = Math.Round(p_oDTMaster(0).Item("nTranTotl") / 1.12, 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")) + pnCharge + 0.00001, 2)
            'MsgBox((p_oDTMaster(0).Item("nTranTotl") / 1.12 - (p_oDTMaster(0).Item("nPWDDiscx")).ToString))
            '.Master("nSalesAmt") = Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + .Master("nDiscount") + .Master("nVatDiscx") + .Master("nPWDDiscx")), 2)

            '270 - (0 + 0 + 14.46 + 24.11), 2)
            '.Master("nSalesAmt") = Math.Round(p_oDTMaster(0).Item("nTranTotl") / 1.12, 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + .Master("nPWDDiscx")), 2)

            .Master("nVATSales") = Math.Round(p_oDTMaster(0).Item("nVATSales"), 2)
            .Master("nVATAmtxx") = Math.Round(p_oDTMaster(0).Item("nVATAmtxx"), 2)

            If p_oDTMaster(0).Item("nPWDDiscx") > 0 Then
                .Master("nSChargex") = IIf(p_bSChargex, (.Master("nSalesAmt")) * (p_nSChargex / 100), 0)
            Else
                .Master("nSChargex") = IIf(p_bSChargex, (.Master("nSalesAmt") / 1.12) * (p_nSChargex / 100), 0)
            End If

            'PayForm will appear
            .ClientNo = p_nNoClient
            .WithDisc = p_nWithDisc
            .TableNo = p_nTableNo
            lbSuccess = .payTransaction()
            'MsgBox("2")

            If lbSuccess Then
                Dim lnPayCtr As Integer
                lnPayCtr = IIf(.CashAmount > 0, 1, 0) +
                            IIf(.CreditCardAmount > 0, 1, 0) +
                            IIf(.CheckAmount > 0, 1, 0) +
                            IIf(.GCAmount > 0, 1, 0) +
                            IIf(.DSAmount > 0, 1, 0)

                If lnPayCtr > 0 Then
                    Dim loSOPay As Payment

                    loSOPay = New Payment(p_oApp)
                    loSOPay.SourceCd = loPayment.SourceCd 'pxeSourceCde
                    loSOPay.SourceNo = loPayment.SourceNo 'p_oDTMaster(0)("sTransNox")
                    loSOPay.NewTransaction()

                    If .CashAmount > 0 Then
                        Dim lnCash As Decimal

                        loSOPay.AddPayment()

                        lnCash = .CashAmount
                        If .GCAmount > 0 Then
                            If .CashAmount > .GCAmount Then
                                lnCash = (.Master("nSalesAmt") + .Master("nSChargex")) - .GCAmount
                            Else
                                lnCash = .Master("nSalesAmt") + .Master("nSChargex")
                                lnCash = lnCash - .GCAmount
                            End If
                        End If

                        If .CreditCardAmount > 0 Then
                            lnCash = Math.Abs((.Master("nSalesAmt") + .Master("nSChargex")) - .CreditCardAmount)
                        End If

                        loSOPay.Master(loSOPay.ItemCount - 1, "sTransNox") = .Master("sTransNox")
                        loSOPay.Master(loSOPay.ItemCount - 1, "cPaymForm") = 0
                        loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = lnCash
                    End If

                    If .CreditCardAmount > 0 Then
                        loSOPay.AddPayment()
                        loSOPay.Master(loSOPay.ItemCount - 1, "sTransNox") = .Master("sTransNox")
                        loSOPay.Master(loSOPay.ItemCount - 1, "cPaymForm") = 1
                        loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = .CreditCardAmount
                    End If

                    If .CheckAmount > 0 Then
                        loSOPay.AddPayment()
                        loSOPay.Master(loSOPay.ItemCount - 1, "sTransNox") = .Master("sTransNox")
                        loSOPay.Master(loSOPay.ItemCount - 1, "cPaymForm") = 2
                        loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = .CheckAmount
                    End If



                    If .GCAmount > 0 Then
                        loSOPay.AddPayment()
                        loSOPay.Master(loSOPay.ItemCount - 1, "sTransNox") = .Master("sTransNox")
                        loSOPay.Master(loSOPay.ItemCount - 1, "cPaymForm") = 3

                        Dim lnActGC As Decimal = 0
                        lnActGC = .Master("nSalesAmt") + .Master("nSChargex")
                        lnActGC = lnActGC - (.Master("nDiscount") + .Master("nVatDiscx") + .Master("nPWDDiscx"))

                        If .CashAmount > 0 Then
                            If (.CashAmount + .GCAmount) > (.Master("nSalesAmt") + .Master("nSChargex")) Then
                                If .CashAmount > .GCAmount Then
                                    If .GCAmount <= (.Master("nSalesAmt") + .Master("nSChargex")) Then
                                        lnActGC = .GCAmount
                                    Else
                                        lnActGC = (.Master("nSalesAmt") + .Master("nSChargex")) - .CashAmount
                                    End If
                                Else
                                    lnActGC = .GCAmount
                                End If
                            Else
                                If .CashAmount > .GCAmount Then
                                    lnActGC = .GCAmount
                                Else
                                    lnActGC = lnActGC - .CashAmount
                                End If
                            End If
                            loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = lnActGC
                        Else
                            If .GCAmount > lnActGC Then
                                loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = lnActGC
                            Else
                                loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = .GCAmount
                            End If
                        End If
                    End If

                    If .DSAmount > 0 Then
                        loSOPay.AddPayment()
                        loSOPay.Master(loSOPay.ItemCount - 1, "sTransNox") = .Master("sTransNox")
                        loSOPay.Master(loSOPay.ItemCount - 1, "cPaymForm") = 4
                        'loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = .DSAmount
                        Dim lnActDS As Decimal = 0
                        lnActDS = .Master("nSalesAmt") + .Master("nSChargex")
                        lnActDS = lnActDS - (.Master("nDiscount") + .Master("nVatDiscx") + .Master("nPWDDiscx"))

                        If .DSAmount > 0 Then
                            If (.DSAmount + .DSAmount) > (.Master("nSalesAmt") + .Master("nSChargex")) Then
                                If .DSAmount > .DSAmount Then
                                    If .DSAmount <= (.Master("nSalesAmt") + .Master("nSChargex")) Then
                                        lnActDS = .DSAmount
                                    Else
                                        lnActDS = (.Master("nSalesAmt") + .Master("nSChargex")) - .DSAmount
                                    End If
                                Else
                                    lnActDS = .DSAmount
                                End If
                            Else
                                If .DSAmount > .DSAmount Then
                                    lnActDS = .GCAmount
                                Else
                                    lnActDS = lnActDS - .CashAmount
                                End If
                            End If
                            loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = lnActDS
                        Else
                            If .DSAmount > lnActDS Then
                                loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = lnActDS
                            Else
                                loSOPay.Master(loSOPay.ItemCount - 1, "nAmountxx") = .DSAmount
                            End If
                        End If
                    End If

                    loSOPay.SaveTransaction()
                    'MsgBox("3")
                End If
            End If
        End With

        If lbSuccess Then
            PostOrder(lbSplitted, lsSourceNo)
        End If

        Return lbSuccess
    End Function

    Public Function MergeOrder() As Boolean
        Dim loMerge As MergeOrder

        loMerge = New MergeOrder(p_oApp)

        With loMerge
            .Branch = p_oApp.BranchCode
            .Terminal = p_sTermnl
            .ShowMergeTable()
        End With

        Return True
    End Function

    Private Function SplitOrder(ByRef loSource As String, ByRef loSrcCode As String, ByRef loCancelled As Boolean, ByRef loSplitType As String, Optional loBillNmbr As String = "") As Boolean
        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        Dim loSplit As SplitOrder

        loSplit = New SplitOrder(p_oApp)

        With loSplit
            .Branch = p_oApp.BranchCode
            .GroupNo = 1
            .POSNumbr = p_sTermnl
            .SourceNo = p_oDTMaster(0)("sTransNox")
            Debug.Print(p_oDTMaster(0).Item("nTranTotl"))
            .SalesTotal = p_oDTMaster(0).Item("nTranTotl") - Math.Round((p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)
            .OrderDetail = p_oDTDetail
            .IsWithSCharge = IIf(p_oDTMaster(0)("cSChargex") = "x", False, p_oDTMaster(0)("cSChargex"))
            .SCharge = p_nSChargex
            .OpenBySource()

            If .WasSplitted Then
                .ShowSplitPaymentForm()
                If .Cancelled Then
                    loCancelled = True
                    Return True
                Else
                    loCancelled = False
                End If

                loSplitType = .SplitType
                loSource = .Master("sTransNox") ' .TransNo
                loSrcCode = "SOSp"

                p_oDTMaster(0).Item("nDiscount") = .Master("nDiscount")
                p_oDTMaster(0).Item("nVatDiscx") = .Master("nVatDiscx")
                p_oDTMaster(0).Item("nPWDDiscx") = .Master("nPWDDiscx")
                Debug.Print(.Master("nAmountxx"))
                p_oDTMaster(0).Item("nTranTotl") = .Master("nAmountxx")

                Debug.Print(.Master("nAmountxx"))
                'total off split .Master("nAmountxx") + .Master("nVATAmtxx")
                p_nBill = .Master("nAmountxx") + .Master("nVATAmtxx")
                p_nCharge = .SCharge
                p_oDTMaster(0).Item("nVoidTotl") = .Master("nVoidTotl")
                p_oDTMaster(0).Item("nVATSales") = .Master("nVATSales")
                p_oDTMaster(0).Item("nVATAmtxx") = .Master("nVATAmtxx")
                p_oDTMaster(0).Item("nNonVATxx") = .Master("nNonVATxx")
                loBillNmbr = IFNull(.Master("sBillNmbr"), "")

                Return True
            Else
                Return False
            End If
        End With
    End Function

    Private Function showMergeDiscount() As Boolean
        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        Dim loFrm As frmDiscountMerger
        loFrm = New frmDiscountMerger
        loFrm.TransClass = New New_Sales_Order(p_oApp)
        loFrm.Show()

        Return True
    End Function

    Public Function SplitOrder() As Boolean
        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        Dim loSplit As SplitOrder

        loSplit = New SplitOrder(p_oApp)

        With loSplit
            .Branch = p_oApp.BranchCode
            .GroupNo = 1
            .POSNumbr = p_sTermnl
            .SourceNo = p_oDTMaster(0)("sTransNox")

            .SalesTotal = Math.Round((p_oDTMaster(0).Item("nTranTotl") + IFNull(p_oDTMaster(0).Item("nSChargex"), 0)) - Math.Round((p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2), 2)
            .OrderDetail = p_oDTDetail
            .IsWithSCharge = IIf(p_oDTMaster(0)("cSChargex") = "x", False, p_oDTMaster(0)("cSChargex"))
            .SCharge = p_nSChargex
            .OpenBySource()
            If .WasSplitted Then

                MsgBox("Unable to Re-Split Order! Please Pay Transaction", MsgBoxStyle.Information, "Notice")

                Return False
                Dim lnCtr As Integer

                For lnCtr = 1 To .GroupNo
                    .SetNo = lnCtr


                    If .Master("cPaidxxxx") <> "0" Then

                    End If


                Next
            End If

            .ShowSplitForm()
        End With

        Return True
    End Function

    Private Function Charge_PostOrder(ByVal foSalesOrder As New_Sales_Order, Optional ByVal fbSplitted As Boolean = False) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lnRow As Integer
        Dim lbPaidAll As Boolean

        If fbSplitted = True Then
            lsSQL = "SELECT" &
                "  a.sTransNox" &
                ", a.cPaymForm" &
                ", a.sReferNox" &
                ", a.nAmountxx" &
                ", a.cSplitTyp" &
                ", IF(b.sTransNox IS NULL, 0, 1) xPaidxxxx" &
            " FROM Order_Split a" &
                 " LEFT JOIN Receipt_Master b" &
                    " ON a.sTransNox = b.sSourceNo" &
                    " AND b.sSourceCd = 'SOSp'" &
            " WHERE a.sReferNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                " AND IF(b.sTransNox IS NULL, 0, 1) = '0'"

            loDT = p_oApp.ExecuteQuery(lsSQL)

            If loDT.Rows.Count > 0 Then
                lbPaidAll = False
                Return True
            Else
                lsSQL = "UPDATE " & pxeMasTable &
                        " SET cTranStat = " & strParm("5") &
                            ", nTranTotl = " & foSalesOrder.p_oDTMaster(0).Item("nTranTotl") &
                        " WHERE sTransnox = " & strParm(foSalesOrder.p_oDTMaster(0).Item("sTransNox"))
                lbPaidAll = True
            End If
        Else
            If Trim(IFNull(foSalesOrder.p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
                lsSQL = "UPDATE " & pxeMasTable &
                        " SET cTranStat = " & strParm("5") &
                            ", nTranTotl = " & foSalesOrder.p_oDTMaster(0).Item("nTranTotl") &
                        " WHERE sTransnox = " & strParm(foSalesOrder.p_oDTMaster(0).Item("sTransNox"))
            Else
                lsSQL = "UPDATE " & pxeMasTable &
                        " SET cTranStat = " & strParm("5") &
                            ", nTranTotl = " & foSalesOrder.p_oDTMaster(0).Item("nTranTotl") &
                        " WHERE sMergeIDx = " & strParm(foSalesOrder.p_oDTMaster(0).Item("sMergeIDx"))
            End If
            lbPaidAll = True
        End If

        Try
            p_oApp.BeginTransaction()
            p_oApp.Execute(lsSQL, pxeMasTable)
        Catch ex As Exception
            p_oApp.RollBackTransaction()
            MsgBox(ex.Message)
        Finally
            p_oApp.CommitTransaction()
        End Try

        If p_oDTMaster(0).Item("sTableNox") <> "" And
            lbPaidAll Then
            If CInt(p_oDTMaster(0).Item("sTableNox")) > 0 Then
                If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
                    lsSQL = "UPDATE Table_Master SET" &
                            " cStatusxx = '0'" &
                            ", nOccupnts = '0'" &
                        " WHERE nTableNox = " & CDbl(foSalesOrder.p_oDTMaster(0).Item("sTableNox"))

                    Try
                        lnRow = p_oApp.Execute(lsSQL, "Table_Master")
                        If lnRow <= 0 Then
                            MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                            Return False
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                Else
                    lsSQL = "SELECT sTableNox FROM SO_Master WHERE sMergeIDx = " & strParm(foSalesOrder.p_oDTMaster(0).Item("sMergeIDx"))

                    loDT = p_oApp.ExecuteQuery(lsSQL)

                    Try
                        Dim lnCtr As Integer

                        For lnCtr = 0 To loDT.Rows.Count - 1
                            If IFNull(loDT(lnRow)("sTableNox"), "") <> "" Then
                                lsSQL = "UPDATE Table_Master SET" &
                                            " cStatusxx = '0'" &
                                        " WHERE nTableNox = " & CDbl(loDT(lnCtr)("sTableNox"))

                                lnRow = p_oApp.Execute(lsSQL, "Table_Master")

                                If lnRow <= 0 Then
                                    MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                    Return False
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If
            End If
        End If

        Return True
    End Function

    Public Function ChangeQty(ByVal fnRowNo As Integer) As Boolean
        'Do not allow kwik-deduction if ordered item has a promo...
        If p_oDTDetail(fnRowNo).Item("cWthPromo") = "1" Then
            MsgBox("Changing the quantity of Ordered item with promo is not allowed!" & vbCrLf &
                   "Please reverse this order and enter the new order information...", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Sales Order")
            Return False
        End If

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

        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        RaiseEvent DisplayRow(fnRowNo)
        Return True

    End Function

    Public Function ChangePrice(ByVal fnRowNo As Integer) As Boolean
        Dim lsSQL As String
        Dim lnNewPrice As Double

        If p_oDTDetail(fnRowNo).Item("cWthPromo") = "1" Then
            MsgBox("Changing the price of Ordered item with promo is not allowed!" & vbCrLf &
                   "Please reverse this order and enter the new order information...", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Sales Order")
            Return False
        End If

        Dim loForm As frmChangePrice

        loForm = New frmChangePrice
        With loForm
            .Description = p_oDTDetail(fnRowNo)("sDescript")
            .UnitPrice = p_oDTDetail(fnRowNo)("nUnitPrce")
            .ShowDialog()

            If .Cancel Then Return False

            If Not p_oApp.getUserApproval() Then Return False

            lnNewPrice = .UnitPrice

            If lnNewPrice = p_oDTDetail(fnRowNo)("nUnitPrce") Then Return True
        End With

        Try
            If p_sParent = "" Then p_oApp.BeginTransaction()

            lsSQL = "UPDATE " & pxeDetTable &
                    " SET nUnitPrce = " & lnNewPrice &
                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                        " AND nEntryNox = " & fnRowNo + 1

            p_oApp.Execute(lsSQL, pxeDetTable)
        Catch ex As Exception
            If p_sParent = "" Then p_oApp.RollBackTransaction()
            Return False
        Finally
            If p_sParent = "" Then p_oApp.CommitTransaction()
        End Try

        p_oDTDetail(fnRowNo).Item("nUnitPrce") = lnNewPrice

        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        RaiseEvent DisplayRow(fnRowNo)

        Return True
    End Function

    Public Function ManageTable(ByVal fnTableNo As Integer) As Boolean
        Dim lbSuccess As Boolean
        Dim loDT As DataTable

        loDT = p_oApp.ExecuteQuery("SELECT sTransNox,cTranType FROM SO_Master" &
                                            " WHERE sTableNox = " & fnTableNo &
                                                " AND cTranStat = 0")

        If loDT.Rows.Count = 0 Then Return False

        If IsNothing(p_oTable) Then
            p_oTable = New TableMaster(p_oApp)
        End If
        p_oTable.TranNo = loDT.Rows(0)("sTransNox")
        With p_oTable
            If Not .OpenTable(fnTableNo) Then
                MsgBox("No table with the given number found.", MsgBoxStyle.Information, "Notice")
                Return False
            End If

            '.Master("nOccupnts") = IFNull(p_oDTMaster(0).Item("nOccupnts"), 0)
            .WithSCharge = IIf(p_oDTMaster.Rows(0)("cSChargex") = "x", False, p_oDTMaster.Rows(0)("cSChargex"))
            .TranType = (p_oDTMaster.Rows(0)("cTranType"))
            lbSuccess = .showManageTable
            p_bSChargex = .WithSCharge
            p_sTrantype = .TranType
            'Jovan revised computation for service charge based on total amount due
            p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2))) * (p_nSChargex / 100), 0)
            p_oDTMaster(0).Item("nOccupnts") = .Master("nOccupnts")
            p_oDTMaster(0).Item("cSChargex") = IIf(.WithSCharge, 1, 0)
            'p_oDTMaster(0).Item("cTranType") = IIf(.isDelivery, 1, 0)
            p_oDTMaster(0).Item("cTranType") = .Master("cTranType")

            If lbSuccess Then
                p_sTrantype = .TranType
                Dim lsSQL As String = "UPDATE SO_Master SET" &
                                        " cSChargex = " & strParm(p_oDTMaster.Rows(0)("cSChargex")) &
                                        ", cTranType = " & strParm(p_sTrantype) &
                                    " WHERE sTransNox = " & strParm(p_oDTMaster.Rows(0)("sTransNox"))
                If lsSQL <> "" Then p_oApp.Execute(lsSQL, "SO_Master")
            End If
        End With

        Return lbSuccess
    End Function

    Public Function showTable() As Boolean
        Dim lbSuccess As Boolean

        If IsNothing(p_oTable) Then p_oTable = New TableMaster(p_oApp)

        With p_oTable
            .SalesOrder = Me
            .Terminal = p_sTermnl

            lbSuccess = .showTables
            If lbSuccess Then
                If .TableNo <> "" Then LoadTable(.TableNo)
            End If
            'If Not .OpenTable(fnTableNo) Then
            '    MsgBox("No table with the given number found.", MsgBoxStyle.Information, "Notice")
            '    Return False
            'End If

            ''.Master("nOccupnts") = IFNull(p_oDTMaster(0).Item("nOccupnts"), 0)
            '.WithSCharge = IIf(p_oDTMaster.Rows(0)("cSChargex") = "x", False, p_oDTMaster.Rows(0)("cSChargex"))
            'lbSuccess = .showManageTable
            'p_bSChargex = .WithSCharge

            ''Jovan revised computation for service charge based on total amount due
            'p_oDTMaster.Rows(0)("nSChargex") = IIf(p_bSChargex, ((Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2))) * (p_nSChargex / 100), 0)
            'p_oDTMaster(0).Item("nOccupnts") = .Master("nOccupnts")
            'p_oDTMaster.Rows(0)("cSChargex") = IIf(.WithSCharge, 1, 0)

            'If lbSuccess Then
            '    Dim lsSQL As String = "UPDATE SO_Master SET" &
            '                            " cSChargex = " & strParm(p_oDTMaster.Rows(0)("cSChargex")) &
            '                        " WHERE sTransNox = " & strParm(p_oDTMaster.Rows(0)("sTransNox"))
            '    If lsSQL <> "" Then p_oApp.Execute(lsSQL, "SO_Master")
            'End If
        End With

        Return lbSuccess
    End Function

    Private Function PostChargeOrder() As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer

        lsSQL = "UPDATE " & pxeMasTable &
                        " SET cTranStat = " & strParm(5) &
                        ", nPrntBill = nPrntBill + 1" &
                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox"))

        Try
            lnRow = p_oApp.Execute(lsSQL, pxeMasTable)
            If lnRow <= 0 Then
                MsgBox("Unable to Post Charge Order Transaction!!!" & vbCrLf &
                        "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try

        'this required generate Daily_Summary first
        'lsSQL = "UPDATE Daily_Summary" &
        '            " SET nChrgAmnt = nChrgAmnt + " & CDbl(p_oDTMaster(0).Item("nTranTotl")) &
        '         " WHERE sTranDate = " & strParm(Format(p_dPOSDatex, "yyyyMMdd")) &
        '            " AND sCRMNumbr = " & strParm(p_sPOSNo) &
        '            " AND sCashierx = " & strParm(p_oApp.UserID)

        'Try
        '    lnRow = p_oApp.Execute(lsSQL, pxeMasTable)
        '    If lnRow <= 0 Then
        '        MsgBox("Unable to Post Charge Order Transaction!!" & vbCrLf &
        '                "Please generate Daily Summary !!!", MsgBoxStyle.Critical, "WARNING")
        '        Return False
        '    End If
        'Catch ex As Exception
        '    Throw ex
        'End Try

        Return True
    End Function

    Private Function PostOrder(Optional ByVal fbSplitted As Boolean = False, Optional ByVal fsSplitNox As String = "") As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lnRow As Integer
        Dim lbPaidAll As Boolean

        If fbSplitted = True Then
            lsSQL = "SELECT" &
                "  a.sTransNox" &
                ", a.cPaymForm" &
                ", a.sReferNox" &
                ", a.nAmountxx" &
                ", a.cSplitTyp" &
                ", IF(b.sTransNox IS NULL, 0, 1) xPaidxxxx" &
            " FROM Order_Split a" &
                 " LEFT JOIN Receipt_Master b" &
                    " ON a.sTransNox = b.sSourceNo" &
                    " AND b.sSourceCd = 'SOSp'" &
            " WHERE a.sReferNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                " AND IF(b.sTransNox IS NULL, 0, 1) = '0'"

            loDT = p_oApp.ExecuteQuery(lsSQL)

            If loDT.Rows.Count > 0 Then
                lsSQL = "UPDATE Order_Split SET" &
                            " cTranStat = " & strParm(xeTranStat.TRANS_POSTED) &
                        " WHERE sTransnox = " & strParm(fsSplitNox)
                p_oApp.Execute(lsSQL, "Order_Split")
                'MsgBox("4")
                lbPaidAll = False
                Return True
            Else
                lsSQL = "UPDATE Order_Split SET" &
                           " cTranStat = " & strParm(xeTranStat.TRANS_POSTED) &
                       " WHERE sTransnox = " & strParm(fsSplitNox)
                p_oApp.Execute(lsSQL, "Order_Split")
                'MsgBox("4")

                lsSQL = "UPDATE " & pxeMasTable &
                        " SET cTranStat = " & strParm(xeTranStat.TRANS_POSTED) &
                            ", nTranTotl = " & p_oDTMaster(0).Item("nTranTotl") &
                        " WHERE sTransnox = " & strParm(p_oDTMaster(0).Item("sTransNox"))
                lbPaidAll = True
            End If
        Else
            If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
                lsSQL = "UPDATE " & pxeMasTable &
                        " SET cTranStat = " & strParm(xeTranStat.TRANS_POSTED) &
                            ", nTranTotl = " & p_oDTMaster(0).Item("nTranTotl") &
                        " WHERE sTransnox = " & strParm(p_oDTMaster(0).Item("sTransNox"))
            Else
                lsSQL = "UPDATE " & pxeMasTable &
                        " SET cTranStat = " & strParm(xeTranStat.TRANS_POSTED) &
                            ", nTranTotl = " & p_oDTMaster(0).Item("nTranTotl") &
                        " WHERE sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx"))
            End If
            lbPaidAll = True
        End If


        Try
            p_oApp.BeginTransaction()
            p_oApp.Execute(lsSQL, pxeMasTable)
            'MsgBox("5")
        Catch ex As Exception
            p_oApp.RollBackTransaction()
            MsgBox(ex.Message)
        Finally
            p_oApp.CommitTransaction()
        End Try


        If p_oDTMaster(0).Item("sTableNox") <> "" And
            lbPaidAll Then
            If CInt(p_oDTMaster(0).Item("sTableNox")) > 0 Then

                If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
                    lsSQL = "UPDATE Table_Master SET" &
                            " cStatusxx = '0'" &
                        " WHERE nTableNox = " & CDbl(p_oDTMaster(0).Item("sTableNox"))

                    Try
                        lnRow = p_oApp.Execute(lsSQL, "Table_Master")
                        If lnRow <= 0 Then
                            MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                            Return False
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                Else
                    lsSQL = "SELECT sTableNox FROM SO_Master WHERE sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx"))

                    loDT = p_oApp.ExecuteQuery(lsSQL)

                    Try
                        Dim lnCtr As Integer

                        For lnCtr = 0 To loDT.Rows.Count - 1
                            If IFNull(loDT(lnRow)("sTableNox"), "") <> "" Then
                                lsSQL = "UPDATE Table_Master SET" &
                                            " cStatusxx = '0'" &
                                        " WHERE nTableNox = " & CDbl(loDT(lnCtr)("sTableNox"))

                                lnRow = p_oApp.Execute(lsSQL, "Table_Master")

                                If lnRow <= 0 Then
                                    MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                    Return False
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If
            End If
        End If

        Return True
    End Function

    Public Function ChargeOrder() As Boolean
        Dim lnRep As Integer
        Dim lnQRResult() As String
        Dim lbSuccess As Boolean
        Dim lbSubsidy As Boolean = True
        Dim lbHsComboMeal As Boolean = False
        Dim loCharge As ChargeInvoice
        Dim loChargeMeal As ChargeInvoiceMeal

        If p_oDTMaster(0).Item("sTransNox") = "" Then Return False


        lnRep = MsgBox("Are you sure you want to charge order?", vbQuestion & vbYesNo, "CONFIRMATION")
        If lnRep = vbNo Then Return False

        If Not p_oApp.getUserApproval() Then Return False
        If (p_oApp.BranchCode = "P013") Then


            p_oFormChargeCriteria = New frmChargeCriteria(p_oApp)
            With p_oFormChargeCriteria
                .TopMost = True
                .ChargeInformation = ""
                .ShowDialog()

                p_bCancelled = .Cancelled
                If Not p_bCancelled Then
                    If .ChargeInformation <> "" Then
                        If Not RequestEmployee(.ChargeInformation) Then
                            Return False
                        End If
                    Else
                        If Not ShowQRForm() Then
                            Return False
                        End If
                    End If
                End If
            End With

            If p_sQRCode <> "" Then
                lnQRResult = validateQR(p_sQRCode)
                loChargeMeal = New ChargeInvoiceMeal(p_oApp)
                If lnQRResult.Count <= 0 Then
                    Return False
                End If

                'check if has combo meal
                For lnCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
                    If p_oDTDetail.Rows(lnCtr)("cComboMlx") = 1 Then
                        lbHsComboMeal = True
                        Exit For
                    End If
                Next

                If lbHsComboMeal Then
                    If Not getGuanzonSubsidy(lnQRResult(0), lnQRResult(1)) Then
                        lbSubsidy = False
                    End If
                End If

                'Recompute total
                p_oDTMaster(0).Item("nDiscount") = 0.00
                p_oDTMaster(0).Item("nVatDiscx") = 0.00
                p_oDTMaster(0).Item("nPWDDiscx") = 0.00

                p_oDTMaster(0).Item("nVATSales") = 0.00
                p_oDTMaster(0).Item("nVATAmtxx") = 0.00
                p_oDTMaster(0).Item("nNonVATxx") = 0.00

                p_oDtaDiscx = LoadDiscount(pxeSourceCde, p_oDTMaster(0).Item("sTransNox"))

                'Recompute total
                Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

                With loChargeMeal

                    .Cashier = p_sCashierx
                    .POSNumbr = p_sTermnl
                    .CRMNumbr = p_sPOSNo
                    .SerialNo = p_sSerial
                    .Discount = p_oDtaDiscx
                    .HasSubsidy = lbSubsidy
                    .HasComboMeal = lbHsComboMeal
                    .SalesOrder = p_oDTDetail
                    .ChargeInformation = lnQRResult
                    .NewTransaction()

                    .Master("sEmployID") = lnQRResult(0)
                    .Master("nTotalAmt") = p_oDTMaster(0)("nTranTotl")
                    .Master("dTransact") = p_oDTMaster(0)("dTransact")
                    .ShowChargeInvoiceMeal()
                    lbSuccess = Not .Cancelled

                End With
                If (lbSuccess) Then

                    'If p_oDTMaster(0).Item("nPrntBill") = 0 Then
                    '    lnRep = MsgBox("Do you want to print bill transaction?", vbQuestion & vbYesNo, "CONFIRMATION")
                    '    If lnRep = vbYes Then PrintBill()
                    'End If

                    ''If IFNull(p_oDTMaster(0).Item("nSChargex"), 0) <> 0 Then
                    ''    MsgBox("Transaction with service charge cannot entry at charge invoice..." & vbCrLf & _
                    ''                    "Please continue for  paying order..", vbCritical)
                    ''Return False
                    ''Exit Function
                    ''End If

                    loCharge = New ChargeInvoice(p_oApp)



                    With loCharge
                        .POSNo = p_sTermnl
                        .NewTransaction()
                        .Master("sClientID") = lnQRResult(0)
                        .Master("sClientNm") = lnQRResult(1)
                        .Master("sAddressx") = ""
                        .Master("sSourceCd") = pxeSourceCde
                        .Master("sSourceNo") = p_oDTMaster(0)("sTransNox")
                        .Master("nAmountxx") = p_oDTMaster(0)("nTranTotl")
                        .Master("nDiscount") = p_oDTMaster(0)("nDiscount")
                        .Master("nVatDiscx") = p_oDTMaster(0)("nVatDiscx")
                        .Master("nPWDDiscx") = p_oDTMaster(0)("nPWDDiscx")
                        .Master("cCollectd") = 0

                        .Cashier = p_sCashierx
                        .SerialNo = p_sSerial
                        .TranMode = p_cTrnMde
                        .AccrdNumber = p_sAccrdt
                        .ClientNo = p_nNoClient
                        .WithDisc = p_nWithDisc
                        .TableNo = p_nTableNo
                        .LogName = p_sLogName

                        .SalesTotal = Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)
                        .Discounts = Math.Round((p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)

                        'for printing
                        If CDate(Format(p_oDTMaster(0)("dTransact"), xsDATE_SHORT)) < CDate(Format(p_oApp.getSysDate, xsDATE_SHORT)) Then
                            .DateTransact = p_oDTMaster(0)("dTransact")
                        Else
                            .DateTransact = p_oApp.getSysDate
                        End If

                        .DateTransact = p_oDTMaster(0)("dTransact")
                        .SalesOrder = p_oDTDetail
                        .NonVAT = p_oDTMaster(0).Item("nNonVATxx") - p_oDTMaster(0).Item("nVatDiscx")

                        'jovan 2021-04-17
                        .Discount = p_oDtaDiscx

                        If p_oDiscount.HasDiscount Then
                            '.Discount = p_oDiscount.DiscountsMaster

                            If p_oDiscount.Master("cNoneVATx") = "1" Then
                                .DiscAmount = p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")
                            Else
                                .DiscAmount = p_oDTMaster(0).Item("nDiscount")
                            End If
                        End If


                        lbSuccess = .SaveTransaction()


                        If lbSuccess Then
                            If PostOrder() Then
                                .printReciept()
                                'If Not PostChargeOrder() Then
                                '    MsgBox("Unable to post charge invoice", vbCritical)
                                'End If
                            End If
                        End If
                    End With
                End If
            End If




        Else

            If p_oDTMaster(0).Item("nPrntBill") = 0 Then
                lnRep = MsgBox("Do you want to print bill transaction?", vbQuestion & vbYesNo, "CONFIRMATION")
                If lnRep = vbYes Then PrintBill()
            End If

            ''If IFNull(p_oDTMaster(0).Item("nSChargex"), 0) <> 0 Then
            ''    MsgBox("Transaction with service charge cannot entry at charge invoice..." & vbCrLf & _
            ''                    "Please continue for  paying order..", vbCritical)
            ''Return False
            ''Exit Function
            ''End If

            loCharge = New ChargeInvoice(p_oApp)

            'Recompute total
            p_oDTMaster(0).Item("nDiscount") = 0.00
            p_oDTMaster(0).Item("nVatDiscx") = 0.00
            p_oDTMaster(0).Item("nPWDDiscx") = 0.00

            p_oDTMaster(0).Item("nVATSales") = 0.00
            p_oDTMaster(0).Item("nVATAmtxx") = 0.00
            p_oDTMaster(0).Item("nNonVATxx") = 0.00

            p_oDtaDiscx = LoadDiscount(pxeSourceCde, p_oDTMaster(0).Item("sTransNox"))

            'Recompute total
            Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

            With loCharge
                .POSNo = p_sTermnl
                .NewTransaction()
                .Master("sSourceCd") = pxeSourceCde
                .Master("sSourceNo") = p_oDTMaster(0)("sTransNox")
                .Master("nAmountxx") = p_oDTMaster(0)("nTranTotl")
                .Master("nDiscount") = p_oDTMaster(0)("nDiscount")
                .Master("nVatDiscx") = p_oDTMaster(0)("nVatDiscx")
                .Master("nPWDDiscx") = p_oDTMaster(0)("nPWDDiscx")
                .Master("cCollectd") = 0

                .Cashier = p_sCashierx
                .SerialNo = p_sSerial
                .TranMode = p_cTrnMde
                .AccrdNumber = p_sAccrdt
                .ClientNo = p_nNoClient
                .WithDisc = p_nWithDisc
                .TableNo = p_nTableNo
                .LogName = p_sLogName

                .SalesTotal = Math.Round(p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)
                .Discounts = Math.Round((p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)

                'for printing
                If CDate(Format(p_oDTMaster(0)("dTransact"), xsDATE_SHORT)) < CDate(Format(p_oApp.getSysDate, xsDATE_SHORT)) Then
                    .DateTransact = p_oDTMaster(0)("dTransact")
                Else
                    .DateTransact = p_oApp.getSysDate
                End If

                .DateTransact = p_oDTMaster(0)("dTransact")
                .SalesOrder = p_oDTDetail
                .NonVAT = p_oDTMaster(0).Item("nNonVATxx") - p_oDTMaster(0).Item("nVatDiscx")

                'jovan 2021-04-17
                .Discount = p_oDtaDiscx

                If p_oDiscount.HasDiscount Then
                    '.Discount = p_oDiscount.DiscountsMaster

                    If p_oDiscount.Master("cNoneVATx") = "1" Then
                        .DiscAmount = p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")
                    Else
                        .DiscAmount = p_oDTMaster(0).Item("nDiscount")
                    End If
                End If

                .ShowChargeInvoice()
                lbSuccess = Not .Cancelled
            End With

            If lbSuccess Then
                If PostOrder() Then
                    If Not PostChargeOrder() Then
                        MsgBox("Unable to post charge invoice", vbCritical)
                    End If
                End If
            End If

        End If
        Return lbSuccess
    End Function
    Private Function getGuanzonSubsidy(fsEmployID As String, fsEmployNme As String)
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim loDTExisting As DataTable


        If fsEmployID = "" Then Return False
        'check successful checkout with discount
        lsSQL = "SELECT a.sTransNox, a.dTransact, b.sClientID, b.sClientNm, a.cSChargex" &
        " FROM SO_Master a " &
        " LEFT JOIN Charge_Invoice b ON a.sTransNox = b.sSourceNo " &
        " LEFT JOIN Discount c ON a.sTransNox = c.sSourceNo " &
        " WHERE b.sClientID = " & strParm(fsEmployID) &
        " AND a.dTransact =" & dateParm(p_oApp.getSysDate) &
        " AND c.sDiscCard = '2401' LIMIT 1"

        loDT = p_oApp.ExecuteQuery(lsSQL)

        'Return false if already used & delete discount if duplicated 
        If loDT.Rows.Count > 0 Then

            lsSQL = "SELECT sTransNox " &
                    " FROM Discount " &
                    " WHERE sSourceNo = " & strParm(p_oDTMaster(0)("sTransNox")) &
                    " AND sIDNumber = " & strParm(fsEmployID) &
                    " AND sDiscCard = '2401' LIMIT 1"

            loDTExisting = p_oApp.ExecuteQuery(lsSQL)

            If loDTExisting.Rows.Count > 0 Then
                lsSQL = "DELETE FROM Discount" &
                           " WHERE sTransNox = " & strParm(loDTExisting.Rows(0).Item("sTransNox")) &
                               " AND sIDNumber = " & strParm(fsEmployID)
                p_oApp.Execute(lsSQL, "Discount")

                lsSQL = "DELETE FROM Discount_Detail" &
                           " WHERE sTransNox = " & strParm(loDTExisting.Rows(0).Item("sTransNox")) &
                               " AND sIDNumber = " & strParm(fsEmployID)
                p_oApp.Execute(lsSQL, "Discount")
            End If
            loDTExisting.Dispose()
            Return False
        End If

        loDT.Dispose()


        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        Dim lsSourceNo As String = ""
        Dim lsSourceCd As String = ""
        Dim lsSplitType As String = ""
        Dim lbCancelled As Boolean = False
        Dim lsCategrIDx As String = ""

        p_oDiscount = New Discount(p_oApp)
        p_oDiscount.POSNumbr = p_sTermnl
        p_oDiscount.SerialNo = p_sSerial

        For lnCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
            If IFNull(p_oDTDetail.Rows(lnCtr)("sCategrID"), "") <> "" Then
                lsCategrIDx = lsCategrIDx & "'" & p_oDTDetail.Rows(lnCtr)("sCategrID") & "',"
            End If
        Next

        p_oDiscount.InitTransaction()
        p_oDiscount.Category = lsCategrIDx.Substring(0, lsCategrIDx.Length - 1)
        p_oDiscount.setTranTotal = p_nTotalSales
        'Auto entry discount

        p_oDiscount.SourceCd = pxeSourceCde
        p_oDiscount.SourceNo = p_oDTMaster(0)("sTransNox")
        p_oDiscount.SearchCard("Guanzon Subsidy")
        p_oDiscount.Master(3) = 1
        p_oDiscount.Master(4) = 1
        p_oDiscount.Detail(0, "sClientNm") = fsEmployNme
        p_oDiscount.Master("sIDNumber") = fsEmployID
        p_oDiscount.Detail(0, "sIDNumber") = fsEmployID


        If Not p_oDiscount.SaveTransaction() Then
            Return False
        End If


        'Recompute total
        If p_oDiscount.ItemCategoryCount > 0 Then
            Dim lbDiscounted As Boolean = False
            For lnCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
                If lbDiscounted = True Then
                    Exit For

                End If
                p_oDTDetail.Rows(lnCtr)("nDiscount") = 0
                p_oDTDetail.Rows(lnCtr)("nAddDiscx") = 0
                If p_oDTDetail.Rows(lnCtr)("sCategrID") <> "" Then
                    For lnRow As Integer = 0 To p_oDiscount.ItemCategoryCount - 1
                        'Debug.Print("dish" & p_oDTDetail.Rows(lnCtr)("sCategrID") & "data" & p_oDiscount.Category(lnRow, 1))
                        If p_oDTDetail.Rows(lnCtr)("sCategrID") = p_oDiscount.Category(lnRow, 1) Then
                            If IFNull(p_oDiscount.Category(lnRow, 2), 0) = 0.0 Then
                                p_oDTDetail.Rows(lnCtr)("nDiscount") = p_oDiscount.Category(lnRow, 3)
                                p_oDTDetail.Rows(lnCtr)("nAddDiscx") = p_oDiscount.Category(lnRow, 4)
                                lbDiscounted = True
                                Exit For
                            Else
                                If p_oDTDetail.Rows(lnCtr)("nUnitPrce") > p_oDiscount.Category(lnRow, 2) Then
                                    p_oDTDetail.Rows(lnCtr)("nDiscount") = p_oDiscount.Category(lnRow, 3)
                                    p_oDTDetail.Rows(lnCtr)("nAddDiscx") = p_oDiscount.Category(lnRow, 4)
                                    lbDiscounted = True
                                    Exit For
                                End If
                            End If
                        End If
                    Next lnRow
                End If
                Call saveDetail(lnCtr)
            Next lnCtr
        Else

            Dim lbDiscounted As Boolean = False
            For lnCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
                If lbDiscounted = True Then
                    Exit For

                End If
                p_oDTDetail.Rows(lnCtr)("nDiscount") = 0
                p_oDTDetail.Rows(lnCtr)("nAddDiscx") = 0
                If p_oDiscount.ItemCategoryCount > 0 Then
                    If IFNull(p_oDiscount.Category(0, "nMinAmtxx"), 0) = 0.0 Then
                        p_oDTDetail.Rows(lnCtr)("nDiscount") = p_oDiscount.Category(0, "nDiscRate")
                        p_oDTDetail.Rows(lnCtr)("nAddDiscx") = p_oDiscount.Category(0, "nDiscAmtx")
                        lbDiscounted = True
                        Exit For
                    Else
                        If p_oDTDetail.Rows(lnCtr)("nUnitPrce") > p_oDiscount.Category(0, "nMinAmtxx") Then
                            p_oDTDetail.Rows(lnCtr)("nDiscount") = p_oDiscount.Category(0, "nDiscRate")
                            p_oDTDetail.Rows(lnCtr)("nAddDiscx") = p_oDiscount.Category(0, "nDiscAmtx")
                            lbDiscounted = True
                            Exit For
                        End If
                    End If
                End If
                Call saveDetail(lnCtr)
            Next
        End If

        Return True
    End Function

    Private Function ShowQRForm() As Boolean

        Dim lnResult As Long
        ' Check if the batch file exists
        If File.Exists(Path.Combine(pxeJavaPath, "reademployee.bat")) Then
            lnResult = RMJExecuteLong(pxeJavaPath, "reademployee.bat", "")

            If lnResult = 0 Then
                If File.Exists(pxeJavaPathTemp & "pos.tmp") Then
                    ' Read and return the content of the file
                    p_sQRCode = File.ReadAllText(pxeJavaPathTemp & "pos.tmp")
                    Return True
                Else
                    MessageBox.Show("System error missing temp. Please inform MIS Support to fix the issue.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            ElseIf lnResult = 1 Then
                MessageBox.Show("Unable to load Employee QR Detail!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            ElseIf lnResult = 2 Or lnResult < 0 Then
                MessageBox.Show("System error. Please inform MIS Support to fix the issue.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        Else
            ' Path check
            MessageBox.Show("File Path Doesn't Exist " & Path.Combine(pxeJavaPath, "reademployee.bat") & " Please Inform MIS Dept !!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
    End Function

    Private Function RequestEmployee(ByVal fsEmployeeID As String) As Boolean

        Dim lnResult As Long
        ' Check if the batch file exists
        If File.Exists(Path.Combine(pxeJavaPath, "requestEmployee.bat")) Then
            lnResult = RMJExecuteLong(pxeJavaPath, "requestEmployee.bat", fsEmployeeID)

            If lnResult = 0 Then
                If File.Exists(pxeJavaPathTemp & "pos.tmp") Then
                    ' Read and return the content of the file
                    p_sQRCode = File.ReadAllText(pxeJavaPathTemp & "pos.tmp")
                    Return True
                Else
                    MessageBox.Show("System error missing temp. Please inform MIS Support to fix the issue.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            ElseIf lnResult = 1 Then
                MessageBox.Show("Unable to load Employee Detail!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            ElseIf lnResult = 2 Then
                MessageBox.Show("System error. Please inform MIS Support to fix the issue.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        Else
            ' Path check
            MessageBox.Show("File Path Doesn't Exist " & Path.Combine(pxeJavaPath, "requestEmployee.bat") & " Please Inform MIS Dept !!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
    End Function

    'Sub ShowQRResult(ByVal fsCryptQR() As String)
    '    p_oFormQRResult = New frmQRResult(p_oApp)
    '    With p_oFormQRResult
    '        .TopMost = True
    '        .ChargeInformation = fsCryptQR

    '        .ShowDialog()

    '        p_bCancelled = .Cancelled

    '    End With
    'End Sub

    Public Function validateQR(ByVal fsQRResult As String) As String()

        If Not fsQRResult = "" Then
            Dim splitQRResult() As String = fsQRResult.Split("»"c)
            Debug.Print(splitQRResult.ToString)

            'ShowQRResult(splitQRResult)


            Return splitQRResult
        End If
    End Function

    Public Function neoChargeOrder() As Boolean
        Dim loDT As DataTable
        Dim loSalesOrder As New_Sales_Order
        Dim lsSQL As String
        Dim lnCtr As Integer
        Dim loCharge As ChargeInvoice
        Dim lnRep As Integer

        lsSQL = "SELECT" &
                    " sTransNox" &
                " FROM SO_Master" &
                " WHERE cTranStat = '0'" &
                    " AND nPrntBill > '0'" &
                    " AND LEFT(sTransNox, 6) = " & strParm(p_oApp.BranchCode & p_sTermnl) &
                " ORDER BY sTransNox"


        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return True
        lnRep = MsgBox("There are open SALES ORDER!" & vbCrLf &
                        "Do you want to close the current billed transaction(s)?", vbQuestion & vbYesNo, "CONFIRMATION")
        If lnRep = vbNo Then Return False

        loSalesOrder = New New_Sales_Order(p_oApp)
        loSalesOrder.InitMachine()
        loSalesOrder.initMaster()

        loCharge = New ChargeInvoice(p_oApp)
        loCharge.HasParent = True
        For lnCtr = 0 To loDT.Rows.Count - 1
            Call loSalesOrder.LoadOrder(loDT.Rows(lnCtr)("sTransNox"))
            With loCharge
                .NewTransaction()
                .Master("sSourceCd") = pxeSourceCde
                .Master("sSourceNo") = loSalesOrder.p_oDTMaster(0).Item("sTransNox")
                .Master("nAmountxx") = loSalesOrder.p_oDTMaster(0).Item("nTranTotl")
                .Master("nDiscount") = loSalesOrder.p_oDTMaster(0).Item("nDiscount")
                .Master("nVatDiscx") = loSalesOrder.p_oDTMaster(0).Item("nVatDiscx")

                .SalesTotal = Math.Round(loSalesOrder.p_oDTMaster(0).Item("nTranTotl"), 2) - Math.Round((loSalesOrder.p_oDTMaster(0).Item("nDiscount") + loSalesOrder.p_oDTMaster(0).Item("nVatDiscx") + loSalesOrder.p_oDTMaster(0).Item("nPWDDiscx")), 2)
                .Discounts = Math.Round((loSalesOrder.p_oDTMaster(0).Item("nDiscount") + loSalesOrder.p_oDTMaster(0).Item("nVatDiscx")), 2)

                .DateTransact = loSalesOrder.Master("dTransact")
                .SalesOrder = loSalesOrder.p_oDTDetail
                .NonVAT = loSalesOrder.p_oDTMaster(0).Item("nNonVATxx") - (loSalesOrder.p_oDTMaster(0).Item("nVatDiscx"))
                .DiscAmount = loSalesOrder.p_oDTMaster(0).Item("nVatDiscx") + loSalesOrder.p_oDTMaster(0).Item("nPWDDiscx")
                Call .SaveTransaction()
            End With
            If Not Charge_PostOrder(loSalesOrder) Then Return False
        Next

        Return True
    End Function

    Public Function Complementary() As Boolean
        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        Dim lbSuccess As Boolean
        Dim loComplementary As Complementary

        loComplementary = New Complementary(p_oApp)

        With loComplementary
            .Branch = p_oApp.BranchCode
            .POSNumbr = p_sTermnl
            .SourceCd = pxeSourceCde
            .SourceNo = p_oDTMaster(0).Item("sTransNox")
            .SalesTotal = p_oDTMaster(0).Item("nTranTotl") - Math.Round((p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)
            .OrderDetail = p_oDTDetail
            If Not .OpenBySource() Then Return False

            .ShowComplementary()

            'lbSuccess = Not .Cancelled

            If .Cancelled Then Return False
            If Not p_oApp.getUserApproval() Then Return False

            lbSuccess = True
            p_nComplmnt = .Master(5)

            If lbSuccess Then
                If p_nComplmnt >= .SalesTotal Then PostOrder()

                p_oApp.SaveEvent("0025", "Order No. " & p_oDTMaster(0)("nContrlNo"), p_sSerial)
            End If

        End With

        Return lbSuccess
    End Function

    Public Function CancelOrder() As Boolean
        Dim lsSQL As String
        ' TODO: display complementary form?

        p_oApp.BeginTransaction()
        If Trim(p_oDTMaster(0).Item("sMergeIDx")) = "" Then
            lsSQL = "UPDATE " & pxeMasTable &
                    " SET cTranStat = " & strParm(xeTranStat.TRANS_CANCELLED) &
                    " WHERE sTransnox = " & strParm(p_oDTMaster(0).Item("sTransNox"))
        Else
            lsSQL = "UPDATE " & pxeMasTable &
                    " SET cTranStat = " & strParm(xeTranStat.TRANS_CANCELLED) &
                    " WHERE sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx"))
        End If

        p_oApp.Execute(lsSQL, pxeMasTable)

        p_oApp.SaveEvent("0011", "Order No. " & p_oDTMaster(0)("nContrlNo"), p_sSerial)

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

        RaiseEvent DisplayRow(fnRowNo)

        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        Return True
    End Function

    Public Function DeductItem(ByVal fnRowNo As Integer) As Boolean
        If p_bExisting Then
            ' new transaction
            If fnRowNo > p_oDTDetail.Rows.Count - 1 Then
                Return False
            ElseIf p_oDTDetail(fnRowNo).Item("cPrintedx") = xeLogical.YES Then
                Call ReverseOrder(fnRowNo, p_nQuantity)
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

        RaiseEvent DisplayRow(fnRowNo)

        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        Return True
    End Function

    Public Function VoidOrder() As Boolean
        Dim loSplit As SplitOrder
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim loDT As DataTable

        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        loSplit = New SplitOrder(p_oApp)

        With loSplit
            .SourceNo = p_oDTMaster(0)("sTransNox")
            .OrderDetail = p_oDTDetail
            .SalesTotal = p_oDTMaster(0).Item("nTranTotl") - Math.Round((p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)
            .OpenBySource()

            If .WasSplitted Then
                If .WasSplitPosted Then
                    MsgBox("Transaction already splitted..." & vbCrLf &
                            "Cannot continue voiding order... Please pay the transaction...")

                    'Return False
                End If
            End If

            If Not p_oApp.getUserApproval() Then Return False

            lsSQL = "SELECT * FROM Receipt_Master" &
                    " WHERE sSourceNo = " & strParm(p_oDTMaster.Rows(0)("sTransNox"))
            loDT = p_oApp.ExecuteQuery(lsSQL)

            If loDT.Rows.Count > 0 Then
                'lsSQL = "UPDATE Receipt_Master SET" &
                '           " cTranStat = " & strParm(xeTranStat.TRANS_CANCELLED) &
                '        " WHERE sSourceNo = " & strParm(p_oDTMaster.Rows(0)("sTransNox")) &
                '            " AND cTranStat = '0'"

                'p_oApp.ExecuteActionQuery(lsSQL)
                MsgBox("Unable to void transaction." & vbCrLf &
                            "Order was already issued receipt" & "(" & loDT.Rows(0)("sORNumber") & ").")
                Return False
            End If
            lsSQL = "UPDATE SO_Master SET" &
                        "  cTranStat = " & strParm(xeTranStat.TRANS_CANCELLED) &
                        ", sModified = " & strParm(p_sUserIDxx) &
                    " WHERE sTransNox = " & strParm(p_oDTMaster.Rows(0)("sTransNox"))
        End With

        Try
            lnRow = p_oApp.Execute(lsSQL, "SO_Master")
            If lnRow <= 0 Then
                MsgBox("Unable to Void Order!!!" & vbCrLf &
                        "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                Return False
            End If

            If p_oDTMaster(0).Item("sTableNox") <> "" Then
                If CInt(p_oDTMaster(0).Item("sTableNox")) > 0 Then
                    If Trim(IFNull(p_oDTMaster(0).Item("sMergeIDx"), "")) = "" Then
                        lsSQL = "UPDATE Table_Master SET" &
                                " cStatusxx = '0'" &
                            " WHERE nTableNox = " & CDbl(p_oDTMaster(0).Item("sTableNox"))

                        Try
                            lnRow = p_oApp.Execute(lsSQL, "Table_Master")
                            If lnRow <= 0 Then
                                MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                        "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                Return False
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try
                    Else
                        lsSQL = "SELECT sTableNox FROM SO_Master WHERE sMergeIDx = " & strParm(p_oDTMaster(0).Item("sMergeIDx"))

                        loDT = p_oApp.ExecuteQuery(lsSQL)

                        Try
                            Dim lnCtr As Integer

                            For lnCtr = 0 To loDT.Rows.Count - 1
                                If IFNull(loDT(lnRow)("sTableNox"), "") <> "" Then
                                    lsSQL = "UPDATE Table_Master SET" &
                                                " cStatusxx = '0'" &
                                                ", nOccupnts = '0'" &
                                            " WHERE nTableNox = " & CDbl(loDT(lnCtr)("sTableNox"))

                                    lnRow = p_oApp.Execute(lsSQL, "Table_Master")

                                    If lnRow <= 0 Then
                                        MsgBox("Unable to Save Transaction!!!" & vbCrLf &
                                                "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                        Return False
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            Throw ex
                        End Try
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try

        p_oApp.SaveEvent("0011", "Order No. " & p_oDTMaster(0)("nContrlNo"), p_sSerial)

        Return True
    End Function

    Public Function SalesReturn() As Boolean
        Dim loReturn As SalesReturn

        loReturn = New SalesReturn(p_oApp)

        With loReturn
            .POSNumbr = p_sTermnl
            .SerialNo = p_sSerial
            .CRMNmber = p_sPOSNo
            .LogName = p_sLogName
            .SerialNo = p_sSerial

            .TranMode = p_cTrnMde
            .AccrdNumber = p_sAccrdt
            .NoClients = p_nNoClient
            .WithDiscount = p_nWithDisc
            .LogName = p_sLogName
            .ShowRetrun()
        End With

        Return True
    End Function

    'Function getUserApproval() As Boolean
    '    Dim lofrmUserDisc As New frmUserDisc
    '    Dim loDT As New DataTable

    '    Dim lnCtr As Integer = 0
    '    Dim lbValid As Boolean = False

    '    With lofrmUserDisc
    '        Do
    '            .TopMost = True
    '            .ShowDialog()
    '            If .Cancelled = True Then
    '                Return False
    '            End If

    '            p_oSC.Connection = p_oApp.Connection
    '            p_oSC.CommandText = getSQ_User()
    '            p_oSC.Parameters.Clear()
    '            p_oSC.Parameters.AddWithValue("?sLogNamex", Encrypt(lofrmUserDisc.LogName, xsSignature))
    '            p_oSC.Parameters.AddWithValue("?sPassword", Encrypt(lofrmUserDisc.Password, xsSignature))

    '            loDT = p_oApp.ExecuteQuery(p_oSC)

    '            If loDT.Rows.Count = 0 Then
    '                MsgBox("User Does Not Exist!" & vbCrLf & "Verify log name and/or password.", vbCritical, "Warning")
    '                lnCtr += 1
    '            Else
    '                If Not isUserActive(loDT) Then
    '                    lnCtr = 0
    '                Else
    '                    If loDT.Rows(0).Item("nUserLevl") > xeUserRights.DATAENTRY Then
    '                        lbValid = True
    '                    Else
    '                        MsgBox("User is not allowed to approve this transaction!" & vbCrLf & "Verify user name and/or password.", vbCritical, "Warning")
    '                        lnCtr += 1
    '                    End If
    '                End If
    '            End If
    '        Loop Until lbValid Or lnCtr = 3
    '    End With

    '    If lbValid Then
    '        p_sUserIDxx = loDT.Rows(0).Item("sUserIDxx")
    '        p_sUserName = loDT.Rows(0).Item("sUserName")
    '        p_sLogNamex = loDT.Rows(0).Item("sLogNamex")
    '        p_nUserLevl = loDT.Rows(0).Item("nUserLevl")

    '    End If
    '    Return lbValid
    'End Function

    'Private Function getSQ_User() As String
    '    Return "SELECT sUserIDxx" &
    '          ", sLogNamex" &
    '          ", sPassword" &
    '          ", sUserName" &
    '          ", nUserLevl" &
    '          ", cUserType" &
    '          ", sProdctID" &
    '          ", cUserStat" &
    '          ", nSysError" &
    '          ", cLogStatx" &
    '          ", cLockStat" &
    '          ", cAllwLock" &
    '       " FROM xxxSysUser" &
    '       " WHERE sLogNamex = ?sLogNamex" &
    '          " AND sPassword = ?sPassword"
    'End Function

    Private Function getSQ_Category() As String
        Return "SELECT" &
                   "  sCategrCd" &
                   ", sDescript" &
                   ", IFNULL(sImgePath, '') sImgePath" &
                   ", cForwardx" &
                   ", cRecdStat" &
               " FROM Product_Category" &
               " WHERE cRecdStat = '1'" &
                    " AND sImgePath <> ''" &
               " ORDER BY cPriority DESC, sDescript ASC"
    End Function

    Private Function getSQL_DetImage(ByVal fsCategrCd As String) As String
        Return "SELECT" &
                    "  sStockIDx" &
                    ", sDescript" &
                    ", sBriefDsc" &
                    ", IFNULL(sImgePath, '') sImgePath" &
                " FROM Inventory" &
                " WHERE cRecdStat = '1'" &
                    " AND sImgePath <> ''" &
                    " AND sCategrID = " & strParm(fsCategrCd)
    End Function

    Function GetCategory() As DataTable
        Dim loDT As DataTable

        loDT = p_oApp.ExecuteQuery(getSQ_Category)

        Return loDT
    End Function

    Function GetDetailImage(ByVal fsCategrCd As String) As DataTable
        Dim loDT As DataTable

        loDT = p_oApp.ExecuteQuery(getSQL_DetImage(fsCategrCd))

        Return loDT
    End Function

    'Private Function isUserActive(ByRef loDT As DataTable) As Boolean
    '    Dim lnCtr As Integer = 0
    '    Dim lbMember As Boolean = False

    '    If loDT.Rows(0).Item("cUserType").Equals(0) Then
    '        For lnCtr = 0 To loDT.Rows.Count - 1
    '            If loDT.Rows(0).Item("sProdctID").Equals(p_oApp.ProductID) Then
    '                Exit For
    '                lbMember = True
    '            End If
    '        Next
    '    Else
    '        lbMember = True
    '    End If

    '    If Not lbMember Then
    '        MsgBox("User is not a member of this application!!!" & vbCrLf &
    '           "Application used is not allowed!!!", vbCritical, "Warning")
    '    End If

    '    ' check user status
    '    If loDT.Rows(0).Item("cUserStat").Equals(xeUserStatus.SUSPENDED) Then
    '        MsgBox("User is currently suspended!!!" & vbCrLf &
    '                 "Application used is not allowed!!!", vbCritical, "Warning")
    '        Return False
    '    End If
    '    Return True
    'End Function

    Private Function saveDetail(ByVal fnRow As Integer) As Boolean
        Dim lsSQL As String

        If Not p_bExisting Then
            p_oDTMaster(0).Item("sTransNox") = getSOTransNo()
            saveMaster()

            'Set the discount to nothing
            p_oDiscount = Nothing
            p_oDtaDiscx = Nothing

            p_bExisting = True
        End If

        p_oDTDetail(p_nRow).Item("sTransNox") = p_oDTMaster(0)("sTransNox")

        Try
            lsSQL = "INSERT INTO " & pxeDetTable &
                " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                    ", nEntryNox = " & fnRow + 1 &
                    ", sStockIDx = " & strParm(p_oDTDetail(fnRow).Item("sStockIDx")) &
                    ", cReversex = " & strParm(p_oDTDetail(fnRow).Item("cReversex")) &
                    ", nQuantity = " & p_oDTDetail(fnRow).Item("nQuantity") &
                    ", nUnitPrce = " & p_oDTDetail(fnRow).Item("nUnitPrce") &
                    ", nDiscount = " & p_oDTDetail(fnRow).Item("nDiscount") &
                    ", nAddDiscx = " & p_oDTDetail(fnRow).Item("nAddDiscx") &
                    ", cPrintedx = " & strParm(p_oDTDetail(fnRow).Item("cPrintedx")) &
                    ", cDetailxx = " & strParm(p_oDTDetail(fnRow).Item("cDetailxx")) &
                    ", cReversed = " & strParm(p_oDTDetail(fnRow).Item("cReversed")) &
                    ", cServedxx = " & strParm(p_oDTDetail(fnRow).Item("cServedxx")) &
                    ", cComboMlx = " & strParm(p_oDTDetail(fnRow).Item("cComboMlx")) &
                    ", cWthPromo = " & strParm(p_oDTDetail(fnRow).Item("cWthPromo")) &
                    ", nComplmnt = " & p_oDTDetail(fnRow).Item("nComplmnt") &
                    ", dModified = " & datetimeParm(p_oDTDetail(fnRow).Item("dModified")) &
                " ON DUPLICATE KEY UPDATE" &
                    "  sStockIDx = " & strParm(p_oDTDetail(fnRow).Item("sStockIDx")) &
                    ", nQuantity = " & p_oDTDetail(fnRow).Item("nQuantity") &
                    ", nUnitPrce = " & p_oDTDetail(fnRow).Item("nUnitPrce") &
                    ", nComplmnt = " & p_oDTDetail(fnRow).Item("nComplmnt") &
                    ", nDiscount = " & p_oDTDetail(fnRow).Item("nDiscount") &
                    ", nAddDiscx = " & p_oDTDetail(fnRow).Item("nAddDiscx") &
                    ", cReversex = " & strParm(p_oDTDetail(fnRow).Item("cReversex")) &
                    ", cReversed = " & strParm(p_oDTDetail(fnRow).Item("cReversed")) &
                    ", dModified = " & datetimeParm(p_oDTDetail(fnRow).Item("dModified"))

            p_oApp.Execute(lsSQL, pxeDetTable)
        Catch ex As Exception
            MsgBox(ex.Message)
            Throw ex
        End Try

        p_oDTDetail(fnRow).Item("cDetSaved") = 1

        Return True
    End Function

    Public Function saveOrder(ByVal fsTableNo As String) As Boolean
        Call createMaster()
        Call initMaster()

        p_oDTMaster(0).Item("sTableNox") = fsTableNo
        p_oDTMaster(0).Item("cSChargex") = 1

        Call createDetail()
        p_oDTDetail.Rows.Add()
        p_oDTDetail(0).Item("sStockIDx") = ""
        p_oDTDetail(0).Item("cReversex") = "+"
        p_oDTDetail(0).Item("nQuantity") = 0
        p_oDTDetail(0).Item("nUnitPrce") = 0
        p_oDTDetail(0).Item("nDiscount") = 0
        p_oDTDetail(0).Item("nAddDiscx") = 0
        p_oDTDetail(0).Item("cPrintedx") = 0
        p_oDTDetail(0).Item("cDetailxx") = 0
        p_oDTDetail(0).Item("cReversed") = 0
        p_oDTDetail(0).Item("cServedxx") = 0
        p_oDTDetail(0).Item("cComboMlx") = 0
        p_oDTDetail(0).Item("cWthPromo") = 0
        p_oDTDetail(0).Item("nComplmnt") = 0
        p_oDTDetail(0).Item("dModified") = p_oApp.SysDate

        Return saveDetail(0)
    End Function

    'Private Function addQty(ByVal fnRowNo As Integer) As Boolean
    '    Dim lsSQL As String
    '    Dim lnRow As Integer
    '    Dim loDT As DataTable

    '    'Do not allow kwik-deduction if ordered item has a promo...
    '    If p_oDTDetail(fnRowNo).Item("cWthPromo") = "1" And p_oDTDetail(fnRowNo).Item("cComboMlx") = "0" Then
    '        MsgBox("Changing the quantity of Ordered item with promo is not allowed!" & vbCrLf & _
    '               "Please reverse this order and enter the new order information...", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Sales Order")
    '        Return False
    '    End If

    '    Try
    '        If p_sParent = "" Then p_oApp.BeginTransaction()

    '        lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo

    '        loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

    '        If loDT.Rows.Count <> 0 Then
    '            lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

    '            loDT = p_oApp.ExecuteQuery(lsSQL)

    '            For lnRow = 1 To loDT.Rows.Count
    '                lsSQL = "UPDATE " & pxeDetTable & _
    '                        " SET nQuantity = nQuantity + " & p_nQuantity & _
    '                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                            " AND nEntryNox = " & fnRowNo + lnRow
    '                'update detail
    '                p_oApp.Execute(lsSQL, pxeDetTable)
    '                p_oDTDetail(fnRowNo + lnRow).Item("nQuantity") += p_nQuantity
    '            Next
    '        End If

    '        lsSQL = "UPDATE " & pxeDetTable & _
    '                " SET nQuantity = nQuantity + " & p_nQuantity & _
    '                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                    " AND nEntryNox = " & fnRowNo

    '        p_oApp.Execute(lsSQL, pxeDetTable)
    '    Catch ex As Exception
    '        If p_sParent = "" Then p_oApp.RollBackTransaction()
    '        Return False
    '    Finally
    '        If p_sParent = "" Then p_oApp.CommitTransaction()
    '    End Try

    '    p_oDTDetail(fnRowNo).Item("nQuantity") += p_nQuantity
    '    RaiseEvent DisplayRow(fnRowNo)

    '    Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
    '    Return True
    'End Function

    Private Function addQty(ByVal fnRowNo As Integer) As Boolean
        Dim lsSQL As String

        'Do not allow kwik-deduction if ordered item has a promo...
        If p_oDTDetail(fnRowNo).Item("cWthPromo") = "1" Then
            MsgBox("Changing the quantity of Ordered item with promo is not allowed!" & vbCrLf &
                   "Please reverse this order and enter the new order information...", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Sales Order")
            Return False
        End If

        If p_oDTDetail(fnRowNo).Item("cComboMlx") = "1" Then
            MsgBox("Changing the quantity of Ordered combo item is not allowed!" & vbCrLf &
                   "Please reverse this order and enter the new order information...", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Sales Order")
            Return False
        End If

        Try
            If p_sParent = "" Then p_oApp.BeginTransaction()

            Dim lnDtlRow As Integer = 0
            Dim lnQuantity As Integer

            'Check if item has detail
            If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
                Do While p_oDTDetail(fnRowNo + lnDtlRow + 1).Item("cDetailxx") = "1"

                    'update all items of this master combo/add-on
                    lnDtlRow += 1
                    lnQuantity = Math.DivRem(p_oDTDetail(fnRowNo + lnDtlRow).Item("nQuantity"), p_oDTDetail(fnRowNo).Item("nQuantity"), 0)
                    lsSQL = "UPDATE " & pxeDetTable &
                           " SET nQuantity = nQuantity + " & (p_nQuantity * lnQuantity) &
                           " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                             " AND nEntryNox = " & (fnRowNo + 1) + lnDtlRow
                    p_oApp.Execute(lsSQL, pxeDetTable)

                    p_oDTDetail(fnRowNo + lnDtlRow).Item("nQuantity") -= (p_nQuantity * lnQuantity)


                    If (fnRowNo + lnDtlRow + 1) > p_oDTDetail.Rows.Count - 1 Then Exit Do
                Loop
            End If

            'Update the master combo/add-on
            lsSQL = "UPDATE " & pxeDetTable &
                    " SET nQuantity = nQuantity +" & p_nQuantity &
                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                        " AND nEntryNox = " & fnRowNo + 1

            p_oApp.Execute(lsSQL, pxeDetTable)

            p_oDTDetail(fnRowNo).Item("nQuantity") += p_nQuantity

        Catch ex As Exception
            If p_sParent = "" Then p_oApp.RollBackTransaction()
            Return False
        Finally
            If p_sParent = "" Then p_oApp.CommitTransaction()
        End Try

        'p_oDTDetail(fnRowNo).Item("nQuantity") += p_nQuantity
        'RaiseEvent DisplayRow(fnRowNo + 1)
        RaiseEvent DisplayRow(fnRowNo)

        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        Return True
    End Function

    'Private Function deductQty(ByVal fnRowNo As Integer) As Boolean
    '    Dim lsSQL As String
    '    Dim lnRow As Integer
    '    Dim loDT As DataTable

    '    'Do not allow kwik-deduction if ordered item has a promo...
    '    If p_oDTDetail(fnRowNo).Item("cWthPromo") = "1" And p_oDTDetail(fnRowNo).Item("cComboMlx") = "0" Then
    '        MsgBox("Changing the quantity of Ordered item with promo is not allowed!" & vbCrLf & _
    '               "Please reverse this order and enter the new order information...", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Sales Order")
    '        Return False
    '    End If

    '    If p_oDTDetail(fnRowNo).Item("nQuantity") = 1 Then
    '        Try
    '            If p_sParent = "" Then p_oApp.BeginTransaction()

    '            lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo
    '            loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

    '            If loDT.Rows.Count <> 0 Then
    '                lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

    '                loDT = p_oApp.ExecuteQuery(lsSQL)

    '                For lnRow = loDT.Rows.Count To 1 Step -1
    '                    lsSQL = "DELETE FROM " & pxeDetTable & _
    '                            " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                                " AND nEntryNox = " & fnRowNo + lnRow
    '                    'delete detail
    '                    p_oApp.Execute(lsSQL, pxeDetTable)
    '                    p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo + lnRow))
    '                Next
    '            End If

    '            lsSQL = "DELETE FROM " & pxeDetTable & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo

    '            'delete combo item
    '            p_oApp.Execute(lsSQL, pxeDetTable)
    '            p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo))

    '            'update entry no
    '            If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
    '                lsSQL = "UPDATE " & pxeDetTable & _
    '                        " SET nEntryNox = nEntryNox - 1" & _
    '                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                            " AND nEntryNox > " & fnRowNo

    '                p_oApp.Execute(lsSQL, pxeDetTable)
    '            End If
    '        Catch ex As Exception
    '            If p_sParent = "" Then p_oApp.RollBackTransaction()
    '            Return False
    '        Finally
    '            If p_sParent = "" Then p_oApp.CommitTransaction()
    '        End Try
    '    Else
    '        Try
    '            If p_sParent = "" Then p_oApp.BeginTransaction()

    '            lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo

    '            loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

    '            If loDT.Rows.Count <> 0 Then
    '                lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

    '                loDT = p_oApp.ExecuteQuery(lsSQL)

    '                For lnRow = 1 To loDT.Rows.Count
    '                    lsSQL = "UPDATE " & pxeDetTable & _
    '                            " SET nQuantity = nQuantity - " & p_nQuantity & _
    '                            " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                                " AND nEntryNox = " & fnRowNo + lnRow

    '                    p_oApp.Execute(lsSQL, pxeDetTable) 'update detail

    '                    p_oDTDetail(fnRowNo + lnRow).Item("nQuantity") -= p_nQuantity
    '                Next
    '            End If

    '            lsSQL = "UPDATE " & pxeDetTable & _
    '                    " SET nQuantity = nQuantity -" & p_nQuantity & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo

    '            p_oApp.Execute(lsSQL, pxeDetTable)
    '        Catch ex As Exception
    '            If p_sParent = "" Then p_oApp.RollBackTransaction()
    '            Return False
    '        Finally
    '            If p_sParent = "" Then p_oApp.CommitTransaction()
    '        End Try

    '        p_oDTDetail(fnRowNo).Item("nQuantity") -= p_nQuantity
    '    End If

    '    RaiseEvent DisplayRow(fnRowNo)

    '    Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
    '    Return True
    'End Function

    Private Function deductQty(ByVal fnRowNo As Integer) As Boolean
        Dim lsSQL As String

        'Do not allow kwik-deduction if ordered item has a promo...
        If p_oDTDetail(fnRowNo).Item("cWthPromo") = "1" Then
            MsgBox("Changing the quantity of Ordered item with promo is not allowed!" & vbCrLf &
                   "Please reverse this order and enter the new order information...", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Sales Order")
            Return False
        End If

        If p_oDTDetail(fnRowNo).Item("cComboMlx") = "1" Then
            MsgBox("Changing the quantity of Ordered combo item is not allowed!" & vbCrLf &
                   "Please reverse this order and enter the new order information...", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Sales Order")
            Return False
        End If

        If p_oDTDetail(fnRowNo).Item("nQuantity") = 1 Then
            Try
                If p_sParent = "" Then p_oApp.BeginTransaction()

                'Check if ordered item is in the middle of ordered items...
                Dim lnDtlRow As Integer = 0
                If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
                    'Check if item has detail
                    Do While p_oDTDetail(fnRowNo + lnDtlRow + 1).Item("cDetailxx") = "1"
                        'Delete all items of this master addon/promo/combo
                        lnDtlRow += 1
                        lsSQL = "DELETE FROM " & pxeDetTable &
                               " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                                 " AND nEntryNox = " & (fnRowNo + 1) + lnDtlRow
                        p_oApp.Execute(lsSQL, pxeDetTable)
                        p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo + lnDtlRow))

                        If (fnRowNo + lnDtlRow + 1) > p_oDTDetail.Rows.Count - 1 Then Exit Do
                    Loop
                End If

                'Delete the zeroed order 
                lsSQL = "DELETE FROM " & pxeDetTable &
                       " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                         " AND nEntryNox = " & fnRowNo + 1
                p_oApp.Execute(lsSQL, pxeDetTable)
                p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo))

                'Update the Value of nEntryNox in our table
                lsSQL = "UPDATE " & pxeDetTable &
                       " SET nEntryNox = nEntryNox - " & ((lnDtlRow + 1) + 1) &
                       " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                         " AND nEntryNox > " & fnRowNo
                p_oApp.Execute(lsSQL, pxeDetTable)

                'Update the Value of nEntryNox in our datatable
                Dim lnLoc As Integer
                For lnLoc = fnRowNo To p_oDTDetail.Rows.Count - 1
                    p_oDTDetail(lnLoc).Item("nEntryNox") = lnLoc
                Next
            Catch ex As Exception
                If p_sParent = "" Then p_oApp.RollBackTransaction()
                Return False
            Finally
                If p_sParent = "" Then p_oApp.CommitTransaction()
            End Try
        Else
            Try
                If p_sParent = "" Then p_oApp.BeginTransaction()

                Dim lnDtlRow As Integer = 0
                Dim lnQuantity As Integer
                'Check if item has detail
                If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
                    Do While p_oDTDetail(fnRowNo + lnDtlRow + 1).Item("cDetailxx") = "1"
                        'update all items of this master combo/add-on
                        lnDtlRow += 1
                        lnQuantity = Math.DivRem(p_oDTDetail(fnRowNo + lnDtlRow).Item("nQuantity"), p_oDTDetail(fnRowNo).Item("nQuantity"), 0)
                        lsSQL = "UPDATE " & pxeDetTable &
                               " SET nQuantity = nQuantity - " & (p_nQuantity * lnQuantity) &
                               " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                                 " AND nEntryNox = " & (fnRowNo + 1) + lnDtlRow
                        p_oApp.Execute(lsSQL, pxeDetTable)

                        p_oDTDetail(fnRowNo + lnDtlRow).Item("nQuantity") -= (p_nQuantity * lnQuantity)

                        If (fnRowNo + lnDtlRow + 1) > p_oDTDetail.Rows.Count - 1 Then Exit Do

                    Loop
                End If

                'Update the master combo/add-on
                lsSQL = "UPDATE " & pxeDetTable &
                        " SET nQuantity = nQuantity -" & p_nQuantity &
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                            " AND nEntryNox = " & fnRowNo + 1
                p_oApp.Execute(lsSQL, pxeDetTable)

                p_oDTDetail(fnRowNo).Item("nQuantity") -= p_nQuantity

            Catch ex As Exception
                If p_sParent = "" Then p_oApp.RollBackTransaction()
                Return False
            Finally
                If p_sParent = "" Then p_oApp.CommitTransaction()
            End Try
        End If

        RaiseEvent DisplayFromRow(fnRowNo)

        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)
        Return True
    End Function

    'Private Function updateQty(fnRowNo As Integer) As Boolean
    '    Dim lsSQL As String
    '    Dim lnRow As Integer
    '    Dim loDT As DataTable

    '    Try
    '        If p_sParent = "" Then p_oApp.BeginTransaction()

    '        If p_nQuantity = 0 Then
    '            lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo
    '            loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

    '            If loDT.Rows.Count <> 0 Then
    '                lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

    '                loDT = p_oApp.ExecuteQuery(lsSQL)

    '                For lnRow = loDT.Rows.Count To 1 Step -1
    '                    lsSQL = "DELETE FROM " & pxeDetTable & _
    '                            " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                                " AND nEntryNox = " & fnRowNo + lnRow

    '                    'delete detail
    '                    p_oApp.Execute(lsSQL, pxeDetTable)
    '                    p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo + lnRow))
    '                Next
    '            End If

    '            lsSQL = "DELETE FROM " & pxeDetTable & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo

    '            'delete combo item
    '            p_oApp.Execute(lsSQL, pxeDetTable)
    '            p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo))

    '            'update entry no
    '            If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
    '                lsSQL = "UPDATE " & pxeDetTable & _
    '                        " SET nEntryNox = nEntryNox - 1" & _
    '                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                            " AND nEntryNox > " & fnRowNo

    '                p_oApp.Execute(lsSQL, pxeDetTable)
    '            End If
    '        Else
    '            lsSQL = "SELECT sStockIDx FROM " & pxeDetTable & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo

    '            loDT = p_oApp.ExecuteQuery(lsSQL) 'check if it is a combo item

    '            If loDT.Rows.Count <> 0 Then
    '                lsSQL = getSQ_Combo(loDT(0)("sStockIDx"))

    '                loDT = p_oApp.ExecuteQuery(lsSQL)

    '                For lnRow = 1 To loDT.Rows.Count
    '                    lsSQL = "UPDATE " & pxeDetTable & _
    '                            " SET nQuantity = " & p_nQuantity & _
    '                            " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                                " AND nEntryNox = " & fnRowNo + lnRow

    '                    'update detail
    '                    p_oApp.Execute(lsSQL, pxeDetTable)
    '                    p_oDTDetail(fnRowNo + lnRow).Item("nQuantity") = p_nQuantity
    '                Next
    '            End If

    '            lsSQL = "UPDATE " & pxeDetTable & _
    '                    " SET nQuantity = " & p_nQuantity & _
    '                    " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
    '                        " AND nEntryNox = " & fnRowNo

    '            p_oApp.Execute(lsSQL, pxeDetTable)

    '            p_oDTDetail(fnRowNo).Item("nQuantity") = p_nQuantity
    '        End If
    '    Catch ex As Exception
    '        If p_sParent = "" Then p_oApp.RollBackTransaction()
    '        Return False
    '    Finally
    '        If p_sParent = "" Then p_oApp.CommitTransaction()
    '    End Try

    '    RaiseEvent DisplayRow(fnRowNo)
    '    Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

    '    Return True
    'End Function

    Private Function updateQty(ByVal fnRowNo As Integer) As Boolean
        Dim lsSQL As String

        Try
            If p_sParent = "" Then p_oApp.BeginTransaction()

            If p_nQuantity = 0 Then

                'Check if ordered item is in the middle of ordered items...
                Dim lnDtlRow As Integer = 0
                If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
                    'Check if item has detail
                    Do While p_oDTDetail(fnRowNo + lnDtlRow + 1).Item("cDetailxx") = "1"
                        'Delete all items of this master addon/promo/combo
                        lnDtlRow += 1
                        lsSQL = "DELETE FROM " & pxeDetTable &
                               " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                                 " AND nEntryNox = " & (fnRowNo + 1) + lnDtlRow
                        p_oApp.Execute(lsSQL, pxeDetTable)
                        p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo + lnDtlRow))

                        If (fnRowNo + lnDtlRow + 1) > p_oDTDetail.Rows.Count - 1 Then Exit Do

                    Loop
                End If

                'Delete the zeroed order 
                lsSQL = "DELETE FROM " & pxeDetTable &
                       " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                         " AND nEntryNox = " & fnRowNo + 1
                p_oApp.Execute(lsSQL, pxeDetTable)
                p_oDTDetail.Rows.Remove(p_oDTDetail.Rows(fnRowNo))

                'Update the Value of nEntryNox in our table
                lsSQL = "UPDATE " & pxeDetTable &
                       " SET nEntryNox = nEntryNox - " & ((lnDtlRow + 1) + 1) &
                       " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                         " AND nEntryNox > " & fnRowNo
                p_oApp.Execute(lsSQL, pxeDetTable)

                'Update the Value of nEntryNox in our datatable
                Dim lnLoc As Integer
                For lnLoc = fnRowNo To p_oDTDetail.Rows.Count - 1
                    p_oDTDetail(lnLoc).Item("nEntryNox") = lnLoc
                Next
            Else
                Dim lnDtlRow As Integer = 0
                Dim lnQuantity As Integer
                'Check if item has detail
                If fnRowNo < p_oDTDetail.Rows.Count - 1 Then
                    Do While p_oDTDetail(fnRowNo + lnDtlRow + 1).Item("cDetailxx") = "1"
                        'update all items of this master combo/add-on
                        lnDtlRow += 1
                        lnQuantity = Math.DivRem(p_oDTDetail(fnRowNo + lnDtlRow).Item("nQuantity"), p_oDTDetail(fnRowNo).Item("nQuantity"), 0)
                        lsSQL = "UPDATE " & pxeDetTable &
                               " SET nQuantity = " & (p_nQuantity * lnQuantity) &
                               " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                                 " AND nEntryNox = " & (fnRowNo + 1) + lnDtlRow
                        p_oApp.Execute(lsSQL, pxeDetTable)

                        p_oDTDetail(fnRowNo + lnDtlRow).Item("nQuantity") = (p_nQuantity * lnQuantity)

                        If (fnRowNo + lnDtlRow + 1) > p_oDTDetail.Rows.Count - 1 Then Exit Do

                    Loop
                End If

                'Update the master combo/add-on
                lsSQL = "UPDATE " & pxeDetTable &
                        " SET nQuantity = " & p_nQuantity &
                        " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                            " AND nEntryNox = " & fnRowNo + 1
                p_oApp.Execute(lsSQL, pxeDetTable)

                p_oDTDetail(fnRowNo).Item("nQuantity") = p_nQuantity
            End If
        Catch ex As Exception
            If p_sParent = "" Then p_oApp.RollBackTransaction()
            Return False
        Finally
            If p_sParent = "" Then p_oApp.CommitTransaction()
        End Try

        RaiseEvent DisplayFromRow(fnRowNo)
        Call computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

        Return True
    End Function

    Private Function saveMaster() As Boolean
        Dim lsSQL As String


        p_oDTMaster(0).Item("nContrlNo") = getNextControl()

        lsSQL = "INSERT INTO " & pxeMasTable &
                " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                    ", dTransact = " & dateParm(p_oDTMaster(0).Item("dTransact")) &
                    ", nContrlNo = " & p_oDTMaster(0).Item("nContrlNo") &
                    ", nTranTotl = " & p_oDTMaster(0).Item("nTranTotl") &
                    ", sCashierx = " & strParm(p_sCashierx) &
                    ", sWaiterID = " & strParm(p_oDTMaster(0).Item("sWaiterID")) &
                    ", sTableNox = " & strParm(p_oDTMaster(0).Item("sTableNox")) &
                    ", nOccupnts = " & IFNull(p_oDTMaster(0).Item("nOccupnts"), 0) &
                    ", cTranStat = " & strParm(p_oDTMaster(0).Item("cTranStat")) &
                    ", cSChargex = " & strParm(p_oDTMaster(0).Item("cSChargex")) &
                    ", sModified = " & strParm(p_oApp.UserID) &
                    ", dModified = " & datetimeParm(p_oApp.getSysDate)


        p_oApp.Execute(lsSQL, pxeMasTable)

        RaiseEvent MasterRetreived(0, p_oDTMaster(0).Item("sTransNox"))

        p_oApp.SaveEvent("0017", "Order No. " & p_oDTMaster(0).Item("nContrlNo") &
                                "/Table No. " & IFNull(p_oDTMaster(0).Item("sTableNox"), ""), p_sSerial)

        lsSQL = "UPDATE Cash_Reg_Machine SET" &
                            "  sTransNox = " & strParm(p_oDTMaster(0)("sTransNox")) &
                        " WHERE sIDNumber = " & strParm(p_sPOSNo)

        Try
            p_oApp.Execute(lsSQL, "Cash_Reg_Machine")
        Catch ex As Exception
            MsgBox(ex.Message)
            Throw ex
        End Try

        Return True
    End Function

    Private Function updateCashRegMachine(ByRef fsTransNox As String,
                                          ByVal fbNeoTrans As Boolean) As Boolean
        Dim lsSQL As String

        If fbNeoTrans Then fsTransNox = getNextMasterNo()

        lsSQL = "UPDATE Cash_Reg_Machine SET" &
                    " sMasterNo = " & strParm(fsTransNox) &
                " WHERE sIDNumber = " & strParm(p_sPOSNo)

        p_oApp.Execute(lsSQL, "Cash_Reg_Machine")
        Return True
    End Function

    'Private Function computeTotal() As Boolean
    '    Dim lnRow As Integer
    '    Dim lnTotal As Double = 0.0
    '    Dim lnSlPrc As Double = 0.0
    '    Dim lnDiscntbl As Double = 0.0
    '    Dim lnZeroRatd As Double = 0.0

    '    For lnRow = 0 To p_oDTDetail.Rows.Count - 1
    '        If p_oDTDetail(lnRow).Item("cComplmnt") = "0" Then
    '            If p_oDtaDiscx Is Nothing Then
    '                'Create Total Price per detail
    '                lnSlPrc = (p_oDTDetail(lnRow).Item("nUnitPrce") * _
    '                            (100 - p_oDTDetail(lnRow).Item("nDiscount")) / 100 -
    '                            p_oDTDetail(lnRow).Item("nAddDiscx")) * _
    '                        IIf(p_oDTDetail(lnRow).Item("cReversex") = "+", 1, -1) * _
    '                        p_oDTDetail(lnRow).Item("nQuantity")

    '                lnTotal += lnSlPrc

    '                'Get Total Price that doesn't have a discount since we only apply card discounts for item without discount
    '                If p_oDTDetail(lnRow).Item("nDiscount") = 0 And p_oDTDetail(lnRow).Item("nAddDiscx") = 0 Then
    '                    lnDiscntbl += lnSlPrc
    '                End If
    '            Else
    '                If p_oDtaDiscx(0).Item("sCategrID") = "" Then
    '                    lnSlPrc = p_oDTDetail(lnRow).Item("nUnitPrce") * _
    '                              IIf(p_oDTDetail(lnRow).Item("cReversex") = "+", 1, -1) * _
    '                              p_oDTDetail(lnRow).Item("nQuantity")

    '                    lnTotal += lnSlPrc

    '                    lnDiscntbl += lnSlPrc
    '                Else
    '                    Dim loDta() As DataRow
    '                    loDta = p_oDtaDiscx.Select("sCategrID = " & strParm(p_oDtaDiscx(0).Item("sCategrID")))

    '                    If loDta.Count > 0 Then

    '                    End If
    '                End If
    '            End If

    '            'kalyptus - 2016.11.28 11:33am
    '            'TODO: Adding of tag whether an ITEM is Vatable or NOT...
    '            'If p_oDTDetail(lnRow).Item("cNoneVATx") = "1" Then
    '            '    lnZeroRatd += lnSlPrc
    '            'End If
    '        End If

    '    Next
    '    p_oDTMaster(0).Item("nTranTotl") = lnTotal
    '    p_oDTMaster(0).Item("nDiscntbl") = lnDiscntbl
    '    p_oDTMaster(0).Item("nZeroRatd") = lnZeroRatd

    '    'VAT is 12 % of sales
    '    'TODO: load VAT percent of sales from CONFIG
    '    Dim lnVatPerc As Double = 1.12

    '    'kalyptus - 2016.11.29 09:20am
    '    'RULE: Each transaction uses one discount card...
    '    If Not p_oDiscount Is Nothing Then
    '        If p_oDiscount.HasDiscount Then

    '            Dim lnAmtx As Decimal = p_oDTMaster(0).Item("nDiscntbl") / p_oDiscount.Master("nNoClient") * p_oDiscount.Master("nWithDisc")
    '            Dim lnDisc As Decimal

    '            If p_oDiscount.Master("cNoneVatx") = "1" Then
    '                Dim lnNonVat As Decimal
    '                lnNonVat = Math.Round(lnAmtx / lnVatPerc, 2)
    '                p_oDTMaster(0).Item("nNonVATxx") = lnNonVat
    '                p_oDTMaster(0).Item("nDiscAmtN") = lnAmtx - lnNonVat

    '                lnDisc = (p_oDiscount.Master("nDiscRate") / 100) * lnNonVat
    '                lnDisc += p_oDiscount.Master("nAddDiscx")

    '                p_oDTMaster(0).Item("nDiscAmtN") = p_oDTMaster(0).Item("nDiscAmtN") + lnDisc
    '            Else
    '                lnDisc = (p_oDiscount.Master("nDiscRate") / 100) * lnAmtx
    '                lnDisc += p_oDiscount.Master("nAddDiscx")
    '                p_oDTMaster(0).Item("nDiscAmtV") = lnDisc
    '            End If
    '        End If
    '    End If

    '    p_oDTMaster(0).Item("nVATSales") = Math.Round((lnTotal - (lnZeroRatd + p_oDTMaster(0).Item("nNonVATxx") + p_oDTMaster(0).Item("nDiscAmtV") + p_oDTMaster(0).Item("nDiscAmtN"))) / lnVatPerc)
    '    p_oDTMaster(0).Item("nVATAmtxx") = (lnTotal - (lnZeroRatd + p_oDTMaster(0).Item("nNonVATxx") + p_oDTMaster(0).Item("nDiscAmtV") + p_oDTMaster(0).Item("nDiscAmtN"))) - p_oDTMaster(0).Item("nVATSales")

    '    Return True
    'End Function

    Private Function createMaster() As Boolean
        p_oDTMaster = New DataTable("Master")
        p_oDTMaster.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTMaster.Columns.Add("sMasterNo", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTMaster.Columns.Add("sOrderNox", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTMaster.Columns.Add("sBillNmbr", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTMaster.Columns.Add("dTransact", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("nContrlNo", System.Type.GetType("System.Int32"))
        p_oDTMaster.Columns.Add("sIDNumber", System.Type.GetType("System.Int32"))
        p_oDTMaster.Columns.Add("nTranTotl", System.Type.GetType("System.Decimal"))
        p_oDTMaster.Columns.Add("sCashrNme", System.Type.GetType("System.String")).MaxLength = 64
        p_oDTMaster.Columns.Add("sWaiterNm", System.Type.GetType("System.String")).MaxLength = 64
        p_oDTMaster.Columns.Add("sCashierx", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("sWaiterID", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTMaster.Columns.Add("sReceiptx", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("sMergeIDx", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("sTableNox", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTMaster.Columns.Add("nOccupnts", System.Type.GetType("System.Int32"))
        p_oDTMaster.Columns.Add("cTranStat", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTMaster.Columns.Add("nPrntBill", System.Type.GetType("System.Int32"))
        p_oDTMaster.Columns.Add("dPrntBill", System.Type.GetType("System.DateTime"))

        p_oDTMaster.Columns.Add("nVATSales", System.Type.GetType("System.Decimal")) 'VATable Sales
        p_oDTMaster.Columns.Add("nVATAmtxx", System.Type.GetType("System.Decimal")) 'VAT Amount
        p_oDTMaster.Columns.Add("nNonVATxx", System.Type.GetType("System.Decimal")) 'Non-VAT Sale
        p_oDTMaster.Columns.Add("nZeroRatd", System.Type.GetType("System.Decimal")) 'Zero Rated Sales
        p_oDTMaster.Columns.Add("nVoidTotl", System.Type.GetType("System.Decimal")) 'Zero Rated Sales
        p_oDTMaster.Columns.Add("nDiscount", System.Type.GetType("System.Decimal")) 'Regular Discount
        p_oDTMaster.Columns.Add("nVatDiscx", System.Type.GetType("System.Decimal")) '12%VAT Discount
        p_oDTMaster.Columns.Add("nPWDDiscx", System.Type.GetType("System.Decimal")) 'Senior/PWD Discount
        p_oDTMaster.Columns.Add("nDiscntbl", System.Type.GetType("System.Decimal")) 'Discountable Amount
        p_oDTMaster.Columns.Add("nSChargex", System.Type.GetType("System.Decimal")) 'Service Charge
        p_oDTMaster.Columns.Add("cSChargex", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTMaster.Columns.Add("cTranType", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTMaster.Columns.Add("dModified", System.Type.GetType("System.DateTime"))

        'p_oDTMaster.Columns.Add("nDiscAmtV", System.Type.GetType("System.Decimal"))
        'p_oDTMaster.Columns.Add("nDiscAmtN", System.Type.GetType("System.Decimal"))

        Return True
    End Function

    Private Function initMaster() As Boolean
        p_oDTMaster.Rows.Add()
        p_oDTMaster(0).Item("sTransNox") = ""
        p_oDTMaster(0).Item("sMasterNo") = ""
        p_oDTMaster(0).Item("sOrderNox") = ""
        p_oDTMaster(0).Item("sBillNmbr") = ""
        p_oDTMaster(0).Item("dTransact") = p_oApp.getSysDate
        p_oDTMaster(0).Item("nContrlNo") = 0
        p_oDTMaster(0).Item("sIDNumber") = 0
        p_oDTMaster(0).Item("nTranTotl") = 0.0
        p_oDTMaster(0).Item("sCashrNme") = p_oApp.UserName
        p_oDTMaster(0).Item("sWaiterNm") = ""
        p_oDTMaster(0).Item("sCashierx") = p_sCashierx
        p_oDTMaster(0).Item("sWaiterID") = ""
        p_oDTMaster(0).Item("sReceiptx") = ""
        p_oDTMaster(0).Item("sMergeIDx") = ""
        p_oDTMaster(0).Item("sTableNox") = ""
        p_oDTMaster(0).Item("cSChargex") = "x"
        p_oDTMaster(0).Item("cTranType") = ""
        p_oDTMaster(0).Item("cTranStat") = "0"
        p_oDTMaster(0).Item("nPrntBill") = 0

        p_oDTMaster(0).Item("nVATSales") = 0.0
        p_oDTMaster(0).Item("nVATAmtxx") = 0.0
        p_oDTMaster(0).Item("nNonVATxx") = 0.0
        p_oDTMaster(0).Item("nZeroRatd") = 0.0
        p_oDTMaster(0).Item("nDiscntbl") = 0.0

        p_oDTMaster(0).Item("nVoidTotl") = 0.0
        p_oDTMaster(0).Item("nDiscount") = 0.0
        p_oDTMaster(0).Item("nVatDiscx") = 0.0
        p_oDTMaster(0).Item("nPWDDiscx") = 0.0

        Return True
    End Function

    Private Function createDetail() As Boolean
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nEntryNox", System.Type.GetType("System.Int16")).AutoIncrement = True
        p_oDTDetail.Columns.Add("sBarcodex", System.Type.GetType("System.String")).MaxLength = 17
        p_oDTDetail.Columns.Add("sDescript", System.Type.GetType("System.String")).MaxLength = 64
        p_oDTDetail.Columns.Add("sBriefDsc", System.Type.GetType("System.String")).MaxLength = 16
        p_oDTDetail.Columns.Add("nUnitPrce", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cReversex", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("nQuantity", System.Type.GetType("System.Int32"))
        p_oDTDetail.Columns.Add("nDiscount", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nAddDiscx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("cPrintedx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("nComplmnt", System.Type.GetType("System.Int32"))
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
        p_oDTDetail.Columns.Add("cServedxx", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTDetail.Columns.Add("sReplItem", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTDetail.Columns.Add("dModified", System.Type.GetType("System.DateTime"))
        p_oDTDetail.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTDetail.Columns.Add("nAmountxx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("sPrntPath", System.Type.GetType("System.String")).MaxLength = 128
        Return True
    End Function

    Private Function newDetail(Optional ByVal fnRowPos As Integer = -1) As Boolean
        If fnRowPos = -1 Then
            p_oDTDetail.Rows.Add(p_oDTDetail.NewRow)
            p_nRow = p_oDTDetail.Rows.Count - 1
        Else
            'Insert the new Item after the 
            If fnRowPos = p_oDTDetail.Rows.Count - 1 Then
                p_oDTDetail.Rows.Add(p_oDTDetail.NewRow)
                p_nRow = p_oDTDetail.Rows.Count - 1
            Else
                p_oDTDetail.Rows.InsertAt(p_oDTDetail.NewRow, fnRowPos - 1)
                p_nRow = fnRowPos - 1
            End If
        End If

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
        p_oDTDetail(p_nRow).Item("nComplmnt") = 0
        p_oDTDetail(p_nRow).Item("cDetSaved") = 0
        p_oDTDetail(p_nRow).Item("cReversed") = "0"
        p_oDTDetail(p_nRow).Item("cServedxx") = "0"
        p_oDTDetail(p_nRow).Item("sReplItem") = ""

        p_oDTDetail(p_nRow).Item("nEntryNox") = p_nRow
        p_oDTDetail(p_nRow).Item("dModified") = p_oApp.SysDate

        'kalyptus - 2017.01.07 05:03pm
        'If this add-on is inserted at the middle of an order then update the nEntryNox of the other items
        'please see procAddOn()
        If fnRowPos < p_oDTDetail.Rows.Count - 1 Then
            Dim lnLoc As Integer
            For lnLoc = fnRowPos + 1 To p_oDTDetail.Rows.Count - 1
                p_oDTDetail(lnLoc).Item("nEntryNox") = lnLoc
            Next
        End If

        Return True
    End Function

    Private Function newDetail(ByVal foDT As DataTable,
                               ByRef fnQty As Integer) As Boolean
        Dim lbAddNewRow As Boolean = True

        For nCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
            If p_oDTDetail(nCtr)("sBarcodex") = foDT.Rows(0)("sBarcodex") Then

                If p_oDTDetail(nCtr)("cPrintedx") = xeLogical.NO And p_oDTDetail(nCtr)("cReversed") = xeLogical.NO And p_oDTDetail(nCtr)("cWthPromo") = xeLogical.NO And p_oDTDetail(nCtr)("cDetailxx") = xeLogical.NO Then
                    If p_oDTDetail(nCtr).Item("cComboMlx") <> "1" Then
                        fnQty = p_oDTDetail(nCtr)("nQuantity")
                        lbAddNewRow = False
                        p_nRow = nCtr
                        Exit For
                    End If
                End If
            End If
        Next nCtr

        If lbAddNewRow Then
            fnQty = 0
            Call newDetail()
        End If

        p_oDTDetail(p_nRow).Item("sBarcodex") = foDT(0).Item("sBarcodex")
        p_oDTDetail(p_nRow).Item("sDescript") = foDT(0).Item("sDescript")
        p_oDTDetail(p_nRow).Item("sBriefDsc") = foDT(0).Item("sBriefDsc")
        p_oDTDetail(p_nRow).Item("nUnitPrce") = foDT(0).Item("nSelPrice")
        p_oDTDetail(p_nRow).Item("sStockIDx") = foDT(0).Item("sStockIDx")
        p_oDTDetail(p_nRow).Item("nDiscLev1") = foDT(0).Item("nDiscLev1")
        p_oDTDetail(p_nRow).Item("nDiscLev2") = foDT(0).Item("nDiscLev2")
        p_oDTDetail(p_nRow).Item("nDiscLev3") = foDT(0).Item("nDiscLev3")
        p_oDTDetail(p_nRow).Item("nDiscAmtx") = foDT(0).Item("nDiscAmtx")
        p_oDTDetail(p_nRow).Item("nDealrDsc") = foDT(0).Item("nDealrDsc")
        p_oDTDetail(p_nRow).Item("cComboMlx") = foDT(0).Item("cComboMlx")
        p_oDTDetail(p_nRow).Item("cWthPromo") = foDT(0).Item("cWthPromo")
        p_oDTDetail(p_nRow).Item("sCategrID") = foDT(0).Item("sCategrID")

        p_oDTDetail(p_nRow).Item("sPrntPath") = foDT(0).Item("sPrntPath")
        Debug.Print(IFNull(p_oDTDetail(p_nRow).Item("sPrntPath"),))
        p_oDTDetail(p_nRow).Item("dModified") = p_oApp.SysDate

        Return True
    End Function

    Private Sub clearDetail(ByVal fnRow As Integer)
        p_oDTDetail(fnRow).Item("sBarcodex") = ""
        p_oDTDetail(fnRow).Item("sDescript") = ""
        p_oDTDetail(fnRow).Item("sBriefDsc") = ""
        p_oDTDetail(fnRow).Item("nUnitPrce") = 0.0
        p_oDTDetail(fnRow).Item("sStockIDx") = ""
        p_oDTDetail(fnRow).Item("nDiscLev1") = 0
        p_oDTDetail(fnRow).Item("nDiscLev2") = 0
        p_oDTDetail(fnRow).Item("nDiscLev3") = 0
        p_oDTDetail(fnRow).Item("nDiscAmtx") = 0.0
        p_oDTDetail(fnRow).Item("nDealrDsc") = 0.0
        p_oDTDetail(fnRow).Item("cComboMlx") = 0
        p_oDTDetail(fnRow).Item("cWthPromo") = 0
        p_oDTDetail(fnRow).Item("sCategrID") = ""
        p_oDTDetail(fnRow).Item("sPrntPath") = ""
        p_oDTDetail(fnRow).Item("dModified") = p_oApp.SysDate
    End Sub

    Private Function procAddOn(ByVal fnRow As Integer) As Boolean
        'If p_oDTDetail(fnRow).Item("cPrintedx") = "1" Then
        '    MsgBox("This order was already printed. Please reverse this order and enter again!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Sales Order")
        '    Exit Sub
        'End If

        Dim loAddOn As clsPromoAddOn
        Dim lbAdded As Boolean = False
        loAddOn = New clsPromoAddOn(p_oApp)

        If loAddOn.searchAddOn(p_oDTDetail(fnRow).Item("sStockIDx"), p_oDTMaster(0).Item("dTransact")) Then
            Dim lnRow As Integer

            For lnRow = 0 To loAddOn.ItemCount - 1
                If loAddOn.Detail(lnRow, "cSelected") = "1" Then
                    If Not lbAdded Then lbAdded = True
                    'If loAddOn.Detail(lnRow, "sReplItem") = "" Then

                    'Get the last Detail of the combo Item...
                    Dim lnLoc As Integer
                    For lnLoc = fnRow + 1 To p_oDTDetail.Rows.Count - 1
                        If p_oDTDetail(lnLoc).Item("cDetailxx") = "0" Then Exit For
                    Next

                    'Add a detail after the location of end of the details for this combo item
                    newDetail(lnLoc + 1)
                    p_oDTDetail(lnLoc).Item("sStockIDx") = loAddOn.Detail(lnRow, "sStockIDx")
                    p_oDTDetail(lnLoc).Item("sBarCodex") = loAddOn.Detail(lnRow, "sBarCodex")
                    p_oDTDetail(lnLoc).Item("sBriefDsc") = loAddOn.Detail(lnRow, "sBriefDsc")
                    p_oDTDetail(lnLoc).Item("sDescript") = loAddOn.Detail(lnRow, "sBriefDsc")
                    p_oDTDetail(lnLoc).Item("sCategrID") = ""

                    'p_oDTDetail(lnLoc).Item("nQuantity") = loAddOn.Detail(lnRow, "nQuantity") * p_oDTDetail(lnLoc - 1).Item("nQuantity")
                    p_oDTDetail(lnLoc).Item("nQuantity") = loAddOn.Detail(lnRow, "nQuantity")
                    p_oDTDetail(lnLoc).Item("nUnitPrce") = loAddOn.Detail(lnRow, "nUnitPrce")
                    p_oDTDetail(lnLoc).Item("cDetailxx") = "1"

                    If p_oDTDetail(fnRow).Item("cDetSaved") = xeLogical.YES Then RaiseEvent DisplayFromRow(fnRow)

                    'kalyptus - 2017.01.07 05:03pm
                    'If this add-on is inserted at the middle of an order then update the nEntryNox of the other save items
                    'please see newDetail(fnRows)
                    If lnLoc < p_oDTDetail.Rows.Count - 1 Then
                        Dim lsSQL As String
                        lsSQL = "UPDATE " & pxeDetTable &
                                " SET nEntryNox = nEntryNox + 1" &
                                " WHERE sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) &
                                    " AND nEntryNox > " & lnLoc + 1
                        Try
                            p_oApp.Execute(lsSQL, pxeDetTable)
                        Catch ex As Exception
                            MsgBox(ex.Message)
                            Throw ex
                        End Try
                    End If
                    'Else
                    '    'Search the item to be replace 
                    '    Dim lnLoc As Integer
                    '    For lnLoc = fnRow + 1 To p_oDTDetail.Rows.Count - 1
                    '        'If searching extends the boundary of the combo then do not add the add/on
                    '        If p_oDTDetail(lnLoc).Item("cDetailxx") = "0" Then Exit For

                    '        If p_oDTDetail(lnLoc).Item("sStockIDx") = loAddOn.Detail(lnRow, "sReplItem") Then
                    '            p_oDTDetail(lnLoc).Item("sStockIDx") = loAddOn.Detail(lnRow, "sStockIDx")

                    '            p_oDTDetail(lnLoc).Item("sBarCodex") = loAddOn.Detail(lnRow, "sBarCodex")
                    '            p_oDTDetail(lnLoc).Item("sBriefDsc") = loAddOn.Detail(lnRow, "sBriefDsc")
                    '            p_oDTDetail(lnLoc).Item("sCategrID") = ""

                    '            p_oDTDetail(lnLoc).Item("nQuantity") = loAddOn.Detail(lnRow, "nQuantity") * p_oDTDetail(lnLoc).Item("nQuantity")
                    '            p_oDTDetail(lnLoc).Item("nUnitPrce") = loAddOn.Detail(lnRow, "nUnitPrce")
                    '            p_oDTDetail(lnLoc).Item("sReplItem") = loAddOn.Detail(lnRow, "sReplItem")

                    '            'Note: We do not need to reset the value of this combo detail as detail since it was already set
                    '            If p_oDTDetail(lnLoc).Item("cDetSaved") = xeLogical.YES Then RaiseEvent DisplayRow(lnLoc)
                    '        End If
                    '    Next
                    'End If
                End If
            Next

            If Not lbAdded Then Return False
        Else
            Return False
        End If

        Return True
    End Function

    Private Function procDisc(ByVal fnRow As Integer) As Boolean
        Dim loDta As DataTable

        Dim loDisc As clsPromoDiscount
        loDisc = New clsPromoDiscount(p_oApp)

        'We used dModified since discount is based on the time of order for the promo...
        loDta = loDisc.searchDiscount(p_oDTDetail(fnRow).Item("sCategrID"), p_oDTDetail(fnRow).Item("sStockIDx"), p_oDTDetail(fnRow).Item("dModified"), p_oDTDetail(fnRow).Item("nQuantity"))


        If loDta Is Nothing Then Return False
        If loDta.Rows.Count = 0 Then Return False

        Dim lnOldQty As Integer = p_oDTDetail(fnRow).Item("nQuantity")
        p_oDTDetail(fnRow).Item("nQuantity") = loDta(0).Item("nQuantity")
        p_oDTDetail(fnRow).Item("nDiscount") = loDta(0).Item("nDDiscRte")
        p_oDTDetail(fnRow).Item("nAddDiscx") = loDta(0).Item("nDDiscAmt")
        p_oDTDetail(fnRow).Item("nDiscLev1") = 0 ' remove any discount coz promo items overrides all regulary discounting
        p_oDTDetail(fnRow).Item("nDiscLev2") = 0
        p_oDTDetail(fnRow).Item("nDiscLev3") = 0
        p_oDTDetail(fnRow).Item("nDiscAmtx") = 0
        p_oDTDetail(fnRow).Item("nDealrDsc") = 0
        p_oDTDetail(fnRow).Item("dModified") = p_oApp.SysDate

        Dim lnDtlRow As Integer = 0
        If p_oDTDetail(fnRow).Item("cComboMlx") = xeLogical.YES Then
            Dim lnQuantity As Integer
            Dim lnRem As Integer
            Do While p_oDTDetail(fnRow + lnDtlRow + 1).Item("cDetailxx") = "1"

                'update all items of this master combo/add-on
                lnDtlRow += 1
                lnQuantity = Math.DivRem(p_oDTDetail(fnRow + lnDtlRow).Item("nQuantity"), lnOldQty, lnRem)

                p_oDTDetail(fnRow + lnDtlRow).Item("nQuantity") = (loDta(0).Item("nQuantity") * lnQuantity)

                If (fnRow + lnDtlRow + 1) > p_oDTDetail.Rows.Count - 1 Then Exit Do
            Loop
        Else
            'lnDtlRow += 1
        End If

        If loDta(0).Item("nPromBght") > 0 Then
            newDetail(fnRow + lnDtlRow + 1)
            p_oDTDetail(p_nRow).Item("sStockIDx") = loDta(0).Item("sStockIDx")
            p_oDTDetail(p_nRow).Item("sCategrID") = loDta(0).Item("sCategrCd")
            p_oDTDetail(p_nRow).Item("sBarcodex") = loDta(0).Item("sBarcodex")
            p_oDTDetail(p_nRow).Item("sDescript") = loDta(0).Item("sDescript")
            p_oDTDetail(p_nRow).Item("sBriefDsc") = loDta(0).Item("sBriefDsc")
            p_oDTDetail(p_nRow).Item("nUnitPrce") = loDta(0).Item("nUnitPrce")
            p_oDTDetail(p_nRow).Item("cReversex") = "+"
            p_oDTDetail(p_nRow).Item("nQuantity") = loDta(0).Item("nPromBght")
            p_oDTDetail(p_nRow).Item("nDiscount") = loDta(0).Item("nDiscRate")
            p_oDTDetail(p_nRow).Item("nAddDiscx") = loDta(0).Item("nDiscAmtx")
            p_oDTDetail(p_nRow).Item("cDetailxx") = "1"
            p_oDTDetail(p_nRow).Item("dModified") = p_oApp.SysDate

            If loDta(0).Item("cComboMlx") = xeLogical.YES Then
                p_nQuantity = loDta(0).Item("nPromBght")
                Call procCombo()
            End If
        End If

        RaiseEvent DisplayRow(p_nRow)
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
                If MsgBox("Item is on Promo." & vbCrLf &
                        "However the minimum quantity of " & loDT(0).Item("nMinQtyxx") & " is not reach!" & vbCrLf &
                        "Discount is " & Format(loDT(0).Item("nDiscRate"), "#0.00") & "% and/or " & Format(loDT(0).Item("nDiscAmtx"), "#,##0.00") & "PHP" & vbCrLf &
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

                Call newDetail(loDT2, 0)

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
            p_oDTDetail(p_nRow).Item("nQuantity") = p_nQuantity * loDT(lnRow).Item("nQuantity")
            p_oDTDetail(p_nRow).Item("dModified") = Now
            p_oDTDetail(p_nRow).Item("cDetailxx") = 1

            RaiseEvent DisplayRow(p_nRow)
        Next

        Return True
    End Function

    Public Function GetTables() As DataTable
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = "SELECT nTableNox" &
                    ", cStatusxx" &
                    ", dReserved" &
                    ", nCapacity" &
                    ", nOccupnts" &
                " FROM Table_Master"

        loDT = p_oApp.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then MsgBox("No tables on record!", MsgBoxStyle.Critical, "Warning")

        Return loDT
    End Function

    Public Function PayCharge() As Boolean
        Dim loFormCharge As frmChargeInvoiceCollection
        loFormCharge = New frmChargeInvoiceCollection

        With loFormCharge
            .AppDriver = p_oApp
            .TopMost = True
            .ShowDialog()

            If Not .Cancelled Then
                MsgBox("Charge Invoices has been paid successfully", MsgBoxStyle.Information, "Notice")
            End If
        End With

        Return True
    End Function

    Private Function checkIfSplitted(ByVal fsReferNox As String) As Boolean
        Dim lsSQL As String
        Dim loDTa As DataTable

        lsSQL = "SELECT * " &
            " FROM Order_Split" &
            " WHERE sReferNox = " & strParm(fsReferNox)
        loDTa = p_oApp.ExecuteQuery(lsSQL)

        If loDTa.Rows.Count = 0 Then
            checkIfSplitted = False
        Else
            checkIfSplitted = True
        End If

        Return checkIfSplitted
    End Function

    Public Function IssueDiscount() As Boolean
        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        Dim lsSourceNo As String = ""
        Dim lsSourceCd As String = ""
        Dim lsSplitType As String = ""
        Dim lbCancelled As Boolean = False
        Dim lsCategrIDx As String = ""

        p_oDiscount = New Discount(p_oApp)
        p_oDiscount.POSNumbr = p_sTermnl
        p_oDiscount.SerialNo = p_sSerial

        For lnCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
            If IFNull(p_oDTDetail.Rows(lnCtr)("sCategrID"), "") <> "" Then
                lsCategrIDx = lsCategrIDx & "'" & p_oDTDetail.Rows(lnCtr)("sCategrID") & "',"
            End If
        Next


        If SplitOrder(lsSourceNo, lsSourceCd, lbCancelled, lsSplitType) Then
            If lbCancelled Then Return False

            p_oDiscount.SourceCd = lsSourceCd
            p_oDiscount.SourceNo = lsSourceNo
        Else
            p_oDiscount.SourceCd = pxeSourceCde
            p_oDiscount.SourceNo = p_oDTMaster(0)("sTransNox")
        End If

        p_oDiscount.InitTransaction()
        p_oDiscount.Category = lsCategrIDx.Substring(0, lsCategrIDx.Length - 1)
        p_oDiscount.setTranTotal = p_nTotalSales
        p_oDiscount.ShowDiscount()

        'Recompute total
        If p_oDiscount.ItemCategoryCount > 0 Then
            Dim lbDiscounted As Boolean = False
            For lnCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
                If lbDiscounted = True Then
                    Exit For
                End If
                p_oDTDetail.Rows(lnCtr)("nDiscount") = 0
                p_oDTDetail.Rows(lnCtr)("nAddDiscx") = 0
                If p_oDTDetail.Rows(lnCtr)("sCategrID") <> "" Then
                    For lnRow As Integer = 0 To p_oDiscount.ItemCategoryCount - 1
                        If p_oDTDetail.Rows(lnCtr)("sCategrID") = p_oDiscount.Category(lnRow, "sCategrID") Then
                            If IFNull(p_oDiscount.Category(lnRow, "nMinAmtxx"), 0) = 0.0 Then
                                p_oDTDetail.Rows(lnCtr)("nDiscount") = p_oDiscount.Category(lnRow, "nDiscRate")
                                p_oDTDetail.Rows(lnCtr)("nAddDiscx") = p_oDiscount.Category(lnRow, "nDiscAmtx")
                                lbDiscounted = True
                                Exit For
                            Else
                                If p_oDTDetail.Rows(lnCtr)("nUnitPrce") > p_oDiscount.Category(lnRow, "nMinAmtxx") Then
                                    p_oDTDetail.Rows(lnCtr)("nDiscount") = p_oDiscount.Category(lnRow, "nDiscRate")
                                    p_oDTDetail.Rows(lnCtr)("nAddDiscx") = p_oDiscount.Category(lnRow, "nDiscAmtx")
                                    lbDiscounted = True
                                    Exit For
                                End If
                            End If
                        End If
                    Next lnRow
                End If
                Call saveDetail(lnCtr)
            Next lnCtr
        Else

            Dim lbDiscounted As Boolean = False
            For lnCtr As Integer = 0 To p_oDTDetail.Rows.Count - 1
                If lbDiscounted = True Then
                    Exit For
                End If
                p_oDTDetail.Rows(lnCtr)("nDiscount") = 0
                p_oDTDetail.Rows(lnCtr)("nAddDiscx") = 0
                If p_oDiscount.ItemCategoryCount > 0 Then
                    If IFNull(p_oDiscount.Category(0, "nMinAmtxx"), 0) = 0.0 Then
                        p_oDTDetail.Rows(lnCtr)("nDiscount") = p_oDiscount.Category(0, "nDiscRate")
                        p_oDTDetail.Rows(lnCtr)("nAddDiscx") = p_oDiscount.Category(0, "nDiscAmtx")

                        lbDiscounted = True
                        Exit For
                    Else
                        If p_oDTDetail.Rows(lnCtr)("nUnitPrce") > p_oDiscount.Category(0, "nMinAmtxx") Then
                            p_oDTDetail.Rows(lnCtr)("nDiscount") = p_oDiscount.Category(0, "nDiscRate")
                            p_oDTDetail.Rows(lnCtr)("nAddDiscx") = p_oDiscount.Category(0, "nDiscAmtx")


                            lbDiscounted = True
                            Exit For
                        End If
                    End If
                End If
                Call saveDetail(lnCtr)
            Next
        End If
        'computeTotal(p_oDTMaster, p_oDTDetail, p_oDtaDiscx, p_oDiscount)

        Return True
    End Function

    Public Function LoadDiscount(ByVal fsSourceCD As String, ByVal fsSourceNo As String) As DataTable
        p_oDiscount = New Discount(p_oApp)
        p_oDiscount.POSNumbr = p_sTermnl
        p_oDiscount.SourceNo = fsSourceNo
        p_oDiscount.SourceCd = fsSourceCD
        p_oDiscount.InitTransaction()
        p_oDiscount.OpenTransaction()

        Dim loDiscDtl As DataTable
        loDiscDtl = Nothing

        If p_oDiscount.HasDiscount Then
            Dim lsSQL As String
            lsSQL = "SELECT" &
                        "  a.nNoClient" &
                        ", a.nWithDisc" &
                        ", b.sIDNumber" &
                        ", b.sClientNm" &
                        ", d.sCategrID" &
                        ", d.nMinAmtxx" &
                        ", d.nDiscRate" &
                        ", d.nDiscAmtx" &
                        ", d.sCardIDxx" &
                        ", c.cNoneVatx" &
                        ", e.sTransNox" &
                        ", e.sTableNox" &
                              " FROM Discount a," &
                                    " Discount_Detail b," &
                                    " Discount_Card c," &
                                    " Discount_Card_Detail d," &
                                    " SO_Master e" &
                                       " WHERE a.sTransNox = b.sTransNox" &
                                            " AND a.sDiscCard = c.sCardIDxx" &
                                            " AND c.sCardIDxx = d.sCardIDxx" &
                                            " AND a.sSourceNo = e.sTransNox" &
                                            " AND d.sCardIDxx = " & strParm(p_oDiscount.Master("sCardIDxx")) &
                                            " AND a.sSourceNo = " & strParm(fsSourceNo) &
                                            " ORDER BY b.nEntryNox ASC" &
                                            " LIMIT 1"
            Try
                loDiscDtl = p_oApp.ExecuteQuery(lsSQL)

                p_nNoClient = 0
                p_nWithDisc = 0
                p_nTableNo = 0
                'p_sTrantype = ""

                If loDiscDtl.Rows.Count = 0 Then
                    lsSQL = "SELECT" &
                                "  a.sCategrID" &
                                ", a.nMinAmtxx" &
                                ", a.nDiscRate" &
                                ", a.nDiscAmtx" &
                                ", a.sCardIDxx" &
                                ", b.cNoneVatx" &
                                ", d.sIDNumber" &
                            " FROM Discount_Card_Detail a" &
                                ", Discount_Card b" &
                                    " LEFT JOIN Discount c" &
                                        " LEFT JOIN Discount_Detail d" &
                                            " ON c.sTransNox = d.sTransNox" &
                                        " ON b.sCardIDxx = c.sDiscCard" &
                                        " AND c.sSourceCd = 'SO'" &
                                        " AND c.sSourceNo = " & strParm(fsSourceNo) &
                            " WHERE a.sCardIDxx = b.sCardIDxx" &
                                " AND a.sCardIDxx = " & strParm(p_oDiscount.Master("sCardIDxx"))

                    loDiscDtl = p_oApp.ExecuteQuery(lsSQL)
                    If loDiscDtl.Rows.Count = 0 Then loDiscDtl = Nothing
                Else
                    p_nNoClient = loDiscDtl(0).Item("nNoClient")
                    p_nWithDisc = loDiscDtl(0).Item("nWithDisc")
                    p_nTableNo = IIf(loDiscDtl(0).Item("sTableNox") = "", 0, loDiscDtl(0).Item("sTableNox"))
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                loDiscDtl = Nothing
            End Try
        Else
            loDiscDtl = Nothing
        End If

        Return loDiscDtl
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
        lsSQL = "SELECT" &
                       "  a.sClientID" &
                       ", a.sCompnyNm" &
                       ", CONCAT(IF(IFNull(a.sHouseNox, '') = '', '', CONCAT(a.sHouseNox, ' ')), a.sAddressx, ', ', b.sTownName, ', ', c.sProvName, ' ', b.sZippCode) xAddressx " &
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
                                             , "sClientID»sCompnyNm»xAddressx" _
                                             , "ID»Client Name»Address",
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

        lsSQL = "SELECT sTransNox" &
                " FROM Cash_Reg_Machine" &
                " WHERE sIDNumber = " & strParm(p_sPOSNo)
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

            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            If loDT.Rows(0).Item("sTransNox") = "" Then
                loDA.FillSchema(loDS, SchemaType.Source)
                lnLen = loDS.Tables(0).Columns(0).MaxLength
                lnCode = 1

                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)
            Else
                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)

                lsSQL = loDT.Rows(0).Item("sTransNox")
                lnLen = Len(lsSQL)

                lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
            End If
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function

    Private Function getNextMasterNo() As String
        Dim loDA As New MySqlDataAdapter
        Dim loDT As New DataTable
        Dim loDS As New DataSet
        Dim lsSQL As String
        Dim lnCounter As Integer
        Dim lnCode As Long
        Dim lnLen As Long
        Dim lsStr As String = ""

        lsSQL = "SELECT sMasterNo" &
                " FROM Cash_Reg_Machine" &
                " WHERE sIDNumber = " & strParm(p_sPOSNo)
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

            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            If IFNull(loDT.Rows(0).Item("sMasterNo"), "") = "" Then
                loDA.FillSchema(loDS, SchemaType.Source)
                lnLen = loDS.Tables(0).Columns(0).MaxLength
                lnCode = 1

                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)
            Else
                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)

                lsSQL = loDT.Rows(0).Item("sMasterNo")
                lnLen = Len(lsSQL)

                lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
            End If
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function

    Private Function getNextOrderNo() As String
        Dim loDA As New MySqlDataAdapter
        Dim loDT As New DataTable
        Dim loDS As New DataSet
        Dim lsSQL As String
        Dim lnCounter As Integer
        Dim lnCode As Long
        Dim lnLen As Long
        Dim lsStr As String = ""

        lsSQL = "SELECT sOrderNox" &
                " FROM Cash_Reg_Machine" &
                " WHERE sIDNumber = " & strParm(p_sPOSNo)
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

            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            If IFNull(loDT.Rows(0).Item("sOrderNox"), "") = "" Then
                loDA.FillSchema(loDS, SchemaType.Source)
                lnLen = loDS.Tables(0).Columns(0).MaxLength
                lnCode = 1

                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)
            Else
                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)

                lsSQL = loDT.Rows(0).Item("sOrderNox")
                lnLen = Len(lsSQL)

                lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
            End If
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function

    Private Function getNextBillNo() As String
        Dim loDA As New MySqlDataAdapter
        Dim loDT As New DataTable
        Dim loDS As New DataSet
        Dim lsSQL As String
        Dim lnCounter As Integer
        Dim lnCode As Long
        Dim lnLen As Long
        Dim lsStr As String = ""

        lsSQL = "SELECT sBillNmbr" &
                " FROM Cash_Reg_Machine" &
                " WHERE sIDNumber = " & strParm(p_sPOSNo)
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

            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            If IFNull(loDT.Rows(0).Item("sBillNmbr"), "") = "" Then
                loDA.FillSchema(loDS, SchemaType.Source)
                lnLen = loDS.Tables(0).Columns(0).MaxLength
                lnCode = 1

                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)
            Else
                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)

                lsSQL = loDT.Rows(0).Item("sBillNmbr")
                lnLen = Len(lsSQL)

                lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
            End If
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
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

        lsSQL = "SELECT nContrlNo" &
                " FROM " & pxeMasTable &
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate().ToString("yy") & "%")) &
                    " AND dTransact LIKE " & strParm(Format(p_oApp.getSysDate().ToString("yyyy-MM") & "%")) &
                " ORDER BY nContrlNo DESC" &
                " LIMIT 1"

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            Return 1
        Else
            Return IFNull(loDT(0).Item("nContrlNo"), 0) + 1
        End If
    End Function

    Private Function getSOTransNo() As String
        Dim loDA As New MySqlDataAdapter
        Dim loDT As New DataTable
        Dim loDS As New DataSet
        Dim lsSQL As String
        Dim lnCounter As Integer
        Dim lnCode As Long
        Dim lnLen As Long
        Dim lsStr As String = ""

        lsSQL = "SELECT sTransNox" &
                " FROM " & pxeMasTable &
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate().ToString("yy") & "%")) &
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

            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            If loDT.Rows(0).Item("sTransNox") = "" Then
                loDA.FillSchema(loDS, SchemaType.Source)
                lnLen = loDS.Tables(0).Columns(0).MaxLength
                lnCode = 1

                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)
            Else
                lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
                lnCounter = Len(lsSQL)

                lsSQL = loDT.Rows(0).Item("sTransNox")
                lnLen = Len(lsSQL)

                lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
            End If
        End If

        If lsSQL = "" Then
            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
        Else
            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        End If

        If lsSQL = "" Then
            Return Format(lnCode, lsStr.PadRight(lnCounter, "0"))
        Else
            Return Left(lsSQL, lnCounter) & Format(lnCode, lsStr.PadRight(lnLen - lnCounter, "0"))
        End If
    End Function


    Private Function getNextMergeID() As String
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lnCtr As Integer

        lsSQL = "SELECT sMergeIDx" &
                " FROM " & pxeMasTable &
                " WHERE sMergeIDx LIKE " & strParm(p_sBranchCd & Format(p_oApp.getSysDate(), "yy") & "%") &
                " ORDER BY sMergeIDx DESC" &
                " LIMIT 1"

        loDT = p_oApp.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnCtr = 1
        Else
            lnCtr = Mid(loDT(0).Item(0), Len(p_sBranchCd & "yy") + 1)
        End If

        Return p_sBranchCd & Format(p_oApp.getSysDate(), "yy") & Format(lnCtr, pxeCtrFormat)
    End Function

    Public Function InitMachine() As Boolean
        If p_sPOSNo = "" Then
            MsgBox("Invalid Machine Identification Info Detected...")
            Return False
        End If
        Dim lsSQL As String
        lsSQL = "SELECT" &
                       "  sAccredtn" &
                       ", dAcctnFrm" &
                       ", dAcctnTru" &
                       ", sPermitNo" &
                       ", dPTUFromx" &
                       ", dPTUThrux" &
                       ", sSerialNo" &
                       ", nPOSNumbr" &
                       ", cTranMode" &
                       ", nSChargex" &
               " FROM Cash_Reg_Machine" &
               " WHERE sIDNumber = " & strParm(p_sPOSNo)

        Dim loDta As DataTable
        loDta = p_oApp.ExecuteQuery(lsSQL)

        If loDta.Rows.Count <> 1 Then
            MsgBox("Invalid Config for MIN Detected...")
            Return False
        End If

        p_sAccrdt = loDta(0).Item("sAccredtn")
        p_dAccFrm = IFNull(loDta(0).Item("dAcctnFrm"), "1900-01-01")
        p_dAccTru = IFNull(loDta(0).Item("dAcctnTru"), "1900-01-01")
        p_sPermit = loDta(0).Item("sPermitNo")
        p_dPTUFrm = IFNull(loDta(0).Item("dPTUFromx"), "1900-01-01")
        p_dPTUTru = IFNull(loDta(0).Item("dPTUThrux"), "1900-01-01")
        p_sSerial = loDta(0).Item("sSerialNo")
        p_sTermnl = loDta(0).Item("nPOSNumbr")
        p_cTrnMde = loDta(0).Item("cTranMode")
        p_nSChargex = loDta(0).Item("nSChargex")
        p_bSChargex = False

        Return True
    End Function

    Private Function getSQ_ChargedOrder() As String
        Return _
            "SELECT sTransNox" &
                ", nContrlNo" &
                ", dTransact" &
                ", sReceiptx" &
                ", nTranTotl" &
                ", sCashierx" &
                ", sTableNox" &
                ", sWaiterID" &
                ", sMergeIDx" &
                ", cTranStat" &
                ", dModified" &
                " FROM " & pxeMasTable &
            " WHERE cTranStat = '5'" &
            " ORDER BY sTransNox ASC"
    End Function

    'Returns the SQL for the Management of the Master Part of the Transaction
    Private Function getSQ_Order() As String
        Return _
            "SELECT sTransNox" &
                ", nContrlNo" &
                ", dTransact" &
                ", sReceiptx" &
                ", nTranTotl" &
                ", sCashierx" &
                ", sTableNox" &
                ", sWaiterID" &
                ", sMergeIDx" &
                ", cTranStat" &
                ", sBillNmbr" &
                ", nPrntBill" &
                ", dPrntBill" &
                ", dModified" &
                ", cSChargex" &
                ", cTranType" &
                " FROM " & pxeMasTable &
            " WHERE cTranStat = '0'" &
                " AND LEFT(sTransNox, 6) = " & strParm(p_oApp.BranchCode & p_sTermnl) &
            " ORDER BY sTransNox ASC"
    End Function

    Private Function getSQ_OpenOrder() As String
        Return _
            "SELECT a.sTransNox" &
                ", a.nContrlNo" &
                ", a.dTransact" &
                ", a.sReceiptx" &
                ", a.nTranTotl" &
                ", a.sCashierx" &
                ", a.sTableNox" &
                ", a.sWaiterID" &
                ", a.sMergeIDx" &
                ", a.cTranStat" &
                ", a.sBillNmbr" &
                ", a.nPrntBill" &
                ", a.dPrntBill" &
                ", a.dModified" &
                ", a.cSChargex" &
                ", a.cTranType" &
                ", a.sCustName" &
            " FROM " & pxeMasTable & " a" &
                " LEFT JOIN Receipt_Master b" &
                "  ON a.sTransNox = b.sSourceNo" &
            " WHERE a.cTranStat IN ('0', '2')" &
                " AND (ISNULL(b.cTranStat) OR b.cTranStat = 0)" &
                " AND LEFT(a.sTransNox, 6) = " & strParm(p_oApp.BranchCode & p_sTermnl) &
                " AND ISNULL(b.sORNumber)" &
            " ORDER BY a. sTransNox ASC"
    End Function

    Private Function getSQ_neoOpenOrder() As String
        Return _
            "SELECT a.sTransNox" &
                ", a.nContrlNo" &
                ", a.dTransact" &
                ", a.sReceiptx" &
                ", a.nTranTotl" &
                ", a.sCashierx" &
                ", a.sTableNox" &
                ", a.sWaiterID" &
                ", a.sMergeIDx" &
                ", a.cTranStat" &
                ", a.sBillNmbr" &
                ", a.nPrntBill" &
                ", a.dPrntBill" &
                ", a.dModified" &
                ", a.cSChargex" &
                ", a.sCustName" &
            " FROM " & pxeMasTable & " a" &
            " WHERE a.dTransact = " & dateParm(p_dPOSDatex) &
                " AND a.cTranStat IN ('0', '2')" &
                " AND LEFT(a.sTransNox, 6) = " & strParm(p_oApp.BranchCode & p_sTermnl) &
            " ORDER BY a. sTransNox ASC"
    End Function

    'Returns the SQL for the Management of the Master Part of the Transaction
    Private Function getSQ_Detail() As String
        Return _
            "SELECT b.sBarcodex" &
                ", b.sDescript" &
                ", b.sBriefDsc" &
                ", a.nUnitPrce" &
                ", a.cReversex" &
                ", a.nQuantity" &
                ", a.nDiscount" &
                ", a.nAddDiscx" &
                ", b.nDiscLev1" &
                ", b.nDiscLev2" &
                ", b.nDiscLev3" &
                ", b.nDealrDsc" &
                ", a.sStockIDx" &
                ", b.sCategrID" &
                ", a.cPrintedx" &
                ", a.sTransNox" &
                ", a.nComplmnt" &
                ", a.nEntryNox" &
                ", a.cServedxx" &
                ", a.cDetailxx" &
                ", a.sReplItem" &
                ", a.cReversed" &
                ", a.cComboMlx" &
                ", a.cWthPromo" &
                ", a.dModified" &
                ", c.sPrntPath" &
            " FROM " & pxeDetTable & " a" &
                ", Inventory b" &
                    " LEFT JOIN Product_Category c" &
                        " ON b.sCategrID = c.sCategrCd" &
            " WHERE a.sStockIDx = b.sStockIDx" &
            " ORDER BY a.nEntryNox"
    End Function

    Private Function getSQ_Detail_WithMerge() As String
        Return _
            "SELECT b.sBarcodex" &
                ", b.sDescript" &
                ", b.sBriefDsc" &
                ", a.nUnitPrce" &
                ", a.cReversex" &
                ", a.nQuantity" &
                ", a.nDiscount" &
                ", a.nAddDiscx" &
                ", b.nDiscLev1" &
                ", b.nDiscLev2" &
                ", b.nDiscLev3" &
                ", b.nDealrDsc" &
                ", a.sStockIDx" &
                ", b.sCategrID" &
                ", a.cPrintedx" &
                ", a.sTransNox" &
                ", a.nComplmnt" &
                ", a.nEntryNox" &
                ", a.cServedxx" &
                ", a.cDetailxx" &
                ", a.sReplItem" &
                ", a.cReversed" &
                ", a.cComboMlx" &
                ", a.cWthPromo" &
                ", a.dModified" &
                ", c.sTableNox" &
                ", c.sMergeIDx" &
                ", d.sPrntPath" &
            " FROM " & pxeDetTable & " a" &
                ", Inventory b" &
                    " LEFT JOIN Product_Category d" &
                        " ON b.sCategrID = d.sCategrCd" &
                ", " & pxeMasTable & " c" &
            " WHERE a.sStockIDx = b.sStockIDx" &
                " AND a.sTransNox = c.sTransNox" &
            " ORDER BY a.sTransNox, a.nEntryNox"
    End Function

    Private Function getSQ_Search() As String
        'Kalyptus - 2016.11.04 09:33am
        'Added the fields cWthPromo and cComboMlx to the SQL statement

        Return _
            "SELECT a.sBarcodex" &
                ", a.sDescript" &
                ", a.nUnitPrce" &
                ", a.nSelPrice" &
                ", a.cWthPromo" &
                ", a.cComboMlx" &
                ", a.sBriefDsc" &
                ", a.sStockIDx" &
                ", a.nDiscLev1" &
                ", a.nDiscLev2" &
                ", a.nDiscLev3" &
                ", a.nDealrDsc" &
                ", a.sCategrID" &
                ", 0 nDiscAmtx" &
                ", b.sPrntPath" &
            " FROM Inventory a" &
                " LEFT JOIN Product_Category b" &
                    " ON a.sCategrID = b.sCategrCd" &
            " WHERE a.sInvTypID = " & strParm(pxeFinGoods) &
                " AND a.cRecdStat = '1'"
    End Function

    Private Function getSQ_Discount() As String
        Return _
            "SELECT" &
                "  nNoClient" &
                ", nWithDisc" &
            " FROM Discount"
    End Function

    Private Function getSQ_Promo(ByVal fsStockIDx As String, ByVal fsCategrID As String) As String
        Dim lsSQL As String

        lsSQL =
            "SELECT a.sTransNox" &
                ", a.nDiscRate" &
                ", a.nDiscAmtx" &
                ", a.nMinQtyxx" &
                ", a.nExtDRate" &
                ", a.nExtDAmtx" &
                ", a.nExtQtyxx" &
                ", a.dHappyHrF" &
                ", a.dHappyHrT" &
                ", a.dPromoFrm" &
                ", a.dPromoTru" &
            " FROM Sales_Promo a"

        If fsCategrID = vbEmpty Then
            lsSQL = lsSQL &
                " WHERE a.sStockIDx = " & strParm(fsStockIDx)
        Else
            lsSQL = lsSQL &
                " WHERE ( a.sStockIDx = " & strParm(fsStockIDx) &
                    " OR a.sCategrID = " & strParm(fsCategrID) & " )"
        End If

        lsSQL = lsSQL &
                    " AND SYSDATE() BETWEEN a.dPromoFrm AND a.dPromoTru" &
                    " AND ( a.tHappyHrF IS NULL" &
                        " OR TIME(SYSDATE()) BETWEEN a.tHappyHrF AND a.tHappyHrT )" &
                    " AND a.cRecdStat = " & xeRecordStat.RECORD_NEW &
                " ORDER BY a.dPromoFrm DESC LIMIT 1"
        Return lsSQL
    End Function

    Private Function getSQ_Combo(ByVal fsStockIDx As String) As String
        Return _
            "SELECT a.sComboIDx" &
                ", b.sBarcodex" &
                ", b.sDescript" &
                ", a.nQuantity" &
                ", b.sCategrID" &
                ", b.sStockIDx" &
                ", b.sBriefDsc" &
            " FROM Combo_Meals a" &
                ", Inventory b" &
            " WHERE a.sStockIDx = b.sStockIDx" &
                " AND a.sComboIDx = " & strParm(fsStockIDx) &
            " ORDER BY a.nEntryNox"
    End Function

    Public Function ProcTXReading() As Boolean
        'If p_oApp.UserLevel < xeUserRights.SUPERVISOR Then Return False

        Dim loDailySales As DailySales

        loDailySales = New DailySales(p_oApp)


        With loDailySales
            If Not .initMachine() Then
                MsgBox("Work Station is not Registered.", MsgBoxStyle.Exclamation, "Warning")
                Return False
            End If

            .Master("nSChargex") = p_nSChargex
            .Cashier = p_sCashierx

            Return .PrintTXReading(Format(p_dPOSDatex, "yyyyMMdd"), p_sPOSNo, False)
        End With
    End Function

    Public Function ProcTZReading() As Boolean
        Dim loFormZReading As frmZReading
        Dim lsSQL As String
        Dim lnCtr As Integer

        loFormZReading = New frmZReading(p_oApp)
        loFormZReading.PosDate = p_dPOSDatex
        loFormZReading.ShowDialog()

        If Not loFormZReading.Cancelled Then
            Dim loZReading As PRN_TZ_Reading

            loZReading = New PRN_TZ_Reading(p_oApp)
            loZReading.CashierName = getCashier(p_oApp.UserID)

            If Format(p_dPOSDatex, "yyyyMMdd") = Format(loFormZReading.POSFrom, "yyyyMMdd") And Format(p_dPOSDatex, "yyyyMMdd") = Format(loFormZReading.POSThru, "yyyyMMdd") Then
                If ProcTXReading() Then
                    If loZReading.PrintTZReading(Format(p_dPOSDatex, "yyyyMMdd"),
                                                        Format(p_dPOSDatex, "yyyyMMdd"),
                                                        p_sPOSNo, False) Then

                        Return True
                    End If
                End If
            Else
                If Not p_oApp.getUserApproval() Then
                    Return False
                    Exit Function
                End If

                lsSQL = "SELECT sTranDate, nZReadCtr FROM Daily_Summary" &
                        " WHERE sTranDate BETWEEN " & strParm(Format(loFormZReading.POSFrom, "yyyyMMdd")) & " AND " & strParm(Format(loFormZReading.POSThru, "yyyyMMdd")) &
                            " AND sCRMNumbr = " & strParm(p_sPOSNo) &
                            " AND cTranStat IN ('2')" &
                       " ORDER BY sTranDate"

                Dim loDta As DataTable
                loDta = p_oApp.ExecuteQuery(lsSQL)

                If loDta.Rows.Count = 0 Then
                    MsgBox("There are no transaction for the given date....", , "New_Sales_Order")
                    Return True
                End If

                For lnCtr = 0 To loDta.Rows.Count - 1
                    If loZReading.PrintTZReading(loDta.Rows(lnCtr)("sTranDate"),
                                                            loDta.Rows(lnCtr)("sTranDate"),
                                                            p_sPOSNo, True, loDta.Rows(lnCtr)("nZReadCtr")) Then
                    End If
                Next
                MsgBox("Z-Reading was perform successfully!!", , "ProcTZReading")

                Return True

            End If
        End If

        Return False
    End Function

    Public Function ReComputeReading() As Boolean
        If Not p_oApp.getUserApproval() Then
            Return False
            Exit Function
        End If

        Dim lsSQL As String
        Dim loDT As DataTable

        'On Error GoTo errProc

        lsSQL = "SELECT a.sTranDate" &
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
                " FROM Daily_Summary a" &
                " WHERE a.cTranStat = '2'" &
                " ORDER BY a.sTranDate"

        loDT = p_oApp.ExecuteQuery(lsSQL)

        Dim lnCtr As Integer

        Dim loDailySales As DailySales
        Dim loZReading As PRN_TZ_Reading

        loDailySales = New DailySales(p_oApp)
        loDailySales.initMachine()
        loZReading = New PRN_TZ_Reading(p_oApp)

        lsSQL = "UPDATE Cash_Reg_Machine" &
                   " SET nVATSales = 0" &
                      ", nVATAmtxx = 0" &
                      ", nNonVATxx = 0" &
                      ", nZeroRatd = 0" &
                      ", nSalesTot = 0" &
                      ", nSChrgAmt = 0" &
                   " WHERE sIDNumber = " & strParm(p_sPOSNo)
        p_oApp.Execute(lsSQL, "Cash_Reg_Machine")

        'Dim loDTGC As DataTable
        'lsSQL = "SELECT * FROM Gift_Certificate_Trans"
        'loDTGC = p_oApp.ExecuteQuery(lsSQL)

        'Dim loDTPaym As DataTable
        'Dim lnAmount As Decimal
        'Dim lnCtrx As Integer

        'lsSQL = "SELECT * FROM Gift_Certificate_Trans a" &
        '            " , Receipt_Master b" &
        '            " WHERE a.sSourceNo = b.sSourceNo" &
        '                " AND b.cTranStat <> '3'"
        'loDTGC = p_oApp.ExecuteQuery(lsSQL)

        'If loDTGC.Rows.Count > 0 Then
        '    For lnCtr = 0 To loDTGC.Rows.Count - 1
        '        lsSQL = "SELECT * FROM Payment" &
        '                    " WHERE sSourceNo = " & strParm(loDTGC.Rows(lnCtr)("sSourceNo")) &
        '                    " ORDER BY nEntryNox"
        '        loDTPaym = New DataTable
        '        loDTPaym = p_oApp.ExecuteQuery(lsSQL)

        '        If loDTPaym.Rows.Count = 1 Then
        '            lnAmount = (loDTGC.Rows(lnCtr)("nSalesAmt") +
        '                        loDTGC.Rows(lnCtr)("nSChargex")) -
        '                        (loDTGC.Rows(lnCtr)("nDiscount") +
        '                        loDTGC.Rows(lnCtr)("nVatDiscx") +
        '                        loDTGC.Rows(lnCtr)("nPWDDiscx"))

        '            p_oApp.Execute("UPDATE Payment SET" &
        '                                " nAmountxx = " & CDec(lnAmount) &
        '                            " WHERE sSourceNo = " & strParm(loDTGC.Rows(lnCtr)("sSourceNo")), "Payment")

        '            p_oApp.Execute("UPDATE Receipt_Master SET" &
        '                                "  nCashAmtx = 0" &
        '                                ", nTendered = 0" &
        '                            " WHERE sSourceNo = " & strParm(loDTGC.Rows(lnCtr)("sSourceNo")), "Receipt_Master")
        '        Else
        '            For lnCtrx = 0 To loDTPaym.Rows.Count - 1
        '                If loDTPaym.Rows(lnCtrx)("cPaymForm") = 0 Then
        '                    p_oApp.Execute("UPDATE Payment SET" &
        '                                " nAmountxx = " & CDec(loDTGC.Rows(lnCtr)("nCashAmtx") - loDTGC.Rows(lnCtr)("nAmountxx")) &
        '                            " WHERE sSourceNo = " & strParm(loDTGC.Rows(lnCtr)("sSourceNo")) &
        '                                " AND nEntryNox = " & CInt(loDTPaym.Rows(lnCtrx)("nEntryNox")), "Payment")
        '                Else
        '                    p_oApp.Execute("UPDATE Payment SET" &
        '                               " nAmountxx = " & CDec(loDTGC.Rows(lnCtr)("nAmountxx")) &
        '                           " WHERE sSourceNo = " & strParm(loDTGC.Rows(lnCtr)("sSourceNo")) &
        '                               " AND nEntryNox = " & CInt(loDTPaym.Rows(lnCtrx)("nEntryNox")), "Payment")
        '                End If
        '            Next
        '            p_oApp.Execute("UPDATE Receipt_Master SET" &
        '                               "  nCashAmtx =  " & CDec(loDTGC.Rows(lnCtr)("nCashAmtx") - loDTGC.Rows(lnCtr)("nAmountxx")) &
        '                           " WHERE sSourceNo = " & strParm(loDTGC.Rows(lnCtr)("sSourceNo")), "Receipt_Master")
        '        End If
        '    Next
        'End If

        For lnCtr = 0 To loDT.Rows.Count - 1
            Dim lsDate As String
            lsDate = Left(loDT.Rows(lnCtr)("sTranDate"), 4) & "-" &
                     loDT.Rows(lnCtr)("sTranDate").ToString.Substring(4, 2) & "-" &
                     Right(loDT.Rows(lnCtr)("sTranDate"), 2)

            p_oApp.Execute("UPDATE Receipt_Master SET" &
                                       " sCashierx = " & strParm(loDT.Rows(lnCtr)("sCashierx")) &
                                   " WHERE dTransact = " & dateParm(lsDate) &
                                       " AND sCashierx = ''", "Receipt_Master")

            loDailySales.doPrintTXReadingReg(loDT.Rows(lnCtr)("sTranDate"), p_sPOSNo, loDT.Rows(lnCtr)("sCashierx"))
            'loDailySales.doWriteTXReadingReg(loDT.Rows(lnCtr)("sTranDate"), p_sPOSNo, loDT.Rows(lnCtr)("sCashierx"))
            'MsgBox("Print Z Reading" & "- " & loDT.Rows(lnCtr)("sTranDate"))
            'loZReading.doPrintTZReadingReg(loDT.Rows(lnCtr)("sTranDate"),
            '                                loDT.Rows(lnCtr)("sTranDate"),
            '                                p_sPOSNo, lnCtr + 1)
            'loZReading.doWriteTZReadingReg(loDT.Rows(lnCtr)("sTranDate"),
            '                                loDT.Rows(lnCtr)("sTranDate"),
            '                                p_sPOSNo, lnCtr + 1)
        Next

        Return True
        'errProc:
        '        Return False
    End Function

    Public Function getCashier(ByVal sCashierx As String) As String
        Dim lsSQL As String
        Dim lsCashierNm As String
        Dim loDta As DataTable

        lsSQL = "SELECT" &
                    " a.sUserName" &
                    " FROM xxxSysUser a" &
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

    Public Sub CancelOR()
        If Not p_oApp.getUserApproval() Then Exit Sub

        Dim lsSQL As String
        Dim loDTMaster As DataTable
        Dim loDTDetail As DataTable
        Dim loSalesReturn As SalesReturn

        'show list of OR to reprint
        lsSQL = "SELECT sORNumber" &
                     ", dTransact" &
                     ", nSalesAmt" &
                     ", IF(sSourceCd = 'SO', 'Sales Order', 'Splitted Order') sSourceNm" &
                     ", sSourceCD" &
                     ", sSourceNo" &
                     ", sTransNox" &
               " FROM Receipt_Master" &
               " WHERE sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " AND dTransact = " & dateParm(p_oApp.getSysDate) &
                 " AND cTranStat NOT IN ('3', 4)"

        Dim loRow As DataRow = KwikSearch(p_oApp _
                                         , lsSQL _
                                         , True _
                                         , "" _
                                         , "sORNumber»dTransact»nSalesAmt»sSourceNm" _
                                         , "SI No»Date»Sales Amount»Source",
                                         , "sORNumber»dTransact»nSalesAmt»IF(sSourceCd = 'SO', 'Sales Order', 'Splitted Order')" _
                                         , 0)
        If IsNothing(loRow) Then
            Exit Sub
        End If

        lsSQL = "UPDATE Receipt_Master SET" &
                    "  cTranStat = " & strParm(xeTranStat.TRANS_CANCELLED) &
                " WHERE sTransNox = " & strParm(loRow("sTransNox"))

        If p_oApp.Execute(lsSQL, "Receipt_Master") <= 0 Then
            MsgBox("Unable to cancel invoice.", MsgBoxStyle.Critical, "Warning")
        End If

        loSalesReturn = New SalesReturn(p_oApp)
        '        loSalesReturn.
        '        loSalesReturn.Master()
        '        "Field"	"Type"	"Null"	"Key"	"Default"	"Extra"
        '"sTransNox"	"varchar(14)"	"NO"	"PRI"	\N	""
        '"dTransact"	"date"	"YES"	""	\N	""
        '"sRemarksx"	"varchar(64)"	"YES"	""	\N	""
        '"sSourceNo"	"varchar(14)"	"YES"	"MUL"	\N	""
        '"sSourceCd"	"varchar(4)"	"YES"	""	\N	""
        '"nTranAmtx"	"decimal(10,2)"	"YES"	""	"0.00"	""
        '"sApproved"	"varchar(10)"	"YES"	""	\N	""
        '"cTranStat"	"char(1)"	"YES"	""	"0"	""
        '"sModified"	"varchar(10)"	"YES"	""	\N	""
        '"dModified"	"datetime"	"YES"	""	\N	""

        'Load Receipt
        lsSQL = "SELECT a.sTransNox" &
                    ", a.dTransact" &
                    ", b.sTableNox" &
                    ", d.sIDNumber" &
                    ", d.sClientNm" &
                    ", c.nNoClient" &
                    ", c.nWithDisc" &
                    ", a.sORNumber" &
                    ", a.nSalesAmt" &
                    ", a.nVATSales" &
                    ", a.nVATAmtxx" &
                    ", a.nZeroRatd" &
                    ", a.nDiscount" &
                    ", a.nVatDiscx" &
                    ", a.nPWDDiscx" &
                    ", a.nTendered" &
                    ", a.nCashAmtx" &
                    ", a.sSourceCd" &
                    ", a.sSourceNo" &
                    ", a.sCashierx" &
                " FROM Receipt_Master a" &
                " LEFT JOIN SO_Master b" &
                    " ON a.sSourceNo = b.sTransNox" &
                " LEFT JOIN Discount c" &
                    " ON b.sTransNox = c.sSourceNo" &
                " LEFT JOIN Discount_Detail d" &
                    " ON c.sTransNox =  d.sTransNox" &
                " WHERE a.sSourceCD = " & strParm(loRow("sSourceCd")) &
                " AND a.sSourceNo = " & strParm(loRow("sSourceNo"))
        loDTMaster = p_oApp.ExecuteQuery(lsSQL)

        If loRow("sSourceCD") = "SO" Then
            lsSQL = "SELECT b.sBarcodex" &
                         ", b.sDescript" &
                         ", b.sBriefDsc" &
                         ", a.nUnitPrce" &
                         ", a.cReversex" &
                         ", a.nQuantity" &
                         ", a.nDiscount" &
                         ", a.nAddDiscx" &
                         ", a.sStockIDx" &
                         ", b.sCategrID" &
                         ", a.cPrintedx" &
                         ", a.sTransNox" &
                         ", a.nComplmnt" &
                         ", a.nEntryNox" &
                         ", a.cServedxx" &
                         ", a.cDetailxx" &
                         ", a.sReplItem" &
                         ", a.cReversed" &
                         ", a.cComboMlx" &
                         ", a.cWthPromo" &
                         ", 0 nAmountxx" &
            " FROM " & pxeDetTable & " a" &
                ", Inventory b" &
            " WHERE a.sTransNox = " & strParm(loRow("sSourceNo")) &
              " AND a.sStockIDx = b.sStockIDx" &
            " ORDER BY a.nEntryNox"
        Else
            lsSQL = "SELECT b.sBarcodex" &
                         ", b.sDescript" &
                         ", b.sBriefDsc" &
                         ", a.nUnitPrce" &
                         ", '+' cReversex" &
                         ", a.nQuantity" &
                         ", 0 nDiscount" &
                         ", 0 nAddDiscx" &
                         ", a.sStockIDx" &
                         ", b.sCategrID" &
                         ", '1' cPrintedx" &
                         ", a.sTransNox" &
                         ", 0 nComplmnt" &
                         ", a.nEntryNox" &
                         ", '0' cServedxx" &
                         ", '0' cDetailxx" &
                         ", '' sReplItem" &
                         ", '0' cReversed" &
                         ", '0' cComboMlx" &
                         ", '0' cWthPromo" &
                         ", c.nAmountxx" &
            " FROM Order_Split_Detail a" &
                ", Inventory b" &
                ", Order_Split c" &
            " WHERE a.sTransNox = " & strParm(loRow(0).Item("sSourceNo")) &
              " AND a.sStockIDx = b.sStockIDx" &
              " AND a.sTransNox = c.sTransNox" &
            " ORDER BY a.nEntryNox"
        End If

        loDTDetail = p_oApp.ExecuteQuery(lsSQL)

        Dim loPayment As Receipt
        Dim lsReferNo As String = getNextMasterNo()

        loPayment = New Receipt(p_oApp)
        loPayment.PosDate = p_oApp.getSysDate
        loPayment.SourceCd = loDTMaster(0).Item("sSourceCD")
        loPayment.SourceNo = loDTMaster(0).Item("sSourceNo")
        loPayment.LogName = p_sLogName
        loPayment.POSNumbr = p_sTermnl
        loPayment.CRMNumbr = p_sPOSNo
        loPayment.SalesOrder = loDTDetail
        loPayment.TranMode = p_cTrnMde
        loPayment.ClientNo = IFNull(loDTMaster(0).Item("nNoClient"), 0)
        loPayment.WithDisc = IFNull(loDTMaster(0).Item("nWithDisc"), 0)
        loPayment.TableNo = IIf(IFNull(loDTMaster(0).Item("sTableNox"), "") = "", 0, loDTMaster(0).Item("sTableNox"))
        loPayment.MasterNo = lsReferNo
        If loRow("sSourceCD") = "SO" Then loPayment.SplitType = 2
        loPayment.OpenBySource()

        'unknownpurpose already setted in openbysource
        'Dim lnNotVat As Decimal
        'lnNotVat = loDTMaster(0).Item("nSalesAmt") + loDTMaster(0).Item("nVatDiscx") + loDTMaster(0).Item("nPWDDiscx")
        'lnNotVat = lnNotVat - (loDTMaster(0).Item("nVatSales") + loDTMaster(0).Item("nVatAmtxx"))
        ''loPayment.NonVAT = lnNotVat

        Dim loDiscDtl As DataTable
        loDiscDtl = LoadDiscount(loDTMaster(0).Item("sSourceCD"), loDTMaster(0).Item("sSourceNo"))
        loPayment.Discounts = loDiscDtl

        If p_oDiscount.HasDiscount Then
            'loPayment.Discounts = p_oDiscount.Discounts

            If p_oDiscount.Master("cNoneVATx") = "1" Then
                loPayment.DiscAmount = loDTMaster(0).Item("nVatDiscx") + loDTMaster(0).Item("nPWDDiscx")
                loPayment.NonVAT = loDTMaster.Rows(0)("nSalesAmt") + loDTMaster.Rows(0)("nPWDDiscx")
            Else
                loPayment.DiscAmount = loDTMaster(0).Item("nDiscount")
                loPayment.NonVAT = 0.00
            End If
        End If

        p_oApp.SaveEvent("0010", "Cancelled SI No. " & loRow("sORNumber") & "/Amount " & loRow("nSalesAmt"), p_sSerial)
        Dim lsSourceNo As String = ""
        If updateCashRegMachine(lsSourceNo, True) Then loPayment.printCancelled(lsSourceNo)
    End Sub

    Public Sub PrintChargeOR()
        Dim lsSQL As String

        lsSQL = "SELECT" &
                    "  RIGHT(a.sTransNox, 8) sTransNox" &
                    ", c.dTransact dTransact" &
                    ", IF(IFNULL(b.sCompnyNm, '') = '', a.sClientNm, b.sCompnyNm) sClientNm" &
                    ", a.nAmountxx nAmountxx" &
                    ", a.nDiscount nDiscount" &
                    ", a.nVatDiscx nVatDiscx" &
                    ", a.nPWDDIscx nPWDDIscx" &
                    ", a.sSourceCd sSourceCd" &
                    ", a.sSourceNo sSourceNo" &
                " FROM Charge_Invoice a" &
                    " LEFT JOIN Client_Master b" &
                        " ON a.sClientID = b.sClientID" &
                    " LEFT JOIN SO_Master c" &
                        " ON a.sSourceNo = c.sTransNox" &
                " WHERE a.sSourceCd = 'SO'" &
                    " AND a.cTranStat = '1'" &
                    " AND a.cORPrintx = '0'"

        Dim loRow As DataRow = KwikSearch(p_oApp _
                                         , lsSQL _
                                         , True _
                                         , "" _
                                         , "sTransNox»dTransact»sClientNm»nAmountxx" _
                                         , "CI No»Date»Name»Amount",
                                         , "RIGHT(a.sTransNox, 8)»c.dTransact»IF(IFNULL(b.sCompnyNm, '') = '', a.sClientNm, b.sCompnyNm)»a.nAmountxx" _
                                         , 0)
        If IsNothing(loRow) Then
            Exit Sub
        End If

        If LoadChargeOrder(loRow("sSourceNo")) Then

            p_nBill = p_oDTMaster(0).Item("nTranTotl")
            p_nCharge = IFNull(p_oDTMaster(0).Item("nSChargex"), 0)


            If PayOrder() Then
                lsSQL = "UPDATE Charge_Invoice SET " &
                            " cTranStat = " & strParm(xeTranStat.TRANS_CLOSED) &
                        " WHERE sSourceNo = " & strParm(loRow("sSourceNo")) &
                            " AND sSourceCD = " & strParm(loRow("sSourceCD"))
            End If
        End If
    End Sub

    Public Sub Reprint()
        If Not p_oApp.getUserApproval() Then Exit Sub

        Dim lsSQL As String
        Dim loDTMaster As DataTable
        Dim loDTDetail As DataTable

        'show list of OR to reprint
        lsSQL = "SELECT sORNumber" &
                     ", dTransact" &
                     ", nSalesAmt" &
                     ", IF(sSourceCd = 'SO', 'Sales Order', 'Splitted Order') sSourceNm" &
                     ", sSourceCD" &
                     ", sSourceNo" &
               " FROM Receipt_Master" &
               " WHERE sTransNox LIKE " & strParm(p_oApp.BranchCode & p_sTermnl & "%") &
                 " AND dTransact <= " & dateParm(DateAdd(DateInterval.Day, 31, p_oApp.getSysDate))

        Dim loRow As DataRow = KwikSearch(p_oApp _
                                         , lsSQL _
                                         , True _
                                         , "" _
                                         , "sORNumber»dTransact»nSalesAmt»sSourceNm" _
                                         , "SI No»Date»Sales Amount»Source",
                                         , "sORNumber»dTransact»nSalesAmt»IF(sSourceCd = 'SO', 'Sales Order', 'Splitted Order')" _
                                         , 0)
        If IsNothing(loRow) Then
            Exit Sub
        End If

        'Load Receipt
        'lsSQL = "SELECT sTransNox" & _
        '             ", dTransact" & _
        '             ", sORNumber" & _
        '             ", nSalesAmt" & _
        '             ", nVATSales" & _
        '             ", nVATAmtxx" & _
        '             ", nZeroRatd" & _
        '             ", nDiscount" & _
        '             ", nVatDiscx" & _
        '             ", nPWDDiscx" & _
        '             ", nTendered" & _
        '             ", nCashAmtx" & _
        '             ", sSourceCd" & _
        '             ", sSourceNo" & _
        '             ", sCashierx" & _
        '       " FROM Receipt_Master" & _
        '       " WHERE sSourceCD = " & strParm(loRow("sSourceCd")) & _
        '         " AND sSourceNo = " & strParm(loRow("sSourceNo"))

        lsSQL = "SELECT a.sTransNox" &
                    ", a.dTransact" &
                    ", b.sTableNox" &
                    ", d.sIDNumber" &
                    ", d.sClientNm" &
                    ", c.nNoClient" &
                    ", c.nWithDisc" &
                    ", a.sORNumber" &
                    ", a.nSalesAmt" &
                    ", a.nVATSales" &
                    ", a.nVATAmtxx" &
                    ", a.nZeroRatd" &
                    ", a.nDiscount" &
                    ", a.nVatDiscx" &
                    ", a.nPWDDiscx" &
                    ", a.nTendered" &
                    ", a.nCashAmtx" &
                    ", a.sSourceCd" &
                    ", a.sSourceNo" &
                    ", a.sCashierx" &
                " FROM Receipt_Master a" &
                " LEFT JOIN SO_Master b" &
                    " ON a.sSourceNo = b.sTransNox" &
                " LEFT JOIN Discount c" &
                    " ON b.sTransNox = c.sSourceNo" &
                " LEFT JOIN Discount_Detail d" &
                    " ON c.sTransNox =  d.sTransNox" &
                " WHERE a.sSourceCD = " & strParm(loRow("sSourceCd")) &
                " AND a.sSourceNo = " & strParm(loRow("sSourceNo"))

        loDTMaster = p_oApp.ExecuteQuery(lsSQL)
        Debug.Print(lsSQL)

        If loRow("sSourceCD") = "SO" Then
            lsSQL = "SELECT b.sBarcodex" &
                         ", b.sDescript" &
                         ", b.sBriefDsc" &
                         ", a.nUnitPrce" &
                         ", a.cReversex" &
                         ", a.nQuantity" &
                         ", a.nDiscount" &
                         ", a.nAddDiscx" &
                         ", a.sStockIDx" &
                         ", b.sCategrID" &
                         ", a.cPrintedx" &
                         ", a.sTransNox" &
                         ", a.nComplmnt" &
                         ", a.nEntryNox" &
                         ", a.cServedxx" &
                         ", a.cDetailxx" &
                         ", a.sReplItem" &
                         ", a.cReversed" &
                         ", a.cComboMlx" &
                         ", a.cWthPromo" &
                         ", 0 nAmountxx" &
            " FROM " & pxeDetTable & " a" &
                ", Inventory b" &
            " WHERE a.sTransNox = " & strParm(loRow("sSourceNo")) &
              " AND a.sStockIDx = b.sStockIDx" &
            " ORDER BY a.nEntryNox"
        Else
            lsSQL = "SELECT b.sBarcodex" &
                         ", b.sDescript" &
                         ", b.sBriefDsc" &
                         ", a.nUnitPrce" &
                         ", '+' cReversex" &
                         ", a.nQuantity" &
                         ", 0 nDiscount" &
                         ", 0 nAddDiscx" &
                         ", a.sStockIDx" &
                         ", b.sCategrID" &
                         ", '1' cPrintedx" &
                         ", a.sTransNox" &
                         ", 0 nComplmnt" &
                         ", a.nEntryNox" &
                         ", '0' cServedxx" &
                         ", '0' cDetailxx" &
                         ", '' sReplItem" &
                         ", '0' cReversed" &
                         ", '0' cComboMlx" &
                         ", '0' cWthPromo" &
                         ", c.nAmountxx" &
            " FROM Order_Split_Detail a" &
                ", Inventory b" &
                ", Order_Split c" &
            " WHERE a.sTransNox = " & strParm(loRow(0).Item("sSourceNo")) &
              " AND a.sStockIDx = b.sStockIDx" &
              " AND a.sTransNox = c.sTransNox" &
            " ORDER BY a.nEntryNox"
        End If

        loDTDetail = p_oApp.ExecuteQuery(lsSQL)
        Debug.Print(lsSQL)

        Dim loPayment As Receipt
        Dim lsNeoTrans As String = ""
        Dim lsReferNo As String = getNextMasterNo()
        loPayment = New Receipt(p_oApp)
        loPayment.PosDate = p_oApp.getSysDate
        loPayment.SourceCd = loDTMaster(0).Item("sSourceCD")
        loPayment.SourceNo = loDTMaster(0).Item("sSourceNo")
        loPayment.LogName = p_sLogName
        loPayment.POSNumbr = p_sTermnl
        loPayment.CRMNumbr = p_sPOSNo
        loPayment.SalesOrder = loDTDetail
        loPayment.TranMode = p_cTrnMde
        loPayment.ClientNo = IFNull(loDTMaster(0).Item("nNoClient"), 0)
        loPayment.WithDisc = IFNull(loDTMaster(0).Item("nWithDisc"), 0)
        loPayment.TableNo = IIf(IFNull(loDTMaster(0).Item("sTableNox"), "") = "", 0, loDTMaster(0).Item("sTableNox"))
        loPayment.MasterNo = lsReferNo
        loPayment.SplitType = 2
        loPayment.OpenBySource()

        Call updateCashRegMachine(lsNeoTrans, True)

        Dim lnNotVat As Decimal
        'lnNotVat = loDTMaster(0).Item("nSalesAmt") + loDTMaster(0).Item("nVatDiscx") + loDTMaster(0).Item("nPWDDiscx")
        'lnNotVat = lnNotVat - (loDTMaster(0).Item("nVatSales") + loDTMaster(0).Item("nVatAmtxx"))
        'loPayment.NonVAT = lnNotVat / 1.12

        Dim loDiscDtl As DataTable
        loDiscDtl = LoadDiscount(loDTMaster(0).Item("sSourceCD"), loDTMaster(0).Item("sSourceNo"))
        'jovan 03-11-21
        loPayment.Discounts = loDiscDtl

        If p_oDiscount.HasDiscount Then
            'loPayment.Discounts = p_oDiscount.Discounts

            If p_oDiscount.Master("cNoneVATx") = "1" Then
                loPayment.DiscAmount = loDTMaster(0).Item("nVatDiscx") + loDTMaster(0).Item("nPWDDiscx")
            Else
                loPayment.DiscAmount = loDTMaster(0).Item("nDiscount")
            End If
        End If

        'loPayment.Master("nDiscount") = Math.Round(loDTMaster(0).Item("nDiscount"), 2)
        'loPayment.Master("nVatDiscx") = Math.Round(loDTMaster(0).Item("nVatDiscx"), 2)
        'loPayment.Master("nPWDDiscx") = Math.Round(loDTMaster(0).Item("nPWDDiscx"), 2)

        'loPayment.Master("nSalesAmt") = Math.Round(loDTMaster(0).Item("nSalesAmt"), 2) - Math.Round((p_oDTMaster(0).Item("nVoidTotl") + p_oDTMaster(0).Item("nDiscount") + p_oDTMaster(0).Item("nVatDiscx") + p_oDTMaster(0).Item("nPWDDiscx")), 2)

        'loPayment.Master("nVATSales") = Math.Round(loDTMaster(0).Item("nVATSales"), 2)
        'loPayment.Master("nVATAmtxx") = Math.Round(loDTMaster(0).Item("nVATAmtxx"), 2)

        'If loPayment.Master("nPWDDiscx") > 0 Then
        '    loPayment.Master("nSChargex") = IIf(p_bSChargex, (loPayment.Master("nSalesAmt")) * (p_nSChargex / 100), 0)
        'Else
        '    loPayment.Master("nSChargex") = IIf(p_bSChargex, (loPayment.Master("nSalesAmt") / 1.12) * (p_nSChargex / 100), 0)
        'End If

        p_oApp.SaveEvent("0014", "SI No. " & loRow("sORNumber"), p_sSerial)

        Dim lnRow As Integer
        lsSQL = "UPDATE Daily_Summary SET" &
                        "  nRepAmntx = nRepAmntx + " & CLng(loDTMaster.Rows(0)("nSalesAmt")) &
                        ", nReprintx = nReprintx + 1" &
                " WHERE sTranDate = " & strParm(Format(p_dPOSDatex, "yyyyMMdd")) &
                    " AND sCRMNumbr = " & strParm(p_sPOSNo) &
                    " AND sCashierx = " & strParm(p_sCashierx)
        Debug.Print(lsSQL)
        Try
            lnRow = p_oApp.Execute(lsSQL, "Daily_Summary")
            If lnRow <= 0 Then
                MsgBox("Unable to Reprint Transaction!!!" & vbCrLf &
                        "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                Exit Sub
            End If
        Catch ex As Exception
            Throw ex
        End Try

        loPayment.printReciept(True)
    End Sub

    Private Function getUserName(ByVal fsUserIDxx) As String
        Dim loDT As DataTable

        loDT = p_oApp.ExecuteQuery("SELECT sUserName FROM xxxSysUser WHERE sUserIDxx = " & strParm(fsUserIDxx))

        If loDT.Rows.Count = 0 Then
            Return ""
        Else
            Return loDT(0)("sUserName")
        End If
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTMaster = Nothing
        p_oDTDetail = Nothing
        p_oDiscount = Nothing
        p_oDtaDiscx = Nothing
        p_oTable = Nothing

        p_bShowMsg = False
        p_bExisting = False

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")
        p_sVATReg = Environment.GetEnvironmentVariable("REG-TIN-No")

        Call InitMachine()

        p_sBranchCd = p_oApp.BranchCode
        p_sBranchNm = p_oApp.BranchName
        p_sAddressx = p_oApp.Address & ", " & p_oApp.TownCity & ", " & p_oApp.Province

        Call createMaster()
        Call initMaster()

        Call createDetail()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub getPriceUpdateToday(Optional ByVal fbCheckPrice = False)
        Dim loDT As New DataTable
        Dim lsSQL As String
        Dim lnCount As Integer
        Dim lnRow As Integer

        If Not fbCheckPrice Then
            lsSQL = AddCondition(getSQ_Inventory, " dPricexxx = " & dateParm(p_oApp.getSysDate) & " AND dModified < " & dateParm(p_oApp.getSysDate) & " ORDER BY dModified DESC ")
        Else
            lsSQL = AddCondition(getSQ_Inventory, " dPricexxx < " & dateParm(p_oApp.getSysDate) & " AND dModified < " & dateParm(p_oApp.getSysDate) & " ORDER BY dModified DESC ")
        End If

        loDT = p_oApp.ExecuteQuery(lsSQL)
        lnCount = loDT.Rows.Count
        If loDT.Rows.Count <= 0 Then
            Exit Sub
        End If

        Dim loHsUpdate As Boolean

        For x As Integer = x To lnCount
            Dim loHistoryData As New DataTable
            lsSQL = ""
            lsSQL = AddCondition(getSQ_HistoryPrice, " dPricexxx = " & dateParm(loDT(x).Item("dPricexxx")) & " AND sStockIDx = " & strParm(loDT(x).Item("sStockIDx")) & " GROUP BY sStockIDx ORDER BY dModified DESC LIMIT 1 ")
            loHistoryData = p_oApp.ExecuteQuery(lsSQL)

            If loHistoryData.Rows.Count <= 0 Then
                Exit For
            End If

            loHsUpdate = loDT(x).Item("nUnitPrce") <> loHistoryData(0).Item("nPurPrice") Or
                loDT(x).Item("nSelPrice") <> loHistoryData(0).Item("nSelPrice") Or
                loDT(x).Item("sCategrID") <> loHistoryData(0).Item("sCategrID") Or
                loDT(x).Item("cRecdStat") <> loHistoryData(0).Item("cRecdStat")
            lsSQL = ""
            If loHsUpdate Then

                lsSQL = "UPDATE Inventory SET" &
                            " nUnitPrce = " & strParm(loHistoryData(0).Item("nPurPrice")) &
                            ", nSelPrice = " & strParm(loHistoryData(0).Item("nSelPrice")) &
                            ", sCategrID = " & strParm(loHistoryData(0).Item("sCategrID")) &
                            ", cRecdStat = " & strParm(loHistoryData(0).Item("cRecdStat")) &
                            ", sModified = " & strParm(loHistoryData(0).Item("sModified")) &
                            ", dModified = " & datetimeParm(p_oApp.getSysDate) &
                            " WHERE sStockIDx = " & strParm(loHistoryData(0).Item("sStockIDx"))

                If lsSQL <> "" Then
                    Try
                        lnRow = p_oApp.Execute(lsSQL, "Inventory")
                        If lnRow <= 0 Then

                            Exit For
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If
            Else
                Exit For
            End If

        Next

    End Sub



    Private Function getSQ_HistoryPrice() As String
        Return "SELECT sStockIDx" &
                    ", dPricexxx" &
                    ", nPurPrice" &
                    ", nSelPrice" &
                    ", sCategrID" &
                    ", cRecdStat" &
                    ", sModified" &
              " FROM Price_History "
    End Function

    Private Function getSQ_Inventory() As String
        Return "SELECT sStockIDx" &
                    ", dPricexxx" &
                    ", nUnitPrce" &
                    ", nSelPrice" &
                    ", sCategrID" &
                    ", cRecdStat" &
              " FROM Inventory "
    End Function
End Class