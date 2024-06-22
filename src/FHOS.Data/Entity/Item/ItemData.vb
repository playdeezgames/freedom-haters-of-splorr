Public Class ItemData
    Inherits IdentifiedEntityData
    Implements IItemData

    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer)
        MyBase.New(connection, "Item", id)
    End Sub
End Class
