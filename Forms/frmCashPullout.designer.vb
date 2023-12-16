<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCashPullout
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
        Me.cmdButton00 = New System.Windows.Forms.Button()
        Me.cmdButton01 = New System.Windows.Forms.Button()
        Me.pnlAmount = New System.Windows.Forms.Panel()
        Me.txtField82 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtField81 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblCashier = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtField83 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtField04 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnlMain.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.pnlAmount.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.Transparent
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlMain.Controls.Add(Me.pnlButtons)
        Me.pnlMain.Controls.Add(Me.pnlAmount)
        Me.pnlMain.Location = New System.Drawing.Point(5, 23)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(344, 286)
        Me.pnlMain.TabIndex = 2
        '
        'pnlButtons
        '
        Me.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlButtons.Controls.Add(Me.cmdButton00)
        Me.pnlButtons.Controls.Add(Me.cmdButton01)
        Me.pnlButtons.Location = New System.Drawing.Point(3, 241)
        Me.pnlButtons.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(335, 40)
        Me.pnlButtons.TabIndex = 7
        '
        'cmdButton00
        '
        Me.cmdButton00.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmdButton00.FlatAppearance.BorderSize = 0
        Me.cmdButton00.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton00.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton00.ForeColor = System.Drawing.Color.White
        Me.cmdButton00.Location = New System.Drawing.Point(200, 2)
        Me.cmdButton00.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton00.Name = "cmdButton00"
        Me.cmdButton00.Size = New System.Drawing.Size(64, 32)
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
        Me.cmdButton01.Location = New System.Drawing.Point(266, 2)
        Me.cmdButton01.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton01.Name = "cmdButton01"
        Me.cmdButton01.Size = New System.Drawing.Size(64, 32)
        Me.cmdButton01.TabIndex = 14
        Me.cmdButton01.Text = "CANCEL"
        Me.cmdButton01.UseVisualStyleBackColor = False
        '
        'pnlAmount
        '
        Me.pnlAmount.BackColor = System.Drawing.Color.Transparent
        Me.pnlAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAmount.Controls.Add(Me.txtField82)
        Me.pnlAmount.Controls.Add(Me.Label5)
        Me.pnlAmount.Controls.Add(Me.txtField81)
        Me.pnlAmount.Controls.Add(Me.Label2)
        Me.pnlAmount.Controls.Add(Me.lblCashier)
        Me.pnlAmount.Controls.Add(Me.Label1)
        Me.pnlAmount.Controls.Add(Me.txtField83)
        Me.pnlAmount.Controls.Add(Me.Label3)
        Me.pnlAmount.Controls.Add(Me.Label4)
        Me.pnlAmount.Controls.Add(Me.txtField04)
        Me.pnlAmount.Location = New System.Drawing.Point(3, 2)
        Me.pnlAmount.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlAmount.Name = "pnlAmount"
        Me.pnlAmount.Size = New System.Drawing.Size(335, 238)
        Me.pnlAmount.TabIndex = 5
        '
        'txtField82
        '
        Me.txtField82.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField82.ForeColor = System.Drawing.Color.Red
        Me.txtField82.Location = New System.Drawing.Point(173, 109)
        Me.txtField82.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField82.MaxLength = 16
        Me.txtField82.Name = "txtField82"
        Me.txtField82.ReadOnly = True
        Me.txtField82.Size = New System.Drawing.Size(154, 26)
        Me.txtField82.TabIndex = 10
        Me.txtField82.Text = "0.00"
        Me.txtField82.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(60, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(99, 16)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Total Pullout:"
        '
        'txtField81
        '
        Me.txtField81.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField81.Location = New System.Drawing.Point(173, 167)
        Me.txtField81.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField81.MaxLength = 16
        Me.txtField81.Name = "txtField81"
        Me.txtField81.ReadOnly = True
        Me.txtField81.Size = New System.Drawing.Size(154, 26)
        Me.txtField81.TabIndex = 8
        Me.txtField81.Text = "0.00"
        Me.txtField81.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(73, 177)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Petty Cash:"
        '
        'lblCashier
        '
        Me.lblCashier.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCashier.ForeColor = System.Drawing.Color.White
        Me.lblCashier.Location = New System.Drawing.Point(30, 32)
        Me.lblCashier.Name = "lblCashier"
        Me.lblCashier.Size = New System.Drawing.Size(304, 30)
        Me.lblCashier.TabIndex = 6
        Me.lblCashier.Text = "Michael Cuison"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(4, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Cashier"
        '
        'txtField83
        '
        Me.txtField83.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField83.ForeColor = System.Drawing.Color.Green
        Me.txtField83.Location = New System.Drawing.Point(173, 138)
        Me.txtField83.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField83.MaxLength = 16
        Me.txtField83.Name = "txtField83"
        Me.txtField83.ReadOnly = True
        Me.txtField83.Size = New System.Drawing.Size(154, 26)
        Me.txtField83.TabIndex = 4
        Me.txtField83.Text = "0.00"
        Me.txtField83.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(32, 148)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(127, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Cashbox Amount:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(26, 82)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(133, 24)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Cash Pullout:"
        '
        'txtField04
        '
        Me.txtField04.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField04.ForeColor = System.Drawing.Color.Red
        Me.txtField04.Location = New System.Drawing.Point(173, 75)
        Me.txtField04.Margin = New System.Windows.Forms.Padding(2)
        Me.txtField04.Name = "txtField04"
        Me.txtField04.Size = New System.Drawing.Size(154, 31)
        Me.txtField04.TabIndex = 2
        Me.txtField04.Text = "0.00"
        Me.txtField04.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(2, 4)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(94, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Cash Pullout"
        '
        'frmCashPullout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = Global.ggcRetailSales.My.Resources.Resources.background
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(354, 315)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.pnlMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCashPullout"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TransparencyKey = System.Drawing.Color.Yellow
        Me.pnlMain.ResumeLayout(False)
        Me.pnlButtons.ResumeLayout(False)
        Me.pnlAmount.ResumeLayout(False)
        Me.pnlAmount.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents pnlAmount As System.Windows.Forms.Panel
    Friend WithEvents txtField04 As System.Windows.Forms.TextBox
    Friend WithEvents cmdButton01 As System.Windows.Forms.Button
    Friend WithEvents cmdButton00 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtField83 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblCashier As System.Windows.Forms.Label
    Friend WithEvents txtField81 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtField82 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
