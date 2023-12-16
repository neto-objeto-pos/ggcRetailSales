Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms
Imports ggcRetailParams

Public Class frmAddOns
    Private WithEvents p_oAddOn As clsPromoAddOn
    Private pnLoadx As Integer
    Private poControl As Control
    Private p_bCancelled As Boolean
    Private pnActiveRow As Integer

    ReadOnly Property Cancelled() As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    Property AddOn() As clsPromoAddOn
        Set(ByVal oAddOn As clsPromoAddOn)
            p_oAddOn = oAddOn
        End Set
        Get
            Return p_oAddOn
        End Get
    End Property

    Private Sub frmAddOns_Keydown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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

    Private Sub frmAddOns_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If pnLoadx = 0 Then
            setVisible()

            clearFields()
            loadDetail()

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
                p_bCancelled = False
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
        Me.Location = New Point(610, 110)
        Me.Visible = True
    End Sub

    Private Sub clearFields()
        With p_oAddOn
            lblMaster02.Text = .Master(2)

        End With
    End Sub

    Private Sub loadDetail(Optional ByVal lnRow As Integer = 0)
        Dim lnCtr As Integer

        Call InitializeDataGrid()
        Call initGrid()

        With DataGridView1
            .RowCount = p_oAddOn.ItemCount

            For lnCtr = 0 To p_oAddOn.ItemCount - 1
                .Item(0, lnCtr).Value = lnCtr + 1
                .Item(1, lnCtr).Value = p_oAddOn.Detail(lnCtr, "sBriefDsc")
                .Item(2, lnCtr).Value = Format(p_oAddOn.Detail(lnCtr, "nUnitPrce"), "#,##0.00")
                .Item(3, lnCtr).Value = p_oAddOn.Detail(lnCtr, "nQuantity")
                .Item(4, lnCtr).Value = p_oAddOn.Detail(lnCtr, "xBriefDsc")
                .Item(5, lnCtr).Value = IIf(p_oAddOn.Detail(lnCtr, "cSelected") = "0", "NO", "YES")
            Next

            pnActiveRow = lnRow
            .ClearSelection()
            .Rows(pnActiveRow).Selected = True

            setFieldInfo()
        End With
    End Sub

    Private Sub setFieldInfo()
        lblMaster02.Text = p_oAddOn.Master("sBriefDsc")

        txtField02.Text = p_oAddOn.Detail(pnActiveRow, "sBriefDsc")
        CheckBox1.Checked = IIf(p_oAddOn.Detail(pnActiveRow, "cSelected") = "0", False, True)
        CheckBox1.Focus()
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
            .ColumnCount = 6

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "PROMO ITEM"
            .Columns(2).HeaderText = "AMOUNT"
            .Columns(3).HeaderText = "QTY"
            .Columns(4).HeaderText = "REPLACE ITEM"
            .Columns(5).HeaderText = "SELECTED"

            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 170
            .Columns(2).Width = 85
            .Columns(3).Width = 50
            .Columns(4).Width = 170
            .Columns(5).Width = 90

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
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

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

    Private Sub DataGridView1_Click(sender As Object, e As System.EventArgs) Handles DataGridView1.Click
        With DataGridView1
            pnActiveRow = .CurrentCell.RowIndex

            Call setFieldInfo()
        End With
    End Sub

    Private Sub frmAddOns_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Me.Focus()
    End Sub

    Private Sub CheckBox1_Click(sender As Object, e As System.EventArgs) Handles CheckBox1.Click
        p_oAddOn.Detail(pnActiveRow, "cSelected") = IIf(CheckBox1.Checked = True, "1", "0")

        Call loadDetail(pnActiveRow)
    End Sub
End Class