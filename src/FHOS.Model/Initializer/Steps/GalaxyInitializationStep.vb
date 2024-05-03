Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class GalaxyInitializationStep
    Inherits InitializationStep
    Private Const GalaxyColumns = 63
    Private Const GalaxyRows = 63
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
        Dim starMap = universe.CreateMap(MapTypes.Stellar, GalaxyName, GalaxyColumns, GalaxyRows, LocationTypes.Void)
        Dim stars As New List(Of (Column As Integer, Row As Integer))
        Dim tries As Integer = 0
        Const MaximumTries = 5000
        Dim MinimumDistance = GalacticDensities.Descriptors(embarkSettings.GalacticDensity).MinimumDistance
        While tries < MaximumTries
            Dim column = RNG.FromRange(0, GalaxyColumns - 1)
            Dim row = RNG.FromRange(0, GalaxyRows - 1)
            If stars.All(Function(star) (column - star.Column) * (column - star.Column) + (row - star.Row) * (row - star.Row) >= MinimumDistance * MinimumDistance) Then
                Dim starType = GalacticAges.Descriptors(embarkSettings.GalacticAge).GenerateStarType()
                stars.Add((column, row))
                Dim location = starMap.GetLocation(column, row)
                location.LocationType = StarTypes.Descriptors(starType).LocationType
                location.Tutorial = TutorialTypes.StarSystemEntry
                Dim starSystemName As String = nameGenerator.GenerateUnusedName
                location.Place = universe.CreateStarSystem(starSystemName, starType)
                addStep(New StarSystemInitializationStep(location, nameGenerator), False)
                tries = 0
            Else
                tries += 1
            End If
        End While
        addStep(New AvatarInitializationStep(universe, starMap, embarkSettings), False)
    End Sub
End Class
