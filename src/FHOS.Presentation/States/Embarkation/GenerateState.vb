Imports SPLORR.UI

Friend Class GenerateState
    Inherits BaseGameState(Of Model.IUniverseModel)
    Private _timeStart As DateTimeOffset

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
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim font = Context.Font(UIFontName)
        Context.ShowHeader(displayBuffer, font, "Generating...", Context.UIPalette.Header, Context.UIPalette.Background)
        font.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, Context.ViewCenter.Y - font.HalfHeight * 3), $"Steps Completed: {Context.Model.Generator.StepsCompleted}", Context.UIPalette.MenuItem)
        font.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, Context.ViewCenter.Y - font.HalfHeight), $"Steps To Go: {Context.Model.Generator.StepsRemaining}", Context.UIPalette.MenuItem)
        font.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, Context.ViewCenter.Y + font.HalfHeight), $"Time Taken: {(DateTimeOffset.Now - _timeStart).TotalSeconds:f1}s", Context.UIPalette.MenuItem)
        Context.Model.Generator.Generate()
        If Context.Model.Generator.Done Then
            SetState(GameState.Neutral)
        End If
    End Sub
    Public Overrides Sub OnStart()
        MyBase.OnStart()
        _timeStart = DateTimeOffset.Now
    End Sub
End Class
