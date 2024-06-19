Public Interface IGroupedEntityData
    Inherits IEntityData
    Sub SetYokedGroup(yokeType As String, groupId As Integer?)
    Function GetYokedGroup(yokeType As String) As Integer?
End Interface
