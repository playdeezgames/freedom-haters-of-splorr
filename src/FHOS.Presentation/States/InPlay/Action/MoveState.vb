Imports FHOS.Model
Imports SPLORR.UI

Friend Class MoveState
    Inherits BaseGameState(Of IUniverseModel)

    Private ReadOnly facing As Integer

    Public Sub New(
                  controller As FHOSController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of IUniverseModel),
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
            If .CanMove Then
                .SetFacing(facing)
                .Move(Persistence.Facing.Deltas(facing))
            End If
        End With
        SetState(BoilerplateState.Neutral)
    End Sub
End Class
