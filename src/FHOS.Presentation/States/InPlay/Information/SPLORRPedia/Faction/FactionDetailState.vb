Imports FHOS.Model
Imports SPLORR.UI

Friend Class FactionDetailState
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
                Context.Model.Ephemerals.SelectedFaction.Pop()
                SetState(Nothing)
            Case Command.A
                SetState(GameState.FactionPlanetList)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        With Context.Model.Ephemerals.SelectedFaction.Peek
            Context.ShowHeader(displayBuffer, font, .Name, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim position = (Context.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Authority: { .Properties.Authority.LevelName}({ .Properties.Authority.Value})", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Standards: { .Properties.Standards.LevelName}({ .Properties.Standards.Value})", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Conviction: { .Properties.Conviction.LevelName}({ .Properties.Conviction.Value})", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Planets: { .Properties.PlanetCount}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Other Faction Relationships:", Hues.Black)
            For Each otherFaction In Context.Model.Pedia.Factions.Where(Function(x) x.Name <> .Name)
                position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"{otherFaction.Name}: { .RelationNameTo(otherFaction)}", Hues.Black)
            Next
            Context.ShowStatusBar(
                displayBuffer,
                font,
                Context.
                ControlsText(aButton:="Planets...", bButton:="Go Back"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub
End Class
