Friend Class LocationModel
    Implements ILocationModel

    Private ReadOnly location As Persistence.ILocation

    Protected Sub New(universe As Persistence.IUniverse, boardPosition As (X As Integer, Y As Integer))
        Dim mapPosition As (X As Integer, Y As Integer) = (
            boardPosition.X + universe.Avatar.Actor.Location.Position.Column,
            boardPosition.Y + universe.Avatar.Actor.Location.Position.Row)
        Me.location = universe.Avatar.Actor.Location.Map.GetLocation(mapPosition.X, mapPosition.Y)
    End Sub

    Friend Shared Function FromBoardPosition(universe As Persistence.IUniverse, boardPosition As (X As Integer, Y As Integer)) As ILocationModel
        Return New LocationModel(universe, boardPosition)
    End Function

    Public ReadOnly Property Exists As Boolean Implements ILocationModel.Exists
        Get
            Return location IsNot Nothing
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

    Public ReadOnly Property HasActor As Boolean Implements ILocationModel.HasActor
        Get
            Return Actor IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property Sprite As (Glyph As Char, Foreground As Integer, Background As Integer) Implements ILocationModel.Sprite
        Get
            With LocationTypes.Descriptors(location.EntityType)
                Return (.Glyph, .Foreground, .Background)
            End With
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ILocationModel.Name
        Get
            Return LocationTypes.Descriptors(location.EntityType).Name
        End Get
    End Property
End Class
