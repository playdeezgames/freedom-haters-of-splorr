Imports System.Text.RegularExpressions
Imports FHOS.Persistence

Friend Class PlanetOrbitInitializationStep
    Inherits InitializationStep
    Private ReadOnly location As ILocation
    Private ReadOnly planetGroup As IGroup
    Public Sub New(location As ILocation, planetGroup As IGroup)
        Me.location = location
        Me.planetGroup = planetGroup
    End Sub
    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim planet = location.Actor
        Dim actors = location.Map.Locations.Where(Function(x) If(x.Actor?.Descriptor?.IsPlanet, False)).Select(Function(x) x.Actor)
        Dim map = MapTypes.Descriptors(MapTypes.PlanetOrbit).CreateMap($"{planet.YokedGroup(YokeTypes.Planet).EntityName} Orbit", planet.Universe)
        map.YokedGroup(YokeTypes.Planet) = planetGroup
        planet.Properties.Interior = map
        For Each actor In actors
            actor.Properties.Interior = map
        Next
        Dim targetActor = actors.OrderBy(Function(x) x.Location.Position.Column).ThenBy(Function(x) x.Location.Position.Row).First()
        PlaceBoundaryActors(targetActor, planet.Properties.Interior.Size.Columns, planet.Properties.Interior.Size.Rows)
        PlacePlanet(planet)
        addStep(New EncounterInitializationStep(planet.Properties.Interior), True)
    End Sub
    Private Sub PlacePlanet(externalActor As IActor)
        Dim planetCenterColumn = externalActor.Properties.Interior.Size.Columns \ 2
        Dim planetCenterRow = externalActor.Properties.Interior.Size.Rows \ 2
        For Each delta In Grid5x5.Descriptors
            PlacePlanetSection(
                externalActor,
                externalActor.Properties.Interior.GetLocation(
                    planetCenterColumn + delta.Value.Delta.X,
                    planetCenterRow + delta.Value.Delta.Y),
                delta.Key)
        Next
    End Sub
    Private Shared Sub PlacePlanetSection(externalActor As IActor, location As ILocation, sectionName As String)
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakePlanetSection(externalActor.Descriptor.Subtype, sectionName)).CreateActor(location, externalActor.YokedGroup(YokeTypes.Planet).EntityName)
        location.EntityType = LocationTypes.Planet
        actor.YokedGroup(YokeTypes.Planet) = externalActor.YokedGroup(YokeTypes.Planet)
    End Sub
End Class
