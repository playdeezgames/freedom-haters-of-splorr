Public Interface IGroupModel
    ReadOnly Property Name As String
    Function RelationNameTo(otherGroup As IGroupModel) As String
    'parents
    ReadOnly Property Parents As IGroupParentsModel
    'children
    ReadOnly Property ChildPlanets As IEnumerable(Of IGroupModel)
    ReadOnly Property ChildSatellites As IEnumerable(Of IGroupModel)
    ReadOnly Property ChildPlanetFactions As IEnumerable(Of IGroupModel)
    'properties
    ReadOnly Property Properties As IGroupPropertiesModel
End Interface
