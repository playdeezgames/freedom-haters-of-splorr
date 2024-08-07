Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class StarSystemInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As ILocation
    Private ReadOnly nameGenerator As NameGenerator
    Private ReadOnly starSystemGroup As IGroup

    Public Overrides ReadOnly Property Name As String
        Get
            Return $"Initializing Star System {starSystemGroup.EntityName}..."
        End Get
    End Property

    Sub New(location As ILocation, nameGenerator As NameGenerator, starSystemGroup As IGroup)
        Me.location = location
        Me.nameGenerator = nameGenerator
        Me.starSystemGroup = starSystemGroup
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim descriptor = MapTypes.Descriptors(MapTypes.StarSystem)
        Dim actor = location.Actor
        Dim map = descriptor.CreateMap($"{actor.EntityName} System", actor.Universe)
        map.YokedGroup(YokeTypes.StarSystem) = starSystemGroup
        actor.Interior = map
        PlaceBoundaryActors(actor, descriptor.Size.Columns, descriptor.Size.Rows)
        PlaceStar(actor, addStep)
        actor.Yokes.Group(YokeTypes.StarSystem).Statistics(StatisticTypes.PlanetCount) = PlacePlanets(actor, addStep)
        addStep(New EncounterInitializationStep(map), True)
    End Sub

    Private Function PlacePlanets(actor As IActor, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim planets As New List(Of (Column As Integer, Row As Integer)) From
            {
                (actor.Interior.Size.Columns \ 2, actor.Interior.Size.Rows \ 2)
            }
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim starType = StarTypes.Descriptors(actor.Descriptor.Subtype)
        Dim MinimumDistance = starType.MinimumPlanetaryDistance
        Dim maximumPlanetCount As Integer = starType.GenerateMaximumPlanetCount()
        Dim planetCount = 0
        While planetCount < maximumPlanetCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, actor.Interior.Size.Columns - 3)
            Dim row = RNG.FromRange(1, actor.Interior.Size.Rows - 3)
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
        Dim location = exteriorActor.Interior.GetLocation(column, row)
        location.EntityType = LocationTypes.PlanetVicinity
        Dim planetVicinityGroup = location.Universe.Factory.CreateGroup(GroupTypes.PlanetVicinity, nameGenerator.GenerateUnusedName)
        planetVicinityGroup.Metadatas(MetadataTypes.Subtype) = planetType
        planetVicinityGroup.AddParent(exteriorActor.Yokes.Group(YokeTypes.StarSystem))
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakePlanetVicinity(planetType)).CreateActor(location, $"{planetVicinityGroup.EntityName}")
        actor.Yokes.Group(YokeTypes.PlanetVicinity) = planetVicinityGroup
        planetVicinityGroup.Statistics(StatisticTypes.ShipyardCount) = 0
        planetVicinityGroup.Statistics(StatisticTypes.TradingPostCount) = 0
        planetVicinityGroup.Statistics(StatisticTypes.StarDockCount) = 0
        planetVicinityGroup.Statistics(StatisticTypes.StarGateCount) = 0
        planetVicinityGroup.Statistics(StatisticTypes.TechLevel) = RNG.RollDice("2d6+-2d1")
        actor.Yokes.Group(YokeTypes.StarSystem) = exteriorActor.Yokes.Group(YokeTypes.StarSystem)
        addStep(New PlanetVicinityInitializationStep(location, nameGenerator), False)
    End Sub

    Private Sub PlaceStar(externalActor As IActor, addStep As Action(Of InitializationStep, Boolean))
        Dim starColumn = externalActor.Interior.Size.Columns \ 2
        Dim starRow = externalActor.Interior.Size.Rows \ 2
        Dim location = externalActor.Interior.GetLocation(starColumn, starRow)
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakeStarVicinity(externalActor.Descriptor.Subtype)).CreateActor(location, externalActor.Yokes.Group(YokeTypes.StarSystem).EntityName)
        location.EntityType = LocationTypes.StarVicinity
        addStep(New StarVicinityInitializationStep(location), False)
    End Sub
End Class
