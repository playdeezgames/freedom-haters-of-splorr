Imports FHOS.Data

Friend Class Place
    Inherits PlaceDataClient
    Implements IPlace

    Protected Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, placeId As Integer?) As IPlace
        If placeId.HasValue Then
            Return New Place(universeData, placeId.Value)
        End If
        Return Nothing
    End Function

    Public ReadOnly Property PlaceType As String Implements IPlace.PlaceType
        Get
            Return EntityData.Metadatas(MetadataTypes.PlaceType)
        End Get
    End Property

    Public ReadOnly Property Subtype As String Implements IPlace.Subtype
        Get
            Return EntityData.Metadatas(MetadataTypes.Subtype)
        End Get
    End Property

    Public ReadOnly Property Family As IPlaceFamily Implements IPlace.Family
        Get
            Return PlaceFamily.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Properties As IPlaceProperties Implements IPlace.Properties
        Get
            Return PlaceProperties.FromId(UniverseData, Id)
        End Get
    End Property

    Public ReadOnly Property Factory As IPlaceFactory Implements IPlace.Factory
        Get
            Return PlaceFactory.FromId(UniverseData, Id)
        End Get
    End Property
End Class
