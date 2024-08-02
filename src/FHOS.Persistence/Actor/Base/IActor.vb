Public Interface IActor
    Inherits INamedEntity
    ReadOnly Property Equipment As IActorEquipment
    Property Location As ILocation
    Property Interior As IMap
    Property Costume As String
    ReadOnly Property Yokes As IActorYokes
    ReadOnly Property Inventory As IActorInventory
    ReadOnly Property Offers As IActorOffers
    ReadOnly Property Prices As IActorPrices
    Function GetReputation(group As IGroup) As Integer?
    Sub SetReputation(group As IGroup, reputation As Integer?)
End Interface
