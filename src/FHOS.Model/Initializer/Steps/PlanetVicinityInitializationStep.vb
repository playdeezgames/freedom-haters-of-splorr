Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class PlanetVicinityInitializationStep
    Inherits InitializationStep
    Private ReadOnly planetVicinityLocation As ILocation
    Private ReadOnly nameGenerator As NameGenerator
    Sub New(location As ILocation, nameGenerator As NameGenerator)
        Me.planetVicinityLocation = location
        Me.nameGenerator = nameGenerator
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim place = planetVicinityLocation.Place
        place.Properties.Map = MapTypes.Descriptors(MapTypes.PlanetVicinity).CreateMap($"{place.Properties.Name} Vicinity", place.Universe)
        PlaceBoundaries(place, planetVicinityLocation, place.Properties.Map.Size.Columns, place.Properties.Map.Size.Rows)
        PlacePlanet(place, addStep, place.Subtype)
        place.Family.SatelliteCount = PlaceSatellites(place, addStep)
        addStep(New EncounterInitializationStep(place.Properties.Map), True)
    End Sub
    Private Function PlaceSatellites(planetVicinity As IPlace, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim satellites As List(Of (Column As Integer, Row As Integer)) =
            Grid3x3.Descriptors.
            Select(Function(x) (x.Value.Delta.X + planetVicinity.Properties.Map.Size.Columns \ 2, x.Value.Delta.Y + planetVicinity.Properties.Map.Size.Rows \ 2)).
            ToList
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim planetType = PlanetTypes.Descriptors(planetVicinity.Subtype)
        Dim MinimumDistance = planetType.MinimumSatelliteDistance
        Dim maximumSatelliteCount As Integer = planetType.GenerateMaximumSatelliteCount()
        Dim satelliteCount = 0
        While satelliteCount < maximumSatelliteCount AndAlso tries < MaximumTries
            Dim column = RNG.FromRange(1, planetVicinity.Properties.Map.Size.Columns - 3)
            Dim row = RNG.FromRange(1, planetVicinity.Properties.Map.Size.Rows - 3)
            If satellites.All(Function(satellite) (column - satellite.Column) * (column - satellite.Column) + (row - satellite.Row) * (row - satellite.Row) >= MinimumDistance * MinimumDistance) Then
                Dim satelliteType As String = planetType.GenerateSatelliteType()
                satellites.Add((column, row))
                Dim location = planetVicinity.Properties.Map.GetLocation(column, row)
                Dim satelliteName = nameGenerator.GenerateUnusedName
                Dim actor = ActorTypes.Descriptors(ActorTypes.MakeSatellite(satelliteType)).CreateActor(location, satelliteName)
                actor.Properties.Name = satelliteName
                addStep(New SatelliteOrbitInitializationStep(location), False)
                satelliteCount += 1
                tries = 0
            Else
                tries += 1
            End If
        End While
        Return satelliteCount
    End Function
    Private Sub PlacePlanet(place As IPlace, addStep As Action(Of InitializationStep, Boolean), subType As String)
        Dim centerColumn = place.Properties.Map.Size.Columns \ 2
        Dim centerRow = place.Properties.Map.Size.Rows \ 2
        For Each delta In Grid3x3.Descriptors
            PlacePlanetSectionActor(place.Properties.Map.GetLocation(centerColumn + delta.Value.Delta.X, centerRow + delta.Value.Delta.Y), subType, delta.Value.SectionName)
        Next
        addStep(New PlanetOrbitInitializationStep(place.Properties.Map.GetLocation(centerColumn, centerRow)), False)
    End Sub

    Private Shared Sub PlacePlanetSectionActor(location As ILocation, subType As String, sectionName As String)
        Dim descriptor = ActorTypes.Descriptors(ActorTypes.MakePlanetSection(subType, sectionName))
        descriptor.CreateActor(location, "Planet")
    End Sub
End Class
