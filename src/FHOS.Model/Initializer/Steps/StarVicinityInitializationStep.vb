Imports FHOS.Persistence

Friend Class StarVicinityInitializationStep
    Inherits InitializationStep
    Private Const StarVicinityColumns = 15
    Private Const StarVicinityRows = 15
    Private ReadOnly starLocation As ILocation
    Sub New(location As ILocation)
        Me.starLocation = location
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim starVicinity = starLocation.Place
        starVicinity.Map = starVicinity.Universe.CreateMap(
            MapTypes.StarVicinity,
            $"{starVicinity.Name} Vicinity",
            StarVicinityColumns,
            StarVicinityRows,
            LocationTypes.Void)
        PlaceBoundaries(starVicinity, starLocation, StarVicinityColumns, StarVicinityRows)
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
End Class
