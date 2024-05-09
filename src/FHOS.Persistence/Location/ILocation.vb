Public Interface ILocation
    Inherits IEntity
    ReadOnly Property Map As IMap
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    Property LocationType As String
    Property Actor As IActor
    Property Teleporter As ITeleporter
    ReadOnly Property HasTeleporter As Boolean
    Property Tutorial As String
    Property Place As IPlace
    Function CreateActor(actorType As String) As IActor
    Function CreateTeleporterTo() As ITeleporter
End Interface
