Imports System.Runtime.CompilerServices
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module ActorExtensions
    <Extension>
    Function CostumeDescriptor(actorProperties As IActorProperties) As CostumeTypeDescriptor
        Return CostumeTypes.Descriptors(actorProperties.CostumeType)
    End Function
    <Extension>
    Function Descriptor(actor As IActor) As ActorTypeDescriptor
        Return ActorTypes.Descriptors(actor.EntityType)
    End Function

    <Extension>
    Function NeedsOxygen(actor As IActor) As Boolean
        Return actor.State.LifeSupport.NeedsTopOff
    End Function

    <Extension>
    Sub Move(actor As IActor, facing As Integer)
        actor.State.Facing = facing
        Dim delta = Persistence.Facing.Deltas(facing)
        If Not CanMove(actor) Then
            Return
        End If
        DoTurn(actor)
        DoFuelConsumption(actor)
        Dim location = actor.State.Location
        Dim nextColumn = location.Position.Column + delta.X
        Dim nextRow = location.Row + delta.Y
        Dim map = location.Map
        Dim nextLocation = map.GetLocation(nextColumn, nextRow)
        If nextLocation Is Nothing Then
            Return
        End If
        If nextLocation.Actor IsNot Nothing Then
            actor.State.Interactor = nextLocation.Actor
            Return
        End If
        If nextLocation.HasTargetLocation Then
            nextLocation = nextLocation.TargetLocation
        End If
        If Not CanEnterLocation(nextLocation, actor) Then
            Return
        End If
        SetLocation(actor, nextLocation)
    End Sub

    <Extension>
    Function CanEnterLocation(nextLocation As ILocation, actor As IActor) As Boolean
        Dim descriptor = LocationTypes.Descriptors(nextLocation.EntityType)
        Return descriptor.IsEnterable
    End Function

    <Extension>
    Sub DoFuelConsumption(actor As IActor)
        If actor.State.FuelTank IsNot Nothing Then
            actor.State.FuelTank.CurrentValue -= 1
            If actor.State.FuelTank.CurrentValue = actor.State.FuelTank.MinimumValue.Value Then
                actor.Tutorial.Add(TutorialTypes.OutOfFuel)
            End If
        End If
    End Sub

    <Extension>
    Function CanMove(actor As IActor) As Boolean
        Return actor.State.FuelTank Is Nothing OrElse
                AvatarModel.FromActor(actor).Vessel.FuelPercent.Value > 0
    End Function

    <Extension>
    Sub SetLocation(actor As IActor, location As ILocation)
        actor.State.Location = location
    End Sub

    <Extension>
    Sub GoToOtherActor(actor As IActor, otherActor As IActor, postAction As Action(Of Boolean))
        Dim destinations = otherActor.State.Location.GetEmptyNeighbors()
        If Not destinations.Any Then
            postAction(False)
            Return
        End If
        Dim destination = RNG.FromEnumerable(destinations)
        SetLocation(actor, destination)
        postAction(True)
    End Sub

    <Extension>
    Sub DoTurn(actor As IActor)
        actor.Universe.Turn += 1
        actor.State.LifeSupport.CurrentValue -= 1
    End Sub
End Module
