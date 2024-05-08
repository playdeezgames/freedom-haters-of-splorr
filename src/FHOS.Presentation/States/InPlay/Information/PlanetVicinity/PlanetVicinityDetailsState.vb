Imports FHOS.Model
Imports SPLORR.UI

Friend Class PlanetVicinityDetailsState
    Inherits BaseGameState(Of Model.IUniverseModel)

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
        SetState(GameState.StarSystemList)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim planetVicinity As IPlaceModel = PlanetVicinityListState.SelectedPlanetVicinity
        With Context
            Dim font = .Font(UIFontName)
            .ShowHeader(
                displayBuffer,
                font,
                $"{planetVicinity.Name} Vicinity",
                .UIPalette.Header,
                .UIPalette.Background)
            Dim position = (.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Type: {planetVicinity.PlanetType}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Satellites: {planetVicinity.SatelliteCount}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"System: {planetVicinity.Parent.Name}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"System Position: ({planetVicinity.X},{planetVicinity.Y})", Hue.Black)
            .ShowStatusBar(
                displayBuffer,
                font,
                .ControlsText(bButton:="Cancel"),
                .UIPalette.Background,
                .UIPalette.Footer)
        End With
    End Sub
End Class
