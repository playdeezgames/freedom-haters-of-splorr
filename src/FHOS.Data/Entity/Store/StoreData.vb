Public Class StoreData
    Inherits IdentifiedEntityData
    Implements IStoreData
    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, "Store", id, metadatas:=metadatas)
    End Sub
End Class
