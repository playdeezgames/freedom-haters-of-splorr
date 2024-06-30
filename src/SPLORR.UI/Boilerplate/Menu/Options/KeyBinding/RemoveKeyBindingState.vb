Friend Class RemoveKeyBindingState(Of TModel)
    Inherits BasePickerState(Of TModel, String)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Remove Key Binding",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.KeyBindings)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Context.SelectedKey = value.Item
        SetState(BoilerplateState.ConfirmDeleteKeyBinding)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return Context.
            KeyBindings.
            KeysTable.
            Select(Function(x) ($"{x.Key} -> [{x.Value}]", x.Key)).
            ToList
    End Function
End Class
