Imports System.Runtime.CompilerServices
Imports FHOS.Persistence
Imports SPLORR.Game

Friend Module ActorExtensions
    <Extension>
    Sub ClearInteractor(actor As IActor)
        actor.YokedActor(YokeTypes.Interactor) = Nothing
    End Sub
    <Extension>
    Function Interactor(actor As IActor) As IActor
        Return actor.YokedActor(YokeTypes.Interactor)
    End Function
    <Extension>
    Function CostumeDescriptor(actor As IActor) As CostumeTypeDescriptor
        Return CostumeTypes.Descriptors(actor.CostumeType)
    End Function
    <Extension>
    Function Descriptor(actor As IActor) As ActorTypeDescriptor
        Return ActorTypes.Descriptors(actor.EntityType)
    End Function

    <Extension>
    Function NeedsOxygen(actor As IActor) As Boolean
        Return actor.YokedStore(YokeTypes.LifeSupport).NeedsTopOff
    End Function

    <Extension>
    Sub Move(actor As IActor, facing As Integer)
        actor.Statistics(StatisticTypes.Facing) = facing
        Dim delta = Facings.Deltas(facing)
        If Not CanMove(actor) Then
            Return
        End If
        DoTurn(actor)
        DoFuelConsumption(actor)
        Dim location = actor.Location
        Dim nextColumn = location.Position.Column + delta.X
        Dim nextRow = location.Position.Row + delta.Y
        Dim map = location.Map
        Dim nextLocation = map.GetLocation(nextColumn, nextRow)
        If nextLocation Is Nothing Then
            Return
        End If
        If nextLocation.Actor IsNot Nothing Then
            actor.YokedActor(YokeTypes.Interactor) = nextLocation.Actor
            Return
        End If
        If Not CanEnterLocation(nextLocation, actor) Then
            Return
        End If
        SetLocation(actor, nextLocation)
    End Sub

    <Extension>
    Function CanEnterLocation(nextLocation As ILocation, actor As IActor) As Boolean
        Dim descriptor = LocationTypes.Descriptors(nextLocation.EntityType)
        Return descriptor.IsEnterable AndAlso actor IsNot Nothing
    End Function

    <Extension>
    Sub DoFuelConsumption(actor As IActor)
        If actor.YokedStore(YokeTypes.FuelTank) IsNot Nothing Then
            actor.YokedStore(YokeTypes.FuelTank).CurrentValue -= 1
            If actor.YokedStore(YokeTypes.FuelTank).CurrentValue = actor.YokedStore(YokeTypes.FuelTank).MinimumValue.Value Then
                actor.Universe.Messages.Add("Out of Fuel!",
                    {
                        ("You are out of fuel!", Hues.Black),
                        ("", Hues.Black),
                        ("To send a distress signal,", Hues.Black),
                        ("press [A] from the NAV SCREEN", Hues.Black),
                        ("then choose 'Distress Signal'", Hues.Black)
                    })
            End If
        End If
    End Sub

    <Extension>
    Function CanMove(actor As IActor) As Boolean
        Return actor.YokedStore(YokeTypes.FuelTank) Is Nothing OrElse
                AvatarModel.FromActor(actor).Vessel.FuelPercent.Value > 0
    End Function

    <Extension>
    Sub SetLocation(actor As IActor, location As ILocation)
        actor.Location = location
    End Sub

    <Extension>
    Sub GoToOtherActor(actor As IActor, otherActor As IActor, postAction As Action(Of Boolean, IActor))
        Dim destinations = otherActor.Location.GetEmptyNeighbors()
        If Not destinations.Any Then
            postAction(False, otherActor)
            Return
        End If
        Dim destination = RNG.FromEnumerable(destinations)
        SetLocation(actor, destination)
        postAction(True, otherActor)
    End Sub

    <Extension>
    Sub DoTurn(actor As IActor)
        actor.Universe.Turn += 1
        actor.YokedStore(YokeTypes.LifeSupport).CurrentValue -= 1
    End Sub
    <Extension>
    Sub SetStarSystem(actor As IActor, starSystemGroup As IGroup)
        Dim oldStarSystem = actor.YokedGroup(YokeTypes.StarSystem)
        If starSystemGroup IsNot Nothing AndAlso (oldStarSystem Is Nothing OrElse oldStarSystem.Id <> starSystemGroup.Id) Then
            starSystemGroup.Statistics(StatisticTypes.VisitCount) += 1
        End If
        actor.YokedGroup(YokeTypes.StarSystem) = starSystemGroup
    End Sub
End Module
