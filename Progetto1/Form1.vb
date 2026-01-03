Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Windows.Forms
Imports FastReport
Imports FastReport.Utils
Imports MySql.Data.MySqlClient
Imports OfficeOpenXml
Imports OfficeOpenXml.Style


Public Class Form1

    Private ConnectionString As String = ""
    Private bsAnagrafico As BindingSource

    ' Layout file e stato highlight
    Private Const GridLayoutFileName As String = "gridlayout.json"
    Private highlightEnabled As Boolean = True

    Private dataScadenzaCache As DateTime = DateTime.MinValue
    ' ✅ NUOVO: Flag per evitare di riconfigurare la griglia dopo il primo caricamento
    Private isGridConfigured As Boolean = False
    ' ✅ NUOVO: Flag per disabilitare il salvataggio automatico durante la configurazione
    Private isConfiguringGrid As Boolean = False


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ✅ Forza il cursore normale IMMEDIATAMENTE
        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default

        Try
            AppContext.SetSwitch("Switch.System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", True)
        Catch ex As Exception
        End Try

        ConnectionString = ReadConnectionString()
        dataScadenzaCache = GetDataScadenzaRiferimento()
        bsAnagrafico = New BindingSource()
        EnhanceGridLookAndFeel()

        ' ✅ Forza il refresh del form PRIMA di caricare i dati
        Me.Show()
        Me.Refresh()
        Application.DoEvents()

        LoadZones()
        LoadQualifiche()
        LoadSessoOptions()
        LoadData()
        AttachGridContextMenu()

        ' ✅ Forza il cursore normale ALLA FINE
        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
        Me.Refresh()
    End Sub

    ' --- Migliorie visuali e comportamentali per dgvAnagrafico ---
    Private Sub EnhanceGridLookAndFeel()
        dgvAnagrafico.AllowUserToAddRows = False
        dgvAnagrafico.AllowUserToDeleteRows = False
        dgvAnagrafico.AllowUserToOrderColumns = True
        dgvAnagrafico.AllowUserToResizeColumns = True
        dgvAnagrafico.ReadOnly = True
        dgvAnagrafico.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAnagrafico.MultiSelect = False
        dgvAnagrafico.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAnagrafico.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvAnagrafico.RowHeadersVisible = False

        ' Alternating row color
        dgvAnagrafico.EnableHeadersVisualStyles = False
        dgvAnagrafico.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDark
        dgvAnagrafico.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White
        dgvAnagrafico.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245)
        dgvAnagrafico.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue

        AddHandler dgvAnagrafico.KeyDown, AddressOf dgvAnagrafico_KeyDown
        AddHandler dgvAnagrafico.CellDoubleClick, AddressOf dgvAnagrafico_CellDoubleClick
        ' ✅ AGGIUNTO: Salva automaticamente quando l'utente ridimensiona una colonna
        AddHandler dgvAnagrafico.ColumnWidthChanged, AddressOf dgvAnagrafico_ColumnWidthChanged
    End Sub

    ' ✅ NUOVO METODO: Salvataggio automatico del layout
    Private Sub dgvAnagrafico_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs)
        ' ✅ NUOVO: Non salvare durante la configurazione iniziale
        If isConfiguringGrid Then Return
        ' Salva automaticamente dopo un breve ritardo per evitare salvataggi multipli
        Static lastSave As DateTime = DateTime.MinValue
        If (DateTime.Now - lastSave).TotalSeconds > 2 Then
            SaveGridLayout(Path.Combine(Application.StartupPath, GridLayoutFileName))
            lastSave = DateTime.Now
        End If

    End Sub

    ' Carica le zone nel ComboBox con codice e descrizione affiancati
    Private Sub LoadZones()
        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT Cod_zona, Desc_zona FROM zone ORDER BY Desc_zona"
                Using cmd As New MySqlCommand(sql, conn)
                    Dim da As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    ' Aggiungi una colonna calcolata per mostrare Cod_zona + Desc_zona
                    dt.Columns.Add("DisplayText", GetType(String), "Cod_zona + ' ' + Desc_zona")

                    If Me.Controls.ContainsKey("cmbZona") Then
                        Dim cmb = CType(Me.Controls("cmbZona"), ComboBox)
                        cmb.DisplayMember = "DisplayText"  ' Mostra la colonna calcolata
                        cmb.ValueMember = "Cod_zona"       ' Il valore rimane Cod_zona
                        cmb.DataSource = dt
                        cmb.SelectedIndex = -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore caricamento zone: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Carica le qualifiche nel ComboBox
    Private Sub LoadQualifiche()
        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT Cod_qualifica, Desc_qualifica FROM qualifica ORDER BY Desc_qualifica"
                Using cmd As New MySqlCommand(sql, conn)
                    Dim da As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    cmbQualifica.DisplayMember = "Desc_qualifica"
                    cmbQualifica.ValueMember = "Cod_qualifica"
                    cmbQualifica.DataSource = dt
                    cmbQualifica.SelectedIndex = -1
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore caricamento qualifiche: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Carica le opzioni di sesso nel ComboBox
    Private Sub LoadSessoOptions()
        cmbSesso.Items.Clear()
        cmbSesso.Items.Add("M")
        cmbSesso.Items.Add("F")
        cmbSesso.SelectedIndex = -1
    End Sub

    ' Carica i dati dal DB e li lega al BindingSource
    Private Sub LoadData(Optional filter As String = "")
        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                ' MODIFICATO: Aggiunto LEFT JOIN con zone per la ricerca, ma selezioniamo solo le colonne originali
                Dim sql As String = "SELECT a.id, a.ANA_Cognome, a.ANA_Nome, a.ANA_data_nascita, a.ANA_Sesso, a.ANA_Qualifica, " &
                               "a.ANA_indirizzo, a.ANA_civico, a.ANA_localita, a.ANA_Prov, a.ANA_Cap, a.ANA_Data_Iscrizione, a.ANA_Cellulare, " &
                               "a.ANA_Codice_Fiscale, a.ANA_Zona, a.ANA_Scad_tessera, a.ANA_Socio, a.ANA_Milite, " &
                               "a.ANA_Annullato, a.ANA_Assicurato " &
                               "FROM anagrafico a " &
                               "LEFT JOIN zone z ON a.ANA_Zona = z.Cod_zona"

                If Not String.IsNullOrWhiteSpace(filter) Then
                    ' MODIFICATO: Aggiunta ricerca su codice e descrizione zona
                    sql &= " WHERE a.ANA_Cognome LIKE @f " &
                       "OR a.ANA_Nome LIKE @f " &
                       "OR a.ANA_Codice_Fiscale LIKE @f " &
                       "OR a.ANA_Cellulare LIKE @f " &
                       "OR a.ANA_Qualifica LIKE @f " &
                       "OR z.Cod_zona LIKE @f " &
                       "OR z.Desc_zona LIKE @f"
                End If
                sql &= " ORDER BY a.id"

                Using cmd As New MySqlCommand(sql, conn)
                    If Not String.IsNullOrWhiteSpace(filter) Then
                        cmd.Parameters.AddWithValue("@f", $"%{filter}%")
                    End If
                    Dim da As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    bsAnagrafico.DataSource = dt
                    dgvAnagrafico.DataSource = bsAnagrafico
                End Using
            End Using

            ' ✅ MODIFICATO: Configura la griglia SOLO la prima volta
            If Not isGridConfigured Then
                ConfigureGrid()
                isGridConfigured = True
            End If
            RestoreGridLayout(Path.Combine(Application.StartupPath, GridLayoutFileName))
            ApplyRowHighlight()
            lblStatus.Text = $"Caricati record: {dgvAnagrafico.Rows.Count}"
        Catch ex As Exception
            MessageBox.Show($"Errore caricamento: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ValidateInput() As Boolean
        If String.IsNullOrWhiteSpace(txtCognome.Text) Then
            MessageBox.Show("Cognome è obbligatorio.", "Validazione", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtNome.Text) Then
            MessageBox.Show("Nome è obbligatorio.", "Validazione", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Function ToIntOrZero(value As Object) As Integer
        If value Is Nothing OrElse IsDBNull(value) Then Return 0
        Try
            Return Convert.ToInt32(value)
        Catch
            Dim s = Convert.ToString(value)
            Dim n As Integer
            If Integer.TryParse(s, n) Then Return n
            Return 0
        End Try
    End Function

    ' Configura intestazioni, formati date e colonne checkbox per i tinyint
    Private Sub ConfigureGrid()
        If dgvAnagrafico.DataSource Is Nothing Then Return
        ' ✅ NUOVO: Disabilita il salvataggio automatico durante la configurazione
        isConfiguringGrid = True
        Try
            For Each col As DataGridViewColumn In dgvAnagrafico.Columns
                col.SortMode = DataGridViewColumnSortMode.Automatic
                col.Resizable = DataGridViewTriState.True
            Next

            If dgvAnagrafico.Columns.Contains("id") Then dgvAnagrafico.Columns("id").HeaderText = "ID"
            If dgvAnagrafico.Columns.Contains("ANA_Cognome") Then dgvAnagrafico.Columns("ANA_Cognome").HeaderText = "Cognome"
            If dgvAnagrafico.Columns.Contains("ANA_Nome") Then dgvAnagrafico.Columns("ANA_Nome").HeaderText = "Nome"
            If dgvAnagrafico.Columns.Contains("ANA_data_nascita") Then
                dgvAnagrafico.Columns("ANA_data_nascita").HeaderText = "Data di nascita"
                dgvAnagrafico.Columns("ANA_data_nascita").DefaultCellStyle.Format = "dd/MM/yyyy"
            End If
            If dgvAnagrafico.Columns.Contains("ANA_Sesso") Then dgvAnagrafico.Columns("ANA_Sesso").HeaderText = "Sesso"
            If dgvAnagrafico.Columns.Contains("ANA_Qualifica") Then dgvAnagrafico.Columns("ANA_Qualifica").HeaderText = "Qualifica"
            If dgvAnagrafico.Columns.Contains("ANA_indirizzo") Then dgvAnagrafico.Columns("ANA_indirizzo").HeaderText = "Indirizzo"
            If dgvAnagrafico.Columns.Contains("ANA_civico") Then dgvAnagrafico.Columns("ANA_civico").HeaderText = "Civico"
            If dgvAnagrafico.Columns.Contains("ANA_localita") Then dgvAnagrafico.Columns("ANA_localita").HeaderText = "Località"
            If dgvAnagrafico.Columns.Contains("ANA_Prov") Then dgvAnagrafico.Columns("ANA_Prov").HeaderText = "Prov."
            If dgvAnagrafico.Columns.Contains("ANA_Cap") Then dgvAnagrafico.Columns("ANA_Cap").HeaderText = "CAP"
            If dgvAnagrafico.Columns.Contains("ANA_Codice_Fiscale") Then dgvAnagrafico.Columns("ANA_Codice_Fiscale").HeaderText = "Codice Fiscale"
            If dgvAnagrafico.Columns.Contains("ANA_Zona") Then dgvAnagrafico.Columns("ANA_Zona").HeaderText = "Zona"
            If dgvAnagrafico.Columns.Contains("ANA_Data_Iscrizione") Then
                dgvAnagrafico.Columns("ANA_Data_Iscrizione").HeaderText = "Data Iscrizione"
                dgvAnagrafico.Columns("ANA_Data_Iscrizione").DefaultCellStyle.Format = "dd/MM/yyyy"
            End If
            If dgvAnagrafico.Columns.Contains("ANA_Scad_tessera") Then
                dgvAnagrafico.Columns("ANA_Scad_tessera").HeaderText = "Scad.Tessera"
                dgvAnagrafico.Columns("ANA_Scad_tessera").DefaultCellStyle.Format = "dd/MM/yyyy"
            End If
            If dgvAnagrafico.Columns.Contains("ANA_Cellulare") Then dgvAnagrafico.Columns("ANA_Cellulare").HeaderText = "Cellulare"

            Dim tinyCols As New Dictionary(Of String, String) From {
                {"ANA_Socio", "Socio"},
                {"ANA_Milite", "Milite"},
                {"ANA_Annullato", "Annullato"},
                {"ANA_Assicurato", "Assicurato"}
            }

            For Each kvp In tinyCols
                Dim colName = kvp.Key
                Dim header = kvp.Value
                If dgvAnagrafico.Columns.Contains(colName) Then
                    Dim idx = dgvAnagrafico.Columns(colName).Index
                    dgvAnagrafico.Columns.RemoveAt(idx)
                    Dim chk As New DataGridViewCheckBoxColumn()
                    chk.Name = colName
                    chk.DataPropertyName = colName
                    chk.HeaderText = header
                    chk.TrueValue = 1
                    chk.FalseValue = 0
                    If colName = "ANA_Assicurato" Then
                        chk.ThreeState = True
                        chk.IndeterminateValue = DBNull.Value
                    Else
                        chk.ThreeState = False
                    End If
                    chk.SortMode = DataGridViewColumnSortMode.Automatic
                    dgvAnagrafico.Columns.Insert(idx, chk)
                End If
            Next

            ' Riordina le colonne: Sesso subito dopo Nome
            If dgvAnagrafico.Columns.Contains("ANA_Nome") AndAlso dgvAnagrafico.Columns.Contains("ANA_Sesso") Then
                Dim nomeIndex As Integer = dgvAnagrafico.Columns("ANA_Nome").DisplayIndex
                dgvAnagrafico.Columns("ANA_Sesso").DisplayIndex = nomeIndex + 1

                ' Sposta le altre colonne di conseguenza per evitare conflitti
                For Each col As DataGridViewColumn In dgvAnagrafico.Columns
                    If col.Name <> "ANA_Sesso" AndAlso col.Name <> "ANA_Nome" Then
                        If col.DisplayIndex > nomeIndex AndAlso col.DisplayIndex <= nomeIndex + 1 Then
                            col.DisplayIndex += 1
                        End If
                    End If
                Next
            End If

            Try
                'dgvAnagrafico.AutoResizeColumns() 'lascia le dimensioni dell'utente
                If dgvAnagrafico.Columns.Contains("ANA_Cognome") Then
                    bsAnagrafico.Sort = "ANA_Cognome ASC, ANA_Nome ASC"
                End If
            Catch
            End Try
        Catch ex As Exception
        Finally
            ' ✅ NUOVO: Riabilita il salvataggio automatico
            isConfiguringGrid = False
        End Try
    End Sub

    ' --- Event handlers e utility per la griglia ---
    Private Sub dgvAnagrafico_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.C Then
            If dgvAnagrafico.CurrentCell IsNot Nothing Then
                Clipboard.SetText(Convert.ToString(dgvAnagrafico.CurrentCell.Value))
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub dgvAnagrafico_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            btnUpdate.Focus()
        End If
    End Sub

    Private Sub ApplyRowHighlight()

        Try
            If Not highlightEnabled Then Return
            If dgvAnagrafico.Columns.Contains("ANA_Scad_tessera") Then
                ' Leggi la data di riferimento dalla tabella tes_scad
                Dim dataRiferimento As DateTime = GetDataScadenzaRiferimento()

                For Each row As DataGridViewRow In dgvAnagrafico.Rows
                    If row.IsNewRow Then Continue For
                    Dim v = row.Cells("ANA_Scad_tessera").Value
                    If v IsNot Nothing AndAlso Not IsDBNull(v) Then
                        Dim d As DateTime
                        If DateTime.TryParse(Convert.ToString(v), d) Then
                            ' MODIFICATO: Confronta con dataRiferimento invece di DateTime.Today
                            If d.Date < dataRiferimento.Date Then
                                row.DefaultCellStyle.BackColor = Color.LightCoral
                                row.DefaultCellStyle.ForeColor = Color.White
                            Else
                                row.DefaultCellStyle.BackColor = Color.White
                                row.DefaultCellStyle.ForeColor = Color.Black
                            End If
                        End If
                    Else
                        row.DefaultCellStyle.BackColor = Color.White
                        row.DefaultCellStyle.ForeColor = Color.Black
                    End If
                Next
            End If
        Catch
        End Try
    End Sub

    Private Sub ClearRowHighlight()
        Try
            For Each row As DataGridViewRow In dgvAnagrafico.Rows
                row.DefaultCellStyle.BackColor = Color.White
                row.DefaultCellStyle.ForeColor = Color.Black
            Next
        Catch
        End Try
    End Sub

    ' --- Context menu: copia cella o esporta CSV
    Private Sub AttachGridContextMenu()
        Dim cms = New ContextMenuStrip()
        Dim copyCell = New ToolStripMenuItem("Copia cella")
        Dim exportCsv = New ToolStripMenuItem("Esporta CSV...")
        AddHandler copyCell.Click, Sub(sender, e)
                                       If dgvAnagrafico.CurrentCell IsNot Nothing Then
                                           Clipboard.SetText(Convert.ToString(dgvAnagrafico.CurrentCell.Value))
                                       End If
                                   End Sub
        AddHandler exportCsv.Click, AddressOf ExportGridCsvDialog
        cms.Items.Add(copyCell)
        cms.Items.Add(exportCsv)
        dgvAnagrafico.ContextMenuStrip = cms
    End Sub

    Private Sub ExportGridCsvDialog(sender As Object, e As EventArgs)
        Using sfd As New SaveFileDialog()
            sfd.Filter = "CSV (*.csv)|*.csv|Tutti i file (*.*)|*.*"
            sfd.FileName = "anagrafico.csv"
            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    ExportGridToCsv(sfd.FileName)
                    MessageBox.Show("Esportazione completata.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show($"Errore esportazione: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub ExportGridToCsv(path As String)
        Dim sb As New StringBuilder()
        Dim cols = dgvAnagrafico.Columns.Cast(Of DataGridViewColumn)().Where(Function(c) c.Visible).OrderBy(Function(c) c.DisplayIndex)
        sb.AppendLine(String.Join(";", cols.Select(Function(c) $"""{c.HeaderText}""")))
        For Each row As DataGridViewRow In dgvAnagrafico.Rows
            If Not row.IsNewRow Then
                Dim values = cols.Select(Function(c)
                                             Dim v = row.Cells(c.Name).Value
                                             If v Is Nothing OrElse IsDBNull(v) Then Return """"
                                             Dim s = v.ToString().Replace("""", """""")
                                             Return $"""{s}"""
                                         End Function)
                sb.AppendLine(String.Join(";", values))
            End If
        Next
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8)
    End Sub

    ' --- Salvataggio / ripristino layout colonne ---
    Private Class ColumnLayout
        Public Property Name As String
        Public Property DisplayIndex As Integer
        Public Property Width As Integer
        Public Property Visible As Boolean
    End Class

    Private Sub SaveGridLayout(path As String)
        Try
            Dim cols = dgvAnagrafico.Columns.Cast(Of DataGridViewColumn)().Select(Function(c) New ColumnLayout() With {
                .Name = c.Name,
                .DisplayIndex = c.DisplayIndex,
                .Width = c.Width,
                .Visible = c.Visible
            }).ToList()
            Dim json = JsonSerializer.Serialize(cols, New JsonSerializerOptions() With {.WriteIndented = True})
            File.WriteAllText(path, json, Encoding.UTF8)
        Catch
        End Try
    End Sub

    Private Sub RestoreGridLayout(path As String)
        Try
            If Not File.Exists(path) Then Return
            Dim json = File.ReadAllText(path, Encoding.UTF8)
            Dim colsDef = JsonSerializer.Deserialize(Of List(Of ColumnLayout))(json)
            If colsDef Is Nothing Then Return

            For Each def In colsDef
                If dgvAnagrafico.Columns.Contains(def.Name) Then
                    Dim c = dgvAnagrafico.Columns(def.Name)
                    c.DisplayIndex = Math.Max(0, Math.Min(def.DisplayIndex, dgvAnagrafico.ColumnCount - 1))
                    c.Width = Math.Max(20, def.Width)
                    c.Visible = def.Visible
                End If
            Next
        Catch
        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveGridLayout(Path.Combine(Application.StartupPath, GridLayoutFileName))
    End Sub

    ' --- Gestori eventi per i pulsanti ---
    Private Sub btnExportCsv_Click(sender As Object, e As EventArgs) Handles btnExportCsv.Click
        ExportGridCsvDialog(sender, e)
    End Sub

    Private Sub btnSaveLayout_Click(sender As Object, e As EventArgs) Handles btnSaveLayout.Click
        SaveGridLayout(Path.Combine(Application.StartupPath, GridLayoutFileName))
        MessageBox.Show("Layout salvato.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnRestoreLayout_Click(sender As Object, e As EventArgs) Handles btnRestoreLayout.Click
        RestoreGridLayout(Path.Combine(Application.StartupPath, GridLayoutFileName))
        'ConfigureGrid()
        ApplyRowHighlight()
        MessageBox.Show("Layout ripristinato.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnToggleHighlight_Click(sender As Object, e As EventArgs) Handles btnToggleHighlight.Click
        highlightEnabled = Not highlightEnabled
        If highlightEnabled Then
            ApplyRowHighlight()
            btnToggleHighlight.Text = "Evid. scadute: ON"
        Else
            ClearRowHighlight()
            btnToggleHighlight.Text = "Evid. scadute: OFF"
        End If
    End Sub

    ' --- Gestori eventi CRUD ---
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not ValidateInput() Then Return

        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                ' Leggi la data di scadenza dalla tabella tes_scad
                Dim dataScadenzaTessera As DateTime = GetDataScadenzaRiferimento()
                Dim sql As String = "INSERT INTO anagrafico (ANA_Cognome, ANA_Nome, ANA_data_nascita, ANA_Sesso, ANA_Qualifica, ANA_indirizzo, ANA_civico, ANA_localita, ANA_Prov, ANA_Cap, ANA_Data_Iscrizione, ANA_Cellulare, ANA_Codice_Fiscale, ANA_Zona, ANA_Scad_tessera, ANA_Socio, ANA_Milite, ANA_Annullato, ANA_Assicurato) VALUES (@Cognome, @Nome, @DataNascita, @Sesso, @Qualifica, @Indirizzo, @Civico, @Localita, @Prov, @Cap, @DataIscrizione, @Cellulare, @CF, @Zona, @ScadTessera, @Socio, @Milite, @Annullato, @Assicurato)"
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Cognome", txtCognome.Text.Trim())
                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim())
                    cmd.Parameters.AddWithValue("@DataNascita", If(dtpDataNascita.Checked, CType(dtpDataNascita.Value, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@Sesso", If(cmbSesso.SelectedIndex = -1, DBNull.Value, cmbSesso.SelectedItem.ToString()))
                    cmd.Parameters.AddWithValue("@Qualifica", If(cmbQualifica.SelectedValue Is Nothing, DBNull.Value, cmbQualifica.SelectedValue))
                    cmd.Parameters.AddWithValue("@Indirizzo", If(String.IsNullOrWhiteSpace(txtIndirizzo.Text), DBNull.Value, txtIndirizzo.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Civico", If(String.IsNullOrWhiteSpace(txtCivico.Text), DBNull.Value, txtCivico.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Localita", If(String.IsNullOrWhiteSpace(txtLocalita.Text), DBNull.Value, txtLocalita.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Prov", If(String.IsNullOrWhiteSpace(txtProv.Text), DBNull.Value, txtProv.Text.Trim().ToUpper()))
                    cmd.Parameters.AddWithValue("@Cap", If(String.IsNullOrWhiteSpace(txtCap.Text), DBNull.Value, txtCap.Text.Trim()))
                    cmd.Parameters.AddWithValue("@DataIscrizione", If(dtpDataIscrizione.Checked, CType(dtpDataIscrizione.Value.Date, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@Cellulare", If(String.IsNullOrWhiteSpace(txtCellulare.Text), DBNull.Value, txtCellulare.Text.Trim()))
                    cmd.Parameters.AddWithValue("@CF", If(String.IsNullOrWhiteSpace(txtCF.Text), DBNull.Value, txtCF.Text.Trim().ToUpper()))
                    cmd.Parameters.AddWithValue("@Zona", If(cmbZona.SelectedValue Is Nothing, DBNull.Value, cmbZona.SelectedValue))
                    cmd.Parameters.AddWithValue("@ScadTessera", dtpScadTessera.Value.Date)
                    cmd.Parameters.AddWithValue("@Socio", If(chkSocio.Checked, 1, 0))
                    cmd.Parameters.AddWithValue("@Milite", If(chkMilite.Checked, 1, 0))
                    cmd.Parameters.AddWithValue("@Annullato", If(chkAnnullato.Checked, 1, 0))
                    cmd.Parameters.AddWithValue("@Assicurato", If(chkAssicurato.CheckState = CheckState.Indeterminate, DBNull.Value, If(chkAssicurato.Checked, CType(1, Object), CType(0, Object))))
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record aggiunto con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                    ClearFields()
                    ' Imposta la data di iscrizione a oggi per il prossimo inserimento
                    dtpDataIscrizione.Value = DateTime.Today
                    dtpDataIscrizione.Checked = True
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore inserimento: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If String.IsNullOrWhiteSpace(txtId.Text) Then
            MessageBox.Show("Seleziona un record dalla griglia.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not ValidateInput() Then Return

        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "UPDATE anagrafico SET ANA_Cognome=@Cognome, ANA_Nome=@Nome, ANA_data_nascita=@DataNascita, ANA_Sesso=@Sesso, ANA_Qualifica=@Qualifica, ANA_indirizzo=@Indirizzo, ANA_civico=@Civico, ANA_localita=@Localita, ANA_Prov=@Prov, ANA_Cap=@Cap, ANA_Data_Iscrizione=@DataIscrizione, ANA_Cellulare=@Cellulare, ANA_Codice_Fiscale=@CF, ANA_Zona=@Zona, ANA_Scad_tessera=@ScadTessera, ANA_Socio=@Socio, ANA_Milite=@Milite, ANA_Annullato=@Annullato, ANA_Assicurato=@Assicurato WHERE id=@Id"
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Id", Integer.Parse(txtId.Text))
                    cmd.Parameters.AddWithValue("@Cognome", txtCognome.Text.Trim())
                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim())
                    cmd.Parameters.AddWithValue("@DataNascita", If(dtpDataNascita.Checked, CType(dtpDataNascita.Value, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@Sesso", If(cmbSesso.SelectedIndex = -1, DBNull.Value, cmbSesso.SelectedItem.ToString()))
                    cmd.Parameters.AddWithValue("@Qualifica", If(cmbQualifica.SelectedValue Is Nothing, DBNull.Value, cmbQualifica.SelectedValue))
                    cmd.Parameters.AddWithValue("@Indirizzo", If(String.IsNullOrWhiteSpace(txtIndirizzo.Text), DBNull.Value, txtIndirizzo.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Civico", If(String.IsNullOrWhiteSpace(txtCivico.Text), DBNull.Value, txtCivico.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Localita", If(String.IsNullOrWhiteSpace(txtLocalita.Text), DBNull.Value, txtLocalita.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Prov", If(String.IsNullOrWhiteSpace(txtProv.Text), DBNull.Value, txtProv.Text.Trim().ToUpper()))
                    cmd.Parameters.AddWithValue("@Cap", If(String.IsNullOrWhiteSpace(txtCap.Text), DBNull.Value, txtCap.Text.Trim()))
                    cmd.Parameters.AddWithValue("@DataIscrizione", If(dtpDataIscrizione.Checked, CType(dtpDataIscrizione.Value.Date, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@Cellulare", If(String.IsNullOrWhiteSpace(txtCellulare.Text), DBNull.Value, txtCellulare.Text.Trim()))
                    cmd.Parameters.AddWithValue("@CF", If(String.IsNullOrWhiteSpace(txtCF.Text), DBNull.Value, txtCF.Text.Trim().ToUpper()))
                    cmd.Parameters.AddWithValue("@Zona", If(cmbZona.SelectedValue Is Nothing, DBNull.Value, cmbZona.SelectedValue))
                    cmd.Parameters.AddWithValue("@ScadTessera", dtpScadTessera.Value.Date)
                    cmd.Parameters.AddWithValue("@Socio", If(chkSocio.Checked, 1, 0))
                    cmd.Parameters.AddWithValue("@Milite", If(chkMilite.Checked, 1, 0))
                    cmd.Parameters.AddWithValue("@Annullato", If(chkAnnullato.Checked, 1, 0))
                    cmd.Parameters.AddWithValue("@Assicurato", If(chkAssicurato.CheckState = CheckState.Indeterminate, DBNull.Value, If(chkAssicurato.Checked, CType(1, Object), CType(0, Object))))
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record aggiornato con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                    ClearFields()
                    'RestoreGridLayout(Path.Combine(Application.StartupPath, GridLayoutFileName))
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore aggiornamento: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If String.IsNullOrWhiteSpace(txtId.Text) Then
            MessageBox.Show("Seleziona un record dalla griglia.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim result = MessageBox.Show($"Confermi l'eliminazione del record ID {txtId.Text}?", "Conferma eliminazione", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result <> DialogResult.Yes Then Return

        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                Using cmd As New MySqlCommand("DELETE FROM anagrafico WHERE id=@Id", conn)
                    cmd.Parameters.AddWithValue("@Id", Integer.Parse(txtId.Text))
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record eliminato con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()
                    ClearFields()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore eliminazione: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadData()
        ClearFields()
        lblStatus.Text = "Dati ricaricati."
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadData(txtSearch.Text.Trim())
    End Sub

    Private Sub ClearFields()
        txtId.Clear()
        txtCognome.Clear()
        txtNome.Clear()
        dtpDataNascita.Checked = False
        dtpDataNascita.Value = DateTime.Today
        cmbSesso.SelectedIndex = -1  ' MODIFICATO
        cmbQualifica.SelectedIndex = -1
        txtIndirizzo.Clear()
        txtCivico.Clear()
        txtLocalita.Clear()
        txtProv.Clear()
        txtCap.Clear()
        dtpDataIscrizione.Checked = False
        dtpDataIscrizione.Value = DateTime.Today
        txtCellulare.Clear()
        txtCF.Clear()
        cmbZona.SelectedIndex = -1
        dtpScadTessera.Value = GetDataScadenzaRiferimento()
        chkSocio.Checked = False
        chkMilite.Checked = False
        chkAnnullato.Checked = False
        chkAssicurato.CheckState = CheckState.Unchecked
        txtSearch.Clear()
    End Sub

    Private Sub dgvAnagrafico_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAnagrafico.SelectionChanged
        If dgvAnagrafico.CurrentRow Is Nothing OrElse dgvAnagrafico.CurrentRow.IsNewRow Then Return

        Try
            Dim row = dgvAnagrafico.CurrentRow
            txtId.Text = Convert.ToString(row.Cells("id").Value)
            txtCognome.Text = Convert.ToString(row.Cells("ANA_Cognome").Value)
            txtNome.Text = Convert.ToString(row.Cells("ANA_Nome").Value)

            Dim dataNascita = row.Cells("ANA_data_nascita").Value
            If dataNascita IsNot Nothing AndAlso Not IsDBNull(dataNascita) Then
                dtpDataNascita.Value = Convert.ToDateTime(dataNascita)
                dtpDataNascita.Checked = True
            Else
                dtpDataNascita.Checked = False
            End If

            ' MODIFICATO: Gestione ComboBox Sesso
            Dim sesso = Convert.ToString(row.Cells("ANA_Sesso").Value)
            If Not String.IsNullOrWhiteSpace(sesso) Then
                Dim sessoUpper = sesso.ToUpper()
                If sessoUpper = "M" OrElse sessoUpper = "F" Then
                    cmbSesso.SelectedItem = sessoUpper
                Else
                    cmbSesso.SelectedIndex = -1
                End If
            Else
                cmbSesso.SelectedIndex = -1
            End If

            ' MODIFICATO: Gestione ComboBox Qualifica
            Dim qualifica = row.Cells("ANA_Qualifica").Value
            If qualifica IsNot Nothing AndAlso Not IsDBNull(qualifica) Then
                cmbQualifica.SelectedValue = qualifica
            Else
                cmbQualifica.SelectedIndex = -1
            End If

            txtIndirizzo.Text = Convert.ToString(row.Cells("ANA_indirizzo").Value)
            txtCivico.Text = Convert.ToString(row.Cells("ANA_civico").Value)
            txtLocalita.Text = Convert.ToString(row.Cells("ANA_localita").Value)
            txtProv.Text = Convert.ToString(row.Cells("ANA_Prov").Value)
            txtCap.Text = Convert.ToString(row.Cells("ANA_Cap").Value)

            Dim dataIscrizione = row.Cells("ANA_Data_Iscrizione").Value
            If dataIscrizione IsNot Nothing AndAlso Not IsDBNull(dataIscrizione) Then
                dtpDataIscrizione.Value = Convert.ToDateTime(dataIscrizione)
                dtpDataIscrizione.Checked = True
            Else
                dtpDataIscrizione.Checked = False
            End If

            txtCellulare.Text = Convert.ToString(row.Cells("ANA_Cellulare").Value)
            txtCF.Text = Convert.ToString(row.Cells("ANA_Codice_Fiscale").Value)

            Dim zona = row.Cells("ANA_Zona").Value
            If zona IsNot Nothing AndAlso Not IsDBNull(zona) Then
                cmbZona.SelectedValue = zona
            Else
                cmbZona.SelectedIndex = -1
            End If

            Dim scadTessera = row.Cells("ANA_Scad_tessera").Value
            If scadTessera IsNot Nothing AndAlso Not IsDBNull(scadTessera) Then
                dtpScadTessera.Value = Convert.ToDateTime(scadTessera)
            Else
                dtpScadTessera.Value = DateTime.Today
            End If

            chkSocio.Checked = (ToIntOrZero(row.Cells("ANA_Socio").Value) = 1)
            chkMilite.Checked = (ToIntOrZero(row.Cells("ANA_Milite").Value) = 1)
            chkAnnullato.Checked = (ToIntOrZero(row.Cells("ANA_Annullato").Value) = 1)

            Dim assic = row.Cells("ANA_Assicurato").Value
            If assic Is Nothing OrElse IsDBNull(assic) Then
                chkAssicurato.CheckState = CheckState.Indeterminate
            Else
                chkAssicurato.Checked = (ToIntOrZero(assic) = 1)
            End If
        Catch ex As Exception
        End Try
    End Sub

    ' --- Funzioni utility ---
    Private Function ReadConnectionString() As String
        Dim configPath = Path.Combine(Application.StartupPath, "dbconfig.json")

        Try
            If File.Exists(configPath) Then
                Dim json = File.ReadAllText(configPath)
                Dim cfg = JsonSerializer.Deserialize(Of DbConfig)(json)
                If cfg IsNot Nothing Then
                    Return $"Server={cfg.Server};Database={cfg.Database};Uid={cfg.User};Pwd={cfg.Password};"
                Else
                    ' File esiste ma è vuoto o malformato
                    MessageBox.Show("Il file dbconfig.json esiste ma non contiene dati validi." & Environment.NewLine &
                                   "Verrà usata la connessione di fallback.",
                                   "Configurazione DB",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Warning)
                End If
            Else
                ' AGGIUNTO: File non esiste
                MessageBox.Show("Il file dbconfig.json non è stato trovato." & Environment.NewLine &
                               "Verrà usata la connessione di fallback." & Environment.NewLine & Environment.NewLine &
                               "Percorso cercato: " & configPath,
                               "Configurazione DB",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            ' Errore durante la lettura/parsing
            MessageBox.Show($"Errore durante la lettura di dbconfig.json:" & Environment.NewLine & Environment.NewLine &
                           $"{ex.Message}" & Environment.NewLine & Environment.NewLine &
                           "Verrà usata la connessione di fallback.",
                           "Configurazione DB",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
        End Try

        ' Fallback in tutti i casi
        Return "Server=localhost;Database=db_01;Uid=root;Pwd=Mauro1963?;"
    End Function

    Private Class DbConfig
        Public Property Server As String
        Public Property Database As String
        Public Property User As String
        Public Property Password As String
    End Class



    Private Function CaricaDatiFiltrati(zona As String, qualifica As String, soloSoci As Boolean, soloMiliti As Boolean) As DataTable
        Dim dt As New DataTable("Anagrafico")

        ' Definisci le colonne
        dt.Columns.Add("id", GetType(Integer))
        dt.Columns.Add("Cognome", GetType(String))
        dt.Columns.Add("Nome", GetType(String))
        dt.Columns.Add("DataNascita", GetType(String))
        dt.Columns.Add("Sesso", GetType(String))
        dt.Columns.Add("Qualifica", GetType(String))
        dt.Columns.Add("Indirizzo", GetType(String))
        dt.Columns.Add("Civico", GetType(String))
        dt.Columns.Add("Localita", GetType(String))
        dt.Columns.Add("Provincia", GetType(String))
        dt.Columns.Add("CAP", GetType(String))
        dt.Columns.Add("DataIscrizione", GetType(String))
        dt.Columns.Add("Cellulare", GetType(String))
        dt.Columns.Add("CodiceFiscale", GetType(String))
        dt.Columns.Add("Zona", GetType(String))
        dt.Columns.Add("ScadenzaTessera", GetType(String))
        dt.Columns.Add("Socio", GetType(String))
        dt.Columns.Add("Milite", GetType(String))
        dt.Columns.Add("Annullato", GetType(String))

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()

            ' Query SQL con filtri dinamici
            Dim sql As String = "SELECT id, ANA_Cognome, ANA_Nome, ANA_data_nascita, ANA_Sesso, " +
                       "ANA_Qualifica, ANA_indirizzo, ANA_civico, ANA_localita, ANA_Prov, " +
                       "ANA_Cap, ANA_Data_Iscrizione, ANA_Cellulare, ANA_Codice_Fiscale, " +
                       "ANA_Zona, ANA_Scad_tessera, ANA_Socio, ANA_Milite, ANA_Annullato " +
                       "FROM anagrafico WHERE 1=1"

            ' Aggiungi filtri solo se i parametri non sono vuoti
            If Not String.IsNullOrWhiteSpace(zona) Then
                sql &= " AND TRIM(ANA_Zona) = @Zona"
            End If

            If Not String.IsNullOrWhiteSpace(qualifica) Then
                sql &= " AND ANA_Qualifica = @Qualifica"
            End If

            If soloSoci Then
                sql &= " AND ANA_Socio = 1"
            End If

            If soloMiliti Then
                sql &= " AND ANA_Milite = 1"
            End If

            sql &= " ORDER BY ANA_Cognome, ANA_Nome"

            Using cmd As New MySqlCommand(sql, conn)
                ' Aggiungi i parametri alla query
                If Not String.IsNullOrWhiteSpace(zona) Then
                    cmd.Parameters.AddWithValue("@Zona", zona)
                End If

                If Not String.IsNullOrWhiteSpace(qualifica) Then
                    cmd.Parameters.AddWithValue("@Qualifica", qualifica)
                End If

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim row As DataRow = dt.NewRow()
                        row("id") = reader("id")
                        row("Cognome") = If(IsDBNull(reader("ANA_Cognome")), "", reader("ANA_Cognome"))
                        row("Nome") = If(IsDBNull(reader("ANA_Nome")), "", reader("ANA_Nome"))
                        row("DataNascita") = If(IsDBNull(reader("ANA_data_nascita")), "", CDate(reader("ANA_data_nascita")).ToString("dd/MM/yyyy"))
                        row("Sesso") = If(IsDBNull(reader("ANA_Sesso")), "", reader("ANA_Sesso"))
                        row("Qualifica") = If(IsDBNull(reader("ANA_Qualifica")), "", reader("ANA_Qualifica"))
                        row("Indirizzo") = If(IsDBNull(reader("ANA_indirizzo")), "", reader("ANA_indirizzo"))
                        row("Civico") = If(IsDBNull(reader("ANA_civico")), "", reader("ANA_civico"))
                        row("Localita") = If(IsDBNull(reader("ANA_localita")), "", reader("ANA_localita"))
                        row("Provincia") = If(IsDBNull(reader("ANA_Prov")), "", reader("ANA_Prov"))
                        row("CAP") = If(IsDBNull(reader("ANA_Cap")), "", reader("ANA_Cap"))
                        row("DataIscrizione") = If(IsDBNull(reader("ANA_Data_Iscrizione")), "", CDate(reader("ANA_Data_Iscrizione")).ToString("dd/MM/yyyy"))
                        row("Cellulare") = If(IsDBNull(reader("ANA_Cellulare")), "", reader("ANA_Cellulare"))
                        row("CodiceFiscale") = If(IsDBNull(reader("ANA_Codice_Fiscale")), "", reader("ANA_Codice_Fiscale"))
                        row("Zona") = If(IsDBNull(reader("ANA_Zona")), "", reader("ANA_Zona"))
                        row("ScadenzaTessera") = If(IsDBNull(reader("ANA_Scad_tessera")), "", CDate(reader("ANA_Scad_tessera")).ToString("dd/MM/yyyy"))
                        row("Socio") = If(ToIntOrZero(reader("ANA_Socio")) = 1, "Sì", "No")
                        row("Milite") = If(ToIntOrZero(reader("ANA_Milite")) = 1, "Sì", "No")
                        row("Annullato") = If(ToIntOrZero(reader("ANA_Annullato")) = 1, "Sì", "No")
                        dt.Rows.Add(row)
                    End While
                End Using
            End Using
        End Using

        Return dt
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using frmFiltri As New FormFiltriReport(ConnectionString)
                If frmFiltri.ShowDialog() = DialogResult.OK Then

                    ' Carica i dati filtrati
                    Dim dtFiltrato As DataTable = CaricaDatiFiltrati(
                    frmFiltri.ZonaSelezionata,
                    frmFiltri.QualificaSelezionata,
                    frmFiltri.SoloSoci,
                    frmFiltri.SoloMiliti)

                    ' Verifica che ci siano dati
                    If dtFiltrato.Rows.Count = 0 Then
                        MessageBox.Show("Nessun dato trovato con i filtri specificati.",
                              "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                    ' ✅ USA LA CLASSE HELPER PER CARICARE IL REPORT
                    Dim report As Report = FastReportHelper.CaricaReport(
                    "Stampa2.frx",
                    dtFiltrato,
                    "Anagrafico")

                    ' Imposta i parametri del report
                    FastReportHelper.ImpostaParametro(report, "Para1", $"Zona: {frmFiltri.ZonaDescrizione}")
                    FastReportHelper.ImpostaParametro(report, "Para2", $"Qualifica: {frmFiltri.QualificaDescrizione}")

                    ' Mostra il report
                    FastReportHelper.MostraReport(report)

                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore durante la generazione del report:{vbCrLf}{vbCrLf}{ex.Message}",
                   "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Legge la data di scadenza di riferimento dalla tabella tes_scad
    Private Function GetDataScadenzaRiferimento() As DateTime

        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT data_scad_tes FROM tes_scad LIMIT 1"
                Using cmd As New MySqlCommand(sql, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        Return Convert.ToDateTime(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore lettura data scadenza: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        ' Fallback alla data odierna in caso di errore
        Return DateTime.Today
    End Function

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        ExportGridExcelDialogEPPlus(sender, e)
    End Sub

    Private Sub ExportGridExcelDialogEPPlus(sender As Object, e As EventArgs)
        Using sfd As New SaveFileDialog()
            sfd.Filter = "Excel (*.xlsx)|*.xlsx|Tutti i file (*.*)|*.*"
            sfd.FileName = "anagrafico.xlsx"
            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    ExportGridToExcelEPPlus(sfd.FileName)
                    MessageBox.Show("Esportazione Excel completata.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show($"Errore esportazione Excel: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub ExportGridToExcelEPPlus(path As String)

        Using package As New ExcelPackage()
            Dim worksheet = package.Workbook.Worksheets.Add("Anagrafico")

            ' Ottieni colonne visibili ordinate
            Dim cols = dgvAnagrafico.Columns.Cast(Of DataGridViewColumn)() _
            .Where(Function(c) c.Visible) _
            .OrderBy(Function(c) c.DisplayIndex) _
            .ToList()

            ' Header Row - con formattazione
            For colIndex As Integer = 0 To cols.Count - 1
                Dim cell = worksheet.Cells(1, colIndex + 1)
                cell.Value = cols(colIndex).HeaderText
                cell.Style.Font.Bold = True
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightGray)
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
            Next

            ' Data Rows
            Dim rowIndex As Integer = 2
            For Each row As DataGridViewRow In dgvAnagrafico.Rows
                If Not row.IsNewRow Then
                    For colIndex As Integer = 0 To cols.Count - 1
                        Dim col = cols(colIndex)
                        Dim v = row.Cells(col.Name).Value
                        Dim cell = worksheet.Cells(rowIndex, colIndex + 1)

                        If v IsNot Nothing AndAlso Not IsDBNull(v) Then
                            If TypeOf v Is DateTime Then
                                cell.Value = CDate(v)
                                cell.Style.Numberformat.Format = "dd/MM/yyyy"
                            ElseIf TypeOf v Is Integer Then
                                Dim intVal As Integer = CInt(v)
                                If intVal = 0 OrElse intVal = 1 Then
                                    cell.Value = If(intVal = 1, "Sì", "No")
                                Else
                                    cell.Value = v
                                End If
                            ElseIf TypeOf v Is Boolean Then
                                cell.Value = If(CBool(v), "Sì", "No")
                            Else
                                cell.Value = v
                            End If
                        End If
                    Next
                    rowIndex += 1
                End If
            Next

            worksheet.Cells(worksheet.Dimension.Address).AutoFitColumns()

            Dim fileInfo As New FileInfo(path)
            package.SaveAs(fileInfo)
        End Using
    End Sub

End Class