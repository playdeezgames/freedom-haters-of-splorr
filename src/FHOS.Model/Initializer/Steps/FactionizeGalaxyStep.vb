Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class FactionizeGalaxyStep
    Inherits InitializationStep

    Private ReadOnly universe As Persistence.IUniverse

    Public Sub New(universe As Persistence.IUniverse)
        Me.universe = universe
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        FactionizePlanets(universe.Groups.Where(Function(x) x.GroupType = GroupTypes.Faction))
    End Sub

    Private Sub FactionizePlanets(factions As IEnumerable(Of IGroup))
        Dim planets = New HashSet(Of IGroup)(universe.Groups.Where(Function(x) x.GroupType = GroupTypes.PlanetVicinity))
        For Each group In factions
            For Each dummy In Enumerable.Range(0, group.Statistics(StatisticTypes.MinimumPlanetCount).Value)
                Dim planet = RNG.FromEnumerable(planets)
                planet.AddParent(group)
                group.Statistics(StatisticTypes.PlanetCount) = If(group.Statistics(StatisticTypes.PlanetCount), 0) + 1
                planets.Remove(planet)
            Next
        Next
        For Each remainingPlanet In planets
            Dim group = RNG.FromEnumerable(factions)
            group.Statistics(StatisticTypes.PlanetCount) = If(group.Statistics(StatisticTypes.PlanetCount), 0) + 1
            remainingPlanet.AddParent(group)
        Next
    End Sub
End Class
