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
        actor.Properties.Group.PlanetCount = PlacePlanets(actor, addStep)
        addStep(New EncounterInitializationStep(actor.Properties.Interior), True)
    End Sub

    Private Function PlacePlanets(actor As IActor, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim planets As New List(Of (Column As Integer, Row As Integer)) From
            {
                (actor.Properties.Interior.Size.Columns \ 2, actor.Properties.Interior.Size.Rows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim starType = StarTypes.Descriptors(actor.Descriptor.Subtype)
        Dim MinimumDistance = starType.MinimumPlanetaryDistance
        Dim maximumPlanetCount As Integer = starType.GenerateMaximumPlanetCount()
        Dim planetCount = 0
        While planetCount < maximumPlanetCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, actor.Properties.Interior.Size.Columns - 3)
            Dim row = RNG.FromRange(1, actor.Properties.Interior.Size.Rows - 3)
            If planets.All(Function(planet) (column - planet.Column) * (column - planet.Column) + (row - planet.Row) * (row - planet.Row) >= MinimumDistance * MinimumDistance) Then
                planets.Add((column, row))
                CreatePlanetVicinity(actor, addStep, starType, column, row)
                planetCount += 1
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return planetCount
    End Function

    Private Sub CreatePlanetVicinity(exteriorActor As IActor, addStep As Action(Of InitializationStep, Boolean), starType As StarTypeDescriptor, column As Integer, row As Integer)
        Dim planetType = starType.GeneratePlanetType()
        Dim location = exteriorActor.Properties.Interior.GetLocation(column, row)
        location.LocationType = LocationTypes.PlanetVicinity
        Dim group = location.Universe.Factory.CreateGroup(GroupTypes.PlanetVicinity, nameGenerator.GenerateUnusedName)
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakePlanetVicinity(planetType)).CreateActor(location, $"{group.Name}")
        actor.Properties.Group = group
        addStep(New PlanetVicinityInitializationStep(location, nameGenerator), False)
    End Sub

    Private Sub PlaceStar(externalActor As IActor, addStep As Action(Of InitializationStep, Boolean))
        Dim starColumn = externalActor.Properties.Interior.Size.Columns \ 2
        Dim starRow = externalActor.Properties.Interior.Size.Rows \ 2
        Dim location = externalActor.Properties.Interior.GetLocation(starColumn, starRow)
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakeStarVicinity(externalActor.Descriptor.Subtype)).CreateActor(location, externalActor.Properties.Group.Name)
        actor.Properties.Group = externalActor.Properties.Group
        location.LocationType = LocationTypes.StarVicinity
        addStep(New StarVicinityInitializationStep(location), False)
    End Sub
End Class
