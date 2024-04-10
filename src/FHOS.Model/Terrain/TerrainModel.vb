Friend Class TerrainModel
    Implements ITerrainModel

    Private ReadOnly cell As Persistence.ICell

    Public Sub New(cell As Persistence.ICell)
        Me.cell = cell
    End Sub

    Public ReadOnly Property Glyph As Char Implements ITerrainModel.Glyph
        Get
            Return "."c
        End Get
    End Property

    Public ReadOnly Property Hue As Integer Implements ITerrainModel.Hue
        Get
            Return 8
        End Get
    End Property
End Class
