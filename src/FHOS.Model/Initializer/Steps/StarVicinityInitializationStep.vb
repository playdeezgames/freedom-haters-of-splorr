Imports FHOS.Persistence

Friend Class StarVicinityInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As ILocation
    Sub New(location As ILocation)
        Me.location = location
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim descriptor = MapTypes.Descriptors(MapTypes.StarVicinity)
        Dim place = location.Place
        'Dim actor = location.Actor
        Dim map = descriptor.CreateMap($"{place.Properties.Name} Vicinity", place.Universe)
        place.Properties.Map = map
        'actor.Properties.Interior = map
        PlaceBoundaries(place, location, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(place)
        addStep(New EncounterInitializationStep(place.Properties.Map), True)
    End Sub
    Private Sub PlaceStar(starVicinity As IPlace)
        Dim starColumn = starVicinity.Properties.Map.Size.Columns \ 2
        Dim starRow = starVicinity.Properties.Map.Size.Rows \ 2
        Dim locationType = LocationTypes.MakeStar(starVicinity.Subtype)
        Dim location = starVicinity.Properties.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Place = starVicinity.Factory.CreateStar()
            .Tutorial = TutorialTypes.RefuelAtStar
            'TODO: initialize further down?
        End With
    End Sub
End Class
