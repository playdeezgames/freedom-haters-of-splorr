Imports System.Text.Json.Serialization

Public Class BucketData(Of TEntity)
    Property Entities As New List(Of TEntity)
    Property Recycled As New HashSet(Of Integer)
    Function CreateOrRecycle(data As TEntity) As Integer
        Dim entityId As Integer
        If Recycled.Any Then
            entityId = Recycled.First
            Recycled.Remove(entityId)
            Entities(entityId) = data
        Else
            entityId = Entities.Count
            Entities.Add(data)
        End If
        Return entityId
    End Function
End Class
