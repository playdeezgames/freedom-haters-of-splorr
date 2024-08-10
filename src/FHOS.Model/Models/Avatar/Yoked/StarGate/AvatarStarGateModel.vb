Imports SPLORR.Game

Friend Class AvatarStarGateModel
    Inherits AvatarYokedModel
    Implements IAvatarStarGateModel

    Protected Sub New(actor As Persistence.IActor)
        MyBase.New(actor, YokeTypes.StarGate)
    End Sub

    Public ReadOnly Property AvailableGates As IEnumerable(Of (Text As String, Item As IActorModel)) Implements IAvatarStarGateModel.AvailableGates
        Get
            Dim faction = YokedActor.Yokes.Group(YokeTypes.Faction)
            Dim gates = actor.Universe.Actors.Where(Function(x) ActorTypes.Descriptors(x.EntityType).Flag(FlagTypes.IsStarGate) AndAlso x.Yokes.Group(YokeTypes.Faction).Id = faction.Id)
            Return gates.Select(Function(x) (x.EntityName, ActorModel.FromActor(x)))
        End Get
    End Property

    Public Sub Enter(gate As IActorModel) Implements IAvatarStarGateModel.Enter
        actor.GoToOtherActor(
            ActorModel.GetActor(gate),
            Sub(success, otherActor)
                If Not success Then
                    actor.Dialog = New DestinationBlockedDialog(actor, actor.Dialog)
                Else
                    actor.SetStarSystem(otherActor.Yokes.Group(YokeTypes.StarSystem))
                End If
                actor.Yokes.Actor(YokeTypes.StarGate) = Nothing
            End Sub)
    End Sub

    Protected Overrides Sub OnLeave()
        'do nothing
    End Sub

    Friend Shared Function FromActor(actor As Persistence.IActor) As IAvatarStarGateModel
        Return New AvatarStarGateModel(actor)
    End Function
End Class
