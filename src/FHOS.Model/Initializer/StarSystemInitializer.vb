Imports System.Text
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module StarSystemInitializer
    Private Const SystemMapColumns = 31
    Private Const SystemMapRows = 31

    Friend Sub Initialize(starSystem As IStarSystem, starLocation As ILocation, starFlag As String)
        starSystem.Map = starSystem.Universe.CreateMap(
            MapTypes.System,
            $"{starSystem.Name} System",
            SystemMapColumns,
            SystemMapRows,
            LocationTypes.Void)
        PlaceSystemBoundaries(starSystem, starLocation, starFlag)
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
                location.Tutorial = TutorialTypes.PlanetaryEntry
                Dim planetName = $"{starSystem.Name} {Romanize(index)}"
                index += 1
                location.Planet = starSystem.Universe.CreatePlanet(planetName, planetType)
                PlanetInitializer.Initialize(location.Planet)
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
        With starSystem.Map.GetLocation(starColumn, starRow)
            .LocationType = locationType
            .Star = starSystem.Universe.CreateStar(starSystem.Name, starSystem.StarType)
            .Tutorial = TutorialTypes.StarApproach
            StarInitializer.Initialize(.Star)
        End With
    End Sub

    Private Sub PlaceSystemBoundaries(starSystem As IStarSystem, starLocation As ILocation, starFlag As String)
        Dim teleporter = starSystem.Universe.CreateTeleporter(starLocation)

        With starSystem.Map.GetLocation(0, 0)
            .LocationType = VoidNorthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With

        With starSystem.Map.GetLocation(SystemMapColumns - 1, 0)
            .LocationType = VoidNorthEastArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetLocation(0, SystemMapRows - 1)
            .LocationType = VoidSouthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetLocation(0, SystemMapRows - 1)
            .LocationType = VoidSouthWestArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        With starSystem.Map.GetLocation(SystemMapColumns - 1, SystemMapRows - 1)
            .LocationType = VoidSouthEastArrow
            .Teleporter = teleporter
            .SetFlag(starFlag)
        End With
        For Each row In Enumerable.Range(1, SystemMapRows - 2)
            With starSystem.Map.GetLocation(0, row)
                .Teleporter = teleporter
                .LocationType = VoidWestArrow
            End With
            With starSystem.Map.GetLocation(1, row)
                .SetFlag(starFlag)
            End With
            With starSystem.Map.GetLocation(SystemMapColumns - 1, row)
                .Teleporter = teleporter
                .LocationType = VoidEastArrow
            End With
            With starSystem.Map.GetLocation(SystemMapColumns - 2, row)
                .SetFlag(starFlag)
            End With
        Next
        For Each column In Enumerable.Range(1, SystemMapColumns - 2)
            With starSystem.Map.GetLocation(column, 0)
                .Teleporter = teleporter
                .LocationType = VoidNorthArrow
            End With
            With starSystem.Map.GetLocation(column, 1)
                .SetFlag(starFlag)
            End With
            With starSystem.Map.GetLocation(column, SystemMapRows - 1)
                .Teleporter = teleporter
                .LocationType = VoidSouthArrow
            End With
            With starSystem.Map.GetLocation(column, SystemMapRows - 2)
                .SetFlag(starFlag)
            End With
        Next
    End Sub
End Module
