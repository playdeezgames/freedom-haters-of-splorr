Imports FHOS.Model
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
        Dim details = TutorialDetails(Context.Model.Avatar.CurrentTutorial)
        Context.ShowHeader(displayBuffer, font, details.Header, Context.UIPalette.Header, Context.UIPalette.Background)
        Dim y = Context.ViewCenter.Y - details.Lines.Count * font.HalfHeight
        For Each line In details.Lines
            font.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, y), line.Text, line.Hue)
            y += font.Height
        Next
        Context.ShowStatusBar(displayBuffer, font, Context.ControlsText(aButton:="Continue"), Context.UIPalette.Background, Context.UIPalette.Footer)
    End Sub

    Private Shared ReadOnly TutorialDetails As IReadOnlyDictionary(Of String, TutorialDetail) =
        New Dictionary(Of String, TutorialDetail) From
        {
            {
                TutorialTypes.StarSystemEntry,
                New TutorialDetail(
                    "Entering Star Systems",
                    {
                        New TutorialDetailLine("You have reached a star system!", Hue.Black),
                        New TutorialDetailLine("", Hue.Black),
                        New TutorialDetailLine("To enter it,", Hue.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hue.Black),
                        New TutorialDetailLine("then choose 'Enter System'", Hue.Black)
                    })}
        }
End Class
