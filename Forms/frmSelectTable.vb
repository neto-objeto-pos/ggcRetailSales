Imports ggcAppDriver
Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmSelectTable
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_oApp As Grider

    Private pbCancel As Boolean
    Private p_sTableNo As String
    Private p_bSChargex As Boolean
    Private p_sWaiterID As String
    Private p_nOccupants As String

    WriteOnly Property AppDriver()
        Set(ByVal value)
            p_oApp = value
        End Set
    End Property

    Property Waiter()
        Set(ByVal value)
            p_sWaiterID = value
        End Set
        Get
            Return p_sWaiterID
        End Get
    End Property

    Property Occupants()
        Set(ByVal value)
            p_nOccupants = value
        End Set
        Get
            Return p_nOccupants
        End Get
    End Property

    ReadOnly Property Cancel()
        Get
            Return pbCancel
        End Get
    End Property

    Property TableNo()
        Set(value)
            p_sTableNo = value
        End Set
        Get
            Return p_sTableNo
        End Get
    End Property
    Property isWSCharge()
        Set(Value)
            p_bSChargex = Value
        End Set
        Get
            Return p_bSChargex
        End Get
    End Property

    Private Sub frmPayGC_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                pbCancel = True
                setVisible(False)
                Me.Hide()
            Case Keys.Return, Keys.Down
                SetNextFocus()
            Case Keys.Up
                SetPreviousFocus()
        End Select
    End Sub

    Private Sub frmPay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If pnLoadx = 0 Then
            clearFields()
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)

            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpCancelHandler(Me, GetType(TextBox), "txtField", "Validating", AddressOf txtField_Validating)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)

            If IsNothing(p_oApp) Then
                MsgBox("Application Driver Not Set!!!", vbCritical, "Warning")
                Me.Close()
            End If

            txtField00.Text = p_sTableNo
            chk00.CheckState = IIf(p_bSChargex, CheckState.Checked, CheckState.Unchecked)
            If p_sWaiterID <> "" Then
                Dim loDT As DataTable
                Dim lsSQL As String

                lsSQL = "SELECT sClientID, sCompnyNm FROM Client_Master WHERE sClientID = " & strParm(p_sWaiterID)
                loDT = p_oApp.ExecuteQuery(lsSQL)
                If loDT.Rows.Count <> 0 Then
                    txtField01.Text = loDT(0)("sCompnyNm")
                    txtField01.Tag = loDT(0)("sClientID")
                End If
            End If

        pnLoadx = 1
        End If
    End Sub

    Private Sub frmPay_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        setVisible(True)
    End Sub

    Private Sub txtField_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
            End Select
        End If

        poControl = loTxt

        loTxt.BackColor = Color.Azure
        loTxt.SelectAll()
    End Sub

    Private Sub txtField_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                Case 2
                    If Not IsNumeric(loTxt.Text) Then
                        loTxt.Text = 0
                    End If

            End Select
        End If

        loTxt.BackColor = SystemColors.Window
        poControl = Nothing
    End Sub

    Private Sub txtField_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
            End Select
        End If
    End Sub

    Private Sub txtField_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
            End Select
        End If
    End Sub

    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 0 'OK
                If Not isEntryOk() Then Exit Sub
                p_sTableNo = txtField00.Text
                p_sWaiterID = txtField01.Tag
                p_bSChargex = IIf(chk00.CheckState = CheckState.Checked, True, False)

                If IsNumeric(txtField02.Text) Then
                    p_nOccupants = CInt(txtField02.Text)
                Else
                    p_nOccupants = 0
                End If
                pbCancel = False
            Case 1 'CANCEL
                pbCancel = True
        End Select

        Application.DoEvents()
        setVisible(False)
        Me.Hide()
endProc:
        Exit Sub
    End Sub

    Public Function isEntryOk()
        If txtField00.Text = "" Then
            MsgBox("Please input table No!", vbInformation + vbCritical, "Warning")
            txtField00.Focus()
            Return False
        ElseIf CInt(txtField00.Text) > 100 Then
            MsgBox("Please input valid table no!", vbInformation + vbCritical, "Warning")
            txtField00.Focus()
            Return False
        ElseIf txtField01.Text = "" Then
            MsgBox("Please input waiter!", vbInformation + vbCritical, "Warning")
            txtField01.Focus()
            Return False
        ElseIf txtField02.Text = "" Or txtField02.Text <= CInt(0) Then
            MsgBox("Please input occupants!", vbInformation + vbCritical, "Warning")
            txtField02.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub setVisible(ByVal lbShow As Boolean)
        If lbShow Then
            pnlMain.Visible = True

            Me.Opacity = 100
            txtField00.Focus()
        Else
            Me.Opacity = 0
            Me.BackColor = Color.Orange
            Me.TransparencyKey = Me.BackColor
        End If
    End Sub

    Private Sub clearFields()
        txtField00.Text = ""
        txtField01.Text = ""
        txtField02.Text = CInt(1)
    End Sub

    Private Sub txtField00_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtField00.KeyPress
        If e.KeyChar <> ControlChars.Back Then
            e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = "," Or e.KeyChar = "-")
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

    Private Sub txtField01_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtField01.KeyDown
        
        If e.KeyCode = Keys.Return Or e.KeyCode = Keys.F3 Then
            Call getClient(txtField01.Text, False)
        End If
    End Sub

    Private Sub txtField01_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtField01.Validating
        If txtField01.Text = "" Then txtField01.Tag = ""
    End Sub

    Private Sub getClient(ByVal value As String, ByVal byCode As Boolean)
        Dim loClient As ggcClient.Client

        loClient = New ggcClient.Client(p_oApp)
        Try
            If loClient.SearchClient(value, byCode) = True Then
                txtField01.Text = loClient.Master("sCompnyNm")
                txtField01.Tag = loClient.Master("sClientID")
            Else
                txtField01.Text = ""
                txtField01.Tag = ""
            End If
        Catch

        End Try

    End Sub

    Private Sub initForm(ByVal lbShow As Boolean)
        If lbShow Then
            txtField00.Enabled = lbShow
            txtField01.Enabled = lbShow
            txtField02.Enabled = lbShow
        Else
            txtField00.Enabled = Not lbShow
            txtField01.Enabled = Not lbShow
            txtField02.Enabled = Not lbShow
        End If
    End Sub
End Class