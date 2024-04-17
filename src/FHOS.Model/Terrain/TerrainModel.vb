Friend Class TerrainModel
    Implements ITerrainModel

    Private ReadOnly cell As Persistence.ILocation

    Public Sub New(cell As Persistence.ILocation)
        Me.cell = cell
    End Sub

    Public ReadOnly Property Glyph As Char Implements ITerrainModel.Glyph
        Get
            Return TerrainTypes.Descriptors(cell.LocationType).Glyph
        End Get
    End Property

    Public ReadOnly Property Foreground As Integer Implements ITerrainModel.Foreground
        Get
            Return TerrainTypes.Descriptors(cell.LocationType).Foreground
        End Get
    End Property

    Public ReadOnly Property Background As Integer Implements ITerrainModel.Background
        Get
            Return TerrainTypes.Descriptors(cell.LocationType).Background
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ITerrainModel.Name
        Get
            Return TerrainTypes.Descriptors(cell.LocationType).Name
        End Get
    End Property
End Class
