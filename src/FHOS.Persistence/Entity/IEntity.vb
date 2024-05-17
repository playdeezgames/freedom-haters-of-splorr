Public Interface IEntity
    ReadOnly Property Id As Integer
    Sub SetFlag(flag As String)
    Function HasFlag(flag As String) As Boolean
    Sub ClearFlag(flag As String)
    Property Flags(flag As String) As Boolean
End Interface
