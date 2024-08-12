Friend Class ProductionCommodityTypeDescriptor
    Inherits CommodityTypeDescriptor
    Public Sub New()
        MyBase.New(CommodityTypes.Production)
    End Sub

    Public Overrides Function GenerateSupply() As Double
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GenerateDemand() As Double
        Throw New NotImplementedException()
    End Function
End Class
