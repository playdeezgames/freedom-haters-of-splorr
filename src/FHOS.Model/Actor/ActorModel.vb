﻿Friend Class ActorModel
    Implements IActorModel

    Private ReadOnly actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property Glyph As Char Implements IActorModel.Glyph
        Get
            Return ActorTypes.Descriptors(actor.ActorType).Glyphs(actor.Facing)
        End Get
    End Property

    Public ReadOnly Property Hue As Integer Implements IActorModel.Hue
        Get
            Return ActorTypes.Descriptors(actor.ActorType).Hue
        End Get
    End Property
End Class