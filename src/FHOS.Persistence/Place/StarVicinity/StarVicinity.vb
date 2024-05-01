Imports FHOS.Data

Friend Class StarVicinity
    Inherits Place
    Implements IStarVicinity

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Public ReadOnly Property StarType As String Implements IStarVicinity.StarType
        Get
            Return PlaceData.Metadatas(MetadataTypes.StarType)
        End Get
    End Property

    Private Sub LegacyAddStar(star As IPlace)
        PlaceData.Descendants.Add(star.Id)
    End Sub

    Public Function LegacyCreateStar() As IPlace Implements IStarVicinity.LegacyCreateStar
        Dim star = New Place(
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
                        {StatisticTypes.StarVicinityId, Id}
                    }
                }))
        LegacyAddStar(star)
        Return star
    End Function

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
                        {StatisticTypes.StarVicinityId, Id}
                    }
                }))
        AddStar(star)
        Return star
    End Function

    Private Sub AddStar(star As Star)
        PlaceData.Descendants.Add(star.Id)
    End Sub
End Class
