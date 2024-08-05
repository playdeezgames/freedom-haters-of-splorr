Imports FHOS.Persistence

Friend Class ItemModel
    Implements IItemModel

    Private ReadOnly item As IItem

    Public Sub New(item As IItem)
        Me.item = item
    End Sub

    Public ReadOnly Property DisplayName As String Implements IItemModel.DisplayName
        Get
            Return item.EntityName
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IItemModel.Description
        Get
            Return item.Descriptor.Description(item)
        End Get
    End Property

    Public ReadOnly Property InstallFee As Integer Implements IItemModel.InstallFee
        Get
            Return item.Descriptor.InstallFee
        End Get
    End Property

    Friend Shared Function FromItem(item As IItem) As IItemModel
        Return New ItemModel(item)
    End Function
    Friend Shared Function GetItem(model As IItemModel) As IItem
        Return CType(model, ItemModel).item
    End Function
End Class
