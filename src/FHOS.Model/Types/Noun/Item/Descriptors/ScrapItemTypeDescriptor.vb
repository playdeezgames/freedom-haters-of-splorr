﻿Imports FHOS.Data
Imports FHOS.Persistence

Friend Class ScrapItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.Scrap,
            "Scrap",
            New Dictionary(Of String, Double),
            offer:=1)
    End Sub

    Public Overrides ReadOnly Property Description(item As IItem) As IEnumerable(Of String)
        Get
            Return {"This item is a pile of junk that was floating around in space."}
        End Get
    End Property

    Protected Overrides Sub Initialize(item As IItem)
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog)
    End Function
End Class
