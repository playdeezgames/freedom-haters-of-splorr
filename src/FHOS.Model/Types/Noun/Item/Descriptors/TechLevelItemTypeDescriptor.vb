Friend MustInherit Class TechLevelItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Protected Sub New(
                     itemType As String,
                     name As String,
                     Optional offer As Integer = 0,
                     Optional price As Integer = 0,
                     Optional installFee As Integer = 0,
                     Optional uninstallFee As Integer = 0)
        MyBase.New(itemType, name, offer, price, installFee, uninstallFee)
    End Sub
End Class
