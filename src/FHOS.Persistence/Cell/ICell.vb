Public Interface ICell
    ReadOnly Property Id As Integer
    ReadOnly Property Map As IMap
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    Property TerrainType As String
    Property Character As ICharacter
    Property StarSystem As IStarSystem
    Property Teleporter As ITeleporter
    ReadOnly Property HasTeleporter As Boolean
    Property Tutorial As String
    Sub SetFlag(flag As String)
    Function HasFlag(name As String) As Boolean
End Interface
