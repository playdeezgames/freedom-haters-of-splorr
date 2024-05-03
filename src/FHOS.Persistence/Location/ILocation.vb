Public Interface ILocation
    ReadOnly Property Id As Integer
    ReadOnly Property Map As IMap
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    Property LocationType As String
    Property Actor As IActor
    Property Teleporter As ITeleporter
    ReadOnly Property HasTeleporter As Boolean
    Property Tutorial As String
    Sub SetFlag(flag As String)
    Function HasFlag(name As String) As Boolean
    Property LegacyStarVicinity As IStarVicinity
    Property LegacyPlanet As IPlanet
    Property LegacySatellite As ISatellite
    Property LegacyStar As IStar
    Property Place As IPlace
    Function CreateActor(actorType As String) As IActor
    Function CreateTeleporterTo() As ITeleporter
End Interface
