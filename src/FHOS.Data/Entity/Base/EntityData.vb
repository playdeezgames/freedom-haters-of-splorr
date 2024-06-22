Public MustInherit Class EntityData
    Implements IEntityData
    Protected ReadOnly _connection As SqliteConnection
    Public Sub New(
                  connection As SqliteConnection,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        Me._connection = connection
        Me.Statistics = If(statistics IsNot Nothing, New Dictionary(Of String, Integer)(statistics), New Dictionary(Of String, Integer))
        Me.Metadatas = If(metadatas IsNot Nothing, New Dictionary(Of String, String)(metadatas), New Dictionary(Of String, String))
    End Sub
    Public Property Statistics As Dictionary(Of String, Integer)
    Public Property Metadatas As New Dictionary(Of String, String)
    Protected MustOverride Sub WriteDatabaseFlag(flagType As String)
    Protected MustOverride Sub ClearDatabaseFlag(flagType As String)
    Protected MustOverride Function ReadDatabaseFlag(flagType As String) As Boolean
    Protected MustOverride Sub WriteDatabaseStatistic(statisticType As String, statisticValue As Integer)
    Protected MustOverride Sub ClearDatabaseStatistic(statisticType As String)
    'Protected MustOverride Function ReadDatabaseStatistic(statisticType As String) As Integer?

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
            Statistics(statisticType) = value.Value
            WriteDatabaseStatistic(statisticType, value.Value)
        Else
            Statistics.Remove(statisticType)
            ClearDatabaseStatistic(statisticType)
        End If
    End Sub

    Public Sub SetMetadata(metadataType As String, value As String) Implements IEntityData.SetMetadata
        If String.IsNullOrWhiteSpace(metadataType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        If value IsNot Nothing Then
            Metadatas(metadataType) = value
        Else
            Metadatas.Remove(metadataType)
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
        'Return ReadDatabaseStatistic(statisticType)
        Dim result As Integer
        If Statistics.TryGetValue(statisticType, result) Then
            Return result
        End If
        Return Nothing
    End Function

    Public Function GetMetadata(metadataType As String) As String Implements IEntityData.GetMetadata
        If String.IsNullOrWhiteSpace(metadataType) Then
            Throw New InvalidOperationException("metadataType null or whitespace!")
        End If
        Dim result As String = Nothing
        If Metadatas.TryGetValue(metadataType, result) Then
            Return result
        End If
        Return Nothing
    End Function
End Class
