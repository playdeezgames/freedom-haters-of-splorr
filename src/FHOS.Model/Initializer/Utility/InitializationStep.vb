Imports FHOS.Persistence

Friend MustInherit Class InitializationStep
    MustOverride Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
    Protected Shared Sub PlaceBoundaries(place As IPlace, outsideLocation As ILocation, columns As Integer, rows As Integer)
        Dim teleporter = outsideLocation.CreateTeleporterTo()
        Dim identifier = place.Identifier
        For Each corner In GetCorners(columns, rows)
            PlaceBoundary(place.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(columns, rows)
            PlaceBoundary(place.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            place.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(identifier)
        Next
    End Sub
End Class
