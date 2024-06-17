Public MustInherit Class EntityData
    Implements IEntityData
    Public Property Flags As New HashSet(Of String)
    Property Statistics As New Dictionary(Of String, Integer) Implements IEntityData.Statistics
    Property Metadatas As New Dictionary(Of String, String) Implements IEntityData.Metadatas

    Public Sub SetFlag(flagType As String) Implements IEntityData.SetFlag
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        Flags.Add(flagType)
    End Sub

    Public Sub ClearFlag(flagType As String) Implements IEntityData.ClearFlag
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        Flags.Remove(flagType)
    End Sub

    Public Sub SetStatistic(statisticType As String, value As Integer?) Implements IEntityData.SetStatistic
        If String.IsNullOrWhiteSpace(statisticType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        If value.HasValue Then
            Statistics(statisticType) = value.Value
        Else
            Statistics.Remove(statisticType)
        End If
    End Sub

    Public Function HasFlag(flagType As String) As Boolean Implements IEntityData.HasFlag
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        Return Flags.Contains(flagType)
    End Function

    Public Function GetStatistic(statisticType As String) As Integer? Implements IEntityData.GetStatistic
        If String.IsNullOrWhiteSpace(statisticType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        Dim result As Integer
        If Statistics.TryGetValue(statisticType, result) Then
            Return result
        End If
        Return Nothing
    End Function
End Class
