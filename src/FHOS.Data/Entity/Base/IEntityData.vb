Public Interface IEntityData
    Property Flags As HashSet(Of String)
    Property Statistics As Dictionary(Of String, Integer)
    Property Metadatas As Dictionary(Of String, String)
End Interface
