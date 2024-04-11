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
    End Sub

    Private Sub RenderStatistics(displayBuffer As IPixelSink, uiFont As Font, position As (X As Integer, Y As Integer))
        uiFont.WriteText(displayBuffer, (position.X, position.Y + uiFont.Height * 0), $"{Context.Model.Avatar.MapName} ({Context.Model.Avatar.X},{Context.Model.Avatar.Y})", 0)
    End Sub
End Class
