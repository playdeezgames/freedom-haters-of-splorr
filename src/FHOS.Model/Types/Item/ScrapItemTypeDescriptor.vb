Imports FHOS.Persistence

Friend Class ScrapItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(ItemTypes.Scrap, "Scrap", offer:=1)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
    End Sub
End Class
