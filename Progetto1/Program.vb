Imports System.Windows.Forms
Imports System.IO

Module Program
    Private logFile As String = Path.Combine(Application.StartupPath, "debug.log")

    <STAThread()>
    Sub Main()
        Try
            ' Pulisci log precedente
            If File.Exists(logFile) Then File.Delete(logFile)

            Log("=== AVVIO APPLICAZIONE ===")

            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)

            Log("Apertura FormLogin...")

            ' Mostra il login
            Using loginForm As New FormLogin()
                Dim loginResult = loginForm.ShowDialog()
                Log($"Login DialogResult: {loginResult}")

                If loginResult = DialogResult.OK Then
                    Log("Login OK - Verifica sessione...")

                    ' Verifica sessione
                    If UserSession.IsAuthenticated() Then
                        Log($"Utente autenticato: {UserSession.Current.Username}")
                        Log("Creazione FormMain...")

                        Dim mainForm As New FormMain()
                        Log("FormMain creato - Avvio Application.Run...")

                        Application.Run(mainForm)

                        Log("FormMain chiuso")
                    Else
                        Log("ERRORE: Sessione non valida!")
                        MessageBox.Show("Errore: Sessione non valida!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    Log($"Login annullato o fallito: {loginResult}")
                End If
            End Using

            ' Logout alla chiusura
            UserSession.Logout()
            Log("=== FINE APPLICAZIONE ===")

            ' Mostra log alla fine
            MessageBox.Show($"Log salvato in:{vbCrLf}{logFile}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            Log($"ERRORE CRITICO: {ex.Message}{Environment.NewLine}{ex.StackTrace}")
            MessageBox.Show($"ERRORE CRITICO:{vbCrLf}{vbCrLf}{ex.Message}{vbCrLf}{vbCrLf}{ex.StackTrace}",
                          "Errore Applicazione", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Log(message As String)
        Try
            File.AppendAllText(logFile, $"[{DateTime.Now:HH:mm:ss.fff}] {message}{Environment.NewLine}")
        Catch
            ' Ignora errori di log
        End Try
    End Sub
End Module