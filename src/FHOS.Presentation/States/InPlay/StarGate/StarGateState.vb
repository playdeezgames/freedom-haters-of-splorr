Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarGateState
    Inherits BasePickerState(Of IUniverseModel, IActorModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Choose Destination",
            context.ChooseOrCancel,
            GameState.LeaveStarGate)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IActorModel))
        Context.Model.State.Avatar.StarGate.Enter(value.Item)
        SetState(BoilerplateState.Neutral)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IActorModel))
        Return Context.Model.State.Avatar.StarGate.AvailableGates.OrderBy(Function(x) x.Text).ToList
    End Function
End Class
