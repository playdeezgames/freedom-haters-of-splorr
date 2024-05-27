Imports FHOS.Persistence

Friend MustInherit Class StationActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Friend Sub New(actorType As String, costumeGenerator As IReadOnlyDictionary(Of String, Integer), Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(actorType, costumeGenerator, spawnRolls)
    End Sub

    Protected MustOverride Function MakeName(planet As IActor) As String

    Protected Overrides Sub Initialize(actor As IActor)
        Dim planet = actor.State.Location.Map.GetCenterLocation().Actor
        actor.Properties.Name = MakeName(planet)
        actor.Properties.Group = planet.Properties.Group.Group

        actor.State.Location.LocationType = LocationTypes.ActorAdjacent
        For Each neighbor In actor.State.Location.GetEmptyNeighborsOfType(LocationTypes.Void)
            neighbor.LocationType = LocationTypes.ActorAdjacent
        Next
    End Sub
End Class
