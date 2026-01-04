Imports System.Security.Cryptography
Imports System.Text

Public Class UserSession
    ' Singleton per mantenere la sessione corrente
    Private Shared _currentUser As UserSession

    Public Property UserId As Integer
    Public Property Username As String
    Public Property Nome As String
    Public Property Cognome As String
    Public Property Ruolo As String
    Public Property LoginTime As DateTime

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Current As UserSession
        Get
            If _currentUser Is Nothing Then
                _currentUser = New UserSession()
            End If
            Return _currentUser
        End Get
    End Property

    Public Shared Sub Login(userId As Integer, username As String, nome As String, cognome As String, ruolo As String)
        Current.UserId = userId
        Current.Username = username
        Current.Nome = nome
        Current.Cognome = cognome
        Current.Ruolo = ruolo
        Current.LoginTime = DateTime.Now
    End Sub

    Public Shared Sub Logout()
        _currentUser = Nothing
    End Sub

    Public Shared Function IsAuthenticated() As Boolean
        Return Current.UserId > 0
    End Function

    Public Shared Function IsAdmin() As Boolean
        Return Current.Ruolo = "admin"
    End Function

    Public Shared Function CanEdit() As Boolean
        Return Current.Ruolo = "admin" OrElse Current.Ruolo = "operatore"
    End Function

    Public Shared Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Dim builder As New StringBuilder()
            For Each b In bytes
                builder.Append(b.ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function
End Class