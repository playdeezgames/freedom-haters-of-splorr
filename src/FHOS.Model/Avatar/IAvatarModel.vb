Public Interface IAvatarModel
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    ReadOnly Property MapName As String
    ReadOnly Property HasVerbs As Boolean

    Sub LegacyMove(delta As (X As Integer, Y As Integer))
    Sub LegacySetFacing(facing As Integer)
    Sub Move(facing As Integer, delta As (X As Integer, Y As Integer))
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

    ReadOnly Property CurrentPlace As IPlaceModel
    ReadOnly Property AvailableVerbs As IEnumerable(Of String)
    Sub DoVerb(verbType As String)

    ReadOnly Property PlaceList As IEnumerable(Of (Text As String, Item As IPlaceModel))
    ReadOnly Property KnowsStarVicinities As Boolean
    ReadOnly Property StarVicinityList As IEnumerable(Of (Text As String, Item As IPlaceModel))
    ReadOnly Property PlanetVicinityList As IEnumerable(Of (Text As String, Item As IPlaceModel))
    ReadOnly Property KnowsPlaces As Boolean
End Interface
