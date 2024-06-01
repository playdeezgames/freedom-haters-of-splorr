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

    Public ReadOnly Property Groups As IEnumerable(Of IGroup) Implements IUniverse.Groups
        Get
            Dim factionIds = New HashSet(Of Integer)(Enumerable.Range(0, UniverseData.Groups.Entities.Count))
            For Each recycledId In UniverseData.Groups.Recycled
                factionIds.Remove(recycledId)
            Next
            Return factionIds.Select(Function(x) Group.FromId(UniverseData, x))
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
            Return UniverseData.Statistics(LegacyStatisticTypes.Turn)
        End Get
        Set(value As Integer)
            UniverseData.Statistics(LegacyStatisticTypes.Turn) = value
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
End Class
