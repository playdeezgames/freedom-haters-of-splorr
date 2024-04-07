Imports FHOS.Model
Imports SPLORR.UI

Friend Class NavigationState
    Inherits BaseGameState(Of IWorldModel)
    Private Const ViewRadius = 6
    Private Const ViewColumns = ViewRadius * 2 + 1
    Private Const ViewRows = ViewRadius * 2 + 1

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.Start, Command.B
                SetState(BoilerplateState.GameMenu)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(Context.UIPalette.Background)
        Dim spriteFont = Context.Font(SpriteFontName)
        Dim cellWidth = spriteFont.TextWidth(ChrW(0))
        Dim cellHeight = spriteFont.Height
        RenderBoard(
            displayBuffer,
            spriteFont,
            (ViewWidth \ 2 - ViewColumns * cellWidth \ 2, ViewHeight \ 2 - ViewRows * cellHeight \ 2 - cellHeight \ 2),
            (cellWidth, cellHeight))
        Context.ShowStatusBar(
            displayBuffer,
            Context.Font(UIFontName),
            Context.ControlsText(Nothing, "Game Menu"),
            Context.UIPalette.Background,
            Context.UIPalette.Footer)
    End Sub

    Private Sub RenderBoard(
                           displayBuffer As IPixelSink,
                           spriteFont As Font,
                           position As (X As Integer, Y As Integer),
                           cellSize As (Width As Integer, Height As Integer))
        Dim x = position.X
        For Each deltaX In Enumerable.Range(-ViewRadius, ViewColumns)
            Dim y = position.Y
            For Each deltaY In Enumerable.Range(-ViewRadius, ViewRows)
                RenderCell(
                    displayBuffer,
                    spriteFont,
                    (deltaX, deltaY),
                    (x, y))
                y += cellSize.Height
            Next
            x += cellSize.Width
        Next
    End Sub

    Private Sub RenderCell(
                          displayBuffer As IPixelSink,
                          spriteFont As Font,
                          boardPosition As (X As Integer, Y As Integer),
                          plotPosition As (X As Integer, Y As Integer))
        Dim boardCell = Context.Model.Board.GetCell(boardPosition)
        spriteFont.WriteText(displayBuffer, plotPosition, ChrW(63), 0)
    End Sub
End Class
