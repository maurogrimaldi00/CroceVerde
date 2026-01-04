Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Json
Imports MySql.Data.MySqlClient

Public Class FormLogin
    Private connectionString As String

    Public Sub New()
        InitializeComponent()
        connectionString = ReadConnectionString()
    End Sub

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.AcceptButton = btnLogin
        Me.CancelButton = btnCancel
        txtPassword.UseSystemPasswordChar = True
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            MessageBox.Show("Inserire username.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsername.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Inserire password.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPassword.Focus()
            Return
        End If

        If AuthenticateUser(txtUsername.Text.Trim(), txtPassword.Text) Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Credenziali non valide.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtPassword.Clear()
            txtPassword.Focus()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function AuthenticateUser(username As String, password As String) As Boolean
        Try
            Dim passwordHash As String = UserSession.HashPassword(password)

            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim sql As String = "SELECT id, username, nome, cognome, ruolo FROM utenti " &
                                   "WHERE username = @username AND password_hash = @password AND attivo = 1"

                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@username", username)
                    cmd.Parameters.AddWithValue("@password", passwordHash)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ' Login riuscito - memorizza sessione
                            UserSession.Login(
                                reader.GetInt32("id"),
                                reader.GetString("username"),
                                If(IsDBNull(reader("nome")), "", reader.GetString("nome")),
                                If(IsDBNull(reader("cognome")), "", reader.GetString("cognome")),
                                reader.GetString("ruolo")
                            )
                            reader.Close()

                            ' Aggiorna ultimo accesso
                            Dim updateSql As String = "UPDATE utenti SET ultimo_accesso = NOW() WHERE id = @id"
                            Using updateCmd As New MySqlCommand(updateSql, conn)
                                updateCmd.Parameters.AddWithValue("@id", UserSession.Current.UserId)
                                updateCmd.ExecuteNonQuery()
                            End Using

                            Return True
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore autenticazione: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return False
    End Function

    Private Function ReadConnectionString() As String
        Dim configPath = Path.Combine(Application.StartupPath, "dbconfig.json")

        Try
            If File.Exists(configPath) Then
                Dim json = File.ReadAllText(configPath)
                Dim cfg = JsonSerializer.Deserialize(Of DbConfig)(json)
                If cfg IsNot Nothing Then
                    Return $"Server={cfg.Server};Database={cfg.Database};Uid={cfg.User};Pwd={cfg.Password};"
                End If
            End If
        Catch ex As Exception
            MessageBox.Show($"Errore lettura configurazione: {ex.Message}", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        Return "Server=localhost;Database=db_01;Uid=root;Pwd=Mauro1963?;"
    End Function

    Private Class DbConfig
        Public Property Server As String
        Public Property Database As String
        Public Property User As String
        Public Property Password As String
    End Class
End Class