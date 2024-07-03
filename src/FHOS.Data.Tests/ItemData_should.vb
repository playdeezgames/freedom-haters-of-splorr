Public Class ItemData_should
    Inherits IdentifiedEntityData_should(Of ItemData)
    Protected Overrides Function CreateSut() As ItemData
        Return New ItemData(0)
    End Function
End Class

