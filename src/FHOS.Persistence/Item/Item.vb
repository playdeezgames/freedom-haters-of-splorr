Friend Class Item
    Inherits ItemDataClient
    Implements IItem

    Public Sub New(universeData As Data.UniverseData, entityId As Integer)
        MyBase.New(universeData, entityId)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IItem
        Return New Item(universeData, id)
    End Function
End Class
