<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents dgvAnagrafico As System.Windows.Forms.DataGridView
    Friend WithEvents txtId As System.Windows.Forms.TextBox
    Friend WithEvents txtCognome As System.Windows.Forms.TextBox
    Friend WithEvents txtNome As System.Windows.Forms.TextBox
    Friend WithEvents dtpDataNascita As System.Windows.Forms.DateTimePicker
    Friend WithEvents cmbSesso As System.Windows.Forms.ComboBox
    Friend WithEvents cmbQualifica As System.Windows.Forms.ComboBox
    Friend WithEvents txtIndirizzo As System.Windows.Forms.TextBox
    Friend WithEvents txtCivico As System.Windows.Forms.TextBox
    Friend WithEvents txtLocalita As System.Windows.Forms.TextBox
    Friend WithEvents txtProv As System.Windows.Forms.TextBox
    Friend WithEvents txtCap As System.Windows.Forms.TextBox
    Friend WithEvents dtpDataIscrizione As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtCellulare As System.Windows.Forms.TextBox
    Friend WithEvents txtCF As System.Windows.Forms.TextBox
    Friend WithEvents cmbZona As System.Windows.Forms.ComboBox
    Friend WithEvents dtpScadTessera As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkSocio As System.Windows.Forms.CheckBox
    Friend WithEvents chkMilite As System.Windows.Forms.CheckBox
    Friend WithEvents chkAnnullato As System.Windows.Forms.CheckBox
    Friend WithEvents chkAssicurato As System.Windows.Forms.CheckBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnStampa As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnExportCsv As System.Windows.Forms.Button
    Friend WithEvents btnSaveLayout As System.Windows.Forms.Button
    Friend WithEvents btnRestoreLayout As System.Windows.Forms.Button
    Friend WithEvents btnToggleHighlight As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        dgvAnagrafico = New DataGridView()
        txtId = New TextBox()
        txtCognome = New TextBox()
        txtNome = New TextBox()
        dtpDataNascita = New DateTimePicker()
        cmbSesso = New ComboBox()
        cmbQualifica = New ComboBox()
        txtIndirizzo = New TextBox()
        txtCivico = New TextBox()
        txtLocalita = New TextBox()
        txtProv = New TextBox()
        txtCap = New TextBox()
        dtpDataIscrizione = New DateTimePicker()
        txtCellulare = New TextBox()
        txtCF = New TextBox()
        cmbZona = New ComboBox()
        dtpScadTessera = New DateTimePicker()
        chkSocio = New CheckBox()
        chkMilite = New CheckBox()
        chkAnnullato = New CheckBox()
        chkAssicurato = New CheckBox()
        btnAdd = New Button()
        btnUpdate = New Button()
        btnDelete = New Button()
        btnRefresh = New Button()
        btnClear = New Button()
        txtSearch = New TextBox()
        btnSearch = New Button()
        btnStampa = New Button()
        lblStatus = New Label()
        btnExportCsv = New Button()
        btnSaveLayout = New Button()
        btnRestoreLayout = New Button()
        btnToggleHighlight = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        CType(dgvAnagrafico, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvAnagrafico
        ' 
        dgvAnagrafico.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvAnagrafico.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAnagrafico.Location = New Point(12, 12)
        dgvAnagrafico.MultiSelect = False
        dgvAnagrafico.Name = "dgvAnagrafico"
        dgvAnagrafico.ReadOnly = True
        dgvAnagrafico.RowHeadersWidth = 51
        dgvAnagrafico.RowTemplate.Height = 25
        dgvAnagrafico.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAnagrafico.Size = New Size(810, 420)
        dgvAnagrafico.TabIndex = 0
        ' 
        ' txtId
        ' 
        txtId.Location = New Point(790, 12)
        txtId.Name = "txtId"
        txtId.ReadOnly = True
        txtId.Size = New Size(200, 27)
        txtId.TabIndex = 1
        ' 
        ' txtCognome
        ' 
        txtCognome.Location = New Point(790, 50)
        txtCognome.Name = "txtCognome"
        txtCognome.Size = New Size(200, 27)
        txtCognome.TabIndex = 2
        ' 
        ' txtNome
        ' 
        txtNome.Location = New Point(790, 88)
        txtNome.Name = "txtNome"
        txtNome.Size = New Size(200, 27)
        txtNome.TabIndex = 3
        ' 
        ' dtpDataNascita
        ' 
        dtpDataNascita.Format = DateTimePickerFormat.Short
        dtpDataNascita.Location = New Point(790, 126)
        dtpDataNascita.Name = "dtpDataNascita"
        dtpDataNascita.ShowCheckBox = True
        dtpDataNascita.Size = New Size(200, 27)
        dtpDataNascita.TabIndex = 4
        ' 
        ' cmbSesso
        ' 
        cmbSesso.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSesso.FormattingEnabled = True
        cmbSesso.Items.AddRange(New Object() {"M", "F"})
        cmbSesso.Location = New Point(790, 164)
        cmbSesso.Name = "cmbSesso"
        cmbSesso.Size = New Size(50, 28)
        cmbSesso.TabIndex = 5
        ' 
        ' cmbQualifica
        ' 
        cmbQualifica.DropDownStyle = ComboBoxStyle.DropDownList
        cmbQualifica.FormattingEnabled = True
        cmbQualifica.Location = New Point(850, 164)
        cmbQualifica.Name = "cmbQualifica"
        cmbQualifica.Size = New Size(140, 28)
        cmbQualifica.TabIndex = 6
        ' 
        ' txtIndirizzo
        ' 
        txtIndirizzo.Location = New Point(790, 202)
        txtIndirizzo.Name = "txtIndirizzo"
        txtIndirizzo.Size = New Size(200, 27)
        txtIndirizzo.TabIndex = 7
        ' 
        ' txtCivico
        ' 
        txtCivico.Location = New Point(790, 240)
        txtCivico.Name = "txtCivico"
        txtCivico.Size = New Size(80, 27)
        txtCivico.TabIndex = 8
        ' 
        ' txtLocalita
        ' 
        txtLocalita.Location = New Point(790, 278)
        txtLocalita.Name = "txtLocalita"
        txtLocalita.Size = New Size(200, 27)
        txtLocalita.TabIndex = 9
        ' 
        ' txtProv
        ' 
        txtProv.Location = New Point(976, 278)
        txtProv.Name = "txtProv"
        txtProv.Size = New Size(50, 27)
        txtProv.TabIndex = 10
        ' 
        ' txtCap
        ' 
        txtCap.Location = New Point(790, 316)
        txtCap.Name = "txtCap"
        txtCap.Size = New Size(80, 27)
        txtCap.TabIndex = 11
        ' 
        ' dtpDataIscrizione
        ' 
        dtpDataIscrizione.Format = DateTimePickerFormat.Short
        dtpDataIscrizione.Location = New Point(790, 354)
        dtpDataIscrizione.Name = "dtpDataIscrizione"
        dtpDataIscrizione.ShowCheckBox = True
        dtpDataIscrizione.Size = New Size(200, 27)
        dtpDataIscrizione.TabIndex = 12
        ' 
        ' txtCellulare
        ' 
        txtCellulare.Location = New Point(790, 392)
        txtCellulare.Name = "txtCellulare"
        txtCellulare.Size = New Size(200, 27)
        txtCellulare.TabIndex = 13
        ' 
        ' txtCF
        ' 
        txtCF.Location = New Point(790, 430)
        txtCF.Name = "txtCF"
        txtCF.Size = New Size(200, 27)
        txtCF.TabIndex = 14
        ' 
        ' cmbZona
        ' 
        cmbZona.DropDownStyle = ComboBoxStyle.DropDownList
        cmbZona.FormattingEnabled = True
        cmbZona.Location = New Point(790, 468)
        cmbZona.Name = "cmbZona"
        cmbZona.Size = New Size(200, 28)
        cmbZona.TabIndex = 15
        ' 
        ' dtpScadTessera
        ' 
        dtpScadTessera.Format = DateTimePickerFormat.Short
        dtpScadTessera.Location = New Point(790, 506)
        dtpScadTessera.Name = "dtpScadTessera"
        dtpScadTessera.Size = New Size(200, 27)
        dtpScadTessera.TabIndex = 16
        ' 
        ' chkSocio
        ' 
        chkSocio.AutoSize = True
        chkSocio.Location = New Point(790, 544)
        chkSocio.Name = "chkSocio"
        chkSocio.Size = New Size(68, 24)
        chkSocio.TabIndex = 17
        chkSocio.Text = "Socio"
        chkSocio.UseVisualStyleBackColor = True
        ' 
        ' chkMilite
        ' 
        chkMilite.AutoSize = True
        chkMilite.Location = New Point(860, 544)
        chkMilite.Name = "chkMilite"
        chkMilite.Size = New Size(69, 24)
        chkMilite.TabIndex = 18
        chkMilite.Text = "Milite"
        chkMilite.UseVisualStyleBackColor = True
        ' 
        ' chkAnnullato
        ' 
        chkAnnullato.AutoSize = True
        chkAnnullato.Location = New Point(930, 544)
        chkAnnullato.Name = "chkAnnullato"
        chkAnnullato.Size = New Size(95, 24)
        chkAnnullato.TabIndex = 19
        chkAnnullato.Text = "Annullato"
        chkAnnullato.UseVisualStyleBackColor = True
        ' 
        ' chkAssicurato
        ' 
        chkAssicurato.AutoSize = True
        chkAssicurato.Location = New Point(790, 574)
        chkAssicurato.Name = "chkAssicurato"
        chkAssicurato.Size = New Size(99, 24)
        chkAssicurato.TabIndex = 20
        chkAssicurato.Text = "Assicurato"
        chkAssicurato.ThreeState = True
        chkAssicurato.UseVisualStyleBackColor = True
        ' 
        ' btnAdd
        ' 
        btnAdd.Location = New Point(790, 606)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(95, 30)
        btnAdd.TabIndex = 21
        btnAdd.Text = "Aggiungi"
        btnAdd.UseVisualStyleBackColor = True
        ' 
        ' btnUpdate
        ' 
        btnUpdate.Location = New Point(895, 606)
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Size = New Size(95, 30)
        btnUpdate.TabIndex = 22
        btnUpdate.Text = "Aggiorna"
        btnUpdate.UseVisualStyleBackColor = True
        ' 
        ' btnDelete
        ' 
        btnDelete.Location = New Point(790, 642)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(95, 30)
        btnDelete.TabIndex = 23
        btnDelete.Text = "Elimina"
        btnDelete.UseVisualStyleBackColor = True
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Location = New Point(895, 642)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(95, 30)
        btnRefresh.TabIndex = 24
        btnRefresh.Text = "Ricarica"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' btnClear
        ' 
        btnClear.Location = New Point(790, 678)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(200, 30)
        btnClear.TabIndex = 25
        btnClear.Text = "Pulisci campi"
        btnClear.UseVisualStyleBackColor = True
        ' 
        ' txtSearch
        ' 
        txtSearch.Location = New Point(12, 444)
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(620, 27)
        txtSearch.TabIndex = 26
        ' 
        ' btnSearch
        ' 
        btnSearch.Location = New Point(642, 444)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(130, 27)
        btnSearch.TabIndex = 27
        btnSearch.Text = "Cerca"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' btnStampa
        ' 
        btnStampa.Location = New Point(642, 480)
        btnStampa.Name = "btnStampa"
        btnStampa.Size = New Size(130, 27)
        btnStampa.TabIndex = 28
        btnStampa.Text = "Stampa"
        btnStampa.UseVisualStyleBackColor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblStatus.AutoSize = True
        lblStatus.Location = New Point(12, 720)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(53, 20)
        lblStatus.TabIndex = 29
        lblStatus.Text = "Pronto"
        ' 
        ' btnExportCsv
        ' 
        btnExportCsv.Location = New Point(642, 516)
        btnExportCsv.Name = "btnExportCsv"
        btnExportCsv.Size = New Size(130, 27)
        btnExportCsv.TabIndex = 30
        btnExportCsv.Text = "Esporta CSV"
        btnExportCsv.UseVisualStyleBackColor = True
        ' 
        ' btnSaveLayout
        ' 
        btnSaveLayout.Location = New Point(642, 552)
        btnSaveLayout.Name = "btnSaveLayout"
        btnSaveLayout.Size = New Size(130, 27)
        btnSaveLayout.TabIndex = 31
        btnSaveLayout.Text = "Salva layout"
        btnSaveLayout.UseVisualStyleBackColor = True
        ' 
        ' btnRestoreLayout
        ' 
        btnRestoreLayout.Location = New Point(642, 588)
        btnRestoreLayout.Name = "btnRestoreLayout"
        btnRestoreLayout.Size = New Size(130, 27)
        btnRestoreLayout.TabIndex = 32
        btnRestoreLayout.Text = "Ripristina layout"
        btnRestoreLayout.UseVisualStyleBackColor = True
        ' 
        ' btnToggleHighlight
        ' 
        btnToggleHighlight.Location = New Point(642, 624)
        btnToggleHighlight.Name = "btnToggleHighlight"
        btnToggleHighlight.Size = New Size(130, 27)
        btnToggleHighlight.TabIndex = 33
        btnToggleHighlight.Text = "Evid. scadute"
        btnToggleHighlight.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(1000, 133)
        Label1.Name = "Label1"
        Label1.Size = New Size(38, 20)
        Label1.TabIndex = 34
        Label1.Text = "nasc"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(1000, 361)
        Label2.Name = "Label2"
        Label2.Size = New Size(71, 20)
        Label2.TabIndex = 35
        Label2.Text = "iscrizione"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(1000, 506)
        Label3.Name = "Label3"
        Label3.Size = New Size(70, 20)
        Label3.TabIndex = 36
        Label3.Text = "scadenza"
        ' 
        ' Form1
        ' 
        ClientSize = New Size(1090, 760)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(lblStatus)
        Controls.Add(btnToggleHighlight)
        Controls.Add(btnRestoreLayout)
        Controls.Add(btnSaveLayout)
        Controls.Add(btnExportCsv)
        Controls.Add(btnStampa)
        Controls.Add(btnSearch)
        Controls.Add(txtSearch)
        Controls.Add(btnClear)
        Controls.Add(btnRefresh)
        Controls.Add(btnDelete)
        Controls.Add(btnUpdate)
        Controls.Add(btnAdd)
        Controls.Add(chkAssicurato)
        Controls.Add(chkAnnullato)
        Controls.Add(chkMilite)
        Controls.Add(chkSocio)
        Controls.Add(dtpScadTessera)
        Controls.Add(cmbZona)
        Controls.Add(txtCF)
        Controls.Add(txtCellulare)
        Controls.Add(dtpDataIscrizione)
        Controls.Add(txtCap)
        Controls.Add(txtProv)
        Controls.Add(txtLocalita)
        Controls.Add(txtCivico)
        Controls.Add(txtIndirizzo)
        Controls.Add(cmbQualifica)
        Controls.Add(cmbSesso)
        Controls.Add(dtpDataNascita)
        Controls.Add(txtNome)
        Controls.Add(txtCognome)
        Controls.Add(txtId)
        Controls.Add(dgvAnagrafico)
        MinimumSize = New Size(1056, 799)
        Name = "Form1"
        Text = "Anagrafico - CRUD"
        CType(dgvAnagrafico, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label

End Class