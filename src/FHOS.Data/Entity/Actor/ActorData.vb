Public Class ActorData
    Inherits GroupedEntityData
    Public Property Children As New HashSet(Of Integer)
    Public Property Equipment As New HashSet(Of Integer)
    Public Property YokedItems As New Dictionary(Of String, Integer)
    Public Property YokedActors As New Dictionary(Of String, Integer)
    Public Property YokedStores As New Dictionary(Of String, Integer)
    Public Property Inventory As New HashSet(Of Integer)
    Public Property Offers As New HashSet(Of String)
    Public Property Prices As New HashSet(Of String)

    Public ReadOnly Property HasChildren As Boolean
        Get
            Return Children.Any
        End Get
    End Property

    Public ReadOnly Property AllChildren As IEnumerable(Of Integer)
        Get
            Return Children
        End Get
    End Property

    Public ReadOnly Property AllEquipment As IEnumerable(Of Integer)
        Get
            Return YokedItems.Values.Distinct
        End Get
    End Property

    Public ReadOnly Property AllItems As IEnumerable(Of Integer)
        Get
            Return Inventory
        End Get
    End Property

    Public ReadOnly Property HasOffers As Boolean
        Get
            Return Offers.Any
        End Get
    End Property

    Public ReadOnly Property AllOffers As IEnumerable(Of String)
        Get
            Return Offers
        End Get
    End Property

    Public ReadOnly Property HasPrices As Boolean
        Get
            Return Prices.Any
        End Get
    End Property

    Public ReadOnly Property AllPrices As IEnumerable(Of String)
        Get
            Return Prices
        End Get
    End Property

    Public Sub AddChild(childId As Integer)
        Children.Add(childId)
    End Sub

    Public Sub AddEquipment(itemId As Integer)
        Equipment.Add(itemId)
    End Sub

    Public Sub AddItem(itemId As Integer)
        Inventory.Add(itemId)
    End Sub

    Public Sub RemoveItem(itemId As Integer)
        Inventory.Remove(itemId)
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

    Public Sub AddOffer(itemType As String)
        Offers.Add(itemType)
    End Sub

    Public Sub AddPrice(itemType As String)
        Prices.Add(itemType)
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
