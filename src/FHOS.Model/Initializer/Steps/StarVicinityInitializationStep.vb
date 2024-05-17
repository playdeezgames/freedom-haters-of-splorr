Imports FHOS.Persistence

Friend Class StarVicinityInitializationStep
    Inherits InitializationStep
    Private ReadOnly starLocation As ILocation
    Sub New(location As ILocation)
        Me.starLocation = location
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim descriptor = MapTypes.Descriptors(MapTypes.StarVicinity)
        Dim starVicinity = starLocation.Place
        starVicinity.Map = descriptor.CreateMap($"{starVicinity.Name} Vicinity", starVicinity.Universe)
        PlaceBoundaries(starVicinity, starLocation, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(starVicinity)
    End Sub
    Private Sub PlaceStar(starVicinity As IPlace)
        Dim starColumn = starVicinity.Map.Size.Columns \ 2
        Dim starRow = starVicinity.Map.Size.Rows \ 2
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
