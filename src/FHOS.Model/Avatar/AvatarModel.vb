Imports FHOS.Persistence

Friend Class AvatarModel
    Inherits BaseAvatarModel
    Implements IAvatarModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarModel
        If actor Is Nothing Then
            Return Nothing
        End If
        Return New AvatarModel(actor)
    End Function

    Public ReadOnly Property Tutorial As IAvatarTutorialModel Implements IAvatarModel.Tutorial
        Get
            Return New AvatarTutorialModel(avatar)
        End Get
    End Property

    Public ReadOnly Property Bio As IAvatarBioModel Implements IAvatarModel.Bio
        Get
            Return AvatarBioModel.FromActor(avatar)
        End Get
    End Property

    Public ReadOnly Property Verbs As IAvatarVerbsModel Implements IAvatarModel.Verbs
        Get
            Return AvatarVerbsModel.FromActor(avatar)
        End Get
    End Property

    Public ReadOnly Property KnownPlaces As IAvatarKnownPlacesModel Implements IAvatarModel.KnownPlaces
        Get
            Return AvatarKnownPlacesModel.FromActor(avatar)
        End Get
    End Property

    Public ReadOnly Property Stack As IAvatarStackModel Implements IAvatarModel.Stack
        Get
            Return AvatarStackModel.FromActor(avatar)
        End Get
    End Property

    Public ReadOnly Property Status As IAvatarStatusModel Implements IAvatarModel.Status
        Get
            Return AvatarStatusModel.FromActor(avatar)
        End Get
    End Property

    Public ReadOnly Property State As IAvatarStateModel Implements IAvatarModel.State
        Get
            Return AvatarStateModel.FromActor(avatar)
        End Get
    End Property

    Public ReadOnly Property Wallet As IAvatarWalletModel Implements IAvatarModel.Wallet
        Get
            Return AvatarWalletModel.FromActor(avatar)
        End Get
    End Property

    Public ReadOnly Property Interaction As IAvatarInteractionModel Implements IAvatarModel.Interaction
        Get
            Return AvatarInteractionModel.FromActor(avatar)
        End Get
    End Property

    Public ReadOnly Property Vessel As IAvatarVesselModel Implements IAvatarModel.Vessel
        Get
            Return AvatarVesselModel.FromActor(avatar)
        End Get
    End Property
End Class
