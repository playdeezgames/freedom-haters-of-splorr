Imports FHOS.Data

Friend Class LocationDataClient
    Inherits TypedEntityDataClient(Of ILocationData)

    Public Sub New(
                  universeData As IUniverseData,
                  locationId As Integer)
        MyBase.New(
            universeData,
            locationId,
            Function(u, i) u.Locations.Entities(i),
            Sub(u, i) u.Locations.Recycled.Add(i))
    End Sub
End Class
