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
        PopState()
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        With FactionListState.SelectedFaction
            Context.ShowHeader(displayBuffer, font, .Name, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim position = (Context.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Authority: { .Authority.LevelName}({ .Authority.Value})", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Standards: { .Standards.LevelName}({ .Standards.Value})", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Conviction: { .Conviction.LevelName}({ .Conviction.Value})", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Planets: { .PlanetCount}", Hues.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Other Faction Relationships:", Hues.Black)
            For Each otherFaction In Context.Model.Pedia.FactionList.Where(Function(x) x.Text <> .Name)
                position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"{otherFaction.Faction.Name}: { .RelationNameTo(otherFaction.Faction)}", Hues.Black)
            Next
            Context.ShowStatusBar(
                displayBuffer,
                font,
                Context.
                ControlsText(bButton:="Go Back"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub
End Class
