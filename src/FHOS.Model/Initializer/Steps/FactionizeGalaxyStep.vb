Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class FactionizeGalaxyStep
    Inherits InitializationStep

    Private ReadOnly universe As Persistence.IUniverse

    Public Sub New(universe As Persistence.IUniverse)
        Me.universe = universe
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim factions = universe.Factions
        Dim planets = New HashSet(Of IPlace)(universe.GetPlacesOfType(PlaceTypes.Planet))
        For Each faction In factions
            For Each dummy In Enumerable.Range(0, faction.MinimumPlanetCount)
                Dim planet = RNG.FromEnumerable(planets)
                planet.Properties.Faction = faction
                faction.PlanetCount += 1
                planets.Remove(planet)
            Next
        Next
        For Each remainingPlanet In planets
            remainingPlanet.Properties.Faction = RNG.FromEnumerable(factions)
            remainingPlanet.Properties.Faction.PlanetCount += 1
        Next
    End Sub
End Class
