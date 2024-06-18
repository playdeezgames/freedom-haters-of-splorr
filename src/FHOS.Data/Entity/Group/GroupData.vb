Public Class GroupData
    Inherits EntityData
    Implements IGroupData
    Property Children As New HashSet(Of Integer) Implements IGroupData.Children
    Property Parents As New HashSet(Of Integer) Implements IGroupData.Parents

    Public ReadOnly Property HasChildren As Boolean Implements IGroupData.HasChildren
        Get
            Return Children.Any
        End Get
    End Property

    Public ReadOnly Property AllChildren As IEnumerable(Of Integer) Implements IGroupData.AllChildren
        Get
            Return Children
        End Get
    End Property

    Public ReadOnly Property HasParents As Boolean Implements IGroupData.HasParents
        Get
            Return Parents.Any
        End Get
    End Property

    Public ReadOnly Property AllParents As IEnumerable(Of Integer) Implements IGroupData.AllParents
        Get
            Return Parents
        End Get
    End Property

    Public Function HasChild(childId As Integer) As Boolean Implements IGroupData.HasChild
        Return Children.Contains(childId)
    End Function

    Public Function HasParent(parentId As Integer) As Boolean Implements IGroupData.HasParent
        Return Parents.Contains(parentId)
    End Function
End Class
