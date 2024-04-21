Friend Class GalacticDensityModel
    Implements IGalacticDensityModel
    Private Shared _galacticDensity As String = GalacticDensities.Average

    Public ReadOnly Property CurrentName As String Implements IGalacticDensityModel.CurrentName
        Get
            Return GalacticDensities.Descriptors(_galacticDensity).DisplayName
        End Get
    End Property
    Public ReadOnly Property Current As String Implements IGalacticDensityModel.Current
        Get
            Return _galacticDensity
        End Get
    End Property


    Public ReadOnly Property Options As IEnumerable(Of (Text As String, Item As String)) Implements IGalacticDensityModel.Options
        Get
            Return GalacticDensities.Descriptors.OrderBy(Function(x) x.Value.Ordinal).Select(Function(x) (x.Value.DisplayName, x.Key))
        End Get
    End Property

    Public Sub SetDensity(density As String) Implements IGalacticDensityModel.SetDensity
        _galacticDensity = density
    End Sub
End Class
