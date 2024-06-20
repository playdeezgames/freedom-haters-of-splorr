Public Interface IGroupData
    Inherits IIdentifiedEntityData
    ReadOnly Property HasChildren As Boolean
    ReadOnly Property AllChildren As IEnumerable(Of Integer)
    Sub AddChild(childId As Integer)
    Sub RemoveChild(childId As Integer)
    Function HasChild(childId As Integer) As Boolean
    ReadOnly Property HasParents As Boolean
    ReadOnly Property AllParents As IEnumerable(Of Integer)
    Sub AddParent(parentId As Integer)
    Sub RemoveParent(parentId As Integer)
    Function HasParent(parentId As Integer) As Boolean
End Interface
