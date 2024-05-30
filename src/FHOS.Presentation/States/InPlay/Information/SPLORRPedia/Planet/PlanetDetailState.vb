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
    Private actor As IActorModel = Nothing

    Public Overrides Sub HandleCommand(cmd As String)
        PlanetListState.SelectedPlanet.Pop()
        SetState(Nothing)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        With PlanetListState.SelectedPlanet.Peek
            Context.ShowHeader(displayBuffer, font, .Name, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim position = (Context.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Type: {actor.Subtype}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Star System: {actor.StarSystem.Name}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Satellites: {actor.SatelliteCount}", Hues.Black)
            Context.ShowStatusBar(
                displayBuffer,
                font,
                Context.
                ControlsText(bButton:="Go Back"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        actor = PlanetListState.SelectedPlanet.Peek.Actors.Single(Function(x) x.IsPlanetVicinity)
    End Sub
End Class
