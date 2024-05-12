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
        With FactionListState.SelectedFaction
            Context.ShowHeader(displayBuffer, font, .Name, Context.UIPalette.Header, Context.UIPalette.Background)
            Context.ShowStatusBar(
                displayBuffer,
                font,
                Context.
                ControlsText(bButton:="Go Back"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub
End Class
