﻿Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module PlanetVicinityInitializer
    Private Const PlanetVicinityColumns = 15
    Private Const PlanetVicinityRows = 15
    Friend Sub Initialize(planetVicinity As IPlanetVicinity, planetVicinityLocation As ILocation)
        planetVicinity.Map = planetVicinity.Universe.CreateMap(
            MapTypes.System,
            $"{planetVicinity.Name} Vicinity",
            PlanetVicinityColumns,
            PlanetVicinityRows,
            LocationTypes.Void)
        PlaceBoundaries(planetVicinity, planetVicinityLocation)
        PlacePlanet(planetVicinity)
        PlaceSatellites(planetVicinity)
    End Sub

    Private Sub PlaceSatellites(planetVicinity As IPlanetVicinity)
        Dim satellites As New List(Of (Column As Integer, Row As Integer)) From
            {
                (PlanetVicinityColumns \ 2, PlanetVicinityRows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim planetType = PlanetTypes.Descriptors(planetVicinity.PlanetType)
        Dim MinimumDistance = planetType.MinimumSatelliteDistance
        Dim index = 1
        While tries < MaximumTries
            Dim column = RNG.FromRange(1, PlanetVicinityColumns - 3)
            Dim row = RNG.FromRange(1, PlanetVicinityRows - 3)
            If satellites.All(Function(satellite) (column - satellite.Column) * (column - satellite.Column) + (row - satellite.Row) * (row - satellite.Row) >= MinimumDistance * MinimumDistance) Then
                Dim satelliteType As String = planetType.GenerateSatelliteType()
                satellites.Add((column, row))
                Dim location = planetVicinity.Map.GetLocation(column, row)
                location.LocationType = SatelliteTypes.Descriptors(satelliteType).LocationType
                location.Tutorial = TutorialTypes.SatelliteApproach
                Dim satelliteName = $"{planetVicinity.Name} {ChrW(AscW("A"c) + index)}"
                index += 1
                location.Satellite = planetVicinity.Universe.CreateSatellite(satelliteName, satelliteType)
                SatelliteInitializer.Initialize(location.Satellite, location)
                tries = 0
            Else
                tries += 1
            End If
        End While
    End Sub

    Private Sub PlacePlanet(planetVicinity As IPlanetVicinity)
        Dim starColumn = PlanetVicinityColumns \ 2
        Dim starRow = PlanetVicinityRows \ 2
        Dim locationType = PlanetTypes.Descriptors(planetVicinity.PlanetType).LocationType
        Dim location = planetVicinity.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            'TODO: create  planet
            .Tutorial = TutorialTypes.PlanetLand
            'TODO: initialize planet
            .Planet = planetVicinity.Universe.CreatePlanet(planetVicinity.Name, planetVicinity.PlanetType)
            PlanetInitializer.Initialize(.Planet)
        End With
    End Sub

    Private Sub PlaceBoundaries(planetVicinity As IPlanetVicinity, planetVicinityLocation As ILocation)
        Dim teleporter = planetVicinity.Universe.CreateTeleporter(planetVicinityLocation)
        Dim starFlag = planetVicinity.Name
        For Each corner In GetCorners(PlanetVicinityColumns, PlanetVicinityRows)
            PlaceBoundary(planetVicinity.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(PlanetVicinityColumns, PlanetVicinityRows)
            PlaceBoundary(planetVicinity.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            planetVicinity.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(starFlag)
        Next
    End Sub
End Module