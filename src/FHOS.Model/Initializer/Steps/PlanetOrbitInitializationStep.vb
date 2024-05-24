Imports FHOS.Persistence

Friend Class PlanetOrbitInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As ILocation
    Public Sub New(location As ILocation)
        Me.location = location
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim planet = location.Actor
        Dim actors = location.Map.Locations.Where(Function(x) If(x.Actor?.Properties?.IsPlanet, False)).Select(Function(x) x.Actor)
        Dim map = MapTypes.Descriptors(MapTypes.PlanetOrbit).CreateMap($"{planet.Properties.Group.Name} Orbit", planet.Universe)
        planet.Properties.Interior = map
        For Each actor In actors
            actor.Properties.Interior = map
        Next
        Dim targetActor = actors.OrderBy(Function(x) x.State.Location.Column).ThenBy(Function(x) x.State.Location.Row).First()
        PlaceBoundaryActors(targetActor, planet.Properties.Interior.Size.Columns, planet.Properties.Interior.Size.Rows)
        PlacePlanet(planet)
        addStep(New EncounterInitializationStep(planet.Properties.Interior), True)
    End Sub
    Private Sub PlacePlanet(planet As IActor)
        Dim planetCenterColumn = planet.Properties.Interior.Size.Columns \ 2
        Dim planetCenterRow = planet.Properties.Interior.Size.Rows \ 2
        For Each delta In Grid5x5.Descriptors
            PlacePlanetSection(
                planet,
                planet.Properties.Interior.GetLocation(
                    planetCenterColumn + delta.Value.Delta.X,
                    planetCenterRow + delta.Value.Delta.Y),
                delta.Key)
        Next
    End Sub
    Private Shared Sub PlacePlanetSection(planet As IActor, location As ILocation, sectionName As String)
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakePlanetSection(planet.Properties.Subtype, sectionName)).CreateActor(location, planet.Properties.Group.Name)
        actor.Properties.Group = planet.Properties.Group
    End Sub
End Class
