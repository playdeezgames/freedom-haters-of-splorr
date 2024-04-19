Public MustInherit Class BaseConfirmState(Of TModel)
    Inherits BasePickerState(Of TModel, Boolean)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of TModel), headerText As String, statusBarText As String, cancelGameState As String)
        MyBase.New(parent, setState, context, headerText, statusBarText, cancelGameState)
    End Sub
End Class
