Public Interface IActor
    Inherits INamedEntity
    ReadOnly Property Family As IActorFamily
    ReadOnly Property Equipment As IActorEquipment
    Property Location As ILocation
    Property Interior As IMap
    Property Costume As String
    ReadOnly Property Yokes As IActorYokes
End Interface
