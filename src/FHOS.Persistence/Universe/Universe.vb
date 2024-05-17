Imports FHOS.Data

Public Class Universe
    Inherits UniverseDataClient
    Implements IUniverse
    Sub New(universeData As UniverseData)
        MyBase.New(universeData)
    End Sub

    Public ReadOnly Property Messages As IMessages Implements IUniverse.Messages
        Get
            Return New Messages(UniverseData.Messages)
        End Get
    End Property

    Public ReadOnly Property Places As IEnumerable(Of IPlace) Implements IUniverse.Places
        Get
            Dim placeIds = New HashSet(Of Integer)(Enumerable.Range(0, UniverseData.Places.Entities.Count))
            For Each recycledId In UniverseData.Places.Recycled
                placeIds.Remove(recycledId)
            Next
            Return placeIds.Select(Function(x) Place.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Factions As IEnumerable(Of IFaction) Implements IUniverse.Factions
        Get
            Dim factionIds = New HashSet(Of Integer)(Enumerable.Range(0, UniverseData.Factions.Entities.Count))
            For Each recycledId In UniverseData.Factions.Recycled
                factionIds.Remove(recycledId)
            Next
            Return factionIds.Select(Function(x) Faction.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Actors As IEnumerable(Of IActor) Implements IUniverse.Actors
        Get
            Dim actorIds = New HashSet(Of Integer)(Enumerable.Range(0, UniverseData.Actors.Entities.Count))
            For Each recycledId In UniverseData.Actors.Recycled
                actorIds.Remove(recycledId)
            Next
            Return actorIds.Select(Function(x) Actor.FromId(UniverseData, x))
        End Get
    End Property

    Public Property Turn As Integer Implements IUniverse.Turn
        Get
            Return UniverseData.Statistics(StatisticTypes.Turn)
        End Get
        Set(value As Integer)
            UniverseData.Statistics(StatisticTypes.Turn) = value
        End Set
    End Property


    Public ReadOnly Property Avatar As IAvatar Implements IUniverse.Avatar
        Get
            Return Persistence.Avatar.FromData(UniverseData)
        End Get
    End Property

    Public ReadOnly Property Factory As IUniverseFactory Implements IUniverse.Factory
        Get
            Return UniverseFactory.FromData(UniverseData)
        End Get
    End Property

    Public Function GetPlacesOfType(placeType As String) As IEnumerable(Of IPlace) Implements IUniverse.GetPlacesOfType
        Return Places.Where(Function(x) x.PlaceType = placeType)
    End Function
End Class
