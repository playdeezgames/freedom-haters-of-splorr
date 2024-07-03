Imports FHOS.Model
Imports SPLORR.UI

Friend Class InventoryState
    Inherits BasePickerState(Of IUniverseModel, IAvatarInventoryItemStackModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Inventory",
            context.ChooseOrCancel,
            GameState.ActionMenu)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IAvatarInventoryItemStackModel))
        Context.Model.Ephemerals.InventoryItemStack = value.Item
        SetState(GameState.InventoryActionSelect)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IAvatarInventoryItemStackModel))
        Return Context.Model.State.Avatar.Inventory.ItemStacks.Select(Function(x) ($"{x.ItemName}(x{x.Count})", x)).ToList
    End Function
End Class
