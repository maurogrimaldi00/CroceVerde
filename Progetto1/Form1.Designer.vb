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
    Friend WithEvents txtScadTessera As System.Windows.Forms.TextBox
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
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnExportCsv As System.Windows.Forms.Button
    Friend WithEvents btnToggleHighlight As System.Windows.Forms.Button
    Friend WithEvents btnRinnovoTessera As System.Windows.Forms.Button


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
        txtScadTessera = New TextBox()
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
        lblStatus = New Label()
        btnExportCsv = New Button()
        btnToggleHighlight = New Button()
        btnRinnovoTessera = New Button()
        Button1 = New Button()
        Label9 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Label7 = New Label()
        Label8 = New Label()
        Label1 = New Label()
        Label10 = New Label()
        Label11 = New Label()
        Label12 = New Label()
        Label13 = New Label()
        Label14 = New Label()
        Label15 = New Label()
        Label16 = New Label()
        Label17 = New Label()
        Label21 = New Label()
        btnExportExcel = New Button()
        btnChiudi = New Button()
        CType(dgvAnagrafico, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvAnagrafico
        ' 
        dgvAnagrafico.AllowUserToResizeRows = False
        dgvAnagrafico.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvAnagrafico.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAnagrafico.Location = New Point(15, 15)
        dgvAnagrafico.Margin = New Padding(4)
        dgvAnagrafico.MultiSelect = False
        dgvAnagrafico.Name = "dgvAnagrafico"
        dgvAnagrafico.ReadOnly = True
        dgvAnagrafico.RowHeadersWidth = 51
        dgvAnagrafico.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAnagrafico.Size = New Size(1846, 542)
        dgvAnagrafico.TabIndex = 0
        dgvAnagrafico.UseWaitCursor = True
        ' 
        ' txtId
        ' 
        txtId.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtId.BackColor = Color.FromArgb(CByte(192), CByte(255), CByte(192))
        txtId.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtId.Location = New Point(289, 634)
        txtId.Margin = New Padding(4)
        txtId.Name = "txtId"
        txtId.ReadOnly = True
        txtId.Size = New Size(83, 30)
        txtId.TabIndex = 1
        txtId.TextAlign = HorizontalAlignment.Center
        txtId.UseWaitCursor = True
        ' 
        ' txtCognome
        ' 
        txtCognome.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtCognome.BackColor = Color.FromArgb(CByte(192), CByte(255), CByte(192))
        txtCognome.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtCognome.Location = New Point(380, 634)
        txtCognome.Margin = New Padding(4)
        txtCognome.Name = "txtCognome"
        txtCognome.Size = New Size(249, 30)
        txtCognome.TabIndex = 2
        txtCognome.UseWaitCursor = True
        ' 
        ' txtNome
        ' 
        txtNome.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtNome.BackColor = Color.FromArgb(CByte(192), CByte(255), CByte(192))
        txtNome.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtNome.Location = New Point(638, 634)
        txtNome.Margin = New Padding(4)
        txtNome.Name = "txtNome"
        txtNome.Size = New Size(249, 30)
        txtNome.TabIndex = 3
        txtNome.UseWaitCursor = True
        ' 
        ' dtpDataNascita
        ' 
        dtpDataNascita.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        dtpDataNascita.Format = DateTimePickerFormat.Short
        dtpDataNascita.Location = New Point(895, 638)
        dtpDataNascita.Margin = New Padding(4)
        dtpDataNascita.Name = "dtpDataNascita"
        dtpDataNascita.ShowCheckBox = True
        dtpDataNascita.Size = New Size(193, 27)
        dtpDataNascita.TabIndex = 4
        dtpDataNascita.UseWaitCursor = True
        ' 
        ' cmbSesso
        ' 
        cmbSesso.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        cmbSesso.BackColor = Color.FromArgb(CByte(192), CByte(255), CByte(192))
        cmbSesso.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSesso.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbSesso.FormattingEnabled = True
        cmbSesso.Items.AddRange(New Object() {"M", "F"})
        cmbSesso.Location = New Point(1096, 636)
        cmbSesso.Margin = New Padding(4)
        cmbSesso.Name = "cmbSesso"
        cmbSesso.Size = New Size(62, 31)
        cmbSesso.TabIndex = 5
        cmbSesso.UseWaitCursor = True
        ' 
        ' cmbQualifica
        ' 
        cmbQualifica.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        cmbQualifica.DropDownStyle = ComboBoxStyle.DropDownList
        cmbQualifica.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbQualifica.FormattingEnabled = True
        cmbQualifica.Location = New Point(960, 790)
        cmbQualifica.Margin = New Padding(4)
        cmbQualifica.Name = "cmbQualifica"
        cmbQualifica.Size = New Size(174, 31)
        cmbQualifica.TabIndex = 6
        cmbQualifica.UseWaitCursor = True
        ' 
        ' txtIndirizzo
        ' 
        txtIndirizzo.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtIndirizzo.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtIndirizzo.Location = New Point(289, 706)
        txtIndirizzo.Margin = New Padding(4)
        txtIndirizzo.Name = "txtIndirizzo"
        txtIndirizzo.Size = New Size(340, 30)
        txtIndirizzo.TabIndex = 7
        txtIndirizzo.UseWaitCursor = True
        ' 
        ' txtCivico
        ' 
        txtCivico.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtCivico.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCivico.Location = New Point(638, 706)
        txtCivico.Margin = New Padding(4)
        txtCivico.Name = "txtCivico"
        txtCivico.Size = New Size(65, 30)
        txtCivico.TabIndex = 8
        txtCivico.TextAlign = HorizontalAlignment.Center
        txtCivico.UseWaitCursor = True
        ' 
        ' txtLocalita
        ' 
        txtLocalita.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtLocalita.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtLocalita.Location = New Point(712, 706)
        txtLocalita.Margin = New Padding(4)
        txtLocalita.Name = "txtLocalita"
        txtLocalita.Size = New Size(280, 30)
        txtLocalita.TabIndex = 9
        txtLocalita.UseWaitCursor = True
        ' 
        ' txtProv
        ' 
        txtProv.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtProv.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtProv.Location = New Point(1005, 706)
        txtProv.Margin = New Padding(4)
        txtProv.Name = "txtProv"
        txtProv.Size = New Size(60, 30)
        txtProv.TabIndex = 10
        txtProv.TextAlign = HorizontalAlignment.Center
        txtProv.UseWaitCursor = True
        ' 
        ' txtCap
        ' 
        txtCap.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtCap.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCap.Location = New Point(1074, 706)
        txtCap.Margin = New Padding(4)
        txtCap.Name = "txtCap"
        txtCap.Size = New Size(84, 30)
        txtCap.TabIndex = 11
        txtCap.TextAlign = HorizontalAlignment.Center
        txtCap.UseWaitCursor = True
        ' 
        ' dtpDataIscrizione
        ' 
        dtpDataIscrizione.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        dtpDataIscrizione.Format = DateTimePickerFormat.Short
        dtpDataIscrizione.Location = New Point(289, 863)
        dtpDataIscrizione.Margin = New Padding(4)
        dtpDataIscrizione.Name = "dtpDataIscrizione"
        dtpDataIscrizione.ShowCheckBox = True
        dtpDataIscrizione.Size = New Size(182, 27)
        dtpDataIscrizione.TabIndex = 12
        dtpDataIscrizione.UseWaitCursor = True
        ' 
        ' txtCellulare
        ' 
        txtCellulare.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtCellulare.BackColor = Color.FromArgb(CByte(255), CByte(224), CByte(192))
        txtCellulare.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtCellulare.Location = New Point(290, 780)
        txtCellulare.Margin = New Padding(4)
        txtCellulare.Name = "txtCellulare"
        txtCellulare.Size = New Size(194, 30)
        txtCellulare.TabIndex = 13
        txtCellulare.TextAlign = HorizontalAlignment.Center
        txtCellulare.UseWaitCursor = True
        ' 
        ' txtCF
        ' 
        txtCF.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtCF.Font = New Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCF.Location = New Point(493, 784)
        txtCF.Margin = New Padding(4)
        txtCF.Name = "txtCF"
        txtCF.Size = New Size(249, 30)
        txtCF.TabIndex = 14
        txtCF.TextAlign = HorizontalAlignment.Center
        txtCF.UseWaitCursor = True
        ' 
        ' cmbZona
        ' 
        cmbZona.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        cmbZona.BackColor = Color.FromArgb(CByte(255), CByte(255), CByte(128))
        cmbZona.DropDownStyle = ComboBoxStyle.DropDownList
        cmbZona.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cmbZona.FormattingEnabled = True
        cmbZona.Location = New Point(750, 786)
        cmbZona.Margin = New Padding(4)
        cmbZona.Name = "cmbZona"
        cmbZona.Size = New Size(202, 31)
        cmbZona.TabIndex = 15
        cmbZona.UseWaitCursor = True
        ' 
        ' txtScadTessera
        ' 
        txtScadTessera.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        txtScadTessera.BackColor = Color.FromArgb(CByte(255), CByte(255), CByte(192))
        txtScadTessera.Font = New Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtScadTessera.Location = New Point(662, 865)
        txtScadTessera.Margin = New Padding(4)
        txtScadTessera.Name = "txtScadTessera"
        txtScadTessera.ReadOnly = True
        txtScadTessera.Size = New Size(156, 30)
        txtScadTessera.TabIndex = 16
        txtScadTessera.TextAlign = HorizontalAlignment.Center
        txtScadTessera.UseWaitCursor = True
        ' 
        ' chkSocio
        ' 
        chkSocio.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        chkSocio.AutoSize = True
        chkSocio.Location = New Point(1180, 634)
        chkSocio.Margin = New Padding(4)
        chkSocio.Name = "chkSocio"
        chkSocio.Size = New Size(68, 24)
        chkSocio.TabIndex = 17
        chkSocio.Text = "Socio"
        chkSocio.UseVisualStyleBackColor = True
        chkSocio.UseWaitCursor = True
        ' 
        ' chkMilite
        ' 
        chkMilite.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        chkMilite.AutoSize = True
        chkMilite.Location = New Point(1180, 674)
        chkMilite.Margin = New Padding(4)
        chkMilite.Name = "chkMilite"
        chkMilite.Size = New Size(69, 24)
        chkMilite.TabIndex = 18
        chkMilite.Text = "Milite"
        chkMilite.UseVisualStyleBackColor = True
        chkMilite.UseWaitCursor = True
        ' 
        ' chkAnnullato
        ' 
        chkAnnullato.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        chkAnnullato.AutoSize = True
        chkAnnullato.Location = New Point(1180, 755)
        chkAnnullato.Margin = New Padding(4)
        chkAnnullato.Name = "chkAnnullato"
        chkAnnullato.Size = New Size(95, 24)
        chkAnnullato.TabIndex = 19
        chkAnnullato.Text = "Annullato"
        chkAnnullato.UseVisualStyleBackColor = True
        chkAnnullato.UseWaitCursor = True
        ' 
        ' chkAssicurato
        ' 
        chkAssicurato.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        chkAssicurato.AutoSize = True
        chkAssicurato.Location = New Point(1180, 714)
        chkAssicurato.Margin = New Padding(4)
        chkAssicurato.Name = "chkAssicurato"
        chkAssicurato.Size = New Size(99, 24)
        chkAssicurato.TabIndex = 20
        chkAssicurato.Text = "Assicurato"
        chkAssicurato.ThreeState = True
        chkAssicurato.UseVisualStyleBackColor = True
        chkAssicurato.UseWaitCursor = True
        ' 
        ' btnAdd
        ' 
        btnAdd.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnAdd.BackColor = Color.FromArgb(CByte(128), CByte(255), CByte(128))
        btnAdd.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAdd.Location = New Point(1306, 626)
        btnAdd.Margin = New Padding(4)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(119, 38)
        btnAdd.TabIndex = 21
        btnAdd.Text = "Aggiungi"
        btnAdd.UseVisualStyleBackColor = False
        btnAdd.UseWaitCursor = True
        ' 
        ' btnUpdate
        ' 
        btnUpdate.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnUpdate.BackColor = Color.FromArgb(CByte(192), CByte(255), CByte(255))
        btnUpdate.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnUpdate.Location = New Point(1438, 626)
        btnUpdate.Margin = New Padding(4)
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Size = New Size(119, 38)
        btnUpdate.TabIndex = 22
        btnUpdate.Text = "Aggiorna"
        btnUpdate.UseVisualStyleBackColor = False
        btnUpdate.UseWaitCursor = True
        ' 
        ' btnDelete
        ' 
        btnDelete.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnDelete.BackColor = Color.FromArgb(CByte(255), CByte(128), CByte(128))
        btnDelete.Location = New Point(1306, 672)
        btnDelete.Margin = New Padding(4)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(119, 38)
        btnDelete.TabIndex = 23
        btnDelete.Text = "Elimina"
        btnDelete.UseVisualStyleBackColor = False
        btnDelete.UseWaitCursor = True
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnRefresh.BackColor = Color.FromArgb(CByte(255), CByte(255), CByte(192))
        btnRefresh.Location = New Point(1438, 672)
        btnRefresh.Margin = New Padding(4)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(119, 38)
        btnRefresh.TabIndex = 24
        btnRefresh.Text = "Ricarica"
        btnRefresh.UseVisualStyleBackColor = False
        btnRefresh.UseWaitCursor = True
        ' 
        ' btnClear
        ' 
        btnClear.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnClear.BackColor = Color.Silver
        btnClear.Location = New Point(1306, 582)
        btnClear.Margin = New Padding(4)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(250, 38)
        btnClear.TabIndex = 25
        btnClear.Text = "Pulisci campi"
        btnClear.UseVisualStyleBackColor = False
        btnClear.UseWaitCursor = True
        ' 
        ' txtSearch
        ' 
        txtSearch.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtSearch.BackColor = Color.FromArgb(CByte(255), CByte(255), CByte(192))
        txtSearch.Location = New Point(289, 569)
        txtSearch.Margin = New Padding(4)
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(774, 27)
        txtSearch.TabIndex = 26
        txtSearch.UseWaitCursor = True
        ' 
        ' btnSearch
        ' 
        btnSearch.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnSearch.BackColor = Color.FromArgb(CByte(255), CByte(192), CByte(255))
        btnSearch.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnSearch.Location = New Point(1076, 569)
        btnSearch.Margin = New Padding(4)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(162, 34)
        btnSearch.TabIndex = 27
        btnSearch.Text = "Ricerche"
        btnSearch.TextImageRelation = TextImageRelation.TextAboveImage
        btnSearch.UseVisualStyleBackColor = False
        btnSearch.UseWaitCursor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblStatus.AutoSize = True
        lblStatus.Location = New Point(289, 944)
        lblStatus.Margin = New Padding(4, 0, 4, 0)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(53, 20)
        lblStatus.TabIndex = 29
        lblStatus.Text = "Pronto"
        lblStatus.UseWaitCursor = True
        ' 
        ' btnExportCsv
        ' 
        btnExportCsv.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnExportCsv.BackColor = Color.FromArgb(CByte(255), CByte(255), CByte(128))
        btnExportCsv.Location = New Point(1564, 630)
        btnExportCsv.Margin = New Padding(4)
        btnExportCsv.Name = "btnExportCsv"
        btnExportCsv.Size = New Size(250, 34)
        btnExportCsv.TabIndex = 30
        btnExportCsv.Text = "Esporta CSV"
        btnExportCsv.UseVisualStyleBackColor = False
        btnExportCsv.UseWaitCursor = True
        ' 
        ' btnToggleHighlight
        ' 
        btnToggleHighlight.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnToggleHighlight.BackColor = Color.FromArgb(CByte(192), CByte(192), CByte(0))
        btnToggleHighlight.Location = New Point(1307, 718)
        btnToggleHighlight.Margin = New Padding(4)
        btnToggleHighlight.Name = "btnToggleHighlight"
        btnToggleHighlight.Size = New Size(250, 34)
        btnToggleHighlight.TabIndex = 33
        btnToggleHighlight.Text = "Tessere Scadute ON/OFF"
        btnToggleHighlight.UseVisualStyleBackColor = False
        btnToggleHighlight.UseWaitCursor = True
        ' 
        ' btnRinnovoTessera
        ' 
        btnRinnovoTessera.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnRinnovoTessera.BackColor = Color.FromArgb(CByte(255), CByte(215), CByte(0))
        btnRinnovoTessera.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRinnovoTessera.Location = New Point(1564, 721)
        btnRinnovoTessera.Margin = New Padding(4)
        btnRinnovoTessera.Name = "btnRinnovoTessera"
        btnRinnovoTessera.Size = New Size(250, 32)
        btnRinnovoTessera.TabIndex = 63
        btnRinnovoTessera.Text = "🔄 Rinnovo Tessera"
        btnRinnovoTessera.UseVisualStyleBackColor = False
        btnRinnovoTessera.UseWaitCursor = True
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Button1.BackColor = Color.FromArgb(CByte(255), CByte(192), CByte(128))
        Button1.Location = New Point(1564, 582)
        Button1.Margin = New Padding(4)
        Button1.Name = "Button1"
        Button1.Size = New Size(250, 41)
        Button1.TabIndex = 37
        Button1.Text = "Stampe"
        Button1.UseVisualStyleBackColor = False
        Button1.UseWaitCursor = True
        ' 
        ' Label9
        ' 
        Label9.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label9.AutoSize = True
        Label9.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label9.Location = New Point(289, 610)
        Label9.Margin = New Padding(4, 0, 4, 0)
        Label9.Name = "Label9"
        Label9.Size = New Size(48, 17)
        Label9.TabIndex = 43
        Label9.Text = "Codice"
        Label9.UseWaitCursor = True
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(289, 683)
        Label4.Margin = New Padding(4, 0, 4, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(57, 17)
        Label4.TabIndex = 44
        Label4.Text = "Indirizzo"
        Label4.UseWaitCursor = True
        ' 
        ' Label5
        ' 
        Label5.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(1096, 613)
        Label5.Margin = New Padding(4, 0, 4, 0)
        Label5.Name = "Label5"
        Label5.Size = New Size(42, 17)
        Label5.TabIndex = 45
        Label5.Text = "Sesso"
        Label5.UseWaitCursor = True
        ' 
        ' Label6
        ' 
        Label6.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label6.Location = New Point(898, 613)
        Label6.Margin = New Padding(4, 0, 4, 0)
        Label6.Name = "Label6"
        Label6.Size = New Size(82, 17)
        Label6.TabIndex = 46
        Label6.Text = "Data Nascita"
        Label6.UseWaitCursor = True
        ' 
        ' Label7
        ' 
        Label7.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label7.AutoSize = True
        Label7.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label7.Location = New Point(638, 613)
        Label7.Margin = New Padding(4, 0, 4, 0)
        Label7.Name = "Label7"
        Label7.Size = New Size(44, 17)
        Label7.TabIndex = 47
        Label7.Text = "Nome"
        Label7.UseWaitCursor = True
        ' 
        ' Label8
        ' 
        Label8.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label8.AutoSize = True
        Label8.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label8.Location = New Point(380, 613)
        Label8.Margin = New Padding(4, 0, 4, 0)
        Label8.Name = "Label8"
        Label8.Size = New Size(65, 17)
        Label8.TabIndex = 48
        Label8.Text = "Cognome"
        Label8.UseWaitCursor = True
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(1094, 683)
        Label1.Margin = New Padding(4, 0, 4, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(31, 17)
        Label1.TabIndex = 49
        Label1.Text = "Cap"
        Label1.UseWaitCursor = True
        ' 
        ' Label10
        ' 
        Label10.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label10.AutoSize = True
        Label10.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label10.Location = New Point(1020, 683)
        Label10.Margin = New Padding(4, 0, 4, 0)
        Label10.Name = "Label10"
        Label10.Size = New Size(23, 17)
        Label10.TabIndex = 50
        Label10.Text = "PV"
        Label10.UseWaitCursor = True
        ' 
        ' Label11
        ' 
        Label11.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label11.AutoSize = True
        Label11.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label11.Location = New Point(712, 683)
        Label11.Margin = New Padding(4, 0, 4, 0)
        Label11.Name = "Label11"
        Label11.Size = New Size(52, 17)
        Label11.TabIndex = 51
        Label11.Text = "Località"
        Label11.UseWaitCursor = True
        ' 
        ' Label12
        ' 
        Label12.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label12.AutoSize = True
        Label12.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label12.Location = New Point(635, 683)
        Label12.Margin = New Padding(4, 0, 4, 0)
        Label12.Name = "Label12"
        Label12.Size = New Size(42, 17)
        Label12.TabIndex = 52
        Label12.Text = "Civico"
        Label12.UseWaitCursor = True
        ' 
        ' Label13
        ' 
        Label13.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label13.AutoSize = True
        Label13.Font = New Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label13.Location = New Point(676, 838)
        Label13.Margin = New Padding(4, 0, 4, 0)
        Label13.Name = "Label13"
        Label13.Size = New Size(113, 17)
        Label13.TabIndex = 53
        Label13.Text = "Scadenza Tessera"
        Label13.UseWaitCursor = True
        ' 
        ' Label14
        ' 
        Label14.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label14.AutoSize = True
        Label14.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label14.Location = New Point(493, 758)
        Label14.Margin = New Padding(4, 0, 4, 0)
        Label14.Name = "Label14"
        Label14.Size = New Size(90, 17)
        Label14.TabIndex = 54
        Label14.Text = "Codice Fiscale"
        Label14.UseWaitCursor = True
        ' 
        ' Label15
        ' 
        Label15.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label15.AutoSize = True
        Label15.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label15.Location = New Point(290, 755)
        Label15.Margin = New Padding(4, 0, 4, 0)
        Label15.Name = "Label15"
        Label15.Size = New Size(58, 17)
        Label15.TabIndex = 55
        Label15.Text = "Telefono"
        Label15.UseWaitCursor = True
        ' 
        ' Label16
        ' 
        Label16.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label16.AutoSize = True
        Label16.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label16.Location = New Point(289, 838)
        Label16.Margin = New Padding(4, 0, 4, 0)
        Label16.Name = "Label16"
        Label16.Size = New Size(93, 17)
        Label16.TabIndex = 56
        Label16.Text = "Data Iscrizione"
        Label16.UseWaitCursor = True
        ' 
        ' Label17
        ' 
        Label17.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label17.AutoSize = True
        Label17.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label17.Location = New Point(960, 762)
        Label17.Margin = New Padding(4, 0, 4, 0)
        Label17.Name = "Label17"
        Label17.Size = New Size(58, 17)
        Label17.TabIndex = 57
        Label17.Text = "Qualifica"
        Label17.UseWaitCursor = True
        ' 
        ' Label21
        ' 
        Label21.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label21.Location = New Point(752, 760)
        Label21.Margin = New Padding(4, 0, 4, 0)
        Label21.Name = "Label21"
        Label21.Size = New Size(37, 17)
        Label21.TabIndex = 61
        Label21.Text = "Zona"
        Label21.UseWaitCursor = True
        ' 
        ' btnExportExcel
        ' 
        btnExportExcel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnExportExcel.BackColor = Color.FromArgb(CByte(128), CByte(255), CByte(128))
        btnExportExcel.Location = New Point(1564, 674)
        btnExportExcel.Margin = New Padding(4)
        btnExportExcel.Name = "btnExportExcel"
        btnExportExcel.Size = New Size(250, 39)
        btnExportExcel.TabIndex = 62
        btnExportExcel.Text = "Esporta Excel xlsx"
        btnExportExcel.UseVisualStyleBackColor = False
        btnExportExcel.UseWaitCursor = True
        ' 
        ' btnChiudi
        ' 
        btnChiudi.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnChiudi.BackColor = Color.FromArgb(CByte(128), CByte(128), CByte(255))
        btnChiudi.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnChiudi.Location = New Point(1450, 790)
        btnChiudi.Margin = New Padding(4)
        btnChiudi.Name = "btnChiudi"
        btnChiudi.Size = New Size(239, 36)
        btnChiudi.TabIndex = 64
        btnChiudi.Text = "🔙 Chiudi e Torna al Menu"
        btnChiudi.UseVisualStyleBackColor = False
        btnChiudi.UseWaitCursor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(120.0F, 120.0F)
        AutoScaleMode = AutoScaleMode.Dpi
        ClientSize = New Size(1876, 987)
        Controls.Add(btnChiudi)
        Controls.Add(btnExportExcel)
        Controls.Add(Label21)
        Controls.Add(Label17)
        Controls.Add(Label16)
        Controls.Add(Label15)
        Controls.Add(Label14)
        Controls.Add(Label13)
        Controls.Add(Label12)
        Controls.Add(Label11)
        Controls.Add(Label10)
        Controls.Add(Label1)
        Controls.Add(Label8)
        Controls.Add(Label7)
        Controls.Add(Label6)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label9)
        Controls.Add(btnRinnovoTessera)
        Controls.Add(Button1)
        Controls.Add(lblStatus)
        Controls.Add(btnToggleHighlight)
        Controls.Add(btnExportCsv)
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
        Controls.Add(txtScadTessera)
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
        Margin = New Padding(4)
        MinimumSize = New Size(1316, 987)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Anagrafica Generale"
        UseWaitCursor = True
        CType(dgvAnagrafico, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub
    Friend WithEvents Button1 As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents btnExportExcel As Button
    Friend WithEvents btnChiudi As Button

End Class