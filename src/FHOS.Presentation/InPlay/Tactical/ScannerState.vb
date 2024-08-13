Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ScannerState
    Inherits BoardState
    Implements IState

    Private ReadOnly target As (X As Integer, Y As Integer)

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, target As (X As Integer, Y As Integer))
        MyBase.New(model, ui, endState)
        Me.target = target
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.Write((Mood.Prompt, "Arrows: "))
        ui.Write((Mood.Info, "Move | "))
        ui.Write((Mood.Prompt, "Tab: "))
        ui.WriteLine((Mood.Info, "Navigation"))
        RenderBoard(target)
        Dim targetLocation = model.State.GetLocation(target)
        ui.WriteLine(
            (Mood.Purple, Messages.Scanner),
            (Mood.Info, $"{targetLocation.Name}"))
        Dim actor = targetLocation.Actor
        If actor IsNot Nothing Then
            ui.WriteLine((Mood.Info, $"{actor.Name}"))
        End If
        Do
            Select Case ui.ReadKey()
                Case KeyNames.Tab
                    Return New NeutralState(model, ui, endState)
                Case KeyNames.UpArrow
                    Return MoveTarget(0, -1)
                Case KeyNames.DownArrow
                    Return MoveTarget(0, 1)
                Case KeyNames.LeftArrow
                    Return MoveTarget(-1, 0)
                Case KeyNames.RightArrow
                    Return MoveTarget(1, 0)
            End Select
        Loop
    End Function

    Private Function MoveTarget(deltaX As Integer, deltaY As Integer) As IState
        Dim nextX = Math.Clamp(target.X + deltaX, MinimumColumn, MaximumColumn)
        Dim nextY = Math.Clamp(target.Y + deltaY, MinimumRow, MaximumRow)
        Dim nextLocation = model.State.GetLocation((nextX, nextY))
        If nextLocation IsNot Nothing AndAlso nextLocation.Exists Then
            Return New ScannerState(model, ui, endState, (nextX, nextY))
        End If
        Return Me
    End Function
End Class
