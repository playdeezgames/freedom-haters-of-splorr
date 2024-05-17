Friend Module MapTypes
    Friend ReadOnly Galaxy As String = NameOf(Galaxy)
    Friend ReadOnly StarSystem As String = NameOf(StarSystem)
    Friend ReadOnly StarVicinity As String = NameOf(StarVicinity)
    Friend ReadOnly PlanetVicinity As String = NameOf(PlanetVicinity)
    Friend ReadOnly PlanetOrbit As String = NameOf(PlanetOrbit)
    Friend ReadOnly SatelliteOrbit As String = NameOf(SatelliteOrbit)
    Friend ReadOnly Vessel As String = NameOf(Vessel)
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, MapTypeDescriptor) =
        New List(Of MapTypeDescriptor) From
        {
            New MapTypeDescriptor(Galaxy, (63, 63), LocationTypes.Void),
            New MapTypeDescriptor(StarSystem, (31, 31), LocationTypes.Void),
            New MapTypeDescriptor(StarVicinity, (15, 15), LocationTypes.Void),
            New MapTypeDescriptor(PlanetVicinity, (15, 15), LocationTypes.Void),
            New MapTypeDescriptor(PlanetOrbit, (11, 11), LocationTypes.Void),
            New MapTypeDescriptor(SatelliteOrbit, (9, 9), LocationTypes.Void),
            New MapTypeDescriptor(Vessel, (5, 5), LocationTypes.Air)
        }.ToDictionary(Function(x) x.MapType, Function(x) x)
End Module
