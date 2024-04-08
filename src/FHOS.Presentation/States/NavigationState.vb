Imports FHOS.Model
Imports SPLORR.UI

Friend Class NavigationState
    Inherits BaseGameState(Of IWorldModel)
    Private Const ViewColumns = 15
    Private Const ViewRows = 13

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
        Context.ShowStatusBar(
            displayBuffer,
            Context.Font(UIFontName),
            Context.ControlsText("Actions", "Menu"),
            Context.UIPalette.Background,
            Context.UIPalette.Footer)
    End Sub

    Private Sub RenderStatistics(displayBuffer As IPixelSink, uiFont As Font, position As (X As Integer, Y As Integer))
        uiFont.WriteText(displayBuffer, position, $"X: {Context.Model.Avatar.X}", 0)
        uiFont.WriteText(displayBuffer, (position.X, position.Y + uiFont.Height), $"Y: {Context.Model.Avatar.Y}", 0)
        uiFont.WriteText(displayBuffer, (position.X, position.Y + uiFont.Height * 2), $"D: {Context.Model.Avatar.Facing}", 0)
    End Sub

    Private Sub RenderBoard(
                           displayBuffer As IPixelSink,
                           spriteFont As Font,
                           position As (X As Integer, Y As Integer),
                           cellSize As (Width As Integer, Height As Integer))
        Dim x = position.X
        For Each deltaX In Enumerable.Range(-ViewColumns \ 2, ViewColumns)
            Dim y = position.Y
            For Each deltaY In Enumerable.Range(-ViewRows \ 2, ViewRows)
                RenderCell(
                    displayBuffer,
                    spriteFont,
                    (deltaX, deltaY),
                    (x, y),
                    cellSize)
                y += cellSize.Height
            Next
            x += cellSize.Width
        Next
    End Sub

    Private Sub RenderCell(
                          displayBuffer As IPixelSink,
                          uiFont As Font,
                          boardPosition As (X As Integer, Y As Integer),
                          plotPosition As (X As Integer, Y As Integer),
                          cellSize As (Width As Integer, Height As Integer))
        Dim boardCell = Context.Model.Board.GetCell(boardPosition)
        If Not boardCell.Exists Then
            Return
        End If
        displayBuffer.Fill(plotPosition, cellSize, 0)
        uiFont.WriteText(displayBuffer, plotPosition, ".", 8)
    End Sub
End Class
