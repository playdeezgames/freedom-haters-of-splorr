Public Interface IAvatarVesselModel
    ReadOnly Property AvailableCrew As IEnumerable(Of (Name As String, Actor As IActorModel))
    ReadOnly Property OxygenPercent As Integer
    ReadOnly Property OxygenQuantity As Integer
    ReadOnly Property OxygenMaximum As Integer
    ReadOnly Property FuelPercent As Integer?
    ReadOnly Property FuelQuantity As Integer?
    ReadOnly Property FuelMaximum As Integer?
End Interface
