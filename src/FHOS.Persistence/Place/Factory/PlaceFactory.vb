Imports FHOS.Data

Friend Class PlaceFactory
    Inherits PlaceDataClient
    Implements IPlaceFactory

    Public Sub New(universeData As UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, id As Integer) As IPlaceFactory
        Return New PlaceFactory(universeData, id)
    End Function

    Public Function CreateStarVicinity(x As Integer, y As Integer) As IPlace Implements IPlaceFactory.CreateStarVicinity
        Dim starVicinity = Place.FromId(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.Subtype, Place.FromId(UniverseData, Id).Subtype},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                        {MetadataTypes.PlaceType, PlaceTypes.StarVicinity}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id},
                        {StatisticTypes.X, x},
                        {StatisticTypes.Y, y}
                    }
                }))
        AddPlace(starVicinity)
        Return starVicinity
    End Function

    Public Function CreatePlanetVicinity(
                                        planetName As String,
                                        planetType As String,
                                        x As Integer,
                                        y As Integer) As IPlace Implements IPlaceFactory.CreatePlanetVicinity
        Dim planetVicinity = Place.FromId(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, planetName},
                        {MetadataTypes.Subtype, planetType},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString},
                        {MetadataTypes.PlaceType, PlaceTypes.PlanetVicinity}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id},
                        {StatisticTypes.X, x},
                        {StatisticTypes.Y, y}
                    }
                }))
        AddPlace(planetVicinity)
        Return planetVicinity
    End Function

    Public Function CreateStar() As IPlace Implements IPlaceFactory.CreateStar
        Dim star = Place.FromId(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Place.FromId(UniverseData, Id).Properties.Name},
                        {MetadataTypes.PlaceType, PlaceTypes.Star},
                        {MetadataTypes.Subtype, Place.FromId(UniverseData, Id).Subtype}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(star)
        Return star
    End Function

    Public Function CreatePlanet() As IPlace Implements IPlaceFactory.CreatePlanet
        Dim planet As IPlace = Place.FromId(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Place.FromId(UniverseData, Id).Properties.Name},
                        {MetadataTypes.PlaceType, PlaceTypes.Planet},
                        {MetadataTypes.Subtype, Place.FromId(UniverseData, Id).Subtype},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(planet)
        Return planet
    End Function

    Public Function CreateSatellite(satelliteName As String, satelliteType As String) As IPlace Implements IPlaceFactory.CreateSatellite
        Dim satellite As IPlace = Place.FromId(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, satelliteName},
                        {MetadataTypes.PlaceType, PlaceTypes.Satellite},
                        {MetadataTypes.Subtype, satelliteType},
                        {MetadataTypes.Identifier, Guid.NewGuid.ToString}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(satellite)
        Return satellite
    End Function

    Protected Sub AddPlace(place As IPlace)
        EntityData.Descendants.Add(place.Id)
    End Sub
End Class
