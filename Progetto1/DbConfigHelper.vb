Imports System.IO
Imports System.Text.Json

''' <summary>
''' Classe helper per la gestione centralizzata della configurazione del database
''' </summary>
Public Class DbConfigHelper

    ''' <summary>
    ''' Legge la connection string dal file dbconfig.json
    ''' </summary>
    ''' <returns>Connection string MySQL</returns>
    Public Shared Function ReadConnectionString() As String
        Dim configPath = Path.Combine(Application.StartupPath, "dbconfig.json")

        Try
            If File.Exists(configPath) Then
                Dim json = File.ReadAllText(configPath)
                Dim cfg = JsonSerializer.Deserialize(Of DbConfig)(json)
                If cfg IsNot Nothing Then
                    Return $"Server={cfg.Server};Database={cfg.Database};Uid={cfg.User};Pwd={cfg.Password};"
                Else
                    ' File esiste ma è vuoto o malformato
                    System.Diagnostics.Debug.WriteLine("Il file dbconfig.json esiste ma non contiene dati validi.")
                End If
            Else
                ' File non esiste
                System.Diagnostics.Debug.WriteLine("Il file dbconfig.json non è stato trovato. Percorso: " & configPath)
            End If
        Catch ex As Exception
            ' Errore durante la lettura/parsing
            System.Diagnostics.Debug.WriteLine($"Errore durante la lettura di dbconfig.json: {ex.Message}")
        End Try

        ' Fallback in tutti i casi
        Return "Server=localhost;Database=db_01;Uid=root;Pwd=Mauro1963?;"
    End Function

    ''' <summary>
    ''' Classe interna per la deserializzazione del file JSON
    ''' </summary>
    Private Class DbConfig
        Public Property Server As String
        Public Property Database As String
        Public Property User As String
        Public Property Password As String
    End Class

End Class