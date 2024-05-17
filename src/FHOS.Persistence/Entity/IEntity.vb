Public Interface IEntity
    ReadOnly Property Id As Integer
    Function HasFlag(flag As String) As Boolean
    Property Flags(flag As String) As Boolean
End Interface
