Public Class StoreData_should
    Inherits IdentifiedEntityData_should(Of StoreData)
    Protected Overrides Function CreateSut() As StoreData
        Return New StoreData(0)
    End Function
End Class
