Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Json

Public Class FormMain
    Inherits Form

    Private connectionString As String
    Private sidebar As Panel
    Private contentPanel As Panel
    Private activeForm As Form
    Private isSidebarVisible As Boolean = True
    Private btnToggleSidebar As Button

    Public Sub New()

        MyBase.New()
        Try
            ' Debug
            System.Diagnostics.Debug.WriteLine("FormMain.New() - Inizio")

            ' Inizializza il form base
            Me.Text = "Gestione Croce Verde"
            Me.Size = New Size(1280, 720)
            Me.MinimumSize = New Size(1024, 600)
            Me.StartPosition = FormStartPosition.CenterScreen

            ' Carica la connessione
            connectionString = ReadConnectionString()

            System.Diagnostics.Debug.WriteLine("FormMain.New() - Fine")
        Catch ex As Exception
            MessageBox.Show($"Errore in FormMain.New(): {ex.Message}{vbCrLf}{ex.StackTrace}",
                      "Errore Costruttore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            System.Diagnostics.Debug.WriteLine("FormMain_Load - Inizio")

            ' Verifica UserSession
            If Not UserSession.IsAuthenticated() Then
                MessageBox.Show("Sessione utente non valida!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

            Me.WindowState = FormWindowState.Maximized
            Me.Text = $"Gestione Croce Verde - {UserSession.Current.Nome} {UserSession.Current.Cognome} ({UserSession.Current.Ruolo})"

            System.Diagnostics.Debug.WriteLine("FormMain_Load - Inizializzazione sidebar")
            InitializeSidebar()

            System.Diagnostics.Debug.WriteLine("FormMain_Load - Inizializzazione content panel")
            InitializeContentPanel()

            System.Diagnostics.Debug.WriteLine("FormMain_Load - Inizializzazione toggle button")
            InitializeToggleButton()

            System.Diagnostics.Debug.WriteLine("FormMain_Load - Show welcome screen")
            ShowWelcomeScreen()

            ' Nascondi la sidebar all'avvio (opzionale)
            ToggleSidebar()

            System.Diagnostics.Debug.WriteLine("FormMain_Load - Fine")
        Catch ex As Exception
            MessageBox.Show($"Errore in FormMain_Load: {ex.Message}{vbCrLf}{ex.StackTrace}",
                          "Errore Caricamento", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitializeSidebar()
        Try
            sidebar = New Panel() With {
                .Dock = DockStyle.Left,
                .Width = 220,
                .BackColor = Color.FromArgb(45, 45, 48)
            }
            Me.Controls.Add(sidebar)

            ' Header
            Dim header As New Panel() With {
                .Dock = DockStyle.Top,
                .Height = 60,
                .BackColor = Color.FromArgb(30, 30, 32)
            }

            Dim lblAppName As New Label() With {
                .Text = "Menu Generale",
                .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                .ForeColor = Color.White,
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleCenter
            }
            header.Controls.Add(lblAppName)
            sidebar.Controls.Add(header)

            ' User Info
            Dim userPanel As New Panel() With {
                .Dock = DockStyle.Top,
                .Height = 50,
                .BackColor = Color.FromArgb(37, 37, 38)
            }

            Dim lblUser As New Label() With {
                .Text = $"{UserSession.Current.Nome} {UserSession.Current.Cognome}" & vbCrLf & $"({UserSession.Current.Ruolo})",
                .Font = New Font("Segoe UI", 9),
                .ForeColor = Color.LightGray,
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleCenter
            }
            userPanel.Controls.Add(lblUser)
            sidebar.Controls.Add(userPanel)
            userPanel.BringToFront()

            ' Menu Items con FlowLayoutPanel (layout adattivo)
            Dim menuFlow As New FlowLayoutPanel() With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.TopDown,
                .AutoScroll = True,
                .WrapContents = False,
                .Padding = New Padding(10, 130, 10, 10) '130 aumenta padding top per distanziare dal userPanel
            }
            sidebar.Controls.Add(menuFlow)

            ' ANAGRAFICA
            AddMenuCategory(menuFlow, "GESTIONE")
            AddMenuItem(menuFlow, "👥 Anagrafico Soci", AddressOf MenuAnagrafico_Click)
            AddMenuItem(menuFlow, "🗺️ Gestione Zone", AddressOf MenuZone_Click)
            AddMenuItem(menuFlow, "🎓 Gestione Qualifiche", AddressOf MenuQualifiche_Click)
            AddSeparator(menuFlow)

            ' GESTIONE
            AddMenuCategory(menuFlow, "GESTIONE")
            AddMenuItem(menuFlow, "🎫 Tessere", AddressOf MenuTessere_Click)
            AddMenuItem(menuFlow, "🏥 Assicurazioni", AddressOf MenuAssicurazioni_Click)
            AddSeparator(menuFlow)

            ' REPORT
            AddMenuCategory(menuFlow, "REPORT")
            AddMenuItem(menuFlow, "📊 Stampe e Report", AddressOf MenuReport_Click)
            AddSeparator(menuFlow)

            ' AMMINISTRAZIONE (solo admin)
            If UserSession.IsAdmin() Then
                AddMenuCategory(menuFlow, "AMMINISTRAZIONE")
                AddMenuItem(menuFlow, "👤 Gestione Utenti", AddressOf MenuUtenti_Click)
                AddMenuItem(menuFlow, "⚙️ Impostazioni DB", AddressOf MenuImpostazioni_Click)
                AddSeparator(menuFlow)
            End If

            ' LOGOUT (sempre in fondo)
            Dim btnLogout As New Button() With {
                .Text = "🚪 Esci",
                .Font = New Font("Segoe UI", 10),
                .ForeColor = Color.White,
                .BackColor = Color.FromArgb(192, 0, 0),
                .FlatStyle = FlatStyle.Flat,
                .Width = 200,
                .Height = 40,
                .Margin = New Padding(0, 10, 0, 0),
                .Cursor = Cursors.Hand
            }
            btnLogout.FlatAppearance.BorderSize = 0
            AddHandler btnLogout.Click, AddressOf btnLogout_Click
            menuFlow.Controls.Add(btnLogout)

        Catch ex As Exception
            MessageBox.Show($"Errore InitializeSidebar: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub

    Private Sub InitializeToggleButton()
        ' Crea il pulsante hamburger per toggle della sidebar
        btnToggleSidebar = New Button() With {
            .Text = "☰",
            .Font = New Font("Segoe UI", 20, FontStyle.Bold),
            .Size = New Size(50, 50),
            .Location = New Point(10, 10),
            .BackColor = Color.FromArgb(0, 122, 204),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand,
            .TabStop = False
        }
        btnToggleSidebar.FlatAppearance.BorderSize = 0
        btnToggleSidebar.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 100, 180)

        AddHandler btnToggleSidebar.Click, AddressOf btnToggleSidebar_Click

        ' Aggiungi il pulsante sopra tutti gli altri controlli
        Me.Controls.Add(btnToggleSidebar)
        btnToggleSidebar.BringToFront()
    End Sub

    Private Sub btnToggleSidebar_Click(sender As Object, e As EventArgs)
        ToggleSidebar()
    End Sub



    Private Sub ToggleSidebar()
        isSidebarVisible = Not isSidebarVisible
        sidebar.Visible = isSidebarVisible

        ' Aggiorna posizione pulsante
        If isSidebarVisible Then
            btnToggleSidebar.Location = New Point(sidebar.Width + 10, 10)
        Else
            btnToggleSidebar.Location = New Point(10, 10)
        End If
    End Sub

    Private Sub AddMenuCategory(parent As FlowLayoutPanel, text As String)
        Dim lbl As New Label() With {
            .Text = text,
            .Font = New Font("Segoe UI", 9, FontStyle.Bold),
            .ForeColor = Color.Gray,
            .Width = 200,
            .Height = 25,  '25
            .Margin = New Padding(0, 5, 0, 5),
            .Padding = New Padding(5, 0, 0, 0)
        }
        parent.Controls.Add(lbl)
    End Sub

    Private Sub AddMenuItem(parent As FlowLayoutPanel, text As String, handler As EventHandler)
        Dim btn As New Button() With {
            .Text = text,
            .Font = New Font("Segoe UI", 10),
            .ForeColor = Color.White,
            .BackColor = Color.FromArgb(45, 45, 48),
            .FlatStyle = FlatStyle.Flat,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Width = 200,
            .Height = 40,
            .Margin = New Padding(0, 2, 0, 2),
            .Cursor = Cursors.Hand
        }

        btn.FlatAppearance.BorderSize = 0
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 64)
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 122, 204)

        AddHandler btn.Click, handler
        parent.Controls.Add(btn)
    End Sub

    Private Sub AddSeparator(parent As FlowLayoutPanel)
        Dim sep As New Panel() With {
            .Height = 1,
            .Width = 200,
            .Margin = New Padding(0, 8, 0, 8),
            .BackColor = Color.FromArgb(62, 62, 64)
        }
        parent.Controls.Add(sep)
    End Sub

    Private Sub InitializeContentPanel()
        Try
            contentPanel = New Panel() With {
                .Dock = DockStyle.Fill,
                .BackColor = Color.White
            }
            Me.Controls.Add(contentPanel)
        Catch ex As Exception
            MessageBox.Show($"Errore InitializeContentPanel: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub

    Private Sub ShowFormInPanel(form As Form)
        Try
            ' Chiudi il form precedente
            If activeForm IsNot Nothing Then
                activeForm.Close()
                activeForm.Dispose()
            End If

            contentPanel.Controls.Clear()

            form.TopLevel = False
            form.FormBorderStyle = FormBorderStyle.None
            form.Dock = DockStyle.Fill
            contentPanel.Controls.Add(form)
            form.Show()

            activeForm = form
        Catch ex As Exception
            MessageBox.Show($"Errore ShowFormInPanel: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ShowWelcomeScreen()
        Try
            Dim welcomeLabel As New Label() With {
                .Text = $"Benvenuto, {UserSession.Current.Nome}!" & vbCrLf & vbCrLf &
                       "Seleziona una voce dal menu per iniziare." & vbCrLf &
                       "Usa il pulsante ☰ per aprire/chiudere il menu.",
                .Font = New Font("Segoe UI", 16),
                .TextAlign = ContentAlignment.MiddleCenter,
                .Dock = DockStyle.Fill,
                .ForeColor = Color.Gray
            }
            contentPanel.Controls.Add(welcomeLabel)
        Catch ex As Exception
            MessageBox.Show($"Errore ShowWelcomeScreen: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Event Handlers Menu
    Private Sub MenuAnagrafico_Click(sender As Object, e As EventArgs)
        Try
            Dim frm As New Form1()
            ShowFormInPanel(frm)
        Catch ex As Exception
            MessageBox.Show($"Errore apertura Anagrafico: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MenuZone_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Funzione Zone in sviluppo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MenuQualifiche_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Funzione Qualifiche in sviluppo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MenuTessere_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Funzione Tessere in sviluppo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MenuAssicurazioni_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Funzione Assicurazioni in sviluppo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MenuReport_Click(sender As Object, e As EventArgs)
        Try
            ' ✅ MODIFICATO: Apri FormFiltriReport invece di Form1
            Using frmFiltri As New FormFiltriReport(connectionString)
                frmFiltri.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show($"Errore apertura Report: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MenuUtenti_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Funzione Gestione Utenti in sviluppo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MenuImpostazioni_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Funzione Impostazioni in sviluppo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs)
        Dim result = MessageBox.Show("Vuoi disconnetterti?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            UserSession.Logout()
            Me.Close()
        End If
    End Sub

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
            System.Diagnostics.Debug.WriteLine($"Errore ReadConnectionString: {ex.Message}")
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