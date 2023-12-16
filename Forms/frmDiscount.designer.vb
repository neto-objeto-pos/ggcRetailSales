<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiscount
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
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.cmdButton02 = New System.Windows.Forms.Button()
        Me.cmdButton03 = New System.Windows.Forms.Button()
        Me.cmdButton00 = New System.Windows.Forms.Button()
        Me.cmdButton01 = New System.Windows.Forms.Button()
        Me.pnlAmount = New System.Windows.Forms.Panel()
        Me.pnDetail = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtField10 = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dgvClients = New System.Windows.Forms.DataGridView()
        Me.txtField09 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtField05 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtField06 = New System.Windows.Forms.TextBox()
        Me.pnClients = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtField03 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtField04 = New System.Windows.Forms.TextBox()
        Me.txtField01 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtField02 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtField11 = New System.Windows.Forms.TextBox()
        Me.pnlMain.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.pnlAmount.SuspendLayout()
        Me.pnDetail.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvClients, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnClients.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.Transparent
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlMain.Controls.Add(Me.pnlButtons)
        Me.pnlMain.Controls.Add(Me.pnlAmount)
        Me.pnlMain.Location = New System.Drawing.Point(5, 20)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(379, 515)
        Me.pnlMain.TabIndex = 2
        '
        'pnlButtons
        '
        Me.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlButtons.Controls.Add(Me.cmdButton02)
        Me.pnlButtons.Controls.Add(Me.cmdButton03)
        Me.pnlButtons.Controls.Add(Me.cmdButton00)
        Me.pnlButtons.Controls.Add(Me.cmdButton01)
        Me.pnlButtons.Location = New System.Drawing.Point(2, 467)
        Me.pnlButtons.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(369, 40)
        Me.pnlButtons.TabIndex = 7
        '
        'cmdButton02
        '
        Me.cmdButton02.BackColor = System.Drawing.Color.Red
        Me.cmdButton02.FlatAppearance.BorderSize = 0
        Me.cmdButton02.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton02.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton02.ForeColor = System.Drawing.Color.White
        Me.cmdButton02.Location = New System.Drawing.Point(5, 3)
        Me.cmdButton02.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton02.Name = "cmdButton02"
        Me.cmdButton02.Size = New System.Drawing.Size(64, 30)
        Me.cmdButton02.TabIndex = 15
        Me.cmdButton02.Text = "ADD"
        Me.cmdButton02.UseVisualStyleBackColor = False
        '
        'cmdButton03
        '
        Me.cmdButton03.BackColor = System.Drawing.Color.Red
        Me.cmdButton03.FlatAppearance.BorderSize = 0
        Me.cmdButton03.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton03.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton03.ForeColor = System.Drawing.Color.White
        Me.cmdButton03.Location = New System.Drawing.Point(72, 3)
        Me.cmdButton03.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton03.Name = "cmdButton03"
        Me.cmdButton03.Size = New System.Drawing.Size(90, 30)
        Me.cmdButton03.TabIndex = 16
        Me.cmdButton03.Text = "DELETE ROW"
        Me.cmdButton03.UseVisualStyleBackColor = False
        '
        'cmdButton00
        '
        Me.cmdButton00.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmdButton00.FlatAppearance.BorderSize = 0
        Me.cmdButton00.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton00.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton00.ForeColor = System.Drawing.Color.White
        Me.cmdButton00.Location = New System.Drawing.Point(231, 3)
        Me.cmdButton00.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton00.Name = "cmdButton00"
        Me.cmdButton00.Size = New System.Drawing.Size(64, 30)
        Me.cmdButton00.TabIndex = 13
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
        Me.cmdButton01.Location = New System.Drawing.Point(297, 3)
        Me.cmdButton01.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton01.Name = "cmdButton01"
        Me.cmdButton01.Size = New System.Drawing.Size(64, 30)
        Me.cmdButton01.TabIndex = 14
        Me.cmdButton01.Text = "CANCEL"
        Me.cmdButton01.UseVisualStyleBackColor = False
        '
        'pnlAmount
        '
        Me.pnlAmount.BackColor = System.Drawing.Color.Transparent
        Me.pnlAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAmount.Controls.Add(Me.pnDetail)
        Me.pnlAmount.Controls.Add(Me.pnClients)
        Me.pnlAmount.Controls.Add(Me.txtField01)
        Me.pnlAmount.Controls.Add(Me.Label3)
        Me.pnlAmount.Controls.Add(Me.Label4)
        Me.pnlAmount.Controls.Add(Me.txtField02)
        Me.pnlAmount.Location = New System.Drawing.Point(3, 2)
        Me.pnlAmount.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlAmount.Name = "pnlAmount"
        Me.pnlAmount.Size = New System.Drawing.Size(369, 457)
        Me.pnlAmount.TabIndex = 5
        '
        'pnDetail
        '
        Me.pnDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnDetail.Controls.Add(Me.Label10)
        Me.pnDetail.Controls.Add(Me.txtField11)
        Me.pnDetail.Controls.Add(Me.Label9)
        Me.pnDetail.Controls.Add(Me.txtField10)
        Me.pnDetail.Controls.Add(Me.Panel1)
        Me.pnDetail.Controls.Add(Me.txtField09)
        Me.pnDetail.Controls.Add(Me.Label5)
        Me.pnDetail.Controls.Add(Me.txtField05)
        Me.pnDetail.Controls.Add(Me.Label7)
        Me.pnDetail.Controls.Add(Me.Label8)
        Me.pnDetail.Controls.Add(Me.txtField06)
        Me.pnDetail.Location = New System.Drawing.Point(4, 119)
        Me.pnDetail.Name = "pnDetail"
        Me.pnDetail.Size = New System.Drawing.Size(358, 331)
        Me.pnDetail.TabIndex = 18
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(2, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(96, 16)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Client Name:"
        '
        'txtField10
        '
        Me.txtField10.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField10.Location = New System.Drawing.Point(97, 4)
        Me.txtField10.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField10.MaxLength = 120
        Me.txtField10.Name = "txtField10"
        Me.txtField10.Size = New System.Drawing.Size(255, 24)
        Me.txtField10.TabIndex = 9
        Me.txtField10.Text = "Cuison, Michael"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.dgvClients)
        Me.Panel1.Location = New System.Drawing.Point(1, 143)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(354, 183)
        Me.Panel1.TabIndex = 17
        '
        'dgvClients
        '
        Me.dgvClients.AllowUserToAddRows = False
        Me.dgvClients.AllowUserToDeleteRows = False
        Me.dgvClients.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvClients.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvClients.Location = New System.Drawing.Point(0, 0)
        Me.dgvClients.Name = "dgvClients"
        Me.dgvClients.ReadOnly = True
        Me.dgvClients.Size = New System.Drawing.Size(350, 179)
        Me.dgvClients.TabIndex = 15
        '
        'txtField09
        '
        Me.txtField09.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField09.Location = New System.Drawing.Point(67, 113)
        Me.txtField09.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField09.MaxLength = 64
        Me.txtField09.Name = "txtField09"
        Me.txtField09.Size = New System.Drawing.Size(285, 24)
        Me.txtField09.TabIndex = 14
        Me.txtField09.Text = "1234567890123456"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(156, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(105, 16)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Discount Rate"
        '
        'txtField05
        '
        Me.txtField05.Enabled = False
        Me.txtField05.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField05.Location = New System.Drawing.Point(260, 33)
        Me.txtField05.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField05.MaxLength = 6
        Me.txtField05.Name = "txtField05"
        Me.txtField05.Size = New System.Drawing.Size(92, 24)
        Me.txtField05.TabIndex = 10
        Me.txtField05.Text = "10"
        Me.txtField05.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(144, 68)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(117, 16)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Additional Disc."
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(-1, 121)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 16)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Remarks"
        '
        'txtField06
        '
        Me.txtField06.Enabled = False
        Me.txtField06.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField06.Location = New System.Drawing.Point(260, 60)
        Me.txtField06.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField06.MaxLength = 6
        Me.txtField06.Name = "txtField06"
        Me.txtField06.Size = New System.Drawing.Size(92, 24)
        Me.txtField06.TabIndex = 12
        Me.txtField06.Text = "10"
        Me.txtField06.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'pnClients
        '
        Me.pnClients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnClients.Controls.Add(Me.Label1)
        Me.pnClients.Controls.Add(Me.txtField03)
        Me.pnClients.Controls.Add(Me.Label2)
        Me.pnClients.Controls.Add(Me.txtField04)
        Me.pnClients.Location = New System.Drawing.Point(4, 64)
        Me.pnClients.Name = "pnClients"
        Me.pnClients.Size = New System.Drawing.Size(358, 53)
        Me.pnClients.TabIndex = 17
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(161, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "No. of Clients"
        '
        'txtField03
        '
        Me.txtField03.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField03.Location = New System.Drawing.Point(260, 0)
        Me.txtField03.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField03.MaxLength = 2
        Me.txtField03.Name = "txtField03"
        Me.txtField03.Size = New System.Drawing.Size(92, 24)
        Me.txtField03.TabIndex = 6
        Me.txtField03.Text = "10"
        Me.txtField03.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(174, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "w/ Discount"
        '
        'txtField04
        '
        Me.txtField04.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField04.Location = New System.Drawing.Point(260, 27)
        Me.txtField04.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField04.MaxLength = 2
        Me.txtField04.Name = "txtField04"
        Me.txtField04.Size = New System.Drawing.Size(92, 24)
        Me.txtField04.TabIndex = 8
        Me.txtField04.Text = "10"
        Me.txtField04.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtField01
        '
        Me.txtField01.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField01.Location = New System.Drawing.Point(107, 38)
        Me.txtField01.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField01.MaxLength = 16
        Me.txtField01.Name = "txtField01"
        Me.txtField01.Size = New System.Drawing.Size(255, 24)
        Me.txtField01.TabIndex = 4
        Me.txtField01.Text = "1234567890123456"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(3, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Card Number"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(3, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 24)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Card"
        '
        'txtField02
        '
        Me.txtField02.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField02.ForeColor = System.Drawing.Color.Green
        Me.txtField02.Location = New System.Drawing.Point(62, 5)
        Me.txtField02.Margin = New System.Windows.Forms.Padding(2)
        Me.txtField02.Name = "txtField02"
        Me.txtField02.Size = New System.Drawing.Size(300, 29)
        Me.txtField02.TabIndex = 2
        Me.txtField02.Text = "Xurpas Loyalty Card"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(2, 1)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(68, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Discount"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(132, 93)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(123, 16)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Minimum Amount"
        '
        'txtField11
        '
        Me.txtField11.Enabled = False
        Me.txtField11.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField11.Location = New System.Drawing.Point(260, 86)
        Me.txtField11.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField11.MaxLength = 6
        Me.txtField11.Name = "txtField11"
        Me.txtField11.Size = New System.Drawing.Size(92, 24)
        Me.txtField11.TabIndex = 20
        Me.txtField11.Text = "10"
        Me.txtField11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmDiscount
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = Global.ggcRetailSales.My.Resources.Resources.mainbackground
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(388, 539)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.pnlMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDiscount"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TransparencyKey = System.Drawing.Color.Yellow
        Me.pnlMain.ResumeLayout(False)
        Me.pnlButtons.ResumeLayout(False)
        Me.pnlAmount.ResumeLayout(False)
        Me.pnlAmount.PerformLayout()
        Me.pnDetail.ResumeLayout(False)
        Me.pnDetail.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.dgvClients, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnClients.ResumeLayout(False)
        Me.pnClients.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents pnlAmount As System.Windows.Forms.Panel
    Friend WithEvents txtField02 As System.Windows.Forms.TextBox
    Friend WithEvents cmdButton01 As System.Windows.Forms.Button
    Friend WithEvents cmdButton00 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtField03 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtField01 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtField04 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtField06 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtField05 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtField09 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmdButton02 As System.Windows.Forms.Button
    Friend WithEvents cmdButton03 As System.Windows.Forms.Button
    Friend WithEvents pnClients As System.Windows.Forms.Panel
    Friend WithEvents pnDetail As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dgvClients As System.Windows.Forms.DataGridView
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtField10 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtField11 As System.Windows.Forms.TextBox
End Class
