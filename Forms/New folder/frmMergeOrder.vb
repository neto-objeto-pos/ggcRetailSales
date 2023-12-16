Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms

Imports ggcAppDriver

Public Class frmMergeOrder
    Private Const pxeMaxTable As Integer = 50
    Private Const pxeOccupiedTag As String = "Y"
    Private Const pxeUnoccupdTag As String = "N"
    Private Const pxeGrpdTbleTag As String = "G"
    Private Const pxeAddedTblTag As String = "A"

    Private poMerge As MergeOrder

    Private pnLoadx As Integer
    Private poControl As Control
    Private pbCancelled As Boolean
    Private psMergeTable As String
    Private psAddToMerge As String
    Private psMergeTrans As String
    Private psAddMgTrans As String

    Enum xeTableStatus
        xeEmpty = 0
        xeOccupied = 1
        xeReserved = 2
        xeDirty = 3
        xeNONE = 4
    End Enum

    WriteOnly Property Merge() As MergeOrder
        Set(ByVal oMerge As MergeOrder)
            poMerge = oMerge
        End Set
    End Property

    ReadOnly Property Cancelled() As Boolean
        Get
            Return pbCancelled
        End Get
    End Property

    Private Sub frmPayGC_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Close()
                Me.Dispose()
                pbCancelled = True
        End Select
    End Sub

    Private Sub frmPay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        setVisible()

        If pnLoadx = 0 Then
            clearFields()
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)
            Call grpEventHandler(Me, GetType(Label), "lblTable", "Click", AddressOf lblTable_Click)
            Call grpEventHandler(Me, GetType(Label), "lblTable", "DoubleClick", AddressOf lblTable_DoubleClick)

            loadTable()

            pnLoadx = 1
        End If
    End Sub

    Private Sub frmPay_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        setVisible()
    End Sub

    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 0 'Save
                Dim lsStr1 As String = Mid(psMergeTrans, 2)
                Dim lsStr2 As String = Mid(psMergeTable, 2)

                If psAddMgTrans <> "" Then
                    lsStr1 = Mid(psAddMgTrans, 2) & IIf(lsStr1 = "", "", "»" & lsStr1)
                End If

                If psAddToMerge <> "" Then
                    lsStr2 = Mid(psAddToMerge, 2) & IIf(lsStr2 = "", "", "»" & lsStr2)
                End If

                If poMerge.SaveTransaction(lsStr1, lsStr2) Then loadTable()
            Case 1 'Cancel
                Me.Close()
            Case 2 'Refresh
                loadTable()
        End Select
endProc:
        Exit Sub
    End Sub

    'this will set table numbers and set them to empty
    Private Sub initTable()
        Dim lnCtr As Integer
        Dim loLbl As Label
        Dim loLbl2 As Label

        For lnCtr = 0 To pxeMaxTable - 1
            loLbl = CType(FindLabel(Me, "lblTable" & Format(lnCtr, "00")), Label)
            loLbl.Text = ""

            loLbl2 = CType(FindLabel(Me, "lblRefer" & Format(lnCtr, "00")), Label)
            loLbl2.Text = ""

            markTable(lnCtr, xeTableStatus.xeNONE)
        Next

        psMergeTable = ""
        psAddToMerge = ""
        psMergeTrans = ""
        psAddMgTrans = ""
    End Sub

    Private Sub loadTable()
        Dim loDT As DataTable
        Dim lnCtr As Integer
        Dim loLbl As Label
        Dim loRef As Label

        loDT = poMerge.GetTables

        Call initTable()

        For lnCtr = 0 To pxeMaxTable - 1
            loLbl = CType(FindLabel(Me, "lblTable" & Format(lnCtr, "00")), Label)
            loRef = CType(FindLabel(Me, "lblRefer" & Format(lnCtr, "00")), Label)

            If lnCtr <= loDT.Rows.Count - 1 Then
                loLbl.Text = loDT(lnCtr)("nTableNox")

                If CInt(loDT(lnCtr)("cStatusxx")) = xeTableStatus.xeOccupied Then
                    loRef.Text = Strings.Right(IIf(IFNull(loDT(lnCtr)("sMergeIDx"), "") = "", _
                                                   loDT(lnCtr)("nContrlNo"), _
                                                   loDT(lnCtr)("sMergeIDx")), 4)
                Else
                    loRef.Text = ""
                End If

                markTable(lnCtr, CInt(loDT(lnCtr)("cStatusxx")))
            Else
                markTable(lnCtr, xeTableStatus.xeNONE)
            End If
        Next
    End Sub

    'fsTables is based on label index 0 to max table, not the given table number
    Private Sub markTable(ByVal fsTables As String, ByVal fsStatus As xeTableStatus)
        Dim lvBackColor As Color
        Dim lvForeColor As Color

        Select Case fsStatus
            Case xeTableStatus.xeEmpty
                lvBackColor = Color.White
                lvForeColor = Color.Green
            Case xeTableStatus.xeOccupied
                lvBackColor = Color.Green
                lvForeColor = Color.White
            Case xeTableStatus.xeReserved
                lvBackColor = Color.Yellow
                lvForeColor = Color.White
            Case xeTableStatus.xeDirty
                lvBackColor = Color.Red
                lvForeColor = Color.White
            Case xeTableStatus.xeNONE
                lvBackColor = Color.Black
                lvForeColor = Color.Black
        End Select

        Dim lasDetail() As String = Split(fsTables, ",")
        Dim lnCtr As Integer
        Dim loLbl As Label
        Dim loLbl2 As Label

        For lnCtr = 0 To UBound(lasDetail)
            loLbl = CType(FindLabel(Me, "lblTable" & Format(CInt(lasDetail(lnCtr)), "00")), Label)
            loLbl2 = CType(FindLabel(Me, "lblRefer" & Format(CInt(lasDetail(lnCtr)), "00")), Label)

            loLbl.BackColor = lvBackColor
            loLbl.ForeColor = lvForeColor
            loLbl.Tag = IIf(fsStatus = xeTableStatus.xeOccupied, pxeOccupiedTag, pxeUnoccupdTag)

            loLbl2.BackColor = lvBackColor
            loLbl2.ForeColor = lvForeColor
        Next
    End Sub

    Private Sub setVisible()
        Me.BackColor = Color.Black
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        Me.Visible = False
        Me.Location = New Point(190, 110)
        Me.BackgroundImage = Nothing
        Me.TransparencyKey = Me.BackColor
        Me.Visible = True
    End Sub

    Private Sub showGroup(ByVal fnTableNox As String, fnIndex As Integer)
        Dim loDT As DataTable
        loDT = poMerge.GetTable(Str(fnTableNox))

        If IsNothing(loDT) Then GoTo endProc 'table no is non existent

        Dim lnTableNox As Integer = loDT(0)("nTableNox")

        If IFNull(loDT(0)("sMergeIDx"), "") = "" Then GoTo initialItem

        loDT = poMerge.GetTables(loDT(0)("sMergeIDx"))

        Dim loLbl As Label
        Dim loRef As Label
        Dim loDT2 As DataTable

        If IsNothing(loDT) Then
initialItem:
            'no merge id
            loLbl = CType(FindLabel(Me, "lblTable" & Format(fnIndex, "00")), Label)

            loLbl.BackColor = Color.Blue
            loLbl.ForeColor = Color.White
            loLbl.Tag = pxeAddedTblTag

            loRef = CType(FindLabel(Me, "lblRefer" & Format(fnIndex, "00")), Label)
            loRef.BackColor = Color.Blue
            loRef.ForeColor = Color.White
            loRef.Text = "ADDED"

            loDT2 = poMerge.GetTableTrans(loLbl.Text)
            If Not IsNothing(loDT2) Then
                psAddToMerge = psAddToMerge & "»" & Trim(loLbl.Text)
                psAddMgTrans = psAddMgTrans & "»" & Trim(loDT2(0)("sTransNox"))
            End If
        Else
            Dim lnCtr As Integer
            Dim lnRow As Integer

            For lnRow = 0 To loDT.Rows.Count - 1
                For lnCtr = 0 To pxeMaxTable - 1
                    loLbl = CType(FindLabel(Me, "lblTable" & Format(lnCtr, "00")), Label)

                    If loDT(lnRow)("nTableNox").ToString = Trim(loLbl.Text) Then 'matched
                        loLbl.BackColor = Color.Blue
                        loLbl.ForeColor = Color.White
                        loLbl.Tag = pxeGrpdTbleTag

                        loRef = CType(FindLabel(Me, "lblRefer" & Format(lnCtr, "00")), Label)
                        loRef.BackColor = Color.Blue
                        loRef.ForeColor = Color.White

                        loDT2 = poMerge.GetTableTrans(loLbl.Text)
                        If Not IsNothing(loDT2) Then
                            psMergeTable = psMergeTable & "»" & Trim(loRef.Text)
                            psMergeTrans = psMergeTrans & "»" & Trim(loDT2(0)("sTransNox"))
                        End If

                        Exit For
                    End If
                Next
            Next
        End If
endProc:
        Exit Sub
    End Sub

    Private Sub addToGroup(ByVal lnIndex As Integer)
        Dim loLbl As Label
        Dim loRef As Label

        loLbl = CType(FindLabel(Me, "lblTable" & Format(lnIndex, "00")), Label)
        loRef = CType(FindLabel(Me, "lblRefer" & Format(lnIndex, "00")), Label)

        If loLbl.Tag <> pxeGrpdTbleTag Then
            If loLbl.Tag = pxeOccupiedTag Then
                If Len(loRef.Text) = 4 Then
                    showGroup(loRef.Text, Val(Mid(loLbl.Name, 9)))
                Else
                    loLbl.BackColor = Color.Green
                    loLbl.ForeColor = Color.White
                    loLbl.Tag = pxeAddedTblTag

                    loRef.BackColor = Color.Green
                    loRef.ForeColor = Color.White
                    loRef.Text = "ADDED"

                    Dim loDT As DataTable
                    loDT = poMerge.GetTableTrans(loLbl.Text)
                    If Not IsNothing(loDT) Then
                        psAddToMerge = psAddToMerge & "»" & Trim(loRef.Text)
                        psAddMgTrans = psAddMgTrans & "»" & Trim(loDT(0)("sTransNox"))
                    End If
                End If
            ElseIf loLbl.Tag = pxeAddedTblTag Then
                Occupied(lnIndex, False)
            End If
        End If
    End Sub

    Private Sub Occupied(ByVal lsValue As String, ByVal lbOccupied As Boolean, Optional ByVal lsRefVal As String = "")
        Dim lasValue() As String
        Dim loLbl As Label
        Dim loRef As Label
        Dim lnCtr As Integer

        lasValue = Split(lsValue, "»")

        For lnCtr = 0 To UBound(lasValue)
            loLbl = CType(FindLabel(Me, "lblTable" & Format(Int(lasValue(lnCtr)), "00")), Label)
            loRef = CType(FindLabel(Me, "lblRefer" & Format(Int(lasValue(lnCtr)), "00")), Label)

            If lbOccupied Then
                loLbl.BackColor = Color.Gray
                loLbl.ForeColor = Color.White
                loLbl.Tag = pxeOccupiedTag

                loRef.BackColor = Color.Gray
                loRef.ForeColor = Color.White
            Else
                loLbl.BackColor = Color.White
                loLbl.ForeColor = Color.Green
                loLbl.Tag = pxeUnoccupdTag

                loRef.BackColor = Color.White
                loRef.ForeColor = Color.Green
            End If

            loRef.Text = lsRefVal
        Next
    End Sub

    Private Sub Occupied(ByVal lnIndex As Integer, ByVal lbOccupied As Boolean, Optional ByVal lsRefVal As String = "")
        Dim loLbl As Label
        Dim loRef As Label

        loLbl = CType(FindLabel(Me, "lblTable" & Format(lnIndex, "00")), Label)
        loRef = CType(FindLabel(Me, "lblRefer" & Format(lnIndex, "00")), Label)

        If lbOccupied Then
            loLbl.BackColor = Color.Gray
            loLbl.ForeColor = Color.White
            loLbl.Tag = pxeOccupiedTag

            loRef.BackColor = Color.Gray
            loRef.ForeColor = Color.White
        Else
            loLbl.BackColor = Color.White
            loLbl.ForeColor = Color.Green
            loLbl.Tag = pxeUnoccupdTag

            loRef.BackColor = Color.White
            loRef.ForeColor = Color.Green
        End If

        loRef.Text = lsRefVal
    End Sub

    Private Sub clearFields()
        Dim lnCtr As Integer

        For lnCtr = 0 To 31
            Occupied(lnCtr, False)
        Next
    End Sub

    Private Sub lblTable_Click(sender As System.Object, e As System.EventArgs)
        Dim loLbl As Label
        loLbl = CType(sender, System.Windows.Forms.Label)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loLbl.Name, 9))

        If loLbl.Tag = pxeUnoccupdTag Then Exit Sub 'don't tag unoccupied tables

        If psMergeTable = "" Then
            showGroup(loLbl.Text, lnIndex)
        Else
            addToGroup(lnIndex)
        End If
endProc:
        Exit Sub
    End Sub

    Private Sub lblTable_DoubleClick(sender As Object, e As System.EventArgs)
        Dim loLbl As Label
        Dim loRef As Label

        loLbl = CType(sender, System.Windows.Forms.Label)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loLbl.Name, 9))

        If loLbl.Tag <> pxeGrpdTbleTag Then Exit Sub 'don't need to untagged not merged table

        loRef = CType(FindLabel(Me, "lblRefer" & Format(lnIndex, "00")), Label)

        Dim loDT As DataTable
        loDT = poMerge.GetTable(loLbl.Text)

        If CInt(loDT(0)("cStatusxx")) = xeTableStatus.xeOccupied Then
            loRef.Text = Strings.Right(IIf(IFNull(loDT(0)("sMergeIDx"), "") = "", _
                                                   loDT(0)("nContrlNo"), _
                                                   loDT(0)("sMergeIDx")), 4)
        Else
            loRef.Text = ""
        End If

        markTable(lnIndex, CInt(loDT(0)("cStatusxx")))

        loDT = poMerge.GetTableTrans(loLbl.Text)

        If Not IsNothing(loDT) Then
            psMergeTable = Replace(psMergeTable, loRef.Text & "»", "")
            psMergeTrans = Replace(psMergeTrans, loDT(0)("sTransNox"), "")
        End If
    End Sub

    Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or 33554432
            Return cp
        End Get
    End Property

    Private Sub PreventFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub
End Class