Friend Class GalacticAgeModel
    Implements IGalacticAgeModel
    Private ReadOnly embarkSettings As EmbarkSettings
    Private ReadOnly persistSettings As Action
    Protected Sub New(embarkSettings As EmbarkSettings, persistSettings As Action)
        Me.embarkSettings = embarkSettings
        Me.persistSettings = persistSettings
    End Sub

    Friend Shared Function FromSettings(embarkSettings As EmbarkSettings, persistSettings As Action) As IGalacticAgeModel
        Return New GalacticAgeModel(embarkSettings, persistSettings)
    End Function

    Public ReadOnly Property CurrentName As String Implements IGalacticAgeModel.CurrentName
        Get
            Return GalacticAges.Descriptors(embarkSettings.GalacticAge).DisplayName
        End Get
    End Property
    Public ReadOnly Property Current As String Implements IGalacticAgeModel.Current
        Get
            Return embarkSettings.GalacticAge
        End Get
    End Property

    Public ReadOnly Property Options As IEnumerable(Of (Text As String, Item As String)) Implements IGalacticAgeModel.Options
        Get
            Return GalacticAges.Descriptors.OrderBy(Function(x) x.Value.Ordinal).Select(Function(x) (x.Value.DisplayName, x.Key))
        End Get
    End Property

    Public Sub SetAge(age As String) Implements IGalacticAgeModel.SetAge
        embarkSettings.GalacticAge = age
        persistSettings()
    End Sub
End Class
