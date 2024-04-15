Friend Class AddKeyBindingState(Of TModel)
    Inherits BasePickerState(Of TModel, String)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Add Key Binding...",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.KeyBindings)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case GoBackText
                SetState(BoilerplateState.KeyBindings)
            Case Else
                KeyBindingsState(Of TModel).SelectedKey = value.Item
                SetState(BoilerplateState.AddBoundCommand)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                (GoBackText, GoBackText)
            }
        result.AddRange(Context.KeyBindings.UnboundKeys)
        Return result
    End Function
End Class
