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
        Dim destinationLocation = RNG.FromEnumerable(starSystemActor.Properties.Interior.Locations.Where(Function(x) actorDescriptor.CanSpawn(x)))
        destinationLocation.Actor = actorDescriptor.CreateActor(destinationLocation, "Wormhole")
        startLocation.Actor.YokedActor(YokeTypes.Target) = destinationLocation.Actor
        destinationLocation.Actor.YokedActor(YokeTypes.Target) = startLocation.Actor
        starSystemActor.YokedGroup(YokeTypes.StarSystem).Statistics(StatisticTypes.WormholeCount) += 1
    End Sub
End Class
