Public Class StoreData
    Inherits EntityData
    Implements IStoreData
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics:=statistics)
    End Sub
End Class
