Public Class LocationData_should
    Inherits IdentifiedEntityData_should(Of ILocationData)
    Public Sub New()
        MyBase.New("Location")
    End Sub
    Protected Overrides Function CreateSut() As ILocationData
        Return New LocationData(Connection, 0)
    End Function
End Class
