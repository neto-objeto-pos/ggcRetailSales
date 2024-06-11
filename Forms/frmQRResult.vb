Imports System.Windows.Forms
Imports ggcAppDriver

Public Class frmQRResult
    Private p_oAppDriver As GRider
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_bCancelled As Boolean
    Private p_sChargeInfo() As String
    Private p_sRunningTotal As String


    ReadOnly Property Cancelled As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    Public Property ChargeInformation As String()
        Set(value As String())
            p_sChargeInfo = value
        End Set
        Get
            Return p_sChargeInfo
        End Get
    End Property


    Public Sub New(oDriver As GRider)
        InitializeComponent()
        p_oAppDriver = oDriver
        p_bCancelled = True
    End Sub
    Private Sub Form_Keydown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.Dispose()
                Me.Close()
            Case Keys.Return, Keys.Down
                SetNextFocus()
            Case Keys.Up
                SetPreviousFocus()
        End Select
    End Sub
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If pnLoadx = 0 Then
            pnLoadx = 1
            clearFields()
            loadRecord()
        End If

    End Sub

    Private Sub loadRecord()
        If p_sChargeInfo.Length = 5 Then

            For lnRow As Integer = 0 To p_sChargeInfo.Length - 1

                Dim loTextField As TextBox = Me.Controls.Find("txtField" & lnRow.ToString("00"), True).FirstOrDefault()
                If loTextField IsNot Nothing Then
                    Select Case lnRow
                        Case 4
                            p_sRunningTotal = p_sChargeInfo(lnRow)

                        Case Else
                            loTextField.Text = p_sChargeInfo(lnRow)
                    End Select
                End If
            Next
            loadMessage()
        End If

    End Sub

    Private Sub loadMessage()
        lblMessage.Text = "Hi " & p_sChargeInfo(0) & "! Thank you for ordering here is your running balance ₱ " & p_sRunningTotal

    End Sub

    Private Sub clearFields()

        txtField00.Text = ""
        txtField01.Text = ""
        txtField02.Text = ""
        txtField03.Text = ""
        p_sRunningTotal = "0.00"
        lblMessage.Text = ""


    End Sub

    Private Sub cmdButton00_Click(sender As Object, e As EventArgs) Handles cmdButton00.Click
        Me.Dispose()
        Me.Close()
    End Sub
End Class