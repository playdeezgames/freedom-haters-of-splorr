Imports FHOS.Persistence

Friend Class ArrowActorDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(directionName As String)
        MyBase.New(
            ActorTypes.MakeArrow(directionName),
            New Dictionary(Of String, Integer) From {{CostumeTypes.MakeArrow(directionName), 1}},
            New Dictionary(Of String, String))
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim map = actor.Location.Map
        Dim mapTypeDescriptor = MapTypes.Descriptors(map.EntityType)
        Return {
            ($"You can depart the {mapTypeDescriptor.MapTypeName}.", Hues.LightGray)
            }
    End Function
End Class
