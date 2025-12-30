<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormFiltriReport
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

    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbZonaFiltro As System.Windows.Forms.ComboBox
    Friend WithEvents cmbQualificaFiltro As System.Windows.Forms.ComboBox
    Friend WithEvents chkSoloSoci As System.Windows.Forms.CheckBox
    Friend WithEvents chkSoloMiliti As System.Windows.Forms.CheckBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnAnnulla As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbZonaFiltro = New System.Windows.Forms.ComboBox()
        Me.cmbQualificaFiltro = New System.Windows.Forms.ComboBox()
        Me.chkSoloSoci = New System.Windows.Forms.CheckBox()
        Me.chkSoloMiliti = New System.Windows.Forms.CheckBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnAnnulla = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Zona:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Qualifica:"
        '
        'cmbZonaFiltro
        '
        Me.cmbZonaFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbZonaFiltro.FormattingEnabled = True
        Me.cmbZonaFiltro.Location = New System.Drawing.Point(100, 22)
        Me.cmbZonaFiltro.Name = "cmbZonaFiltro"
        Me.cmbZonaFiltro.Size = New System.Drawing.Size(280, 23)
        Me.cmbZonaFiltro.TabIndex = 2
        '
        'cmbQualificaFiltro
        '
        Me.cmbQualificaFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbQualificaFiltro.FormattingEnabled = True
        Me.cmbQualificaFiltro.Location = New System.Drawing.Point(100, 62)
        Me.cmbQualificaFiltro.Name = "cmbQualificaFiltro"
        Me.cmbQualificaFiltro.Size = New System.Drawing.Size(280, 23)
        Me.cmbQualificaFiltro.TabIndex = 3
        '
        'chkSoloSoci
        '
        Me.chkSoloSoci.AutoSize = True
        Me.chkSoloSoci.Location = New System.Drawing.Point(15, 25)
        Me.chkSoloSoci.Name = "chkSoloSoci"
        Me.chkSoloSoci.Size = New System.Drawing.Size(76, 19)
        Me.chkSoloSoci.TabIndex = 4
        Me.chkSoloSoci.Text = "Solo Soci"
        Me.chkSoloSoci.UseVisualStyleBackColor = True
        '
        'chkSoloMiliti
        '
        Me.chkSoloMiliti.AutoSize = True
        Me.chkSoloMiliti.Location = New System.Drawing.Point(15, 55)
        Me.chkSoloMiliti.Name = "chkSoloMiliti"
        Me.chkSoloMiliti.Size = New System.Drawing.Size(85, 19)
        Me.chkSoloMiliti.TabIndex = 5
        Me.chkSoloMiliti.Text = "Solo Militi"
        Me.chkSoloMiliti.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(200, 240)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(90, 30)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnAnnulla
        '
        Me.btnAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnAnnulla.Location = New System.Drawing.Point(300, 240)
        Me.btnAnnulla.Name = "btnAnnulla"
        Me.btnAnnulla.Size = New System.Drawing.Size(90, 30)
        Me.btnAnnulla.TabIndex = 7
        Me.btnAnnulla.Text = "Annulla"
        Me.btnAnnulla.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkSoloSoci)
        Me.GroupBox1.Controls.Add(Me.chkSoloMiliti)
        Me.GroupBox1.Location = New System.Drawing.Point(20, 105)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(360, 90)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filtri aggiuntivi"
        '
        'FormFiltriReport
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnAnnulla
        Me.ClientSize = New System.Drawing.Size(404, 285)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnAnnulla)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.cmbQualificaFiltro)
        Me.Controls.Add(Me.cmbZonaFiltro)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormFiltriReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Filtri Report Anagrafico"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
End Class