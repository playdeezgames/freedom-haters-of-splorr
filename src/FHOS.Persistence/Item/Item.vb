Imports Microsoft.Data.Sqlite

Friend Class Item
    Inherits ItemDataClient
    Implements IItem

    Public Sub New(universeData As Data.IUniverseData, connection As SqliteConnection, entityId As Integer)
        MyBase.New(universeData, connection, entityId)
    End Sub

    Friend Shared Function FromId(universeData As Data.IUniverseData, connection As SqliteConnection, id As Integer) As IItem
        Return New Item(universeData, connection, id)
    End Function
End Class
