Imports FHOS.Persistence

Friend Class AvatarStateModel
    Inherits BaseAvatarModel
    Implements IAvatarStateModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property Position As (X As Integer, Y As Integer) Implements IAvatarStateModel.Position
        Get
            Return actor.Location.Position
        End Get
    End Property

    Public ReadOnly Property MapName As String Implements IAvatarStateModel.MapName
        Get
            Return actor.Location.Map.EntityName
        End Get
    End Property

    Public ReadOnly Property HasDialog As Boolean Implements IAvatarStateModel.HasDialog
        Get
            Return actor.Dialog IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property Dialog As IAvatarStateDialogModel Implements IAvatarStateModel.Dialog
        Get
            Return AvatarStateDialogModel.FromDialog(actor?.Dialog)
        End Get
    End Property

    Private ReadOnly Property IAvatarStateModel_Actor As IActorModel Implements IAvatarStateModel.Actor
        Get
            Return ActorModel.FromActor(actor)
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarStateModel
        Return New AvatarStateModel(actor)
    End Function
End Class
