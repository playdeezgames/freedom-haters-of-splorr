Public Interface IEntity
    ReadOnly Property Id As Integer
    Property Flags(flag As String) As Boolean
    ReadOnly Property Universe As IUniverse
    Sub Recycle()
End Interface
