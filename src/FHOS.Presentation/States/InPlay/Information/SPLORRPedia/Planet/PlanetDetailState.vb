Imports FHOS.Model
Imports SPLORR.UI

Friend Class PlanetDetailState
    Inherits BaseGameState(Of IUniverseModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.B
                PlanetListState.SelectedPlanet.Pop()
                SetState(Nothing)
            Case Command.A
                PushState(GameState.PlanetCrossReference)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        With PlanetListState.SelectedPlanet.Peek
            Context.ShowHeader(displayBuffer, font, .Name, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim position = (Context.ViewCenter.X, font.Height)
            'position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Type: {actor.Subtype}", Hues.Black)
            Context.ShowStatusBar(
                displayBuffer,
                font,
                Context.
                ControlsText(aButton:="Go To...", bButton:="Go Back"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
    End Sub
End Class
