Public Class GroupValueModel
    Implements IGroupValueModel

    Private ReadOnly valueId As Integer

    Public Sub New(valueId As Integer)
        Me.valueId = valueId
    End Sub

    Public ReadOnly Property Name As String Implements IGroupValueModel.Name
        Get
            Return GroupValues.Descriptors(valueId).Name
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IGroupValueModel.Description
        Get
            Return GroupValues.Descriptors(valueId).Description
        End Get
    End Property

    Friend Shared Function FromValueId(valueId As Integer) As IGroupValueModel
        Return New GroupValueModel(valueId)
    End Function
End Class
