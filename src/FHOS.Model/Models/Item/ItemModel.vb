Imports FHOS.Persistence

Friend Class ItemModel
    Implements IItemModel

    Private ReadOnly item As IItem

    Public Sub New(item As IItem)
        Me.item = item
    End Sub

    Public ReadOnly Property DisplayName As String Implements IItemModel.DisplayName
        Get
            Return item.Descriptor.Name
        End Get
    End Property

    Friend Shared Function FromItem(item As IItem) As IItemModel
        Return New ItemModel(item)
    End Function
End Class
