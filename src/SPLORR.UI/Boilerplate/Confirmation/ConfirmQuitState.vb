Friend Class ConfirmQuitState(Of TModel)
    Inherits BasePickerState(Of TModel, String)
    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of TModel))
        MyBase.New(parent, setState, context, "Are you sure you want to quit the game?", context.ControlsText("Choose", "Cancel"), BoilerplateState.MainMenu)
    End Sub
    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case NoText
                SetState(BoilerplateState.MainMenu)
            Case YesText
                SetState(Nothing)
        End Select
    End Sub
    Protected Overrides Function InitializeMenuItems() As List(Of (String, String))
        Return New List(Of (String, String)) From
            {
                (NoText, NoText),
                (YesText, YesText)
            }
    End Function
End Class
