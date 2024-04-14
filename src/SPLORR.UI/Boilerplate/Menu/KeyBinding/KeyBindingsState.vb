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
                SetState(BoilerplateState.Options)
            Case RestoreDefaultsText
                Context.KeyBindings.RestoreDefaults()
                OnStart()
            Case AddText
                SetState(BoilerplateState.AddKeyBinding)
            Case Else
                'TODO: give a message indicating that the keybinding can be removed or ask for confirmation that the key binding be removed
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                (GoBackText, GoBackText),
                (SaveAndExitText, SaveAndExitText),
                (RestoreDefaultsText, RestoreDefaultsText),
                (AddText, AddText)
            }
        result.AddRange(Context.KeyBindings.KeysTable.Select(Function(x) ($"{x.Key} -> [{x.Value}]", x.Key)))
        Return result
    End Function
End Class
