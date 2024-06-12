Imports FHOS.Persistence

Friend Class StarActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(starType As String)
        MyBase.New(ActorTypes.MakeStar(starType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeStar(starType), 1}
            },
            New Dictionary(Of String, String),
            isStar:=True,
            subtype:=starType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result = New List(Of (Text As String, Hue As Integer))
        result.Add(($"Star Type: {StarTypes.Descriptors(actor.Descriptor.Subtype).StarTypeName}", Hues.Black))
        Return result
    End Function
End Class
