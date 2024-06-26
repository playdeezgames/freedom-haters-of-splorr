﻿Public Interface IEntityData
    Function HasFlag(flagName As String) As Boolean
    Sub SetFlag(flagName As String)
    Sub ClearFlag(flagName As String)

    Sub SetStatistic(statisticType As String, value As Integer?)
    Function GetStatistic(statisticType As String) As Integer?

    Sub SetMetadata(metadataType As String, value As String)
    Function GetMetadata(metadataType As String) As String
End Interface
