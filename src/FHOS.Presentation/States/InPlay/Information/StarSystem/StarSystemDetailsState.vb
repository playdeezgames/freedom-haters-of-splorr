Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarSystemDetailsState
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
        Dim starSystem As IPlaceModel = StarSystemListState.SelectedStarSystem
        With Context
            Dim font = .Font(UIFontName)
            .ShowHeader(
                displayBuffer,
                font,
                $"{starSystem.Name} System",
                .UIPalette.Header,
                .UIPalette.Background)
            Dim position = (.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Type: {starSystem.StarType}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Planets: {starSystem.PlanetVicinityCount}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Galaxy Position: ({starSystem.X},{starSystem.Y})", Hue.Black)
            .ShowStatusBar(
                displayBuffer,
                font,
                .ControlsText(bButton:="Cancel"),
                .UIPalette.Background,
                .UIPalette.Footer)
        End With
    End Sub
End Class
