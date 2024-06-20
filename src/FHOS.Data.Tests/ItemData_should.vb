Public Class ItemData_should
    Inherits IdentifiedEntityData_should(Of IItemData)
    Protected Overrides Function CreateSut() As IItemData
        Return New ItemData(0)
    End Function
End Class

