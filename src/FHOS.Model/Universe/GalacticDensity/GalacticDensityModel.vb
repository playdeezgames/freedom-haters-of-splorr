Friend Class GalacticDensityModel
    Implements IGalacticDensityModel
    Private ReadOnly embarkSettings As EmbarkSettings
    Private ReadOnly persistSettings As Action
    Sub New(embarkSettings As EmbarkSettings, persistSettings As Action)
        Me.embarkSettings = embarkSettings
        Me.persistSettings = persistSettings
    End Sub

    Public ReadOnly Property CurrentName As String Implements IGalacticDensityModel.CurrentName
        Get
            Return GalacticDensities.Descriptors(embarkSettings.GalacticDensity).DisplayName
        End Get
    End Property
    Public ReadOnly Property Current As String Implements IGalacticDensityModel.Current
        Get
            Return embarkSettings.GalacticDensity
        End Get
    End Property


    Public ReadOnly Property Options As IEnumerable(Of (Text As String, Item As String)) Implements IGalacticDensityModel.Options
        Get
            Return GalacticDensities.Descriptors.OrderBy(Function(x) x.Value.Ordinal).Select(Function(x) (x.Value.DisplayName, x.Key))
        End Get
    End Property

    Public Sub SetDensity(density As String) Implements IGalacticDensityModel.SetDensity
        embarkSettings.GalacticDensity = density
        persistSettings()
    End Sub
End Class
