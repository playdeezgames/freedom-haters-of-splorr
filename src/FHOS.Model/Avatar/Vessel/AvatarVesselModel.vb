Imports FHOS.Persistence

Friend Class AvatarVesselModel
    Inherits BaseAvatarModel
    Implements IAvatarVesselModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property AvailableCrew As IEnumerable(Of (Name As String, Actor As IActorModel)) Implements IAvatarVesselModel.AvailableCrew
        Get
            Return actor.Family.Children.Select(Function(x) (x.EntityName, ActorModel.FromActor(x)))
        End Get
    End Property

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarVesselModel.OxygenPercent
        Get
            Return actor.State.LifeSupport.Percent.Value
        End Get
    End Property

    Public ReadOnly Property FuelPercent As Integer? Implements IAvatarVesselModel.FuelPercent
        Get
            Return actor.State.FuelTank?.Percent
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarVesselModel
        Return New AvatarVesselModel(actor)
    End Function
End Class
