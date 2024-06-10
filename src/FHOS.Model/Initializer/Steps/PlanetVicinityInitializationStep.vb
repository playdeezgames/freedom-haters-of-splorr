Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class PlanetVicinityInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As ILocation
    Private ReadOnly nameGenerator As NameGenerator
    Sub New(location As ILocation, nameGenerator As NameGenerator)
        Me.location = location
        Me.nameGenerator = nameGenerator
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim actor = location.Actor
        Dim map = MapTypes.Descriptors(MapTypes.PlanetVicinity).CreateMap($"{actor.EntityName} Vicinity", actor.Universe)
        actor.Properties.Interior = map
        PlaceBoundaryActors(
            actor,
            actor.Properties.Interior.Size.Columns,
            actor.Properties.Interior.Size.Rows)
        PlacePlanet(
            actor,
            addStep,
            actor.Descriptor.Subtype)
        Dim planetGroup = actor.YokedGroup(YokeTypes.PlanetVicinity)
        planetGroup.Statistics(StatisticTypes.SatelliteCount) = PlaceSatellites(actor, addStep)
        Dim starSystemGroup = planetGroup.Parents.Single(Function(x) x.EntityType = GroupTypes.StarSystem)
        starSystemGroup.Statistics(StatisticTypes.SatelliteCount) = If(starSystemGroup.Statistics(StatisticTypes.SatelliteCount), 0) + planetGroup.Statistics(StatisticTypes.SatelliteCount)
        addStep(New EncounterInitializationStep(actor.Properties.Interior), True)
    End Sub
    Private Function PlaceSatellites(externalActor As IActor, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim satellites As List(Of (Column As Integer, Row As Integer)) =
            Grid3x3.Descriptors.
            Select(Function(x) (x.Value.Delta.X + externalActor.Properties.Interior.Size.Columns \ 2, x.Value.Delta.Y + externalActor.Properties.Interior.Size.Rows \ 2)).
            ToList
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim planetType = PlanetTypes.Descriptors(externalActor.Descriptor.Subtype)
        Dim MinimumDistance = planetType.MinimumSatelliteDistance
        Dim maximumSatelliteCount As Integer = planetType.GenerateMaximumSatelliteCount()
        Dim satelliteCount = 0
        While satelliteCount < maximumSatelliteCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, externalActor.Properties.Interior.Size.Columns - 3)
            Dim row = RNG.FromRange(1, externalActor.Properties.Interior.Size.Rows - 3)
            If satellites.All(Function(satellite) (column - satellite.Column) * (column - satellite.Column) + (row - satellite.Row) * (row - satellite.Row) >= MinimumDistance * MinimumDistance) Then
                satellites.Add((column, row))
                MakeSatellite(externalActor, addStep, planetType, column, row)
                satelliteCount += 1
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return satelliteCount
    End Function

    Private Sub MakeSatellite(externalActor As IActor, addStep As Action(Of InitializationStep, Boolean), planetType As PlanetTypeDescriptor, column As Integer, row As Integer)
        Dim satelliteType As String = planetType.GenerateSatelliteType()
        Dim location = externalActor.Properties.Interior.GetLocation(column, row)
        Dim satelliteGroup = externalActor.Universe.Factory.CreateGroup(GroupTypes.Satellite, nameGenerator.GenerateUnusedName)
        satelliteGroup.AddParent(externalActor.YokedGroup(YokeTypes.PlanetVicinity))
        satelliteGroup.AddParent(externalActor.YokedGroup(YokeTypes.PlanetVicinity).Parents.Single(Function(x) x.EntityType = GroupTypes.StarSystem))
        Dim satellite = ActorTypes.Descriptors(ActorTypes.MakeSatellite(satelliteType)).CreateActor(location, satelliteGroup.EntityName)
        satellite.YokedGroup(YokeTypes.Satellite) = satelliteGroup
        satellite.YokedGroup(YokeTypes.PlanetVicinity) = externalActor.YokedGroup(YokeTypes.PlanetVicinity)
        satellite.YokedGroup(YokeTypes.StarSystem) = externalActor.YokedGroup(YokeTypes.StarSystem)
        location.EntityType = LocationTypes.Satellite
        addStep(New SatelliteOrbitInitializationStep(location), False)
    End Sub

    Private Sub PlacePlanet(externalActor As IActor, addStep As Action(Of InitializationStep, Boolean), subType As String)
        Dim centerColumn = externalActor.Properties.Interior.Size.Columns \ 2
        Dim centerRow = externalActor.Properties.Interior.Size.Rows \ 2
        Dim planetGroup = externalActor.Universe.Factory.CreateGroup(GroupTypes.Planet, externalActor.YokedGroup(YokeTypes.PlanetVicinity).EntityName)
        planetGroup.Statistics(StatisticTypes.ShipyardCount) = 0
        planetGroup.AddParent(externalActor.YokedGroup(YokeTypes.PlanetVicinity))
        For Each delta In Grid3x3.Descriptors
            PlacePlanetSectionActor(
                planetGroup,
                externalActor.Properties.Interior.GetLocation(centerColumn + delta.Value.Delta.X, centerRow + delta.Value.Delta.Y),
                subType,
                delta.Value.SectionName)
        Next
        addStep(New PlanetOrbitInitializationStep(externalActor.Properties.Interior.GetLocation(centerColumn, centerRow), planetGroup), False)
    End Sub

    Private Shared Sub PlacePlanetSectionActor(
                                              planetGroup As IGroup,
                                              location As ILocation,
                                              subType As String,
                                              sectionName As String)
        Dim descriptor = ActorTypes.Descriptors(ActorTypes.MakePlanetSection(subType, sectionName))
        Dim actor = descriptor.CreateActor(location, planetGroup.EntityName)
        actor.YokedGroup(YokeTypes.Planet) = planetGroup
    End Sub
End Class
