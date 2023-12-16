Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms
Imports ggcAppDriver

Public Class frmDiscountMerger
    Private WithEvents p_oTrans As New_Sales_Order
    Private pnLoadx As Integer
    Private poControl As Control
    Private p_bCancelled As Boolean
    Private pnActiveRow As Integer

    ReadOnly Property Cancelled() As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property

    WriteOnly Property TransClass() As New_Sales_Order
        Set(ByVal value As New_Sales_Order)
            p_oTrans = value
        End Set
    End Property

    Private Sub frmfrmDiscountMerger_Keydown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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

    Private Sub frmfrmDiscountMerger_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If pnLoadx = 0 Then
            setVisible()

            Call InitializeTableOrder()
            Call InitializeOrderDetail()
            Call initGridDataTable()
            Call initGridMergeOrder()

            clearFields()

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
                'If Not isEntryOK() Then GoTo endProc

                'If Not poSplit.SaveTransaction() Then GoTo endProc

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
    End Sub

    Private Sub showComputation()
        'lblMaster04.Text = FormatNumber(poSplit.Master("nTranTotl"), 2)
        'lblMaster13.Text = FormatNumber(poSplit.Master("nVATSales"), 2) 'vat sales
        'lblMaster14.Text = FormatNumber(poSplit.Master("nVATAmtxx"), 2) 'vat amount
        'lblMaster15.Text = FormatNumber(poSplit.Master("nNonVATxx"), 2) 'non vat
        'lblMaster17.Text = FormatNumber(poSplit.Master("nDiscount") + poSplit.Master("nVatDiscx") + poSplit.Master("nPWDDiscx"), 2)
        'lblAmount.Text = FormatNumber(CDbl(lblMaster04.Text) - CDbl(lblMaster17.Text), 2) 'amount due
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

    Private Sub InitializeTableOrder()
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

    Private Sub initGridDataTable(Optional ByVal lnRows As Integer = 1)
        With DataGridView1
            .RowCount = lnRows

            'Set No of Columns
            .ColumnCount = 3

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "Table No"
            .Columns(2).HeaderText = "Remarks"

            'Set Column Sizes
            .Columns(0).Width = 30
            .Columns(1).Width = 200
            .Columns(2).Width = 80

            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable


            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

        End With
    End Sub

    Private Sub DataGridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.Click
        With DataGridView1
            pnActiveRow = .CurrentCell.RowIndex

            'If RadioButton3.Checked = False Then
            '    cmbField.SelectedIndex = pnActiveRow
            '    cmbField.Focus()
            'Else
            '    showComputation()

            '.ClearSelection()
            '.Rows(pnActiveRow).Selected = True

            'txtField02.Text = poSplit.Detail(pnActiveRow, "nGrpQt" & Format(poSplit.SetNo, "000"))
            'End If

            'txtField02.Focus()
            'txtField02.SelectAll()
        End With
    End Sub


    Private Sub InitializeOrderDetail()
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

    Private Sub initGridMergeOrder()
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

    Private Sub txtField_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                'Case 0
                '    txtField00.SelectAll()
                'Case 2
                '    txtField02.SelectAll()
            End Select
        End If
    End Sub

    Private Sub txtField02_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtField00_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class