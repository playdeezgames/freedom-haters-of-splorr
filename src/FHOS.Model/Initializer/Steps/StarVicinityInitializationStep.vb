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
        Dim starVicinity = starLocation.Place
        starVicinity.Map = starVicinity.Universe.CreateMap(
            MapTypes.System,
            $"{starVicinity.Name} Vicinity",
            StarVicinityColumns,
            StarVicinityRows,
            LocationTypes.Void)
        starVicinity.Map.Place = starVicinity
        PlaceBoundaries(starVicinity, starLocation)
        PlaceStar(starVicinity)
    End Sub
    Private Sub PlaceStar(starVicinity As IPlace)
        Dim starColumn = StarVicinityColumns \ 2
        Dim starRow = StarVicinityRows \ 2
        Dim locationType = StarTypes.Descriptors(starVicinity.StarType).LocationType
        Dim location = starVicinity.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Place = starVicinity.CreateStar()
            .Tutorial = TutorialTypes.RefuelAtStar
            'TODO: initialize further down?
        End With
    End Sub
    Private Sub PlaceBoundaries(starVicinity As IPlace, starLocation As ILocation)
        Dim teleporter = starLocation.CreateTeleporterTo()
        Dim identifier = starVicinity.Identifier
        For Each corner In GetCorners(StarVicinityColumns, StarVicinityRows)
            PlaceBoundary(starVicinity.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(StarVicinityColumns, StarVicinityRows)
            PlaceBoundary(starVicinity.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            starVicinity.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(identifier)
        Next
    End Sub
End Class
