Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.ComponentModel

Public Class FormFiltriReport
    Private _connectionString As String

    ' Proprietà pubbliche per leggere i filtri selezionati
    ' AGGIUNTO: Attributo per evitare la serializzazione nel designer
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ZonaSelezionata As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property QualificaSelezionata As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ZonaDescrizione As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property QualificaDescrizione As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SoloSoci As Boolean

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SoloMiliti As Boolean

    Public Sub New(connectionString As String)
        InitializeComponent()
        _connectionString = connectionString
    End Sub

    Private Sub FormFiltriReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CaricaZone()
        CaricaQualifiche()
    End Sub

    Private Sub CaricaZone()
        Try
            Using conn As New MySqlConnection(_connectionString)
                conn.Open()
                Dim sql As String = "SELECT Cod_zona, Desc_zona FROM zone ORDER BY Desc_zona"
                Using cmd As New MySqlCommand(sql, conn)
                    Dim da As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    ' Aggiungi riga "Tutte"
                    Dim rowAll As DataRow = dt.NewRow()
                    rowAll("Cod_zona") = DBNull.Value
                    rowAll("Desc_zona") = "-- Tutte le zone --"
                    dt.Rows.InsertAt(rowAll, 0)

                    ' Aggiungi colonna calcolata
                    dt.Columns.Add("DisplayText", GetType(String))
                    For Each row As DataRow In dt.Rows
                        If IsDBNull(row("Cod_zona")) Then
                            row("DisplayText") = row("Desc_zona").ToString()
                        Else
                            row("DisplayText") = row("Cod_zona").ToString() & " " & row("Desc_zona").ToString()
                        End If
                    Next

                    cmbZonaFiltro.DisplayMember = "DisplayText"
                    cmbZonaFiltro.ValueMember = "Cod_zona"
                    cmbZonaFiltro.DataSource = dt
                    cmbZonaFiltro.SelectedIndex = 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore caricamento zone: {ex.Message}", "Errore",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CaricaQualifiche()
        Try
            Using conn As New MySqlConnection(_connectionString)
                conn.Open()
                Dim sql As String = "SELECT Cod_qualifica, Desc_qualifica FROM qualifica ORDER BY Desc_qualifica"
                Using cmd As New MySqlCommand(sql, conn)
                    Dim da As New MySqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)

                    ' Aggiungi riga "Tutte"
                    Dim rowAll As DataRow = dt.NewRow()
                    rowAll("Cod_qualifica") = DBNull.Value
                    rowAll("Desc_qualifica") = "-- Tutte le qualifiche --"
                    dt.Rows.InsertAt(rowAll, 0)

                    cmbQualificaFiltro.DisplayMember = "Desc_qualifica"
                    cmbQualificaFiltro.ValueMember = "Cod_qualifica"
                    cmbQualificaFiltro.DataSource = dt
                    cmbQualificaFiltro.SelectedIndex = 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore caricamento qualifiche: {ex.Message}", "Errore",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        ' Salva i valori selezionati
        ' ? CORRETTO: Gestisce correttamente DBNull.Value
        If cmbZonaFiltro.SelectedValue Is Nothing OrElse IsDBNull(cmbZonaFiltro.SelectedValue) Then
            ZonaSelezionata = ""
        Else
            ZonaSelezionata = cmbZonaFiltro.SelectedValue.ToString()
        End If

        If cmbQualificaFiltro.SelectedValue Is Nothing OrElse IsDBNull(cmbQualificaFiltro.SelectedValue) Then
            QualificaSelezionata = ""
        Else
            QualificaSelezionata = cmbQualificaFiltro.SelectedValue.ToString()
        End If

        ' Salva anche le descrizioni per mostrarle nel report
        ZonaDescrizione = If(String.IsNullOrEmpty(ZonaSelezionata), "Tutte", cmbZonaFiltro.Text)
        QualificaDescrizione = If(String.IsNullOrEmpty(QualificaSelezionata), "Tutte", cmbQualificaFiltro.Text)

        SoloSoci = chkSoloSoci.Checked
        SoloMiliti = chkSoloMiliti.Checked

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnAnnulla_Click(sender As Object, e As EventArgs) Handles btnAnnulla.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class