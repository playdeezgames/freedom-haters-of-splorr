Public Class ItemData_should
    Inherits EntityData_should(Of IItemData)
    Protected Overrides Function CreateSut() As IItemData
        Return New ItemData
    End Function
End Class

