Imports FHOS.Persistence

Friend Class AvatarStateModel
    Inherits BaseAvatarModel
    Implements IAvatarStateModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property Position As (X As Integer, Y As Integer) Implements IAvatarStateModel.Position
        Get
            With actor.Location
                Return .Position
            End With
        End Get
    End Property

    Public ReadOnly Property MapName As String Implements IAvatarStateModel.MapName
        Get
            Return actor.Location.Map.EntityName
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarStateModel
        Return New AvatarStateModel(actor)
    End Function
End Class
