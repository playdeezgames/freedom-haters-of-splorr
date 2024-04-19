Friend Class ConfirmDeleteKeyBindingState(Of TModel)
    Inherits BasePickerState(Of TModel, Boolean)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "<placeholder>",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.KeyBindings)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As Boolean))
        Select Case value.Item
            Case False
                SetState(BoilerplateState.KeyBindings)
            Case True
                Context.KeyBindings.Unbind(KeyBindingsState(Of TModel).SelectedKey)
                SetState(BoilerplateState.KeyBindings)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As Boolean))
        Return New List(Of (Text As String, Item As Boolean)) From
            {
                (NoText, False),
                (YesText, True)
            }
    End Function

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        If Not Context.KeyBindings.CanUnbind(KeyBindingsState(Of TModel).SelectedKey) Then
            SetState(BoilerplateState.CannotUnbindKeyBinding)
            Return
        End If
        Me.HeaderText = $"Unbind '{KeyBindingsState(Of TModel).SelectedKey}'?"
    End Sub
End Class
