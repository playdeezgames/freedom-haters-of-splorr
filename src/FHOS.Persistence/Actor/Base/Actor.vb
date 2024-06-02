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

    Public ReadOnly Property ActorType As String Implements IActor.ActorType
        Get
            Return EntityData.Metadatas(LegacyMetadataTypes.ActorType)
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

    Public ReadOnly Property GroupCategory(group As IGroup) As String Implements IActor.GroupCategory
        Get
            Return Nothing
        End Get
    End Property
End Class
