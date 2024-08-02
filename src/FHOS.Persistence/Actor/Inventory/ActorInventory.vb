Imports FHOS.Data

Friend Class ActorInventory
    Inherits ActorDataClient
    Implements IActorInventory
    Protected Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property Items As IEnumerable(Of IItem) Implements IActorInventory.Items
        Get
            Return EntityData.AllItems.Select(Function(x) Item.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property ItemCountOfType(itemType As String) As Integer Implements IActorInventory.ItemCountOfType
        Get
            Return Items.Count(Function(x) x.EntityType = itemType)
        End Get
    End Property

    Public ReadOnly Property ItemsOfType(itemType As String) As IEnumerable(Of IItem) Implements IActorInventory.ItemsOfType
        Get
            Return Items.Where(Function(x) x.EntityType = itemType)
        End Get
    End Property

    Public ReadOnly Property AnyOfType(itemType As String) As Boolean Implements IActorInventory.AnyOfType
        Get
            Return Items.Any(Function(x) x.EntityType = itemType)
        End Get
    End Property

    Public Sub Add(item As IItem) Implements IActorInventory.Add
        EntityData.AddInventoryItem(item.Id)
    End Sub

    Public Sub Remove(item As IItem) Implements IActorInventory.Remove
        EntityData.RemoveInventoryItem(item.Id)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, actorId As Integer?) As IActorInventory
        If actorId.HasValue Then
            Return New ActorInventory(universeData, actorId.Value)
        End If
        Return Nothing
    End Function
End Class
