Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class LoadState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim loadableSlots = model.LoadableSlots()
        If Not loadableSlots.Any Then
            ui.Message((Mood.Danger, "No Saves Exist!"))
            Return endState
        End If
        Dim menu As New List(Of (String, Integer?)) From
            {
                (Choices.Leave, Nothing)
            }
        menu.AddRange(loadableSlots.Select(Of (String, Integer?))(Function(x) (model.GetSlotName(x), x)))
        Dim choice = ui.Choose(Of Integer?)(
            (Mood.Prompt, Prompts.LoadGame),
            menu.ToArray)
        If Not choice.HasValue Then
            Return endState
        End If
        model.LoadGame(choice.Value)
        Return New NeutralState(model, ui, endState)
    End Function
End Class
