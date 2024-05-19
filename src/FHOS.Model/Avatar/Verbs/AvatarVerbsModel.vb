Imports FHOS.Persistence

Friend Class AvatarVerbsModel
    Inherits BaseAvatarModel
    Implements IAvatarVerbsModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property Available As IEnumerable(Of (Text As String, VerbType As String)) Implements IAvatarVerbsModel.Available
        Get
            Return VerbTypes.Descriptors.Keys.
                Where(Function(x) VerbTypes.Descriptors(x).Visible AndAlso VerbTypes.Descriptors(x).IsAvailable(actor)).
                Select(Function(x) (VerbTypes.Descriptors(x).Text, x))
        End Get
    End Property

    Public ReadOnly Property HasAny As Boolean Implements IAvatarVerbsModel.HasAny
        Get
            Return Available.Any
        End Get
    End Property

    Public Sub Perform(verbType As String) Implements IAvatarVerbsModel.Perform
        If VerbTypes.Descriptors(verbType).IsAvailable(actor) Then
            VerbTypes.Descriptors(verbType).Perform(actor)
        End If
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarVerbsModel
        Return New AvatarVerbsModel(actor)
    End Function
End Class
