Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class ActorYokes
    Inherits ActorDataClient
    Implements IActorYokes

    Protected Sub New(
                     universeData As IUniverseData,
                     connection As SqliteConnection,
                     actorId As Integer)
        MyBase.New(
            universeData,
            connection,
            actorId)
    End Sub

    Public Property Actor(yokeType As String) As IActor Implements IActorYokes.Actor
        Get
            Return Persistence.Actor.FromId(UniverseData, Connection, EntityData.GetYokedActor(yokeType))
        End Get
        Set(value As IActor)
            EntityData.SetYokedActor(yokeType, value?.Id)
        End Set
    End Property

    Public Property Group(yokeType As String) As IGroup Implements IActorYokes.Group
        Get
            Return Persistence.Group.FromId(UniverseData, Connection, EntityData.GetYokedGroup(yokeType))
        End Get
        Set(value As IGroup)
            EntityData.SetYokedGroup(yokeType, value?.Id)
        End Set
    End Property

    Public Property Store(yokeType As String) As IStore Implements IActorYokes.Store
        Get
            Return Persistence.Store.FromId(UniverseData, Connection, EntityData.GetYokedStore(yokeType))
        End Get
        Set(value As IStore)
            EntityData.SetYokedStore(yokeType, value?.Id)
        End Set
    End Property

    Friend Shared Function FromId(universeData As IUniverseData, connection As SqliteConnection, actorId As Integer?) As IActorYokes
        If actorId.HasValue Then
            Return New ActorYokes(universeData, connection, actorId.Value)
        End If
        Return Nothing
    End Function

End Class
