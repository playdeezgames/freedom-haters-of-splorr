Imports FHOS.Persistence

Friend Class AvatarStateModel
    Inherits BaseAvatarModel
    Implements IAvatarStateModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property Position As (X As Integer, Y As Integer) Implements IAvatarStateModel.Position
        Get
            With avatar.Location
                Return (.Column, .Row)
            End With
        End Get
    End Property

    Public ReadOnly Property MapName As String Implements IAvatarStateModel.MapName
        Get
            Return avatar.Location.Map.Name
        End Get
    End Property

    Public ReadOnly Property CurrentPlace As IPlaceModel Implements IAvatarStateModel.CurrentPlace
        Get
            If avatar.Location.Place IsNot Nothing Then
                Return PlaceModel.FromPlace(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property

    Friend Shared Function FromActor(avatar As IActor) As IAvatarStateModel
        Return New AvatarStateModel(avatar)
    End Function
End Class
