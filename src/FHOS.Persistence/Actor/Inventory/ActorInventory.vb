Imports FHOS.Data

Friend Class ActorInventory
    Inherits ActorDataClient
    Implements IActorInventory
    Protected Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property Items As IEnumerable(Of IItem) Implements IActorInventory.Items
        Get
            Return EntityData.Inventory.Select(Function(x) Item.FromId(UniverseData, x))
        End Get
    End Property

    Public Sub Add(item As IItem) Implements IActorInventory.Add
        EntityData.Inventory.Add(item.Id)
    End Sub

    Public Sub Remove(item As IItem) Implements IActorInventory.Remove
        EntityData.Inventory.Remove(item.Id)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, actorId As Integer?) As IActorInventory
        If actorId.HasValue Then
            Return New ActorInventory(universeData, actorId.Value)
        End If
        Return Nothing
    End Function
End Class
