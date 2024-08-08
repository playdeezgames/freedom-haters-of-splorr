Imports FHOS.Persistence

Friend Class DistressSignalOperationTypeDescriptor
    Inherits OperationTypeDescriptor

    Friend Sub New()
        MyBase.New(DistressSignal, "Signal Distress")
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        actor.Dialog = New EmergencyRefuelDialog(actor, actor.Dialog)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.FuelTank IsNot Nothing AndAlso AvatarModel.FromActor(actor).Vessel.FuelPercent.Value = 0
    End Function
End Class
