Imports FastReport
Imports FastReport.Data
Imports FastReport.Utils
Imports System.Data

''' <summary>
''' Classe helper per semplificare la gestione dei report FastReport
''' </summary>
Public Class FastReportHelper

    ''' <summary>
    ''' Carica e configura un report FastReport con un DataTable
    ''' </summary>
    ''' <param name="templatePath">Percorso del file .frx</param>
    ''' <param name="dataTable">DataTable con i dati da visualizzare</param>
    ''' <param name="dataSourceName">Nome del datasource nel report (es. "Anagrafico")</param>
    ''' <returns>Report configurato pronto per essere mostrato</returns>
    Public Shared Function CaricaReport(templatePath As String,
                                        dataTable As DataTable,
                                        dataSourceName As String) As Report
        Dim report As New Report()

        Try
            ' Carica il template
            report.Load(templatePath)

            ' Disabilita tutti i datasource esistenti
            For Each ds As DataSourceBase In report.Dictionary.DataSources
                ds.Enabled = False
            Next

            ' Cancella eventuali connessioni esistenti
            report.Dictionary.Connections.Clear()

            ' Registra il nuovo DataTable
            report.RegisterData(dataTable, dataSourceName)

            ' Abilita il datasource appena registrato
            Dim registeredDataSource = report.GetDataSource(dataSourceName)
            If registeredDataSource IsNot Nothing Then
                registeredDataSource.Enabled = True
            Else
                Throw New Exception($"DataSource '{dataSourceName}' non trovato nel report.")
            End If

            Return report

        Catch ex As Exception
            Throw New Exception($"Errore durante il caricamento del report: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' Configura e mostra il report in anteprima
    ''' </summary>
    ''' <param name="report">Report da mostrare</param>
    ''' <param name="mostraPulsantiExport">Se True, mostra pulsanti per esportazione PDF/Excel</param>
    Public Shared Sub MostraReport(report As Report, Optional mostraPulsantiExport As Boolean = True)
        Try
            ' Configura i pulsanti dell'anteprima
            If mostraPulsantiExport Then
                Config.PreviewSettings.Buttons = PreviewButtons.Print Or
                                                 PreviewButtons.Save Or
                                                 PreviewButtons.Zoom Or
                                                 PreviewButtons.Email Or
                                                 PreviewButtons.Find
            Else
                Config.PreviewSettings.Buttons = PreviewButtons.Print Or
                                                 PreviewButtons.Zoom Or
                                                 PreviewButtons.Find
            End If

            ' Prepara il report (elabora i dati)
            report.Prepare()

            ' Mostra l'anteprima
            report.Show()

        Catch ex As Exception
            Throw New Exception($"Errore durante la visualizzazione del report: {ex.Message}", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Imposta un parametro nel report
    ''' </summary>
    ''' <param name="report">Report in cui impostare il parametro</param>
    ''' <param name="nomeParametro">Nome del parametro (es. "Para1")</param>
    ''' <param name="valore">Valore da assegnare</param>
    Public Shared Sub ImpostaParametro(report As Report, nomeParametro As String, valore As String)
        Try
            report.SetParameterValue(nomeParametro, valore)
        Catch ex As Exception
            Throw New Exception($"Errore durante l'impostazione del parametro '{nomeParametro}': {ex.Message}", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Verifica se un file template esiste
    ''' </summary>
    ''' <param name="templatePath">Percorso del file .frx</param>
    ''' <returns>True se il file esiste</returns>
    Public Shared Function VerificaTemplate(templatePath As String) As Boolean
        Return IO.File.Exists(templatePath)
    End Function

End Class