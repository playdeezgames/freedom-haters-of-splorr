Imports System.Runtime.CompilerServices
Imports SPLORR.Game

Friend Class TransportToLocationTypeModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor
    Private ReadOnly locationType As String

    Public Sub New(actor As Persistence.IActor, locationType As String)
        Me.actor = actor
        Me.locationType = locationType
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        Dim destinations = actor.YokedActor(YokeTypes.Interactor).YokedActor(YokeTypes.Target).Properties.Interior.Locations.Where(Function(x) x.EntityType = locationType)
        If Not destinations.Any Then
            actor.Universe.Messages.Add("Destination Blocked!", ("The other end is blocked!", Hues.Red))
            actor.YokedActor(YokeTypes.Interactor) = Nothing
            Return
        End If
        Dim destination = RNG.FromEnumerable(destinations)
        SetLocation(actor, destination)
        actor.YokedActor(YokeTypes.Interactor) = Nothing
    End Sub
End Class
