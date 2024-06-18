Public Interface IActorData
    Inherits IEntityData
    Sub AddChild(childId As Integer)
    ReadOnly Property HasChildren As Boolean
    ReadOnly Property AllChildren As IEnumerable(Of Integer)
    ReadOnly Property AllEquipment As IEnumerable(Of Integer)
    Sub AddEquipment(itemId As Integer)
    Property Inventory As HashSet(Of Integer)
    Property YokedActors As Dictionary(Of String, Integer)
    Property YokedStores As Dictionary(Of String, Integer)
    Property YokedGroups As Dictionary(Of String, Integer)
End Interface
