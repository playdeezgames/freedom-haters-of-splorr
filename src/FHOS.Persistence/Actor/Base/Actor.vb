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

    Public Sub AddGroup(group As IGroup) Implements IActor.AddGroup
        If group IsNot Nothing Then
            EntityData.Groups.Add(group.Id)
        End If
    End Sub

    Public Sub RemoveGroup(group As IGroup) Implements IActor.RemoveGroup
        If group IsNot Nothing Then
            EntityData.Groups.Remove(group.Id)
        End If
    End Sub

    Public ReadOnly Property ActorType As String Implements IActor.ActorType
        Get
            Return EntityData.Metadatas(MetadataTypes.ActorType)
        End Get
    End Property

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

    Public ReadOnly Property Groups As IEnumerable(Of IGroup) Implements IActor.Groups
        Get
            Return EntityData.Groups.Select(Function(x) Group.FromId(UniverseData, x))
        End Get
    End Property
End Class
