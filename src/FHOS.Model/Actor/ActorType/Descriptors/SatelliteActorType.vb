﻿Imports FHOS.Persistence

Friend Class SatelliteActorType
    Inherits ActorTypeDescriptor


    Public Sub New(satelliteType As String)
        MyBase.New(ActorTypes.MakeSatellite(satelliteType), New Dictionary(Of String, Integer) From {{CostumeTypes.MakeSatellite(satelliteType), 1}}, New Dictionary(Of String, String))
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        actor.Properties.IsSatellite = True
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {("That's no moon. It's a space station!", Hues.Black)}
    End Function
End Class