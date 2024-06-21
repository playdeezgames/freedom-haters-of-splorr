Public Class StoreData_should
    Inherits IdentifiedEntityData_should(Of IStoreData)

    Public Sub New()
        MyBase.New("Store")
    End Sub

    Protected Overrides Function CreateSut() As IStoreData
        Return New StoreData(Connection, 0)
    End Function
End Class
