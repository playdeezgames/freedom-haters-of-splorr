Imports FHOS.Data

Friend Class ActorYokes
    Inherits ActorDataClient
    Implements IActorYokes

    Protected Sub New(universeData As UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public Property YokedActor(yokeType As String) As IActor Implements IActorYokes.YokedActor
        Get
            Dim id As Integer
            If EntityData.YokedActors.TryGetValue(yokeType, id) Then
                Return Actor.FromId(UniverseData, id)
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

    Public Property YokedGroup(yokeType As String) As IGroup Implements IActorYokes.YokedGroup
        Get
            Dim id As Integer
            If EntityData.YokedGroups.TryGetValue(yokeType, id) Then
                Return Group.FromId(UniverseData, id)
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

    Public Property YokedStore(yokeType As String) As IStore Implements IActorYokes.YokedStore
        Get
            Dim id As Integer
            If EntityData.YokedStores.TryGetValue(yokeType, id) Then
                Return Store.FromId(UniverseData, id)
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
