Imports FHOS.Model
Imports SPLORR.UI

Friend Class KnownStarSystemDetailsState
    Inherits KnownPlaceDetailsState

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IUniverseModel))
        MyBase.New(parent, setState, context, GameState.KnownStarSystemList)
    End Sub

    Protected Overrides ReadOnly Property HeaderText As String
        Get
            Return $"{Place.Name} System"
        End Get
    End Property

    Protected Overrides ReadOnly Property Details As IEnumerable(Of (Text As String, Hue As Integer))
        Get
            Return {
                    ($"Type: {Place.Subtype}", Hues.Black),
                    ($"Planets: {Place.PlanetVicinityCount}", Hues.Black),
                    ($"Galaxy Position: ({Place.X},{Place.Y})", Hues.Black)
                }
        End Get
    End Property
End Class
