Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class SelectCrewMemberState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim choice = ui.Choose((Mood.Prompt, "Select Crew Member"), model.State.Avatar.Vessel.AvailableCrew.ToArray)
        model.State.Avatar.Stack.Push(choice)
        Return New NeutralState(model, ui, endState)
    End Function
End Class
