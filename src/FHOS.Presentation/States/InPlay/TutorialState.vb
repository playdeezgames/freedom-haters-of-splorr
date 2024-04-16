Imports SPLORR.UI

Friend Class TutorialState
    Inherits BaseGameState(Of Model.IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        If cmd = Command.A Then
            Context.Model.Avatar.DismissTutorial()
            SetState(BoilerplateState.Neutral)
        End If
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        Context.ShowHeader(displayBuffer, font, Context.Model.Avatar.CurrentTutorial, Context.UIPalette.Header, Context.UIPalette.Background)
        Context.ShowStatusBar(displayBuffer, font, Context.ControlsText(aButton:="Continue"), Context.UIPalette.Background, Context.UIPalette.Footer)
    End Sub
End Class
