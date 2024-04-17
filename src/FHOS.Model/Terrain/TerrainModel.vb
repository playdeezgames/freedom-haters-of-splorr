Friend Class TerrainModel
    Implements ILocationTypeModel

    Private ReadOnly cell As Persistence.ILocation

    Public Sub New(cell As Persistence.ILocation)
        Me.cell = cell
    End Sub

    Public ReadOnly Property Glyph As Char Implements ILocationTypeModel.Glyph
        Get
            Return LocationTypes.Descriptors(cell.LocationType).Glyph
        End Get
    End Property

    Public ReadOnly Property Foreground As Integer Implements ILocationTypeModel.Foreground
        Get
            Return LocationTypes.Descriptors(cell.LocationType).Foreground
        End Get
    End Property

    Public ReadOnly Property Background As Integer Implements ILocationTypeModel.Background
        Get
            Return LocationTypes.Descriptors(cell.LocationType).Background
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ILocationTypeModel.Name
        Get
            Return LocationTypes.Descriptors(cell.LocationType).Name
        End Get
    End Property
End Class
