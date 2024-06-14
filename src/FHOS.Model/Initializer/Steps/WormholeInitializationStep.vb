Imports SPLORR.Game

Friend Class WormholeInitializationStep
    Inherits InitializationStep

    Private ReadOnly startLocation As Persistence.ILocation

    Public Sub New(startLocation As Persistence.ILocation)
        Me.startLocation = startLocation
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim actorDescriptor = ActorTypes.Descriptors(ActorTypes.Wormhole)
        Dim starSystemActor = RNG.FromEnumerable(startLocation.Universe.Actors.Where(Function(x) x.Descriptor.IsStarSystem))
        Dim destinationLocation = RNG.FromEnumerable(starSystemActor.Interior.Locations.Where(Function(x) actorDescriptor.CanSpawn(x)))
        Dim wormholeActor = actorDescriptor.CreateActor(destinationLocation, "Wormhole")
        destinationLocation.Actor = wormholeActor
        startLocation.Actor.Yokes.YokedActor(YokeTypes.Target) = destinationLocation.Actor
        destinationLocation.Actor.Yokes.YokedActor(YokeTypes.Target) = startLocation.Actor
        Dim starSystemGroup = starSystemActor.Yokes.YokedGroup(YokeTypes.StarSystem)
        starSystemGroup.Statistics(StatisticTypes.WormholeCount) += 1
        wormholeActor.Yokes.YokedGroup(YokeTypes.StarSystem) = starSystemGroup
    End Sub
End Class
