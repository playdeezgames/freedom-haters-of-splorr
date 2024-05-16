Friend Class LocationModel
    Implements ILocationModel

    Private ReadOnly location As Persistence.ILocation

    Protected Sub New(universe As Persistence.IUniverse, boardPosition As (X As Integer, Y As Integer))
        Dim mapPosition As (X As Integer, Y As Integer) = (
            boardPosition.X + universe.Avatar.AvatarActor.Location.Column,
            boardPosition.Y + universe.Avatar.AvatarActor.Location.Row)
        Me.location = universe.Avatar.AvatarActor.Location.Map.GetLocation(mapPosition.X, mapPosition.Y)
    End Sub

    Friend Shared Function FromBoardPosition(universe As Persistence.IUniverse, boardPosition As (X As Integer, Y As Integer)) As ILocationModel
        Return New LocationModel(universe, boardPosition)
    End Function

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
            Return ActorModel.FromActor(locationActor)
        End Get
    End Property

    Public ReadOnly Property Place As IPlaceModel Implements ILocationModel.Place
        Get
            Dim locationStarSystem = location.Place
            If locationStarSystem Is Nothing Then
                Return Nothing
            End If
            Return PlaceModel.FromPlace(locationStarSystem)
        End Get
    End Property

    Public ReadOnly Property HasDetails As Boolean Implements ILocationModel.HasDetails
        Get
            Return Place IsNot Nothing OrElse Actor IsNot Nothing
        End Get
    End Property
End Class
