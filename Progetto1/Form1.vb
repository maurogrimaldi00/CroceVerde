Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Text.Json
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing
Imports Microsoft.Reporting.WinForms
Imports System.Drawing.Printing

Public Class Form1

    Private ConnectionString As String = ""
    Private bsAnagrafico As BindingSource

    ' Layout file e stato highlight
    Private Const GridLayoutFileName As String = "gridlayout.json"
    Private highlightEnabled As Boolean = True

    ' Variabili per stampa
    Private printDoc As PrintDocument
    Private printDataTable As DataTable
    Private currentPrintRow As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Abilita BinaryFormatter per ReportViewer (necessario per .NET recente)
        Try
            AppContext.SetSwitch("Switch.System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", True)
        Catch ex As Exception
            ' Ignora se già impostato
        End Try

        ' Legge la stringa di connessione da file esterno
        ConnectionString = ReadConnectionString()

        ' BindingSource per abilitare sort/filter e separare il DataTable dal DataGridView
        bsAnagrafico = New BindingSource()

        ' Imposta comportamenti della griglia (generici)
        EnhanceGridLookAndFeel()

        LoadZones()
        LoadData()
        AttachGridContextMenu()
    End Sub

    ' --- Migliorie visuali e comportamentali per dgvAnagrafico ---
    Private Sub EnhanceGridLookAndFeel()
        dgvAnagrafico.AllowUserToAddRows = False
        dgvAnagrafico.AllowUserToDeleteRows = False
        dgvAnagrafico.AllowUserToOrderColumns = True
        dgvAnagrafico.ReadOnly = True
        dgvAnagrafico.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAnagrafico.MultiSelect = False
        dgvAnagrafico.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        dgvAnagrafico.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        dgvAnagrafico.RowHeadersVisible = False

        ' Alternating row color
        dgvAnagrafico.EnableHeadersVisualStyles = False
        dgvAnagrafico.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDark
        dgvAnagrafico.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White
        dgvAnagrafico.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245)
        dgvAnagrafico.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue

        AddHandler dgvAnagrafico.KeyDown, AddressOf dgvAnagrafico_KeyDown
        AddHandler dgvAnagrafico.CellDoubleClick, AddressOf dgvAnagrafico_CellDoubleClick
    End Sub

    ' Carica le zone nel ComboBox (se presente nel Designer)
    Private Sub LoadZones()
        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT Cod_zona, Desc_zona FROM zone ORDER BY Desc_zona"
                Using cmd As New MySqlCommand(sql, conn)
                    Dim da As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    If Me.Controls.ContainsKey("cmbZona") Then
                        Dim cmb = CType(Me.Controls("cmbZona"), ComboBox)
                        cmb.DisplayMember = "Desc_zona"
                        cmb.ValueMember = "Cod_zona"
                        cmb.DataSource = dt
                        cmb.SelectedIndex = -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore caricamento zone: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Carica i dati dal DB e li lega al BindingSource
    Private Sub LoadData(Optional filter As String = "")
        Try
            Using conn As New MySqlConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT id, ANA_Cognome, ANA_Nome, ANA_data_nascita, ANA_Sesso, ANA_Qualifica, ANA_indirizzo, ANA_civico, ANA_localita, ANA_Prov, ANA_Cap, ANA_Cellulare, ANA_Codice_Fiscale, ANA_Zona, ANA_Scad_tessera, ANA_Socio, ANA_Milite, ANA_Annullato, ANA_Assicurato FROM anagrafico"
                If Not String.IsNullOrWhiteSpace(filter) Then
                    sql &= " WHERE ANA_Cognome LIKE @f OR ANA_Nome LIKE @f OR ANA_Codice_Fiscale LIKE @f OR ANA_Cellulare LIKE @f OR ANA_Qualifica LIKE @f"
                End If
                sql &= " ORDER BY id"
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

            ConfigureGrid()
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
        Try
            For Each col As DataGridViewColumn In dgvAnagrafico.Columns
                col.SortMode = DataGridViewColumnSortMode.Automatic
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
            If dgvAnagrafico.Columns.Contains("ANA_Cellulare") Then dgvAnagrafico.Columns("ANA_Cellulare").HeaderText = "Cellulare"
            If dgvAnagrafico.Columns.Contains("ANA_Codice_Fiscale") Then dgvAnagrafico.Columns("ANA_Codice_Fiscale").HeaderText = "Codice Fiscale"
            If dgvAnagrafico.Columns.Contains("ANA_Zona") Then dgvAnagrafico.Columns("ANA_Zona").HeaderText = "Zona"
            If dgvAnagrafico.Columns.Contains("ANA_Scad_tessera") Then
                dgvAnagrafico.Columns("ANA_Scad_tessera").HeaderText = "Scadenza tessera"
                dgvAnagrafico.Columns("ANA_Scad_tessera").DefaultCellStyle.Format = "dd/MM/yyyy"
            End If

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

            Try
                dgvAnagrafico.AutoResizeColumns()
                If dgvAnagrafico.Columns.Contains("ANA_Cognome") Then
                    bsAnagrafico.Sort = "ANA_Cognome ASC, ANA_Nome ASC"
                End If
            Catch
            End Try
        Catch ex As Exception
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
                For Each row As DataGridViewRow In dgvAnagrafico.Rows
                    If row.IsNewRow Then Continue For
                    Dim v = row.Cells("ANA_Scad_tessera").Value
                    If v IsNot Nothing AndAlso Not IsDBNull(v) Then
                        Dim d As DateTime
                        If DateTime.TryParse(Convert.ToString(v), d) Then
                            If d.Date < DateTime.Today Then
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
        ConfigureGrid()
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
                Dim sql As String = "INSERT INTO anagrafico (ANA_Cognome, ANA_Nome, ANA_data_nascita, ANA_Sesso, ANA_Qualifica, ANA_indirizzo, ANA_civico, ANA_localita, ANA_Prov, ANA_Cap, ANA_Cellulare, ANA_Codice_Fiscale, ANA_Zona, ANA_Scad_tessera, ANA_Socio, ANA_Milite, ANA_Annullato, ANA_Assicurato) VALUES (@Cognome, @Nome, @DataNascita, @Sesso, @Qualifica, @Indirizzo, @Civico, @Localita, @Prov, @Cap, @Cellulare, @CF, @Zona, @ScadTessera, @Socio, @Milite, @Annullato, @Assicurato)"
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Cognome", txtCognome.Text.Trim())
                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim())
                    cmd.Parameters.AddWithValue("@DataNascita", If(dtpDataNascita.Checked, CType(dtpDataNascita.Value, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@Sesso", If(String.IsNullOrWhiteSpace(txtSesso.Text), DBNull.Value, txtSesso.Text.Trim().ToUpper()))
                    cmd.Parameters.AddWithValue("@Qualifica", If(String.IsNullOrWhiteSpace(txtQualifica.Text), DBNull.Value, txtQualifica.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Indirizzo", If(String.IsNullOrWhiteSpace(txtIndirizzo.Text), DBNull.Value, txtIndirizzo.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Civico", If(String.IsNullOrWhiteSpace(txtCivico.Text), DBNull.Value, txtCivico.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Localita", If(String.IsNullOrWhiteSpace(txtLocalita.Text), DBNull.Value, txtLocalita.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Prov", If(String.IsNullOrWhiteSpace(txtProv.Text), DBNull.Value, txtProv.Text.Trim().ToUpper()))
                    cmd.Parameters.AddWithValue("@Cap", If(String.IsNullOrWhiteSpace(txtCap.Text), DBNull.Value, txtCap.Text.Trim()))
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
                Dim sql As String = "UPDATE anagrafico SET ANA_Cognome=@Cognome, ANA_Nome=@Nome, ANA_data_nascita=@DataNascita, ANA_Sesso=@Sesso, ANA_Qualifica=@Qualifica, ANA_indirizzo=@Indirizzo, ANA_civico=@Civico, ANA_localita=@Localita, ANA_Prov=@Prov, ANA_Cap=@Cap, ANA_Cellulare=@Cellulare, ANA_Codice_Fiscale=@CF, ANA_Zona=@Zona, ANA_Scad_tessera=@ScadTessera, ANA_Socio=@Socio, ANA_Milite=@Milite, ANA_Annullato=@Annullato, ANA_Assicurato=@Assicurato WHERE id=@Id"
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Id", Integer.Parse(txtId.Text))
                    cmd.Parameters.AddWithValue("@Cognome", txtCognome.Text.Trim())
                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim())
                    cmd.Parameters.AddWithValue("@DataNascita", If(dtpDataNascita.Checked, CType(dtpDataNascita.Value, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@Sesso", If(String.IsNullOrWhiteSpace(txtSesso.Text), DBNull.Value, txtSesso.Text.Trim().ToUpper()))
                    cmd.Parameters.AddWithValue("@Qualifica", If(String.IsNullOrWhiteSpace(txtQualifica.Text), DBNull.Value, txtQualifica.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Indirizzo", If(String.IsNullOrWhiteSpace(txtIndirizzo.Text), DBNull.Value, txtIndirizzo.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Civico", If(String.IsNullOrWhiteSpace(txtCivico.Text), DBNull.Value, txtCivico.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Localita", If(String.IsNullOrWhiteSpace(txtLocalita.Text), DBNull.Value, txtLocalita.Text.Trim()))
                    cmd.Parameters.AddWithValue("@Prov", If(String.IsNullOrWhiteSpace(txtProv.Text), DBNull.Value, txtProv.Text.Trim().ToUpper()))
                    cmd.Parameters.AddWithValue("@Cap", If(String.IsNullOrWhiteSpace(txtCap.Text), DBNull.Value, txtCap.Text.Trim()))
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
        txtSesso.Clear()
        txtQualifica.Clear()
        txtIndirizzo.Clear()
        txtCivico.Clear()
        txtLocalita.Clear()
        txtProv.Clear()
        txtCap.Clear()
        txtCellulare.Clear()
        txtCF.Clear()
        cmbZona.SelectedIndex = -1
        dtpScadTessera.Value = DateTime.Today
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

            txtSesso.Text = Convert.ToString(row.Cells("ANA_Sesso").Value)
            txtQualifica.Text = Convert.ToString(row.Cells("ANA_Qualifica").Value)
            txtIndirizzo.Text = Convert.ToString(row.Cells("ANA_indirizzo").Value)
            txtCivico.Text = Convert.ToString(row.Cells("ANA_civico").Value)
            txtLocalita.Text = Convert.ToString(row.Cells("ANA_localita").Value)
            txtProv.Text = Convert.ToString(row.Cells("ANA_Prov").Value)
            txtCap.Text = Convert.ToString(row.Cells("ANA_Cap").Value)
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
        Try
            Dim configPath = Path.Combine(Application.StartupPath, "dbconfig.json")
            If File.Exists(configPath) Then
                Dim json = File.ReadAllText(configPath)
                Dim cfg = JsonSerializer.Deserialize(Of DbConfig)(json)
                If cfg IsNot Nothing Then
                    Return $"Server={cfg.Server};Database={cfg.Database};Uid={cfg.User};Pwd={cfg.Password};"
                End If
            End If
        Catch ex As Exception
            MessageBox.Show($"Attenzione: impossibile leggere dbconfig.json, verrà usata la connessione di fallback.{Environment.NewLine}{ex.Message}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        Return "Server=localhost;Database=db_01;Uid=root;Pwd=Mauro1963?;"
    End Function

    Private Class DbConfig
        Public Property Server As String
        Public Property Database As String
        Public Property User As String
        Public Property Password As String
    End Class

    Private Function CreaDtAnagrafico() As DataTable
        Dim dt As New DataTable("dtAnagrafico")
        dt.Columns.Add("id", GetType(Integer))
        dt.Columns.Add("ANA_Cognome", GetType(String))
        dt.Columns.Add("ANA_Nome", GetType(String))
        dt.Columns.Add("ANA_data_nascita", GetType(DateTime))
        dt.Columns.Add("ANA_Sesso", GetType(String))
        dt.Columns.Add("ANA_Qualifica", GetType(String))
        dt.Columns.Add("ANA_indirizzo", GetType(String))
        dt.Columns.Add("ANA_civico", GetType(String))
        dt.Columns.Add("ANA_localita", GetType(String))
        dt.Columns.Add("ANA_Prov", GetType(String))
        dt.Columns.Add("ANA_Cap", GetType(String))
        dt.Columns.Add("ANA_Cellulare", GetType(String))
        dt.Columns.Add("ANA_Codice_Fiscale", GetType(String))
        dt.Columns.Add("ANA_Zona", GetType(String))
        dt.Columns.Add("ANA_Scad_tessera", GetType(DateTime))
        dt.Columns.Add("ANA_Socio", GetType(Boolean))
        dt.Columns.Add("ANA_Milite", GetType(Boolean))
        dt.Columns.Add("ANA_Annullato", GetType(Boolean))
        dt.Columns.Add("ANA_Assicurato", GetType(Boolean))
        Return dt
    End Function

    ' --- Stampa Report ---
    Private Sub StampaReport()
        Try
            printDataTable = CaricaDatiPerReport()
            If printDataTable.Rows.Count = 0 Then
                MessageBox.Show("Nessun dato da stampare.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            printDoc = New PrintDocument()
            AddHandler printDoc.PrintPage, AddressOf PrintDocument_PrintPage

            Dim pageSetup As New PageSettings()
            pageSetup.Landscape = True
            pageSetup.Margins = New Margins(40, 40, 50, 50)
            printDoc.DefaultPageSettings = pageSetup

            Dim printPreview As New PrintPreviewDialog()
            printPreview.Document = printDoc
            printPreview.Size = New Size(1200, 800)
            printPreview.StartPosition = FormStartPosition.CenterScreen
            printPreview.ShowIcon = False
            printPreview.Text = "Anteprima Report Anagrafico"

            currentPrintRow = 0
            printPreview.ShowDialog()
        Catch ex As Exception
            MessageBox.Show($"Errore durante la generazione del report: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        Try
            Dim yPos As Single = e.MarginBounds.Top
            Dim xPos As Single = e.MarginBounds.Left
            Dim titleFont As New Font("Arial", 18, FontStyle.Bold)
            Dim subtitleFont As New Font("Arial", 10, FontStyle.Italic)
            Dim headerFont As New Font("Arial", 8, FontStyle.Bold)
            Dim dataFont As New Font("Arial", 7)
            Dim lineHeight As Single = dataFont.GetHeight(e.Graphics)

            If currentPrintRow = 0 Then
                e.Graphics.DrawString("REPORT ANAGRAFICO", titleFont, Brushes.Black, e.MarginBounds.Left, yPos)
                yPos += titleFont.GetHeight(e.Graphics) * 1.3
                e.Graphics.DrawString($"Data stampa: {DateTime.Now:dd/MM/yyyy HH:mm} | Totale record: {printDataTable.Rows.Count}", subtitleFont, Brushes.Gray, e.MarginBounds.Left, yPos)
                yPos += subtitleFont.GetHeight(e.Graphics) * 2
            End If

            Dim colWidths As New Dictionary(Of String, Single) From {
                {"id", 35}, {"Cognome", 90}, {"Nome", 80}, {"DataNascita", 70},
                {"Localita", 100}, {"Provincia", 30}, {"Cellulare", 85},
                {"CodiceFiscale", 105}, {"ScadenzaTessera", 75},
                {"Socio", 35}, {"Milite", 35}, {"Annullato", 45}
            }

            Dim colHeaders As New Dictionary(Of String, String) From {
                {"id", "ID"}, {"Cognome", "Cognome"}, {"Nome", "Nome"},
                {"DataNascita", "Nascita"}, {"Localita", "Località"},
                {"Provincia", "Pr"}, {"Cellulare", "Cellulare"},
                {"CodiceFiscale", "Codice Fiscale"}, {"ScadenzaTessera", "Scad.Tessera"},
                {"Socio", "Socio"}, {"Milite", "Milite"}, {"Annullato", "Ann."}
            }

            Dim currentX As Single = xPos
            Dim headerHeight As Single = lineHeight * 1.8
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(70, 70, 70)), xPos, yPos, e.MarginBounds.Width, headerHeight)

            For Each col In colWidths
                Dim headerRect As New RectangleF(currentX + 3, yPos + 3, col.Value - 6, headerHeight - 6)
                Dim sf As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center, .Trimming = StringTrimming.EllipsisCharacter}
                e.Graphics.DrawString(colHeaders(col.Key), headerFont, Brushes.White, headerRect, sf)
                currentX += col.Value
            Next
            yPos += headerHeight + 2

            Dim rowHeight As Single = lineHeight * 1.4
            While currentPrintRow < printDataTable.Rows.Count AndAlso yPos < e.MarginBounds.Bottom - rowHeight - 20
                Dim row As DataRow = printDataTable.Rows(currentPrintRow)
                currentX = xPos

                If currentPrintRow Mod 2 = 0 Then
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(248, 248, 248)), xPos, yPos, e.MarginBounds.Width, rowHeight)
                End If

                Dim isScaduto As Boolean = False
                If Not String.IsNullOrEmpty(row("ScadenzaTessera").ToString()) Then
                    Dim dataScad As DateTime
                    If DateTime.TryParseExact(row("ScadenzaTessera").ToString(), "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, dataScad) Then
                        If dataScad.Date < DateTime.Today Then
                            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(255, 200, 200)), xPos, yPos, e.MarginBounds.Width, rowHeight)
                            isScaduto = True
                        End If
                    End If
                End If

                For Each col In colWidths
                    Dim cellValue As String = If(row(col.Key) IsNot Nothing, row(col.Key).ToString(), "")
                    Dim cellRect As New RectangleF(currentX + 3, yPos + 2, col.Value - 6, rowHeight - 4)
                    Dim sf As New StringFormat() With {.Trimming = StringTrimming.EllipsisCharacter, .LineAlignment = StringAlignment.Center}
                    Dim brush As Brush = If(isScaduto, Brushes.DarkRed, Brushes.Black)
                    e.Graphics.DrawString(cellValue, dataFont, brush, cellRect, sf)
                    currentX += col.Value
                Next

                e.Graphics.DrawLine(New Pen(Color.FromArgb(230, 230, 230), 0.5), xPos, yPos + rowHeight, xPos + e.MarginBounds.Width, yPos + rowHeight)
                yPos += rowHeight
                currentPrintRow += 1
            End While

            Dim pageNumber As Integer = (currentPrintRow \ 40) + 1
            Dim footerText As String = $"Pagina {pageNumber} | Record visualizzati: {currentPrintRow} di {printDataTable.Rows.Count}"
            e.Graphics.DrawString(footerText, New Font("Arial", 8), Brushes.Gray, e.MarginBounds.Left, e.MarginBounds.Bottom + 5)
            e.Graphics.DrawLine(New Pen(Color.Gray, 1), e.MarginBounds.Left, e.MarginBounds.Bottom, e.MarginBounds.Right, e.MarginBounds.Bottom)
            e.HasMorePages = (currentPrintRow < printDataTable.Rows.Count)
        Catch ex As Exception
            MessageBox.Show($"Errore durante la stampa: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.HasMorePages = False
        End Try
    End Sub

    Private Function CaricaDatiPerReport() As DataTable
        Dim dt As New DataTable("Anagrafico")
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
        dt.Columns.Add("Cellulare", GetType(String))
        dt.Columns.Add("CodiceFiscale", GetType(String))
        dt.Columns.Add("Zona", GetType(String))
        dt.Columns.Add("ScadenzaTessera", GetType(String))
        dt.Columns.Add("Socio", GetType(String))
        dt.Columns.Add("Milite", GetType(String))
        dt.Columns.Add("Annullato", GetType(String))

        Using conn As New MySqlConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "SELECT id, ANA_Cognome, ANA_Nome, ANA_data_nascita, ANA_Sesso, ANA_Qualifica, ANA_indirizzo, ANA_civico, ANA_localita, ANA_Prov, ANA_Cap, ANA_Cellulare, ANA_Codice_Fiscale, ANA_Zona, ANA_Scad_tessera, ANA_Socio, ANA_Milite, ANA_Annullato FROM anagrafico ORDER BY ANA_Cognome, ANA_Nome"
            Using cmd As New MySqlCommand(sql, conn)
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

    Private Sub btnStampa_Click(sender As Object, e As EventArgs) Handles btnStampa.Click
        StampaReport()
    End Sub
End Class