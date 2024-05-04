Imports FHOS.Model
Imports SPLORR.UI

Friend Class ApproachStarState
    Inherits BaseGameState(Of Model.IUniverseModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        Context.Model.Avatar.DoVerb(VerbTypes.ApproachStarVicinity)
        SetState(BoilerplateState.Neutral)
    End Sub
End Class
