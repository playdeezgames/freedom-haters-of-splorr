Friend Class SaveConfirmationState(Of TModel)
    Inherits BaseGameState(Of TModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        SetState(BoilerplateState.GameMenu)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFont)
        font.WriteCenteredText(
            displayBuffer,
            (Context.ViewCenter.X, Context.ViewCenter.Y - font.HalfHeight),
            "Game Saved.",
            Context.UIPalette.MenuItem)
        Context.ShowStatusBar(
            displayBuffer,
            font,
            Context.ControlsText(aButton:="Continue"),
            Context.UIPalette.Background,
            Context.UIPalette.Footer)
    End Sub
End Class
