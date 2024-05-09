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

    Public Property Location As ILocation Implements IActor.Location
        Get
            Return New Location(UniverseData, EntityData.Statistics(StatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.Statistics(StatisticTypes.LocationId) Then
                Location.Actor = Nothing
                EntityData.Statistics(StatisticTypes.LocationId) = value.Id
                value.Actor = Me
                TriggerTutorial(value.Tutorial)
            End If
        End Set
    End Property

    Public Sub TriggerTutorial(tutorial As String) Implements IActor.TriggerTutorial
        If tutorial Is Nothing Then
            Return
        End If
        EntityData.Tutorials.Enqueue(tutorial)
    End Sub

    Public Sub DismissTutorial() Implements IActor.DismissTutorial
        If HasTutorial Then
            EntityData.Tutorials.Dequeue()
        End If
    End Sub

    Public Sub AddKnownPlace(place As IPlace) Implements IActor.AddKnownPlace
        If Not EntityData.Places.Discovered.ContainsKey(place.Id) Then
            EntityData.Places.Discovered(place.Id) = Turn
        End If
        EntityData.Places.Visited(place.Id) = Turn
    End Sub

    Public Sub AddMessage(header As String, ParamArray lines() As (Text As String, Hue As Integer)) Implements IActor.AddMessage
        UniverseData.Messages.Enqueue(New MessageData With
                                  {
                                    .Header = header,
                                    .Lines = lines.Select(Function(x) New MessageLineData With {.Text = x.Text, .Hue = x.Hue}).ToList
                                  })
    End Sub

    Public Function KnowsPlacesOfType(placeType As String) As Boolean Implements IActor.KnowsPlacesOfType
        Return KnownPlaces.Any(Function(x) x.PlaceType = placeType)
    End Function

    Public Function GetKnownPlacesOfType(placeType As String) As IEnumerable(Of IPlace) Implements IActor.GetKnownPlacesOfType
        Return KnownPlaces.Where(Function(x) x.PlaceType = placeType)
    End Function

    Public ReadOnly Property ActorType As String Implements IActor.ActorType
        Get
            Return EntityData.Metadatas(MetadataTypes.ActorType)
        End Get
    End Property

    Public Property MaximumOxygen As Integer Implements IActor.MaximumOxygen
        Get
            Return Math.Max(0, EntityData.Statistics(StatisticTypes.MaximumOxygen))
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.MaximumOxygen) = Math.Max(0, value)
        End Set
    End Property

    Public Property Oxygen As Integer Implements IActor.Oxygen
        Get
            Return Math.Clamp(EntityData.Statistics(StatisticTypes.Oxygen), 0, MaximumOxygen)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.Oxygen) = Math.Clamp(value, 0, MaximumOxygen)
        End Set
    End Property

    Public Property Facing As Integer Implements IActor.Facing
        Get
            Return EntityData.Statistics(StatisticTypes.Facing)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.Facing) = value
        End Set
    End Property

    Public ReadOnly Property HasTutorial As Boolean Implements IActor.HasTutorial
        Get
            Return EntityData.Tutorials.Any
        End Get
    End Property

    Public ReadOnly Property CurrentTutorial As String Implements IActor.CurrentTutorial
        Get
            If HasTutorial Then
                Return EntityData.Tutorials.Peek
            End If
            Return Nothing
        End Get
    End Property

    Public Property MaximumFuel As Integer Implements IActor.MaximumFuel
        Get
            Return Math.Max(0, EntityData.Statistics(StatisticTypes.MaximumFuel))
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.MaximumFuel) = Math.Max(0, value)
        End Set
    End Property

    Public Property Fuel As Integer Implements IActor.Fuel
        Get
            Return Math.Clamp(EntityData.Statistics(StatisticTypes.Fuel), 0, MaximumFuel)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.Fuel) = Math.Clamp(value, 0, MaximumFuel)
        End Set
    End Property

    Public ReadOnly Property KnowsPlaces As Boolean Implements IActor.KnowsPlaces
        Get
            Return EntityData.Places.Discovered.Any
        End Get
    End Property

    Public ReadOnly Property KnownPlaces As IEnumerable(Of IPlace) Implements IActor.KnownPlaces
        Get
            Return EntityData.Places.Discovered.Select(Function(x) New Place(UniverseData, x.Key)).OrderBy(Function(x) x.Name)
        End Get
    End Property

    Public Property Turn As Integer Implements IActor.Turn
        Get
            Return EntityData.Statistics(StatisticTypes.Turn)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.Turn) = value
        End Set
    End Property

    Public Property Jools As Integer Implements IActor.Jools
        Get
            Return EntityData.Statistics(StatisticTypes.Jools)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.Jools) = value
        End Set
    End Property

    Public ReadOnly Property HasFuel As Boolean Implements IActor.HasFuel
        Get
            Return Fuel > 0
        End Get
    End Property

    Public Property MinimumJools As Integer Implements IActor.MinimumJools
        Get
            Return EntityData.Statistics(StatisticTypes.MinimumJools)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.MinimumJools) = value
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

    Public ReadOnly Property Universe As IUniverse Implements IActor.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Public Property HomePlanet As IPlace Implements IActor.HomePlanet
        Get
            Dim id = 0
            If EntityData.Statistics.TryGetValue(StatisticTypes.HomePlanetId, id) Then
                Return New Place(UniverseData, id)
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
End Class
