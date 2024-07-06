Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class EnterInteriorInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Private ReadOnly prompt As String
    Private ReadOnly check As Func(Of IActor, Boolean)

    Public Sub New(interactionType As String, prompt As String, check As Func(Of IActor, Boolean))
        MyBase.New(interactionType)
        Me.prompt = prompt
        Me.check = check
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return prompt
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return check(actor.Interactor)
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               DoTurn(a)
                                               With a.Interactor.Interior
                                                   Dim starSystemGroup = .YokedGroup(YokeTypes.StarSystem)
                                                   If starSystemGroup IsNot Nothing Then
                                                       a.SetStarSystem(starSystemGroup)
                                                   End If
                                                   SetLocation(a, RNG.FromEnumerable(.Locations.Where(Function(x) x.Flags(FlagTypes.IsEdge) AndAlso x.Actor Is Nothing)))
                                               End With
                                               a.ClearInteractor()
                                           End Sub)
    End Function
End Class
