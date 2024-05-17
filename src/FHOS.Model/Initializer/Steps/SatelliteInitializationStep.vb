Imports FHOS.Persistence

Friend Class SatelliteInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As Persistence.ILocation

    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim satellite = location.Place
        satellite.Map = MapTypes.Descriptors(MapTypes.SatelliteOrbit).CreateMap($"{satellite.Name} Orbit", satellite.Universe)
        PlaceBoundaries(satellite, location, satellite.Map.Size.Columns, satellite.Map.Size.Rows)
        PlaceSatellite(satellite)
    End Sub
    Private ReadOnly satelliteSectionDeltas As IReadOnlyList(Of (DeltaX As Integer, DeltaY As Integer, SectionName As String)) =
        New List(Of (DeltaX As Integer, DeltaY As Integer, SectionName As String)) From
        {
            (-1, -1, LocationTypes.TopLeft),
            (0, -1, LocationTypes.TopCenter),
            (1, -1, LocationTypes.TopRight),
            (-1, 0, LocationTypes.CenterLeft),
            (0, 0, LocationTypes.Center),
            (1, 0, LocationTypes.CenterRight),
            (-1, 1, LocationTypes.BottomLeft),
            (0, 1, LocationTypes.BottomCenter),
            (1, 1, LocationTypes.BottomRight)
        }

    Private Sub PlaceSatellite(satellite As IPlace)
        Dim planetCenterColumn = satellite.Map.Size.Columns \ 2
        Dim planetCenterRow = satellite.Map.Size.Rows \ 2
        For Each delta In satelliteSectionDeltas
            PlaceSatelliteSection(satellite.Subtype, satellite.Map.GetLocation(planetCenterColumn + delta.DeltaX, planetCenterRow + delta.DeltaY), delta.SectionName)
        Next
    End Sub

    Private Sub PlaceSatelliteSection(satelliteType As String, location As ILocation, sectionName As String)
        Dim locationType = SatelliteTypes.Descriptors(satelliteType).SectionLocationType(sectionName)
        With location
            .LocationType = locationType
        End With
    End Sub
End Class
