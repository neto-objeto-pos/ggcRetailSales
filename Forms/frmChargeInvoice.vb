Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms

Imports ggcReceipt
Imports ggcAppDriver

Public Class frmChargeInvoice
    Private WithEvents poCharge As ChargeInvoice
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_nSales As Decimal
    Private p_nDiscount As Decimal
    Private p_bCancelled As Boolean

    WriteOnly Property ChargeInvoice() As ChargeInvoice
        Set(ByVal oChargeInvoice As ChargeInvoice)
            poCharge = oChargeInvoice
        End Set
    End Property

    WriteOnly Property SalesTotal As Decimal
        Set(ByVal value As Decimal)
            p_nSales = value
        End Set
    End Property

    WriteOnly Property Discounts As Decimal
        Set(ByVal value As Decimal)
            p_nDiscount = value
        End Set
    End Property

    ReadOnly Property Cancelled As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    Private Sub Form_Keydown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                p_bCancelled = True
                Me.Close()
                Me.Dispose()
            Case Keys.Return, Keys.Down
                SetNextFocus()
            Case Keys.Up
                SetPreviousFocus()
        End Select
    End Sub

    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        setVisible()

        If pnLoadx = 0 Then
            clearFields()

            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpCancelHandler(Me, GetType(TextBox), "txtField", "Validating", AddressOf txtField_Validating)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)

            pnLoadx = 1
        End If
    End Sub

    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 0
                p_bCancelled = Not poCharge.SaveTransaction()

                If Not p_bCancelled Then
                    Me.Close()
                    Me.Dispose()
                End If
            Case 1
                p_bCancelled = True
                Me.Close()
                Me.Dispose()
        End Select
endProc:
        Exit Sub
    End Sub

    Private Sub setVisible()
        Me.Visible = False
        Me.TransparencyKey = Nothing
        Me.Location = New Point(507, 90)

        txtField00.MaxLength = 64
        txtField01.MaxLength = 128

        Me.Visible = True
    End Sub

    Private Sub clearFields()
        lblBill.Text = Format(p_nSales, "#,##0.00")
        lblDiscount.Text = Format(p_nDiscount, "#,##0.00")
        txtField01.Text = ""
        txtField00.Text = ""
        txtField00.Focus()
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
                Case 0
                    poCharge.Master("sClientNm") = loTxt.Text
                Case 1
                    poCharge.Master("xAddressx") = loTxt.Text
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
    End Sub

    Private Sub txtField_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))
    End Sub
End Class