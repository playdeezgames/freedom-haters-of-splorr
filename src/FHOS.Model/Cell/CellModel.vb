Friend Class CellModel
    Implements ICellModel

    Private ReadOnly cell As Persistence.ICell

    Public Sub New(world As Persistence.IWorld, boardPosition As (X As Integer, Y As Integer))
        Dim mapPosition As (X As Integer, Y As Integer) = (boardPosition.X + world.Avatar.Cell.Column, boardPosition.Y + world.Avatar.Cell.Row)
        Me.cell = world.Avatar.Cell.Map.GetCell(mapPosition.X, mapPosition.Y)
    End Sub

    Public ReadOnly Property Exists As Boolean Implements ICellModel.Exists
        Get
            Return cell IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property Terrain As ITerrainModel Implements ICellModel.Terrain
        Get
            If Not Exists Then
                Return Nothing
            End If
            Return New TerrainModel(cell)
        End Get
    End Property

    Public ReadOnly Property Character As ICharacterModel Implements ICellModel.Character
        Get
            Dim cellCharacter = cell?.Character
            If cellCharacter Is Nothing Then
                Return Nothing
            End If
            Return New CharacterModel(cell.Character)
        End Get
    End Property
End Class
