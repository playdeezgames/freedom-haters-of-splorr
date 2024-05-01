Imports System.Text
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class StarSystemInitializationStep
    Inherits InitializationStep
    Private Const SystemMapColumns = 31
    Private Const SystemMapRows = 31
    ReadOnly starLocation As ILocation
    Sub New(location As ILocation)
        Me.starLocation = location
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep))
        Dim starSystem = starLocation.StarSystem
        starSystem.Map = starSystem.Universe.CreateMap(
MapTypes.System,
            $"{starSystem.Name} System",
            SystemMapColumns,
            SystemMapRows,
LocationTypes.Void)
        starSystem.Map.StarSystem = starSystem
        PlaceBoundaries(starSystem, starLocation)
        PlaceStar(starSystem, addStep)
        PlacePlanets(starSystem, addStep)
    End Sub

    Private Sub PlacePlanets(starSystem As IStarSystem, addStep As Action(Of InitializationStep))
        Dim planets As New List(Of (Column As Integer, Row As Integer)) From
            {
                (SystemMapColumns \ 2, SystemMapRows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim starType = StarTypes.Descriptors(starSystem.StarType)
        Dim MinimumDistance = starType.MinimumPlanetaryDistance
        Dim index = 1
        While tries < MaximumTries
            Dim column = RNG.FromRange(1, SystemMapColumns - 3)
            Dim row = RNG.FromRange(1, SystemMapRows - 3)
            If planets.All(Function(planet) (column - planet.Column) * (column - planet.Column) + (row - planet.Row) * (row - planet.Row) >= MinimumDistance * MinimumDistance) Then
                Dim planetType = starType.GeneratePlanetType()
                planets.Add((column, row))
                Dim location = starSystem.Map.GetLocation(column, row)
                location.LocationType = PlanetTypes.Descriptors(planetType).LocationType
                location.Tutorial = TutorialTypes.PlanetVicinityApproach
                Dim planetName = Guid.NewGuid.ToString
                index += 1
                location.PlanetVicinity = starSystem.CreatePlanetVicinity(planetName, planetType)
                addStep(New PlanetVicinityInitializationStep(location))
                tries = 0
            Else
                tries += 1
            End If
        End While
    End Sub

    Private Sub PlaceStar(starSystem As IStarSystem, addStep As Action(Of InitializationStep))
        Dim starColumn = SystemMapColumns \ 2
        Dim starRow = SystemMapRows \ 2
        Dim locationType = StarTypes.Descriptors(starSystem.StarType).LocationType
        Dim location = starSystem.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .StarVicinity = starSystem.CreateStarVicinity()
            .Tutorial = TutorialTypes.StarVicinityApproach
            addStep(New StarVicinityInitializationStep(location))
        End With
    End Sub
    Private Sub PlaceBoundaries(starSystem As IStarSystem, starLocation As ILocation)
        Dim teleporter = starLocation.CreateTeleporterTo()
        Dim identifier = starSystem.Identifier
        For Each corner In GetCorners(SystemMapColumns, SystemMapRows)
            PlaceBoundary(starSystem.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(SystemMapColumns, SystemMapRows)
            PlaceBoundary(starSystem.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            starSystem.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(identifier)
        Next
    End Sub
End Class
