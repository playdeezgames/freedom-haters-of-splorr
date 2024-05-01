Friend Class Place
    Inherits PlaceDataClient
    Implements IPlace

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlace.Id
        Get
            Return PlaceId
        End Get
    End Property

    Public ReadOnly Property Universe As IUniverse Implements IPlace.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property
End Class
