Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class InteractionState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.WriteLine(model.State.Avatar.Interaction.Lines.Select(Function(x) (moodTable(x.Hue), x.Text)).ToArray)
        Dim choice = ui.Choose((Mood.Prompt, String.Empty), model.State.Avatar.Interaction.AvailableChoices.ToArray)
        choice.Perform()
        Return New NeutralState(model, ui, endState)
    End Function
End Class
