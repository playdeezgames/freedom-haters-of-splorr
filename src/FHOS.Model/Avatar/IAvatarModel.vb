Public Interface IAvatarModel
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    ReadOnly Property MapName As String
    ReadOnly Property HasActions As Boolean

    Sub Move(delta As (X As Integer, Y As Integer))
    Sub SetFacing(facing As Integer)
    Sub DoDistressSignal()
    ReadOnly Property KnowsPlanetVicinities As Boolean
    ReadOnly Property Turn As Integer

    ReadOnly Property Jools As Integer
    ReadOnly Property MinimumJools As Integer

    ReadOnly Property OxygenPercent As Integer
    ReadOnly Property OxygenHue As Integer

    ReadOnly Property IsGameOver As Boolean
    ReadOnly Property IsDead As Boolean
    ReadOnly Property IsBankrupt As Boolean

    ReadOnly Property FuelPercent As Integer
    ReadOnly Property FuelHue As Integer
    ReadOnly Property CanMove As Boolean

    ReadOnly Property Tutorial As IAvatarTutorialModel

    ReadOnly Property Place As IAvatarPlaceModel
    ReadOnly Property LegacyStarSystem As IAvatarStarSystemModel
    ReadOnly Property LegacyStarVicinity As IAvatarStarVicinityModel
    ReadOnly Property LegacyStar As IAvatarStarModel

    ReadOnly Property LegacyPlanetVicinity As IAvatarPlanetVicinityModel
    ReadOnly Property LegacyPlanet As IAvatarPlanetModel
    ReadOnly Property LegacySatellite As IAvatarSatelliteModel
    ReadOnly Property KnowsStarSystems As Boolean
    ReadOnly Property StarSystemList As IEnumerable(Of (Text As String, Item As IStarSystemModel))
    ReadOnly Property KnowsStarVicinities As Boolean
    ReadOnly Property StarVicinityList As IEnumerable(Of (Text As String, Item As IStarVicinityModel))
    ReadOnly Property PlanetVicinityList As IEnumerable(Of (Text As String, Item As IPlanetVicinityModel))
    ReadOnly Property KnowsPlaces As Boolean
End Interface
