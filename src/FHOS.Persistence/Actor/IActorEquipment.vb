Public Interface IActorEquipment
    Sub Equip(item As IItem)
    ReadOnly Property All As IEnumerable(Of IItem)
End Interface
