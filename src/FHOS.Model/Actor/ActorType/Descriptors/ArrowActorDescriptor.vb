Imports FHOS.Persistence

Friend Class ArrowActorDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(directionName As String)
        MyBase.New(
            $"Arrow{directionName}",
            New Dictionary(Of String, Integer) From {{CostumeTypes.MakeArrow(directionName), 1}},
            New Dictionary(Of String, String))
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        Throw New NotImplementedException()
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Throw New NotImplementedException()
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Throw New NotImplementedException()
    End Function
End Class
