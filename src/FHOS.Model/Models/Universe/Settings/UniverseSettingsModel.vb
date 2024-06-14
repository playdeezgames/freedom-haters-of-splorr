Friend Class UniverseSettingsModel
    Implements IUniverseSettingsModel
    Private ReadOnly EmbarkSettings As EmbarkSettings
    Private ReadOnly PersistEmbarkSettings As Action

    Protected Sub New(embarkSettings As EmbarkSettings, persistEmbarkSettings As Action)
        Me.EmbarkSettings = embarkSettings
        Me.PersistEmbarkSettings = persistEmbarkSettings
    End Sub
    Friend Shared Function FromSettings(embarkSettings As EmbarkSettings, persistEmbarkSettings As Action) As IUniverseSettingsModel
        Return New UniverseSettingsModel(embarkSettings, persistEmbarkSettings)
    End Function

    Public ReadOnly Property GalacticAge As IGalacticAgeModel Implements IUniverseSettingsModel.GalacticAge
        Get
            Return GalacticAgeModel.FromSettings(EmbarkSettings, PersistEmbarkSettings)
        End Get
    End Property

    Public ReadOnly Property GalacticDensity As IGalacticDensityModel Implements IUniverseSettingsModel.GalacticDensity
        Get
            Return GalacticDensityModel.FromSettings(EmbarkSettings, PersistEmbarkSettings)
        End Get
    End Property

    Public ReadOnly Property StartingWealth As IStartingWealthLevelModel Implements IUniverseSettingsModel.StartingWealth
        Get
            Return StartingWealthLevelModel.FromSettings(EmbarkSettings, PersistEmbarkSettings)
        End Get
    End Property

    Public ReadOnly Property FactionCount As IFactionCountModel Implements IUniverseSettingsModel.FactionCount
        Get
            Return FactionCountModel.FromSettings(EmbarkSettings, PersistEmbarkSettings)
        End Get
    End Property
End Class
