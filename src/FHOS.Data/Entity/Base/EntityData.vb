Public MustInherit Class EntityData
    Public Sub New(
                  Optional flags As ISet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        Me.Statistics = If(statistics IsNot Nothing, New Dictionary(Of String, Integer)(statistics), New Dictionary(Of String, Integer))
        Me.Flags = If(flags IsNot Nothing, New HashSet(Of String)(flags), New HashSet(Of String))
        Me.Metadatas = If(metadatas IsNot Nothing, New Dictionary(Of String, String)(metadatas), New Dictionary(Of String, String))
    End Sub
    Public Property Flags As New HashSet(Of String)
    Public Property Statistics As New Dictionary(Of String, Integer)
    Public Property Metadatas As New Dictionary(Of String, String)

    Public Sub SetFlag(flagType As String)
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        Flags.Add(flagType)
    End Sub

    Public Sub ClearFlag(flagType As String)
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        Flags.Remove(flagType)
    End Sub

    Public Sub SetStatistic(statisticType As String, value As Integer?)
        If String.IsNullOrWhiteSpace(statisticType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        If value.HasValue Then
            Statistics(statisticType) = value.Value
        Else
            Statistics.Remove(statisticType)
        End If
    End Sub

    Public Sub SetMetadata(metadataType As String, value As String)
        If String.IsNullOrWhiteSpace(metadataType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        If value IsNot Nothing Then
            Metadatas(metadataType) = value
        Else
            Metadatas.Remove(metadataType)
        End If
    End Sub

    Public Function HasFlag(flagType As String) As Boolean
        If String.IsNullOrWhiteSpace(flagType) Then
            Throw New InvalidOperationException("flagType null or whitespace!")
        End If
        Return Flags.Contains(flagType)
    End Function

    Public Function GetStatistic(statisticType As String) As Integer?
        If String.IsNullOrWhiteSpace(statisticType) Then
            Throw New InvalidOperationException("statisticType null or whitespace!")
        End If
        Dim result As Integer
        If Statistics.TryGetValue(statisticType, result) Then
            Return result
        End If
        Return Nothing
    End Function

    Public Function GetMetadata(metadataType As String) As String
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
