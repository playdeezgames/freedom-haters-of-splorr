Imports FHOS.Model
Imports SPLORR.UI

Friend Class DeadState
    Inherits BaseMessageState(Of Model.IUniverseModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            BoilerplateState.MainMenu)
    End Sub

    Protected Overrides Function MessageHue() As Integer
        Return Hue.Red
    End Function

    Protected Overrides Function MessageText() As String
        Return "Yer Dead!"
    End Function
End Class
