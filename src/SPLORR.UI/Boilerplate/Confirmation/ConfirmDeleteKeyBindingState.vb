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
            BoilerplateState.RemoveKeyBinding,
            BoilerplateState.RemoveKeyBinding)
    End Sub

    Protected Overrides Sub OnConfirm()
        Context.KeyBindings.Unbind(Context.SelectedKey)
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        If Not Context.KeyBindings.CanUnbind(Context.SelectedKey) Then
            SetState(BoilerplateState.CannotUnbindKeyBinding)
            Return
        End If
        Me.HeaderText = $"Unbind '{Context.SelectedKey}'?"
    End Sub
End Class
