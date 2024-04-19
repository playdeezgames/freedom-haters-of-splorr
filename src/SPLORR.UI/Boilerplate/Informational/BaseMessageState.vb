Public MustInherit Class BaseMessageState(Of TModel)
    Inherits BaseGameState(Of TModel)

    Private NextGameState As String

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel),
                  nextGameState As String)
        MyBase.New(parent, setState, context)
        Me.NextGameState = nextGameState
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        SetState(NextGameState)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFont)
        font.WriteCenteredText(
            displayBuffer,
            (Context.ViewCenter.X, Context.ViewCenter.Y - font.HalfHeight),
            MessageText,
            MessageHue)
        Context.ShowStatusBar(
            displayBuffer,
            font,
            Context.ControlsText(aButton:="Continue"),
            Context.UIPalette.Background,
            Context.UIPalette.Footer)
    End Sub

    Protected MustOverride Function MessageHue() As Integer
    Protected MustOverride Function MessageText() As String
End Class
