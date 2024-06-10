Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class GalaxyInitializationStep
    Inherits InitializationStep
    Private Const GalaxyName = "Galaxy Map"
    Private ReadOnly universe As IUniverse
    Private ReadOnly embarkSettings As EmbarkSettings
    Private ReadOnly nameGenerator As NameGenerator
    Sub New(universe As IUniverse, embarkSettings As EmbarkSettings, nameGenerator As NameGenerator)
        Me.universe = universe
        Me.embarkSettings = embarkSettings
        Me.nameGenerator = nameGenerator
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim starMap = MapTypes.Descriptors(MapTypes.Galaxy).CreateMap(
            GalaxyName,
            universe)
        Dim stars As New List(Of (Column As Integer, Row As Integer))
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim MinimumDistance = GalacticDensities.Descriptors(embarkSettings.GalacticDensity).MinimumDistance
        While tries < MaximumTries
            Dim column = RNG.FromRange(0, starMap.Size.Columns - 1)
            Dim row = RNG.FromRange(0, starMap.Size.Rows - 1)
            If stars.All(Function(star) (column - star.Column) * (column - star.Column) + (row - star.Row) * (row - star.Row) >= MinimumDistance * MinimumDistance) Then
                stars.Add((column, row))
                AddStarSystem(addStep, starMap, column, row)
                tries = 0
            Else
                tries += 1
            End If
        End While
        addStep(New NexusInitializationStep(universe, embarkSettings), True)
        addStep(New EncounterInitializationStep(starMap), True)
        addStep(New AvatarInitializationStep(universe, embarkSettings), True)
    End Sub

    Private Sub AddStarSystem(addStep As Action(Of InitializationStep, Boolean), map As IMap, column As Integer, row As Integer)
        Dim starType = GalacticAges.Descriptors(embarkSettings.GalacticAge).GenerateStarType()
        Dim location = map.GetLocation(column, row)
        Dim starSystemGroup = map.Universe.Factory.CreateGroup(GroupTypes.StarSystem, nameGenerator.GenerateUnusedName)
        starSystemGroup.Statistics(StatisticTypes.Scrap) = 0
        starSystemGroup.Statistics(StatisticTypes.VisitCount) = 0
        starSystemGroup.Statistics(StatisticTypes.WormholeCount) = 0
        starSystemGroup.Statistics(StatisticTypes.StarGateCount) = 0
        starSystemGroup.Statistics(StatisticTypes.ShipyardCount) = 0
        starSystemGroup.Statistics(StatisticTypes.TradingPostCount) = 0
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakeStarSystem(starType)).CreateActor(location, $"{starSystemGroup.EntityName}")
        location.EntityType = LocationTypes.StarSystem
        actor.YokedGroup(YokeTypes.StarSystem) = starSystemGroup
        addStep(New StarSystemInitializationStep(location, nameGenerator, starSystemGroup), False)
    End Sub
End Class
