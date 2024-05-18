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
            Return InteractionTypes.Descriptors.Where(Function(x) x.Value.IsAvailable(actor)).Select(Function(x) (x.Value.GetText(actor), x.Value.ToInteraction(actor))).ToList
        End Get
    End Property

    Public ReadOnly Property Lines As IEnumerable(Of (Text As String, Hue As Integer)) Implements IAvatarInteractionModel.Lines
        Get
            Dim result As New List(Of (Text As String, Hue As Integer))
            Dim interactor = actor.State.Interactor
            result.Add((interactor.Properties.Name, Hues.LightGray))
            result.AddRange(ActorTypes.Descriptors(interactor.ActorType).DescribeInteractor(interactor))
            Return result
        End Get
    End Property

    Public Sub Leave() Implements IAvatarInteractionModel.Leave
        actor.State.Interactor = Nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarInteractionModel
        Return New AvatarInteractionModel(actor)
    End Function
End Class
