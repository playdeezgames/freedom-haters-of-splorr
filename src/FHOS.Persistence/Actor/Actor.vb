Imports FHOS.Data

Friend Class Actor
    Inherits ActorDataClient
    Implements IActor
    Implements IActorFamily

    Protected Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, actorId As Integer?) As IActor
        If actorId.HasValue Then
            Return New Actor(universeData, actorId.Value)
        End If
        Return Nothing
    End Function

    Public Property Location As ILocation Implements IActor.Location
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


    Public Sub AddChild(crew As IActor) Implements IActorFamily.AddChild
        EntityData.Children.Add(crew.Id)
        crew.Family.Parent = Me
    End Sub

    Public ReadOnly Property ActorType As String Implements IActor.ActorType
        Get
            Return EntityData.Metadatas(MetadataTypes.ActorType)
        End Get
    End Property

    Public Property Facing As Integer Implements IActor.Facing
        Get
            Return EntityData.Statistics(StatisticTypes.Facing)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.Facing) = value
        End Set
    End Property



    Public Property Faction As IFaction Implements IActor.Faction
        Get
            Dim id = GetStatistic(StatisticTypes.FactionId)
            If id.HasValue Then
                Return Persistence.Faction.FromId(UniverseData, id.Value)
            End If
            Return Nothing
        End Get
        Set(value As IFaction)
            If value IsNot Nothing Then
                EntityData.Statistics(StatisticTypes.FactionId) = value.Id
            Else
                EntityData.Statistics.Remove(StatisticTypes.FactionId)
            End If
        End Set
    End Property

    Public Property HomePlanet As IPlace Implements IActor.HomePlanet
        Get
            Dim id = 0
            If EntityData.Statistics.TryGetValue(StatisticTypes.HomePlanetId, id) Then
                Return Place.FromId(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IPlace)
            If value IsNot Nothing Then
                EntityData.Statistics(StatisticTypes.HomePlanetId) = value.Id
            Else
                EntityData.Statistics.Remove(StatisticTypes.HomePlanetId)
            End If
        End Set
    End Property

    Public Property Interactor As IActor Implements IActor.Interactor
        Get
            Return Actor.FromId(UniverseData, GetStatistic(StatisticTypes.InteractorId))
        End Get
        Set(value As IActor)
            SetStatistic(StatisticTypes.InteractorId, value?.Id)
        End Set
    End Property

    Public Property Name As String Implements IActor.Name
        Get
            Return GetMetadata(MetadataTypes.Name)
        End Get
        Set(value As String)
            SetMetadata(MetadataTypes.Name, value)
        End Set
    End Property

    Public Property Interior As IMap Implements IActor.Interior
        Get
            Return Map.FromId(UniverseData, GetStatistic(StatisticTypes.MapId))
        End Get
        Set(value As IMap)
            SetStatistic(StatisticTypes.MapId, value?.Id)
        End Set
    End Property

    Public Property Parent As IActor Implements IActorFamily.Parent
        Get
            Return Actor.FromId(UniverseData, GetStatistic(StatisticTypes.ParentId))
        End Get
        Set(value As IActor)
            SetStatistic(StatisticTypes.ParentId, value?.Id)
        End Set
    End Property

    Public ReadOnly Property HasChildren As Boolean Implements IActorFamily.HasChildren
        Get
            Return EntityData.Children.Any
        End Get
    End Property

    Public ReadOnly Property Children As IEnumerable(Of IActor) Implements IActorFamily.Children
        Get
            Return EntityData.Children.Select(Function(x) Actor.FromId(UniverseData, x))
        End Get
    End Property

    Public Property LifeSupport As IStore Implements IActor.LifeSupport
        Get
            Return Store.FromId(UniverseData, GetStatistic(StatisticTypes.LifeSupportId))
        End Get
        Set(value As IStore)
            SetStatistic(StatisticTypes.LifeSupportId, value?.Id)
        End Set
    End Property

    Public Property Wallet As IStore Implements IActor.Wallet
        Get
            Return Store.FromId(UniverseData, GetStatistic(StatisticTypes.WalletId))
        End Get
        Set(value As IStore)
            SetStatistic(StatisticTypes.WalletId, value?.Id)
        End Set
    End Property

    Public Property Costume As String Implements IActor.Costume
        Get
            Return GetMetadata(MetadataTypes.Costume)
        End Get
        Set(value As String)
            SetMetadata(MetadataTypes.Costume, value)
        End Set
    End Property

    Public Property FuelTank As IStore Implements IActor.FuelTank
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
            Return Me
        End Get
    End Property
End Class
