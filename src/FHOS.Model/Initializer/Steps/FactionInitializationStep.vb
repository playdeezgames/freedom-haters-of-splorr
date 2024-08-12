Imports SPLORR.Game

Friend Class FactionInitializationStep
    Inherits InitializationStep

    Private ReadOnly universe As Persistence.IUniverse
    Private ReadOnly embarkSettings As EmbarkSettings

    Public Sub New(universe As Persistence.IUniverse, embarkSettings As EmbarkSettings)
        Me.universe = universe
        Me.embarkSettings = embarkSettings
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        CreateSIGFED()
        Dim names As New HashSet(Of String)
        For Each dummy In Enumerable.Range(0, embarkSettings.FactionCount)
            Dim name As String = GenerateName(names)
            Dim faction = universe.Factory.CreateFaction(name, 0, RNG.RollDice("10d11+-10d1"), RNG.RollDice("10d11+-10d1"), RNG.RollDice("10d11+-10d1"))
            faction.GenerateValues()
        Next
    End Sub

    Private Sub CreateSIGFED()
        universe.Factory.CreateFaction("SIGMO Federation", 1, 100, 100, 100)
    End Sub

    Private ReadOnly firstParts As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "People's",
            "Socialist",
            "Merchant's",
            "Democratic",
            "Invincible",
            "Academics'",
            "Farmers'",
            "Philosophers'",
            "Altruistic",
            "United"
        }


    Private ReadOnly secondParts As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            "Republic",
            "League",
            "Monarchy",
            "Alliance",
            "State",
            "Principality",
            "Federation",
            "Sultanate",
            "Commonwealth",
            "Meritocracy"
        }

    Public Overrides ReadOnly Property Name As String
        Get
            Return $"Initializing Factions..."
        End Get
    End Property

    Private Function GenerateName(names As HashSet(Of String)) As String
        Dim name As String
        Do
            name = $"{RNG.FromEnumerable(firstParts)} {RNG.FromEnumerable(secondParts)}"
        Loop While names.Contains(name)
        names.Add(name)
        Return name
    End Function
End Class
