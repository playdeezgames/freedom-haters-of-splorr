Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module PlanetInitializer
    Private Const PlanetVicinityColumns = 15
    Private Const PlanetVicinityRows = 15
    Friend Sub Initialize(planet As IPlanet, planetLocation As ILocation)
        planet.Map = planet.Universe.CreateMap(
            MapTypes.System,
            $"{planet.Name} Vicinity",
            PlanetVicinityColumns,
            PlanetVicinityRows,
            LocationTypes.Void)
        PlaceBoundaries(planet, planetLocation)
        PlacePlanet(planet)
        PlaceSatellites(planet)
    End Sub

    Private Sub PlaceSatellites(planet As IPlanet)
        Dim satellites As New List(Of (Column As Integer, Row As Integer)) From
            {
                (PlanetVicinityColumns \ 2, PlanetVicinityRows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim planetType = PlanetTypes.Descriptors(planet.PlanetType)
        Dim MinimumDistance = planetType.MinimumSatelliteDistance
        Dim index = 1
        While tries < MaximumTries
            Dim column = RNG.FromRange(1, PlanetVicinityColumns - 3)
            Dim row = RNG.FromRange(1, PlanetVicinityRows - 3)
            If satellites.All(Function(satellite) (column - satellite.Column) * (column - satellite.Column) + (row - satellite.Row) * (row - satellite.Row) >= MinimumDistance * MinimumDistance) Then
                Dim satelliteType As String = planetType.GenerateSatelliteType()
                satellites.Add((column, row))
                Dim location = planet.Map.GetLocation(column, row)
                location.LocationType = SatelliteTypes.Descriptors(satelliteType).LocationType
                location.Tutorial = TutorialTypes.SatelliteApproach
                Dim satelliteName = $"{planet.Name} {ChrW(AscW("A"c) + index)}"
                index += 1
                location.Satellite = planet.Universe.CreateSatellite(satelliteName, satelliteType)
                SatelliteInitializer.Initialize(location.Satellite, location)
                tries = 0
            Else
                tries += 1
            End If
        End While
    End Sub

    Private Sub PlacePlanet(planet As IPlanet)
        Dim starColumn = PlanetVicinityColumns \ 2
        Dim starRow = PlanetVicinityRows \ 2
        Dim locationType = PlanetTypes.Descriptors(planet.PlanetType).LocationType
        Dim location = planet.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            'TODO: create surface of planet
            .Tutorial = TutorialTypes.PlanetLand
            'TODO: initialize surface of planet
        End With
    End Sub

    Private Sub PlaceBoundaries(planet As IPlanet, planetLocation As ILocation)
        Dim teleporter = planet.Universe.CreateTeleporter(planetLocation)
        Dim starFlag = planet.Name
        For Each corner In GetCorners(PlanetVicinityColumns, PlanetVicinityRows)
            PlaceBoundary(planet.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(PlanetVicinityColumns, PlanetVicinityRows)
            PlaceBoundary(planet.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            planet.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(starFlag)
        Next
    End Sub
End Module
