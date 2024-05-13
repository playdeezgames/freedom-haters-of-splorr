Imports FHOS.Model
Imports SPLORR.UI

Friend Class FactionDetailState
    Inherits BaseGameState(Of Model.IUniverseModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        SetState(GameState.FactionList)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        Dim avatarFaction = Context.Model.Avatar.Faction
        With FactionListState.SelectedFaction
            Context.ShowHeader(displayBuffer, font, .Name, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim position = (Context.ViewCenter.X, font.Height)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Authority: { .Authority}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Standards: { .Standards}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Conviction: { .Conviction}", Hue.Black)
            position = font.WriteCenteredTextLines(displayBuffer, position, Context.ViewSize.Width, $"Relation to {avatarFaction.Name}: { .RelationNameTo(avatarFaction)}", Hue.Black)
            Context.ShowStatusBar(
                displayBuffer,
                font,
                Context.
                ControlsText(bButton:="Go Back"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub
End Class
