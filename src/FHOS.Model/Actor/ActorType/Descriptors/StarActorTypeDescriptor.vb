﻿Imports FHOS.Persistence

Friend Class StarActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Private starType As String

    Public Sub New(starType As String)
        MyBase.New(ActorTypes.MakeStar(starType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeStar(starType), 1}
            },
            New Dictionary(Of String, String))
        Me.starType = starType
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        actor.Properties.Subtype = starType
        actor.Properties.IsStar = True
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {("It's a star! It's real hot!", Hues.Black)}
    End Function
End Class