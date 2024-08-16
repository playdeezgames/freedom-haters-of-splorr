Imports FHOS.Persistence

Friend Class AvatarModel
    Inherits BaseAvatarModel
    Implements IAvatarModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarModel
        If actor Is Nothing Then
            Return Nothing
        End If
        Return New AvatarModel(actor)
    End Function

    Public ReadOnly Property Bio As IAvatarBioModel Implements IAvatarModel.Bio
        Get
            Return AvatarBioModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Operations As IAvatarOperationsModel Implements IAvatarModel.Operations
        Get
            Return AvatarOperationsModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Status As IAvatarStatusModel Implements IAvatarModel.Status
        Get
            Return AvatarStatusModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property State As IAvatarStateModel Implements IAvatarModel.State
        Get
            Return AvatarStateModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Vessel As IAvatarVesselModel Implements IAvatarModel.Vessel
        Get
            Return AvatarVesselModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Jools As Integer Implements IAvatarModel.Jools
        Get
            Return actor.GetJools
        End Get
    End Property

    Public ReadOnly Property Inventory As IAvatarInventoryModel Implements IAvatarModel.Inventory
        Get
            Return AvatarInventoryModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Equipment As IAvatarEquipmentModel Implements IAvatarModel.Equipment
        Get
            Return AvatarEquipmentModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Yokes As IAvatarYokesModel Implements IAvatarModel.Yokes
        Get
            Return AvatarYokesModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Dialogs(model As IItemModel) As IEnumerable(Of (String, IAvatarItemDialogModel)) Implements IAvatarModel.Dialogs
        Get
            Dim item = ItemModel.GetItem(model)
            Return item.Descriptor.Dialogs(actor, item, actor.Dialog).Select(Function(x) (x.Key, AvatarItemDialogModel.FromDialog(actor, x.Value)))
        End Get
    End Property
End Class
