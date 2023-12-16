<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPickTablex
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
        Me.txtField02 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtField01 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtField00 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnlMain.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.pnlAmount.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.Transparent
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMain.Controls.Add(Me.pnlButtons)
        Me.pnlMain.Controls.Add(Me.pnlAmount)
        Me.pnlMain.Location = New System.Drawing.Point(5, 23)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(344, 155)
        Me.pnlMain.TabIndex = 2
        '
        'pnlButtons
        '
        Me.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlButtons.Controls.Add(Me.cmdButton00)
        Me.pnlButtons.Controls.Add(Me.cmdButton01)
        Me.pnlButtons.Location = New System.Drawing.Point(1, 111)
        Me.pnlButtons.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(339, 40)
        Me.pnlButtons.TabIndex = 7
        '
        'cmdButton00
        '
        Me.cmdButton00.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmdButton00.FlatAppearance.BorderSize = 0
        Me.cmdButton00.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton00.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton00.ForeColor = System.Drawing.Color.White
        Me.cmdButton00.Location = New System.Drawing.Point(204, 3)
        Me.cmdButton00.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton00.Name = "cmdButton00"
        Me.cmdButton00.Size = New System.Drawing.Size(64, 32)
        Me.cmdButton00.TabIndex = 7
        Me.cmdButton00.Text = "OK"
        Me.cmdButton00.UseVisualStyleBackColor = False
        '
        'cmdButton01
        '
        Me.cmdButton01.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmdButton01.FlatAppearance.BorderSize = 0
        Me.cmdButton01.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton01.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton01.ForeColor = System.Drawing.Color.White
        Me.cmdButton01.Location = New System.Drawing.Point(270, 3)
        Me.cmdButton01.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton01.Name = "cmdButton01"
        Me.cmdButton01.Size = New System.Drawing.Size(64, 32)
        Me.cmdButton01.TabIndex = 8
        Me.cmdButton01.Text = "CANCEL"
        Me.cmdButton01.UseVisualStyleBackColor = False
        '
        'pnlAmount
        '
        Me.pnlAmount.BackColor = System.Drawing.Color.Transparent
        Me.pnlAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAmount.Controls.Add(Me.txtField02)
        Me.pnlAmount.Controls.Add(Me.Label1)
        Me.pnlAmount.Controls.Add(Me.txtField01)
        Me.pnlAmount.Controls.Add(Me.Label3)
        Me.pnlAmount.Controls.Add(Me.Label4)
        Me.pnlAmount.Controls.Add(Me.txtField00)
        Me.pnlAmount.Location = New System.Drawing.Point(1, 2)
        Me.pnlAmount.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlAmount.Name = "pnlAmount"
        Me.pnlAmount.Size = New System.Drawing.Size(339, 107)
        Me.pnlAmount.TabIndex = 5
        '
        'txtField02
        '
        Me.txtField02.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField02.Location = New System.Drawing.Point(286, 68)
        Me.txtField02.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField02.Name = "txtField02"
        Me.txtField02.ReadOnly = True
        Me.txtField02.Size = New System.Drawing.Size(48, 26)
        Me.txtField02.TabIndex = 8
        Me.txtField02.Text = "10"
        Me.txtField02.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(182, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 16)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "No. of Clients"
        '
        'txtField01
        '
        Me.txtField01.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField01.Location = New System.Drawing.Point(107, 39)
        Me.txtField01.Margin = New System.Windows.Forms.Padding(1)
        Me.txtField01.Name = "txtField01"
        Me.txtField01.ReadOnly = True
        Me.txtField01.Size = New System.Drawing.Size(227, 26)
        Me.txtField01.TabIndex = 6
        Me.txtField01.Text = "1234-5678-9012-3456"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(3, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Card Number"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(3, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 24)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Card"
        '
        'txtField00
        '
        Me.txtField00.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtField00.ForeColor = System.Drawing.Color.Green
        Me.txtField00.Location = New System.Drawing.Point(62, 5)
        Me.txtField00.Margin = New System.Windows.Forms.Padding(2)
        Me.txtField00.Name = "txtField00"
        Me.txtField00.Size = New System.Drawing.Size(272, 31)
        Me.txtField00.TabIndex = 4
        Me.txtField00.Text = "Xurpas Loyalty Card"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(2, 2)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(68, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Discount"
        '
        'frmPickTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = Global.ggcRetailSales.My.Resources.Resources.abstract_dark_wood_textures_backgrounds_1280x720
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(354, 184)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.pnlMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPickTable"
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
    Friend WithEvents txtField00 As System.Windows.Forms.TextBox
    Friend WithEvents cmdButton01 As System.Windows.Forms.Button
    Friend WithEvents cmdButton00 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtField02 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtField01 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
