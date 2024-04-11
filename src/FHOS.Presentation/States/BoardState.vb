Imports FHOS.Model
Imports SPLORR.UI

Friend MustInherit Class BoardState
    Inherits BaseGameState(Of IWorldModel)
    Protected Const ViewColumns = 21
    Protected Const ViewRows = 21
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Protected Sub RenderBoard(
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
                    (x, y))
                y += cellSize.Height
            Next
            x += cellSize.Width
        Next
    End Sub

    Private Sub RenderCell(
                          displayBuffer As IPixelSink,
                          uiFont As Font,
                          boardPosition As (X As Integer, Y As Integer),
                          plotPosition As (X As Integer, Y As Integer))
        Dim cellModel = Context.Model.Board.GetCell(boardPosition)
        If Not cellModel.Exists Then
            Return
        End If

        Dim terrainModel = cellModel.Terrain
        uiFont.WriteText(displayBuffer, plotPosition, ChrW(15), terrainModel.Background)
        uiFont.WriteText(displayBuffer, plotPosition, terrainModel.Glyph, terrainModel.Foreground)

        Dim characterModel = cellModel.Character
        If characterModel IsNot Nothing Then
            uiFont.WriteText(displayBuffer, plotPosition, characterModel.Glyph, characterModel.Hue)
        End If
    End Sub
End Class
