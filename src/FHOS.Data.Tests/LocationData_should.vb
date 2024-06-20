Public Class LocationData_should
    Inherits IdentifiedEntityData_should(Of ILocationData)
    Protected Overrides Function CreateSut() As ILocationData
        Return New LocationData(0)
    End Function
End Class
