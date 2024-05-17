Imports FHOS.Data

Friend Class Actor
    Inherits ActorDataClient
    Implements IActor
    Implements IActorState
    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, actorId As Integer?) As IActor
        If actorId.HasValue Then
            Return New Actor(universeData, actorId.Value)
        End If
        Return Nothing
    End Function

    Public Property Location As ILocation Implements IActorState.Location
        Get
            Return Persistence.Location.FromId(UniverseData, EntityData.Statistics(StatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.Statistics(StatisticTypes.LocationId) Then
                Location.Actor = Nothing
                EntityData.Statistics(StatisticTypes.LocationId) = value.Id
                value.Actor = Me
                Tutorial.Add(value.Tutorial)
            End If
        End Set
    End Property

    Public ReadOnly Property ActorType As String Implements IActor.ActorType
        Get
            Return EntityData.Metadatas(MetadataTypes.ActorType)
        End Get
    End Property

    Public Property Facing As Integer Implements IActorState.Facing
        Get
            Return EntityData.Statistics(StatisticTypes.Facing)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.Facing) = value
        End Set
    End Property

    Public Property Interactor As IActor Implements IActorState.Interactor
        Get
            Return Actor.FromId(UniverseData, GetStatistic(StatisticTypes.InteractorId))
        End Get
        Set(value As IActor)
            SetStatistic(StatisticTypes.InteractorId, value?.Id)
        End Set
    End Property

    Public Property LifeSupport As IStore Implements IActorState.LifeSupport
        Get
            Return Store.FromId(UniverseData, GetStatistic(StatisticTypes.LifeSupportId))
        End Get
        Set(value As IStore)
            SetStatistic(StatisticTypes.LifeSupportId, value?.Id)
        End Set
    End Property

    Public Property Wallet As IStore Implements IActorState.Wallet
        Get
            Return Store.FromId(UniverseData, GetStatistic(StatisticTypes.WalletId))
        End Get
        Set(value As IStore)
            SetStatistic(StatisticTypes.WalletId, value?.Id)
        End Set
    End Property

    Public Property FuelTank As IStore Implements IActorState.FuelTank
        Get
            Return Store.FromId(UniverseData, GetStatistic(StatisticTypes.FuelTankId))
        End Get
        Set(value As IStore)
            SetStatistic(StatisticTypes.FuelTankId, value?.Id)
        End Set
    End Property

    Public ReadOnly Property KnownPlaces As IActorKnownPlaces Implements IActor.KnownPlaces
        Get
            Return ActorKnownPlaces.FromId(UniverseData, Id)
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
            Throw New NotImplementedException()
        End Get
    End Property
End Class
