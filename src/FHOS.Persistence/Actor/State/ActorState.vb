Friend Class ActorState
    Inherits ActorDataClient
    Implements IActorState

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public Property Location As ILocation Implements IActorState.Location
        Get
            Return Persistence.Location.FromId(UniverseData, EntityData.Statistics(StatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.Statistics(StatisticTypes.LocationId) Then
                Location.Actor = Nothing
                EntityData.Statistics(StatisticTypes.LocationId) = value.Id
                value.Actor = Actor.FromId(UniverseData, Id)
                ActorTutorial.FromId(UniverseData, Id).Add(value.Tutorial)
            End If
        End Set
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

    Public Property Scrap As Integer Implements IActorState.Scrap
        Get
            Return If(GetStatistic(StatisticTypes.Scrap), 0)
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.Scrap, value)
        End Set
    End Property

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorState
        Return New ActorState(universeData, id)
    End Function
End Class
