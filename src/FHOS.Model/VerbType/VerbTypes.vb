Imports FHOS.Persistence
Imports SPLORR.Game

Public Module VerbTypes
    Public ReadOnly ApproachPlanetVicinity As String = NameOf(ApproachPlanetVicinity)
    Public ReadOnly ApproachStarVicinity As String = NameOf(ApproachStarVicinity)
    Public ReadOnly DistressSignal As String = NameOf(DistressSignal)
    Public ReadOnly EnterOrbit As String = NameOf(EnterOrbit)
    Public ReadOnly EnterStarSystem As String = NameOf(EnterStarSystem)
    Public ReadOnly EnterWormhole As String = NameOf(EnterWormhole)
    Public ReadOnly KnownPlaces As String = NameOf(KnownPlaces)
    Public ReadOnly MoveRight As String = NameOf(MoveRight)
    Public ReadOnly MoveUp As String = NameOf(MoveUp)
    Public ReadOnly MoveDown As String = NameOf(MoveDown)
    Public ReadOnly MoveLeft As String = NameOf(MoveLeft)
    Public ReadOnly RefillOxygen As String = NameOf(RefillOxygen)
    Public ReadOnly Refuel As String = NameOf(Refuel)
    Public ReadOnly SPLORRPedia As String = NameOf(SPLORRPedia)
    Public ReadOnly Status As String = NameOf(Status)
    Public ReadOnly Crew As String = NameOf(Crew)
    Public ReadOnly Vessel As String = NameOf(Vessel)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, VerbTypeDescriptor) =
        New Dictionary(Of String, VerbTypeDescriptor) From
        {
            {EnterStarSystem, New VerbTypeDescriptor(EnterStarSystem,
                "Enter Star System",
                Function(actor) actor.State.Location.Place?.PlaceType = PlaceTypes.StarSystem,
                Sub(actor)
                    DoTurn(actor)
                    With actor.State.Location.Place
                        SetLocation(actor, RNG.FromEnumerable(.Properties.Map.Locations.Where(Function(x) x.Flags(.Properties.Identifier) AndAlso x.Actor Is Nothing)))
                    End With
                End Sub)},
            {ApproachPlanetVicinity, New VerbTypeDescriptor(ApproachPlanetVicinity, "Approach Planet", Function(actor) actor.State.Location.Place?.PlaceType = PlaceTypes.PlanetVicinity,
                Sub(actor)
                    DoTurn(actor)
                    With actor.State.Location.Place
                        SetLocation(actor, RNG.FromEnumerable(.Properties.Map.Locations.Where(Function(x) x.Flags(.Properties.Identifier) AndAlso x.Actor Is Nothing)))
                    End With
                End Sub)},
            {ApproachStarVicinity, New VerbTypeDescriptor(ApproachStarVicinity, "Approach Star", Function(actor) actor.State.Location.Place?.PlaceType = PlaceTypes.StarVicinity,
                Sub(actor)
                    DoTurn(actor)
                    With actor.State.Location.Place
                        SetLocation(actor, RNG.FromEnumerable(.Properties.Map.Locations.Where(Function(x) x.Flags(.Properties.Identifier) AndAlso x.Actor Is Nothing)))
                    End With

                End Sub)},
            {RefillOxygen, New VerbTypeDescriptor(RefillOxygen, "Refill Oxygen", Function(actor)
                                                                                     Dim placeType = actor.State.Location.Place?.PlaceType
                                                                                     If placeType <> PlaceTypes.Planet Then
                                                                                         Return False
                                                                                     End If
                                                                                     Return PlanetTypes.Descriptors(actor.State.Location.Place.Subtype).CanRefillOxygen
                                                                                 End Function,
                Sub(actor)
                    actor.State.LifeSupport.CurrentValue = actor.State.LifeSupport.MaximumValue.Value

                End Sub)},
            {Refuel, New VerbTypeDescriptor(Refuel, "Refuel", Function(actor)
                                                                  Return actor.State.Location.Place?.PlaceType = PlaceTypes.Star
                                                              End Function,
                Sub(actor)
                    actor.State.FuelTank.CurrentValue = actor.State.FuelTank.MaximumValue.Value

                End Sub)},
            {EnterWormhole, New VerbTypeDescriptor(EnterWormhole, "Enter Wormhole", Function(actor)
                                                                                        Return actor.State.Location.Place?.PlaceType = PlaceTypes.Wormhole
                                                                                    End Function,
                Sub(actor)
                    DoTurn(actor)
                    With actor.State.Location.Place
                        SetLocation(actor, .Properties.WormholeDestination)
                    End With
                End Sub)},
            {EnterOrbit, New VerbTypeDescriptor(EnterOrbit, "Enter Orbit", Function(actor)
                                                                               Return actor.State.Location.Place?.PlaceType = PlaceTypes.Planet OrElse actor.State.Location.Place?.PlaceType = PlaceTypes.Satellite
                                                                           End Function,
                Sub(actor)
                    DoTurn(actor)
                    With actor.State.Location.Place
                        SetLocation(actor, RNG.FromEnumerable(.Properties.Map.Locations.Where(Function(x) x.Flags(.Properties.Identifier) AndAlso x.Actor Is Nothing)))
                    End With
                End Sub)},
            {KnownPlaces, New VerbTypeDescriptor(KnownPlaces, "Known Places...", Function(actor)
                                                                                     Return actor.KnownPlaces.HasAny
                                                                                 End Function,
                Sub(actor)
                    DoTurn(actor)
                    With actor.State.Location.Place
                        SetLocation(actor, RNG.FromEnumerable(.Properties.Map.Locations.Where(Function(x) x.Flags(.Properties.Identifier) AndAlso x.Actor Is Nothing)))
                    End With

                End Sub)},
            {DistressSignal, New VerbTypeDescriptor(DistressSignal, "Signal Distress", Function(actor)
                                                                                           Return actor.State.FuelTank IsNot Nothing AndAlso
    AvatarModel.FromActor(actor).Vessel.FuelPercent.Value = 0
                                                                                       End Function,
                Sub(actor)
                    Dim fuelAdded = actor.State.FuelTank.MaximumValue.Value - actor.State.FuelTank.CurrentValue
                    Dim fuelPrice = 1 'TODO: don't just pick a magic number!
                    Dim price = fuelPrice * fuelAdded
                    actor.State.FuelTank.CurrentValue = actor.State.FuelTank.MaximumValue.Value
                    actor.State.Wallet.CurrentValue -= fuelAdded * fuelPrice
                    actor.Universe.Messages.Add(
            "Emergency Refuel",
            ($"Added {fuelAdded} fuel!", Hues.Black),
            ($"Price {price} jools!", Hues.Black))

                End Sub)},
            {Status, New VerbTypeDescriptor(Status, "Status", Function(actor) True,
                Sub(actor)
                    Return
                End Sub)},
            {MoveUp, New VerbTypeDescriptor(MoveUp, "Move Up", Function(actor)
                                                                   Return CanMove(actor)
                                                               End Function,
                Sub(actor)
                    Move(actor, Facing.Up)
                End Sub, visible:=False)},
            {MoveRight, New VerbTypeDescriptor(MoveRight, "Move Right", Function(actor)
                                                                            Return CanMove(actor)
                                                                        End Function,
                Sub(actor)
                    Move(actor, Facing.Right)

                End Sub, visible:=False)},
            {MoveDown, New VerbTypeDescriptor(MoveDown, "Move Down", Function(actor)
                                                                         Return CanMove(actor)
                                                                     End Function,
                Sub(actor)
                    Move(actor, Facing.Down)

                End Sub, visible:=False)},
            {MoveLeft, New VerbTypeDescriptor(MoveLeft, "Move Left", Function(actor)
                                                                         Return CanMove(actor)
                                                                     End Function,
                Sub(actor)
                    Move(actor, Facing.Left)

                End Sub, visible:=False)},
            {SPLORRPedia, New VerbTypeDescriptor(SPLORRPedia, "SPLORR!!Pedia", Function(actor)
                                                                                   Return True
                                                                               End Function,
                Sub(actor)
                    Return
                End Sub)},
            {Crew, New VerbTypeDescriptor(Crew, "Crew...", Function(actor)
                                                               Return actor.Family.HasChildren
                                                           End Function,
                Sub(actor)
                    Return
                End Sub)},
            {Vessel, New VerbTypeDescriptor(Vessel, "Vessel...", Function(actor)
                                                                     Return actor.Family.Parent IsNot Nothing
                                                                 End Function,
                Sub(actor)
                    Return
                End Sub)}
        }
End Module
