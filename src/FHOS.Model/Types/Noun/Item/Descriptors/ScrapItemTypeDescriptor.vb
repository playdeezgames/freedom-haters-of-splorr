Imports FHOS.Persistence

Friend Class ScrapItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.Scrap,
            "Scrap",
            offer:=1)
    End Sub

    Public Overrides ReadOnly Property Description(item As IItem) As String
        Get
            Return "This item is a pile of junk that was floating around in space."
        End Get
    End Property

    Protected Overrides Sub Initialize(item As IItem)
    End Sub
End Class
