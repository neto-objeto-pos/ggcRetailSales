Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmSplitOrder
    Private WithEvents poSplit As SplitOrder
    Private pnLoadx As Integer
    Private poControl As Control
    Private p_bCancelled As Boolean
    Private pnActiveRow As Integer

    ReadOnly Property Cancelled() As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    WriteOnly Property SplitOrder() As SplitOrder
        Set(ByVal oSplitOrder As SplitOrder)
            poSplit = oSplitOrder
        End Set
    End Property

    Private Sub frmSplitOrder_Keydown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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

    Private Sub frmSplitOrder_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If pnLoadx = 0 Then
            setVisible()

            showDetail(True)
            clearFields()
            loadDetail()

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
            Case 0 'ok
                If Not poSplit.SaveTransaction() Then GoTo endProc

                p_bCancelled = False
            Case 1 'close
                p_bCancelled = True
        End Select

        Me.Close()
        Me.Dispose()
endProc:
        Exit Sub
    End Sub

    Private Sub showDetail(ByVal lbShow As Boolean)
        Dim lvDetailLoc As New Point(3, 155)
        Dim lvButtonLoc As New Point(3, 471)
        Dim lvMPnelOrgx As New Size(611, 516)
        Dim lvMPnelNewx As New Size(611, 200)
        Dim lvFormOrgxx As New Size(621, 638)
        Dim lvFormNewxx As New Size(621, 638)

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
        Me.Location = New Point(610, 110)
        Me.Visible = True
    End Sub

    Private Sub clearFields()
        With poSplit
            lblBill.Text = Format(.SalesTotal, "#,##0.00")

            If .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByAmount Then
                RadioButton1.Checked = True
            ElseIf .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByPercentage Then
                RadioButton2.Checked = True
            Else
                RadioButton3.Checked = True
            End If

            If .WasSplitted Then
                Call initCombo(.GroupNo)
            Else
                Call initCombo(.GroupNo)
            End If

            If RadioButton3.Checked Then
                Call loadDetail()
            Else
                txtField02.Text = lblBill.Text
                txtField02.Focus()
                txtField02.SelectAll()
            End If
        End With
    End Sub

    Private Sub loadDetail()
        Dim lnCtr As Integer
        Dim lnIndex As Integer
        Dim lnQuantity As Integer
        Dim lnAmount As Double

        Call InitializeDataGrid()

        txtField00.Text = poSplit.GroupNo

        If poSplit.SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByAmount Then
            RadioButton1.Checked = True

            lnAmount = 0

            MsgBox(poSplit.Detail(lnCtr, "nGrpAm" & Format(cmbField.SelectedIndex + 1, "000")))
            If poSplit.Detail(lnCtr, "nGrpAm" & Format(cmbField.SelectedIndex + 1, "000")) = 0 Then
                For lnIndex = 0 To cmbField.Items.Count - 1
                    If cmbField.SelectedIndex <> lnIndex Then
                        lnAmount += poSplit.Detail(lnCtr, "nGrpAm" & Format(lnIndex + 1, "000"))
                    End If
                Next

                lnAmount = poSplit.SalesTotal - lnAmount
            Else
                lnAmount = poSplit.Detail(lnCtr, "nGrpAm" & Format(cmbField.SelectedIndex + 1, "000"))
            End If

            txtField02.Text = lnAmount
        ElseIf poSplit.SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByPercentage Then
            RadioButton2.Checked = True

            lnAmount = 0
            For lnIndex = 0 To cmbField.Items.Count - 1
                If cmbField.SelectedIndex <> lnIndex Then
                    lnAmount += poSplit.Detail(lnCtr, "nGrpAm" & Format(lnIndex + 1, "000"))
                End If
            Next

            lnAmount = (1 - (lnAmount / poSplit.SalesTotal)) * 100

            txtField02.Text = Math.Round(lnAmount, 2) & "%"
        Else
            RadioButton3.Checked = True

            Call initGrid()

            With DataGridView1
                .RowCount = IIf(poSplit.ItemCount = 0, 1, poSplit.ItemCount)

                pnActiveRow = 0

                .ClearSelection()
                .Rows(pnActiveRow).Selected = True

                For lnCtr = 0 To poSplit.ItemCount - 1
                    .Item(0, lnCtr).Value = lnCtr + 1
                    .Item(1, lnCtr).Value = poSplit.Detail(lnCtr, "sStockIDx")
                    .Item(2, lnCtr).Value = poSplit.Detail(lnCtr, "nUnitPrce")

                    lnQuantity = 0
                    For lnIndex = 0 To cmbField.Items.Count - 1
                        lnQuantity += poSplit.Detail(lnCtr, "nGrpQt" & Format(lnIndex + 1, "000"))
                    Next

                    lnQuantity = poSplit.Detail(lnCtr, "nQuantity") - lnQuantity

                    .Item(3, lnCtr).Value = poSplit.Detail(lnCtr, "nQuantity")
                    .Item(4, lnCtr).Value = lnQuantity
                    .Item(5, lnCtr).Value = poSplit.Detail(lnCtr, "nGrpQt" & Format(cmbField.SelectedIndex + 1, "000"))
                Next

                txtField02.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(cmbField.SelectedIndex + 1, "000"))
            End With

            GoTo endProc
        End If

        Call initGridAmount()

        With DataGridView1
            .RowCount = IIf(poSplit.GroupNo = 0, 1, poSplit.GroupNo)

            pnActiveRow = 0

            .ClearSelection()
            .Rows(pnActiveRow).Selected = True

            For lnCtr = 0 To poSplit.GroupNo - 1
                .Item(0, lnCtr).Value = lnCtr + 1
                .Item(1, lnCtr).Value = "SET " & lnCtr + 1
                .Item(2, lnCtr).Value = Format(poSplit.Detail(0, "nGrpAm" & Format(lnCtr + 1, "000")), "#,##0.00")
                .Item(3, lnCtr).Value = Math.Round((poSplit.Detail(0, "nGrpAm" & Format(lnCtr + 1, "000")) / poSplit.SalesTotal) * 100, 2) & "%"
            Next
        End With
endProc:
        cmbField.Focus()
    End Sub

    Private Sub initCombo(ByVal lnGroupNo As Integer)
        With cmbField
            .Items.Clear()

            If lnGroupNo = 1 Then
                .Items.Add("1")
            Else
                Dim lnCtr As Integer

                For lnCtr = 1 To lnGroupNo
                    .Items.Add(lnCtr)
                Next
            End If

            .SelectedItem = .Items(0)
        End With
    End Sub

    Private Sub loadSplit()
        With poSplit
            Call initCombo(.GroupNo)
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

    Private Sub txtField_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                Case 2
                    If RadioButton2.Checked Then loTxt.Text = Replace(loTxt.Text, "%", "")
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

        Dim lnCombo As Integer

        lnCombo = cmbField.SelectedIndex + 1

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                Case 0 'group number
                    poSplit.GroupNo = loTxt.Text

                    Call loadSplit()
                Case 2 'value
                    If Not IsNumeric(loTxt.Text) Then
                        loTxt.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(lnCombo, "000"))
                        GoTo endProc
                    ElseIf loTxt.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(lnCombo, "000")) Then
                        GoTo endProc
                    End If

                    If RadioButton1.Checked Then
                        poSplit.Detail(pnActiveRow, "nGrpAm" & Format(lnCombo, "000")) = loTxt.Text
                    ElseIf RadioButton2.Checked Then
                        If loTxt.Text > 100 Then loTxt.Text = 100

                        poSplit.Detail(pnActiveRow, "nGrpAm" & Format(lnCombo, "000")) = (loTxt.Text / 100) * poSplit.SalesTotal
                    Else
                        poSplit.Detail(pnActiveRow, "nGrpQt" & Format(lnCombo, "000")) = loTxt.Text
                    End If

                    If RadioButton1.Checked Or RadioButton2.Checked Then
                        If cmbField.SelectedIndex < poSplit.GroupNo - 1 Then
                            cmbField.SelectedIndex += 1
                        Else
                            cmbField.SelectedIndex = 0
                        End If
                    Else
                        Call loadDetail()
                    End If
            End Select
        End If

endProc:
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
                    End Select
                End If
        End Select
    End Sub

    Private Sub RadioButton_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles _
        RadioButton1.CheckedChanged, RadioButton2.CheckedChanged, RadioButton3.CheckedChanged

        With poSplit
            If RadioButton1.Checked = True Then
                .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByAmount
            ElseIf RadioButton2.Checked = True Then
                .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByPercentage
            Else
                .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByMenu
            End If
        End With
        
        Call clearFields()
        Call loadSplit()
        Call loadDetail()
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
            .Columns(1).HeaderText = "Item Description"
            .Columns(2).HeaderText = "Unit Price"
            .Columns(3).HeaderText = "ORG Qty"
            .Columns(4).HeaderText = "REM Qty"
            .Columns(5).HeaderText = "SET Qty"

            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 230
            .Columns(2).Width = 80
            .Columns(3).Width = 85
            .Columns(4).Width = 85
            .Columns(5).Width = 85

            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
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
        End With
    End Sub

    Private Sub initGridAmount()
        With DataGridView1
            .RowCount = 0

            'Set No of Columns
            .ColumnCount = 4

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "GROUP NAME"
            .Columns(2).HeaderText = "SHARE AMOUNT"
            .Columns(3).HeaderText = "PERCENTAGE"

            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 225
            .Columns(2).Width = 170
            .Columns(3).Width = 170

            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'Set No of Rows
            .RowCount = 1
        End With
    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As System.EventArgs) Handles DataGridView1.Click
        With DataGridView1
            pnActiveRow = .CurrentCell.RowIndex

            If RadioButton3.Checked = True Then
                txtField02.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(cmbField.SelectedIndex + 1, "000"))
            End If

            txtField02.Focus()
            txtField02.SelectAll()
        End With
    End Sub

    Private Sub cmbField_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbField.SelectedIndexChanged
        If pnLoadx = 1 Then
            loadDetail()
            txtField02.Focus()
        End If
    End Sub
End Class