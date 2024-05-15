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

    Public ReadOnly Property Glyph As Char Implements IActorModel.Glyph
        Get
            Return CostumeTypes.Descriptors(actor.Costume).Glyphs(actor.Facing)
        End Get
    End Property

    Public ReadOnly Property Hue As Integer Implements IActorModel.Hue
        Get
            Return CostumeTypes.Descriptors(actor.Costume).Hue
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IActorModel.Name
        Get
            Return actor.Name
        End Get
    End Property

    Public ReadOnly Property Faction As IFactionModel Implements IActorModel.Faction
        Get
            Return FactionModel.FromFaction(actor.Faction)
        End Get
    End Property

    Friend Shared Function GetActor(model As IActorModel) As IActor
        Return CType(model, ActorModel).actor
    End Function
End Class
