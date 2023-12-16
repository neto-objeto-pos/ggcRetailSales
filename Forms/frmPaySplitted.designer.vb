<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPaySplitted
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.pnlBill = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblBill = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.pnlButton = New System.Windows.Forms.Panel()
        Me.cmdButton01 = New System.Windows.Forms.Button()
        Me.cmdButton00 = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblMaster04 = New System.Windows.Forms.Label()
        Me.lblMaster17 = New System.Windows.Forms.Label()
        Me.lblMaster13 = New System.Windows.Forms.Label()
        Me.lblMaster14 = New System.Windows.Forms.Label()
        Me.lblMaster15 = New System.Windows.Forms.Label()
        Me.lblAmount = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.txtDetail00 = New System.Windows.Forms.TextBox()
        Me.pnlMain.SuspendLayout()
        Me.pnlBill.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlDetail.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlButton.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.Transparent
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlMain.Controls.Add(Me.pnlBill)
        Me.pnlMain.Controls.Add(Me.Panel1)
        Me.pnlMain.Controls.Add(Me.pnlDetail)
        Me.pnlMain.Location = New System.Drawing.Point(502, 3)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(410, 656)
        Me.pnlMain.TabIndex = 2
        '
        'pnlBill
        '
        Me.pnlBill.BackColor = System.Drawing.Color.Transparent
        Me.pnlBill.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlBill.Controls.Add(Me.Label2)
        Me.pnlBill.Controls.Add(Me.lblBill)
        Me.pnlBill.Location = New System.Drawing.Point(3, 3)
        Me.pnlBill.Name = "pnlBill"
        Me.pnlBill.Size = New System.Drawing.Size(400, 91)
        Me.pnlBill.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Gainsboro
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label2.Location = New System.Drawing.Point(8, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 20)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "AMOUNT DUE"
        '
        'lblBill
        '
        Me.lblBill.BackColor = System.Drawing.Color.Gainsboro
        Me.lblBill.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblBill.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBill.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblBill.Location = New System.Drawing.Point(2, 4)
        Me.lblBill.Name = "lblBill"
        Me.lblBill.Size = New System.Drawing.Size(391, 81)
        Me.lblBill.TabIndex = 2
        Me.lblBill.Text = "00,000.00"
        Me.lblBill.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.RadioButton3)
        Me.Panel1.Controls.Add(Me.RadioButton2)
        Me.Panel1.Controls.Add(Me.RadioButton1)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Location = New System.Drawing.Point(3, 97)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(400, 78)
        Me.Panel1.TabIndex = 15
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Enabled = False
        Me.RadioButton3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton3.ForeColor = System.Drawing.Color.White
        Me.RadioButton3.Location = New System.Drawing.Point(286, 41)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(105, 24)
        Me.RadioButton3.TabIndex = 6
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "per Detail"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Enabled = False
        Me.RadioButton2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton2.ForeColor = System.Drawing.Color.White
        Me.RadioButton2.Location = New System.Drawing.Point(131, 41)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(142, 24)
        Me.RadioButton2.TabIndex = 5
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "by Percentage"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Enabled = False
        Me.RadioButton1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton1.ForeColor = System.Drawing.Color.White
        Me.RadioButton1.Location = New System.Drawing.Point(4, 41)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(112, 24)
        Me.RadioButton1.TabIndex = 4
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "by Amount"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(3, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 20)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Split Type"
        '
        'pnlDetail
        '
        Me.pnlDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlDetail.Controls.Add(Me.DataGridView1)
        Me.pnlDetail.Location = New System.Drawing.Point(3, 178)
        Me.pnlDetail.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Size = New System.Drawing.Size(400, 473)
        Me.pnlDetail.TabIndex = 8
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(2)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(396, 469)
        Me.DataGridView1.TabIndex = 21
        '
        'pnlButton
        '
        Me.pnlButton.BackColor = System.Drawing.Color.Transparent
        Me.pnlButton.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlButton.Controls.Add(Me.cmdButton01)
        Me.pnlButton.Controls.Add(Me.cmdButton00)
        Me.pnlButton.Location = New System.Drawing.Point(913, 4)
        Me.pnlButton.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlButton.Name = "pnlButton"
        Me.pnlButton.Size = New System.Drawing.Size(106, 656)
        Me.pnlButton.TabIndex = 43
        '
        'cmdButton01
        '
        Me.cmdButton01.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdButton01.FlatAppearance.BorderSize = 0
        Me.cmdButton01.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton01.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton01.ForeColor = System.Drawing.Color.White
        Me.cmdButton01.Location = New System.Drawing.Point(1, 48)
        Me.cmdButton01.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton01.Name = "cmdButton01"
        Me.cmdButton01.Size = New System.Drawing.Size(100, 40)
        Me.cmdButton01.TabIndex = 35
        Me.cmdButton01.Text = "CANCEL"
        Me.cmdButton01.UseVisualStyleBackColor = False
        '
        'cmdButton00
        '
        Me.cmdButton00.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdButton00.FlatAppearance.BorderSize = 0
        Me.cmdButton00.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton00.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton00.ForeColor = System.Drawing.Color.White
        Me.cmdButton00.Location = New System.Drawing.Point(1, 3)
        Me.cmdButton00.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton00.Name = "cmdButton00"
        Me.cmdButton00.Size = New System.Drawing.Size(100, 40)
        Me.cmdButton00.TabIndex = 33
        Me.cmdButton00.Text = "OKAY"
        Me.cmdButton00.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Controls.Add(Me.txtDetail00)
        Me.Panel2.Location = New System.Drawing.Point(2, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(498, 656)
        Me.Panel2.TabIndex = 44
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Label16)
        Me.Panel3.Controls.Add(Me.Label15)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.Label13)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Controls.Add(Me.lblMaster04)
        Me.Panel3.Controls.Add(Me.lblMaster17)
        Me.Panel3.Controls.Add(Me.lblMaster13)
        Me.Panel3.Controls.Add(Me.lblMaster14)
        Me.Panel3.Controls.Add(Me.lblMaster15)
        Me.Panel3.Controls.Add(Me.lblAmount)
        Me.Panel3.Location = New System.Drawing.Point(3, 499)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(488, 150)
        Me.Panel3.TabIndex = 303
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Courier New", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label16.Location = New System.Drawing.Point(29, 48)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(152, 27)
        Me.Label16.TabIndex = 312
        Me.Label16.Text = "Amount Due"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label15.Location = New System.Drawing.Point(13, 122)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(168, 18)
        Me.Label15.TabIndex = 311
        Me.Label15.Text = "Senior/PWD Sales"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label14.Location = New System.Drawing.Point(73, 101)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(108, 18)
        Me.Label14.TabIndex = 310
        Me.Label14.Text = "VAT Amount"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label13.Location = New System.Drawing.Point(43, 80)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(138, 18)
        Me.Label13.TabIndex = 309
        Me.Label13.Text = "VATable Sales"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(3, 27)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(158, 18)
        Me.Label7.TabIndex = 300
        Me.Label7.Text = "less: Discounts"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label6.Location = New System.Drawing.Point(53, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(128, 18)
        Me.Label6.TabIndex = 297
        Me.Label6.Text = "Sales Amount"
        '
        'lblMaster04
        '
        Me.lblMaster04.BackColor = System.Drawing.Color.Gainsboro
        Me.lblMaster04.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaster04.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblMaster04.Location = New System.Drawing.Point(178, 3)
        Me.lblMaster04.Name = "lblMaster04"
        Me.lblMaster04.Size = New System.Drawing.Size(303, 25)
        Me.lblMaster04.TabIndex = 6
        Me.lblMaster04.Text = "0.00"
        Me.lblMaster04.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMaster17
        '
        Me.lblMaster17.BackColor = System.Drawing.Color.Gainsboro
        Me.lblMaster17.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaster17.ForeColor = System.Drawing.Color.Red
        Me.lblMaster17.Location = New System.Drawing.Point(181, 24)
        Me.lblMaster17.Name = "lblMaster17"
        Me.lblMaster17.Size = New System.Drawing.Size(300, 25)
        Me.lblMaster17.TabIndex = 298
        Me.lblMaster17.Text = "0.00"
        Me.lblMaster17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMaster13
        '
        Me.lblMaster13.BackColor = System.Drawing.Color.Gainsboro
        Me.lblMaster13.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaster13.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblMaster13.Location = New System.Drawing.Point(181, 77)
        Me.lblMaster13.Name = "lblMaster13"
        Me.lblMaster13.Size = New System.Drawing.Size(300, 25)
        Me.lblMaster13.TabIndex = 301
        Me.lblMaster13.Text = "0.00"
        Me.lblMaster13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMaster14
        '
        Me.lblMaster14.BackColor = System.Drawing.Color.Gainsboro
        Me.lblMaster14.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaster14.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblMaster14.Location = New System.Drawing.Point(184, 98)
        Me.lblMaster14.Name = "lblMaster14"
        Me.lblMaster14.Size = New System.Drawing.Size(297, 25)
        Me.lblMaster14.TabIndex = 304
        Me.lblMaster14.Text = "0.00"
        Me.lblMaster14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMaster15
        '
        Me.lblMaster15.BackColor = System.Drawing.Color.Gainsboro
        Me.lblMaster15.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaster15.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblMaster15.Location = New System.Drawing.Point(184, 119)
        Me.lblMaster15.Name = "lblMaster15"
        Me.lblMaster15.Size = New System.Drawing.Size(297, 25)
        Me.lblMaster15.TabIndex = 305
        Me.lblMaster15.Text = "0.00"
        Me.lblMaster15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblAmount
        '
        Me.lblAmount.BackColor = System.Drawing.Color.Gainsboro
        Me.lblAmount.Font = New System.Drawing.Font("Courier New", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblAmount.Location = New System.Drawing.Point(181, 49)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(305, 25)
        Me.lblAmount.TabIndex = 307
        Me.lblAmount.Text = "0.00"
        Me.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Transparent
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel4.Controls.Add(Me.DataGridView2)
        Me.Panel4.Location = New System.Drawing.Point(5, 56)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(487, 439)
        Me.Panel4.TabIndex = 43
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView2.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView2.MultiSelect = False
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(483, 435)
        Me.DataGridView2.TabIndex = 38
        Me.DataGridView2.TabStop = False
        '
        'txtDetail00
        '
        Me.txtDetail00.Enabled = False
        Me.txtDetail00.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetail00.Location = New System.Drawing.Point(5, 6)
        Me.txtDetail00.Name = "txtDetail00"
        Me.txtDetail00.ReadOnly = True
        Me.txtDetail00.Size = New System.Drawing.Size(487, 47)
        Me.txtDetail00.TabIndex = 40
        Me.txtDetail00.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmPaySplitted
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = Global.ggcRetailSales.My.Resources.Resources.mainbackground
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(1024, 663)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.pnlButton)
        Me.Controls.Add(Me.pnlMain)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPaySplitted"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TransparencyKey = System.Drawing.SystemColors.Window
        Me.pnlMain.ResumeLayout(False)
        Me.pnlBill.ResumeLayout(False)
        Me.pnlBill.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlDetail.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlButton.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents pnlBill As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblBill As System.Windows.Forms.Label
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents pnlButton As System.Windows.Forms.Panel
    Friend WithEvents cmdButton01 As System.Windows.Forms.Button
    Friend WithEvents cmdButton00 As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents txtDetail00 As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblMaster04 As System.Windows.Forms.Label
    Friend WithEvents lblMaster17 As System.Windows.Forms.Label
    Friend WithEvents lblMaster13 As System.Windows.Forms.Label
    Friend WithEvents lblMaster14 As System.Windows.Forms.Label
    Friend WithEvents lblMaster15 As System.Windows.Forms.Label
    Friend WithEvents lblAmount As System.Windows.Forms.Label
End Class
