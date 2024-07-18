Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ScannerState
    Inherits BoardState
    Implements IState

    Private ReadOnly cursor As (X As Integer, Y As Integer)

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, cursor As (X As Integer, Y As Integer))
        MyBase.New(model, ui, endState)
        Me.cursor = cursor
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.Write((Mood.Prompt, "Arrows: "))
        ui.Write((Mood.Info, "Move | "))
        ui.Write((Mood.Prompt, "Tab: "))
        ui.WriteLine((Mood.Info, "Navigation"))
        RenderBoard(cursor)
        Dim key = ui.ReadKey()
        Select Case key
            Case KeyNames.Tab
                Return New NeutralState(model, ui, endState)
            Case KeyNames.UpArrow
                Return MoveCursor(0, -1)
            Case KeyNames.DownArrow
                Return MoveCursor(0, 1)
            Case KeyNames.LeftArrow
                Return MoveCursor(-1, 0)
            Case KeyNames.RightArrow
                Return MoveCursor(1, 0)
            Case Else
                Return Me
        End Select
    End Function

    Private Function MoveCursor(deltaX As Integer, deltaY As Integer) As IState
        Dim nextX = Math.Clamp(cursor.X + deltaX, MinimumColumn, MaximumColumn)
        Dim nextY = Math.Clamp(cursor.Y + deltaY, MinimumRow, MaximumRow)
        Dim nextLocation = model.State.GetLocation((nextX, nextY))
        If nextLocation IsNot Nothing AndAlso nextLocation.Exists Then
            Return New ScannerState(model, ui, endState, (nextX, nextY))
        End If
        Return Me
    End Function
End Class
