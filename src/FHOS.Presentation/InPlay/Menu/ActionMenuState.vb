Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ActionMenuState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim choice = ui.Choose(
            (Mood.Prompt, Prompts.ActionMenu),
            model.State.Avatar.Verbs.Available.ToArray)
        Select Case choice
            Case VerbTypes.Status
                Return New StatusState(model, ui, endState)
            Case VerbTypes.Inventory
                Return New InventoryState(model, ui, endState)
            Case VerbTypes.Crew
                Return New SelectCrewMemberState(model, ui, endState)
            Case VerbTypes.Vessel
                model.State.Avatar.Stack.Pop()
                Return New NeutralState(model, ui, endState)
            Case Else
                Return New DoVerbState(model, ui, endState, choice)
        End Select
    End Function
End Class
