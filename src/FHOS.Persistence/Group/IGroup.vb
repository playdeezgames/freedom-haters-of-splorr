Public Interface IGroup
    Inherits INamedEntity
    ReadOnly Property Parents As IEnumerable(Of IGroup)
    Sub AddParent(parent As IGroup)
    Sub RemoveParent(parent As IGroup)
    ReadOnly Property Children As IEnumerable(Of IGroup)
End Interface
