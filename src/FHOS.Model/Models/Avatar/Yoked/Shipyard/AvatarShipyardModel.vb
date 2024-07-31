Imports FHOS.Persistence

Friend Class AvatarShipyardModel
    Inherits AvatarYokedModel
    Implements IAvatarShipyardModel

    Public Sub New(actor As IActor)
        MyBase.New(actor, YokeTypes.Shipyard)
    End Sub

    Public ReadOnly Property CanChangeEquipment As Boolean Implements IAvatarShipyardModel.CanChangeEquipment
        Get
            Return ChangeableEquipmentSlots.Any
        End Get
    End Property

    Public ReadOnly Property CanInstallEquipment As Boolean Implements IAvatarShipyardModel.CanInstallEquipment
        Get
            Return InstallableEquipmentSlots.Any
        End Get
    End Property

    Public ReadOnly Property CanUninstallEquipment As Boolean Implements IAvatarShipyardModel.CanUninstallEquipment
        Get
            Return UninstallableEquipmentSlots.Any
        End Get
    End Property

    Public ReadOnly Property ChangeableEquipmentSlots As IEnumerable(Of IAvatarEquipmentSlotModel) Implements IAvatarShipyardModel.ChangeableEquipmentSlots
        Get
            Return actor.
                Equipment.
                GetRequiredSlots.
                Where(Function(x) actor.Inventory.Items.Any(Function(y) y.Descriptor.EquipSlot = x)).
                Select(Function(x) AvatarEquipmentSlotModel.FromActorAndSlot(actor, x))
        End Get
    End Property

    Public ReadOnly Property InstallableEquipmentSlots As IEnumerable(Of IAvatarEquipmentSlotModel) Implements IAvatarShipyardModel.InstallableEquipmentSlots
        Get
            Return actor.
                Equipment.
                GetInstallableSlots.
                Where(Function(x) actor.Inventory.Items.Any(Function(y) y.Descriptor.EquipSlot = x)).
                Select(Function(x) AvatarEquipmentSlotModel.FromActorAndSlot(actor, x))
        End Get
    End Property

    Public ReadOnly Property UninstallableEquipmentSlots As IEnumerable(Of IAvatarEquipmentSlotModel) Implements IAvatarShipyardModel.UninstallableEquipmentSlots
        Get
            Return actor.
                Equipment.
                GetUninstallableSlots.
                Select(Function(x) AvatarEquipmentSlotModel.FromActorAndSlot(actor, x))
        End Get
    End Property

    Protected Overrides Sub OnLeave()
        'do nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarShipyardModel
        Return New AvatarShipyardModel(actor)
    End Function
End Class
