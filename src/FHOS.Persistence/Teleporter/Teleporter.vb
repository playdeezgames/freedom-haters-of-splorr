Imports FHOS.Data

Friend Class Teleporter
    Inherits TeleporterDataClient
    Implements ITeleporter

    Protected Sub New(universeData As Data.UniverseData, teleporterId As Integer)
        MyBase.New(universeData, teleporterId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, teleporterId As Integer?) As ITeleporter
        If teleporterId.HasValue Then
            Return New Teleporter(universeData, teleporterId.Value)
        End If
        Return Nothing
    End Function

    Public ReadOnly Property Target As ILocation Implements ITeleporter.Target
        Get
            Return Location.FromId(UniverseData, EntityData.Statistics(StatisticTypes.LocationId))
        End Get
    End Property
End Class
