Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmComplementary
    Private WithEvents poComplementary As Complementary
    Private pnLoadx As Integer
    Private poControl As Control
    Private pbCloseForm As Boolean
    Private pnSalesTotl As Decimal
    Private pnActiveRow As Integer

    ReadOnly Property CloseForm() As Boolean
        Get
            Return pbCloseForm
        End Get
    End Property

    WriteOnly Property Complementary() As Complementary
        Set(ByVal oComplementary As Complementary)
            poComplementary = oComplementary
        End Set
    End Property

    Private Sub frmPayCredit_Keydown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                pbCloseForm = True
                Me.Close()
                Me.Dispose()
            Case Keys.Return, Keys.Down
                'If RadioButton2.Checked = True Then SetNextFocus()
            Case Keys.Up
                'If RadioButton2.Checked = True Then SetPreviousFocus()
        End Select
    End Sub

    Private Sub frmPay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        setVisible()

        If pnLoadx = 0 Then
            InitializeDataGrid()
            initGrid()

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

        With poComplementary
            Select Case lnIndex
                Case 0
                    'save
                    If RadioButton1.Checked = True Then If CDbl(txtField05.Text) < CDbl(lblBill.Text) Then GoTo endProc

                    If Not .SaveTransaction() Then GoTo endProc

                    pbCloseForm = False
                Case 1
                    pbCloseForm = True
            End Select
        End With

        Me.Close()
        Me.Dispose()
endProc:
        Exit Sub
    End Sub

    Private Sub showDetail(ByVal lbShow As Boolean)
        Dim lvDetailLoc As New Point(3, 379)
        Dim lvButtonLoc As New Point(3, 379)
        Dim lvMPnelOrgx As New Size(390, 541)
        Dim lvMPnelNewx As New Size(390, 541)
        Dim lvFormOrgxx As New Size(390, 541)
        Dim lvFormNewxx As New Size(390, 541)

        If lbShow Then
            Me.Size = lvFormOrgxx
            pnlDetail.Visible = True
            pnlMain.Size = lvMPnelOrgx
            pnlButtons.Location = lvButtonLoc
        Else
            Me.Size = lvFormNewxx
            pnlDetail.Visible = False
            pnlMain.Size = lvMPnelNewx
            pnlButtons.Location = lvDetailLoc
        End If
    End Sub

    Private Sub setVisible()
        Me.Visible = False
        Me.TransparencyKey = Nothing
        Me.Location = New Point(507, 90)
        Me.Visible = True

        RadioButton1.Checked = True
        txtField05.Focus()
    End Sub

    Private Sub clearFields()
        With poComplementary
            lblBill.Text = Format(.SalesTotal, "#,##0.00")
            txtField05.Text = "0"
            txtField02.Text = .Master(2)
            txtField03.Text = .Master(3)

            If .HasComplementary Then If .SalesTotal > .Master(5) Then RadioButton2.Checked = True

            If RadioButton1.Checked Then
                txtField05.Text = lblBill.Text
                txtField05.ReadOnly = True

                .ComplemtaryType = ggcRetailSales.Complementary.xeCompType.xeCompAmount
                showDetail(False)
            Else
                txtField05.Text = "0"
                txtField05.ReadOnly = False

                .ComplemtaryType = ggcRetailSales.Complementary.xeCompType.xecompItem

                loadDetail()

                showDetail(True)
            End If
        End With

        txtField05.Focus()
        txtField05.SelectAll()
    End Sub

    Private Sub InitializeDataGrid()
        With DataGridView1
            ' Initialize basic DataGridView properties.
            .Dock = DockStyle.Fill
            .BackgroundColor = Color.LightGray
            .BorderStyle = BorderStyle.Fixed3D

            ' Set property values appropriate for read-only display and 
            ' limited interactivity. 
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            .AllowUserToResizeColumns = False
            .ColumnHeadersHeightSizeMode = _
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .AllowUserToResizeRows = False
            .RowHeadersWidthSizeMode = _
                DataGridViewRowHeadersWidthSizeMode.DisableResizing

            ' Set the selection background color for all the cells.
            .DefaultCellStyle.SelectionBackColor = Color.Empty
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' Set RowHeadersDefaultCellStyle.SelectionBackColor so that its default
            ' value won't override DataGridView.DefaultCellStyle.SelectionBackColor.
            .RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty 'Color.White

            ' Set the background color for all rows and for alternating rows. 
            ' The value for alternating rows overrides the value for all rows. 
            .RowsDefaultCellStyle.BackColor = Color.DimGray
            .AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray

            ' Set the row and column header styles.
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .RowHeadersDefaultCellStyle.BackColor = Color.Black

            .Font = New Font("Tahoma", 10)
            .RowTemplate.Height = 23
            .ColumnHeadersHeight = 28
        End With

        With DataGridView1.ColumnHeadersDefaultCellStyle
            .BackColor = Color.Navy
            .ForeColor = Color.White
            .Font = New Font(DataGridView1.Font, FontStyle.Bold)
        End With
    End Sub

    Private Sub initGrid()
        With DataGridView1
            .RowCount = 0

            'Set No of Columns
            .ColumnCount = 6

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "Barrcode"
            .Columns(2).HeaderText = "Description"
            .Columns(3).HeaderText = "Qty"
            .Columns(4).HeaderText = "SRP"
            .Columns(5).HeaderText = "Total"

            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 65
            .Columns(2).Width = 110
            .Columns(3).Width = 45
            .Columns(4).Width = 55
            .Columns(5).Width = 55

            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'Set No of Rows
            .RowCount = 1

            pnActiveRow = 0
        End With
    End Sub

    Private Sub loadDetail()
        Call initGrid()

        With DataGridView1
            Dim lnCtr As Integer

            .RowCount = IIf(poComplementary.ItemCount = 0, 1, poComplementary.ItemCount)

            For lnCtr = 0 To poComplementary.ItemCount - 1
                .Item(0, lnCtr).Value = lnCtr + 1
                .Item(1, lnCtr).Value = poComplementary.Detail(lnCtr, "sBarcodex")
                .Item(2, lnCtr).Value = poComplementary.Detail(lnCtr, "sDescript")
                .Item(3, lnCtr).Value = poComplementary.Detail(lnCtr, "nQuantity")
                .Item(4, lnCtr).Value = Format(poComplementary.Detail(lnCtr, "nUnitPrce"), "#,##0.00")
                .Item(5, lnCtr).Value = Format(poComplementary.Detail(lnCtr, "nQuantity") * poComplementary.Detail(lnCtr, "nUnitPrce"), "#,##0.00")
            Next

            pnActiveRow = 0
            .ClearSelection()
            .Rows(pnActiveRow).Selected = True

            txtField05.Text = poComplementary.Detail(pnActiveRow, "nComplmnt")
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

    Private Sub RadioButton1_CheckedChanged(sender As System.Object, e As System.EventArgs) _
                                            Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged
        Call clearFields()
    End Sub

    Private Sub txtField_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                Case 5
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
                Case 2, 3
                    poComplementary.Master(loIndex) = loTxt.Text
                Case 5
                    If RadioButton2.Checked = True Then
                        If poComplementary.Detail(pnActiveRow, "nQuantity") < CInt(loTxt.Text) Then
                            loTxt.Text = poComplementary.Detail(pnActiveRow, "nQuantity")
                        End If

                        poComplementary.Detail(pnActiveRow, "nComplmnt") = loTxt.Text
                    Else
                        poComplementary.Master(loIndex) = loTxt.Text
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
        End If
    End Sub

    Private Sub txtField_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        Select Case e.KeyCode
            Case Keys.Return
                If Mid(loTxt.Name, 1, 8) = "txtField" Then
                    Select Case loIndex
                        Case 5  'quantity
                            If RadioButton2.Checked = True Then
                                If poComplementary.Detail(pnActiveRow, "nQuantity") < CInt(loTxt.Text) Then
                                    loTxt.Text = poComplementary.Detail(pnActiveRow, "nQuantity")
                                End If

                                poComplementary.Detail(pnActiveRow, "nComplmnt") = loTxt.Text
                            End If
                        Case 2, 3
                            poComplementary.Master(loIndex) = loTxt.Text
                    End Select
                End If
        End Select
    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As System.EventArgs) Handles DataGridView1.Click
        With DataGridView1
            pnActiveRow = .CurrentCell.RowIndex

            If RadioButton2.Checked = True Then txtField05.Text = poComplementary.Detail(pnActiveRow, "nComplmnt")

            txtField05.Focus()
            txtField05.SelectAll()
        End With
    End Sub

    Private Sub txtField05_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtField05.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub
End Class