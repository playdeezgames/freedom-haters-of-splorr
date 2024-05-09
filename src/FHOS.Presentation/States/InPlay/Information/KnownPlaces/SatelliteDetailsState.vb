Imports FHOS.Model
Imports SPLORR.UI

Friend Class SatelliteDetailsState
    Inherits KnownPlaceDetailsState

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IUniverseModel))
        MyBase.New(parent, setState, context, GameState.SatelliteList)
    End Sub

    Protected Overrides ReadOnly Property HeaderText As String
        Get
            Return $"Satellite {Place.Name}"
        End Get
    End Property

    Protected Overrides ReadOnly Property Details As IEnumerable(Of (Text As String, Hue As Integer))
        Get
            Return {
                    ($"Type: {Place.PlanetType}", Hue.Black),
                    ($"Planet: {Place.Parent.Name}", Hue.Black),
                    ($"System: {Place.Parent.Parent.Name}", Hue.Black)
                }
        End Get
    End Property
End Class
