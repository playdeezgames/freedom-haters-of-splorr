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
            flags:={FlagTypes.IsStarVicinity},
            subtype:=starType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result = New List(Of (Text As String, Hue As Integer))
        result.Add(($"Star Type: {StarTypes.Descriptors(actor.Descriptor.Subtype).StarTypeName}", Hues.LightGray))
        Return result
    End Function
End Class
