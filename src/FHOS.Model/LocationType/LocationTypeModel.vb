Friend Class LocationTypeModel
    Implements ILocationTypeModel

    Private ReadOnly location As Persistence.ILocation

    Public Sub New(location As Persistence.ILocation)
        Me.location = location
    End Sub

    Public ReadOnly Property Glyph As Char Implements ILocationTypeModel.Glyph
        Get
            Return LocationTypes.Descriptors(location.LocationType).Glyph
        End Get
    End Property

    Public ReadOnly Property Foreground As Integer Implements ILocationTypeModel.Foreground
        Get
            Return LocationTypes.Descriptors(location.LocationType).Foreground
        End Get
    End Property

    Public ReadOnly Property Background As Integer Implements ILocationTypeModel.Background
        Get
            Return LocationTypes.Descriptors(location.LocationType).Background
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ILocationTypeModel.Name
        Get
            Return LocationTypes.Descriptors(location.LocationType).Name
        End Get
    End Property
End Class
