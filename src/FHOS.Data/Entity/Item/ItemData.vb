Public Class ItemData
    Inherits IdentifiedEntityData
    Implements IItemData

    Public Sub New(id As Integer, Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(id, statistics)
    End Sub
End Class
