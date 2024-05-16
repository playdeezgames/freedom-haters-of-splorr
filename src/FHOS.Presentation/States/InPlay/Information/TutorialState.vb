Imports FHOS.Model
Imports SPLORR.UI

Friend Class TutorialState
    Inherits BaseGameState(Of Model.IUniverseModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        If cmd = Command.A Then
            Context.Model.State.Avatar.Tutorial.Dismiss()
            SetState(BoilerplateState.Neutral)
        End If
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        Dim details = TutorialDetails(Context.Model.State.Avatar.Tutorial.Current)
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
                        New TutorialDetailLine("You have reached a star system!", Hues.Black),
                        New TutorialDetailLine("", Hues.Black),
                        New TutorialDetailLine("To enter it,", Hues.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hues.Black),
                        New TutorialDetailLine("then choose 'Enter Star System'", Hues.Black)
                    })
            },
            {
                TutorialTypes.StarVicinityApproach,
                New TutorialDetail(
                    "Approaching Star",
                    {
                        New TutorialDetailLine("You have reached a star!", Hues.Black),
                        New TutorialDetailLine("", Hues.Black),
                        New TutorialDetailLine("To approach it,", Hues.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hues.Black),
                        New TutorialDetailLine("then choose 'Approach Star'", Hues.Black)
                    })
            },
            {
                TutorialTypes.PlanetVicinityApproach,
                New TutorialDetail(
                    "Approaching Planet",
                    {
                        New TutorialDetailLine("You are near a planet!", Hues.Black),
                        New TutorialDetailLine("", Hues.Black),
                        New TutorialDetailLine("To approach it,", Hues.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hues.Black),
                        New TutorialDetailLine("then choose 'Approach Planet'", Hues.Black)
                    })
            },
            {
                TutorialTypes.EnterSatelliteOrbit,
                New TutorialDetail(
                    "Approaching Satellite",
                    {
                        New TutorialDetailLine("You have reached a Satellite!", Hues.Black),
                        New TutorialDetailLine("", Hues.Black),
                        New TutorialDetailLine("To enter its orbit,", Hues.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hues.Black),
                        New TutorialDetailLine("then choose 'Enter Orbit'", Hues.Black)
                    })
            },
            {
                TutorialTypes.EnterPlanetOrbit,
                New TutorialDetail(
                    "Planet Orbit",
                    {
                        New TutorialDetailLine("You have reached a planet!", Hues.Black),
                        New TutorialDetailLine("", Hues.Black),
                        New TutorialDetailLine("To enter its orbit,", Hues.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hues.Black),
                        New TutorialDetailLine("then choose 'Enter Orbit'", Hues.Black)
                    })
            },
            {
                TutorialTypes.RefuelAtStar,
                New TutorialDetail(
                    "Refuel at Star",
                    {
                        New TutorialDetailLine("You have reached a star!", Hues.Black),
                        New TutorialDetailLine("", Hues.Black),
                        New TutorialDetailLine("To refuel,", Hues.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hues.Black),
                        New TutorialDetailLine("then choose 'Refuel'", Hues.Black)
                    })
            },
            {
                TutorialTypes.OutOfFuel,
                New TutorialDetail(
                    "Out of Fuel!",
                    {
                        New TutorialDetailLine("You are out of fuel!", Hues.Black),
                        New TutorialDetailLine("", Hues.Black),
                        New TutorialDetailLine("To send a distress signal,", Hues.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hues.Black),
                        New TutorialDetailLine("then choose 'Distress Signal'", Hues.Black)
                    })
            },
            {
                TutorialTypes.WormholeEntry,
                New TutorialDetail(
                    "Wormhole!",
                    {
                        New TutorialDetailLine("You have reached a wormhole!", Hues.Black),
                        New TutorialDetailLine("", Hues.Black),
                        New TutorialDetailLine("To enter it,", Hues.Black),
                        New TutorialDetailLine("press [A] from the NAV SCREEN", Hues.Black),
                        New TutorialDetailLine("then choose 'Enter Wormhole'", Hues.Black)
                    })
            }
        }

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        If Context.Model.State.Avatar.Tutorial.IgnoreCurrent Then
            Context.Model.State.Avatar.Tutorial.Dismiss()
            SetState(BoilerplateState.Neutral)
        End If
    End Sub
End Class
