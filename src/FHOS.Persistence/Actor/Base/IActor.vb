Public Interface IActor
    Inherits INamedEntity
    ReadOnly Property Family As IActorFamily
    ReadOnly Property Equipment As IActorEquipment
    Property Location As ILocation
    Property Interior As IMap
    Property Costume As String
    'yokes
    Property YokedActor(yokeType As String) As IActor
    Property YokedGroup(yokeType As String) As IGroup
    Property YokedStore(yokeType As String) As IStore
End Interface
