Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class StarSystemInitializationStep
    Inherits InitializationStep
    Private ReadOnly starLocation As ILocation
    Private ReadOnly nameGenerator As NameGenerator
    Sub New(location As ILocation, nameGenerator As NameGenerator)
        Me.starLocation = location
        Me.nameGenerator = nameGenerator
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim descriptor = MapTypes.Descriptors(MapTypes.StarSystem)
        Dim starSystem = starLocation.Place
        starSystem.Map = descriptor.CreateMap($"{starSystem.Name} System", starSystem.Universe)
        PlaceBoundaries(starSystem, starLocation, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(starSystem, addStep)
        starSystem.PlanetVicinityCount = PlacePlanets(starSystem, addStep)
    End Sub

    Private Function PlacePlanets(starSystem As IPlace, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim planets As New List(Of (Column As Integer, Row As Integer)) From
            {
                (starSystem.Map.Size.Columns \ 2, starSystem.Map.Size.Rows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim starType = StarTypes.Descriptors(starSystem.StarType)
        Dim MinimumDistance = starType.MinimumPlanetaryDistance
        Dim index = 1
        Dim maximumPlanetCount As Integer = starType.GenerateMaximumPlanetCount()
        Dim planetCount = 0
        While planetCount < maximumPlanetCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, starSystem.Map.Size.Columns - 3)
            Dim row = RNG.FromRange(1, starSystem.Map.Size.Rows - 3)
            If planets.All(Function(planet) (column - planet.Column) * (column - planet.Column) + (row - planet.Row) * (row - planet.Row) >= MinimumDistance * MinimumDistance) Then
                Dim planetType = starType.GeneratePlanetType()
                planets.Add((column, row))
                Dim location = starSystem.Map.GetLocation(column, row)
                location.LocationType = PlanetTypes.Descriptors(planetType).LocationType
                location.Tutorial = TutorialTypes.PlanetVicinityApproach
                Dim planetName = nameGenerator.GenerateUnusedName
                index += 1
                location.Place = starSystem.CreatePlanetVicinity(planetName, planetType, column, row)
                addStep(New PlanetVicinityInitializationStep(location, nameGenerator), False)
                planetCount += 1
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return planetCount
    End Function

    Private Sub PlaceStar(starSystem As IPlace, addStep As Action(Of InitializationStep, Boolean))
        Dim starColumn = starSystem.Map.Size.Columns \ 2
        Dim starRow = starSystem.Map.Size.Rows \ 2
        Dim locationType = StarTypes.Descriptors(starSystem.StarType).LocationType
        Dim location = starSystem.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Place = starSystem.CreateStarVicinity(starColumn, starRow)
            .Tutorial = TutorialTypes.StarVicinityApproach
            addStep(New StarVicinityInitializationStep(location), False)
        End With
    End Sub
End Class
