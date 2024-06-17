Public Interface IActorData
    Inherits IEntityData
    Property Children As HashSet(Of Integer)
    Property Equipment As HashSet(Of Integer)
    Property YokedActors As Dictionary(Of String, Integer)
    Property YokedStores As Dictionary(Of String, Integer)
    Property YokedGroups As Dictionary(Of String, Integer)
    Property Inventory As HashSet(Of Integer)
End Interface
