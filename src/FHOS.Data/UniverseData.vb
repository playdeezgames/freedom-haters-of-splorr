Public Class UniverseData
    Inherits EntityData
    Property Maps As New List(Of MapData)
    Property RecycledMaps As New HashSet(Of Integer)

    Property Actors As New List(Of ActorData)
    Property RecycledActors As New HashSet(Of Integer)

    Property Locations As New List(Of LocationData)
    Property RecycledLocations As New HashSet(Of Integer)

    Property StarSystems As New List(Of StarSystemData)
    Property RecycledStarSystems As New HashSet(Of Integer)

    Property Teleporters As New List(Of TeleporterData)
    Property RecycledTeleporters As New HashSet(Of Integer)
End Class
