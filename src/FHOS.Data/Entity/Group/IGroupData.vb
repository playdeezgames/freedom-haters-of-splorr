Public Interface IGroupData
    Inherits IEntityData
    Property Children As HashSet(Of Integer)
    Property Parents As HashSet(Of Integer)
End Interface
