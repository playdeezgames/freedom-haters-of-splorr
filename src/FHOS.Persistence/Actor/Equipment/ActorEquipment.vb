Imports Microsoft.Data.Sqlite

Friend Class ActorEquipment
    Inherits ActorDataClient
    Implements IActorEquipment

    Protected Sub New(universeData As Data.IUniverseData, connection As SqliteConnection, actorId As Integer)
        MyBase.New(universeData, connection, actorId)
    End Sub

    Public ReadOnly Property All As IEnumerable(Of IItem) Implements IActorEquipment.All
        Get
            Return EntityData.AllEquipment.Select(Function(x) Item.FromId(UniverseData, Connection, x))
        End Get
    End Property

    Public Sub Equip(item As IItem) Implements IActorEquipment.Equip
        EntityData.AddEquipment(item.Id)
    End Sub

    Friend Shared Function FromId(universeData As Data.IUniverseData, connection As SqliteConnection, id As Integer) As IActorEquipment
        Return New ActorEquipment(universeData, connection, id)
    End Function
End Class
