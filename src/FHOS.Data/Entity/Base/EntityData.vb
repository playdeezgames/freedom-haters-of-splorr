Public MustInherit Class EntityData
    Implements IEntityData
    Property Flags As New HashSet(Of String) Implements IEntityData.Flags
    Property Statistics As New Dictionary(Of String, Integer) Implements IEntityData.Statistics
    Property Metadatas As New Dictionary(Of String, String) Implements IEntityData.Metadatas

    Public Sub SetFlag(flagName As String) Implements IEntityData.SetFlag
        If String.IsNullOrWhiteSpace(flagName) Then
            Throw New InvalidOperationException("flagName null or whitespace!")
        End If
        Flags.Add(flagName)
    End Sub

    Public Sub ClearFlag(flagName As String) Implements IEntityData.ClearFlag
        Flags.Remove(flagName)
    End Sub

    Public Function HasFlag(flagName As String) As Boolean Implements IEntityData.HasFlag
        Return Flags.Contains(flagName)
    End Function
End Class
