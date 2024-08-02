Public Class ActorData
    Inherits GroupedEntityData
    Public Property YokedItems As New Dictionary(Of String, Integer)
    Public Property YokedActors As New Dictionary(Of String, Integer)
    Public Property YokedStores As New Dictionary(Of String, Integer)
    Public Property InventoryItems As New HashSet(Of Integer)
    Public Property OfferedItems As New HashSet(Of String)
    Public Property PricedItems As New HashSet(Of String)
    Public Property Reputations As New Dictionary(Of Integer, Integer)

    Public ReadOnly Property AllEquipment As IEnumerable(Of Integer)
        Get
            Return YokedItems.Values.Distinct
        End Get
    End Property

    Public ReadOnly Property AllItems As IEnumerable(Of Integer)
        Get
            Return InventoryItems
        End Get
    End Property

    Public ReadOnly Property HasOfferedItems As Boolean
        Get
            Return OfferedItems.Any
        End Get
    End Property

    Public ReadOnly Property AllOfferedItems As IEnumerable(Of String)
        Get
            Return OfferedItems
        End Get
    End Property

    Public ReadOnly Property HasPricedItems As Boolean
        Get
            Return PricedItems.Any
        End Get
    End Property

    Public ReadOnly Property AllPricedItems As IEnumerable(Of String)
        Get
            Return PricedItems
        End Get
    End Property

    Public Sub AddInventoryItem(itemId As Integer)
        InventoryItems.Add(itemId)
    End Sub

    Public Sub RemoveInventoryItem(itemId As Integer)
        InventoryItems.Remove(itemId)
    End Sub

    Public Sub SetYokedActor(yokeType As String, actorId As Integer?)
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        If actorId.HasValue Then
            YokedActors(yokeType) = actorId.Value
        Else
            YokedActors.Remove(yokeType)
        End If
    End Sub

    Public Sub SetYokedItem(yokeType As String, itemId As Integer?)
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        If itemId.HasValue Then
            YokedItems(yokeType) = itemId.Value
        Else
            YokedItems.Remove(yokeType)
        End If
    End Sub

    Public Sub SetYokedStore(yokeType As String, storeId As Integer?)
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        If storeId.HasValue Then
            YokedStores(yokeType) = storeId.Value
        Else
            YokedStores.Remove(yokeType)
        End If
    End Sub

    Public Sub AddOfferedItem(itemType As String)
        OfferedItems.Add(itemType)
    End Sub

    Public Sub AddPricedItem(itemType As String)
        PricedItems.Add(itemType)
    End Sub

    Public Function GetYokedActor(yokeType As String) As Integer?
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        Dim actorId As Integer
        If YokedActors.TryGetValue(yokeType, actorId) Then
            Return actorId
        End If
        Return Nothing
    End Function

    Public Function GetYokedItem(yokeType As String) As Integer?
        If String.IsNullOrWhiteSpace(yokeType) Then
            Throw New InvalidOperationException("yokeType is null or whitespace")
        End If
        Dim itemId As Integer
        If YokedItems.TryGetValue(yokeType, itemId) Then
            Return itemId
        End If
        Return Nothing
    End Function

    Public Function GetYokedStore(yokeType As String) As Integer?
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
