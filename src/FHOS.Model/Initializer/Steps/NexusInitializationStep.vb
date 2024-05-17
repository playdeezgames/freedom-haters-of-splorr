Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class NexusInitializationStep
    Inherits InitializationStep
    Const NexusName = "Nexus"

    Private ReadOnly universe As IUniverse
    Private ReadOnly embarkSettings As EmbarkSettings

    Public Sub New(universe As Persistence.IUniverse, embarkSettings As EmbarkSettings)
        Me.universe = universe
        Me.embarkSettings = embarkSettings
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim nexusMap = MapTypes.Descriptors(MapTypes.Galaxy).CreateMap(
            NexusName,
            universe)
        Dim wormholes As New List(Of (Column As Integer, Row As Integer))
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim MinimumDistance = GalacticDensities.Descriptors(embarkSettings.GalacticDensity).MinimumWormholeDistance
        Dim starSystems = universe.GetPlacesOfType(PlaceTypes.StarSystem)
        While tries < MaximumTries
            Dim column = RNG.FromRange(0, nexusMap.Size.Columns - 1)
            Dim row = RNG.FromRange(0, nexusMap.Size.Rows - 1)
            If wormholes.All(Function(wormhole) (column - wormhole.Column) * (column - wormhole.Column) + (row - wormhole.Row) * (row - wormhole.Row) >= MinimumDistance * MinimumDistance) Then
                wormholes.Add((column, row))
                Dim location = nexusMap.GetLocation(column, row)
                location.LocationType = LocationTypes.Wormhole
                location.Tutorial = TutorialTypes.WormholeEntry
                Dim starSystem = RNG.FromEnumerable(starSystems)
                location.Place = universe.Factory.CreateWormhole($"{starSystem.Name} Wormhole")
                addStep(New WormholeInitializationStep(location, StarSystem), False)
                tries = 0
            Else
                tries += 1
            End If
        End While
    End Sub
End Class
