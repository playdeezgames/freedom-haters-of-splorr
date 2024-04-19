Friend Class ConfirmDeleteKeyBindingState(Of TModel)
    Inherits BaseConfirmState(Of TModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "<placeholder>",
            BoilerplateState.KeyBindings,
            BoilerplateState.KeyBindings)
    End Sub

    Protected Overrides Sub OnConfirm()
        Context.KeyBindings.Unbind(KeyBindingsState(Of TModel).SelectedKey)
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        If Not Context.KeyBindings.CanUnbind(KeyBindingsState(Of TModel).SelectedKey) Then
            SetState(BoilerplateState.CannotUnbindKeyBinding)
            Return
        End If
        Me.HeaderText = $"Unbind '{KeyBindingsState(Of TModel).SelectedKey}'?"
    End Sub
End Class
