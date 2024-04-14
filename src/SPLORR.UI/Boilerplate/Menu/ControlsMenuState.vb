Friend Class ControlsMenuState(Of TModel)
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
            Case CancelText
                SetState(BoilerplateState.Options)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                (CancelText, CancelText),
                (SaveAndExitText, SaveAndExitText),
                (RestoreDefaultsText, RestoreDefaultsText)
            }
        Return result
    End Function
End Class
