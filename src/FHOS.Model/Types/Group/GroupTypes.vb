Friend Module GroupTypes
    Friend ReadOnly Faction As String = NameOf(Faction)
    Friend ReadOnly StarSystem As String = NameOf(StarSystem)
    Friend ReadOnly PlanetVicinity As String = NameOf(PlanetVicinity)
    Friend ReadOnly Planet As String = NameOf(Planet)
    Friend ReadOnly Satellite As String = NameOf(Satellite)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, GroupTypeDescriptor) =
        New List(Of GroupTypeDescriptor) From
        {
            New FactionGroupTypeDescriptor(),
            New StarSystemGroupTypeDescriptor(),
            New PlanetVicinityGroupTypeDescriptor(),
            New PlanetGroupTypeDescriptor(),
            New SatelliteGroupTypeDescriptor()
        }.ToDictionary(Function(x) x.GroupType, Function(x) x)
End Module
