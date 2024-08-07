﻿Friend Module MapTypes
    Friend ReadOnly Galaxy As String = NameOf(Galaxy)
    Friend ReadOnly Nexus As String = NameOf(Nexus)
    Friend ReadOnly StarSystem As String = NameOf(StarSystem)
    Friend ReadOnly StarVicinity As String = NameOf(StarVicinity)
    Friend ReadOnly PlanetVicinity As String = NameOf(PlanetVicinity)
    Friend ReadOnly PlanetOrbit As String = NameOf(PlanetOrbit)
    Friend ReadOnly SatelliteOrbit As String = NameOf(SatelliteOrbit)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, MapTypeDescriptor) =
        New List(Of MapTypeDescriptor) From
        {
            New MapTypeDescriptor(Galaxy, "Galaxy", (63, 63), LocationTypes.Void),
            New MapTypeDescriptor(Nexus, "Nexus", (63, 63), LocationTypes.Void),
            New MapTypeDescriptor(StarSystem, "Star System", (31, 31), LocationTypes.Void),
            New MapTypeDescriptor(StarVicinity, "Star Vicinity", (15, 15), LocationTypes.Void),
            New MapTypeDescriptor(PlanetVicinity, "Planet Vicinity", (15, 15), LocationTypes.Void),
            New MapTypeDescriptor(PlanetOrbit, "Planet Orbit", (11, 11), LocationTypes.Void),
            New MapTypeDescriptor(SatelliteOrbit, "Satellite Orbit", (9, 9), LocationTypes.Void)
        }.ToDictionary(Function(x) x.MapType, Function(x) x)
End Module
