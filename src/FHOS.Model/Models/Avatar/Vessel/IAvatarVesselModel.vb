﻿Public Interface IAvatarVesselModel
    ReadOnly Property AvailableCrew As IEnumerable(Of (Name As String, Actor As IActorModel))
    ReadOnly Property OxygenPercent As Integer
    ReadOnly Property FuelPercent As Integer?
End Interface
