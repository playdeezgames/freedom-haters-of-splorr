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
        starVicinity.Properties.Map = descriptor.CreateMap($"{starVicinity.Properties.Name} Vicinity", starVicinity.Universe)
        PlaceBoundaries(starVicinity, starLocation, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(starVicinity)
        addStep(New EncounterInitializationStep(starVicinity.Properties.Map), True)
    End Sub
    Private Sub PlaceStar(starVicinity As IPlace)
        Dim starColumn = starVicinity.Properties.Map.Size.Columns \ 2
        Dim starRow = starVicinity.Properties.Map.Size.Rows \ 2
        Dim locationType = StarTypes.Descriptors(starVicinity.Subtype).LocationType
        Dim location = starVicinity.Properties.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Place = starVicinity.Factory.CreateStar()
            .Tutorial = TutorialTypes.RefuelAtStar
            'TODO: initialize further down?
        End With
    End Sub
End Class
