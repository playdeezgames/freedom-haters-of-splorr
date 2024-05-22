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
        Dim planetVicinity = planetVicinityLocation.Place
        planetVicinity.Properties.Map = MapTypes.Descriptors(MapTypes.PlanetVicinity).CreateMap($"{planetVicinity.Properties.Name} Vicinity", planetVicinity.Universe)
        PlaceBoundaries(planetVicinity, planetVicinityLocation, planetVicinity.Properties.Map.Size.Columns, planetVicinity.Properties.Map.Size.Rows)
        PlacePlanet(planetVicinity, addStep, planetVicinity.Subtype)
        planetVicinity.Family.SatelliteCount = PlaceSatellites(planetVicinity, addStep)
        addStep(New EncounterInitializationStep(planetVicinity.Properties.Map), True)
    End Sub
    Private Function PlaceSatellites(planetVicinity As IPlace, addStep As Action(Of InitializationStep, Boolean)) As Integer
        Dim satellites As List(Of (Column As Integer, Row As Integer)) =
            planetSectionDeltas.
            Select(Function(x) (x.DeltaX + planetVicinity.Properties.Map.Size.Columns \ 2, x.DeltaY + planetVicinity.Properties.Map.Size.Rows \ 2)).
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
                Dim actor = ActorTypes.Descriptors(ActorTypes.MakeSatellite(satelliteType)).CreateActor(location)
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
    Private ReadOnly planetSectionDeltas As IReadOnlyList(Of (DeltaX As Integer, DeltaY As Integer, SectionName As String)) =
        New List(Of (DeltaX As Integer, DeltaY As Integer, SectionName As String)) From
        {
            (-1, -1, Grid3x3.TopLeft),
            (0, -1, Grid3x3.TopCenter),
            (1, -1, Grid3x3.TopRight),
            (-1, 0, Grid3x3.CenterLeft),
            (0, 0, Grid3x3.Center),
            (1, 0, Grid3x3.CenterRight),
            (-1, 1, Grid3x3.BottomLeft),
            (0, 1, Grid3x3.BottomCenter),
            (1, 1, Grid3x3.BottomRight)
        }
    Private Sub PlacePlanet(planetVicinity As IPlace, addStep As Action(Of InitializationStep, Boolean), planetType As String)
        Dim planetCenterColumn = planetVicinity.Properties.Map.Size.Columns \ 2
        Dim planetCenterRow = planetVicinity.Properties.Map.Size.Rows \ 2
        Dim planet = planetVicinity.Factory.CreatePlanet()
        For Each delta In planetSectionDeltas
            PlacePlanetSection(planet, planetVicinity.Properties.Map.GetLocation(planetCenterColumn + delta.DeltaX, planetCenterRow + delta.DeltaY), delta.SectionName)
            PlacePlanetSectionActor(planetVicinity.Properties.Map.GetLocation(planetCenterColumn + delta.DeltaX, planetCenterRow + delta.DeltaY), planetType, delta.SectionName)
        Next
        addStep(New PlanetOrbitInitializationStep(planetVicinity.Properties.Map.GetLocation(planetCenterColumn, planetCenterRow)), False)
    End Sub

    Private Shared Sub PlacePlanetSection(planet As IPlace, location As ILocation, sectionName As String)
        Dim locationType = PlanetTypes.Descriptors(planet.Subtype).SectionLocationType(sectionName)
        With location
            .LocationType = locationType
            .Tutorial = TutorialTypes.EnterPlanetOrbit
            .Place = planet
        End With
    End Sub
    Private Shared Sub PlacePlanetSectionActor(location As ILocation, planetType As String, sectionName As String)
        Dim descriptor = ActorTypes.Descriptors(ActorTypes.MakePlanetSection(planetType, sectionName))
        descriptor.CreateActor(location)
    End Sub
End Class
