Friend Class CannotUnbindKeyBindingState(Of TModel)
    Inherits BaseMessageState(Of TModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            BoilerplateState.KeyBindings)
    End Sub

    Protected Overrides Function MessageHue() As Integer
        Return Context.UIPalette.Error
    End Function

    Protected Overrides Function MessageText() As String
        Return $"Cannot Unbind '{KeyBindingsState(Of TModel).SelectedKey}'"
    End Function
End Class
