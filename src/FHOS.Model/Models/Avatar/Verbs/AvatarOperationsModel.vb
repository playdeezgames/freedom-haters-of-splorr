Imports FHOS.Persistence

Friend Class AvatarOperationsModel
    Inherits BaseAvatarModel
    Implements IAvatarOperationsModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property Available As IEnumerable(Of (Text As String, OperationType As String)) Implements IAvatarOperationsModel.Available
        Get
            Return OperationTypes.Descriptors.Keys.
                Where(Function(x) OperationTypes.Descriptors(x).Visible AndAlso OperationTypes.Descriptors(x).IsAvailable(actor)).
                Select(Function(x) (OperationTypes.Descriptors(x).Text, x))
        End Get
    End Property

    Public ReadOnly Property HasAny As Boolean Implements IAvatarOperationsModel.HasAny
        Get
            Return Available.Any
        End Get
    End Property

    Public Sub Perform(operationType As String) Implements IAvatarOperationsModel.Perform
        If OperationTypes.Descriptors(operationType).IsAvailable(actor) Then
            OperationTypes.Descriptors(operationType).Perform(actor)
        End If
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarOperationsModel
        Return New AvatarOperationsModel(actor)
    End Function
End Class
