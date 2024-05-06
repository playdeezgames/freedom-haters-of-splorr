Friend Class PlanetInitializationStep
    Inherits InitializationStep

    Private ReadOnly centerLocation As Persistence.ILocation

    Public Sub New(centerLocation As Persistence.ILocation)
        Me.centerLocation = centerLocation
    End Sub

    Public Overrides Sub DoStep(addStep As Action(Of InitializationStep, Boolean))
        'TODO: ???
    End Sub
End Class
