Public Class StoreData
    Inherits IdentifiedEntityData
    Implements IStoreData
    Public Sub New(id As Integer, Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(id, statistics:=statistics)
    End Sub
End Class
