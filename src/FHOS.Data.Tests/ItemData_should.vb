Public Class ItemData_should
    Inherits IdentifiedEntityData_should(Of IItemData)
    Public Sub New()
        MyBase.New("Item")
    End Sub
    Protected Overrides Function CreateSut() As IItemData
        Return New ItemData(Connection, 0)
    End Function
End Class

