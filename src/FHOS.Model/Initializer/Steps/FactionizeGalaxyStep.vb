Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class FactionizeGalaxyStep
    Inherits InitializationStep

    Private ReadOnly universe As Persistence.IUniverse

    Public Sub New(universe As Persistence.IUniverse)
        Me.universe = universe
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim factions = universe.Groups
        FactionizeActors(factions)
    End Sub

    Private Sub FactionizeActors(factions As IEnumerable(Of IGroup))
        Dim planets = New HashSet(Of IActor)(universe.Actors.Where(Function(x) x.Properties.IsPlanet AndAlso x.Properties.SectionName = Grid3x3.Center))
        For Each faction In factions
            For Each dummy In Enumerable.Range(0, faction.MinimumPlanetCount)
                Dim planet = RNG.FromEnumerable(planets)
                planet.Properties.Group = faction
                faction.PlanetCount += 1
                planets.Remove(planet)
            Next
        Next
        For Each remainingPlanet In planets
            remainingPlanet.Properties.Group = RNG.FromEnumerable(factions)
            remainingPlanet.Properties.Group.PlanetCount += 1
        Next
    End Sub
End Class
