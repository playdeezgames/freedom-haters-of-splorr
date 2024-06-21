Imports Microsoft.Data.Sqlite

Friend Class ActorPrices
    Inherits ActorDataClient
    Implements IActorPrices

    Protected Sub New(universeData As Data.IUniverseData, connection As SqliteConnection, actorId As Integer)
        MyBase.New(universeData, connection, actorId)
    End Sub

    Public ReadOnly Property HasAny As Boolean Implements IActorPrices.HasAny
        Get
            Return False
        End Get
    End Property

    Friend Shared Function FromId(universeData As Data.IUniverseData, connection As SqliteConnection, id As Integer) As IActorPrices
        Return New ActorPrices(universeData, connection, id)
    End Function
End Class
