Imports System.Text.Json.Serialization

Public Class BucketData(Of TEntity)
    <JsonPropertyName("e1")>
    Property Entities As New List(Of TEntity)
    <JsonPropertyName("r1")>
    Property Recycled As New HashSet(Of Integer)
End Class
