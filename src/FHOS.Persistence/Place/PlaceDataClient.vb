Imports FHOS.Data

Friend Class PlaceDataClient
    Inherits EntityDataClient(Of PlaceData)

    Public Sub New(
                  universeData As Data.UniverseData,
                  placeId As Integer)
        MyBase.New(
            universeData,
            placeId,
            Function(u, i) u.Places.Entities(i),
            Sub(u, i) u.Places.Recycled.Add(i))
    End Sub
End Class
