Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class StarSystemInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As ILocation
    Private ReadOnly nameGenerator As NameGenerator
    Sub New(location As ILocation, nameGenerator As NameGenerator)
        Me.location = location
        Me.nameGenerator = nameGenerator
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim descriptor = MapTypes.Descriptors(MapTypes.StarSystem)
        Dim place = location.Place
        place.Properties.Map = descriptor.CreateMap($"{place.Properties.Name} System", place.Universe)
        PlaceBoundaries(place, location, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(place, addStep)
        place.Family.PlanetCount = PlacePlanets(place, addStep)
        addStep(New EncounterInitializationStep(place.Properties.Map), True)
    End Sub

    Private Function PlacePlanets(place As IPlace, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim planets As New List(Of (Column As Integer, Row As Integer)) From
            {
                (place.Properties.Map.Size.Columns \ 2, place.Properties.Map.Size.Rows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim starType = StarTypes.Descriptors(place.Subtype)
        Dim MinimumDistance = starType.MinimumPlanetaryDistance
        Dim index = 1
        Dim maximumPlanetCount As Integer = starType.GenerateMaximumPlanetCount()
        Dim planetCount = 0
        While planetCount < maximumPlanetCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, place.Properties.Map.Size.Columns - 3)
            Dim row = RNG.FromRange(1, place.Properties.Map.Size.Rows - 3)
            If planets.All(Function(planet) (column - planet.Column) * (column - planet.Column) + (row - planet.Row) * (row - planet.Row) >= MinimumDistance * MinimumDistance) Then
                Dim planetType = starType.GeneratePlanetType()
                planets.Add((column, row))
                Dim location = place.Properties.Map.GetLocation(column, row)
                location.LocationType = PlanetTypes.Descriptors(planetType).LocationType
                location.Tutorial = TutorialTypes.PlanetVicinityApproach
                Dim planetName = nameGenerator.GenerateUnusedName
                index += 1
                ActorTypes.Descriptors(ActorTypes.MakePlanetVicinity(planetType)).CreateActor(location, planetType)
                location.Actor.Properties.Subtype = planetType
                addStep(New PlanetVicinityInitializationStep(location, nameGenerator), False)
                planetCount += 1
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return planetCount
    End Function

    Private Sub PlaceStar(place As IPlace, addStep As Action(Of InitializationStep, Boolean))
        Dim starColumn = place.Properties.Map.Size.Columns \ 2
        Dim starRow = place.Properties.Map.Size.Rows \ 2
        Dim locationType = LocationTypes.MakeStar(place.Subtype)
        Dim location = place.Properties.Map.GetLocation(starColumn, starRow)
        With location
            .LocationType = locationType
            .Place = place.Factory.CreateStarVicinity(starColumn, starRow)
            .Tutorial = TutorialTypes.StarVicinityApproach
            addStep(New StarVicinityInitializationStep(location), False)
        End With
    End Sub
End Class
