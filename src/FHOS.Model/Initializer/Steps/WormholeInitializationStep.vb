Imports SPLORR.Game

Friend Class WormholeInitializationStep
    Inherits InitializationStep

    Private ReadOnly startLocation As Persistence.ILocation
    Private ReadOnly actor As Persistence.IActor

    Public Sub New(startLocation As Persistence.ILocation, actor As Persistence.IActor)
        Me.startLocation = startLocation
        Me.actor = actor
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim actorDescriptor = ActorTypes.Descriptors(ActorTypes.Wormhole)
        Dim destinationLocation = RNG.FromEnumerable(actor.State.Location.Map.Locations.Where(Function(x) actorDescriptor.CanSpawn(x)))
        destinationLocation.Actor = actorDescriptor.CreateActor(destinationLocation, "Wormhole")
        startLocation.Actor.Properties.TargetActor = destinationLocation.Actor
        destinationLocation.Actor.Properties.TargetActor = startLocation.Actor
    End Sub
End Class
