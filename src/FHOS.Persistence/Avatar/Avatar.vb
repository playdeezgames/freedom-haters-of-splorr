Imports Microsoft.Data.Sqlite

Friend Class Avatar
    Inherits UniverseDataClient
    Implements IAvatar

    Protected Sub New(universeData As Data.IUniverseData, connection As SqliteConnection)
        MyBase.New(universeData, connection)
    End Sub

    Public ReadOnly Property Actor As IActor Implements IAvatar.Actor
        Get
            Dim avatarId As Integer
            If UniverseData.Avatars.TryPeek(avatarId) Then
                Return Persistence.Actor.FromId(UniverseData, Connection, avatarId)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub Push(actor As IActor) Implements IAvatar.Push
        UniverseData.Avatars.Push(actor.Id)
    End Sub

    Friend Shared Function FromData(universeData As Data.IUniverseData, connection As SqliteConnection) As IAvatar
        Return New Avatar(universeData, connection)
    End Function

    Public Function Pop() As IActor Implements IAvatar.Pop
        Return Persistence.Actor.FromId(UniverseData, Connection, UniverseData.Avatars.Pop())
    End Function
End Class
