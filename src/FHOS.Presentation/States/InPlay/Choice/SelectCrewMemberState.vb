Imports FHOS.Model
Imports SPLORR.UI

Friend Class SelectCrewMemberState
    Inherits BasePickerState(Of IUniverseModel, IActorModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Select Crew Member",
            context.ChooseOrCancel,
            GameState.ActionMenu)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IActorModel))
        Context.Model.Avatar.Stack.Push(value.Item)
        SetState(BoilerplateState.Neutral)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IActorModel))
        Return Context.Model.Avatar.Vessel.AvailableCrew.ToList
    End Function
End Class
