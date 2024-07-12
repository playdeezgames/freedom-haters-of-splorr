Imports FHOS.Model
Imports FHOS.Presentation
Imports SPLORR.Presentation.Spectre

Module Program
    Sub Main(args As String())
        Console.Title = "Freedom Haters of SPLORR!!"
        MainMenuState.Start(New UniverseModel, New UIContext)
    End Sub
End Module
