Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarVicinityDetailsState
    Inherits KnownPlaceDetailsState

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IUniverseModel))
        MyBase.New(parent, setState, context, GameState.StarSystemList)
    End Sub

    Protected Overrides ReadOnly Property HeaderText As String
        Get
            Return $"{Place.Name} Vicinity"
        End Get
    End Property

    Protected Overrides ReadOnly Property Details As IEnumerable(Of (Text As String, Hue As Integer))
        Get
            Return {
            ($"Type: {Place.StarType}", Hue.Black),
            ($"System: {Place.Parent.Name}", Hue.Black),
            ($"System Position: ({Place.X},{Place.Y})", Hue.Black)
                }
        End Get
    End Property
End Class
