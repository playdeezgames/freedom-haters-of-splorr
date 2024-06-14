Imports FHOS.Persistence

Friend Class ActorModel
    Implements IActorModel

    Private ReadOnly actor As Persistence.IActor

    Protected Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IActorModel
        If actor Is Nothing Then
            Return Nothing
        End If
        Return New ActorModel(actor)
    End Function

    Private ReadOnly Property Glyph As Char
        Get
            Return actor.CostumeDescriptor.Glyphs(actor.Statistics(StatisticTypes.Facing).Value)
        End Get
    End Property

    Private ReadOnly Property Hue As Integer
        Get
            Return actor.CostumeDescriptor.Hue
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IActorModel.Name
        Get
            Return actor.EntityName
        End Get
    End Property

    Public ReadOnly Property Sprite As (Glyph As Char, Hue As Integer) Implements IActorModel.Sprite
        Get
            Return (Glyph, Hue)
        End Get
    End Property

    Public ReadOnly Property Subtype As String Implements IActorModel.Subtype
        Get
            If IsStarSystem Then
                Return StarTypes.Descriptors(actor.Descriptor.Subtype).StarTypeName
            End If
            Return actor.Descriptor.Subtype
        End Get
    End Property

    Public ReadOnly Property IsStarSystem As Boolean Implements IActorModel.IsStarSystem
        Get
            Return actor.Descriptor.IsStarSystem
        End Get
    End Property

    Public ReadOnly Property Position As (X As Integer, Y As Integer) Implements IActorModel.Position
        Get
            With actor.Location
                Return (.Position.Column, .Position.Row)
            End With
        End Get
    End Property

    Public ReadOnly Property PlanetCount As Integer Implements IActorModel.PlanetCount
        Get
            Return If(actor.YokedGroup(YokeTypes.Faction)?.Statistics(StatisticTypes.PlanetCount), 0)
        End Get
    End Property

    Public ReadOnly Property IsPlanet As Boolean Implements IActorModel.IsPlanet
        Get
            Return actor.Descriptor.IsPlanet
        End Get
    End Property

    Public ReadOnly Property IsSatellite As Boolean Implements IActorModel.IsSatellite
        Get
            Return actor.Descriptor.IsSatellite
        End Get
    End Property

    Public ReadOnly Property IsPlanetVicinity As Boolean Implements IActorModel.IsPlanetVicinity
        Get
            Return actor.Descriptor.IsPlanetVicinity
        End Get
    End Property

    Public ReadOnly Property StarSystem As IGroupModel Implements IActorModel.StarSystem
        Get
            Return GroupModel.FromGroup(actor.YokedGroup(YokeTypes.Faction)?.Parents?.SingleOrDefault(Function(x) x.EntityType = GroupTypes.StarSystem))
        End Get
    End Property

    Public ReadOnly Property SatelliteCount As Integer Implements IActorModel.SatelliteCount
        Get
            Return If(actor.YokedGroup(YokeTypes.Faction)?.Statistics(StatisticTypes.SatelliteCount), 0)
        End Get
    End Property

    Public ReadOnly Property PlanetVicinity As IGroupModel Implements IActorModel.PlanetVicinity
        Get
            Return GroupModel.FromGroup(actor.YokedGroup(YokeTypes.Faction)?.Parents?.SingleOrDefault(Function(x) x.EntityType = GroupTypes.PlanetVicinity))
        End Get
    End Property

    Public ReadOnly Property Faction As IGroupModel Implements IActorModel.Faction
        Get
            Return GroupModel.FromGroup(actor.YokedGroup(YokeTypes.Faction)?.Parents?.SingleOrDefault(Function(x) x.EntityType = GroupTypes.Faction))
        End Get
    End Property

    Friend Shared Function GetActor(model As IActorModel) As IActor
        Return CType(model, ActorModel).actor
    End Function
End Class
