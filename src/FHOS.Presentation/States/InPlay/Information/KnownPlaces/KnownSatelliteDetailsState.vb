Imports FHOS.Model
Imports SPLORR.UI

Friend Class KnownSatelliteDetailsState
    Inherits KnownPlaceDetailsState

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IUniverseModel))
        MyBase.New(parent, setState, context, GameState.KnownSatelliteList)
    End Sub

    Protected Overrides ReadOnly Property HeaderText As String
        Get
            Return $"Satellite {Place.Name}"
        End Get
    End Property

    Protected Overrides ReadOnly Property Details As IEnumerable(Of (Text As String, Hue As Integer))
        Get
            Return {
                    ($"Type: {Place.PlanetType}", Hues.Black),
                    ($"Planet: {Place.Parent.Name}", Hues.Black),
                    ($"System: {Place.Parent.Parent.Name}", Hues.Black)
                }
        End Get
    End Property
End Class
