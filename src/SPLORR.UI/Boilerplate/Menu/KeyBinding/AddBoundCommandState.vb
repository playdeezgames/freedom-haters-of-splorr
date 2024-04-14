Friend Class AddBoundCommandState(Of TModel)
    Inherits BasePickerState(Of TModel, String)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Choose Bound Command",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.AddKeyBinding)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case GoBackText
                SetState(AddKeyBinding)
            Case Else
                Context.KeyBindings.Bind(AddKeyBindingState(Of TModel).SelectedKey, value.Item)
                SetState(BoilerplateState.KeyBindings)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return New List(Of (Text As String, Item As String)) From
            {
                (GoBackText, GoBackText),
                (Command.A, Command.A),
                (Command.B, Command.B),
                (Command.Select, Command.Select),
                (Command.Start, Command.Start),
                (Command.Up, Command.Up),
                (Command.Down, Command.Down),
                (Command.Left, Command.Left),
                (Command.Right, Command.Right)
            }
    End Function
End Class
