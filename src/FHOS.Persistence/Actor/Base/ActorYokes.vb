Imports FHOS.Data

Friend Class ActorYokes
    Inherits ActorDataClient
    Implements IActorYokes

    Protected Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public Property Actor(yokeType As String) As IActor Implements IActorYokes.Actor
        Get
            Dim id As Integer
            If EntityData.YokedActors.TryGetValue(yokeType, id) Then
                Return Persistence.Actor.FromId(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IActor)
            If value IsNot Nothing Then
                EntityData.YokedActors(yokeType) = value.Id
            Else
                EntityData.YokedActors.Remove(yokeType)
            End If
        End Set
    End Property

    Public Property Group(yokeType As String) As IGroup Implements IActorYokes.Group
        Get
            Dim id As Integer
            If EntityData.YokedGroups.TryGetValue(yokeType, id) Then
                Return Persistence.Group.FromId(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IGroup)
            If value IsNot Nothing Then
                EntityData.YokedGroups(yokeType) = value.Id
            Else
                EntityData.YokedGroups.Remove(yokeType)
            End If
        End Set
    End Property

    Public Property Store(yokeType As String) As IStore Implements IActorYokes.Store
        Get
            Dim id As Integer
            If EntityData.YokedStores.TryGetValue(yokeType, id) Then
                Return Persistence.Store.FromId(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IStore)
            If value IsNot Nothing Then
                EntityData.YokedStores(yokeType) = value.Id
            Else
                EntityData.YokedStores.Remove(yokeType)
            End If
        End Set
    End Property

    Friend Shared Function FromId(universeData As UniverseData, actorId As Integer?) As IActorYokes
        If actorId.HasValue Then
            Return New ActorYokes(universeData, actorId.Value)
        End If
        Return Nothing
    End Function

End Class
