Friend Class SaveConfirmationState(Of TModel)
    Inherits BaseGameState(Of TModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of TModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        SetState(BoilerplateState.GameMenu)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFont)
        Dim text = "Game Saved."
        font.WriteText(displayBuffer, (Context.ViewSize.Width \ 2 - font.TextWidth(text) \ 2, Context.ViewSize.Height \ 2 - font.HalfHeight), text, Context.UIPalette.MenuItem)
        Context.ShowStatusBar(displayBuffer, font, Context.ControlsText("Continue", Nothing), Context.UIPalette.Background, Context.UIPalette.Footer)
    End Sub
End Class
