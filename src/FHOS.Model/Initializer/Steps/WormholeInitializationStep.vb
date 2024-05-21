Imports SPLORR.Game

Friend Class WormholeInitializationStep
    Inherits InitializationStep

    Private ReadOnly startLocation As Persistence.ILocation
    Private ReadOnly starSystem As Persistence.IPlace

    Public Sub New(startLocation As Persistence.ILocation, starSystem As Persistence.IPlace)
        Me.startLocation = startLocation
        Me.starSystem = starSystem
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim wormhole = startLocation.Place
        Dim actorDescriptor = ActorTypes.Descriptors(ActorTypes.Wormhole)
        Dim destinationLocation = RNG.FromEnumerable(starSystem.Properties.Map.Locations.Where(Function(x) actorDescriptor.CanSpawn(x)))
        destinationLocation.Actor = actorDescriptor.CreateActor(destinationLocation)
        startLocation.Actor.Properties.TargetActor = destinationLocation.Actor
        destinationLocation.Actor.Properties.TargetActor = startLocation.Actor
    End Sub
End Class
