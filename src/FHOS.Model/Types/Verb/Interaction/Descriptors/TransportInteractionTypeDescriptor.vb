Imports FHOS.Persistence
Imports SPLORR.Game

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
            Return New InteractionModel(actor, Sub(a)
                                                   Dim destinations = a.Interactor.Yokes.Actor(YokeTypes.Target).Interior.Locations.Where(Function(x) x.EntityType = LocationTypes.PlanetAdjacent)
                                                   If Not destinations.Any Then
                                                       a.Universe.Messages.Add("Destination Blocked!", ("The other end is blocked!", Hues.Red))
                                                       a.ClearInteractor()
                                                       Return
                                                   End If
                                                   Dim destination = RNG.FromEnumerable(destinations)
                                                   SetLocation(a, destination)
                                                   a.ClearInteractor()
                                               End Sub)
        End If
        Return New InteractionModel(actor, Sub(a)
                                               a.GoToOtherActor(
                                                    a.Interactor.Yokes.Actor(YokeTypes.Target),
                                                    Sub(success, otherActor)
                                                        If Not success Then
                                                            a.Universe.Messages.Add("Destination Blocked!", ("The other end is blocked!", Hues.Red))
                                                        Else
                                                            If otherActor.Descriptor.IsStarSystem Then
                                                                a.SetStarSystem(Nothing)
                                                            ElseIf otherActor.Descriptor.IsWormhole Then
                                                                a.SetStarSystem(otherActor.Yokes.Group(YokeTypes.StarSystem))
                                                            End If
                                                        End If
                                                        a.ClearInteractor()
                                                    End Sub)
                                           End Sub)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return prompt
    End Function
End Class
