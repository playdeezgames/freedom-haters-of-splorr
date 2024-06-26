﻿Friend Class ActorEquipment
    Inherits ActorDataClient
    Implements IActorEquipment

    Protected Sub New(universeData As Data.IUniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property All As IEnumerable(Of IItem) Implements IActorEquipment.All
        Get
            Return EntityData.AllEquipment.Select(Function(x) Item.FromId(UniverseData, x))
        End Get
    End Property

    Public Sub Equip(item As IItem) Implements IActorEquipment.Equip
        EntityData.AddEquipment(item.Id)
    End Sub

    Friend Shared Function FromId(universeData As Data.IUniverseData, id As Integer) As IActorEquipment
        Return New ActorEquipment(universeData, id)
    End Function
End Class
