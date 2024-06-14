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

    Public ReadOnly Property Verbs As IAvatarVerbsModel Implements IAvatarModel.Verbs
        Get
            Return AvatarVerbsModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Stack As IAvatarStackModel Implements IAvatarModel.Stack
        Get
            Return AvatarStackModel.FromActor(actor)
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

    Public ReadOnly Property Interaction As IAvatarInteractionModel Implements IAvatarModel.Interaction
        Get
            Return AvatarInteractionModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Vessel As IAvatarVesselModel Implements IAvatarModel.Vessel
        Get
            Return AvatarVesselModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property StarGate As IAvatarStarGateModel Implements IAvatarModel.StarGate
        Get
            Return AvatarStarGateModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Jools As Integer Implements IAvatarModel.Jools
        Get
            Return actor.Yokes.YokedStore(YokeTypes.Wallet).CurrentValue
        End Get
    End Property
End Class
