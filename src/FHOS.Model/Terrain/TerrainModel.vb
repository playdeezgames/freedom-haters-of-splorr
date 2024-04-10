Friend Class TerrainModel
    Implements ITerrainModel

    Private ReadOnly cell As Persistence.ICell

    Public Sub New(cell As Persistence.ICell)
        Me.cell = cell
    End Sub

    Public ReadOnly Property Glyph As Char Implements ITerrainModel.Glyph
        Get
            Return TerrainTypes.Descriptors(cell.TerrainType).Glyph
        End Get
    End Property

    Public ReadOnly Property Foreground As Integer Implements ITerrainModel.Foreground
        Get
            Return TerrainTypes.Descriptors(cell.TerrainType).Foreground
        End Get
    End Property

    Public ReadOnly Property Background As Integer Implements ITerrainModel.Background
        Get
            Return TerrainTypes.Descriptors(cell.TerrainType).Background
        End Get
    End Property
End Class
