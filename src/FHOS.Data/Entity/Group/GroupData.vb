Public Class GroupData
    Inherits IdentifiedEntityData

    Public Property Children As New HashSet(Of Integer)
    Public Property Parents As New HashSet(Of Integer)
    Public Property Values As New HashSet(Of Integer)
    Public ReadOnly Property HasValues As Boolean
        Get
            Return Values.Any
        End Get
    End Property

    Public ReadOnly Property HasChildren As Boolean
        Get
            Return Children.Any
        End Get
    End Property

    Public ReadOnly Property AllChildren As IEnumerable(Of Integer)
        Get
            Return Children
        End Get
    End Property
    Public ReadOnly Property AllValues As IEnumerable(Of Integer)
        Get
            Return Values
        End Get
    End Property

    Public ReadOnly Property HasParents As Boolean
        Get
            Return Parents.Any
        End Get
    End Property

    Public ReadOnly Property AllParents As IEnumerable(Of Integer)
        Get
            Return Parents
        End Get
    End Property

    Public Sub AddChild(childId As Integer)
        Children.Add(childId)
    End Sub

    Public Sub AddValue(valueId As Integer)
        Values.Add(valueId)
    End Sub

    Public Sub AddParent(parentId As Integer)
        Parents.Add(parentId)
    End Sub

    Public Sub RemoveChild(childId As Integer)
        Children.Remove(childId)
    End Sub

    Public Sub RemoveParent(parentId As Integer)
        Parents.Remove(parentId)
    End Sub

    Public Sub RemoveValue(valueId As Integer)
        Values.Remove(valueId)
    End Sub

    Public Function HasChild(childId As Integer) As Boolean
        Return Children.Contains(childId)
    End Function

    Public Function HasParent(parentId As Integer) As Boolean
        Return Parents.Contains(parentId)
    End Function
    Public Function HasValue(valueId As Integer) As Boolean
        Return Values.Contains(valueId)
    End Function
End Class
