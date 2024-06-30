Friend Class KeyBindingsState(Of TModel)
    Inherits BasePickerState(Of TModel, String)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Key Bindings",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.Options)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case GoBackText
                Context.KeyBindings.Reload()
                SetState(BoilerplateState.Options)
            Case SaveAndExitText
                Context.KeyBindings.Save()
                ReloadKeyBindings()
                SetState(BoilerplateState.Options)
            Case RestoreDefaultsText
                Context.KeyBindings.RestoreDefaults()
                OnStart()
            Case AddText
                SetState(BoilerplateState.AddKeyBinding)
            Case RemoveText
                SetState(BoilerplateState.RemoveKeyBinding)
            Case Else
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                (GoBackText, GoBackText),
                (SaveAndExitText, SaveAndExitText),
                (RestoreDefaultsText, RestoreDefaultsText),
                (AddText, AddText),
                (RemoveText, RemoveText)
            }
        Return result
    End Function
End Class
