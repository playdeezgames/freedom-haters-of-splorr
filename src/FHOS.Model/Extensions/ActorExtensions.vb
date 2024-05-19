Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module ActorExtensions
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
        Dim nextColumn = location.Column + delta.X
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
        Dim descriptor = LocationTypes.Descriptors(nextLocation.LocationType)
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
        If location.Place IsNot Nothing Then
            actor.KnownPlaces.Add(location.Place)
        End If
    End Sub

    <Extension>
    Sub DoTurn(actor As IActor)
        actor.Universe.Turn += 1
        actor.State.LifeSupport.CurrentValue -= 1
    End Sub
End Module
