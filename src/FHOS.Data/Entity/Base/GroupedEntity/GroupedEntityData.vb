Public MustInherit Class GroupedEntityData
    Inherits IdentifiedEntityData
    Implements IGroupedEntityData
    Public Property YokedGroups As New Dictionary(Of String, Integer)
    Public Sub New(
                  id As Integer,
                  Optional flags As HashSet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(id, statistics:=statistics, flags:=flags, metadatas:=metadatas)
    End Sub

    Public Sub SetYokedGroup(yokeType As String, groupId As Integer?) Implements IGroupedEntityData.SetYokedGroup
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        If groupId.HasValue Then
            YokedGroups(yokeType) = groupId.Value
        Else
            YokedGroups.Remove(yokeType)
        End If
    End Sub

    Public Function GetYokedGroup(yokeType As String) As Integer? Implements IGroupedEntityData.GetYokedGroup
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        Dim groupId As Integer
        If YokedGroups.TryGetValue(yokeType, groupId) Then
            Return groupId
        End If
        Return Nothing
    End Function
End Class
