Imports FHOS.Persistence

Friend Class StarSystemActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(starType As String)
        MyBase.New(ActorTypes.MakeStarSystem(starType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeStarSystem(starType), 1}
            },
            New Dictionary(Of String, String),
            isStarSystem:=True,
            subtype:=starType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result As New List(Of (Text As String, Hue As Integer))
        Dim descriptor = StarTypes.Descriptors(actor.Descriptor.Subtype)
        result.Add(($"Type: {descriptor.StarTypeName}", Hues.Black))
        Dim location = actor.Location
        result.Add(($"Galactic Position: ({location.Position.Column}, {location.Position.Row})", Hues.Black))
        Return result
    End Function
End Class
