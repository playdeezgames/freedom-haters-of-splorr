Public Class StoreData
    Inherits IdentifiedEntityData
    Implements IStoreData
    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer)
        MyBase.New(connection, "Store", id)
    End Sub
End Class
