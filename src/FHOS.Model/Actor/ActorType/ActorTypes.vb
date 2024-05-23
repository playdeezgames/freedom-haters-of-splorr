Friend Module ActorTypes
    Friend ReadOnly Person As String = NameOf(Person)
    Friend ReadOnly PlayerShip As String = NameOf(PlayerShip)
    Friend ReadOnly MilitaryShip As String = NameOf(MilitaryShip)
    Friend ReadOnly TradingPost As String = NameOf(TradingPost)
    Friend ReadOnly StarDock As String = NameOf(StarDock)
    Friend ReadOnly Shipyard As String = NameOf(Shipyard)
    Friend ReadOnly StarGate As String = NameOf(StarGate)
    Friend ReadOnly Debris As String = NameOf(Debris)
    Friend ReadOnly Wormhole As String = NameOf(Wormhole)

    Friend Function MakeArrow(directionName As String) As String
        Return $"Arrow{directionName}"
    End Function

    Friend Function MakeSatellite(satelliteType As String) As String
        Return $"{satelliteType}Moon"
    End Function

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, ActorTypeDescriptor) =
        CreateDescriptors()

    Private Function CreateDescriptors() As IReadOnlyDictionary(Of String, ActorTypeDescriptor)
        Dim descriptorList = New List(Of ActorTypeDescriptor) From
        {
            New PlayerShipActorTypeDescriptor(),
            New MilitaryVesselActorTypeDescriptor(),
            New PersonActorTypeDescriptor(),
            New TradingPostActorTypeDescriptor(),
            New StarDockActorTypeDescriptor(),
            New ShipyardActorTypeDescriptor(),
            New StarGateActorTypeDescriptor(),
            New DebrisActorTypeDescriptor(),
            New WormholeActorTypeDescriptor()
        }
        For Each satelliteType In SatelliteTypes.Descriptors
            descriptorList.Add(New SatelliteActorTypeDescriptor(satelliteType.Key))
        Next
        For Each starType In StarTypes.Descriptors
            descriptorList.Add(New StarVicinityActorTypeDescriptor(starType.Key))
            descriptorList.Add(New StarSystemActorTypeDescriptor(starType.Key))
            descriptorList.Add(New StarActorTypeDescriptor(starType.Key))
        Next
        For Each planetType In PlanetTypes.Descriptors
            descriptorList.Add(New PlanetVicinityActorTypeDescriptor(planetType.Key))
            For Each section In Grid3x3.Descriptors
                descriptorList.Add(New PlanetSectionActorTypeDescriptor(planetType.Key, section.Key))
            Next
            For Each section In Grid5x5.Descriptors
                descriptorList.Add(New PlanetSectionActorTypeDescriptor(planetType.Key, section.Key))
            Next
        Next
        For Each ordinalDirection In OrdinalDirections.Descriptors
            descriptorList.Add(New ArrowActorDescriptor(ordinalDirection.Key))
        Next
        Return descriptorList.
            ToDictionary(Function(x) x.ActorType, Function(x) x)
    End Function

    Friend Function MakePlanetSection(planetType As String, sectionName As String) As String
        Return $"{planetType}Planet{sectionName}"
    End Function

    Friend Function MakePlanetVicinity(planetType As String) As String
        Return $"{planetType}PlanetVicinity"
    End Function

    Friend Function MakeStarVicinity(starType As String) As String
        Return $"{starType}StarVicinity"
    End Function

    Friend Function MakeStarSystem(starType As String) As String
        Return $"{starType}StarSystem"
    End Function

    Friend Function MakeStar(starType As String) As String
        Return $"{starType}Star"
    End Function
End Module
