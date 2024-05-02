Friend Class LocationModel
    Implements ILocationModel

    Private ReadOnly location As Persistence.ILocation

    Public Sub New(universe As Persistence.IUniverse, boardPosition As (X As Integer, Y As Integer))
        Dim mapPosition As (X As Integer, Y As Integer) = (boardPosition.X + universe.Avatar.Location.Column, boardPosition.Y + universe.Avatar.Location.Row)
        Me.location = universe.Avatar.Location.Map.GetLocation(mapPosition.X, mapPosition.Y)
    End Sub

    Public ReadOnly Property Exists As Boolean Implements ILocationModel.Exists
        Get
            Return location IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property LocationType As ILocationTypeModel Implements ILocationModel.LocationType
        Get
            If Not Exists Then
                Return Nothing
            End If
            Return New LocationTypeModel(location)
        End Get
    End Property

    Public ReadOnly Property Actor As IActorModel Implements ILocationModel.Actor
        Get
            Dim locationActor = location?.Actor
            If locationActor Is Nothing Then
                Return Nothing
            End If
            Return New ActorModel(locationActor)
        End Get
    End Property

    Public ReadOnly Property StarSystem As IStarSystemModel Implements ILocationModel.StarSystem
        Get
            Dim locationStarSystem = location.LegacyStarSystem
            If locationStarSystem Is Nothing Then
                Return Nothing
            End If
            Return New StarSystemModel(locationStarSystem)
        End Get
    End Property

    Public ReadOnly Property HasDetails As Boolean Implements ILocationModel.HasDetails
        Get
            Return StarSystem IsNot Nothing OrElse Actor IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property Star As IStarVicinityModel Implements ILocationModel.Star
        Get
            Dim locationStar = location.LegacyStarVicinity
            If locationStar Is Nothing Then
                Return Nothing
            End If
            Return New StarVicinityModel(locationStar)
        End Get
    End Property
End Class
