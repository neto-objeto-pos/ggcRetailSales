Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmChangePrice
    Private pnLoadx As Integer
    Private poControl As Control

    Private pbCancel As Boolean
    Private psDescript As String
    Private pnUnitPrice As Double

    ReadOnly Property Cancel()
        Get
            Return pbCancel
        End Get
    End Property

    WriteOnly Property Description
        Set(value)
            psDescript = value
        End Set
    End Property
    Property UnitPrice()
        Set(value)
            pnUnitPrice = value
        End Set
        Get
            Return pnUnitPrice
        End Get
    End Property

    Private Sub frmChangePrice_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                pbCancel = True
                setVisible(False)
                Me.Hide()
        End Select
    End Sub

    Private Sub frmChangePrice_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If pnLoadx = 0 Then
            clearFields()
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)

            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpCancelHandler(Me, GetType(TextBox), "txtField", "Validating", AddressOf txtField_Validating)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)

            txtField00.Text = pnUnitPrice
            txtField01.Text = psDescript

            pnLoadx = 1
        End If
    End Sub

    Private Sub frmChangePrice_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
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
    End Sub

    Private Sub txtField_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
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

        With txtField00
            Select Case lnIndex
                Case 14 'OK
                    pnUnitPrice = CDbl(txtField00.Text)
                    pbCancel = False
                Case 13 'CANCEL
                    pbCancel = True
                Case 12 'DEL
                    .Text = Strings.Left(.Text, Len(.Text) - 1)

                    If Len(.Text) = 0 Then .Text = "0"
                Case 11 : .Text = "0" 'C
                Case 10 : If InStr(.Text, ".", CompareMethod.Text) = 0 Then .Text = .Text & "." '.
                Case 9 : .Text = IIf(.Text = "0", "9", .Text & "9")
                Case 8 : .Text = IIf(.Text = "0", "8", .Text & "8")
                Case 7 : .Text = IIf(.Text = "0", "7", .Text & "7")
                Case 6 : .Text = IIf(.Text = "0", "6", .Text & "6")
                Case 5 : .Text = IIf(.Text = "0", "5", .Text & "5")
                Case 4 : .Text = IIf(.Text = "0", "4", .Text & "4")
                Case 3 : .Text = IIf(.Text = "0", "3", .Text & "3")
                Case 2 : .Text = IIf(.Text = "0", "2", .Text & "2")
                Case 1 : .Text = IIf(.Text = "0", "1", .Text & "1")
                Case 0 : .Text = IIf(.Text = "0", "0", .Text & "0")
            End Select
        End With

        If lnIndex = 13 Or lnIndex = 14 Then
            Application.DoEvents()
            setVisible(False)
            Me.Hide()
        End If
endProc:
        Exit Sub
    End Sub

    Private Sub setVisible(ByVal lbShow As Boolean)
        If lbShow Then
            pnlMain.Visible = True

            Me.Opacity = 100
        Else
            Me.Opacity = 0
            Me.BackColor = Color.Orange
            Me.TransparencyKey = Me.BackColor
        End If
    End Sub

    Private Sub clearFields()
        txtField00.Text = "0"
        txtField01.Text = ""
    End Sub

    Private Sub txtField00_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtField00.KeyPress
        If e.KeyChar <> ControlChars.Back Then
            e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ".")
        End If
    End Sub
End Class