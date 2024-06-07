Imports FHOS.Data

Friend Class Actor
    Inherits ActorDataClient
    Implements IActor
    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, actorId As Integer?) As IActor
        If actorId.HasValue Then
            Return New Actor(universeData, actorId.Value)
        End If
        Return Nothing
    End Function

    Public ReadOnly Property Tutorial As IActorTutorial Implements IActor.Tutorial
        Get
            Return ActorTutorial.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Family As IActorFamily Implements IActor.Family
        Get
            Return ActorFamily.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Properties As IActorProperties Implements IActor.Properties
        Get
            Return ActorProperties.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property State As IActorState Implements IActor.State
        Get
            Return ActorState.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Equipment As IActorEquipment Implements IActor.Equipment
        Get
            Return ActorEquipment.FromId(UniverseData, Id)
        End Get
    End Property

    Public Property GroupCategory(group As IGroup) As String Implements IActor.GroupCategory
        Get
            Dim category As String = Nothing
            If EntityData.GroupCategories.TryGetValue(group.Id, category) Then
                Return category
            End If
            Return Nothing
        End Get
        Set(value As String)
            EntityData.GroupCategories(group.Id) = value
        End Set
    End Property

    Public ReadOnly Property GroupsOfCategory(categoryType As String) As IEnumerable(Of IGroup) Implements IActor.GroupsOfCategory
        Get
            Return EntityData.GroupCategories.Where(Function(x) x.Value = categoryType).Select(Function(x) Group.FromId(UniverseData, x.Key))
        End Get
    End Property

    Public Property YokedActor(yokeType As String) As IActor Implements IActor.YokedActor
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
End Class
