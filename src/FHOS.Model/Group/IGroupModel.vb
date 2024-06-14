Public Interface IGroupModel
    ReadOnly Property Name As String
    Function RelationNameTo(otherGroup As IGroupModel) As String
    ReadOnly Property Parents As IGroupParentsModel
    ReadOnly Property Properties As IGroupPropertiesModel
    ReadOnly Property Children As IGroupChildrenModel
End Interface
