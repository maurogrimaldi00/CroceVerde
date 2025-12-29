Imports System

Module Program

    <STAThread>
    Sub Main()
        ' Deve essere chiamato PRIMA che qualsiasi libreria usi BinaryFormatter
        AppContext.SetSwitch("Switch.System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", True)

        System.Windows.Forms.Application.EnableVisualStyles()
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(False)
        System.Windows.Forms.Application.Run(New Form1())
    End Sub

End Module