Friend Class ActorEquipment
    Inherits ActorDataClient
    Implements IActorEquipment

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property All As IEnumerable(Of IItem) Implements IActorEquipment.All
        Get
            Return EntityData.Equipment.Select(Function(x) Item.FromId(UniverseData, x))
        End Get
    End Property

    Public Sub Equip(item As IItem) Implements IActorEquipment.Equip
        EntityData.Equipment.Add(item.Id)
    End Sub

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorEquipment
        Return New ActorEquipment(universeData, id)
    End Function
End Class
