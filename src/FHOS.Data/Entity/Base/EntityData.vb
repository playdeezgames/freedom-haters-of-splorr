Public MustInherit Class EntityData
    Implements IEntityData
    Protected ReadOnly _connection As SqliteConnection
    Public Sub New(
                  connection As SqliteConnection)
        Me._connection = connection
    End Sub
    Protected MustOverride Sub WriteDatabaseFlag(flagType As String)
    Protected MustOverride Sub ClearDatabaseFlag(flagType As String)
    Protected MustOverride Function ReadDatabaseFlag(flagType As String) As Boolean
    Protected MustOverride Sub WriteDatabaseStatistic(statisticType As String, statisticValue As Integer)
    Protected MustOverride Sub ClearDatabaseStatistic(statisticType As String)
    Protected MustOverride Function ReadDatabaseStatistic(statisticType As String) As Integer?
    Protected MustOverride Sub WriteDatabaseMetadata(metadataType As String, metadataValue As String)
    Protected MustOverride Sub ClearDatabaseMetadata(metadataType As String)
    Protected MustOverride Function ReadDatabaseMetadata(metadataType As String) As String

    Public Sub SetFlag(flagType As String) Implements IEntityData.SetFlag
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        WriteDatabaseFlag(flagType)
    End Sub

    Public Sub ClearFlag(flagType As String) Implements IEntityData.ClearFlag
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        ClearDatabaseFlag(flagType)
    End Sub

    Public Sub SetStatistic(statisticType As String, value As Integer?) Implements IEntityData.SetStatistic
        If String.IsNullOrWhiteSpace(statisticType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        If value.HasValue Then
            WriteDatabaseStatistic(statisticType, value.Value)
        Else
            ClearDatabaseStatistic(statisticType)
        End If
    End Sub

    Public Sub SetMetadata(metadataType As String, value As String) Implements IEntityData.SetMetadata
        If String.IsNullOrWhiteSpace(metadataType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        If value IsNot Nothing Then
            WriteDatabaseMetadata(metadataType, value)
        Else
            ClearDatabaseMetadata(metadataType)
        End If
    End Sub

    Public Function HasFlag(flagType As String) As Boolean Implements IEntityData.HasFlag
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        Return ReadDatabaseFlag(flagType)
    End Function

    Public Function GetStatistic(statisticType As String) As Integer? Implements IEntityData.GetStatistic
        If String.IsNullOrWhiteSpace(statisticType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        Return ReadDatabaseStatistic(statisticType)
    End Function

    Public Function GetMetadata(metadataType As String) As String Implements IEntityData.GetMetadata
        If String.IsNullOrWhiteSpace(metadataType) Then
            Throw New InvalidOperationException("metadataType null or whitespace!")
        End If
        Return ReadDatabaseMetadata(metadataType)
    End Function
End Class
