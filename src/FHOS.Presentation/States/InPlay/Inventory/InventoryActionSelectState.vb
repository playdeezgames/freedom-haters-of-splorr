Imports SPLORR.UI

Friend Class InventoryActionSelectState
    Inherits BasePickerState(Of Model.IUniverseModel, String)
    Const InspectText = "Inspect..."

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "<placeholder>",
            context.ChooseOrCancel,
            GameState.Inventory)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case InspectText
                SetState(GameState.InventoryInspect)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        HeaderText = Context.Model.Ephemerals.InventoryItemStack.ItemName
        Dim result As New List(Of (Text As String, Item As String))
        result.Add((InspectText, InspectText))
        Return result
    End Function
End Class
