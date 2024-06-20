Public Class StoreData_should
    Inherits IdentifiedEntityData_should(Of IStoreData)
    Protected Overrides Function CreateSut() As IStoreData
        Return New StoreData(Connection, 0)
    End Function
End Class
