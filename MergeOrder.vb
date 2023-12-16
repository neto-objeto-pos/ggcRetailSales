'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
' Guanzon Software Engineering Group
' Guanzon Group of Companies
' Perez Blvd., Dagupan City
'
'     RetMgtSys Merge Order
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
'  Jheff [ 10/12/2016 02:58 pm ]
'     Start coding this object...
'€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
Imports ggcAppDriver
Imports System.Windows.Forms
Imports System.Reflection

Public Class MergeOrder

#Region "Constant"
    Private Const pxeMODULENAME As String = "MergeOrder"
    Private Const pxeMasterTble As String = "SO_Master"
#End Region
    Private p_sTerminal As String

#Region "Protected Members"
    Protected p_oAppDrvr As GRider
    Protected p_oDataTable As DataTable
    Protected p_nEditMode As xeEditMode
    Protected p_oFormMergeTable As frmMergeOrder

    Protected p_sBranchCd As String
    Protected p_bCancelled As Boolean
#End Region

#Region "Properties"
    WriteOnly Property Terminal() As String
        Set(ByVal value As String)
            p_sTerminal = value
        End Set
    End Property
    ReadOnly Property AppDriver As GRider
        Get
            Return p_oAppDrvr
        End Get
    End Property

    Property Branch() As String
        Get
            Return p_sBranchCd
        End Get
        Set(ByVal Value As String)
            p_sBranchCd = Value
        End Set
    End Property
#End Region

#Region "Public Function"
    Function GetTables() As DataTable
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = "SELECT" & _
                    "  a.nTableNox" & _
                    ", a.cStatusxx" & _
                    ", a.dReserved" & _
                    ", a.nCapacity" & _
                    ", a.nOccupnts" & _
                    ", b.nContrlNo" & _
                    ", b.sWaiterID" & _
                    ", b.sMergeIDx" & _
                    ", b.cTranStat" & _
                " FROM Table_Master a" & _
                    " LEFT JOIN SO_Master b" & _
                        " ON a.nTableNox = b.sTableNox" & _
                            " AND b.cTranStat = '0'" & _
                " ORDER BY a.nTableNox"

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then MsgBox("No tables on record!", MsgBoxStyle.Critical, "Warning")

        Return loDT
    End Function

    Function GetTables(ByVal fsMergeIDx As String) As DataTable
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = "SELECT" & _
                    "  a.nTableNox" & _
                    ", a.cStatusxx" & _
                    ", a.dReserved" & _
                    ", a.nCapacity" & _
                    ", a.nOccupnts" & _
                    ", b.nContrlNo" & _
                    ", b.sWaiterID" & _
                    ", b.sMergeIDx" & _
                    ", b.cTranStat" & _
                " FROM Table_Master a" & _
                    " LEFT JOIN SO_Master b" & _
                        " ON a.nTableNox = b.sTableNox" & _
                            " AND b.cTranStat = '0'" & _
                " WHERE b.sMergeIDx = " & strParm(fsMergeIDx) & _
                " ORDER BY a.nTableNox"

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return Nothing

        Return loDT
    End Function


    Public Function getTerminal(ByVal fnTableNo As Integer) As String
        Dim lsSQL As String
        Dim lsTerminal As String
        Dim loDTa As DataTable
        lsSQL = "SELECT sTransNox FROM SO_Master WHERE cTranStat = '0' AND sTableNox = " & strParm(fnTableNo)
        loDTa = p_oAppDrvr.ExecuteQuery(lsSQL)
        If loDTa.Rows.Count = 1 Then
            lsTerminal = loDTa(0).Item("sTransNox")
            Select Case lsTerminal.Substring(0, 6)
                Case "P00101"
                    lsTerminal = "01"
                Case "P00102"
                    lsTerminal = "02"
                Case "P00103"
                    lsTerminal = "03"
                Case "P00104"
                    lsTerminal = "04"
                Case Else
                    lsTerminal = "05"
            End Select
        Else
            lsTerminal = ""
        End If

        Return lsTerminal
    End Function

    Function GetTable(ByVal fsTableNox As String) As DataTable
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = "SELECT" & _
                    "  a.nTableNox" & _
                    ", a.cStatusxx" & _
                    ", a.dReserved" & _
                    ", a.nCapacity" & _
                    ", a.nOccupnts" & _
                    ", b.nContrlNo" & _
                    ", b.sWaiterID" & _
                    ", b.sMergeIDx" & _
                    ", b.cTranStat" & _
                " FROM Table_Master a" & _
                    " LEFT JOIN SO_Master b" & _
                        " ON a.nTableNox = b.sTableNox" & _
                " WHERE a.nTableNox = " & strParm(Trim(fsTableNox)) & _
                " ORDER BY a.nTableNox"

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return Nothing

        Return loDT
    End Function

    Function GetTableTrans(ByVal fsTableNox As String) As DataTable
        Dim loDT As DataTable
        Dim lsSQL As String

        lsSQL = "SELECT sTransNox " & _
                " FROM SO_Master " & _
                " WHERE sTableNox LIKE " & strParm("%" & fsTableNox & "%") & _
                    " AND cTranStat = '0'"

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)

        If loDT.Rows.Count = 0 Then Return Nothing

        Return loDT
    End Function
#End Region

#Region "Private function"
    Function SaveTransaction(ByVal TransNox As String, _
                                     ByVal MergeIDx As String) As Boolean
        Dim lsSQL As String
        Dim lnRow As Integer
        Dim lsNeoMergeID As String
        Dim lnCtr As Integer
        Dim lsTransNox() As String
        Dim lsMergeIDx() As String
        Dim loDt As DataTable

        With p_oDataTable
            If p_bCancelled Then Return False

            If MergeIDx <> "" Then
                lsMergeIDx = Split(MergeIDx, "»")
                For lnCtr = 0 To UBound(lsMergeIDx)
                    'merge id was set to last first chars in the interface; also in numeric format
                    If Len(lsMergeIDx(lnCtr)) = 4 And IsNumeric(lsMergeIDx(lnCtr)) Then
                        lsSQL = "SELECT * FROM " & pxeMasterTble & _
                                " WHERE sMergeIDx LIKE " & strParm("%" & lsMergeIDx(lnCtr))

                        loDt = New DataTable
                        loDt = p_oAppDrvr.ExecuteQuery(lsSQL)
                        If Not loDt.Rows.Count = 0 Then
                            For nCtr As Integer = 0 To loDt.Rows.Count - 1
                                Dim oDt As New DataTable
                                lsSQL = "SELECT * FROM Discount" & _
                                            " WHERE sTransNox = " & strParm(loDt.Rows(nCtr)("sTransNox"))

                                oDt = p_oAppDrvr.ExecuteQuery(lsSQL)
                                If Not oDt.Rows.Count = 0 Then
                                    lsSQL = "DELETE FROM Discount" & _
                                                " WHERE sTransNox = " & strParm(oDt.Rows(0)("sTransNox"))

                                    Try
                                        lnRow = p_oAppDrvr.Execute(lsSQL, "Discount")
                                        If lnRow <= 0 Then
                                            MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                                    "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                            Return False
                                        End If
                                    Catch ex As Exception
                                        Throw ex
                                    End Try
                                End If
                            Next
                        End If

                        lsSQL = "UPDATE " & pxeMasterTble & " SET" & _
                                    " sMergeIDx = " & strParm("") & _
                                " WHERE sMergeIDx LIKE " & strParm("%" & lsMergeIDx(lnCtr))

                        Try
                            lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                            'If lnRow <= 0 Then
                            '    MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                            '            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                            '    Return False
                            'End If
                        Catch ex As Exception
                            Throw ex
                        End Try

                    End If
                Next lnCtr
            End If

            lsNeoMergeID = getNextMergeID()

            If Right(TransNox, 1) = "»" Then TransNox = Left(TransNox, Len(TransNox) - 1)
            If Left(TransNox, 1) = "»" Then TransNox = Right(TransNox, Len(TransNox) - 1)

            lsTransNox = Split(TransNox, "»")
            If UBound(lsTransNox) >= 1 Then
                For lnCtr = 0 To UBound(lsTransNox)
                    If lsTransNox(lnCtr) <> "" Then
                        lsSQL = "UPDATE " & pxeMasterTble & " SET" & _
                                    " sMergeIDx = " & strParm(lsNeoMergeID) & _
                                "WHERE sTransNox = " & strParm(lsTransNox(lnCtr))

                        Try
                            lnRow = p_oAppDrvr.Execute(lsSQL, pxeMasterTble)
                            If lnRow <= 0 Then
                                MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                        "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                Return False
                            End If
                        Catch ex As Exception
                            Throw ex
                        End Try

                        Dim oDt As New DataTable
                        lsSQL = "SELECT * FROM Discount" & _
                                    " WHERE sTransNox = " & strParm(lsTransNox(lnCtr))

                        oDt = p_oAppDrvr.ExecuteQuery(lsSQL)
                        If Not oDt.Rows.Count = 0 Then
                            lsSQL = "DELETE FROM Discount" & _
                                        " WHERE sTransNox = " & strParm(oDt.Rows(0)("sTransNox"))

                            Try
                                lnRow = p_oAppDrvr.Execute(lsSQL, "Discount")
                                If lnRow <= 0 Then
                                    MsgBox("Unable to Save Transaction!!!" & vbCrLf & _
                                            "Please contact GGC SSG/SEG for assistance!!!", MsgBoxStyle.Critical, "WARNING")
                                    Return False
                                End If
                            Catch ex As Exception
                                Throw ex
                            End Try
                        End If
                    End If
                Next lnCtr
            End If
        End With

        Return True
    End Function

    Private Function getNextMergeID() As String
        Dim lsSQL As String
        Dim loDT As DataTable
        Dim lnCtr As Integer

        lsSQL = "SELECT sMergeIDx" & _
                " FROM " & pxeMasterTble & _
                " WHERE sMergeIDx LIKE " & strParm(p_sBranchCd & Format(p_oAppDrvr.getSysDate(), "yy") & "%") & _
                " ORDER BY sMergeIDx DESC" & _
                " LIMIT 1"

        loDT = p_oAppDrvr.ExecuteQuery(lsSQL)
        If loDT.Rows.Count = 0 Then
            lnCtr = 1
        Else
            lnCtr = Mid(loDT(0).Item(0), Len(p_sBranchCd & "yy") + 1) + 1
        End If

        Return p_sBranchCd & Format(p_oAppDrvr.getSysDate(), "yy") & Format(lnCtr, "0000")
    End Function
#End Region

#Region "Public Procedures"
    Sub ShowMergeTable()
        p_oFormMergeTable = New frmMergeOrder
        With p_oFormMergeTable
            .Terminal = p_sTerminal
            .Merge = Me
            .TopMost = True
            .ShowDialog()

            p_bCancelled = .Cancelled
        End With
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New(ByVal foRider As GRider)
        p_oAppDrvr = foRider

        If p_sBranchCd = String.Empty Then p_sBranchCd = p_oAppDrvr.BranchCode

        p_nEditMode = xeEditMode.MODE_UNKNOWN
    End Sub
End Class