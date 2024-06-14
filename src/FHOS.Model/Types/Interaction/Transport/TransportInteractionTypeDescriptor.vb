Imports FHOS.Persistence

Friend Class TransportInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Private ReadOnly prompt As String
    Private ReadOnly check As Func(Of IActor, Boolean)

    Public Sub New(interactionType As String, prompt As String, check As Func(Of IActor, Boolean))
        MyBase.New(interactionType)
        Me.prompt = prompt
        Me.check = check
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Yokes.Actor(YokeTypes.Target) IsNot Nothing AndAlso check(actor.Yokes.Actor(YokeTypes.Interactor))
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        If actor.Descriptor.IsPlanet Then
            Return New TransportToLocationTypeModel(actor, LocationTypes.PlanetAdjacent)
        End If
        Return New TransportToActorInteractionModel(actor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return prompt
    End Function
End Class
