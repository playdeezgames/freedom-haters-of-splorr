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
        starSystem.Properties.Map = descriptor.CreateMap($"{starSystem.Properties.Name} System", starSystem.Universe)
        PlaceBoundaries(starSystem, starLocation, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(starSystem, addStep)
        starSystem.Family.PlanetCount = PlacePlanets(starSystem, addStep)
        addStep(New EncounterInitializationStep(starSystem.Properties.Map), True)
    End Sub

    Private Function PlacePlanets(starSystem As IPlace, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim planets As New List(Of (Column As Integer, Row As Integer)) From
            {
                (starSystem.Properties.Map.Size.Columns \ 2, starSystem.Properties.Map.Size.Rows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim starType = StarTypes.Descriptors(starSystem.Subtype)
        Dim MinimumDistance = starType.MinimumPlanetaryDistance
        Dim index = 1
        Dim maximumPlanetCount As Integer = starType.GenerateMaximumPlanetCount()
        Dim planetCount = 0
        While planetCount < maximumPlanetCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, starSystem.Properties.Map.Size.Columns - 3)
            Dim row = RNG.FromRange(1, starSystem.Properties.Map.Size.Rows - 3)
            If planets.All(Function(planet) (column - planet.Column) * (column - planet.Column) + (row - planet.Row) * (row - planet.Row) >= MinimumDistance * MinimumDistance) Then
                Dim planetType = starType.GeneratePlanetType()
                planets.Add((column, row))
                Dim location = starSystem.Properties.Map.GetLocation(column, row)
                location.LocationType = PlanetTypes.Descriptors(planetType).LocationType
                location.Tutorial = TutorialTypes.PlanetVicinityApproach
                Dim planetName = nameGenerator.GenerateUnusedName
                index += 1
                location.Place = starSystem.Factory.CreatePlanetVicinity(planetName, planetType, column, row)
                ActorTypes.Descriptors(ActorTypes.MakePlanetVicinity(planetType)).CreateActor(location, planetType)
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
        Dim starColumn = starSystem.Properties.Map.Size.Columns \ 2
        Dim starRow = starSystem.Properties.Map.Size.Rows \ 2
        Dim locationType = LocationTypes.MakeStar(starSystem.Subtype)
        Dim location = starSystem.Properties.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Place = starSystem.Factory.CreateStarVicinity(starColumn, starRow)
            .Tutorial = TutorialTypes.StarVicinityApproach
            addStep(New StarVicinityInitializationStep(location), False)
        End With
    End Sub
End Class
