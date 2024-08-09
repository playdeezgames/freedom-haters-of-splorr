Friend MustInherit Class TechLevelItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Protected techLevel As Integer

    Protected Sub New(
                     itemType As String,
                     name As String,
                     techLevel As Integer,
                     Optional offer As Integer = 0,
                     Optional price As Integer = 0,
                     Optional installFee As Integer = 0,
                     Optional uninstallFee As Integer = 0)
        MyBase.New(itemType, name, offer, price, installFee, uninstallFee)
        Me.techLevel = techLevel
    End Sub
End Class
