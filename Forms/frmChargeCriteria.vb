Imports System.Windows.Forms
Imports ggcAppDriver

Public Class frmChargeCriteria
    Private p_oAppDriver As GRider
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_bCancelled As Boolean
    Private p_sEmployeeID As String

    ReadOnly Property Cancelled As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    Public Property ChargeInformation As String
        Set(value As String)
            p_sEmployeeID = value
        End Set
        Get
            Return p_sEmployeeID
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
            txtField01.Text = ""
            p_sEmployeeID = ""
        End If

    End Sub

    Private Sub cmdButton00_Click(sender As Object, e As EventArgs) Handles cmdButton00.Click
        p_sEmployeeID = ""
        p_bCancelled = False
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub cmdButton01_Click(sender As Object, e As EventArgs) Handles cmdButton01.Click
        p_sEmployeeID = Trim(txtField01.Text)

        p_bCancelled = False
        Me.Close()
        Me.Dispose()
    End Sub
End Class