Public Class BucketData(Of TEntity)
    Property Entities As New List(Of TEntity)
    Property Recycled As New HashSet(Of Integer)
End Class
