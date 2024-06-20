Public Class GroupData
    Inherits IdentifiedEntityData
    Implements IGroupData
    Public Sub New(
                  id As Integer,
                  Optional flags As HashSet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(id, statistics:=statistics, flags:=flags, metadatas:=metadatas)
    End Sub

    Property Children As New HashSet(Of Integer)
    Property Parents As New HashSet(Of Integer)

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

    Public Sub AddChild(childId As Integer) Implements IGroupData.AddChild
        Children.Add(childId)
    End Sub

    Public Sub AddParent(parentId As Integer) Implements IGroupData.AddParent
        Parents.Add(parentId)
    End Sub

    Public Sub RemoveChild(childId As Integer) Implements IGroupData.RemoveChild
        Children.Remove(childId)
    End Sub

    Public Sub RemoveParent(parentId As Integer) Implements IGroupData.RemoveParent
        Parents.Remove(parentId)
    End Sub

    Public Function HasChild(childId As Integer) As Boolean Implements IGroupData.HasChild
        Return Children.Contains(childId)
    End Function

    Public Function HasParent(parentId As Integer) As Boolean Implements IGroupData.HasParent
        Return Parents.Contains(parentId)
    End Function
End Class
