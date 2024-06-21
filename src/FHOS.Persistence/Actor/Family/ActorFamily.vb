Friend Class ActorFamily
    Inherits ActorDataClient
    Implements IActorFamily

    Protected Sub New(universeData As Data.IUniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As Data.IUniverseData, id As Integer) As IActorFamily
        Return New ActorFamily(universeData, id)
    End Function

    Public Sub AddChild(child As IActor) Implements IActorFamily.AddChild
        EntityData.AddChild(child.Id)
        child.Family.Parent = Actor.FromId(UniverseData, Id)
    End Sub

    Public Property Parent As IActor Implements IActorFamily.Parent
        Get
            Return Actor.FromId(UniverseData, GetStatistic(PersistenceStatisticTypes.ParentId))
        End Get
        Set(value As IActor)
            SetStatistic(PersistenceStatisticTypes.ParentId, value?.Id)
        End Set
    End Property

    Public ReadOnly Property HasChildren As Boolean Implements IActorFamily.HasChildren
        Get
            Return EntityData.HasChildren
        End Get
    End Property

    Public ReadOnly Property Children As IEnumerable(Of IActor) Implements IActorFamily.Children
        Get
            Return EntityData.AllChildren.Select(Function(x) Actor.FromId(UniverseData, x))
        End Get
    End Property
End Class
