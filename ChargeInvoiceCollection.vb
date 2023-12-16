'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Charge Invoice Collection Object
'
' Copyright 2018 and Beyond
' All Rights Reserved
' ºººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººº
' €  All  rights reserved. No part of this  software  €€  This Software is Owned by        €
' €  may be reproduced or transmitted in any form or  €€                                   €
' €  by   any   means,  electronic   or  mechanical,  €€    GUANZON MERCHANDISING CORP.    €
' €  including recording, or by information  storage  €€     Guanzon Bldg. Perez Blvd.     €
' €  and  retrieval  systems, without  prior written  €€           Dagupan City            €
' €  from the author.                                 €€  Tel No. 522-1085 ; 522-9275      €
' ººººººººººººººººººººººººººººººººººººººººººººººººººººººººººººsºººººººººººººººººººººººººººººº
'
' ==========================================================================================
'  XerSys [ 01/15/2018 01:10 pm ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports MySql.Data.MySqlClient
Imports ggcAppDriver
Imports ggcReceipt
Imports ggcRetailParams

Public Class ChargeInvoiceCollection
    Private Const xsSignature As String = "08220326"
    Private p_oApp As GRider
    Private p_oDTDetail As DataTable
    Private p_oDTMaster As DataTable

    Private p_nEditMode As xeEditMode

    Private p_sPOSNo As String      'MIN:       14121419321782091
    Private p_sVATReg As String
    Private p_sTermnl As String
    Private p_sPermit As String     'Permit No: PR122014-004-D004507-000
    Private p_sSerial As String     'Serial No: L9GF261769
    Private p_sAccrdt As String     'Accrdt No: 038-227471337-000028

    Private p_bExisting As xeLogical
    Private p_nRow As Integer

    Private p_bShowMsg As Boolean
    Private p_sParent As String
    Private p_bNotify As Boolean ' this must be set on configuration table for notifications eg promos

    Private p_sBranchCd As String 'Branch code of the transaction to retrieve
    Private p_sBranchNm As String 'Branch Name of the transaction to retrieve

    Private p_sUserIDxx As String
    Private p_sUserName As String
    Private p_sLogNamex As String
    Private p_nUserLevl As Integer

    Private p_nTotlDisc As Decimal
    Private p_nTotlVatD As Decimal
    Private p_nPWDDIscx As Decimal

    Private p_oDTCharge As DataTable

    Private Const pxeMasTable As String = "Charge_Invoice_Collection_Master"
    Private Const pxeDetTable As String = "Charge_Invoice_Collection_Detail"
    Private Const pxeDelimtr As String = "»"
    Private Const pxeCtrFormat As String = "000000"
    Private Const pxeSourceCde As String = "InCl"       'Charge Invoice Collection 

    Public Event DetailRetrieved(ByVal Row As Integer, _
                                 ByVal Index As Integer, _
                                 ByVal Value As Object)    'Property Detail(Integer)


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


    Public ReadOnly Property ItemCount() As Integer
        Get
            Return p_oDTDetail.Rows.Count
        End Get
    End Property

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

    Public ReadOnly Property ChargeCount() As Integer
        Get
            If IsNothing(p_oDTCharge.Rows.Count) Then
                Return 0
            Else
                Return p_oDTCharge.Rows.Count
            End If
        End Get
    End Property

    Property Master(Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox" : Index = 0
                    Case "dtransact" : Index = 1
                    Case "sclientid" : Index = 2
                    Case "sclientnm" : Index = 3
                    Case "saddressx" : Index = 4
                    Case "ntrantotl" : Index = 5
                    Case "sremarksx" : Index = 6
                    Case "scashrnme" : Index = 7
                    Case "scashierx" : Index = 8
                    Case "ctranstat" : Index = 9
                    Case "smodified" : Index = 10
                    Case "dmodified" : Index = 11
                End Select
            End If
            Return p_oDTMaster(0)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "dtransact" : Index = 1
                        If IsDate(Value) Then
                            p_oDTMaster.Rows(0)(Index) = Value
                        End If
                        Exit Property
                    Case "sclientid" : Index = 2
                    Case "sclientnm" : Index = 3
                    Case "saddressx" : Index = 4
                    Case "sremarksx" : Index = 6
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                End Select
            End If
            p_oDTMaster(0)(Index) = Value
        End Set
    End Property

    Property Detail(ByVal Row As Integer, _
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "nentrynox"
                    Case "ssourcecd"
                    Case "ssourceno"
                    Case "namountxx"    'NET TOTAL FROM SALES(ADD THE VARIOS DISC TO GET THE TRAN TOTAL) 
                    Case "ndiscount"
                    Case "nvatdiscx"
                    Case "npwddiscx"
                    Case "stransnox"
                    Case "dtransact"
                    Case "sinvcenox"
                    Case "sclientnm"
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oDTDetail.Rows(Row)(Index)
        End Get

        Set(ByVal Value As Object)
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "ssourcecd"
                    Case "ssourceno"
                    Case "namountxx"    'NET TOTAL FROM SALES(ADD THE VARIOS DISC TO GET THE TRAN TOTAL)
                        If IsNumeric(Value) Then
                            p_oDTMaster(0).Item("nTranTotl") += (Value - p_oDTDetail(Row).Item(Index))
                            p_oDTDetail.Rows(Row)(Index) = Value
                        End If
                        Exit Property
                    Case "ndiscount"
                        If IsNumeric(Value) Then
                            p_nTotlDisc += (Value - p_oDTDetail(Row).Item(Index))
                            p_oDTDetail.Rows(Row)(Index) = Value
                        End If
                        Exit Property
                    Case "nvatdiscx"
                        If IsNumeric(Value) Then
                            p_nTotlVatD += (Value - p_oDTDetail(Row).Item(Index))
                            p_oDTDetail.Rows(Row)(Index) = Value
                        End If
                        Exit Property
                    Case "npwddiscx"
                        If IsNumeric(Value) Then
                            p_nPWDDIscx += (Value - p_oDTDetail(Row).Item(Index))
                            p_oDTDetail.Rows(Row)(Index) = Value
                        End If
                        Exit Property
                    Case "dtransact", "sinvcenox", "sclientnm", "nbalancex"
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Exit Property
                End Select
            End If
            p_oDTDetail.Rows(Row)(Index) = Value
        End Set
    End Property

    Property Charge(ByVal Row As Integer, _
                    ByVal Index As Object) As Object
        Get
            If Not IsNumeric(Index) Then
                Index = LCase(Index)
                Select Case Index
                    Case "stransnox"
                    Case "dtransact"
                    Case "sclientnm"
                    Case "namountxx"
                    Case "ndiscount"
                    Case "nvatdiscx"
                    Case "npwddiscx"
                    Case "namtpaidx"
                    Case "ssourcecd"
                    Case "ssourceno"
                    Case "ctranstat"
                    Case "sclientid"
                    Case "saddressx"
                    Case Else
                        MsgBox("Invalid Field Detected!!!", MsgBoxStyle.Critical, "WARNING")
                        Return DBNull.Value
                End Select
            End If
            Return p_oDTCharge.Rows(Row)(Index)
        End Get
        Set(ByVal value As Object)
        End Set
    End Property

    Public Function NewTransaction() As Boolean
        If Not loadChargeInvoices() Then Return False

        Call createMaster()
        Call initMaster()
        Call createDetail()

        p_nEditMode = xeEditMode.MODE_ADDNEW

        Return True
    End Function

    Public Function OpenTransaction(ByVal fsTransNox As String) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = AddCondition(getSQ_Master, "a.sTransNox = " & strParm(fsTransNox))

        p_oDTMaster = New DataTable
        p_oDTMaster = p_oApp.ExecuteQuery(lsSQL)

        If p_oDTMaster.Rows.Count = 0 Then Return False

        lsSQL = AddCondition(getSQ_Detail, "a.sTransNox = " & strParm(fsTransNox))
        loDT = New DataTable
        p_oDTDetail = p_oApp.ExecuteQuery(lsSQL)

        If p_oDTDetail.Rows.Count = 0 Then Return False

        Call computeTotal()

        Return True
    End Function

    Public Function SaveTransaction() As Boolean
        If p_oDTMaster(0).Item("cTranStat") = xeTranStat.TRANS_CLOSED Then
            MsgBox("Transaction was already tagged as CLOSED...")
            Return False
        ElseIf p_oDTMaster(0).Item("cTranStat") = xeTranStat.TRANS_POSTED Then
            MsgBox("Transaction was already tagged as POSTED...")
            Return False
        ElseIf p_oDTMaster(0).Item("cTranStat") = xeTranStat.TRANS_CANCELLED Then
            MsgBox("Transaction was already tagged as CANCELLED...")
            Return False
        ElseIf p_oDTMaster(0).Item("cTranStat") = xeTranStat.TRANS_UNKNOWN Then
            MsgBox("UNKNOWN transaction status DETECTED...")
            Return False
        End If

        If (p_oDTDetail(0).Item("sSourceCD") = "" Or p_oDTDetail(0).Item("sSourceNo") = "" Or p_oDTDetail(0).Item("nAmountxx") <= 0) Then
            MsgBox("Invalid Charge Invoice Detail Detected...")
            Return False
        End If

        If (p_oDTMaster(0).Item("sClientNm") = "") Then
            MsgBox("Invalid Client detected Detected...")
            Return False
        End If

        p_oDTMaster(0).Item("sTransNox") = getNextTransNo()

        p_oApp.BeginTransaction()

        Try
            Dim lsSQL As String
            lsSQL = "INSERT INTO " & pxeMasTable & _
                    " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                       ", dTransact = " & dateParm(p_oDTMaster(0).Item("dTransact")) & _
                       ", sClientID = " & strParm(p_oDTMaster(0).Item("sClientID")) & _
                       ", sClientNm = " & strParm(p_oDTMaster(0).Item("sClientNm")) & _
                       ", sAddressx = " & strParm(p_oDTMaster(0).Item("sAddressx")) & _
                       ", nTranTotl = " & p_oDTMaster(0).Item("nTranTotl") & _
                       ", sRemarksx = " & strParm(p_oDTMaster(0).Item("sRemarksx")) & _
                       ", sCashierx = " & strParm(p_oDTMaster(0).Item("sCashierx")) & _
                       ", cTranStat = " & strParm(p_oDTMaster(0).Item("cTranStat")) & _
                       ", sModified = " & strParm(p_oApp.UserID) & _
                       ", dModified = " & datetimeParm(p_oApp.getSysDate)
            Call p_oApp.Execute(lsSQL, pxeMasTable)

            Dim lnCtr As Integer
            For lnCtr = 0 To p_oDTDetail.Rows.Count - 1
                p_oDTDetail(lnCtr).Item("nEntryNox") = lnCtr + 1

                lsSQL = "INSERT INTO " & pxeDetTable & _
                      " SET sTransNox = " & strParm(p_oDTMaster(0).Item("sTransNox")) & _
                         ", nEntryNox = " & p_oDTDetail(lnCtr).Item("nEntryNox") & _
                         ", sSourceCD = " & strParm(p_oDTDetail(lnCtr).Item("sSourceCD")) & _
                         ", sSourceNo = " & strParm(p_oDTDetail(lnCtr).Item("sSourceNo")) & _
                         ", nAmountxx = " & p_oDTDetail(lnCtr).Item("nAmountxx") & _
                         ", dModified = " & datetimeParm(p_oApp.getSysDate)
                Call p_oApp.Execute(lsSQL, pxeDetTable)

                lsSQL = "UPDATE Charge_Invoice SET " & _
                            " cTranStat = " & strParm(xeTranStat.TRANS_CLOSED) & _
                        " WHERE sSourceNo = " & strParm(p_oDTDetail(lnCtr).Item("sSourceNo")) & _
                            " AND sSourceCD = " & strParm(p_oDTDetail(lnCtr).Item("sSourceCD"))

                Call p_oApp.Execute(lsSQL, "Charge_Invoice")
            Next

            p_oApp.CommitTransaction()
        Catch ex As MySqlException
            p_oApp.RollBackTransaction()
            MsgBox(ex.Message)
            Return False
        End Try

        Return True
    End Function

    Public Function PayChargeInvoice()
        If p_oDTMaster(0)("sTransNox") = "" Then Return False

        If p_oDTMaster(0).Item("cTranStat") = xeTranStat.TRANS_CLOSED Then
            MsgBox("Transaction was already tagged as CLOSED...")
            Return False
        ElseIf p_oDTMaster(0).Item("cTranStat") = xeTranStat.TRANS_POSTED Then
            MsgBox("Transaction was already tagged as POSTED...")
            Return False
        ElseIf p_oDTMaster(0).Item("cTranStat") = xeTranStat.TRANS_CANCELLED Then
            MsgBox("Transaction was already tagged as CANCELLED...")
            Return False
        ElseIf p_oDTMaster(0).Item("cTranStat") = xeTranStat.TRANS_UNKNOWN Then
            MsgBox("UNKNOWN transaction status DETECTED...")
            Return False
        End If

        Dim loPayment As Receipt
        loPayment = New Receipt(p_oApp)

        With loPayment
            .POSNumbr = p_sTermnl
            .SourceCd = pxeSourceCde
            .SourceNo = p_oDTMaster(0).Item("sTransNox")
            .NonVAT = 0
            .DiscAmount = p_nTotlDisc
            .PosDate = p_oApp.getSysDate
        End With

        Return True
    End Function

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

    Private Function createMaster() As Boolean
        p_oDTMaster = New DataTable("Master")

        p_oDTMaster.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTMaster.Columns.Add("dTransact", System.Type.GetType("System.DateTime"))
        p_oDTMaster.Columns.Add("sClientID", System.Type.GetType("System.String")).MaxLength = 12
        p_oDTMaster.Columns.Add("sClientNm", System.Type.GetType("System.String")).MaxLength = 120
        p_oDTMaster.Columns.Add("sAddressx", System.Type.GetType("System.String")).MaxLength = 128
        p_oDTMaster.Columns.Add("nTranTotl", System.Type.GetType("System.Decimal"))
        p_oDTMaster.Columns.Add("sRemarksx", System.Type.GetType("System.String")).MaxLength = 128
        p_oDTMaster.Columns.Add("sCashrNme", System.Type.GetType("System.String")).MaxLength = 64
        p_oDTMaster.Columns.Add("sCashierx", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("cTranStat", System.Type.GetType("System.String")).MaxLength = 1
        p_oDTMaster.Columns.Add("sModified", System.Type.GetType("System.String")).MaxLength = 10
        p_oDTMaster.Columns.Add("dModified", System.Type.GetType("System.DateTime"))
        Return True
    End Function

    Private Function initMaster() As Boolean
        p_oDTMaster.Rows.Add()
        p_oDTMaster(0).Item("sTransNox") = getNextTransNo()
        p_oDTMaster(0).Item("dTransact") = p_oApp.getSysDate
        p_oDTMaster(0).Item("sClientID") = ""
        p_oDTMaster(0).Item("sClientNm") = ""
        p_oDTMaster(0).Item("sAddressx") = ""
        p_oDTMaster(0).Item("nTranTotl") = 0.0
        p_oDTMaster(0).Item("sRemarksx") = ""
        p_oDTMaster(0).Item("sCashrNme") = p_oApp.UserName
        p_oDTMaster(0).Item("sCashierx") = p_oApp.UserID
        p_oDTMaster(0).Item("cTranStat") = "0"

        p_nTotlDisc = 0
        p_nTotlVatD = 0
        p_nPWDDIscx = 0

        Return True
    End Function

    Private Function createDetail() As Boolean
        p_oDTDetail = New DataTable("Detail")
        p_oDTDetail.Columns.Add("nEntryNox", System.Type.GetType("System.Int16")).AutoIncrement = True
        p_oDTDetail.Columns.Add("sSourceCD", System.Type.GetType("System.String")).MaxLength = 4
        p_oDTDetail.Columns.Add("sSourceNo", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTDetail.Columns.Add("nAmountxx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nDiscount", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nAddDiscx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nVatDiscx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("nPWDDiscx", System.Type.GetType("System.Decimal"))
        p_oDTDetail.Columns.Add("dModified", System.Type.GetType("System.DateTime"))
        p_oDTDetail.Columns.Add("sTransNox", System.Type.GetType("System.String")).MaxLength = 20
        p_oDTDetail.Columns.Add("dTransact", System.Type.GetType("System.DateTime"))
        p_oDTDetail.Columns.Add("sClientNm", System.Type.GetType("System.String")).MaxLength = 120
        p_oDTDetail.Columns.Add("sInvceNox", System.Type.GetType("System.String")).MaxLength = 8
        Return True
    End Function

    Public Function AddDetail(Optional ByVal fnRowPos As Integer = -1) As Boolean
        Return newDetail(fnRowPos)
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
                p_oDTDetail.Rows.InsertAt(p_oDTDetail.NewRow, fnRowPos)
                p_nRow = fnRowPos
            End If
        End If

        p_oDTDetail(p_nRow).Item("sSourceCD") = ""
        p_oDTDetail(p_nRow).Item("sSourceNo") = ""
        p_oDTDetail(p_nRow).Item("nAmountxx") = 0.0

        p_oDTDetail(p_nRow).Item("nDiscount") = 0.0
        p_oDTDetail(p_nRow).Item("nVatDiscx") = 0.0
        p_oDTDetail(p_nRow).Item("nPWDDiscx") = 0.0

        p_oDTDetail(p_nRow).Item("nEntryNox") = p_nRow
        p_oDTDetail(p_nRow).Item("dModified") = p_oApp.SysDate

        p_oDTDetail(p_nRow).Item("dTransact") = p_oApp.SysDate
        p_oDTDetail(p_nRow).Item("sClientNm") = ""
        p_oDTDetail(p_nRow).Item("sInvceNox") = ""

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

    Private Sub computeTotal()
        Dim lnLoc As Integer
        Dim lnTotal As Decimal

        p_nTotlDisc = 0
        p_nTotlVatD = 0
        p_nPWDDIscx = 0

        For lnLoc = 0 To p_oDTDetail.Rows.Count - 1
            lnTotal += p_oDTDetail(lnLoc).Item("nAmountxx")
            p_nTotlDisc += p_oDTDetail(lnLoc).Item("nDiscount")
            p_nTotlVatD += p_oDTDetail(lnLoc).Item("nVatDiscx")
            p_nPWDDIscx += p_oDTDetail(lnLoc).Item("nPWDDiscx")
        Next

        p_oDTMaster(0).Item("nTranTotl") = lnTotal
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
                " FROM " & pxeMasTable & _
                " WHERE sTransNox LIKE " & strParm(p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy") & "%") & _
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

            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)
        Else
            lsSQL = p_sBranchCd & p_sTermnl & Format(p_oApp.getSysDate(), "yy")
            lnCounter = Len(lsSQL)

            lsSQL = loDT.Rows(0).Item("sTransNox")
            lnLen = Len(lsSQL)

            lnCode = CLng(Mid(lsSQL, lnCounter + 1)) + 1
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

    Private Function getUserName(ByVal fsUserIDxx) As String
        Dim loDT As DataTable

        loDT = p_oApp.ExecuteQuery("SELECT sUserName FROM xxxSysUser WHERE sUserIDxx = " & strParm(fsUserIDxx))

        If loDT.Rows.Count = 0 Then
            Return ""
        Else
            Return loDT(0)("sUserName")
        End If
    End Function

    Private Function loadChargeInvoices() As Boolean
        Dim lsSQL As String

        lsSQL = AddCondition(getSQ_Charge, "a.cTranStat = 0")

        p_oDTCharge = p_oApp.ExecuteQuery(lsSQL)

        If p_oDTCharge.Rows.Count <= 0 Then
            MsgBox("There are no unpaid charge invoice on record.", MsgBoxStyle.Information, "Notice")

            p_oDTCharge = Nothing

            Return False
        End If

        Return True
    End Function

    Private Function getSQ_Charge() As String
        Return _
            "SELECT" & _
                "  a.sTransNox" & _
                ", a.sClientID" & _
                ", a.cBilledxx" & _
                ", a.dBilledxx" & _
                ", a.cPaidxxxx" & _
                ", a.dPaidxxxx" & _
                ", a.cWaivexxx" & _
                ", a.dWaivexxx" & _
                ", a.sWaivexxx" & _
                ", a.nAmountxx" & _
                ", a.nDiscount" & _
                ", a.nVatDiscx" & _
                ", a.nPWDDiscx" & _
                ", a.nAmtPaidx" & _
                ", a.sSourceCd" & _
                ", a.sSourceNo" & _
                ", a.cTranStat" & _
                ", IF(IFNULL(a.sClientID, '') = '', a.sClientNm, b.sCompnyNm) sClientNm" & _
                ", IF(IFNULL(b.sAddressx, '') = '', a.sAddressx, b.sAddressx) sAddressx" & _
                ", a.sModified" & _
                ", a.dModified" & _
                ", c.dTransact" & _
            " FROM Charge_Invoice a" & _
                " LEFT JOIN Client_Master b" & _
                    " ON a.sClientID = b.sClientID" & _
                " LEFT JOIN SO_Master c" & _
                    " ON a.sSourceCd = 'SO'" & _
                        " AND a.sSourceNo = c.sTransNox"
    End Function

    'Returns the SQL for the Management of the Master Part of the Transaction
    Private Function getSQ_Master() As String
        Return _
            "SELECT a.sTransNox" & _
                ", a.dTransact" & _
                ", a.sClientID" & _
                ", a.sClientNm" & _
                ", a.sAddressx" & _
                ", a.nTranTotl" & _
                ", a.sRemarksx" & _
                ", b.sUserName sCashrNme" & _
                ", a.sCashierx" & _
                ", a.cTranStat" & _
                ", a.sModified" & _
                ", a.dModified" & _
            " FROM " & pxeMasTable & " a" & _
                " LEFT JOIN xxxSysUser b ON a.sCashierx = b.sUserIDxx"
    End Function

    'Returns the SQL for the Management of the Master Part of the Transaction
    Private Function getSQ_Detail() As String
        Return _
            "SELECT a.nEntryNox" & _
                ", a.sSourceCD" & _
                ", a.sSourceNo" & _
                ", a.nAmountxx" & _
                ", b.nDiscount" & _
                ", b.nVatDiscx" & _
                ", b.nPWDDiscx" & _
                ", a.dModified" & _
                ", a.sTransNox" & _
            " FROM " & pxeDetTable & " a" & _
                " LEFT JOIN Charge_Invoice b ON a.sSourceNo = b.sTransNox AND a.sSourceCD = " & strParm(pxeSourceCde) & _
            " ORDER BY a.nEntryNox"
    End Function

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        p_oDTMaster = Nothing
        p_oDTDetail = Nothing

        p_bShowMsg = False

        p_sPOSNo = Environment.GetEnvironmentVariable("RMS-CRM-No")
        p_sVATReg = Environment.GetEnvironmentVariable("REG-TIN-No")

        Call InitMachine()

        p_sBranchCd = p_oApp.BranchCode

        Call createMaster()
        Call initMaster()
        Call createDetail()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
