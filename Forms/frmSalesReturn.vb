Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmSalesReturn
    Private WithEvents poReturn As SalesReturn
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_nSales As Decimal
    Private p_nDiscount As Decimal
    Private p_bCancelled As Boolean
    Private p_nTerminal As Integer
    Private p_nActiveRow As Integer

    WriteOnly Property SalesReturn() As SalesReturn
        Set(ByVal oSalesReturn As SalesReturn)
            poReturn = oSalesReturn
        End Set
    End Property

    WriteOnly Property SalesTotal As Decimal
        Set(value As Decimal)
            p_nSales = value
        End Set
    End Property

    WriteOnly Property Discounts As Decimal
        Set(value As Decimal)
            p_nDiscount = value
        End Set
    End Property

    WriteOnly Property Terminal As Integer
        Set(value As Integer)
            p_nTerminal = value
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
        initController(1)
        If pnLoadx = 0 Then
            clearFields()
            InitializeDataGrid()
            initGrid()

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
            Case 0
                If txtOthers01.Text = "" Then Exit Sub
                If Not IsNumeric(txtField05.Text) Or txtField05.Text = 0 Then
                    MsgBox("Invalid return amount. Please verify your entry.", MsgBoxStyle.Critical, "Warning")
                    txtField05.Text = "0.00"
                    txtField05.Focus()
                    Exit Sub
                End If

                poReturn.Master("nTranAmtx") = txtField05.Text
                p_bCancelled = Not poReturn.SaveTransaction()

                If Not p_bCancelled Then
                    Me.Close()
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
        Me.Location = New Point(1, 85)
        Me.Visible = True

        txtOthers01.MaxLength = 64
    End Sub

    Private Sub clearFields()
        lblAmount.Text = "0.00"
        lblMaster04.Text = "0.00"
        lblMaster13.Text = "0.00"
        lblMaster14.Text = "0.00"
        lblMaster15.Text = "0.00"
        lblMaster17.Text = "0.00"

        txtOthers00.Text = Format(p_nTerminal, "00")

        txtField00.Text = ""
        txtField01.Text = ""
        txtField02.Text = ""

        txtOthers01.Text = ""
        txtOthers02.Text = ""
        txtOthers03.Text = ""
        txtOthers04.Text = "0"
        txtField05.Text = CDbl("0.00")

        txtOthers01.Focus()
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
                    poReturn.Master("sRemarksx") = loTxt.Text
                Case 5
                    If Not IsNumeric(loTxt.Text) Then
                        MsgBox("Invalid return amount. Please verify your entry.", MsgBoxStyle.Critical, "Warning")
                        loTxt.Text = "0.00"
                    End If
                    poReturn.Master("nTranAmtx") = loTxt.Text
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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If poReturn.getORNumber(txtOthers01.Text, txtOthers00.Text) Then
            loadDetail()
        Else
            initGrid()
        End If
    End Sub

    Private Sub InitializeDataGrid()
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
            .Font = New Font(DataGridView2.Font, FontStyle.Bold)
        End With
    End Sub

    Private Sub initController(ByVal fnValue As Integer)
        Dim lbShow As Boolean = IIf(fnValue = 1, True, False)

        If lbShow Then
            Panel1.Enabled = False
            txtOthers04.Enabled = False
            DataGridView2.Enabled = False
        Else
            Panel1.Enabled = True
            txtOthers04.Enabled = True
            DataGridView2.Enabled = True
        End If

    End Sub


    Private Sub initGrid()
        With DataGridView2
            .RowCount = 0

            'Set No of Columns
            .ColumnCount = 6

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "Barrcode"
            .Columns(2).HeaderText = "Description"
            .Columns(3).HeaderText = "Qty"
            .Columns(4).HeaderText = "Unit Price"
            .Columns(5).HeaderText = "Return"

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

    Private Sub loadDetail()
        initController(2)
        Dim lnComplmt As Decimal = 0
        With DataGridView2
            .RowCount = IIf(poReturn.ItemCount = 0, 1, poReturn.ItemCount)

            For lnCtr = 0 To poReturn.ItemCount - 1
                .Item(0, lnCtr).Value = lnCtr + 1
                .Item(1, lnCtr).Value = poReturn.Detail(lnCtr, "sBarcodex")
                .Item(2, lnCtr).Value = poReturn.Detail(lnCtr, "sBriefDsc")
                .Item(3, lnCtr).Value = IIf(poReturn.Detail(lnCtr, "cReversex") = "-", poReturn.Detail(lnCtr, "cReversex"), "") & poReturn.Detail(lnCtr, "nQuantity")
                .Item(4, lnCtr).Value = Format(poReturn.Detail(lnCtr, "nUnitPrce") * _
                                                (100 - poReturn.Detail(lnCtr, "nDiscount")) / 100 -
                                                        poReturn.Detail(lnCtr, "nAddDiscx"), "#,##0.00")
                .Item(5, lnCtr).Value = poReturn.Detail(lnCtr, "nReturnxx")

                If poReturn.Detail(lnCtr, "nComplmnt") > 0 Then
                    lnComplmt += poReturn.Detail(lnCtr, "nComplmnt") * (poReturn.Detail(lnCtr, "nUnitPrce") * _
                                                (100 - poReturn.Detail(lnCtr, "nDiscount")) / 100 -
                                                        poReturn.Detail(lnCtr, "nAddDiscx"))
                End If
            Next

            p_nActiveRow = 0
            .ClearSelection()
            .Rows(p_nActiveRow).Selected = True
            txtOthers04.Text = poReturn.Detail(p_nActiveRow, "nReturnxx")
        End With

        txtField00.Text = poReturn.Master("sTransNox")
        txtField01.Text = Format(poReturn.Master("dTransact"), "MMM dd, yyyy")
        txtField02.Text = poReturn.Master("sRemarksx")

        txtOthers01.Text = poReturn.Master("sORNumber")
        txtOthers02.Text = Format(poReturn.Master("dReceiptx"), "MMM dd, yyyy")
        txtOthers03.Text = poReturn.Master("sCashierx")

        lblMaster04.Text = FormatNumber(poReturn.Master("nTranTotl") - poReturn.Master("nVoidTotl"), 2) 'sales total

        lblMaster13.Text = FormatNumber(poReturn.Master("nVATSales"), 2) 'vat sales
        lblMaster14.Text = FormatNumber(poReturn.Master("nVATAmtxx"), 2) 'vat amount
        lblMaster15.Text = FormatNumber(poReturn.Master("nNonVATxx"), 2) 'non vat 

        lblMaster17.Text = FormatNumber(poReturn.Master("nTotDiscx") - lnComplmt, 2)

        lblAmount.Text = FormatNumber(CDbl(lblMaster04.Text) - CDbl(lblMaster17.Text), 2) 'amount due
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        clearFields()
        initGrid()
    End Sub

    Private Sub DataGridView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView2.Click
        p_nActiveRow = DataGridView2.CurrentRow.Index

        txtOthers04.Text = poReturn.Detail(p_nActiveRow, "nReturnxx")
        txtOthers04.Focus()
        txtOthers04.SelectAll()
    End Sub

    Private Sub txtOthers04_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOthers04.GotFocus
        txtOthers04.SelectAll()
    End Sub

    Private Sub txtOthers04_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOthers04.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                If Not IsNumeric(txtOthers04.Text) Then txtOthers04.Text = 0

                If txtOthers04.Text + poReturn.Detail(p_nActiveRow, "nReturnxx") > poReturn.Detail(p_nActiveRow, "nQuantity") Then
                    txtOthers04.Text = poReturn.Detail(p_nActiveRow, "nQuantity")
                End If

                poReturn.Detail(p_nActiveRow, "nReturnxx") = txtOthers04.Text
        End Select
    End Sub


    Private Sub txtOthers04_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOthers04.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtOthers04_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOthers04.LostFocus
        If Not IsNumeric(txtOthers04.Text) Then txtOthers04.Text = 0

        If txtOthers04.Text + poReturn.Detail(p_nActiveRow, "nReturnxx") > poReturn.Detail(p_nActiveRow, "nQuantity") Then
            txtOthers04.Text = poReturn.Detail(p_nActiveRow, "nQuantity")
        End If

        poReturn.Detail(p_nActiveRow, "nReturnxx") = txtOthers04.Text
    End Sub

    Private Sub poReturn_DisplayReturn(ByVal Row As Integer) Handles poReturn.DisplayReturn
        With DataGridView2
            .Item(5, Row).Value = poReturn.Detail(Row, "nReturnxx")

            Dim lnCtr As Integer
            Dim lnTotal As Double = 0

            For lnCtr = 0 To .Rows.Count - 1
                lnTotal += .Item(5, lnCtr).Value * .Item(4, lnCtr).Value
            Next

            txtField05.Text = Format(lnTotal, "#,##0.00")
        End With
    End Sub

    Private Sub txtOthers01_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOthers01.Click
        txtOthers01.SelectAll()
    End Sub

    Private Sub txtOthers01_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOthers01.KeyDown
        If txtOthers01.Text = "" Then Exit Sub
        Select e.KeyCode
            Case Keys.Return
                If poReturn.getORNumber(txtOthers01.Text, txtOthers00.Text) Then
                    loadDetail()
                Else
                    initGrid()
                End If
        End Select
    End Sub
End Class