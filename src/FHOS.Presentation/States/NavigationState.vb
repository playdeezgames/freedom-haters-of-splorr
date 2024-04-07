Imports FHOS.Model
Imports SPLORR.UI

Friend Class NavigationState
    Inherits BaseGameState(Of IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.Start, Command.B
                SetState(BoilerplateState.GameMenu)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Context.ShowStatusBar(displayBuffer, Context.Font(UIFontName), Context.ControlsText(Nothing, "Game Menu"), Context.UIPalette.Background, Context.UIPalette.Footer)
    End Sub
End Class
