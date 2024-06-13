Public Interface IActor
    Inherits INamedEntity
    ReadOnly Property Family As IActorFamily
    ReadOnly Property Properties As IActorProperties
    ReadOnly Property Equipment As IActorEquipment
    Property YokedActor(yokeType As String) As IActor
    Property YokedStore(yokeType As String) As IStore
    Property Location As ILocation
    Property YokedGroup(yokeType As String) As IGroup
    Property Interior As IMap
    Property CostumeType As String
End Interface
