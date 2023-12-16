Imports System.Threading
Imports System.Windows.Forms
Imports System.Drawing
Imports ggcAppDriver

Public Class frmDiscount

    Private Const pxeDiscNone As String = "1701"
    Private pnActiveRow As Integer
    Private pnLoadx As Integer
    Private poControl As Control
    Private p_nTotalsales As Decimal

    Private WithEvents poDiscount As Discount

    Private pbCancel As Boolean

    WriteOnly Property Discount() As Discount
        Set(ByVal oDiscount As Discount)
            poDiscount = oDiscount
        End Set
    End Property

    ReadOnly Property Cancel()
        Get
            Return pbCancel
        End Get
    End Property

    Private Sub initGrid()
        InitializeDataGrid()
        With dgvClients
            .RowCount = 0

            'Set No of Columns
            .ColumnCount = 3

            'Set Column Headers
            .Columns(0).HeaderText = "No"
            .Columns(1).HeaderText = "ID No"
            .Columns(2).HeaderText = "Client Name"
            'Set Column Sizes
            .Columns(0).Width = 50
            .Columns(1).Width = 100
            .Columns(2).Width = 155

            .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft

            'Set No of Rows
            .RowCount = 1

            pnActiveRow = 0
        End With

    End Sub

    Private Sub loadDetail()
        Dim lnCtr As Integer
        Dim lnRow As Integer

        Call initGrid()
        With dgvClients
            If poDiscount.ItemDetailCount > 0 Then
                lnRow = poDiscount.ItemDetailCount
                .RowCount = lnRow
                For lnCtr = 0 To lnRow - 1
                    .Rows(lnCtr).Cells(0).Value = lnCtr + 1
                    .Rows(lnCtr).Cells(1).Value = poDiscount.Detail(lnCtr, "sIDNumber")
                    .Rows(lnCtr).Cells(2).Value = poDiscount.Detail(lnCtr, "sClientNm")
                Next

                .ClearSelection()
                .CurrentCell = .Rows(lnRow - 1).Cells(0)
                .Rows(lnRow - 1).Selected = True

                setFieldValue(lnRow - 1)
            End If
        End With
    End Sub

    Private Sub setFieldValue(ByVal nRow As Integer)
        With dgvClients
            pnActiveRow = nRow
            txtField01.Text = poDiscount.Detail(pnActiveRow, "sIDNumber")
            txtField10.Text = poDiscount.Detail(pnActiveRow, "sClientNm")
            txtField01.Focus()

            'If poDiscount.ItemDetailCount > 0 Then
            '    Dim lnRow As Integer
            '    lnRow = poDiscount.ItemDetailCount
            '    .RowCount = lnRow
            '    For lnCtr = 0 To lnRow - 1
            '        .Rows(lnCtr).Cells(0).Value = lnCtr + 1
            '        .Rows(lnCtr).Cells(1).Value = poDiscount.Detail(lnCtr, "sIDNumber")
            '        .Rows(lnCtr).Cells(2).Value = poDiscount.Detail(lnCtr, "sClientNm")
            '    Next
            'End If
        End With
    End Sub

    Private Sub frmPayGC_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                pbCancel = True
                Me.Hide()
                'pbCancel = True
                'setVisible(False)
                'Me.Hide()
            Case Keys.Return, Keys.Down
                SetNextFocus()
            Case Keys.Up
                SetPreviousFocus()
        End Select
    End Sub

    Private Sub frmPay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If pnLoadx = 0 Then
            'p_oAppDrvr = New ggcAppDriver.GRider("RETMgtSys")
            Me.Location = New Point(565, 110)
            Call grpEventHandler(Me, GetType(Button), "cmdButton", "Click", AddressOf cmdButton_Click)

            Call grpEventHandler(Me, GetType(TextBox), "txtField", "GotFocus", AddressOf txtField_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtField", "LostFocus", AddressOf txtField_LostFocus)
            Call grpCancelHandler(Me, GetType(TextBox), "txtField", "Validating", AddressOf txtField_Validating)
            Call grpKeyHandler(Me, GetType(TextBox), "txtField", "KeyDown", AddressOf txtField_KeyDown)
            initGrid()
            enableControl(False)
            clearFields()

            txtField02.AutoCompleteCustomSource.Clear()
            For Each row As DataRow In poDiscount.GetCard.Rows
                txtField02.AutoCompleteCustomSource.Add(row.Item("sCardDesc").ToString())
            Next

            txtField02.AutoCompleteSource = AutoCompleteSource.CustomSource
            txtField02.AutoCompleteMode = AutoCompleteMode.Suggest
            pnLoadx = 1
        End If
    End Sub

    Private Sub frmPay_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        setVisible(True)
    End Sub

    Private Sub txtField_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtField" Then
            Select Case loIndex
                Case 5
                    loTxt.Text = poDiscount.Master(loIndex)
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
                    If loTxt.Text <> String.Empty Then poDiscount.SearchCard(loTxt.Text)
                    If poDiscount.isVatable = False Then
                        showDetail(True)
                    Else
                        showDetail(False)
                    End If
                    enableControl(poDiscount.Master(2) <> "")

                Case 3, 4, 6
                    If Not IsNumeric(loTxt.Text) Then loTxt.Text = 0

                    If loIndex = 3 Then
                        If CInt(loTxt.Text) < (txtField04.Text) Then
                            MsgBox("No of Clients must not be less than the SC/PWD for Discount.", MsgBoxStyle.Information, "Warning")
                            loTxt.Text = 0
                            txtField04.Text = 0
                            Exit Sub
                        End If
                    ElseIf loIndex = 4 Then
                        If CInt(loTxt.Text) > (txtField03.Text) Then
                            MsgBox("No of Clients must not be less than the SC/PWD for Discount.", MsgBoxStyle.Information, "Warning")
                            loTxt.Text = 0
                            txtField03.Text = 0
                            Exit Sub
                        End If
                    End If

                    poDiscount.Master(loIndex) = loTxt.Text
                Case 5
                    If Not IsNumeric(loTxt.Text) Then loTxt.Text = 0
                    poDiscount.Master(loIndex) = loTxt.Text
                Case 1
                    poDiscount.Detail(pnActiveRow, "sIDNumber") = loTxt.Text
                Case 10
                    poDiscount.Detail(pnActiveRow, "sClientNm") = loTxt.Text
            End Select
        End If

        loTxt.BackColor = SystemColors.Window
        poControl = Nothing
    End Sub

    Private Sub showDetail(ByVal lbShow As Boolean)
        Dim lvDetailLoc As New Point(3, 391)
        Dim lvButtonLoc As New Point(2, 467)
        Dim lvDetail As New Point(2, 119)
        Dim lvClients As New Point(2, 64)

        Dim lcDetailNew As New Point(2, 64)
        Dim lvFormOrgxx As New Size(390, 541)

        If Not lbShow Then
            Me.Size = lvFormOrgxx
            pnClients.Visible = True
            pnlMain.Size = lvFormOrgxx
            pnClients.Location = lvClients
            pnDetail.Location = lvDetail
            pnlButtons.Location = lvButtonLoc
        Else
            Me.Size = lvFormOrgxx
            txtField03.Text = 1
            txtField04.Text = 1
            pnClients.Visible = False
            pnlMain.Size = lvFormOrgxx
            pnDetail.Location = lcDetailNew
            pnlMain.Size = lvFormOrgxx
            pnlButtons.Location = lvButtonLoc
        End If

        initGrid()
    End Sub

    Private Sub InitializeDataGrid()
        With dgvClients
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
            .RowsDefaultCellStyle.BackColor = Color.WhiteSmoke
            .AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro

            ' Set the row and column header styles.
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .RowHeadersDefaultCellStyle.BackColor = Color.Black
        End With

        With dgvClients.ColumnHeadersDefaultCellStyle
            .BackColor = Color.Navy
            .ForeColor = Color.White
            .Font = New Font(dgvClients.Font, FontStyle.Bold)
        End With
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
                        Case 2 'card
                            If loTxt.Text <> String.Empty Then poDiscount.SearchCard(loTxt.Text)
                            If poDiscount.p_nTotalSales < IFNull(poDiscount.Master(10), 0) Then
                                MsgBox("Total Amount due is less than minimum amount!" & vbCrLf & "Unable to load this discount!", _
                                          MsgBoxStyle.Critical, _
                                          "Warning")
                                poDiscount.InitTransaction()
                                ClearDiscount()
                                Exit Sub
                            End If

                            If poDiscount.isVatable = False Then
                                showDetail(True)
                            Else
                                showDetail(False)
                            End If
                            enableControl(poDiscount.Master(2) <> "")

                        Case 10 'Name
                            If Not isDiscountOk() Then Exit Sub
                            poDiscount.Detail(pnActiveRow, "sClientNm") = loTxt.Text
                            If poDiscount.AddDiscount = True Then
                                loadDetail()
                            End If
                    End Select
                End If
        End Select
    End Sub

    Private Sub cmdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim loChk As Button
        loChk = CType(sender, System.Windows.Forms.Button)

        Dim lnIndex As Integer
        lnIndex = Val(Mid(loChk.Name, 10))

        Select Case lnIndex
            Case 0 'OK
                If Len(txtField01.Text) > 16 Then
                    MsgBox("CARD Number to long, must be not greater than 16 charactes including spaces!!!", MsgBoxStyle.Critical, "ALERT")
                    Exit Sub
                End If

                pbCancel = Not poDiscount.SaveTransaction()
                If pbCancel = False Then Me.Close()
            Case 1 'CANCEL
                If MsgBox("Are you sure to cancel adding of discount?", _
                              MsgBoxStyle.Question & MsgBoxStyle.YesNo, _
                              "Confirm") = MsgBoxResult.Yes Then
                    If Not IsNothing(poDiscount.Master("sDiscCard")) Then
                        If poDiscount.CancelDiscount Then
                            poDiscount.InitTransaction()

                            ClearDiscount()
                            'pbCancel = True
                            'Me.Hide()
                        End If
                    End If
                End If
                pbCancel = True
                Me.Hide()
            Case 2 'Add row
                If Not isDiscountOk() Then Exit Sub
                If poDiscount.AddDiscount = True Then
                    loadDetail()
                End If
            Case 3 'Delete row
                If poDiscount.ItemDetailCount > 1 Then
                    If dgvClients.RowCount - 1 > 0 Then
                        poDiscount.DeleteDiscount(pnActiveRow)
                        loadDetail()
                    Else
                        poDiscount.DeleteDiscount(pnActiveRow)
                        poDiscount.AddDiscount()
                        loadDetail()
                    End If
                Else
                    poDiscount.Detail(0, "sIDNumber") = ""
                    poDiscount.Detail(0, "sClientNm") = ""

                    txtField01.Text = ""
                    txtField03.Text = 0
                    txtField04.Text = 0
                    txtField09.Text = ""
                    txtField10.Text = ""
                    loadDetail()
                End If
        End Select
endProc:
        Exit Sub
    End Sub

    Private Sub setVisible(ByVal lbShow As Boolean)
        If lbShow Then
            pnlMain.Visible = True
            Me.Location = New Point(507, 90)
            Me.Opacity = 100
        Else
            Me.Opacity = 0
            Me.BackColor = Color.Orange
            Me.TransparencyKey = Me.BackColor
        End If
    End Sub

    Private Sub clearFields()
        With poDiscount
            txtField01.MaxLength = 16
            txtField02.MaxLength = 32
            txtField09.MaxLength = .MasFldSize(9)

            If .OpenTransaction() Then
                txtField01.Text = IFNull(.Master(1), "")
                txtField02.Text = IFNull(.Master("sDiscCard"), "")
                txtField03.Text = IFNull(.Master(3), "")
                txtField04.Text = IFNull(.Master(4), "")
                txtField09.Text = IFNull(.Master("sRemarksx"), "")
                txtField11.Text = IFNull(.Master(10), "")

                'Original Code
                'txtField05.Text = IFNull(.Master("nDiscRate"), 0) & "%"
                'txtField06.Text = IFNull(.Master("nAddDiscx"), 0)
                'txtField11.Text = IFNull(.Master("nMinAmtxx"), 0)

                txtField05.Text = IFNull(.Category(0, "nDiscRate"), 0) & "%"
                txtField06.Text = IFNull(.Category(0, "nDiscAmtx"), 0)
                txtField11.Text = IFNull(.Category(0, "nMinAmtxx"), 0)
                txtField10.Text = IFNull(.Detail(0, "sClientNm"), "")

            Else
                txtField01.Text = ""
                txtField02.Text = ""
                txtField03.Text = 1
                txtField04.Text = 1
                txtField09.Text = ""
                txtField11.Text = ""

                txtField05.Text = Format(0, xsDECIMAL) & "%"
                txtField06.Text = Format(0, xsDECIMAL)
                txtField11.Text = Format(0, xsDECIMAL)
                txtField10.Text = ""

            End If

            If poDiscount.isVatable = False Then
                showDetail(True)
            Else
                showDetail(False)
            End If

            loadDetail()
            enableControl(False)
        End With
    End Sub

    Private Sub ClearDiscount()
        txtField01.Text = ""
        txtField02.Text = ""
        txtField03.Text = 1
        txtField04.Text = 1
        txtField05.Text = ""
        txtField06.Text = ""
        txtField09.Text = ""
        txtField11.Text = ""
    End Sub

    Private Sub enableControl(ByVal lbEnable As Boolean)
        If lbEnable = False Then
            txtField01.Enabled = False
            txtField03.Enabled = False
            txtField04.Enabled = False
            txtField09.Enabled = False
            txtField10.Enabled = False
        Else
            txtField01.Enabled = True
            txtField03.Enabled = True
            txtField04.Enabled = True
            txtField10.Enabled = True
        End If

    End Sub

    Private Function isDiscountOk() As Boolean
        If Trim(txtField01.Text) = "" Then
            MsgBox("Please input card number to continue.", MsgBoxStyle.Exclamation, "Warning")
            Return False

        End If

        If txtField04.Text <= dgvClients.RowCount - 1 Then
            MsgBox("No. of clients with discount must not greater the entry.", MsgBoxStyle.Exclamation, "Warning")
            Return False
        End If
        Return True
    End Function

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

    Private Sub poDiscount_DetailRetreive(ByVal Row As Integer, ByVal Index As Integer, ByVal Value As String) Handles poDiscount.DetailRetreive
        Dim loTxt As TextBox

        loTxt = CType(FindTextBox(Me, "txtField" & Format(Index, "00")), TextBox)
        Select Case Index
            Case 2
                txtField01.Text = Value
            Case 3
                txtField10.Text = Value
        End Select
    End Sub

    Private Sub poDiscount_MasterRetrieved(ByVal Index As Object, ByVal Value As Object) Handles poDiscount.MasterRetrieved
        Dim loTxt As TextBox

        loTxt = CType(FindTextBox(Me, "txtField" & Format(Index, "00")), TextBox)
        Select Case Index
            Case 2
                loTxt.Text = Value
                enableControl(loTxt.Text <> "")
            Case 3, 4, 9
                loTxt.Text = Value
            Case 5
                loTxt.Text = (Value) & "%"
                Debug.Print(("discount rate") & poDiscount.Master("nDiscRate"))
            Case 6
                loTxt.Text = Value
            Case 10
                txtField11.Text = Value
        End Select

    End Sub

    Private Sub txtOccupants_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtField03.KeyPress, txtField04.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtDiscount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtField05.KeyPress, txtField06.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub dgvClients_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvClients.Click
        With dgvClients
            pnActiveRow = .CurrentCell.RowIndex
            setFieldValue(pnActiveRow)
        End With
    End Sub
End Class