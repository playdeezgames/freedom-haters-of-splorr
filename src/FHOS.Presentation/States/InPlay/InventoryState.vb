Imports FHOS.Model
Imports SPLORR.UI

Friend Class InventoryState
    Inherits BasePickerState(Of IUniverseModel, String)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Inventory",
            context.ChooseOrCancel,
            GameState.ActionMenu)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        SetState(GameState.ActionMenu)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return Context.Model.State.Avatar.Inventory.Summary.ToList
    End Function
End Class
