Friend Class GalacticAgeModel
    Implements IGalacticAgeModel
    Private _galacticAge As String = GalacticAges.Average

    Public ReadOnly Property CurrentName As String Implements IGalacticAgeModel.CurrentName
        Get
            Return GalacticAges.Descriptors(_galacticAge).DisplayName
        End Get
    End Property

    Public ReadOnly Property Options As IEnumerable(Of (Text As String, Item As String)) Implements IGalacticAgeModel.Options
        Get
            Return GalacticAges.Descriptors.OrderBy(Function(x) x.Value.Ordinal).Select(Function(x) (x.Value.DisplayName, x.Key))
        End Get
    End Property

    Public ReadOnly Property Value As String Implements IGalacticAgeModel.Value
        Get
            Return _galacticAge
        End Get
    End Property

    Public Sub SetAge(age As String) Implements IGalacticAgeModel.SetAge
        _galacticAge = age
    End Sub
End Class
