Imports FHOS.Model
Imports SPLORR.UI

Friend Class TutorialState
    Inherits BaseGameState(Of Model.IUniverseModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        If cmd = Command.A Then
            Context.Model.Avatar.Tutorial.Dismiss()
            SetState(BoilerplateState.Neutral)
        End If
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        Dim details = TutorialDetails(Context.Model.Avatar.Tutorial.Current)
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
                        New TutorialDetailLine("then choose 'Enter Star System'", Hue.Black)
                    })
            },
            {
                TutorialTypes.StarVicinityApproach,
                New TutorialDetail(
                    "Approaching Star",
                    {
                        New TutorialDetailLine("You have reached a star!", Hue.Black),
                        New TutorialDetailLine("", Hue.Black),
                        New TutorialDetailLine("To approach it,", Hue.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hue.Black),
                        New TutorialDetailLine("then choose 'Approach Star'", Hue.Black)
                    })
            },
            {
                TutorialTypes.PlanetVicinityApproach,
                New TutorialDetail(
                    "Approaching Planet",
                    {
                        New TutorialDetailLine("You are near a planet!", Hue.Black),
                        New TutorialDetailLine("", Hue.Black),
                        New TutorialDetailLine("To approach it,", Hue.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hue.Black),
                        New TutorialDetailLine("then choose 'Approach Planet'", Hue.Black)
                    })
            },
            {
                TutorialTypes.SatelliteApproach,
                New TutorialDetail(
                    "Approaching Satellite",
                    {
                        New TutorialDetailLine("You have reached a Satellite!", Hue.Black),
                        New TutorialDetailLine("", Hue.Black),
                        New TutorialDetailLine("To approach it,", Hue.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hue.Black),
                        New TutorialDetailLine("then choose 'Approach Satellite'", Hue.Black)
                    })
            },
            {
                TutorialTypes.PlanetLand,
                New TutorialDetail(
                    "Land on Planet",
                    {
                        New TutorialDetailLine("You have reached a planet!", Hue.Black),
                        New TutorialDetailLine("", Hue.Black),
                        New TutorialDetailLine("To land on it,", Hue.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hue.Black),
                        New TutorialDetailLine("then choose 'Land on Planet'", Hue.Black)
                    })
            },
            {
                TutorialTypes.RefuelAtStar,
                New TutorialDetail(
                    "Refuel at Star",
                    {
                        New TutorialDetailLine("You have reached a star!", Hue.Black),
                        New TutorialDetailLine("", Hue.Black),
                        New TutorialDetailLine("To refuel,", Hue.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hue.Black),
                        New TutorialDetailLine("then choose 'Refuel'", Hue.Black)
                    })
            },
            {
                TutorialTypes.OutOfFuel,
                New TutorialDetail(
                    "Out of Fuel!",
                    {
                        New TutorialDetailLine("You are out of fuel!", Hue.Black),
                        New TutorialDetailLine("", Hue.Black),
                        New TutorialDetailLine("To send a distress signal,", Hue.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hue.Black),
                        New TutorialDetailLine("then choose 'Distress Signal'", Hue.Black)
                    })
            }
        }

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        If Context.Model.Avatar.Tutorial.IgnoreCurrent Then
            Context.Model.Avatar.Tutorial.Dismiss()
            SetState(BoilerplateState.Neutral)
        End If
    End Sub
End Class
