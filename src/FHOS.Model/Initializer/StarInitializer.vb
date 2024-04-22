Imports FHOS.Persistence

Friend Module StarInitializer
    Private Const StarVicinityColumns = 15
    Private Const StarVicinityRows = 15
    Friend Sub Initialize(star As IStar, starLocation As ILocation)
        star.Map = star.Universe.CreateMap(
            MapTypes.System,
            $"{star.Name} Vicinity",
            StarVicinityColumns,
            StarVicinityRows,
            LocationTypes.Void)
        PlaceBoundaries(star, starLocation)
        PlaceStar(star)
    End Sub
    Private Sub PlaceStar(star As IStar)
        Dim starColumn = StarVicinityColumns \ 2
        Dim starRow = StarVicinityRows \ 2
        Dim locationType = StarTypes.Descriptors(star.StarType).LocationType
        Dim location = star.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            'TODO: create surface of planet
            'TODO: Tutorial?
            'TODO: initialize surface of planet
        End With
    End Sub
    Private Sub PlaceBoundaries(star As IStar, starLocation As ILocation)
        Dim teleporter = star.Universe.CreateTeleporter(starLocation)
        Dim starFlag = star.Name
        For Each corner In GetCorners(StarVicinityColumns, StarVicinityRows)
            PlaceBoundary(star.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(StarVicinityColumns, StarVicinityRows)
            PlaceBoundary(star.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            star.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(starFlag)
        Next
    End Sub
End Module
