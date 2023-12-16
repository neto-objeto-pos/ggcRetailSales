Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms
Imports ggcAppDriver

Public Class frmPaySplitted
    Private WithEvents poSplit As SplitOrder
    Private pnLoadx As Integer
    Private poControl As Control
    Private p_bCancelled As Boolean
    Private p_sTransNox As String
    Private pnActiveRow As Integer

    ReadOnly Property Cancelled() As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    ReadOnly Property TransNo As String
        Get
            Return p_sTransNox
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
                poSplit.SetNo = pnActiveRow + 1

                If poSplit.Master("cPaidxxxx") = 1 Then
                    p_bCancelled = True
                Else
                    poSplit.SourceNo = p_sTransNox
                    p_bCancelled = False
                End If
            Case 1 'close
                p_bCancelled = True
        End Select

        Me.Close()
        Me.Dispose()
endProc:
        Exit Sub
    End Sub

    Private Sub setVisible()
        Me.Visible = False
        Me.TransparencyKey = Nothing
        Me.Location = New Point(1, 85)
        Me.Visible = True
    End Sub

    Private Sub clearFields()
        With poSplit
            poSplit.SetNo = 1
            lblBill.Text = Format(poSplit.Master("nTranTotl"), "#,##0.00")

            If .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByAmount Then
                RadioButton1.Checked = True
            ElseIf .SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByPercentage Then
                RadioButton2.Checked = True
            Else
                RadioButton3.Checked = True
            End If
        End With
    End Sub

    Private Sub loadDetail()
        Dim lnCtr As Integer

        Call initGrid()

        If poSplit.SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByAmount Then
            RadioButton1.Checked = True
        ElseIf poSplit.SplitType = ggcRetailSales.SplitOrder.xeSplitType.xeSplitByPercentage Then
            RadioButton2.Checked = True
        Else
            RadioButton3.Checked = True
        End If

        With DataGridView1
            .RowCount = poSplit.MasterCount

            If RadioButton3.Checked = True Then
                For lnCtr = 0 To poSplit.GroupNo - 1
                    poSplit.SetNo = lnCtr + 1

                    .Item(0, lnCtr).Value = lnCtr + 1
                    .Item(1, lnCtr).Value = "SET " & lnCtr + 1
                    .Item(2, lnCtr).Value = Format(poSplit.Master("nAmountxx"), "#,##0.00")
                    .Item(3, lnCtr).Value = Math.Round((poSplit.Master("nAmountxx") / poSplit.SalesTotal) * 100, 2) & "%"

                    Select Case poSplit.Master("cPaidxxxx")
                        Case 0
                            .Item(4, lnCtr).Value = "OPEN"
                        Case 1
                            .Item(4, lnCtr).Value = "CLOSED"
                        Case 2
                            .Item(4, lnCtr).Value = "POSTED"
                        Case 3
                            .Item(4, lnCtr).Value = "CANCELLED"
                    End Select
                    .Item(5, lnCtr).Value = IFNull(poSplit.Master("nPrntBill"), 0)
                    If IsDBNull(poSplit.Master("dPrntBill")) Then
                        .Item(6, lnCtr).Value = ""
                    Else
                        .Item(6, lnCtr).Value = Format(poSplit.Master("dPrntBill"), xsDATE_MEDIUM)
                    End If
                Next
            Else
                For lnCtr = 0 To poSplit.GroupNo - 1
                    poSplit.SetNo = lnCtr + 1

                    .Item(0, lnCtr).Value = lnCtr + 1
                    .Item(1, lnCtr).Value = "SET " & lnCtr + 1
                    .Item(2, lnCtr).Value = Format(poSplit.Detail(0, "nGrpAm" & Format(lnCtr + 1, "000")), "#,##0.00")
                    .Item(3, lnCtr).Value = Math.Round((poSplit.Detail(0, "nGrpAm" & Format(lnCtr + 1, "000")) / poSplit.SalesTotal) * 100, 2) & "%"

                    Select Case poSplit.Master("cPaidxxxx")
                        Case 0
                            .Item(4, lnCtr).Value = "OPEN"
                        Case 1
                            .Item(4, lnCtr).Value = "CLOSED"
                        Case 2
                            .Item(4, lnCtr).Value = "POSTED"
                        Case 3
                            .Item(4, lnCtr).Value = "CANCELLED"
                    End Select
                    .Item(5, lnCtr).Value = IFNull(poSplit.Master("nPrntBill"), 0)
                    If IsDBNull(poSplit.Master("dPrntBill")) Then
                        .Item(6, lnCtr).Value = ""
                    Else
                        .Item(6, lnCtr).Value = Format(poSplit.Master("dPrntBill"), xsDATE_MEDIUM)
                    End If
                Next
            End If

            poSplit.SetNo = 1
            p_sTransNox = poSplit.Master("sTransNox")
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
            .ColumnCount = 7

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "GROUP NAME"
            .Columns(2).HeaderText = "SHARE AMOUNT"
            .Columns(3).HeaderText = "PERCENTAGE"
            .Columns(4).HeaderText = "STATUS"
            .Columns(5).HeaderText = "PRINTED"
            .Columns(6).HeaderText = "DATE PRINTED"

            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 125
            .Columns(2).Width = 80
            .Columns(3).Width = 80
            .Columns(4).Width = 80
            .Columns(5).Width = 40
            .Columns(6).Width = 40


            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'Set No of Rows
            .RowCount = 1
        End With
    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As System.EventArgs) Handles DataGridView1.Click
        With DataGridView1
            pnActiveRow = .CurrentCell.RowIndex

            poSplit.SetNo = pnActiveRow + 1

            lblBill.Text = Format(poSplit.Master("nTranTotl"), "#,##0.00")
            p_sTransNox = poSplit.Master("sTransNox") 'sReferNox 2018-03-01

            loadDetailSplitted()
        End With
    End Sub

    Private Sub frmPaySplitted_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Me.Focus()
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

    Private Sub loadDetailSplitted()
        initGridSplit()

        With DataGridView2
            If RadioButton1.Checked Or RadioButton2.Checked Then
                Dim lnCtr As Integer
                For lnCtr = 0 To poSplit.ItemCount - 1
                    '.RowCount = .RowCount + 1
                    .RowCount = IIf(poSplit.ItemCount = 0, 1, poSplit.ItemCount)
                    .Item(0, lnCtr).Value = lnCtr + 1
                    .Item(1, lnCtr).Value = poSplit.Detail(lnCtr, "sBarcodex")
                    .Item(2, lnCtr).Value = poSplit.Detail(lnCtr, "sBriefDsc")
                    .Item(3, lnCtr).Value = IIf(poSplit.Detail(lnCtr, "cReversex") = "-", poSplit.Detail(lnCtr, "cReversex"), "") & poSplit.Detail(lnCtr, "nQuantity")
                    .Item(4, lnCtr).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce"), "#,##0.00")
                    .Item(5, lnCtr).Value = Format(.Item(3, lnCtr).Value * .Item(4, lnCtr).Value, "#,##0.00")
                Next
            Else
                Dim lnCtr As Integer
                .RowCount = IIf(poSplit.ItemCount = 0, 1, poSplit.ItemCount)
                For lnCtr = 0 To poSplit.ItemCount - 1
                    If poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000")) > 0 Then
                        '.RowCount = .RowCount + 1
                        .Item(0, lnCtr).Value = lnCtr + 1
                        .Item(1, lnCtr).Value = poSplit.Detail(lnCtr, "sBarcodex")
                        .Item(2, lnCtr).Value = poSplit.Detail(lnCtr, "sBriefDsc")
                        .Item(3, lnCtr).Value = poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000"))
                        .Item(4, lnCtr).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce"), "#,##0.00")
                        .Item(5, lnCtr).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce") * poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000")), "#,##0.00")

                        '.Item(0, lnCtr).Value = lnCtr + 1
                        '.Item(1, lnCtr).Value = poSplit.Detail(lnCtr, "sBarcodex")
                        '.Item(2, lnCtr).Value = poSplit.Detail(lnCtr, "sBriefDsc")
                        '.Item(3, lnCtr).Value = poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000"))
                        '.Item(4, lnCtr).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce"), "#,##0.00")
                        '.Item(5, lnCtr).Value = Format(poSplit.Detail(lnCtr, "nUnitPrce") * poSplit.Detail(lnCtr, "nGrpQt" & Format(poSplit.SetNo, "000")), "#,##0.00")
                    End If
                Next

                '.ClearSelection()
                'If .RowCount = 0 Then .RowCount = 1
                '.Rows(.Rows.Count - 1).Selected = True
            End If
        End With

        Call showComputation()
    End Sub

    Private Sub showComputation()
        lblMaster04.Text = FormatNumber(poSplit.Master("nTranTotl"), 2)
        lblMaster13.Text = FormatNumber(poSplit.Master("nVATSales"), 2) 'vat sales
        lblMaster14.Text = FormatNumber(poSplit.Master("nVATAmtxx"), 2) 'vat amount
        lblMaster15.Text = FormatNumber(poSplit.Master("nNonVATxx"), 2) 'non vat
        lblMaster17.Text = FormatNumber(poSplit.Master("nDiscount") + poSplit.Master("nVatDiscx") + poSplit.Master("nPWDDiscx"), 2)
        lblAmount.Text = FormatNumber(CDbl(lblMaster04.Text) - CDbl(lblMaster17.Text), 2) 'amount due
        lblAmount.Text = FormatNumber(IIf(poSplit.IsWithSCharge, poSplit.Master("nTranTotl") + (poSplit.Master("nVATSales") * (poSplit.SCharge / 100)), lblAmount.Text), 2)
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
End Class