Imports FHOS.Persistence

Friend Module StarVicinityInitializer
    Private Const StarVicinityColumns = 15
    Private Const StarVicinityRows = 15
    Friend Sub Initialize(star As IStarVicinity, starLocation As ILocation)
        star.Map = star.Universe.CreateMap(
            MapTypes.System,
            $"{star.Name} Vicinity",
            StarVicinityColumns,
            StarVicinityRows,
            LocationTypes.Void)
        PlaceBoundaries(star, starLocation)
        PlaceStar(star)
    End Sub
    Private Sub PlaceStar(starVicinity As IStarVicinity)
        Dim starColumn = StarVicinityColumns \ 2
        Dim starRow = StarVicinityRows \ 2
        Dim locationType = StarTypes.Descriptors(starVicinity.StarType).LocationType
        Dim location = starVicinity.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Star = starVicinity.Universe.CreateStar(starVicinity)
            .Tutorial = TutorialTypes.RefuelAtStar
            'TODO: initialize further down?
        End With
    End Sub
    Private Sub PlaceBoundaries(star As IStarVicinity, starLocation As ILocation)
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
