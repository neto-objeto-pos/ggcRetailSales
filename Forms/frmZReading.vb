Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms
Imports ggcAppDriver

Public Class frmZReading
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_bCancelled As Boolean

    Private p_oAppDriver As GRider
    Private p_dPOSDate As Date
    Private p_dPOSFrom As Date
    Private p_dPOSThru As Date

    Public Sub New(oDriver As GRider)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        p_oAppDriver = oDriver
    End Sub

    ReadOnly Property Cancelled As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    Public WriteOnly Property PosDate() As Date
        Set(ByVal Value As Date)
            p_dPOSDate = Value
        End Set
    End Property

    ReadOnly Property POSFrom As Date
        Get
            Return p_dPOSFrom
        End Get
    End Property

    ReadOnly Property POSThru As Date
        Get
            Return p_dPOSThru
        End Get
    End Property

    Private Sub Form_Keydown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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

    Private Sub Form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        setVisible()

        If pnLoadx = 0 Then
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)

            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpCancelHandler(Me, GetType(TextBox), "txtField", "Validating", AddressOf txtField_Validating)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)
            Call clearFields()

            pnLoadx = 1
        End If
    End Sub

    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 1 'Ok
                p_dPOSFrom = txtField00.Text
                p_dPOSThru = txtField01.Text

                p_bCancelled = False
                Me.Hide()
                Me.Dispose()
            Case 0
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

        Me.Visible = True
    End Sub

    Private Sub clearFields()
        txtField00.Text = Format(p_dPOSDate, "yyyy/MM/dd")
        txtField01.Text = Format(p_dPOSDate, "yyyy/MM/dd")

        txtField01.Focus()
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
                    If Not IsDate(loTxt.Text) Then
                        loTxt.Text = p_dPOSDate
                    End If

                Case 1

                    If Not IsDate(loTxt.Text) Then
                        loTxt.Text = p_dPOSDate
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
    End Sub

    Private Sub txtField_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))
    End Sub
End Class