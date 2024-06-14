Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarSystemDetailState
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
                SetState(Nothing)
                StarSystemListState.SelectedStarSystem.Pop()
            Case Command.A
                PushState(GameState.StarSystemCrossReference)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        With StarSystemListState.SelectedStarSystem.Peek
            Context.ShowHeader(displayBuffer, font, .Name, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim position = (Context.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Type: { .Properties.StarTypeName}", Hues.Black)
            Dim galaxyPosition = .Position
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Position: ({galaxyPosition.Column},{galaxyPosition.Row})", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Planet Count: { .PlanetCount}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Satellite Count: { .SatelliteCount}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Factions Present:", Hues.Black)
            For Each faction In .ChildPlanetFactions
                position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"{faction.Name}", Hues.Black)
            Next
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
