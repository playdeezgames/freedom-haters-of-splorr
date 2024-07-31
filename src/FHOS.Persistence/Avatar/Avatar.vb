Friend Class Avatar
    Inherits UniverseDataClient
    Implements IAvatar

    Protected Sub New(universeData As Data.UniverseData)
        MyBase.New(universeData)
    End Sub

    Public ReadOnly Property Actor As IActor Implements IAvatar.Actor
        Get
            If UniverseData.Avatar.HasValue Then
                Return Persistence.Actor.FromId(UniverseData, UniverseData.Avatar)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub SetActor(actor As IActor) Implements IAvatar.SetActor
        UniverseData.Avatar = actor?.Id
    End Sub

    Friend Shared Function FromData(universeData As Data.UniverseData) As IAvatar
        Return New Avatar(universeData)
    End Function

    Public Function RemoveActor() As IActor Implements IAvatar.RemoveActor
        Dim result = Persistence.Actor.FromId(UniverseData, UniverseData.Avatar)
        UniverseData.Avatar = Nothing
        Return result
    End Function
End Class
