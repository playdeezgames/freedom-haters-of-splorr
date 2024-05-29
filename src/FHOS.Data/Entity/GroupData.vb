Public Class GroupData
    Inherits EntityData
    Property Children As New HashSet(Of Integer)
    Property Parents As New HashSet(Of Integer)
End Class
