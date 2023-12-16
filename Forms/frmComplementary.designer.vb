<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmComplementary
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
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtField05 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.cmdButton00 = New System.Windows.Forms.Button()
        Me.cmdButton01 = New System.Windows.Forms.Button()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.pnlAmount = New System.Windows.Forms.Panel()
        Me.txtField02 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtField03 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnlBill = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblBill = New System.Windows.Forms.Label()
        Me.pnlMain.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.pnlDetail.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAmount.SuspendLayout()
        Me.pnlBill.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.Transparent
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlMain.Controls.Add(Me.Panel1)
        Me.pnlMain.Controls.Add(Me.pnlButtons)
        Me.pnlMain.Controls.Add(Me.pnlDetail)
        Me.pnlMain.Controls.Add(Me.pnlAmount)
        Me.pnlMain.Location = New System.Drawing.Point(5, 107)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(381, 429)
        Me.pnlMain.TabIndex = 2
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.txtField05)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.RadioButton2)
        Me.Panel1.Controls.Add(Me.RadioButton1)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(369, 79)
        Me.Panel1.TabIndex = 15
        '
        'txtField05
        '
        Me.txtField05.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField05.ForeColor = System.Drawing.Color.Black
        Me.txtField05.Location = New System.Drawing.Point(226, 31)
        Me.txtField05.Margin = New System.Windows.Forms.Padding(2)
        Me.txtField05.Name = "txtField05"
        Me.txtField05.Size = New System.Drawing.Size(133, 31)
        Me.txtField05.TabIndex = 14
        Me.txtField05.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(222, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(117, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Value / Quantity"
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton2.ForeColor = System.Drawing.Color.White
        Me.RadioButton2.Location = New System.Drawing.Point(105, 42)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(94, 20)
        Me.RadioButton2.TabIndex = 15
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "per Detail"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton1.ForeColor = System.Drawing.Color.White
        Me.RadioButton1.Location = New System.Drawing.Point(4, 42)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(98, 20)
        Me.RadioButton1.TabIndex = 14
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "by Amount"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(3, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(134, 16)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Complement Type"
        '
        'pnlButtons
        '
        Me.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlButtons.Controls.Add(Me.cmdButton00)
        Me.pnlButtons.Controls.Add(Me.cmdButton01)
        Me.pnlButtons.Location = New System.Drawing.Point(3, 381)
        Me.pnlButtons.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(369, 40)
        Me.pnlButtons.TabIndex = 9
        '
        'cmdButton00
        '
        Me.cmdButton00.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmdButton00.FlatAppearance.BorderSize = 0
        Me.cmdButton00.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton00.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton00.ForeColor = System.Drawing.Color.White
        Me.cmdButton00.Location = New System.Drawing.Point(229, 3)
        Me.cmdButton00.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton00.Name = "cmdButton00"
        Me.cmdButton00.Size = New System.Drawing.Size(64, 30)
        Me.cmdButton00.TabIndex = 7
        Me.cmdButton00.Text = "OK"
        Me.cmdButton00.UseVisualStyleBackColor = False
        '
        'cmdButton01
        '
        Me.cmdButton01.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmdButton01.FlatAppearance.BorderSize = 0
        Me.cmdButton01.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton01.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton01.ForeColor = System.Drawing.Color.White
        Me.cmdButton01.Location = New System.Drawing.Point(296, 3)
        Me.cmdButton01.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton01.Name = "cmdButton01"
        Me.cmdButton01.Size = New System.Drawing.Size(64, 30)
        Me.cmdButton01.TabIndex = 8
        Me.cmdButton01.Text = "CLOSE"
        Me.cmdButton01.UseVisualStyleBackColor = False
        '
        'pnlDetail
        '
        Me.pnlDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlDetail.Controls.Add(Me.DataGridView1)
        Me.pnlDetail.Location = New System.Drawing.Point(3, 158)
        Me.pnlDetail.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Size = New System.Drawing.Size(369, 218)
        Me.pnlDetail.TabIndex = 8
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(365, 214)
        Me.DataGridView1.TabIndex = 22
        Me.DataGridView1.TabStop = False
        '
        'pnlAmount
        '
        Me.pnlAmount.BackColor = System.Drawing.Color.Transparent
        Me.pnlAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAmount.Controls.Add(Me.txtField02)
        Me.pnlAmount.Controls.Add(Me.Label1)
        Me.pnlAmount.Controls.Add(Me.txtField03)
        Me.pnlAmount.Controls.Add(Me.Label8)
        Me.pnlAmount.Location = New System.Drawing.Point(3, 85)
        Me.pnlAmount.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlAmount.Name = "pnlAmount"
        Me.pnlAmount.Size = New System.Drawing.Size(369, 70)
        Me.pnlAmount.TabIndex = 5
        '
        'txtField02
        '
        Me.txtField02.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField02.Location = New System.Drawing.Point(92, 35)
        Me.txtField02.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField02.MaxLength = 64
        Me.txtField02.Name = "txtField02"
        Me.txtField02.Size = New System.Drawing.Size(266, 24)
        Me.txtField02.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(4, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 15)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Remarks"
        '
        'txtField03
        '
        Me.txtField03.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField03.Location = New System.Drawing.Point(92, 7)
        Me.txtField03.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField03.MaxLength = 32
        Me.txtField03.Name = "txtField03"
        Me.txtField03.Size = New System.Drawing.Size(179, 24)
        Me.txtField03.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(4, 13)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 15)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Requested"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(2, 2)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(116, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Complementary"
        '
        'pnlBill
        '
        Me.pnlBill.BackColor = System.Drawing.Color.Transparent
        Me.pnlBill.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlBill.Controls.Add(Me.Label2)
        Me.pnlBill.Controls.Add(Me.lblBill)
        Me.pnlBill.Location = New System.Drawing.Point(5, 23)
        Me.pnlBill.Name = "pnlBill"
        Me.pnlBill.Size = New System.Drawing.Size(381, 81)
        Me.pnlBill.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Gainsboro
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.ForestGreen
        Me.Label2.Location = New System.Drawing.Point(3, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "TOTAL BILL"
        '
        'lblBill
        '
        Me.lblBill.BackColor = System.Drawing.Color.Gainsboro
        Me.lblBill.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBill.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblBill.Location = New System.Drawing.Point(3, 4)
        Me.lblBill.Name = "lblBill"
        Me.lblBill.Size = New System.Drawing.Size(369, 70)
        Me.lblBill.TabIndex = 1
        Me.lblBill.Text = "00,000.00"
        Me.lblBill.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmComplementary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = Global.ggcRetailSales.My.Resources.Resources.mainbackground
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(390, 541)
        Me.Controls.Add(Me.pnlBill)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.pnlMain)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmComplementary"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TransparencyKey = System.Drawing.SystemColors.Window
        Me.pnlMain.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlButtons.ResumeLayout(False)
        Me.pnlDetail.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAmount.ResumeLayout(False)
        Me.pnlAmount.PerformLayout()
        Me.pnlBill.ResumeLayout(False)
        Me.pnlBill.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents pnlAmount As System.Windows.Forms.Panel
    Friend WithEvents txtField05 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents pnlBill As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblBill As System.Windows.Forms.Label
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents cmdButton00 As System.Windows.Forms.Button
    Friend WithEvents cmdButton01 As System.Windows.Forms.Button
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents txtField02 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtField03 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
End Class
