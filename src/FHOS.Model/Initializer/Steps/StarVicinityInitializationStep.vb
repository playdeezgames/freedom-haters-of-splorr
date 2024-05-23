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
    Private Sub PlaceStar(actor As IActor)
        Dim starColumn = actor.Properties.Interior.Size.Columns \ 2
        Dim starRow = actor.Properties.Interior.Size.Rows \ 2
        Dim locationType = LocationTypes.MakeStar(actor.Properties.Subtype)
        Dim location = actor.Properties.Interior.GetLocation(starColumn, starRow)
        ActorTypes.Descriptors(ActorTypes.MakeStar(actor.Properties.Subtype)).CreateActor(location, actor.Properties.Name)
    End Sub
End Class
