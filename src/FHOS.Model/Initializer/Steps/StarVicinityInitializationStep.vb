Imports FHOS.Persistence

Friend Class StarVicinityInitializationStep
    Inherits InitializationStep
    Private Const StarVicinityColumns = 15
    Private Const StarVicinityRows = 15
    Private ReadOnly starLocation As ILocation
    Sub New(location As ILocation)
        Me.starLocation = location
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep))
        Dim starVicinity = starLocation.StarVicinity
        starVicinity.Map = StarVicinity.Universe.CreateMap(
            MapTypes.System,
            $"{StarVicinity.Name} Vicinity",
            StarVicinityColumns,
            StarVicinityRows,
            LocationTypes.Void)
        StarVicinity.Map.StarVicinity = StarVicinity
        PlaceBoundaries(StarVicinity, starLocation)
        PlaceStar(StarVicinity)
    End Sub
    Private Sub PlaceStar(starVicinity As IStarVicinity)
        Dim starColumn = StarVicinityColumns \ 2
        Dim starRow = StarVicinityRows \ 2
        Dim locationType = StarTypes.Descriptors(starVicinity.StarType).LocationType
        Dim location = starVicinity.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Star = starVicinity.CreateStar()
            .Tutorial = TutorialTypes.RefuelAtStar
            'TODO: initialize further down?
        End With
    End Sub
    Private Sub PlaceBoundaries(star As IStarVicinity, starLocation As ILocation)
        Dim teleporter = starLocation.CreateTeleporterTo()
        Dim starFlag = star.Name
        For Each corner In GetCorners(StarVicinityColumns, StarVicinityRows)
            PlaceBoundary(star.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(StarVicinityColumns, StarVicinityRows)
            PlaceBoundary(star.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            star.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(starFlag)
        Next
    End Sub
End Class
