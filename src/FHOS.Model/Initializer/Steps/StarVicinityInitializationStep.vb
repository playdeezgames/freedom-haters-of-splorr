Imports FHOS.Persistence

Friend Class StarVicinityInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As ILocation
    Sub New(location As ILocation)
        Me.location = location
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim descriptor = MapTypes.Descriptors(MapTypes.StarVicinity)
        Dim actor = location.Actor
        Dim map = descriptor.CreateMap($"{actor.Properties.Name} Vicinity", actor.Universe)
        actor.Properties.Interior = map
        PlaceBoundaryActors(actor, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(actor)
        addStep(New EncounterInitializationStep(actor.Properties.Interior), True)
    End Sub
    Private Sub PlaceStar(externalActor As IActor)
        Dim starColumn = externalActor.Properties.Interior.Size.Columns \ 2
        Dim starRow = externalActor.Properties.Interior.Size.Rows \ 2
        Dim location = externalActor.Properties.Interior.GetLocation(starColumn, starRow)
        location.LocationType = LocationTypes.Star
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakeStar(externalActor.Descriptor.Subtype)).CreateActor(location, externalActor.Properties.Name)
        actor.Properties.Group = externalActor.Properties.Group
    End Sub
End Class
