Imports FHOS.Data

Friend Class Actor
    Inherits ActorDataClient
    Implements IActor

    Public Sub New(universeData As Data.UniverseData, actorId As Integer)
        MyBase.New(universeData, actorId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IActor.Id
        Get
            Return ActorId
        End Get
    End Property

    Public Property Location As ILocation Implements IActor.Location
        Get
            Return New Location(UniverseData, ActorData.Statistics(StatisticTypes.LocationId))
        End Get
        Set(value As ILocation)
            If value.Id <> ActorData.Statistics(StatisticTypes.LocationId) Then
                Location.Actor = Nothing
                ActorData.Statistics(StatisticTypes.LocationId) = value.Id
                value.Actor = Me
                TriggerTutorial(value.Tutorial)
            End If
        End Set
    End Property

    Public Sub TriggerTutorial(tutorial As String) Implements IActor.TriggerTutorial
        If tutorial Is Nothing Then
            Return
        End If
        ActorData.Tutorials.Enqueue(tutorial)
    End Sub

    Public Sub DismissTutorial() Implements IActor.DismissTutorial
        If HasTutorial Then
            ActorData.Tutorials.Dequeue()
        End If
    End Sub

    Public Function HasFlag(flag As String) As Boolean Implements IActor.HasFlag
        Return ActorData.Flags.Contains(flag)
    End Function

    Public Sub SetFlag(flag As String) Implements IActor.SetFlag
        ActorData.Flags.Add(flag)
    End Sub

    Public Sub AddPlace(place As IPlace) Implements IActor.AddPlace
        If Not ActorData.Places.Discovered.ContainsKey(place.Id) Then
            ActorData.Places.Discovered(place.Id) = Turn
        End If
        ActorData.Places.Visited(place.Id) = Turn
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
            Return ActorData.Metadatas(MetadataTypes.ActorType)
        End Get
    End Property

    Public Property MaximumOxygen As Integer Implements IActor.MaximumOxygen
        Get
            Return Math.Max(0, ActorData.Statistics(StatisticTypes.MaximumOxygen))
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.MaximumOxygen) = Math.Max(0, value)
        End Set
    End Property

    Public Property Oxygen As Integer Implements IActor.Oxygen
        Get
            Return Math.Clamp(ActorData.Statistics(StatisticTypes.Oxygen), 0, MaximumOxygen)
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.Oxygen) = Math.Clamp(value, 0, MaximumOxygen)
        End Set
    End Property

    Public Property Facing As Integer Implements IActor.Facing
        Get
            Return ActorData.Statistics(StatisticTypes.Facing)
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.Facing) = value
        End Set
    End Property

    Public ReadOnly Property HasTutorial As Boolean Implements IActor.HasTutorial
        Get
            Return ActorData.Tutorials.Any
        End Get
    End Property

    Public ReadOnly Property CurrentTutorial As String Implements IActor.CurrentTutorial
        Get
            If HasTutorial Then
                Return ActorData.Tutorials.Peek
            End If
            Return Nothing
        End Get
    End Property

    Public Property MaximumFuel As Integer Implements IActor.MaximumFuel
        Get
            Return Math.Max(0, ActorData.Statistics(StatisticTypes.MaximumFuel))
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.MaximumFuel) = Math.Max(0, value)
        End Set
    End Property

    Public Property Fuel As Integer Implements IActor.Fuel
        Get
            Return Math.Clamp(ActorData.Statistics(StatisticTypes.Fuel), 0, MaximumFuel)
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.Fuel) = Math.Clamp(value, 0, MaximumFuel)
        End Set
    End Property

    Public ReadOnly Property LegacyKnowsPlaces As Boolean Implements IActor.LegacyKnowsPlaces
        Get
            Return ActorData.Places.Discovered.Any
        End Get
    End Property

    Public ReadOnly Property KnownPlaces As IEnumerable(Of IPlace) Implements IActor.KnownPlaces
        Get
            Return ActorData.Places.Discovered.Select(Function(x) New Place(UniverseData, x.Key)).Where(Function(x) x.PlaceType = PlaceTypes.StarSystem).OrderBy(Function(x) x.Name)
        End Get
    End Property

    Public Property Turn As Integer Implements IActor.Turn
        Get
            Return ActorData.Statistics(StatisticTypes.Turn)
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.Turn) = value
        End Set
    End Property

    Public Property Jools As Integer Implements IActor.Jools
        Get
            Return ActorData.Statistics(StatisticTypes.Jools)
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.Jools) = value
        End Set
    End Property

    Public ReadOnly Property HasFuel As Boolean Implements IActor.HasFuel
        Get
            Return Fuel > 0
        End Get
    End Property

    Public ReadOnly Property KnowsPlanetVicinities As Boolean Implements IActor.KnowsPlanetVicinities
        Get
            Return ActorData.PlanetVicinities.Discovered.Any
        End Get
    End Property

    Public ReadOnly Property LegacyKnownPlanetVicinities As IEnumerable(Of IPlace) Implements IActor.LegacyKnownPlanetVicinities
        Get
            Return ActorData.PlanetVicinities.Discovered.Select(Function(x) New Place(UniverseData, x.Key)).Where(Function(x) x.PlaceType = PlaceTypes.PlanetVicinity).OrderBy(Function(x) x.Name)
        End Get
    End Property

    Public Property MinimumJools As Integer Implements IActor.MinimumJools
        Get
            Return ActorData.Statistics(StatisticTypes.MinimumJools)
        End Get
        Set(value As Integer)
            ActorData.Statistics(StatisticTypes.MinimumJools) = value
        End Set
    End Property

    Public Property Faction As IFaction Implements IActor.Faction
        Get
            Dim id = 0
            If ActorData.Statistics.TryGetValue(StatisticTypes.FactionId, id) Then
                Return New Faction(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IFaction)
            If value IsNot Nothing Then
                ActorData.Statistics(StatisticTypes.FactionId) = value.Id
            Else
                ActorData.Statistics.Remove(StatisticTypes.FactionId)
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
            If ActorData.Statistics.TryGetValue(StatisticTypes.HomePlanetId, id) Then
                Return New Place(UniverseData, id)
            End If
            Return Nothing
        End Get
        Set(value As IPlace)
            If value IsNot Nothing Then
                ActorData.Statistics(StatisticTypes.HomePlanetId) = value.Id
            Else
                ActorData.Statistics.Remove(StatisticTypes.HomePlanetId)
            End If
        End Set
    End Property
End Class
