Public MustInherit Class BaseConfirmState(Of TModel)
    Inherits BasePickerState(Of TModel, Boolean)
    Protected ReadOnly _confirmGameState As String

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel),
                  headerText As String,
                  confirmGameState As String,
                  cancelGameState As String)
        MyBase.New(
            parent,
            setState,
            context,
            headerText,
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            cancelGameState)
        _confirmGameState = confirmGameState
    End Sub
    Protected Overrides Function InitializeMenuItems() As List(Of (String, Boolean))
        Return New List(Of (String, Boolean)) From
            {
                (NoText, False),
                (YesText, True)
            }
    End Function
    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As Boolean))
        Select Case value.Item
            Case False
                SetState(_cancelGameState)
            Case True
                OnConfirm()
                SetState(_confirmGameState)
        End Select
    End Sub

    Protected MustOverride Sub OnConfirm()
End Class
