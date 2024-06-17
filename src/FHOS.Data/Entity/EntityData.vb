Public MustInherit Class EntityData
    Implements IEntityData
    Property Flags As New HashSet(Of String) Implements IEntityData.Flags
    Property Statistics As New Dictionary(Of String, Integer) Implements IEntityData.Statistics
    Property Metadatas As New Dictionary(Of String, String) Implements IEntityData.Metadatas
End Class
