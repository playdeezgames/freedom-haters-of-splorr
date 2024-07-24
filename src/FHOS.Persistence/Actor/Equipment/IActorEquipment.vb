Public Interface IActorEquipment
    Sub Equip(equipSlot As String, item As IItem)
    ReadOnly Property All As IEnumerable(Of IItem)
    ReadOnly Property AllSlots As IEnumerable(Of String)
    Function GetSlot(equipSlot As String) As IItem
    ReadOnly Property Actor As IActor
End Interface
