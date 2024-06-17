Public Interface IEntityData
    Property Flags As HashSet(Of String)
    Function HasFlag(flagName As String) As Boolean
    Sub SetFlag(flagName As String)
    Sub ClearFlag(flagName As String)
    'Sub ClearFlags()
    Property Statistics As Dictionary(Of String, Integer)
    'Sub SetStatistic(statisticType As String, value As Integer?)
    'Function GetStatistic(statisticType As String) As Integer?
    Property Metadatas As Dictionary(Of String, String)
    'Sub SetMetadata(metadataType As String, value As String)
    'Function GetMetadata(metadataType As String) As String
End Interface
