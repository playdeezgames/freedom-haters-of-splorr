Friend Class Item
    Inherits ItemDataClient
    Implements IItem

    Public Sub New(universeData As Data.IUniverseData, entityId As Integer)
        MyBase.New(universeData, entityId)
    End Sub

    Friend Shared Function FromId(universeData As Data.IUniverseData, id As Integer) As IItem
        Return New Item(universeData, id)
    End Function
End Class
