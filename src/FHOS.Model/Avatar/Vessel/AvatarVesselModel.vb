Imports FHOS.Persistence

Friend Class AvatarVesselModel
    Inherits BaseAvatarModel
    Implements IAvatarVesselModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property AvailableCrew As IEnumerable(Of (Name As String, Actor As IActorModel)) Implements IAvatarVesselModel.AvailableCrew
        Get
            Return avatar.Crew.Select(Function(x) (x.Name, ActorModel.FromActor(x)))
        End Get
    End Property

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarVesselModel.OxygenPercent
        Get
            Return avatar.LifeSupport.CurrentValue * 100 \ avatar.LifeSupport.MaximumValue.Value
        End Get
    End Property

    Public ReadOnly Property FuelPercent As Integer Implements IAvatarVesselModel.FuelPercent
        Get
            Return avatar.Fuel * 100 \ avatar.MaximumFuel
        End Get
    End Property

    Friend Shared Function FromActor(avatar As IActor) As IAvatarVesselModel
        Return New AvatarVesselModel(avatar)
    End Function
End Class
