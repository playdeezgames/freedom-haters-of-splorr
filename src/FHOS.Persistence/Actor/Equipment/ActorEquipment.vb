Friend Class ActorEquipment
    Inherits ActorDataClient
    Implements IActorEquipment

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property All As IEnumerable(Of IItem) Implements IActorEquipment.All
        Get
            Return EntityData.AllEquipment.Select(Function(x) Item.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property AllSlots As IEnumerable(Of String) Implements IActorEquipment.AllSlots
        Get
            Return EntityData.YokedItems.Keys
        End Get
    End Property

    Public ReadOnly Property Actor As IActor Implements IActorEquipment.Actor
        Get
            Return Persistence.Actor.FromId(UniverseData, Id)
        End Get
    End Property

    Public Sub Equip(equipSlot As String, item As IItem) Implements IActorEquipment.Equip
        EntityData.SetYokedItem(equipSlot, item?.Id)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorEquipment
        Return New ActorEquipment(universeData, id)
    End Function

    Public Function GetSlot(equipSlot As String) As IItem Implements IActorEquipment.GetSlot
        Dim itemId = EntityData.GetYokedItem(equipSlot)
        If itemId.HasValue Then
            Return New Item(UniverseData, itemId.Value)
        End If
        Return Nothing
    End Function
End Class
