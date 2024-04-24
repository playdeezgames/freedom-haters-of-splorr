Imports System.Text
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module StarSystemInitializer
    Private Const SystemMapColumns = 31
    Private Const SystemMapRows = 31

    Friend Sub Initialize(starSystem As IStarSystem, starLocation As ILocation)
        starSystem.Map = starSystem.Universe.CreateMap(
            MapTypes.System,
            $"{starSystem.Name} System",
            SystemMapColumns,
            SystemMapRows,
            LocationTypes.Void)
        starSystem.Map.StarSystem = starSystem
        PlaceBoundaries(starSystem, starLocation)
        PlaceStar(starSystem)
        PlacePlanets(starSystem)
    End Sub

    Private Sub PlacePlanets(starSystem As IStarSystem)
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
                Dim planetName = $"{starSystem.Name} {Romanize(index)}"
                index += 1
                location.PlanetVicinity = starSystem.Universe.CreatePlanetVicinity(planetName, planetType)
                PlanetVicinityInitializer.Initialize(location.PlanetVicinity, location)
                tries = 0
            Else
                tries += 1
            End If
        End While
    End Sub

    Private ReadOnly romanizers As IReadOnlyList(Of (Value As Integer, Text As String)) =
        New List(Of (Value As Integer, Text As String)) From
        {
            (1000, "M"),
            (900, "CM"),
            (500, "D"),
            (400, "CD"),
            (100, "C"),
            (90, "XC"),
            (50, "L"),
            (40, "XL"),
            (10, "X"),
            (9, "IX"),
            (5, "V"),
            (4, "IV"),
            (1, "I")
        }

    Private Function Romanize(number As Integer) As String
        If number > 4999 OrElse number < 1 Then
            Throw New ArgumentOutOfRangeException()
        End If
        Dim builder As New StringBuilder
        For Each romanizer In romanizers
            While number >= romanizer.Value
                number -= romanizer.Value
                builder.Append(romanizer.Text)
            End While
        Next
        Return builder.ToString
    End Function

    Private Sub PlaceStar(starSystem As IStarSystem)
        Dim starColumn = SystemMapColumns \ 2
        Dim starRow = SystemMapRows \ 2
        Dim locationType = StarTypes.Descriptors(starSystem.StarType).LocationType
        Dim location = starSystem.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .StarVicinity = starSystem.Universe.CreateStarVicinity(starSystem.Name, starSystem.StarType)
            .Tutorial = TutorialTypes.StarVicinityApproach
            StarVicinityInitializer.Initialize(.StarVicinity, location)
        End With
    End Sub
    Private Sub PlaceBoundaries(starSystem As IStarSystem, starLocation As ILocation)
        Dim teleporter = starSystem.Universe.CreateTeleporter(starLocation)
        Dim starFlag = starSystem.Name
        For Each corner In GetCorners(SystemMapColumns, SystemMapRows)
            PlaceBoundary(starSystem.Map.GetLocation(corner.X, corner.Y), corner.LocationType, teleporter)
        Next
        For Each edge In GetEdges(SystemMapColumns, SystemMapRows)
            PlaceBoundary(starSystem.Map.GetLocation(edge.X, edge.Y), edge.LocationType, teleporter)
            starSystem.Map.GetLocation(edge.X + edge.DeltaX, edge.Y + edge.DeltaY).SetFlag(starFlag)
        Next
    End Sub
End Module
