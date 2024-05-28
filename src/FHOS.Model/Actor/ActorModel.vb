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
            Return CostumeTypes.Descriptors(actor.Properties.CostumeType).Glyphs(actor.State.Facing)
        End Get
    End Property

    Private ReadOnly Property Hue As Integer
        Get
            Return CostumeTypes.Descriptors(actor.Properties.CostumeType).Hue
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IActorModel.Name
        Get
            Return actor.Properties.Name
        End Get
    End Property

    Public ReadOnly Property Group As IGroupModel Implements IActorModel.Group
        Get
            Return GroupModel.FromGroup(actor.Properties.Group)
        End Get
    End Property

    Public ReadOnly Property Sprite As (Glyph As Char, Hue As Integer) Implements IActorModel.Sprite
        Get
            Return (Glyph, Hue)
        End Get
    End Property

    Public ReadOnly Property Subtype As String Implements IActorModel.Subtype
        Get
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
            With actor.State.Location
                Return (.Column, .Row)
            End With
        End Get
    End Property

    Friend Shared Function GetActor(model As IActorModel) As IActor
        Return CType(model, ActorModel).actor
    End Function
End Class
