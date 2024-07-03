Public Class LocationData_should
    Inherits IdentifiedEntityData_should(Of LocationData)
    Protected Overrides Function CreateSut() As LocationData
        Return New LocationData With {.Id = 0}
    End Function
End Class
