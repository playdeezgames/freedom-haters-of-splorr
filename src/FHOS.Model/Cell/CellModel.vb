Friend Class CellModel
    Implements ILocationModel

    Private ReadOnly cell As Persistence.ILocation

    Public Sub New(world As Persistence.IUniverse, boardPosition As (X As Integer, Y As Integer))
        Dim mapPosition As (X As Integer, Y As Integer) = (boardPosition.X + world.Avatar.Location.Column, boardPosition.Y + world.Avatar.Location.Row)
        Me.cell = world.Avatar.Location.Map.GetLocation(mapPosition.X, mapPosition.Y)
    End Sub

    Public ReadOnly Property Exists As Boolean Implements ILocationModel.Exists
        Get
            Return cell IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property Terrain As ITerrainModel Implements ILocationModel.Terrain
        Get
            If Not Exists Then
                Return Nothing
            End If
            Return New TerrainModel(cell)
        End Get
    End Property

    Public ReadOnly Property Character As ICharacterModel Implements ILocationModel.Character
        Get
            Dim cellCharacter = cell?.Actor
            If cellCharacter Is Nothing Then
                Return Nothing
            End If
            Return New CharacterModel(cellCharacter)
        End Get
    End Property

    Public ReadOnly Property StarSystem As IStarSystemModel Implements ILocationModel.StarSystem
        Get
            Dim cellStarSystem = cell?.StarSystem
            If cellStarSystem Is Nothing Then
                Return Nothing
            End If
            Return New StarSystemModel(cellStarSystem)
        End Get
    End Property

    Public ReadOnly Property HasDetails As Boolean Implements ILocationModel.HasDetails
        Get
            Return StarSystem IsNot Nothing OrElse Character IsNot Nothing
        End Get
    End Property
End Class
