Friend Class ActorState
    Inherits ActorDataClient
    Implements IActorState

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public Property Location As ILocation Implements IActorState.Location
        Get
            Return Persistence.Location.FromId(UniverseData, EntityData.Statistics(LegacyStatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.Statistics(LegacyStatisticTypes.LocationId) Then
                Location.Actor = Nothing
                EntityData.Statistics(LegacyStatisticTypes.LocationId) = value.Id
                value.Actor = Actor.FromId(UniverseData, Id)
                ActorTutorial.FromId(UniverseData, Id).Add(value.Tutorial)
            End If
        End Set
    End Property

    Public Property Interactor As IActor Implements IActorState.Interactor
        Get
            Return Actor.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.InteractorId))
        End Get
        Set(value As IActor)
            SetStatistic(LegacyStatisticTypes.InteractorId, value?.Id)
        End Set
    End Property

    Public Property LifeSupport As IStore Implements IActorState.LifeSupport
        Get
            Return Store.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.LifeSupportId))
        End Get
        Set(value As IStore)
            SetStatistic(LegacyStatisticTypes.LifeSupportId, value?.Id)
        End Set
    End Property

    Public Property Wallet As IStore Implements IActorState.Wallet
        Get
            Return Store.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.WalletId))
        End Get
        Set(value As IStore)
            SetStatistic(LegacyStatisticTypes.WalletId, value?.Id)
        End Set
    End Property

    Public Property FuelTank As IStore Implements IActorState.FuelTank
        Get
            Return Store.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.FuelTankId))
        End Get
        Set(value As IStore)
            SetStatistic(LegacyStatisticTypes.FuelTankId, value?.Id)
        End Set
    End Property

    Public Property Scrap As Integer Implements IActorState.Scrap
        Get
            Return If(GetStatistic(LegacyStatisticTypes.Scrap), 0)
        End Get
        Set(value As Integer)
            SetStatistic(LegacyStatisticTypes.Scrap, value)
        End Set
    End Property

    Public Property StarGate As IActor Implements IActorState.StarGate
        Get
            Return Actor.FromId(UniverseData, GetStatistic(LegacyStatisticTypes.StarGateId))
        End Get
        Set(value As IActor)
            SetStatistic(LegacyStatisticTypes.StarGateId, value?.Id)
        End Set
    End Property

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IActorState
        Return New ActorState(universeData, id)
    End Function
End Class
