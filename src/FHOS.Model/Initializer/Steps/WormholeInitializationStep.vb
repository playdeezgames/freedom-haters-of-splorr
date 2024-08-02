Imports SPLORR.Game

Friend Class WormholeInitializationStep
    Inherits InitializationStep

    Private ReadOnly startLocation As Persistence.ILocation

    Public Sub New(startLocation As Persistence.ILocation)
        Me.startLocation = startLocation
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Initializing Wormhole..."
        End Get
    End Property

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim actorDescriptor = ActorTypes.Descriptors(ActorTypes.Wormhole)
        Dim starSystemActor = RNG.FromEnumerable(startLocation.Universe.Actors.Where(Function(x) x.Descriptor.IsStarSystem))
        Dim destinationLocation = RNG.FromEnumerable(starSystemActor.Interior.Locations.Where(Function(x) actorDescriptor.CanSpawn(x)))
        Dim wormholeActor = actorDescriptor.CreateActor(destinationLocation, "Wormhole")
        destinationLocation.Actor = wormholeActor
        startLocation.Actor.Yokes.Actor(YokeTypes.Target) = destinationLocation.Actor
        destinationLocation.Actor.Yokes.Actor(YokeTypes.Target) = startLocation.Actor
        Dim starSystemGroup = starSystemActor.Yokes.Group(YokeTypes.StarSystem)
        starSystemGroup.Statistics(StatisticTypes.WormholeCount) += 1
        wormholeActor.Yokes.Group(YokeTypes.StarSystem) = starSystemGroup
    End Sub
End Class
