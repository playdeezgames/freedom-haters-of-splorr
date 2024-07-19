Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class SaveState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim result As New List(Of (String, Integer?)) From
            {
                (Choices.Leave, Nothing)
            }
        result.AddRange(model.AvailableSlots.Select(Of (String, Integer?))(Function(x) ($"{model.GetSlotName(x)}{If(model.DoesSlotExist(x), "(will overwrite)", "")}", x)))
        Dim choice = ui.Choose(
            (Mood.Prompt, Prompts.SaveGame),
            result.ToArray)
        If choice.HasValue Then
            model.SaveGame(choice.Value)
            ui.Message((Mood.Success, "Game Saved!"))
        End If
        Return New NeutralState(model, ui, endState)
    End Function
End Class
