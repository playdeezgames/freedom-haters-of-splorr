Public Interface IActorData
    Inherits IEntityData
    Property LegacyChildren As HashSet(Of Integer)
    Sub AddChild(childId As Integer)
    ReadOnly Property HasChildren As Boolean
    ReadOnly Property Children As IEnumerable(Of Integer)
    Property LegacyEquipment As HashSet(Of Integer)
    Property LegacyInventory As HashSet(Of Integer)
    Property YokedActors As Dictionary(Of String, Integer)
    Property YokedStores As Dictionary(Of String, Integer)
    Property YokedGroups As Dictionary(Of String, Integer)
End Interface
