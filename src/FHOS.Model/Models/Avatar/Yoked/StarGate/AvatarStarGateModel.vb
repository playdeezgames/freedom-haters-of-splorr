Imports SPLORR.Game

Friend Class AvatarStarGateModel
    Inherits BaseAvatarModel
    Implements IAvatarStarGateModel

    Protected Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarStarGateModel.IsActive
        Get
            Return actor.Yokes.Actor(YokeTypes.StarGate) IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property AvailableGates As IEnumerable(Of (Text As String, Item As IActorModel)) Implements IAvatarStarGateModel.AvailableGates
        Get
            Dim faction = actor.Yokes.Group(YokeTypes.Faction)
            Dim gates = actor.Universe.Actors.Where(Function(x) ActorTypes.Descriptors(x.EntityType).IsStarGate AndAlso x.Yokes.Group(YokeTypes.Faction).Id = faction.Id)
            Return gates.Select(Function(x) (x.EntityName, ActorModel.FromActor(x)))
        End Get
    End Property

    Public Sub Enter(gate As IActorModel) Implements IAvatarStarGateModel.Enter
        actor.GoToOtherActor(
            ActorModel.GetActor(gate),
            Sub(success, otherActor)
                If Not success Then
                    actor.Universe.Messages.Add("Destination Blocked!", ("The other end is blocked!", Hues.Red))
                Else
                    actor.SetStarSystem(otherActor.Yokes.Group(YokeTypes.StarSystem))
                End If
                actor.Yokes.Actor(YokeTypes.StarGate) = Nothing
            End Sub)
    End Sub

    Public Sub Leave() Implements IAvatarStarGateModel.Leave
        actor.Yokes.Actor(YokeTypes.StarGate) = Nothing
    End Sub

    Friend Shared Function FromActor(actor As Persistence.IActor) As IAvatarStarGateModel
        Return New AvatarStarGateModel(actor)
    End Function
End Class
