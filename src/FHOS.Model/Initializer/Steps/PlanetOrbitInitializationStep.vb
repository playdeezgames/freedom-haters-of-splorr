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
        Dim map = MapTypes.Descriptors(MapTypes.PlanetOrbit).CreateMap($"{planet.Yokes.YokedGroup(YokeTypes.Planet).EntityName} Orbit", planet.Universe)
        map.YokedGroup(YokeTypes.Planet) = planetGroup
        planet.Interior = map
        For Each actor In actors
            actor.Interior = map
        Next
        Dim targetActor = actors.OrderBy(Function(x) x.Location.Position.Column).ThenBy(Function(x) x.Location.Position.Row).First()
        PlaceBoundaryActors(targetActor, planet.Interior.Size.Columns, planet.Interior.Size.Rows)
        PlacePlanet(planet)
        addStep(New EncounterInitializationStep(planet.Interior), True)
    End Sub
    Private Sub PlacePlanet(externalActor As IActor)
        Dim planetCenterColumn = externalActor.Interior.Size.Columns \ 2
        Dim planetCenterRow = externalActor.Interior.Size.Rows \ 2
        For Each delta In Grid5x5.Descriptors
            PlacePlanetSection(
                externalActor,
                externalActor.Interior.GetLocation(
                    planetCenterColumn + delta.Value.Delta.X,
                    planetCenterRow + delta.Value.Delta.Y),
                delta.Key)
        Next
    End Sub
    Private Shared Sub PlacePlanetSection(externalActor As IActor, location As ILocation, sectionName As String)
        Dim actor = ActorTypes.Descriptors(ActorTypes.MakePlanetSection(externalActor.Descriptor.Subtype, sectionName)).CreateActor(location, externalActor.Yokes.YokedGroup(YokeTypes.Planet).EntityName)
        location.EntityType = LocationTypes.Planet
        actor.Yokes.YokedGroup(YokeTypes.Planet) = externalActor.Yokes.YokedGroup(YokeTypes.Planet)
    End Sub
End Class
