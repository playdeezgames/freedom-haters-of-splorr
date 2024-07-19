Imports FHOS.Model
Imports FHOS.Presentation
Imports Spectre.Console
Imports SPLORR.Presentation
Imports SPLORR.Presentation.Spectre

Module Program
    Sub Main(args As String())
        Try
            Console.Title = "Freedom Haters of SPLORR!!"
            Console.CursorVisible = False
        Catch ex As PlatformNotSupportedException
            'nom!
        End Try
        Dim image = New CanvasImage("SIGMO.png")
        AnsiConsole.Write(image)
        Dim ui = New UIContext
        ui.WriteFiglet((Mood.Title, "SIGMO"))
        ui.Message((Mood.Prompt, String.Empty))
        MainMenuState.Start(New UniverseModel, ui)
    End Sub
End Module
