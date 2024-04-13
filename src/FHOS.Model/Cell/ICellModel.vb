Public Interface ICellModel
    ReadOnly Property Exists As Boolean
    ReadOnly Property Terrain As ITerrainModel
    ReadOnly Property Character As ICharacterModel
    ReadOnly Property StarSystem As IStarSystemModel
    ReadOnly Property HasDetails As Boolean
End Interface
