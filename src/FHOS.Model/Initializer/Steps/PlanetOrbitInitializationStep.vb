Imports FHOS.Persistence

Friend Class PlanetOrbitInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As Persistence.ILocation
    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim planet = location.Place
        planet.Properties.Map = MapTypes.Descriptors(MapTypes.PlanetOrbit).CreateMap($"{planet.Properties.Name} Orbit", planet.Universe)
        PlaceBoundaries(planet, location, planet.Properties.Map.Size.Columns, planet.Properties.Map.Size.Rows)
        PlacePlanet(planet)
        addStep(New EncounterInitializationStep(planet.Properties.Map), True)
    End Sub
    Private ReadOnly planetSectionDeltas As IReadOnlyList(Of (DeltaX As Integer, DeltaY As Integer, SectionName As String)) =
        New List(Of (DeltaX As Integer, DeltaY As Integer, SectionName As String)) From
        {
            (-2, -2, Grid5x5.C1R1),
            (-1, -2, Grid5x5.C2R1),
            (0, -2, Grid5x5.C3R1),
            (1, -2, Grid5x5.C4R1),
            (2, -2, Grid5x5.C5R1),
            (-2, -1, Grid5x5.C1R2),
            (-1, -1, Grid5x5.C2R2),
            (0, -1, Grid5x5.C3R2),
            (1, -1, Grid5x5.C4R2),
            (2, -1, Grid5x5.C5R2),
            (-2, 0, Grid5x5.C1R3),
            (-1, 0, Grid5x5.C2R3),
            (0, 0, Grid5x5.C3R3),
            (1, 0, Grid5x5.C4R3),
            (2, 0, Grid5x5.C5R3),
            (-2, 1, Grid5x5.C1R4),
            (-1, 1, Grid5x5.C2R4),
            (0, 1, Grid5x5.C3R4),
            (1, 1, Grid5x5.C4R4),
            (2, 1, Grid5x5.C5R4),
            (-2, 2, Grid5x5.C1R5),
            (-1, 2, Grid5x5.C2R5),
            (0, 2, Grid5x5.C3R5),
            (1, 2, Grid5x5.C4R5),
            (2, 2, Grid5x5.C5R5)
        }
    Private Sub PlacePlanet(planet As IPlace)
        Dim planetCenterColumn = planet.Properties.Map.Size.Columns \ 2
        Dim planetCenterRow = planet.Properties.Map.Size.Rows \ 2
        For Each delta In planetSectionDeltas
            PlacePlanetSection(planet, planet.Properties.Map.GetLocation(planetCenterColumn + delta.DeltaX, planetCenterRow + delta.DeltaY), delta.SectionName)
        Next
    End Sub
    Private Shared Sub PlacePlanetSection(place As IPlace, location As ILocation, sectionName As String)
        Dim locationType = PlanetTypes.Descriptors(place.Subtype).SectionLocationType(sectionName)
        With location
            .LocationType = locationType
            .Place = place
        End With
    End Sub
End Class
