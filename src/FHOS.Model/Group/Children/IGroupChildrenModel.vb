Public Interface IGroupChildrenModel
    ReadOnly Property ChildPlanets As IEnumerable(Of IGroupModel)
    ReadOnly Property ChildSatellites As IEnumerable(Of IGroupModel)
    ReadOnly Property ChildPlanetFactions As IEnumerable(Of IGroupModel)
End Interface
