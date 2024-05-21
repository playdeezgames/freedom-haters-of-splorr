Imports FHOS.Persistence

Friend Class EnterOrbitInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.EnterOrbit)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Enter Orbit"
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Interactor.Properties.IsSatellite
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New EnterOrbitInteractionModel(actor)
    End Function
End Class
