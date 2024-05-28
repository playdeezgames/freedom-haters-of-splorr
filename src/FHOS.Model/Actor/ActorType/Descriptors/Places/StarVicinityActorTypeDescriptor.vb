Imports FHOS.Persistence

Friend Class StarVicinityActorTypeDescriptor
    Inherits ActorTypeDescriptor


    Public Sub New(starType As String)
        MyBase.New(
            ActorTypes.MakeStarVicinity(starType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeStarVicinity(starType), 1}
            },
            New Dictionary(Of String, String),
            isStarVicinity:=True,
            subtype:=starType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {("It's a star! It's real hot!", Hues.Black)}
    End Function
End Class
