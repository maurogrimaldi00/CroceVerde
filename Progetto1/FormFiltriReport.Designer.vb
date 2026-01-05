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
        Label1 = New Label()
        Label2 = New Label()
        cmbZonaFiltro = New ComboBox()
        cmbQualificaFiltro = New ComboBox()
        chkSoloSoci = New CheckBox()
        chkSoloMiliti = New CheckBox()
        btnOK = New Button()
        btnAnnulla = New Button()
        GroupBox1 = New GroupBox()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(23, 33)
        Label1.Name = "Label1"
        Label1.Size = New Size(46, 20)
        Label1.TabIndex = 0
        Label1.Text = "Zona:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(23, 87)
        Label2.Name = "Label2"
        Label2.Size = New Size(71, 20)
        Label2.TabIndex = 1
        Label2.Text = "Qualifica:"
        ' 
        ' cmbZonaFiltro
        ' 
        cmbZonaFiltro.DropDownStyle = ComboBoxStyle.DropDownList
        cmbZonaFiltro.FormattingEnabled = True
        cmbZonaFiltro.Location = New Point(114, 29)
        cmbZonaFiltro.Margin = New Padding(3, 4, 3, 4)
        cmbZonaFiltro.Name = "cmbZonaFiltro"
        cmbZonaFiltro.Size = New Size(319, 28)
        cmbZonaFiltro.TabIndex = 2
        ' 
        ' cmbQualificaFiltro
        ' 
        cmbQualificaFiltro.DropDownStyle = ComboBoxStyle.DropDownList
        cmbQualificaFiltro.FormattingEnabled = True
        cmbQualificaFiltro.Location = New Point(114, 83)
        cmbQualificaFiltro.Margin = New Padding(3, 4, 3, 4)
        cmbQualificaFiltro.Name = "cmbQualificaFiltro"
        cmbQualificaFiltro.Size = New Size(319, 28)
        cmbQualificaFiltro.TabIndex = 3
        ' 
        ' chkSoloSoci
        ' 
        chkSoloSoci.AutoSize = True
        chkSoloSoci.Location = New Point(17, 33)
        chkSoloSoci.Margin = New Padding(3, 4, 3, 4)
        chkSoloSoci.Name = "chkSoloSoci"
        chkSoloSoci.Size = New Size(93, 24)
        chkSoloSoci.TabIndex = 4
        chkSoloSoci.Text = "Solo Soci"
        chkSoloSoci.UseVisualStyleBackColor = True
        ' 
        ' chkSoloMiliti
        ' 
        chkSoloMiliti.AutoSize = True
        chkSoloMiliti.Location = New Point(17, 73)
        chkSoloMiliti.Margin = New Padding(3, 4, 3, 4)
        chkSoloMiliti.Name = "chkSoloMiliti"
        chkSoloMiliti.Size = New Size(99, 24)
        chkSoloMiliti.TabIndex = 5
        chkSoloMiliti.Text = "Solo Militi"
        chkSoloMiliti.UseVisualStyleBackColor = True
        ' 
        ' btnOK
        ' 
        btnOK.Location = New Point(229, 320)
        btnOK.Margin = New Padding(3, 4, 3, 4)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(103, 40)
        btnOK.TabIndex = 6
        btnOK.Text = "OK"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnAnnulla
        ' 
        btnAnnulla.DialogResult = DialogResult.Cancel
        btnAnnulla.Location = New Point(343, 320)
        btnAnnulla.Margin = New Padding(3, 4, 3, 4)
        btnAnnulla.Name = "btnAnnulla"
        btnAnnulla.Size = New Size(103, 40)
        btnAnnulla.TabIndex = 7
        btnAnnulla.Text = "Annulla"
        btnAnnulla.UseVisualStyleBackColor = True
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(chkSoloSoci)
        GroupBox1.Controls.Add(chkSoloMiliti)
        GroupBox1.Location = New Point(23, 140)
        GroupBox1.Margin = New Padding(3, 4, 3, 4)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Padding = New Padding(3, 4, 3, 4)
        GroupBox1.Size = New Size(411, 120)
        GroupBox1.TabIndex = 8
        GroupBox1.TabStop = False
        GroupBox1.Text = "Filtri aggiuntivi"
        ' 
        ' FormFiltriReport
        ' 
        AcceptButton = btnOK
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnAnnulla
        ClientSize = New Size(462, 380)
        Controls.Add(GroupBox1)
        Controls.Add(btnAnnulla)
        Controls.Add(btnOK)
        Controls.Add(cmbQualificaFiltro)
        Controls.Add(cmbZonaFiltro)
        Controls.Add(Label2)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Margin = New Padding(3, 4, 3, 4)
        MaximizeBox = False
        MinimizeBox = False
        Name = "FormFiltriReport"
        StartPosition = FormStartPosition.CenterParent
        Text = "Filtri Report Anagrafico"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub
End Class