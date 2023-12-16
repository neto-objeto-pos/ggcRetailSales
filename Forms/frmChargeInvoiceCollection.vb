Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms

Imports ggcAppDriver

Public Class frmChargeInvoiceCollection
    Private p_oApp As GRider
    Private p_oCharge As ChargeInvoiceCollection

    Private pbCloseForm As Boolean
    Private pnLoadx As Integer
    Private poControl As Control

    WriteOnly Property AppDriver()
        Set(ByVal value)
            p_oApp = value
        End Set
    End Property

    ReadOnly Property Cancelled()
        Get
            Return pbCloseForm
        End Get
    End Property

    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 0
                If gridCharge.Rows(0).Cells(2).Value = "" Then Exit Sub
                If p_oCharge.SaveTransaction Then
                    pbCloseForm = False
                Else
                    MsgBox("Unable to pay charge invoices.", MsgBoxStyle.Critical, "Warning")
                End If
                Me.Close()
                Me.Dispose()
            Case 1
                pbCloseForm = True
                Me.Close()
                Me.Dispose()
            Case 2 'pay 
                If isEntryOk() Then
                    If Not LockTransaction() Then
                        Call initTransaction()
                    End If
                End If

            Case 4 'reset
                Call initTransaction()
            Case 5 'remove

        End Select
endProc:
        Exit Sub
    End Sub

    Function isEntryOk() As Boolean
        For Each rowOuter As DataGridViewRow In gridInvoices.Rows
            For Each rowInner As DataGridViewRow In gridCharge.Rows
                If rowOuter.Cells(3).Value = rowInner.Cells(3).Value Then
                    Return False
                End If
            Next
        Next

        Return True
    End Function

    Private Sub initTransaction()
        If p_oCharge.NewTransaction Then
            Call clearFields()
            Call InitGrid()
            Call InitGrid2()
            Call loadDetail()
        End If
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
    Private Sub setVisible()
        Me.Visible = False
        Me.TransparencyKey = Nothing
        Me.Location = New Point(507, 90)
        Me.Visible = True
    End Sub

    Private Sub frmChargeInvoiceCollection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        setVisible()

        If pnLoadx = 0 Then
            clearFields()
            InitGrid()
            InitGrid2()

            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpCancelHandler(Me, GetType(TextBox), "txtField", "Validating", AddressOf txtField_Validating)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)

            p_oCharge = New ChargeInvoiceCollection(p_oApp)
            If Not p_oCharge.NewTransaction() Then
                pbCloseForm = True
                Me.Close()
            End If

            Call loadDetail()

            pnLoadx = 1
        End If
    End Sub

    Private Sub clearFields()

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

    Private Sub InitGrid()
        With gridInvoices.ColumnHeadersDefaultCellStyle
            .BackColor = Color.Navy
            .ForeColor = Color.White
            .Font = New Font(gridInvoices.Font, FontStyle.Bold)
        End With

        With gridInvoices
            .Rows.Clear()
            .ColumnCount = 7
            .RowCount = 1

            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False

            .ForeColor = Color.Black

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .GridColor = SystemColors.ActiveBorder
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False


            .Columns(0).Width = 30
            .Columns(1).Width = 45
            .Columns(2).Width = 110
            .Columns(3).Width = 50
            .Columns(4).Width = 45
            .Columns(5).Width = 45
            .Columns(6).Width = 50

            .Columns(0).Name = ""
            .Columns(1).Name = "Date"
            .Columns(2).Name = "Customer"
            .Columns(3).Name = "Invoice No."
            .Columns(4).Name = "Total Amount"
            .Columns(5).Name = "Paid Amount"
            .Columns(6).Name = "Balance"

            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim lnCtr As Integer
            For lnCtr = 0 To 6
                .Columns(lnCtr).SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            .Rows(0).Cells(0).ReadOnly = False
            .Rows(0).Selected = True
            .Enabled = True
        End With
    End Sub

    Private Sub InitGrid2()
        With gridCharge.ColumnHeadersDefaultCellStyle
            .BackColor = Color.Navy
            .ForeColor = Color.White
            .Font = New Font(gridInvoices.Font, FontStyle.Bold)
        End With

        With gridCharge
            .Rows.Clear()
            .ColumnCount = 5
            .RowCount = 1

            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False

            .ForeColor = Color.Black

            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .GridColor = SystemColors.ActiveBorder
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False

            .Columns(0).Width = 30
            .Columns(1).Width = 80
            .Columns(2).Width = 100
            .Columns(3).Width = 80
            .Columns(4).Width = 85

            .Columns(0).Name = "No"
            .Columns(1).Name = "Date"
            .Columns(2).Name = "Customer"
            .Columns(3).Name = "Invoice No."
            .Columns(4).Name = "Amount"

            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            Dim lnCtr As Integer
            For lnCtr = 0 To 4
                .Columns(lnCtr).SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            .Rows(0).Selected = True
        End With
    End Sub

    Private Sub loadDetail()
        With gridInvoices
            .RowCount = p_oCharge.ChargeCount

            For lnCtr As Int32 = 0 To p_oCharge.ChargeCount - 1
                .Item(1, lnCtr).Value = Format(p_oCharge.Charge(lnCtr, "dTransact"), xsDATE_MEDIUM)
                .Item(2, lnCtr).Value = p_oCharge.Charge(lnCtr, "sClientNm")
                .Item(3, lnCtr).Value = Strings.Right(p_oCharge.Charge(lnCtr, "sTransNox"), 8)
                .Item(4, lnCtr).Value = Format(p_oCharge.Charge(lnCtr, "nAmountxx"), xsDECIMAL)
                .Item(5, lnCtr).Value = Format(p_oCharge.Charge(lnCtr, "nAmtPaidx"), xsDECIMAL)
                .Item(6, lnCtr).Value = Format(p_oCharge.Charge(lnCtr, "nAmountxx") - p_oCharge.Charge(lnCtr, "nAmtPaidx"), xsDECIMAL)

                .Rows(lnCtr).Cells(1).ReadOnly = True
                .Rows(lnCtr).Cells(2).ReadOnly = True
                .Rows(lnCtr).Cells(3).ReadOnly = True
                .Rows(lnCtr).Cells(4).ReadOnly = True
                .Rows(lnCtr).Cells(5).ReadOnly = True
                .Rows(lnCtr).Cells(6).ReadOnly = True
            Next
        End With
    End Sub

    Private Function LockTransaction() As Boolean
        Dim lnCtr As Integer
        Dim lbChecked As Boolean

        With gridInvoices

            For lnCtr = 0 To p_oCharge.ChargeCount - 1
                Dim checkCell As DataGridViewCheckBoxCell = _
                                CType(gridInvoices.Rows(lnCtr).Cells(0),  _
                                DataGridViewCheckBoxCell)

                If CType(checkCell.Value, [Boolean]) = True Then
                    If p_oCharge.Master("sClientID") <> "" Or p_oCharge.Master("sClientNm") <> "" Then
                        If p_oCharge.Master("sClientID") <> IFNull(p_oCharge.Charge(lnCtr, "sClientID")) _
                            Or p_oCharge.Master("sClientNm") <> IFNull(p_oCharge.Charge(lnCtr, "sClientNm")) Then
                            MsgBox("It seems that you have selected different accounts to pay." & vbCrLf & _
                                    "Charge invoice transaction will reset. You must select same accounts.", MsgBoxStyle.Critical, "Warning")

                            Return False
                        End If
                    End If

                    p_oCharge.Master("sClientID") = IFNull(p_oCharge.Charge(lnCtr, "sClientID"))
                    p_oCharge.Master("sClientNm") = p_oCharge.Charge(lnCtr, "sClientNm")
                    p_oCharge.Master("sAddressx") = p_oCharge.Charge(lnCtr, "sAddressx")


                    p_oCharge.AddDetail()
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "sSourceNo") = p_oCharge.Charge(lnCtr, "sSourceNo")
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "sSourceCd") = p_oCharge.Charge(lnCtr, "sSourceCd")
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "nAmountxx") = p_oCharge.Charge(lnCtr, "nAmountxx")
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "nDiscount") = p_oCharge.Charge(lnCtr, "nDiscount")
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "nVATDiscx") = p_oCharge.Charge(lnCtr, "nVATDiscx")
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "nPWDDiscx") = p_oCharge.Charge(lnCtr, "nPWDDiscx")
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "dTransact") = p_oCharge.Charge(lnCtr, "dTransact")
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "sClientNm") = p_oCharge.Charge(lnCtr, "sClientNm")
                    p_oCharge.Detail(p_oCharge.ItemCount - 1, "sInvceNox") = Strings.Right(p_oCharge.Charge(lnCtr, "sTransNox"), 8)

                    If Not lbChecked Then lbChecked = True
                End If
            Next

            If lbChecked Then .Enabled = False
        End With

        Call InitGrid2()
        With gridCharge
            If p_oCharge.ItemCount <> 0 Then
                .RowCount = p_oCharge.ItemCount
                For lnCtr = 0 To p_oCharge.ItemCount - 1
                    .Item(0, lnCtr).Value = lnCtr + 1
                    .Item(1, lnCtr).Value = Format(p_oCharge.Detail(lnCtr, "dTransact"), xsDATE_MEDIUM)
                    .Item(2, lnCtr).Value = p_oCharge.Detail(lnCtr, "sClientNm")
                    .Item(3, lnCtr).Value = p_oCharge.Detail(lnCtr, "sInvceNox")
                    .Item(4, lnCtr).Value = Format(p_oCharge.Detail(lnCtr, "nAmountxx"), xsDECIMAL)
                Next
            End If
        End With

        Return True
    End Function

    Private Sub gridInvoices_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridInvoices.CellContentClick
        With gridInvoices
            If e.ColumnIndex = 0 Then
                Dim checkCell As DataGridViewCheckBoxCell = _
                    CType(.Rows(e.RowIndex).Cells(0),  _
                    DataGridViewCheckBoxCell)

                .Rows(e.RowIndex).Cells(0).Value = Not CType(checkCell.Value, [Boolean])
            End If
        End With
    End Sub

    Private Sub gridInvoices_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridInvoices.Click
        With gridInvoices
            If .CurrentCell.ColumnIndex = 0 Then
                .Rows(.CurrentRow.Index).Selected = False
                If .Rows.Count = .CurrentRow.Index + 1 Then
                    .Rows(0).Selected = True
                Else
                    .Rows(.CurrentRow.Index + 1).Selected = True
                End If
            End If
        End With
    End Sub
End Class