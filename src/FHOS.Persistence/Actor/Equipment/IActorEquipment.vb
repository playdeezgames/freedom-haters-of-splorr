Public Interface IActorEquipment
    Sub Equip(equipSlot As String, item As IItem)
    ReadOnly Property All As IEnumerable(Of IItem)
End Interface
