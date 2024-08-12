Friend MustInherit Class CommodityTypeDescriptor
    ReadOnly Property CommodityType As String
    ReadOnly Property BasePricePerUnit As Double
    ReadOnly Property MarkUp As Double
    ReadOnly Property SupplyFactorPerUnit As Double
    ReadOnly Property DemandFactorPerUnit As Double
    MustOverride Function GenerateSupply() As Double
    MustOverride Function GenerateDemand() As Double
    Sub New(commodityType As String)
        Me.CommodityType = commodityType
    End Sub
End Class
