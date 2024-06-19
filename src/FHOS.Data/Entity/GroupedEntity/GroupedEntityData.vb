Public Class GroupedEntityData
    Inherits EntityData
    Implements IGroupedEntityData
    Public Property YokedGroups As New Dictionary(Of String, Integer)
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics)
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
