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
        Dim map = MapTypes.Descriptors(MapTypes.PlanetVicinity).CreateMap($"{actor.Properties.Name} Vicinity", actor.Universe)
        actor.Properties.Interior = map
        PlaceBoundaryActors(actor, actor.Properties.Interior.Size.Columns, actor.Properties.Interior.Size.Rows)
        PlacePlanet(actor, addStep, actor.Properties.Subtype)
        actor.Properties.SatelliteCount = PlaceSatellites(actor, addStep)
        addStep(New EncounterInitializationStep(actor.Properties.Interior), True)
    End Sub
    Private Function PlaceSatellites(actor As IActor, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim satellites As List(Of (Column As Integer, Row As Integer)) =
            Grid3x3.Descriptors.
            Select(Function(x) (x.Value.Delta.X + actor.Properties.Interior.Size.Columns \ 2, x.Value.Delta.Y + actor.Properties.Interior.Size.Rows \ 2)).
            ToList
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim planetType = PlanetTypes.Descriptors(actor.Properties.Subtype)
        Dim MinimumDistance = planetType.MinimumSatelliteDistance
        Dim maximumSatelliteCount As Integer = planetType.GenerateMaximumSatelliteCount()
        Dim satelliteCount = 0
        While satelliteCount < maximumSatelliteCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, actor.Properties.Interior.Size.Columns - 3)
            Dim row = RNG.FromRange(1, actor.Properties.Interior.Size.Rows - 3)
            If satellites.All(Function(satellite) (column - satellite.Column) * (column - satellite.Column) + (row - satellite.Row) * (row - satellite.Row) >= MinimumDistance * MinimumDistance) Then
                Dim satelliteType As String = planetType.GenerateSatelliteType()
                satellites.Add((column, row))
                Dim location = actor.Properties.Interior.GetLocation(column, row)
                Dim satelliteName = nameGenerator.GenerateUnusedName
                Dim satellite = ActorTypes.Descriptors(ActorTypes.MakeSatellite(satelliteType)).CreateActor(location, satelliteName)
                satellite.Properties.Name = satelliteName
                addStep(New SatelliteOrbitInitializationStep(location), False)
                satelliteCount += 1
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return satelliteCount
    End Function
    Private Sub PlacePlanet(actor As IActor, addStep As Action(Of InitializationStep, Boolean), subType As String)
        Dim centerColumn = actor.Properties.Interior.Size.Columns \ 2
        Dim centerRow = actor.Properties.Interior.Size.Rows \ 2
        For Each delta In Grid3x3.Descriptors
            PlacePlanetSectionActor(actor.Properties.Interior.GetLocation(centerColumn + delta.Value.Delta.X, centerRow + delta.Value.Delta.Y), subType, delta.Value.SectionName)
        Next
        addStep(New PlanetOrbitInitializationStep(actor.Properties.Interior.GetLocation(centerColumn, centerRow)), False)
    End Sub

    Private Shared Sub PlacePlanetSectionActor(location As ILocation, subType As String, sectionName As String)
        Dim descriptor = ActorTypes.Descriptors(ActorTypes.MakePlanetSection(subType, sectionName))
        descriptor.CreateActor(location, "Planet")
    End Sub
End Class
