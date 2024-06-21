Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class ActorOffers
    Inherits ActorDataClient
    Implements IActorOffers

    Protected Sub New(universeData As IUniverseData, connection As SqliteConnection, actorId As Integer)
        MyBase.New(universeData, connection, actorId)
    End Sub

    Public ReadOnly Property HasAny As Boolean Implements IActorOffers.HasAny
        Get
            Return False
        End Get
    End Property

    Friend Shared Function FromId(universeData As IUniverseData, connection As SqliteConnection, id As Integer) As IActorOffers
        Return New ActorOffers(universeData, connection, id)
    End Function
End Class
