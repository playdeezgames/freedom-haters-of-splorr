Imports SPLORR.UI

Friend Class MessageState
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
        If cmd = Command.A Then
            Context.Model.Messages.Dismiss()
            SetState(BoilerplateState.Neutral)
        End If
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        With Context.Model.Messages.Current
            Context.ShowHeader(displayBuffer, font, .Header, Context.UIPalette.Header, Context.UIPalette.Background)
            Dim lines = .Lines
            Dim position As (X As Integer, Y As Integer) = (Context.ViewCenter.X, Context.ViewCenter.Y - lines.Count * font.HalfHeight)
            For Each line In lines
                font.WriteCenteredText(displayBuffer, position, line.Text, line.Hue)
                position = (position.X, position.Y + font.Height)
            Next
            Context.ShowStatusBar(
                displayBuffer,
                font,
                Context.
                ControlsText(aButton:="Continue"), Context.UIPalette.Background, Context.UIPalette.Footer)
        End With
    End Sub
End Class
