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
    Friend Shared Function GetActor(model As IActorModel) As IActor
        Return CType(model, ActorModel).actor
    End Function
End Class
