﻿Imports FHOS.Persistence

Friend Class PlanetVicinityActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(planetType As String)
        MyBase.New(ActorTypes.MakePlanetVicinity(planetType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakePlanetVicinity(planetType), 1}
            },
            New Dictionary(Of String, String),
            isPlanetVicinity:=True,
            subtype:=planetType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {("It's a planet! It's real big!", Hues.Black)}
    End Function
End Class