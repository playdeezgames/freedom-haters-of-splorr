Friend Class SaveConfirmationState(Of TModel)
    Inherits BaseMessageState(Of TModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel))
        MyBase.New(
            parent,
            setState,
            context,
            BoilerplateState.GameMenu)
    End Sub

    Protected Overrides Function MessageHue() As Integer
        Return Context.UIPalette.MenuItem
    End Function

    Protected Overrides Function MessageText() As String
        Return "Game Saved."
    End Function
End Class
