Imports System.IO
Imports Microsoft.Reporting.WinForms

Partial Public Class FormReport
    ' Chiamare questo metodo per caricare il report da un DataTable
    Public Sub ShowAnagrafico(dt As DataTable)
        If dt Is Nothing Then
            Throw New ArgumentNullException(NameOf(dt))
        End If

        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ' Percorso del file .rdlc (adatta al tuo layout)
        ReportViewer1.LocalReport.ReportPath = Path.Combine(Application.StartupPath, "Reports", "Anagrafico.rdlc")

        ReportViewer1.LocalReport.DataSources.Clear()
        Dim rds As New ReportDataSource("AnagraficoDataSet", dt) ' nome deve corrispondere al DataSet nel report
        ReportViewer1.LocalReport.DataSources.Add(rds)

        ReportViewer1.RefreshReport()
    End Sub

    ' Esempio: esporta il report corrente in PDF su disco
    Public Sub ExportToPdf(targetPath As String)
        Dim bytes() As Byte = ReportViewer1.LocalReport.Render("PDF")
        File.WriteAllBytes(targetPath, bytes)
    End Sub
End Class