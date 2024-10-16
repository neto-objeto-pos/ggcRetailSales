<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmChargeCriteria
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.cmdButton00 = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdButton01 = New System.Windows.Forms.Button()
        Me.txtField01 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel3
        '
        Me.Panel3.AutoSize = True
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.Controls.Add(Me.cmdButton00)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(10, 152)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        Me.Panel3.Size = New System.Drawing.Size(471, 40)
        Me.Panel3.TabIndex = 8
        '
        'cmdButton00
        '
        Me.cmdButton00.AutoSize = True
        Me.cmdButton00.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmdButton00.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdButton00.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton00.Location = New System.Drawing.Point(0, 5)
        Me.cmdButton00.Name = "cmdButton00"
        Me.cmdButton00.Size = New System.Drawing.Size(471, 35)
        Me.cmdButton00.TabIndex = 0
        Me.cmdButton00.Text = "SCAN QR CODE"
        Me.cmdButton00.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.AutoSize = True
        Me.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Panel1.CausesValidation = False
        Me.Panel1.Controls.Add(Me.lblMessage)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(10, 10)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Padding = New System.Windows.Forms.Padding(3, 3, 3, 6)
        Me.Panel1.Size = New System.Drawing.Size(471, 74)
        Me.Panel1.TabIndex = 6
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.Color.Transparent
        Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMessage.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblMessage.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lblMessage.Location = New System.Drawing.Point(3, 3)
        Me.lblMessage.Margin = New System.Windows.Forms.Padding(3, 0, 3, 10)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Padding = New System.Windows.Forms.Padding(5)
        Me.lblMessage.Size = New System.Drawing.Size(465, 65)
        Me.lblMessage.TabIndex = 6
        Me.lblMessage.Text = "Hi ! Do you have your Empployee QR Code?"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblMessage.UseMnemonic = False
        '
        'Panel2
        '
        Me.Panel2.AutoSize = True
        Me.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.Panel2.Controls.Add(Me.GroupBox1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(10, 84)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(471, 68)
        Me.Panel2.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.GroupBox1.Controls.Add(Me.cmdButton01)
        Me.GroupBox1.Controls.Add(Me.txtField01)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.GroupBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox1.Size = New System.Drawing.Size(471, 68)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "For employees without a QR Code, please enter your Employee ID below."
        '
        'cmdButton01
        '
        Me.cmdButton01.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton01.Location = New System.Drawing.Point(399, 25)
        Me.cmdButton01.Name = "cmdButton01"
        Me.cmdButton01.Size = New System.Drawing.Size(69, 23)
        Me.cmdButton01.TabIndex = 7
        Me.cmdButton01.Text = "SUBMIT"
        Me.cmdButton01.UseVisualStyleBackColor = True
        '
        'txtField01
        '
        Me.txtField01.Location = New System.Drawing.Point(125, 29)
        Me.txtField01.Name = "txtField01"
        Me.txtField01.Size = New System.Drawing.Size(268, 20)
        Me.txtField01.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(20, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "EMPLOYEE ID"
        '
        'frmChargeCriteria
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackgroundImage = Global.ggcRetailSales.My.Resources.Resources.mainbackground
        Me.ClientSize = New System.Drawing.Size(491, 207)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChargeCriteria"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "QR Form"
        Me.TopMost = True
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblMessage As Windows.Forms.Label
    Friend WithEvents Panel2 As Windows.Forms.Panel
    Friend WithEvents Panel3 As Windows.Forms.Panel
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents cmdButton00 As Windows.Forms.Button
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents txtField01 As Windows.Forms.TextBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents cmdButton01 As Windows.Forms.Button
End Class
