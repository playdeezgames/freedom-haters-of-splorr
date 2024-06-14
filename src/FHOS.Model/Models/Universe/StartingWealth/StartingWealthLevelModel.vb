Friend Class StartingWealthLevelModel
    Implements IStartingWealthLevelModel
    Private ReadOnly embarkSettings As EmbarkSettings
    Private ReadOnly persistSettings As Action
    Protected Sub New(embarkSettings As EmbarkSettings, persistSettings As Action)
        Me.embarkSettings = embarkSettings
        Me.persistSettings = persistSettings
    End Sub

    Friend Shared Function FromSettings(embarkSettings As EmbarkSettings, persistSettings As Action) As IStartingWealthLevelModel
        Return New StartingWealthLevelModel(embarkSettings, persistSettings)
    End Function


    Public ReadOnly Property Current As String Implements IStartingWealthLevelModel.Current
        Get
            Return embarkSettings.StartingWealthLevel
        End Get
    End Property

    Public ReadOnly Property CurrentName As String Implements IStartingWealthLevelModel.CurrentName
        Get
            Return StartingWealthLevels.Descriptors(embarkSettings.StartingWealthLevel).DisplayName
        End Get
    End Property

    Public ReadOnly Property Options As IEnumerable(Of (Text As String, Item As String)) Implements IStartingWealthLevelModel.Options
        Get
            Return StartingWealthLevels.Descriptors.OrderBy(Function(x) x.Value.Ordinal).Select(Function(x) (x.Value.DisplayName, x.Key))
        End Get
    End Property

    Public Sub SetWealthLevel(wealthLevel As String) Implements IStartingWealthLevelModel.SetWealthLevel
        embarkSettings.StartingWealthLevel = wealthLevel
        persistSettings()
    End Sub
End Class
