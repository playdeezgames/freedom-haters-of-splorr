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
        Dim map = MapTypes.Descriptors(MapTypes.PlanetVicinity).CreateMap($"{actor.Properties.EntityName} Vicinity", actor.Universe)
        actor.Properties.Interior = map
        PlaceBoundaryActors(
            actor,
            actor.Properties.Interior.Size.Columns,
            actor.Properties.Interior.Size.Rows)
        PlacePlanet(
            actor,
            addStep,
            actor.Descriptor.Subtype)
        actor.Properties.Groups(GroupTypes.PlanetVicinity).Statistics(StatisticTypes.SatelliteCount) = PlaceSatellites(actor, addStep)
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
        satelliteGroup.AddParent(externalActor.Properties.Groups(GroupTypes.PlanetVicinity))
        satelliteGroup.AddParent(externalActor.Properties.Groups(GroupTypes.PlanetVicinity).Parents.Single(Function(x) x.EntityType = GroupTypes.StarSystem))
        Dim satellite = ActorTypes.Descriptors(ActorTypes.MakeSatellite(satelliteType)).CreateActor(location, satelliteGroup.EntityName)
        satellite.Properties.Groups(GroupTypes.Satellite) = satelliteGroup
        satellite.Properties.Groups(GroupTypes.PlanetVicinity) = externalActor.Properties.Groups(GroupTypes.PlanetVicinity)
        satellite.Properties.Groups(GroupTypes.StarSystem) = externalActor.Properties.Groups(GroupTypes.StarSystem)
        location.EntityType = LocationTypes.Satellite
        addStep(New SatelliteOrbitInitializationStep(location), False)
    End Sub

    Private Sub PlacePlanet(externalActor As IActor, addStep As Action(Of InitializationStep, Boolean), subType As String)
        Dim centerColumn = externalActor.Properties.Interior.Size.Columns \ 2
        Dim centerRow = externalActor.Properties.Interior.Size.Rows \ 2
        Dim group = externalActor.Universe.Factory.CreateGroup(GroupTypes.Planet, externalActor.Properties.Groups(GroupTypes.PlanetVicinity).EntityName)
        group.AddParent(externalActor.Properties.Groups(GroupTypes.PlanetVicinity))
        For Each delta In Grid3x3.Descriptors
            PlacePlanetSectionActor(
                group,
                externalActor.Properties.Interior.GetLocation(centerColumn + delta.Value.Delta.X, centerRow + delta.Value.Delta.Y),
                subType,
                delta.Value.SectionName)
        Next
        addStep(New PlanetOrbitInitializationStep(externalActor.Properties.Interior.GetLocation(centerColumn, centerRow)), False)
    End Sub

    Private Shared Sub PlacePlanetSectionActor(
                                              planetGroup As IGroup,
                                              location As ILocation,
                                              subType As String,
                                              sectionName As String)
        Dim descriptor = ActorTypes.Descriptors(ActorTypes.MakePlanetSection(subType, sectionName))
        Dim actor = descriptor.CreateActor(location, planetGroup.EntityName)
        actor.Properties.Groups(GroupTypes.Planet) = planetGroup
    End Sub
End Class
