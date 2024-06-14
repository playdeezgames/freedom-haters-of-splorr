Imports FHOS.Model
Imports SPLORR.UI

Friend MustInherit Class BoardState
    Inherits BaseGameState(Of IUniverseModel)
    Protected Const ViewColumns = 21
    Protected Const ViewRows = 21
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Protected Sub RenderBoard(
                           displayBuffer As IPixelSink,
                           spriteFont As Font,
                           position As (X As Integer, Y As Integer),
                           locationSize As (Width As Integer, Height As Integer))
        Dim x = position.X
        For Each deltaX In Enumerable.Range(-ViewColumns \ 2, ViewColumns)
            Dim y = position.Y
            For Each deltaY In Enumerable.Range(-ViewRows \ 2, ViewRows)
                RenderLocation(
                    displayBuffer,
                    spriteFont,
                    (deltaX, deltaY),
                    (x, y))
                y += locationSize.Height
            Next
            x += locationSize.Width
        Next
    End Sub

    Private Sub RenderLocation(
                          displayBuffer As IPixelSink,
                          uiFont As Font,
                          boardPosition As (X As Integer, Y As Integer),
                          plotPosition As (X As Integer, Y As Integer))
        Dim locationModel = Context.Model.State.GetLocation(boardPosition)
        If Not locationModel.Exists Then
            Return
        End If

        With locationModel.Sprite
            uiFont.WriteLeftText(displayBuffer, plotPosition, ChrW(15), .Background)
            uiFont.WriteLeftText(displayBuffer, plotPosition, .Glyph, .Foreground)
        End With

        Dim actorModel = locationModel.Actor
        If actorModel IsNot Nothing Then
            Dim sprite = actorModel.Sprite
            uiFont.WriteLeftText(displayBuffer, plotPosition, sprite.Glyph, sprite.Hue)
        End If
    End Sub
End Class
