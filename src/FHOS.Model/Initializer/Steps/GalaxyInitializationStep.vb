﻿Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class GalaxyInitializationStep
    Inherits InitializationStep
    Private Const GalaxyColumns = 63
    Private Const GalaxyRows = 63
    Private Const GalaxyName = "Galaxy Map"
    Private ReadOnly universe As IUniverse
    Private ReadOnly embarkSettings As EmbarkSettings
    Sub New(universe As IUniverse, embarkSettings As EmbarkSettings)
        Me.universe = universe
        Me.embarkSettings = embarkSettings
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep))
        Dim starMap = universe.CreateMap(MapTypes.Stellar, GalaxyName, GalaxyColumns, GalaxyRows, LocationTypes.Void)
        Dim stars As New List(Of (Column As Integer, Row As Integer))
        Dim starSystemNames As New HashSet(Of String)
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
                Dim starSystemName As String = GenerateUnusedStarSystemName(starSystemNames)
                location.StarSystem = universe.CreateStarSystem(starSystemName, starType)
                addStep(New StarSystemInitializationStep(location))
                tries = 0
            Else
                tries += 1
            End If
        End While
        addStep(New AvatarInitializationStep(universe, starMap, embarkSettings))
    End Sub
    Private Function GenerateUnusedStarSystemName(starSystemNames As HashSet(Of String)) As String
        Dim starSystemName As String
        Do
            starSystemName = GenerateStarSystemName()
        Loop Until Not starSystemNames.Contains(starSystemName)
        starSystemNames.Add(starSystemName)
        Return starSystemName
    End Function

    ReadOnly ConstellationNames As IReadOnlyList(Of String) = New List(Of String) From
        {
            "Thuspo",
            "Reylon",
            "Shyrana",
            "Larak",
            "Ywan",
            "Meleka",
            "Ardana",
            "Krala",
            "Kerdix",
            "Derna",
            "Zoora",
            "Treyok",
            "Neror",
            "Bejis",
            "Vulerai",
            "Tsanrua",
            "Grot",
            "Jhama",
            "Peshto",
            "Antwerp",
            "Kai",
            "Polatarnia",
            "Mlideen",
            "Shuitan",
            "Ralatlan",
            "Liefzol"
        }

    ReadOnly GreekLetterNames As IReadOnlyList(Of String) = New List(Of String) From
        {
            "Alpha",
            "Beta",
            "Gamma",
            "Delta",
            "Epsilon",
            "Zeta",
            "Eta",
            "Theta",
            "Iota",
            "Kappa",
            "Lambda",
            "Mu",
            "Nu",
            "Xi",
            "Omicron",
            "Pi",
            "Rho",
            "Sigma",
            "Tau",
            "Upsilon",
            "Phi",
            "Chi",
            "Psi",
            "Omega"
        }

    Private Function GenerateStarSystemName() As String
        Return $"{RNG.FromEnumerable(ConstellationNames)} {RNG.FromEnumerable(GreekLetterNames)}"
    End Function
End Class