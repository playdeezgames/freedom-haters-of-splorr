Public Interface IGroup
    Inherits ITypedEntity

    ReadOnly Property LegacyEntityName As String

    ReadOnly Property Parents As IEnumerable(Of IGroup)
    Sub AddParent(parent As IGroup)
    Sub RemoveParent(parent As IGroup)
    ReadOnly Property Children As IEnumerable(Of IGroup)
End Interface
