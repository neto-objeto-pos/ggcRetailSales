'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     Table Manager Object
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
'  iMac [ 01/31/2017 04:00 pm ]
'      Started creating this object.
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports MySql.Data.MySqlClient
Imports ggcAppDriver

Public Class TableMaster
    Private Const pxeMasterTble As String = "Table_Master"

    Private p_oApp As GRider
    Private p_oDTMstr As DataTable
    Private p_sTerminal As String
    Private p_oSalesOrder As New_Sales_Order

    Private p_nEditMode As xeEditMode

    Private p_sParent As String
    Private p_sPOSNo As String
    Private p_sBranchCd As String
    Private p_nTable As Integer
    Private p_bSChargex As Boolean
    Private p_sTableNox As String

    Private Const p_sMasTable As String = "Table_Master"

    Private Const p_sMsgHeadr As String = "Table Manager"

    Public Event MasterRetrieve(ByVal lnIndex As Integer)

    Public Property WithSCharge() As Boolean
        Get
            Return p_bSChargex
        End Get
        Set(ByVal Value As Boolean)
            p_bSChargex = Value
        End Set
    End Property
    Public WriteOnly Property SalesOrder
        Set(foSalesOrder)
            p_oSalesOrder = foSalesOrder
        End Set
    End Property

    Property Terminal() As String
        Get
            Return p_sTerminal
        End Get
        Set(ByVal value As String)
            p_sTerminal = value
        End Set
    End Property

    Property TableNo() As String
        Get
            Return p_sTableNox
        End Get
        Set(ByVal Value As String)
            p_sTableNox = Value
        End Set
    End Property

    Public Property Master(ByVal fvIndex As Object) As Object
        Get
            If Not IsNumeric(fvIndex) Then fvIndex = LCase(fvIndex)

            Select Case fvIndex
                Case "ntablenox" : fvIndex = 0
                Case "cstatusxx" : fvIndex = 1
                Case "noccupnts" : fvIndex = 2
                Case "ncapacity" : fvIndex = 3
                Case "dreserved" : fvIndex = 4
                Case Else
                    MsgBox("Invalid Index Detected. Please verify your entry.", MsgBoxStyle.Critical, p_sMsgHeadr)
            End Select

            Return p_oDTMstr(0)(fvIndex)
        End Get

        Set(value)
            If Not IsNumeric(fvIndex) Then fvIndex = LCase(fvIndex)
            Select Case fvIndex
                Case "cstatusxx"
                    fvIndex = 1

                    If value = 0 Then
                        p_oDTMstr(0)("nOccupnts") = 0
                        RaiseEvent MasterRetrieve(2)
                    Else
                        If p_oDTMstr(0)(fvIndex) <> "0" And value = 2 Then
                            MsgBox("Unable to tag un-empty table to reserved.", MsgBoxStyle.Information, "Notice")
                            Exit Property
                        Else
                            p_oDTMstr(0)("dReserved") = p_oApp.getSysDate
                        End If
                    End If
                Case "noccupnts" : fvIndex = 2
                Case "dreserved" : fvIndex = 4
                    If Not IsDate(value) Then value = p_oApp.getSysDate
                Case Else
                    MsgBox("Invalid Index Detected. Please verify your entry.", MsgBoxStyle.Critical, p_sMsgHeadr)
            End Select

            p_oDTMstr(0)(fvIndex) = value

            RaiseEvent MasterRetrieve(fvIndex)
        End Set
    End Property

    Public Sub New(ByVal foRider As GRider)
        p_oApp = foRider

        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oApp.BranchCode

        p_oDTMstr = Nothing

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub

    Private Function getSQL_Master() As String
        Return "SELECT" & _
                    "  nTableNox" & _
                    ", cStatusxx" & _
                    ", IFNULL(nOccupnts, 0) nOccupnts" & _
                    ", IFNULL(nCapacity, 0) nCapacity" & _
                    ", IFNULL(dReserved, SYSDATE()) dReserved" & _
                " FROM Table_Master"
    End Function

    Function OpenTable(ByVal fnTableNo As Integer) As Boolean
        Try
            Dim lsSQL As String
            lsSQL = AddCondition(getSQL_Master, "nTableNox = " & fnTableNo)
            Debug.Print(lsSQL)
            p_oDTMstr = p_oApp.ExecuteQuery(lsSQL)
        Catch ex As MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End Try

        If p_oDTMstr.Rows.Count = 0 Then
            p_nTable = -1
            Return False
        Else
            p_nTable = fnTableNo
        End If

        Return True
    End Function

    Private Function isEntryOK() As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        Try
            If p_oDTMstr(0)("cStatusxx") = 0 Then
                lsSQL = "SELECT sTransNox FROM SO_Master" & _
                " WHERE sTableNox = " & p_nTable & _
                " AND cTranStat = 0"

                loDT = p_oApp.ExecuteQuery(lsSQL)

                If loDT.Rows.Count > 0 Then
                    If MsgBox("Open Order Exists for this table. Do you want to continue?", _
                              MsgBoxStyle.YesNo, p_sMsgHeadr) = MsgBoxResult.No Then
                        Return False
                    End If
                End If
            End If
        Catch ex As MySqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End Try

        Return True
    End Function

    Function SaveTable() As Boolean
        Dim lsSQL As String
        Try
            If Not isEntryOK() Then Return False

            If p_oDTMstr(0)("cStatusxx") = "2" Then
                lsSQL = "UPDATE " & p_sMasTable &
                    " SET  cStatusxx = " & strParm(p_oDTMstr(0)("cStatusxx")) &
                        ", dReserved = " & datetimeParm(p_oDTMstr(0)("dReserved")) &
                        ", nOccupnts = " & p_oDTMstr(0)("nOccupnts") &
                " WHERE nTableNox = " & p_oDTMstr(0)("nTableNox")
            Else
                lsSQL = "UPDATE " & p_sMasTable &
                    " SET  cStatusxx = " & strParm(p_oDTMstr(0)("cStatusxx")) &
                        ", dReserved = NULL " &
                        ", nOccupnts = " & IIf(p_oDTMstr(0)("cStatusxx") = "1", p_oDTMstr(0)("nOccupnts"), 0) &
                " WHERE nTableNox = " & p_oDTMstr(0)("nTableNox")
            End If

            If p_oApp.Execute(lsSQL, p_sMasTable) = 0 Then
                MsgBox("Unable to Update Table.", MsgBoxStyle.Critical, p_sMsgHeadr)
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, p_sMsgHeadr)
            Return False
        End Try

        Return True
    End Function

    Function showManageTable() As Boolean
        Dim loForm As frmManageTable

        loForm = New frmManageTable
        With loForm
            .TableMaster = Me
            .ShowDialog()

            Return Not .Cancelled
        End With
    End Function

    Function showTables() As Boolean
        Dim loForm As frmTables

        loForm = New frmTables(p_oApp)
        loForm.Terminal = p_sTerminal
        With loForm
            .TableMaster = Me
            .ShowDialog()

            Return Not .Cancelled
        End With
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

        lsSQL = AddCondition("UPDATE Table_Master SET cStatusxx = '1', nOccupnts =" & IFNull(p_oDTMstr(0).Item("nOccupnts"), 0), lsSQL)
        Debug.Print(lsSQL)
        If lsSQL <> "" Then p_oApp.Execute(lsSQL, "Table_Master")

        Return True
    End Function

    Public Function reservedTable(ByVal fsTableNo As String)
        If OpenTable(CInt(fsTableNo)) Then
            p_oDTMstr(0)("cStatusxx") = 2
            p_oDTMstr(0)("dReserved") = p_oApp.getSysDate
            If SaveTable() Then
                Return p_oSalesOrder.saveOrder(fsTableNo)
            End If
        End If

        Return False
    End Function
End Class