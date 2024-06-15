Imports FHOS.Persistence

Friend Class PersonActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.Person,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.Person, Hues.LightGray), 1}
            })
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class
