Public Class GroupValueModel
    Implements IGroupValueModel

    Private ReadOnly valueId As Integer

    Public Sub New(valueId As Integer)
        Me.valueId = valueId
    End Sub

    Friend Shared Function FromValueId(valueId As Integer) As IGroupValueModel
        Return New GroupValueModel(valueId)
    End Function
End Class
