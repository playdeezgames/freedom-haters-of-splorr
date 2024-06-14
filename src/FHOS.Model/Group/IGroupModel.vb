Public Interface IGroupModel
    ReadOnly Property Name As String
    Function RelationNameTo(otherGroup As IGroupModel) As String
    ReadOnly Property Parents As IGroupParentsModel
    ReadOnly Property Properties As IGroupPropertiesModel
    'children
    ReadOnly Property Children As IGroupChildrenModel
    ReadOnly Property ChildPlanets As IEnumerable(Of IGroupModel)
    ReadOnly Property ChildSatellites As IEnumerable(Of IGroupModel)
    ReadOnly Property ChildPlanetFactions As IEnumerable(Of IGroupModel)
End Interface
