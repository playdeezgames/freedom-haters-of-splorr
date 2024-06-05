Public Interface IActorProperties
    Property Interior As IMap
    Function GetGroup(groupType As String) As IGroup
    Sub SetGroup(groupType As String, group As IGroup)
    Property CostumeType As String
    Property TargetActor As IActor
End Interface
