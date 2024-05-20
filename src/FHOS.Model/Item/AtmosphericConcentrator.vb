Imports FHOS.Persistence

Friend Class AtmosphericConcentrator
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(ItemTypes.AtmosphericConcentrator)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Properties.CanRefillOxygen = True
    End Sub
End Class
