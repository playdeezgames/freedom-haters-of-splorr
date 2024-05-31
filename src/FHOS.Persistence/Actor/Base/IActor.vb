Public Interface IActor
    Inherits IEntity
    ReadOnly Property ActorType As String
    ReadOnly Property Tutorial As IActorTutorial
    ReadOnly Property Family As IActorFamily
    ReadOnly Property Properties As IActorProperties
    ReadOnly Property State As IActorState
    ReadOnly Property Equipment As IActorEquipment
    ReadOnly Property Groups As IEnumerable(Of IGroup)
    Sub AddGroup(group As IGroup)

End Interface
