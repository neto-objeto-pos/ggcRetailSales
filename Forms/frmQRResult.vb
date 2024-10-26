Imports System.Drawing
Imports System.Windows.Forms
Imports ggcAppDriver

Public Class frmQRResult
    Private p_oAppDriver As GRider
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_bCancelled As Boolean
    Private p_sChargeInfo() As String
    Private p_sRunningTotal As String
    Private WithEvents poChargeMeal As ChargeInvoiceMeal


    WriteOnly Property ChargeInvoiceMeal() As ChargeInvoiceMeal
        Set(ByVal oChargeInvoiceMeal As ChargeInvoiceMeal)
            poChargeMeal = oChargeInvoiceMeal
        End Set
    End Property
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

    Public Property SummaryTotal As String
        Set(value As String)
            p_sRunningTotal = value
        End Set
        Get
            Return p_sRunningTotal
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

                        Case 0

                        Case Else
                            loTextField.Text = p_sChargeInfo(lnRow)
                    End Select
                End If
            Next
            loadMessage()
        End If

    End Sub

    Private Sub loadMessage()
        lblMessage.Text = "Hi " & p_sChargeInfo(1) & "! Thank you for ordering, Here is your running balance ₱ " & p_sRunningTotal
        If poChargeMeal.HasComboMeal Then
            If poChargeMeal.HasSubsidy Then
                lblSubsidy.Text = "Your Guanzon subsidy is not yet used."
                lblSubsidy.ForeColor = Color.Green
            Else
                lblSubsidy.Text = "Your Guanzon subsidy is already used."
                lblSubsidy.ForeColor = Color.Red
            End If
        Else
            lblSubsidy.Text = ""
            lblSubsidy.ForeColor = Color.Black
        End If
    End Sub

    Private Sub clearFields()

        txtField01.Text = ""
        txtField02.Text = ""
        txtField03.Text = ""
        txtField04.Text = ""
        'p_sRunningTotal = "0.00"
        lblMessage.Text = ""


    End Sub

    Private Sub cmdButton00_Click(sender As Object, e As EventArgs) Handles cmdButton00.Click
        p_bCancelled = Not poChargeMeal.SaveTransaction()

        If Not p_bCancelled Then
            Me.Close()
            Me.Dispose()
        End If
    End Sub

End Class