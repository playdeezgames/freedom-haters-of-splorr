Public Interface IEntity
    ReadOnly Property Id As Integer
    Property Flags(flag As String) As Boolean
    ReadOnly Property Universe As IUniverse
    Sub Recycle()
    Property Statistics(statisticType As String) As Integer?
    Property Metadatas(metadataType As String) As String
End Interface
