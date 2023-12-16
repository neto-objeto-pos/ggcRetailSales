Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmManageTable
    Private WithEvents poTable As TableMaster
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_bCancelled As Boolean

    WriteOnly Property TableMaster() As TableMaster
        Set(ByVal oTableMaster As TableMaster)
            poTable = oTableMaster
        End Set
    End Property

    ReadOnly Property Cancelled As Boolean
        Get
            Return p_bCancelled
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
            Case 0 'Ok
                If chk00.Checked = True Then
                    poTable.WithSCharge = True
                Else
                    poTable.WithSCharge = False
                End If

                If poTable.SaveTable Then
                    p_bCancelled = False
                    Me.Hide()
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
        Me.Location = New Point(507,90)

        txtField02.MaxLength = 2

        Me.Visible = True
    End Sub

    Private Sub clearFields()
        lblBill.Text = Format(poTable.Master("nTableNox"), "00")
        txtField04.ReadOnly = True
        txtField04.Text = ""
        chk00.CheckState = IIf(poTable.WithSCharge, CheckState.Checked, CheckState.Unchecked)

        Select Case poTable.Master("cStatusxx")
            Case 0
                RadioButton1.Checked = True
            Case 1
                RadioButton2.Checked = True
            Case 2
                RadioButton3.Checked = True
            Case 3
                RadioButton4.Checked = True
        End Select

        txtField04.ReadOnly = False
        txtField04.Text = Format(poTable.Master("dReserved"), "MMM dd, yyyy hh:mm:ss")
        txtField02.Text = poTable.Master("nOccupnts")
        txtField03.Text = poTable.Master("nCapacity")
        txtField02.Focus()
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
                Case 4
                    If RadioButton3.Checked Then
                        loTxt.Text = Format(poTable.Master("dReserved"), "yyyy/MM/dd hh:mm:ss")
                    End If
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
                    poTable.Master("nOccupnts") = loTxt.Text
                Case 4
                    If RadioButton3.Checked = True Then
                        If Not IsDate(loTxt.Text) Then
                            loTxt.Text = Format(poTable.Master("dReserved"), "MMM dd, yyyy hh:mm:ss")
                        End If

                        poTable.Master("dReserved") = loTxt.Text
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

    Private Sub poTable_MasterRetrieve(lnIndex As Integer) Handles poTable.MasterRetrieve
        Select Case lnIndex
            Case 1
                Select Case poTable.Master("cStatusxx")
                    Case 0
                        RadioButton1.Checked = True
                    Case 1
                        RadioButton2.Checked = True
                    Case 2
                        RadioButton3.Checked = True

                        txtField04.ReadOnly = False
                        txtField04.Text = Format(poTable.Master("dReserved"), "MMM dd, yyyy hh:mm:ss")
                    Case 3
                        RadioButton4.Checked = True
                End Select
            Case 2
                txtField02.Text = poTable.Master("nOccupnts")
            Case 4
                txtField04.Text = Format(poTable.Master("dReserved"), "MMM dd, yyyy hh:mm:ss")
        End Select
    End Sub

    Private Sub RadioButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If pnLoadx = 1 Then
            If RadioButton1.Checked Then
                poTable.Master("cStatusxx") = 0
            ElseIf RadioButton2.Checked Then
                poTable.Master("cStatusxx") = 1
            ElseIf RadioButton3.Checked Then
                poTable.Master("cStatusxx") = 2
            Else
                poTable.Master("cStatusxx") = 3
            End If

            Call clearFields()
        End If
    End Sub
End Class