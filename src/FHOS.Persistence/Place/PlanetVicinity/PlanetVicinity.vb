Imports FHOS.Data

Friend Class PlanetVicinity
    Inherits Planet
    Implements IPlanetVicinity

    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData, planetId)
    End Sub

    Private Sub AddPlanet(planet As IPlanet)
        PlaceData.Descendants.Add(planet.Id)
    End Sub

    Private Sub LegacyAddSatellite(satellite As IPlace)
        PlaceData.Descendants.Add(satellite.Id)
    End Sub

    Public Function CreatePlanet() As IPlanet Implements IPlanetVicinity.CreatePlanet
        Dim planet As IPlanet = New Planet(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.PlaceType, PlaceTypes.Planet},
                        {MetadataTypes.PlanetType, PlanetType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.PlanetVicinityId, Id}
                    }
                }))
        AddPlanet(planet)
        Return planet
    End Function

    Public Function LegacyCreateSatellite(satelliteName As String, satelliteType As String) As IPlace Implements IPlanetVicinity.LegacyCreateSatellite
        Dim satellite As IPlace = New Place(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, satelliteName},
                        {MetadataTypes.PlaceType, PlaceTypes.Satellite},
                        {MetadataTypes.SatelliteType, satelliteType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.PlanetVicinityId, Id}
                    }
                }))
        LegacyAddSatellite(satellite)
        Return satellite
    End Function

    Public Function CreateSatellite(satelliteName As String, satelliteType As String) As ISatellite Implements IPlanetVicinity.CreateSatellite
        Dim satellite As ISatellite = New Satellite(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, satelliteName},
                        {MetadataTypes.PlaceType, PlaceTypes.Satellite},
                        {MetadataTypes.SatelliteType, satelliteType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.PlanetVicinityId, Id}
                    }
                }))
        AddSatellite(satellite)
        Return satellite
    End Function

    Private Sub AddSatellite(satellite As ISatellite)
        PlaceData.Descendants.Add(satellite.Id)
    End Sub
End Class
