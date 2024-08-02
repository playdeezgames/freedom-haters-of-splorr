Public Interface IAvatarBioModel
    ReadOnly Property HomePlanet As IGroupModel
    ReadOnly Property Faction As IGroupModel
    Function Reputation(group As IGroupModel) As Integer?
End Interface
