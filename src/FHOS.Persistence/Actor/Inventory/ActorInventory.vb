Imports FHOS.Data

Friend Class ActorInventory
    Inherits ActorDataClient
    Implements IActorInventory
    Protected Sub New(universeData As IUniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property Items As IEnumerable(Of IItem) Implements IActorInventory.Items
        Get
            Return EntityData.AllItems.Select(Function(x) Item.FromId(UniverseData, x))
        End Get
    End Property

    Public Sub Add(item As IItem) Implements IActorInventory.Add
        EntityData.AddItem(item.Id)
    End Sub

    Public Sub Remove(item As IItem) Implements IActorInventory.Remove
        EntityData.RemoveItem(item.Id)
    End Sub

    Friend Shared Function FromId(universeData As IUniverseData, actorId As Integer?) As IActorInventory
        If actorId.HasValue Then
            Return New ActorInventory(universeData, actorId.Value)
        End If
        Return Nothing
    End Function
End Class
