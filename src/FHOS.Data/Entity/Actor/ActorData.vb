Public Class ActorData
    Inherits GroupedEntityData
    Implements IActorData
    Public Sub New(
                  id As Integer,
                  Optional flags As HashSet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(id, statistics:=statistics, flags:=flags, metadatas:=metadatas)
    End Sub
    Public Property Children As New HashSet(Of Integer)
    Public Property Equipment As New HashSet(Of Integer)
    Public Property YokedActors As New Dictionary(Of String, Integer)
    Public Property YokedStores As New Dictionary(Of String, Integer)
    Public Property Inventory As New HashSet(Of Integer)

    Public ReadOnly Property HasChildren As Boolean Implements IActorData.HasChildren
        Get
            Return Children.Any
        End Get
    End Property

    Public ReadOnly Property AllChildren As IEnumerable(Of Integer) Implements IActorData.AllChildren
        Get
            Return Children
        End Get
    End Property

    Public ReadOnly Property AllEquipment As IEnumerable(Of Integer) Implements IActorData.AllEquipment
        Get
            Return Equipment
        End Get
    End Property

    Public ReadOnly Property AllItems As IEnumerable(Of Integer) Implements IActorData.AllItems
        Get
            Return Inventory
        End Get
    End Property

    Public Sub AddChild(childId As Integer) Implements IActorData.AddChild
        Children.Add(childId)
    End Sub

    Public Sub AddEquipment(itemId As Integer) Implements IActorData.AddEquipment
        Equipment.Add(itemId)
    End Sub

    Public Sub AddItem(itemId As Integer) Implements IActorData.AddItem
        Inventory.Add(itemId)
    End Sub

    Public Sub RemoveItem(itemId As Integer) Implements IActorData.RemoveItem
        Inventory.Remove(itemId)
    End Sub

    Public Sub SetYokedActor(yokeType As String, actorId As Integer?) Implements IActorData.SetYokedActor
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        If actorId.HasValue Then
            YokedActors(yokeType) = actorId.Value
        Else
            YokedActors.Remove(yokeType)
        End If
    End Sub

    Public Sub SetYokedStore(yokeType As String, storeId As Integer?) Implements IActorData.SetYokedStore
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        If storeId.HasValue Then
            YokedStores(yokeType) = storeId.Value
        Else
            YokedStores.Remove(yokeType)
        End If
    End Sub

    Public Function GetYokedActor(yokeType As String) As Integer? Implements IActorData.GetYokedActor
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        Dim actorId As Integer
        If YokedActors.TryGetValue(yokeType, actorId) Then
            Return actorId
        End If
        Return Nothing
    End Function

    Public Function GetYokedStore(yokeType As String) As Integer? Implements IActorData.GetYokedStore
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        Dim storeId As Integer
        If YokedStores.TryGetValue(yokeType, storeId) Then
            Return storeId
        End If
        Return Nothing
    End Function
End Class
