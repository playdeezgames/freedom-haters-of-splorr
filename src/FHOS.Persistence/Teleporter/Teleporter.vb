Friend Class Teleporter
    Inherits TeleporterDataClient
    Implements ITeleporter

    Public Sub New(universeData As Data.UniverseData, teleporterId As Integer)
        MyBase.New(universeData, teleporterId)
    End Sub

    Public ReadOnly Property Target As ILocation Implements ITeleporter.Target
        Get
            Return New Location(UniverseData, EntityData.Statistics(StatisticTypes.LocationId))
        End Get
    End Property
End Class
