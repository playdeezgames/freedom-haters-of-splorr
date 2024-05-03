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
        planetVicinity.Map.Place = planetVicinity
        PlaceBoundaries(planetVicinity, planetVicinityLocation)
        PlacePlanet(planetVicinity, addStep)
        PlaceSatellites(planetVicinity, addStep)
    End Sub
    Private Sub PlaceSatellites(planetVicinity As IPlace, addStep As Action(Of InitializationStep, Boolean))
        Dim satellites As New List(Of (Column As Integer, Row As Integer)) From
            {
                (PlanetVicinityColumns \ 2, PlanetVicinityRows \ 2)
            }
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
                location.Tutorial = TutorialTypes.SatelliteApproach
                Dim satelliteName = nameGenerator.GenerateUnusedName
                location.Place = planetVicinity.CreateSatellite(satelliteName, satelliteType)
                addStep(New SatelliteInitializationStep(location), False)
                satelliteCount += 1
                tries = 0
            Else
                tries += 1
            End If
        End While
    End Sub
    Private Sub PlacePlanet(planetVicinity As IPlace, addStep As Action(Of InitializationStep, Boolean))
        Dim starColumn = PlanetVicinityColumns \ 2
        Dim starRow = PlanetVicinityRows \ 2
        Dim locationType = PlanetTypes.Descriptors(planetVicinity.PlanetType).LocationType
        Dim location = planetVicinity.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Tutorial = TutorialTypes.PlanetLand
            .Place = planetVicinity.CreatePlanet()
            addStep(New PlanetInitializationStep(location), False)
        End With
    End Sub
    Private Sub PlaceBoundaries(planetVicinity As IPlace, planetVicinityLocation As ILocation)
        Dim teleporter = planetVicinityLocation.CreateTeleporterTo()
        Dim identifier = planetVicinity.Identifier
        For Each corner In GetCorners(PlanetVicinityColumns, PlanetVicinityRows)
            PlaceBoundary(planetVicinity.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(PlanetVicinityColumns, PlanetVicinityRows)
            PlaceBoundary(planetVicinity.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            planetVicinity.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(identifier)
        Next
    End Sub
End Class
