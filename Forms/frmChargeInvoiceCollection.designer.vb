<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChargeInvoiceCollection
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
        Me.cmdButton04 = New System.Windows.Forms.Button()
        Me.cmdButton02 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gridCharge = New System.Windows.Forms.DataGridView()
        Me.gridInvoices = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.cmdButton00 = New System.Windows.Forms.Button()
        Me.cmdButton01 = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnlMain.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridCharge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridInvoices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.Transparent
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlMain.Controls.Add(Me.Panel1)
        Me.pnlMain.Controls.Add(Me.Label1)
        Me.pnlMain.Controls.Add(Me.gridCharge)
        Me.pnlMain.Controls.Add(Me.gridInvoices)
        Me.pnlMain.Controls.Add(Me.pnlButtons)
        Me.pnlMain.Location = New System.Drawing.Point(5, 23)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(389, 473)
        Me.pnlMain.TabIndex = 2
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.cmdButton04)
        Me.Panel1.Controls.Add(Me.cmdButton02)
        Me.Panel1.Location = New System.Drawing.Point(2, 164)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(379, 40)
        Me.Panel1.TabIndex = 21
        '
        'cmdButton04
        '
        Me.cmdButton04.BackColor = System.Drawing.Color.Red
        Me.cmdButton04.FlatAppearance.BorderSize = 0
        Me.cmdButton04.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton04.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton04.ForeColor = System.Drawing.Color.White
        Me.cmdButton04.Location = New System.Drawing.Point(5, 3)
        Me.cmdButton04.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton04.Name = "cmdButton04"
        Me.cmdButton04.Size = New System.Drawing.Size(71, 30)
        Me.cmdButton04.TabIndex = 20
        Me.cmdButton04.TabStop = False
        Me.cmdButton04.Text = "RESET"
        Me.cmdButton04.UseVisualStyleBackColor = False
        '
        'cmdButton02
        '
        Me.cmdButton02.BackColor = System.Drawing.Color.Green
        Me.cmdButton02.FlatAppearance.BorderSize = 0
        Me.cmdButton02.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton02.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton02.ForeColor = System.Drawing.Color.White
        Me.cmdButton02.Location = New System.Drawing.Point(299, 3)
        Me.cmdButton02.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton02.Name = "cmdButton02"
        Me.cmdButton02.Size = New System.Drawing.Size(71, 30)
        Me.cmdButton02.TabIndex = 18
        Me.cmdButton02.TabStop = False
        Me.cmdButton02.Text = "PAY BILL"
        Me.cmdButton02.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(3, 206)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 16)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Pay Charge"
        '
        'gridCharge
        '
        Me.gridCharge.AllowUserToAddRows = False
        Me.gridCharge.AllowUserToDeleteRows = False
        Me.gridCharge.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridCharge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCharge.Location = New System.Drawing.Point(3, 226)
        Me.gridCharge.Name = "gridCharge"
        Me.gridCharge.ReadOnly = True
        Me.gridCharge.Size = New System.Drawing.Size(379, 195)
        Me.gridCharge.TabIndex = 10
        Me.gridCharge.TabStop = False
        '
        'gridInvoices
        '
        Me.gridInvoices.AllowUserToAddRows = False
        Me.gridInvoices.AllowUserToDeleteRows = False
        Me.gridInvoices.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridInvoices.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1})
        Me.gridInvoices.Location = New System.Drawing.Point(3, 3)
        Me.gridInvoices.Name = "gridInvoices"
        Me.gridInvoices.Size = New System.Drawing.Size(379, 152)
        Me.gridInvoices.TabIndex = 3
        Me.gridInvoices.TabStop = False
        '
        'Column1
        '
        Me.Column1.FalseValue = "0"
        Me.Column1.HeaderText = ""
        Me.Column1.IndeterminateValue = "0"
        Me.Column1.Name = "Column1"
        Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column1.TrueValue = "1"
        '
        'pnlButtons
        '
        Me.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlButtons.Controls.Add(Me.cmdButton00)
        Me.pnlButtons.Controls.Add(Me.cmdButton01)
        Me.pnlButtons.Location = New System.Drawing.Point(3, 426)
        Me.pnlButtons.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(379, 40)
        Me.pnlButtons.TabIndex = 9
        '
        'cmdButton00
        '
        Me.cmdButton00.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmdButton00.FlatAppearance.BorderSize = 0
        Me.cmdButton00.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton00.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton00.ForeColor = System.Drawing.Color.White
        Me.cmdButton00.Location = New System.Drawing.Point(237, 3)
        Me.cmdButton00.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton00.Name = "cmdButton00"
        Me.cmdButton00.Size = New System.Drawing.Size(64, 30)
        Me.cmdButton00.TabIndex = 9
        Me.cmdButton00.TabStop = False
        Me.cmdButton00.Text = "PAY"
        Me.cmdButton00.UseVisualStyleBackColor = False
        '
        'cmdButton01
        '
        Me.cmdButton01.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmdButton01.FlatAppearance.BorderSize = 0
        Me.cmdButton01.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdButton01.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdButton01.ForeColor = System.Drawing.Color.White
        Me.cmdButton01.Location = New System.Drawing.Point(304, 3)
        Me.cmdButton01.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdButton01.Name = "cmdButton01"
        Me.cmdButton01.Size = New System.Drawing.Size(64, 30)
        Me.cmdButton01.TabIndex = 10
        Me.cmdButton01.TabStop = False
        Me.cmdButton01.Text = "CLOSE"
        Me.cmdButton01.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(2, 2)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(185, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Charge Invoice Collection"
        '
        'frmChargeInvoiceCollection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = Global.ggcRetailSales.My.Resources.Resources.mainbackground
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(395, 501)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.pnlMain)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChargeInvoiceCollection"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TransparencyKey = System.Drawing.SystemColors.Window
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.gridCharge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridInvoices, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlButtons.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents cmdButton00 As System.Windows.Forms.Button
    Friend WithEvents cmdButton01 As System.Windows.Forms.Button
    Friend WithEvents gridInvoices As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gridCharge As System.Windows.Forms.DataGridView
    Friend WithEvents cmdButton02 As System.Windows.Forms.Button
    Friend WithEvents cmdButton04 As System.Windows.Forms.Button
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
End Class
