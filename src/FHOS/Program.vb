Imports FHOS.Model
Imports FHOS.Presentation
Imports SPLORR.Presentation.Spectre

Module Program
    Sub Main(args As String())
        Try
            Console.Title = "Freedom Haters of SPLORR!!"
            Console.CursorVisible = False
        Catch ex As PlatformNotSupportedException
            'nom!
        End Try
        MainMenuState.Start(New UniverseModel, New UIContext)
    End Sub
End Module
