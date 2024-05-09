Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarVicinityDetailsState
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
        SetState(GameState.StarVicinityList)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim starVicinity As IPlaceModel = KnownPlaceListState.SelectedPlace
        With Context
            Dim font = .Font(UIFontName)
            .ShowHeader(
                displayBuffer,
                font,
                $"{starVicinity.Name} Vicinity",
                .UIPalette.Header,
                .UIPalette.Background)
            Dim position = (.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Type: {starVicinity.StarType}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"System: {starVicinity.Parent.Name}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"System Position: ({starVicinity.X},{starVicinity.Y})", Hue.Black)
            .ShowStatusBar(
                displayBuffer,
                font,
                .ControlsText(bButton:="Cancel"),
                .UIPalette.Background,
                .UIPalette.Footer)
        End With
    End Sub
End Class
