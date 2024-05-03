Imports FHOS.Data

Friend Class StarVicinity
    Inherits Star
    Implements IStarVicinity

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Public Function CreateStar() As IStar Implements IStarVicinity.CreateStar
        Dim star = New Star(
            UniverseData,
                UniverseData.Places.CreateOrRecycle(
                New PlaceData With
                {
                    .Metadatas = New Dictionary(Of String, String) From
                    {
                        {MetadataTypes.Name, Name},
                        {MetadataTypes.PlaceType, PlaceTypes.Star},
                        {MetadataTypes.StarType, StarType}
                    },
                    .Statistics = New Dictionary(Of String, Integer) From
                    {
                        {StatisticTypes.StarVicinityId, Id},
                        {StatisticTypes.ParentId, Id}
                    }
                }))
        AddPlace(star)
        Return star
    End Function
End Class
