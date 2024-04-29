Friend Class PlanetInitializationStep
    Inherits InitializationStep

    Private ReadOnly location As Persistence.ILocation

    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep))
        'TODO: ???
    End Sub
End Class
