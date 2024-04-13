Imports FHOS.Model
Imports SPLORR.UI

Friend Class MoveState
    Inherits BaseGameState(Of IWorldModel)

    Private ReadOnly facing As Integer

    Public Sub New(
                  controller As FHOSController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IWorldModel),
                  facing As Integer)
        MyBase.New(controller, setState, context)
        Me.facing = facing
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        With Context.Model.Avatar
            .SetFacing(facing)
            .Move(Persistence.Facing.Deltas(facing))
        End With
        SetState(BoilerplateState.Neutral)
    End Sub
End Class
