Friend Class FactionCountModel
    Implements IFactionCountModel

    Private embarkSettings As EmbarkSettings
    Private persistSettings As Action

    Public Sub New(embarkSettings As EmbarkSettings, value As Action)
        Me.embarkSettings = embarkSettings
        Me.persistSettings = value
    End Sub

    Public ReadOnly Property CurrentName As String Implements IFactionCountModel.CurrentName
        Get
            Return FactionCounts.Descriptors(embarkSettings.FactionCount).Name
        End Get
    End Property
End Class
