Imports System.Threading
Imports System.Drawing
Imports System.Windows.Forms
Imports ggcAppDriver

Public Class frmTables
    Private WithEvents poTable As TableMaster
    Private Const pxeMaxTable As Integer = 25
    Private pnTablePage As Integer = 0

    Private p_oAppDriver As GRider
    Private p_sTerminal As String

    Private pnLoadx As Integer
    Private poControl As Control

    Private p_bCancelled As Boolean
    Private pnTotalTble As Integer

    Public Sub New(oDriver As GRider)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        p_oAppDriver = oDriver
    End Sub

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

    Property Terminal() As String
        Get
            Return p_sTerminal
        End Get
        Set(ByVal value As String)
            p_sTerminal = value
        End Set
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

            Call grpEventHandler(Me, GetType(TextBox), "txtDetail", "GotFocus", AddressOf txtDetail_GotFocus)
            Call grpEventHandler(Me, GetType(TextBox), "txtDetail", "LostFocus", AddressOf txtDetail_LostFocus)
            Call grpCancelHandler(Me, GetType(TextBox), "txtDetail", "Validating", AddressOf txtDetail_Validating)
            Call grpKeyHandler(Me, GetType(TextBox), "txtDetail", "KeyDown", AddressOf txtDetail_KeyDown)
            Call grpEventHandler(Me, GetType(Label), "lblTable", "Click", AddressOf lblTable_Click)
            Call grpEventHandler(Me, GetType(Label), "lblTable", "DoubleClick", AddressOf lblTable_DoubleClick)

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
                Dim lsTransNo As String = ""

                If Not isEntryOk() Then Exit Sub

                If checkTableStatus(txtDetail00.Text, lsTransNo) Then
                    If lsTransNo = "" Then
                        If poTable.reservedTable(txtDetail00.Text) Then
                            poTable.TableNo = txtDetail00.Text
                            p_bCancelled = False
                            Me.Hide()
                            Me.Dispose()
                        End If
                    Else
                        poTable.TableNo = txtDetail00.Text
                        p_bCancelled = False
                        Me.Hide()
                        Me.Dispose()
                    End If
                End If
            Case 3
                p_bCancelled = True
                Me.Close()
                Me.Dispose()
            Case 4 To 8
                Call loadTable(lnIndex - 3)
        End Select
endProc:
        Exit Sub
    End Sub

    Private Sub setVisible()
        Me.Visible = False
        Me.TransparencyKey = Nothing
        Me.Location = New Point(507, 90)

        txtDetail00.MaxLength = 3

        Me.Visible = True
    End Sub

    Private Sub clearFields()
        txtDetail00.Text = ""
        txtDetail00.Focus()
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

    Private Sub txtDetail_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtDetail" Then
            Select Case loIndex
                Case 0

            End Select
        End If

        poControl = loTxt

        loTxt.BackColor = Color.Azure
        loTxt.SelectAll()
    End Sub

    Private Sub txtDetail_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))

        If Mid(loTxt.Name, 1, 8) = "txtDetail" Then
            Select Case loIndex
                Case 0
                    If Not IsNumeric(loTxt.Text) Then
                        loTxt.Text = 0
                    End If
            End Select
        End If

        loTxt.BackColor = SystemColors.Window
        poControl = Nothing
    End Sub

    Private Sub txtDetail_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))
    End Sub

    Private Sub txtDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim loTxt As TextBox
        loTxt = CType(sender, System.Windows.Forms.TextBox)

        Dim loIndex As Integer
        loIndex = Val(Mid(loTxt.Name, 9))
    End Sub

    Private Sub poTable_MasterRetrieve(lnIndex As Integer) Handles poTable.MasterRetrieve
        Select Case lnIndex
            Case 1

            Case 2

            Case 4

        End Select
    End Sub

    Private Sub loadTable(ByVal fnTablePage As Integer)
        Dim loDT As DataTable
        Dim lnCtr As Integer
        Dim loLbl As Label
        Dim lnStart As Integer
        Dim lnRow As Integer

        loDT = poTable.GetTables()
        pnTotalTble = loDT.Rows.Count
        pnTablePage = fnTablePage

        Select Case pnTablePage
            Case 1
                lnStart = 1
                lnRow = 0
            Case 2
                lnStart = 26
                lnRow = 25
            Case 3
                lnStart = 51
                lnRow = 50
            Case 4
                lnStart = 76
                lnRow = 75
            Case 5
                lnStart = 101
                lnRow = 100
        End Select

        If pnTotalTble > lnStart Then
            Call initTable()
            For lnCtr = (lnStart - 1) To (pnTablePage * 25) - 1
                loLbl = CType(FindLabel(Me, "lblTable" & Format(lnCtr - lnRow, "00")), Label)

                loLbl.Text = loDT(lnCtr)("nTableNox")
                markTable(lnCtr, CInt(loDT(lnCtr)("cStatusxx")))
            Next
        End If
    End Sub

    Private Sub markTable(ByVal fsTables As String, ByVal fsStatus As xeTableStatus)
        Dim lvBackColor As Color
        Dim lvForeColor As Color

        Select Case fsStatus
            Case xeTableStatus.xeEmpty
                lvBackColor = Color.White
                lvForeColor = Color.Green
            Case xeTableStatus.xeOccupied

                If getTerminal(fsTables + 1) = p_sTerminal Then
                    lvBackColor = Color.Green
                    lvForeColor = Color.White
                Else
                    lvBackColor = Color.Indigo
                    lvForeColor = Color.White
                End If
            Case xeTableStatus.xeReserved
                lvBackColor = Color.Yellow
                lvForeColor = Color.White
            Case xeTableStatus.xeDirty
                lvBackColor = Color.Red
                lvForeColor = Color.White
            Case xeTableStatus.xeNONE
                lvBackColor = Color.Black
                lvForeColor = Color.Black
        End Select

        Dim lasDetail() As String = Split(fsTables, ",")
        Dim lnCtr As Integer
        Dim loLbl As Label
        For lnCtr = 0 To UBound(lasDetail)
            loLbl = CType(FindLabel(Me, "lblTable" & Format(CInt(lasDetail(lnCtr)), "00")), Label)

            loLbl.BackColor = lvBackColor
            loLbl.ForeColor = lvForeColor
        Next
    End Sub

    Private Sub initTable()
        Dim lnCtr As Integer
        Dim loLbl As Label

        For lnCtr = 0 To pxeMaxTable - 1
            loLbl = CType(FindLabel(Me, "lblTable" & Format(lnCtr, "00")), Label)
            loLbl.Text = ""
            markTable(lnCtr, xeTableStatus.xeNONE)
        Next
    End Sub

    Private Sub frmTables_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Call loadTable(1)
    End Sub

    Private Function getTerminal(ByVal fnTableNo As Integer) As String
        Dim lsSQL As String
        Dim lsTerminal As String
        Dim loDTa As DataTable

        lsSQL = "SELECT sTransNox FROM SO_Master WHERE cTranStat = '0' AND sTableNox = " & strParm(fnTableNo)
        loDTa = p_oAppDriver.ExecuteQuery(lsSQL)

        If loDTa.Rows.Count = 1 Then
            lsTerminal = loDTa(0).Item("sTransNox")
            Select Case lsTerminal.Substring(0, 6)
                Case "P00101"
                    lsTerminal = "01"
                Case "P00102"
                    lsTerminal = "02"
                Case "P00103"
                    lsTerminal = "03"
                Case "P00104"
                    lsTerminal = "04"
                Case Else
                    lsTerminal = "05"
            End Select
        Else
            lsTerminal = ""
        End If

        Return lsTerminal
    End Function

    Public Function isEntryOk()
        If txtDetail00.Text = "" Then
            MsgBox("Please input table No!", vbInformation + vbCritical, "Warning")
            txtDetail00.Focus()
            Return False
        ElseIf CInt(txtDetail00.Text) > 100 Then
            MsgBox("Please input valid table no!", vbInformation + vbCritical, "Warning")
            txtDetail00.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub lblTable_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loLbl As Label
        Dim lnRep As Integer
        Dim lnTableNo As Integer
        loLbl = CType(sender, System.Windows.Forms.Label)

        Dim lnIndex As Integer
        Dim lsTransNo As String = ""
        Dim lsSQL As String
        Dim loDT As DataTable
        poTable.TableNo = ""

        lnIndex = Val(Mid(loLbl.Name, 9))
        lnTableNo = loLbl.Text
        txtDetail00.Text = lnTableNo

        lsSQL = "SELECT sTransNox FROM SO_Master" &
                        " WHERE sTableNox = " & lnTableNo &
                            " AND cTranStat = 0"

        loDT = p_oAppDriver.ExecuteQuery(lsSQL)

        poTable.TableNo = ""
        If loDT.Rows.Count = 0 Then
            lnRep = MsgBox("Would you like to create new transaction?", vbQuestion & vbYesNo, "CONFIRMATION")
            If lnRep = vbNo Then Exit Sub

            If poTable.reservedTable(lnTableNo) Then
                Select Case CInt(lnTableNo)
                    Case 1 To 25
                        loadTable(1)
                    Case 26 To 50
                        loadTable(2)
                    Case 51 To 75
                        loadTable(3)
                    Case 76 To 100
                        loadTable(4)
                    Case 101 To 125
                        loadTable(5)
                End Select
            End If
        End If
    End Sub

    Private Sub lblTable_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim loLbl As Label
        Dim lnTableNo As Integer
        loLbl = CType(sender, System.Windows.Forms.Label)

        Dim lnIndex As Integer
        Dim lsTransNo As String = ""

        lnIndex = Val(Mid(loLbl.Name, 9))
        lnTableNo = loLbl.Text
        Dim lbSuccess As Boolean
        Dim lbSCharge As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        lsSQL = "SELECT sTransNox, cSChargex FROM SO_Master" &
                        " WHERE sTableNox = " & lnTableNo &
                            " AND cTranStat = 0"
        loDT = p_oAppDriver.ExecuteQuery(lsSQL)

        poTable.TableNo = ""
        If loDT.Rows.Count > 0 Then
            With poTable
                If Not .OpenTable(lnTableNo) Then
                    MsgBox("No table with the given number found.", MsgBoxStyle.Information, "Notice")
                    Exit Sub
                End If

                loDT = p_oAppDriver.ExecuteQuery(lsSQL)

                .WithSCharge = IIf(loDT.Rows(0)("cSChargex") = "x", False, loDT.Rows(0)("cSChargex"))
                lbSuccess = .showManageTable
                lbSCharge = IIf(.WithSCharge, True, False)

                If lbSuccess Then
                    lsSQL = "UPDATE SO_Master SET" &
                                            " cSChargex = " & strParm(IIf(lbSCharge, 1, 0)) &
                                        " WHERE sTransNox = " & strParm(loDT.Rows(0)("sTransNox"))
                    If lsSQL <> "" Then p_oAppDriver.Execute(lsSQL, "SO_Master")

                    lsSQL = "UPDATE Table_Master SET" &
                                            "  nOccupnts = " & CInt(.Master("nOccupnts")) &
                                            ", nCapacity = " & CInt(.Master("nCapacity")) &
                                        " WHERE nTableNox = " & strParm(lnTableNo)
                    If lsSQL <> "" Then p_oAppDriver.Execute(lsSQL, "Table_Master")

                    poTable.TableNo = lnTableNo
                End If
            End With
        End If
    End Sub

    Private Function checkTableStatus(ByVal fsTableNo As String,
                                      ByRef fsTransNo As String) As Boolean
        Dim lsSQL As String
        Dim loDT As DataTable

        fsTransNo = ""
        lsSQL = "SELECT sTransNox FROM SO_Master" &
                        " WHERE sTableNox = " & fsTableNo &
                            " AND cTranStat = 0"

        loDT = p_oAppDriver.ExecuteQuery(lsSQL)
        If loDT.Rows.Count > 0 Then
            If MsgBox("Open Order Exists for this table. Do you want to continue?",
                            MsgBoxStyle.YesNo, "CONFIRMATION") = MsgBoxResult.No Then
                Return False
            End If
            fsTransNo = loDT.Rows(0).Item("sTransNox")
        End If

        Return True
    End Function
End Class