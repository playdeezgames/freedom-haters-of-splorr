Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class PlanetVicinityInitializationStep
    Inherits InitializationStep
    Private Const PlanetVicinityColumns = 15
    Private Const PlanetVicinityRows = 15
    Private ReadOnly planetVicinityLocation As ILocation
    Private ReadOnly nameGenerator As NameGenerator
    Sub New(location As ILocation, nameGenerator As NameGenerator)
        Me.planetVicinityLocation = location
        Me.nameGenerator = nameGenerator
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim planetVicinity = planetVicinityLocation.Place
        planetVicinity.Map = planetVicinity.Universe.CreateMap(
            MapTypes.System,
            $"{planetVicinity.Name} Vicinity",
            PlanetVicinityColumns,
            PlanetVicinityRows,
            LocationTypes.Void)
        PlaceBoundaries(planetVicinity, planetVicinityLocation, PlanetVicinityColumns, PlanetVicinityRows)
        PlacePlanet(planetVicinity, addStep)
        planetVicinity.SatelliteCount = PlaceSatellites(planetVicinity, addStep)
    End Sub
    Private Function PlaceSatellites(planetVicinity As IPlace, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim satellites As List(Of (Column As Integer, Row As Integer)) =
            planetSectionDeltas.
            Select(Function(x) (x.DeltaX + PlanetVicinityColumns \ 2, x.DeltaY + PlanetVicinityRows \ 2)).
            ToList
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim planetType = PlanetTypes.Descriptors(planetVicinity.PlanetType)
        Dim MinimumDistance = planetType.MinimumSatelliteDistance
        Dim maximumSatelliteCount As Integer = planetType.GenerateMaximumSatelliteCount()
        Dim satelliteCount = 0
        While satelliteCount < maximumSatelliteCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, PlanetVicinityColumns - 3)
            Dim row = RNG.FromRange(1, PlanetVicinityRows - 3)
            If satellites.All(Function(satellite) (column - satellite.Column) * (column - satellite.Column) + (row - satellite.Row) * (row - satellite.Row) >= MinimumDistance * MinimumDistance) Then
                Dim satelliteType As String = planetType.GenerateSatelliteType()
                satellites.Add((column, row))
                Dim location = planetVicinity.Map.GetLocation(column, row)
                location.LocationType = SatelliteTypes.Descriptors(satelliteType).LocationType
                location.Tutorial = TutorialTypes.EnterSatelliteOrbit
                Dim satelliteName = nameGenerator.GenerateUnusedName
                location.Place = planetVicinity.CreateSatellite(satelliteName, satelliteType)
                addStep(New SatelliteInitializationStep(location), False)
                satelliteCount += 1
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return satelliteCount
    End Function
    Private ReadOnly planetSectionDeltas As IReadOnlyList(Of (DeltaX As Integer, DeltaY As Integer, SectionName As String)) =
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
    Private Sub PlacePlanet(planetVicinity As IPlace, addStep As Action(Of InitializationStep, Boolean))
        Dim planetCenterColumn = PlanetVicinityColumns \ 2
        Dim planetCenterRow = PlanetVicinityRows \ 2
        Dim planet = planetVicinity.CreatePlanet()
        For Each delta In planetSectionDeltas
            PlacePlanetSection(planet, planetVicinity.Map.GetLocation(planetCenterColumn + delta.DeltaX, planetCenterRow + delta.DeltaY), delta.SectionName)
        Next
        addStep(New PlanetInitializationStep(planetVicinity.Map.GetLocation(planetCenterColumn, planetCenterRow)), False)
    End Sub

    Private Shared Sub PlacePlanetSection(planet As IPlace, location As ILocation, sectionName As String)
        Dim locationType = PlanetTypes.Descriptors(planet.PlanetType).SectionLocationType(sectionName)
        With location
            .LocationType = locationType
            .Tutorial = TutorialTypes.EnterPlanetOrbit
            .Place = planet
        End With
    End Sub
End Class
