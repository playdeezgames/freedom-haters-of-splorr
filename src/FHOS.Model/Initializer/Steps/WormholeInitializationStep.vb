Imports SPLORR.Game

Friend Class WormholeInitializationStep
    Inherits InitializationStep

    Private ReadOnly startLocation As Persistence.ILocation
    Private ReadOnly starSystem As Persistence.IPlace

    Public Sub New(startLocation As Persistence.ILocation, starSystem As Persistence.IPlace)
        Me.startLocation = startLocation
        Me.starSystem = starSystem
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        Dim wormhole = startLocation.Place
        Dim destinationLocation = RNG.FromEnumerable(starSystem.Properties.Map.Locations.Where(Function(x) LocationTypes.Descriptors(x.LocationType).CanPlaceWormhole))
        wormhole.Properties.WormholeDestination = destinationLocation
        destinationLocation.LocationType = LocationTypes.Wormhole
        destinationLocation.Tutorial = TutorialTypes.WormholeEntry
        destinationLocation.Place = wormhole.Universe.Factory.CreateWormhole("Nexus Wormhole")
        destinationLocation.Place.Properties.WormholeDestination = startLocation
    End Sub
End Class
