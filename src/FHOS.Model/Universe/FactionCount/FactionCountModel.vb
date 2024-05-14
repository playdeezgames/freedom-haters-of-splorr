Friend Class FactionCountModel
    Implements IFactionCountModel

    Private ReadOnly embarkSettings As EmbarkSettings
    Private ReadOnly persistSettings As Action

    Protected Sub New(embarkSettings As EmbarkSettings, persistSettings As Action)
        Me.embarkSettings = embarkSettings
        Me.persistSettings = persistSettings
    End Sub

    Friend Shared Function FromSettings(embarkSettings As EmbarkSettings, persistSettings As Action) As IFactionCountModel
        Return New FactionCountModel(embarkSettings, persistSettings)
    End Function

    Public ReadOnly Property CurrentName As String Implements IFactionCountModel.CurrentName
        Get
            Return FactionCounts.Descriptors(embarkSettings.FactionCount).Name
        End Get
    End Property

    Public ReadOnly Property Current As Integer Implements IFactionCountModel.Current
        Get
            Return embarkSettings.FactionCount
        End Get
    End Property

    Public ReadOnly Property Options As IEnumerable(Of (Text As String, Item As Integer)) Implements IFactionCountModel.Options
        Get
            Return FactionCounts.Descriptors.OrderBy(Function(x) x.Key).Select(Function(x) (x.Value.Name, x.Key))
        End Get
    End Property

    Public Sub SetFactionCount(factionCount As Integer) Implements IFactionCountModel.SetFactionCount
        embarkSettings.FactionCount = factionCount
        persistSettings()
    End Sub
End Class
