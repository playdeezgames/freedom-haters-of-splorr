Imports FHOS.Data

Friend Class Faction
    Inherits FactionDataClient
    Implements IFaction
    Friend Shared Function FromId(universeData As UniverseData, id As Integer?) As IFaction
        Return If(id.HasValue, New Faction(universeData, id.Value), Nothing)
    End Function

    Protected Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData, factionId)
    End Sub

    Public ReadOnly Property MinimumPlanetCount As Integer Implements IFaction.MinimumPlanetCount
        Get
            Return EntityData.Statistics(StatisticTypes.MinimumPlanetCount)
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IFaction.Name
        Get
            Return EntityData.Metadatas(MetadataTypes.Name)
        End Get
    End Property
End Class
