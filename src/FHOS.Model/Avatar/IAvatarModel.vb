Public Interface IAvatarModel
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    ReadOnly Property MapName As String
    ReadOnly Property HasVerbs As Boolean

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
    ReadOnly Property AvailableVerbs As IEnumerable(Of (Text As String, VerbType As String))
    Sub DoVerb(verbType As String)
    ReadOnly Property KnowsPlaces As Boolean
    Function KnowsPlacesOfType(placeType As String) As Boolean
    Function GetKnownPlacesOfType(placeType As String) As IEnumerable(Of (Text As String, Item As IPlaceModel))
    Sub LeaveInteraction()
    Sub Push(actor As IActorModel)
    Sub Pop()
    ReadOnly Property HomePlanet As IPlaceModel
    ReadOnly Property Faction As IFactionModel
    ReadOnly Property IsInteracting As Boolean
    ReadOnly Property AvailableCrew As IEnumerable(Of (Name As String, Actor As IActorModel))
End Interface
