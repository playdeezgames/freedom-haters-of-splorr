Public Class LocationData_should
    Inherits EntityData_should(Of ILocationData)
    Protected Overrides Function CreateSut() As ILocationData
        Return New LocationData
    End Function
End Class
