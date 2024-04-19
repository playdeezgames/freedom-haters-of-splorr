Friend Class ConfirmAbandonState(Of TModel)
    Inherits BaseConfirmState(Of TModel)
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Are you sure you want to abandon the game?",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.GameMenu)
    End Sub
    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As Boolean))
        Select Case value.Item
            Case False
                SetState(BoilerplateState.GameMenu)
            Case True
                Context.AbandonGame()
                SetState(BoilerplateState.MainMenu)
        End Select
    End Sub
    Protected Overrides Function InitializeMenuItems() As List(Of (String, Boolean))
        Return New List(Of (String, Boolean)) From
            {
                (NoText, False),
                (YesText, True)
            }
    End Function
End Class
