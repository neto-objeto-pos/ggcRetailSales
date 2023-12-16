Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms
Imports ggcAppDriver

Public Class frmOrderSplit
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

            Call InitializeDataGrid()
            Call InitializeDataGridSplit()

            clearFields()
            loadDetail()
            loadDetailSplitted()

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
                If Not isEntryOK() Then GoTo endProc

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

    Private Function isEntryOK() As Boolean
        With poSplit
            If .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByAmount Or _
                .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByPercentage Then
                'check total splitted amount vs sales amount

                Dim lnIndex As Integer
                Dim lnAmount As Double

                For lnIndex = 0 To poSplit.GroupNo - 1
                    lnAmount += poSplit.Detail(0, "nGrpAm" & Format(lnIndex + 1, "000"))
                Next

                If lnAmount <> .SalesTotal Then
                    MsgBox("Split has invalid amount total. Please verify your entry.", MsgBoxStyle.Critical, "Warning")
                    Return False
                End If
            Else
                Dim lnCtr As Integer

                For lnCtr = 0 To DataGridView1.Rows.Count - 1
                    If DataGridView1.Item(4, lnCtr).Value > 0 Then
                        MsgBox("Items not splitted detected. Please verify your entry.", MsgBoxStyle.Critical, "Warning")
                        Return False
                    End If
                Next
            End If
        End With

        Return True
    End Function

    Private Sub showDetail(ByVal lbShow As Boolean)
        Dim lvDetailLoc As New Point(3, 155)
        Dim lvButtonLoc As New Point(3, 471)
        Dim lvMPnelOrgx As New Size(1024, 768)
        Dim lvMPnelNewx As New Size(1024, 768)
        Dim lvFormOrgxx As New Size(1024, 768)
        Dim lvFormNewxx As New Size(1024, 768)

        If lbShow Then
            Me.Size = lvFormOrgxx
            pnlDetail.Visible = True
            pnlMain.Size = lvMPnelOrgx
        Else
            Me.Size = lvFormNewxx
            pnlDetail.Visible = False
            pnlMain.Size = lvMPnelNewx
        End If
    End Sub

    Private Sub setVisible()
        Me.Visible = False
        Me.TransparencyKey = Nothing
        Me.Location = New Point(1, 90)
        Me.Visible = True
    End Sub

    Private Sub clearFields()
        With poSplit
            lblAmount.Text = "0.00"
            .SetNo = 1

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
                txtField02.Text = lblAmount.Text
            End If
        End With
    End Sub

    Private Sub loadDetail()
        Dim lnCtr As Integer
        Dim lnIndex As Integer
        Dim lnQuantity As Integer
        Dim lnAmount As Double

        txtField00.Text = poSplit.GroupNo

        If poSplit.SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByAmount Then
            RadioButton1.Checked = True

            lnAmount = 0

            If poSplit.Detail(0, "nGrpAm" & Format(poSplit.SetNo, "000")) = 0 Then
                For lnIndex = 0 To poSplit.GroupNo - 1
                    If cmbField.SelectedIndex <> lnIndex Then
                        lnAmount += poSplit.Detail(0, "nGrpAm" & Format(lnIndex + 1, "000"))
                    End If
                Next

                lnAmount = poSplit.SalesTotal - lnAmount
            Else
                lnAmount = poSplit.Detail(0, "nGrpAm" & Format(poSplit.SetNo, "000"))
            End If

            txtField02.Text = lnAmount
        ElseIf poSplit.SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByPercentage Then
            RadioButton2.Checked = True

            lnAmount = 0
            For lnIndex = 0 To poSplit.GroupNo - 1
                If cmbField.SelectedIndex <> lnIndex Then
                    lnAmount += poSplit.Detail(0, "nGrpAm" & Format(lnIndex + 1, "000"))
                End If
            Next

            lnAmount = (1 - (lnAmount / poSplit.SalesTotal)) * 100

            txtField02.Text = Math.Round(lnAmount, 2) & "%"
        Else
            RadioButton3.Checked = True

            With DataGridView1
                Call initGrid(IIf(poSplit.ItemCount = 0, 1, poSplit.ItemCount))

                'pnActiveRow = 0

                '.ClearSelection()
                '.Rows(pnActiveRow).Selected = True

                For lnCtr = 0 To poSplit.ItemCount - 1
                    .Item(0, lnCtr).Value = lnCtr + 1
                    .Item(1, lnCtr).Value = poSplit.Detail(lnCtr, "sStockIDx")
                    .Item(2, lnCtr).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce"), "#,##0.00")

                    lnQuantity = 0
                    For lnIndex = 0 To cmbField.Items.Count - 1
                        lnQuantity += poSplit.Detail(lnCtr, "nGrpQt" & Format(lnIndex + 1, "000"))
                    Next

                    lnQuantity = poSplit.Detail(lnCtr, "nQuantity") - lnQuantity

                    .Item(3, lnCtr).Value = poSplit.Detail(lnCtr, "nQuantity")
                    .Item(4, lnCtr).Value = lnQuantity
                    .Item(5, lnCtr).Value = poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000"))
                Next

                txtField02.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000"))
            End With

            GoTo endProc
        End If

        With DataGridView1
            Call initGridAmount(IIf(poSplit.GroupNo = 0, 1, poSplit.GroupNo))

            'pnActiveRow = 0

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

    Private Sub loadDetailSplitted()
        initGridSplit()

        With DataGridView2
            If RadioButton1.Checked Or RadioButton2.Checked Then
                Dim lnCtr As Integer

                .RowCount = IIf(poSplit.ItemCount = 0, 1, poSplit.ItemCount)

                For lnCtr = 0 To poSplit.ItemCount - 1
                    .Item(0, lnCtr).Value = lnCtr + 1
                    .Item(1, lnCtr).Value = poSplit.Detail(lnCtr, "sBarcodex")
                    .Item(2, lnCtr).Value = poSplit.Detail(lnCtr, "sBriefDsc")
                    .Item(3, lnCtr).Value = IIf(poSplit.Detail(lnCtr, "cReversex") = "-", poSplit.Detail(lnCtr, "cReversex"), "") & poSplit.Detail(lnCtr, "nQuantity")
                    .Item(4, lnCtr).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce"), "#,##0.00")
                    .Item(5, lnCtr).Value = Format(.Item(3, lnCtr).Value * .Item(4, lnCtr).Value, "#,##0.00")
                Next
            Else
                .RowCount = 0

                Dim lnCtr As Integer

                For lnCtr = 0 To poSplit.ItemCount - 1
                    If poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000")) > 0 Then
                        .RowCount = .RowCount + 1
                        .Item(0, .RowCount - 1).Value = lnCtr + 1
                        .Item(1, .RowCount - 1).Value = poSplit.Detail(lnCtr, "sBarcodex")
                        .Item(2, .RowCount - 1).Value = poSplit.Detail(lnCtr, "sBriefDsc")
                        .Item(3, .RowCount - 1).Value = poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000"))
                        .Item(4, .RowCount - 1).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce"), "#,##0.00")
                        .Item(5, .RowCount - 1).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce") * poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000")), "#,##0.00")
                    End If
                Next

                '.ClearSelection()
                'If .RowCount = 0 Then .RowCount = 1
                '.Rows(.Rows.Count - 1).Selected = True
            End If
        End With

        poSplit.SetNo = cmbField.SelectedIndex + 1
        Call showComputation()
    End Sub

    Private Sub showComputation()
        lblMaster04.Text = FormatNumber(IFNull(poSplit.Master("nTranTotl"), 0), 2)
        lblMaster13.Text = FormatNumber(IFNull(poSplit.Master("nVATSales"), 0), 2) 'vat sales
        lblMaster14.Text = FormatNumber(IFNull(poSplit.Master("nVATAmtxx"), 0), 2) 'vat amount
        lblMaster15.Text = FormatNumber(IFNull(poSplit.Master("nNonVATxx"), 0), 2) 'non vat
        lblMaster17.Text = FormatNumber(IFNull(poSplit.Master("nDiscount"), 0) + IFNull(poSplit.Master("nVatDiscx"), 0) + IFNull(poSplit.Master("nPWDDiscx"), 0), 2)
        lblAmount.Text = FormatNumber(CDbl(lblMaster04.Text) - CDbl(lblMaster17.Text), 2) 'amount due
    End Sub

    'Private Sub showComputationNew()
    '    ''jovan - 2020.10.15 revised presentation of discount in interface

    '    Dim lnSubTotal As Double
    '    Dim lnAddDiscount As Double
    '    Dim lnPWDiscount As Double
    '    Dim lnVATExcls As Double
    '    Dim lnDiscount As Double
    '    Dim lnVatRatex As Double = 1.12
    '    'Dim lnSrvCrge As Double
    '    Dim lnAmntDuex As Double

    '    lnSubTotal = poSplit.Master("nTranTotl")

    '    lblMaster04.Text = FormatNumber(lnSubTotal, 2) 'sales total
    '    lnVATExcls = lnSubTotal / lnVatRatex
    '    lblMaster13.Text = FormatNumber(lnVATExcls, 2) 'vat sales

    '    lnDiscount = poSplit.Master("nDiscount") / lnVatRatex

    '    lnAddDiscount = poSplit.Master("nVatDiscx") + poSplit.Master("nPWDDiscx")
    '    lnPWDiscount = poSplit.Master("nPWDDiscx")
    '    lblMaster17.Text = FormatNumber(lnDiscount, 2) 'discounts
    '    lblMaster14.Text = FormatNumber(poSplit.Master("nVATAmtxx"), 2) 'vat amount
    '    lblMaster15.Text = FormatNumber(lnPWDiscount, 2)   'non vat 
    '    lblNetSale.Text = FormatNumber(lnVATExcls - (lnDiscount + lnPWDiscount), 2)
    '    If lnPWDiscount = 0 Then
    '        lnAmntDuex = FormatNumber(lblNetSale.Text + poSplit.Master("nVATAmtxx"), 2)
    '    Else
    '        lnAmntDuex = FormatNumber(lnVATExcls - lnPWDiscount, 2)
    '    End If

    '    'lnSrvCrge = IFNull(poSplit.Master("nSChargex"), 0)
    '    'lblSrvCrge.Text = FormatNumber(lnSrvCrge, 2)
    '    'to remove
    '    'lblAmount.Text = FormatNumber(lnAmntDuex + lnSrvCrge, 2)
    '    lblAmount.Text = FormatNumber(lnAmntDuex, 2)
    '    'poSplit.setTranTotal = lblAmount.Text

    'End Sub

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
            poSplit.SetNo = .SelectedIndex + 1
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

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                Case 0 'group number
                    poSplit.GroupNo = loTxt.Text

                    Call loadSplit()
                Case 2 'value
                    If RadioButton1.Checked Then
                        If Not IsNumeric(loTxt.Text) Then
                            loTxt.Text = poSplit.Detail(0, "nGrpAm" & Format(poSplit.SetNo, "000"))
                            GoTo endProc
                        ElseIf loTxt.Text = poSplit.Detail(0, "nGrpAm" & Format(poSplit.SetNo, "000")) Then
                            GoTo endProc
                        End If

                        'detail is always set to row 0 
                        poSplit.Detail(0, "nGrpAm" & Format(poSplit.SetNo, "000")) = loTxt.Text
                    ElseIf RadioButton2.Checked Then
                        If Replace(loTxt.Text, "%", "") > 100 Then loTxt.Text = 100

                        'detail is always set to row 0
                        poSplit.Detail(0, "nGrpAm" & Format(poSplit.SetNo, "000")) = (Replace(loTxt.Text, "%", "") / 100) * poSplit.SalesTotal
                    Else
                        If Not IsNumeric(loTxt.Text) Then
                            loTxt.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000"))
                            GoTo endProc
                        ElseIf loTxt.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000")) Then
                            GoTo endProc
                        End If

                        'check if quantity is valid
                        If DataGridView1.Item(4, pnActiveRow).Value < loTxt.Text Then loTxt.Text = 0

                        'we are using pnActiveRow as the basis here
                        poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000")) = loTxt.Text
                    End If

                    Call loadDetail()
                    If RadioButton3.Checked Then Call loadDetailSplitted()
                    Call showComputation()
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

        If pnLoadx = 1 Then
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

            If RadioButton3.Checked Then Call loadDetailSplitted()

            txtField00.Focus()
            txtField00.SelectAll()
        End If
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

    Private Sub initGrid(Optional ByVal lnRows As Integer = 1)
        With DataGridView1
            .RowCount = lnRows

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
            .Columns(1).Width = 200
            .Columns(2).Width = 80
            .Columns(3).Width = 80
            .Columns(4).Width = 80
            .Columns(5).Width = 80

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
        End With
    End Sub

    Private Sub initGridAmount(Optional ByVal lnRows As Integer = 1)
        With DataGridView1
            .RowCount = lnRows

            'Set No of Columns
            .ColumnCount = 4

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "GROUP NAME"
            .Columns(2).HeaderText = "SHARE AMOUNT"
            .Columns(3).HeaderText = "PERCENTAGE"

            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 150
            .Columns(2).Width = 105
            .Columns(3).Width = 105

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
        End With
    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As System.EventArgs) Handles DataGridView1.Click
        With DataGridView1
            pnActiveRow = .CurrentCell.RowIndex

            If RadioButton3.Checked = False Then
                cmbField.SelectedIndex = pnActiveRow
                cmbField.Focus()
            Else
                showComputation()

                '.ClearSelection()
                '.Rows(pnActiveRow).Selected = True

                txtField02.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000"))
            End If

            txtField02.Focus()
            txtField02.SelectAll()
        End With
    End Sub

    Private Sub cmbField_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbField.SelectedIndexChanged
        If pnLoadx = 1 Then
            poSplit.SetNo = cmbField.SelectedIndex + 1

            loadDetail()

            If RadioButton3.Checked Then
                loadDetailSplitted()
            Else
                'With DataGridView1
                '    pnActiveRow = cmbField.SelectedIndex
                '    .ClearSelection()
                '    .Rows(pnActiveRow).Selected = True
                'End With
            End If

            showComputation()
        End If
    End Sub

    Private Sub picButton00_Click(sender As Object, e As System.EventArgs) Handles picButton00.Click, picButton01.Click, picButton02.Click
        If RadioButton3.Checked = False Then Exit Sub

        Dim loPic As PictureBox
        loPic = CType(sender, System.Windows.Forms.PictureBox)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loPic.Name, 10))


        Select Case lnIndex
            Case 0 'Remove
                poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000")) = 0
            Case 1 'Deduct
                If DataGridView1.Item(5, pnActiveRow).Value = 0 Then Exit Sub

                poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000")) -= 1
            Case 2 'Add
                If DataGridView1.Item(4, pnActiveRow).Value = 0 Then Exit Sub

                poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000")) += 1
        End Select

        Call loadDetail()
        Call loadDetailSplitted()
    End Sub

    Private Sub picButton00_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles picButton00.MouseDown, picButton01.MouseDown, picButton02.MouseDown
        Dim loPic As PictureBox
        loPic = CType(sender, System.Windows.Forms.PictureBox)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loPic.Name, 10))

        Select Case lnIndex
            Case 0
                loPic.BackgroundImage = My.Resources.Mouse_over_x
            Case 1
                loPic.BackgroundImage = My.Resources.Mouse_over_minus
            Case 2
                loPic.BackgroundImage = My.Resources.Mouse_over_plus
        End Select
    End Sub

    Private Sub picButton00_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles picButton00.MouseUp, picButton01.MouseUp, picButton02.MouseUp
        Dim loPic As PictureBox
        loPic = CType(sender, System.Windows.Forms.PictureBox)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loPic.Name, 10))

        Select Case lnIndex
            Case 0
                loPic.BackgroundImage = My.Resources.Mouse_Up_x
            Case 1
                loPic.BackgroundImage = My.Resources.Mouse_Up_minus
            Case 2
                loPic.BackgroundImage = My.Resources.Mouse_Up_plus
        End Select
    End Sub

    Private Sub InitializeDataGridSplit()
        With DataGridView2
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

        With DataGridView2.ColumnHeadersDefaultCellStyle
            .BackColor = Color.Navy
            .ForeColor = Color.White
            .Font = New Font(DataGridView1.Font, FontStyle.Bold)
        End With
    End Sub

    Private Sub initGridSplit()
        With DataGridView2
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
            .Columns(1).Width = 95
            .Columns(2).Width = 160
            .Columns(3).Width = 40
            .Columns(4).Width = 75
            .Columns(5).Width = 80

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
        End With
    End Sub

    Private Sub txtField_Click(sender As Object, e As System.EventArgs) Handles txtField00.Click, txtField02.Click
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                Case 0
                    txtField00.SelectAll()
                Case 2
                    txtField02.SelectAll()
            End Select
        End If
    End Sub

    Private Sub txtField02_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtField02.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtField00_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtField00.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class