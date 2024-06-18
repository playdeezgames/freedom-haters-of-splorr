Public Class StoreData_should
    Inherits EntityData_should(Of IStoreData)
    Protected Overrides Function CreateSut() As IStoreData
        Return New StoreData
    End Function
End Class
