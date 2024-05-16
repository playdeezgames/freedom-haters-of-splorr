Imports FHOS.Model
Imports SPLORR.UI

Friend Class GameOverState
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
        Return Hues.Red
    End Function

    Protected Overrides Function MessageText() As String
        If Context.Model.State.Avatar.Status.Dead Then
            Return "Yer Dead!"
        ElseIf Context.Model.State.Avatar.Status.Bankrupt Then
            Return "Yer Bankrupt!"
        End If
        Throw New NotImplementedException
    End Function
End Class
