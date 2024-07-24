Imports FHOS.Persistence

Friend Class AvatarShipyardModel
    Inherits AvatarYokedModel
    Implements IAvatarShipyardModel

    Public Sub New(actor As IActor)
        MyBase.New(actor, YokeTypes.Shipyard)
    End Sub

    Public ReadOnly Property CanChangeEquipment As Boolean Implements IAvatarShipyardModel.CanChangeEquipment
        Get
            Return actor.
                Equipment.
                GetRequiredSlots.
                Any(Function(x) actor.Inventory.Items.Any(Function(y) y.Descriptor.EquipSlot = x))
        End Get
    End Property

    Public ReadOnly Property CanInstallEquipment As Boolean Implements IAvatarShipyardModel.CanInstallEquipment
        Get
            Return actor.
                Equipment.
                GetInstallableSlots.
                Any(Function(x) actor.Inventory.Items.Any(Function(y) y.Descriptor.EquipSlot = x))
        End Get
    End Property

    Public ReadOnly Property CanUninstallEquipment As Boolean Implements IAvatarShipyardModel.CanUninstallEquipment
        Get
            Return actor.
                Equipment.
                GetUninstallableSlots().Any
        End Get
    End Property

    Protected Overrides Sub OnLeave()
        'do nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarShipyardModel
        Return New AvatarShipyardModel(actor)
    End Function
End Class
