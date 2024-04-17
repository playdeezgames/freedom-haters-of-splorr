Friend Class GalacticDensityModel
    Implements IGalacticDensityModel
    Private Shared _galacticDensity As String = GalacticDensities.Average

    Public ReadOnly Property GalacticDensityName As String Implements IGalacticDensityModel.GalacticDensityName
        Get
            Return GalacticDensities.Descriptors(_galacticDensity).DisplayName
        End Get
    End Property

    Public ReadOnly Property GalacticDensityOptions As IEnumerable(Of (Text As String, Item As String)) Implements IGalacticDensityModel.GalacticDensityOptions
        Get
            Return GalacticDensities.Descriptors.OrderBy(Function(x) x.Value.Ordinal).Select(Function(x) (x.Value.DisplayName, x.Key))
        End Get
    End Property

    Public ReadOnly Property Value As String Implements IGalacticDensityModel.Value
        Get
            Return _galacticDensity
        End Get
    End Property

    Public Sub SetGalacticDensity(density As String) Implements IGalacticDensityModel.SetGalacticDensity
        _galacticDensity = density
    End Sub
End Class
