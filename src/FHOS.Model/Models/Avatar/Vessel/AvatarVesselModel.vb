Imports FHOS.Persistence

Friend Class AvatarVesselModel
    Inherits BaseAvatarModel
    Implements IAvatarVesselModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarVesselModel.OxygenPercent
        Get
            Return actor.LifeSupport.Percent.Value
        End Get
    End Property

    Public ReadOnly Property FuelPercent As Integer? Implements IAvatarVesselModel.FuelPercent
        Get
            Return actor.FuelTank?.Percent
        End Get
    End Property

    Public ReadOnly Property OxygenQuantity As Integer Implements IAvatarVesselModel.OxygenQuantity
        Get
            Return actor.LifeSupport.CurrentValue
        End Get
    End Property

    Public ReadOnly Property OxygenMaximum As Integer Implements IAvatarVesselModel.OxygenMaximum
        Get
            Return actor.LifeSupport.MaximumValue.Value
        End Get
    End Property

    Public ReadOnly Property FuelQuantity As Integer? Implements IAvatarVesselModel.FuelQuantity
        Get
            Return actor.FuelTank?.CurrentValue
        End Get
    End Property

    Public ReadOnly Property FuelMaximum As Integer? Implements IAvatarVesselModel.FuelMaximum
        Get
            Return actor.FuelTank?.MaximumValue
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarVesselModel
        Return New AvatarVesselModel(actor)
    End Function
End Class
