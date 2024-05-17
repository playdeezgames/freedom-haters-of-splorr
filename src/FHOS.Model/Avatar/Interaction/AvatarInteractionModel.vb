Imports FHOS.Persistence

Friend Class AvatarInteractionModel
    Inherits BaseAvatarModel
    Implements IAvatarInteractionModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarInteractionModel.IsActive
        Get
            Return actor.State.Interactor IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property AvailableChoices As List(Of (Text As String, Item As IInteractionModel)) Implements IAvatarInteractionModel.AvailableChoices
        Get
            Return {("Good-bye", CType(Nothing, IInteractionModel))}.ToList
        End Get
    End Property

    Public Sub Leave() Implements IAvatarInteractionModel.Leave
        actor.State.Interactor = Nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarInteractionModel
        Return New AvatarInteractionModel(actor)
    End Function
End Class
