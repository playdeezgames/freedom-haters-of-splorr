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

    Public Sub AddStarSystem(starSystem As IStarSystem) Implements IActor.AddStarSystem
        If Not ActorData.StarSystems.Discovered.ContainsKey(starSystem.Id) Then
            ActorData.StarSystems.Discovered(starSystem.Id) = Turn
        End If
        ActorData.StarSystems.Visited(starSystem.Id) = Turn
    End Sub

    Public Sub AddMessage(header As String, ParamArray lines() As (Text As String, Hue As Integer)) Implements IActor.AddMessage
        UniverseData.Messages.Enqueue(New MessageData With
                                  {
                                    .Header = header,
                                    .Lines = lines.Select(Function(x) New MessageLineData With {.Text = x.Text, .Hue = x.Hue}).ToList
                                  })
    End Sub

    Public Sub AddStarVicinity(starVicinity As IStarVicinity) Implements IActor.AddStarVicinity
        If Not ActorData.StarVicinities.Discovered.ContainsKey(starVicinity.Id) Then
            ActorData.StarVicinities.Discovered(starVicinity.Id) = Turn
        End If
        ActorData.StarVicinities.Visited(starVicinity.Id) = Turn
    End Sub

    Public Sub AddPlanetVicinity(planetVicinity As IPlanetVicinity) Implements IActor.AddPlanetVicinity
        If Not ActorData.PlanetVicinities.Discovered.ContainsKey(planetVicinity.Id) Then
            ActorData.PlanetVicinities.Discovered(planetVicinity.Id) = Turn
        End If
        ActorData.PlanetVicinities.Visited(planetVicinity.Id) = Turn
    End Sub

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

    Public ReadOnly Property KnowsStarSystems As Boolean Implements IActor.KnowsStarSystems
        Get
            Return ActorData.StarSystems.Discovered.Any
        End Get
    End Property

    Public ReadOnly Property KnownStarSystems As IEnumerable(Of IStarSystem) Implements IActor.KnownStarSystems
        Get
            Return ActorData.StarSystems.Discovered.Select(Function(x) New StarSystem(UniverseData, x.Key)).OrderBy(Function(x) x.Name)
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
End Class
