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
        Dim actor = location.Actor
        actor.Properties.Interior = descriptor.CreateMap($"{actor.Properties.Name} System", actor.Universe)
        PlaceBoundaryActors(actor, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(actor, addStep)
        actor.Properties.PlanetCount = PlacePlanets(actor, addStep)
        addStep(New EncounterInitializationStep(actor.Properties.Interior), True)
    End Sub

    Private Function PlacePlanets(place As IActor, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim planets As New List(Of (Column As Integer, Row As Integer)) From
            {
                (place.Properties.Interior.Size.Columns \ 2, place.Properties.Interior.Size.Rows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim starType = StarTypes.Descriptors(place.Properties.Subtype)
        Dim MinimumDistance = starType.MinimumPlanetaryDistance
        Dim index = 1
        Dim maximumPlanetCount As Integer = starType.GenerateMaximumPlanetCount()
        Dim planetCount = 0
        While planetCount < maximumPlanetCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, place.Properties.Interior.Size.Columns - 3)
            Dim row = RNG.FromRange(1, place.Properties.Interior.Size.Rows - 3)
            If planets.All(Function(planet) (column - planet.Column) * (column - planet.Column) + (row - planet.Row) * (row - planet.Row) >= MinimumDistance * MinimumDistance) Then
                Dim planetType = starType.GeneratePlanetType()
                planets.Add((column, row))
                Dim location = place.Properties.Interior.GetLocation(column, row)
                location.LocationType = PlanetTypes.Descriptors(planetType).LocationType
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

    Private Sub PlaceStar(actor As IActor, addStep As Action(Of InitializationStep, Boolean))
        Dim starColumn = actor.Properties.Interior.Size.Columns \ 2
        Dim starRow = actor.Properties.Interior.Size.Rows \ 2
        Dim locationType = LocationTypes.MakeStar(actor.Properties.Subtype)
        Dim location = actor.Properties.Interior.GetLocation(starColumn, starRow)
        ActorTypes.Descriptors(ActorTypes.MakeStarVicinity(actor.Properties.Subtype)).CreateActor(location, "Star")
        location.Actor.Properties.Subtype = actor.Properties.Subtype
        addStep(New StarVicinityInitializationStep(location), False)
    End Sub
End Class
