Imports FHOS.Model
Imports SPLORR.UI

Friend Class StatusState
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
        SetState(GameState.ActionMenu)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim avatar = Context.Model.Avatar
        With Context
            Dim font = .Font(UIFontName)
            .ShowHeader(
                displayBuffer,
                font,
                $"Status",
                .UIPalette.Header,
                .UIPalette.Background)
            Dim position = (.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"O2: {avatar.Vessel.OxygenPercent}%", Hues.ForPercentage(avatar.Vessel.OxygenPercent))
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Fuel: {avatar.Vessel.FuelPercent}%", Hues.ForPercentage(avatar.Vessel.FuelPercent))
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Faction: {avatar.Bio.Faction.Name}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, .ViewSize.Width, $"Home Planet: {avatar.Bio.HomePlanet.Name}", Hues.Black)
            .ShowStatusBar(
                displayBuffer,
                font,
                .ControlsText(bButton:="Cancel"),
                .UIPalette.Background,
                .UIPalette.Footer)
        End With
    End Sub
End Class
