Imports FHOS.Model
Imports FHOS.Persistence
Imports SPLORR.UI

Friend Class StarSystemListState
    Inherits KnownPlaceListState

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Known Star Systems",
            GameState.KnownPlaces,
            PlaceTypes.StarSystem,
            GameState.StarSystemDetails)
    End Sub
End Class
