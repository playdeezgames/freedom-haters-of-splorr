Public Interface IAvatarModel
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    ReadOnly Property MapName As String
    ReadOnly Property HasActions As Boolean

    Sub Move(delta As (X As Integer, Y As Integer))
    Sub SetFacing(facing As Integer)

    ReadOnly Property OxygenPercent As Integer
    ReadOnly Property OxygenHue As Integer
    ReadOnly Property IsDead As Boolean

    ReadOnly Property FuelPercent As Integer
    ReadOnly Property FuelHue As Integer
    ReadOnly Property CanMove As Boolean

    ReadOnly Property Tutorial As IAvatarTutorialModel

    ReadOnly Property StarSystem As IAvatarStarSystemModel
    ReadOnly Property StarVicinity As IAvatarStarVicinityModel
    ReadOnly Property Star As IAvatarStarModel

    ReadOnly Property PlanetVicinity As IAvatarPlanetVicinityModel
    ReadOnly Property Planet As IAvatarPlanetModel
    ReadOnly Property KnowsStarSystems As Boolean
    ReadOnly Property StarSystemList As IEnumerable(Of (Text As String, Item As Integer))
End Interface
