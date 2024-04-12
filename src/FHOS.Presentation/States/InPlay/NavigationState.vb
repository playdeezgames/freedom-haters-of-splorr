Imports FHOS.Model
Imports SPLORR.UI

Friend Class NavigationState
    Inherits BoardState

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.Start, Command.B
                SetState(BoilerplateState.GameMenu)
            Case Command.Up
                SetState(GameState.MoveUp)
            Case Command.Down
                SetState(GameState.MoveDown)
            Case Command.Right
                SetState(GameState.MoveRight)
            Case Command.Left
                SetState(GameState.MoveLeft)
            Case Command.Select
                SetState(GameState.Scanner)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim uiFont = Context.Font(UIFontName)
        Dim cellWidth = uiFont.TextWidth(ChrW(32))
        Dim cellHeight = uiFont.Height
        RenderBoard(
            displayBuffer,
            uiFont,
            (0, 0),
            (cellWidth, cellHeight))
        RenderStatistics(displayBuffer, uiFont, (ViewColumns * cellWidth, 0))
        Context.ShowStatusBar(displayBuffer, uiFont, Context.ControlsText("", ""), Black, DarkGray)
    End Sub

    Private Sub RenderStatistics(displayBuffer As IPixelSink, uiFont As Font, position As (X As Integer, Y As Integer))

        uiFont.WriteLeftText(displayBuffer, position, $"{Context.Model.Avatar.MapName} ({Context.Model.Avatar.X},{Context.Model.Avatar.Y})", Black)
        position = NextLine(position, uiFont)
        uiFont.WriteLeftText(displayBuffer, position, $"O2: {Context.Model.Avatar.OxygenPercent}%", Context.Model.Avatar.OxygenHue)
    End Sub

    Private Function NextLine(position As (X As Integer, Y As Integer), uiFont As Font) As (X As Integer, Y As Integer)
        Return (position.X, position.Y + uiFont.Height)
    End Function
End Class
