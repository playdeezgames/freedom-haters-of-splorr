Public Class GroupData
    Inherits EntityData
    Implements IGroupData
    Property Children As New HashSet(Of Integer) Implements IGroupData.Children
    Property Parents As New HashSet(Of Integer) Implements IGroupData.Parents
End Class
