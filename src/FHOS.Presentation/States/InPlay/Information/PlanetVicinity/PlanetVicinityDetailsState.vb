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
        SetState(GameState.PlanetVicinityList)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim planetVicinity As IPlaceModel = PlanetVicinityListState.SelectedPlanetVicinity
        With Context
            .ShowHeader(
                displayBuffer,
                .Font(UIFontName),
                planetVicinity.Name,
                .UIPalette.Header,
                .UIPalette.Background)
            .ShowStatusBar(
                displayBuffer,
                .Font(UIFontName),
                .ControlsText(bButton:="Cancel"),
                .UIPalette.Background,
                .UIPalette.Footer)
        End With
    End Sub
End Class
