Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing
Imports ggcAppDriver

Public Class frmCashPullout
    Private pnLoadx As Integer
    Private poControl As Control

    Private WithEvents poCashPullout As CashPullout
    Private p_oAppDriver As GRider

    Private pbCancel As Boolean
    Private psCashierx As String

    ReadOnly Property Cancel()
        Get
            Return pbCancel
        End Get
    End Property

    WriteOnly Property Cashier As String
        Set(ByVal Value As String)
            psCashierx = Value
        End Set
    End Property


    Private Sub frmPayGC_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                pbCancel = True
                Me.Hide()
            Case Keys.Return, Keys.Down
                SetNextFocus()
            Case Keys.Up
                SetPreviousFocus()
        End Select
    End Sub

    Private Sub frmPay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If pnLoadx = 0 Then
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)

            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpCancelHandler(Me, GetType(TextBox), "txtField", "Validating", AddressOf txtField_Validating)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)

            clearFields()

            poCashPullout = New CashPullout(p_oAppDriver)

            pbCancel = True
            With poCashPullout
                .Cashier = psCashierx
                If Not .NewTransaction Then Me.Close()
            End With

            pnLoadx = 1
        End If
    End Sub

    Private Sub txtField_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
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
                Case 4
                    If Not IsNumeric(loTxt.Text) Then loTxt.Text = 0

                    poCashPullout.Master(loIndex) = loTxt.Text
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
        End If
    End Sub

    Private Sub txtField_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        Select Case e.KeyCode
            Case Keys.Return, Keys.F3
                If Mid(loTxt.Name, 1, 8) = "txtField" Then
                End If
        End Select
    End Sub

    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 0 'OK
                pbCancel = Not poCashPullout.SaveTransaction
            Case 1 'CANCEL
                pbCancel = True
        End Select

        Me.Close()
endProc:
        Exit Sub
    End Sub

    Private Sub clearFields()
        With poCashPullout
            lblCashier.Text = ""
            txtField04.Text = "0.00"
            txtField81.Text = "0.00"
            txtField82.Text = "0.00"
            txtField83.Text = "0.00"
        End With
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

    Private Sub poCashPullout_MasterRetrieved(Index As Object, Value As Object) Handles poCashPullout.MasterRetrieved
        Select Case Index
            Case 80
                lblCashier.Text = Value
            Case 4
                txtField04.Text = Format(Value, "#,##0.00")
            Case 81
                txtField81.Text = Format(Value, "#,##0.00")
            Case 82
                txtField82.Text = Format(Value, "#,##0.00")
            Case 83
                txtField83.Text = Format(Value, "#,##0.00")
        End Select
    End Sub

    Private Sub txtOccupants_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtDiscount_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

    Public Sub New(oDriver As GRider)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        p_oAppDriver = oDriver
    End Sub

    Private Sub frmCashPullout_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        txtField04.Focus()
    End Sub

    Private Sub txtField04_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtField04.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

End Class