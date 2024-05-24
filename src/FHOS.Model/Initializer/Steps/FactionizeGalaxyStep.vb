Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class FactionizeGalaxyStep
    Inherits InitializationStep

    Private ReadOnly universe As Persistence.IUniverse

    Public Sub New(universe As Persistence.IUniverse)
        Me.universe = universe
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        FactionizeActors(universe.Groups.Where(Function(x) x.GroupType = GroupTypes.Faction))
    End Sub

    Private Sub FactionizeActors(factions As IEnumerable(Of IGroup))
        Dim planets = New HashSet(Of IActor)(universe.Actors.Where(Function(x) x.Properties.IsPlanet AndAlso x.Properties.SectionName = Grid3x3.Center))
        For Each group In factions
            For Each dummy In Enumerable.Range(0, group.MinimumPlanetCount)
                Dim planet = RNG.FromEnumerable(planets)
                planet.Properties.Group = group
                group.PlanetCount += 1
                planets.Remove(planet)
            Next
        Next
        For Each remainingPlanet In planets
            remainingPlanet.Properties.Group = RNG.FromEnumerable(factions)
            remainingPlanet.Properties.Group.PlanetCount += 1
        Next
    End Sub
End Class
