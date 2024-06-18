Public Interface IActorData
    Inherits IEntityData
    Sub AddChild(childId As Integer)
    ReadOnly Property HasChildren As Boolean
    ReadOnly Property AllChildren As IEnumerable(Of Integer)
    ReadOnly Property AllEquipment As IEnumerable(Of Integer)
    Sub AddEquipment(itemId As Integer)
    ReadOnly Property AllItems As IEnumerable(Of Integer)
    Sub AddItem(itemId As Integer)
    Sub RemoveItem(itemId As Integer)
    Sub SetYokedActor(yokeType As String, actorId As Integer?)
    Function GetYokedActor(yokeType As String) As Integer?
    Sub SetYokedStore(yokeType As String, storeId As Integer?)
    Function GetYokedStore(yokeType As String) As Integer?
    Property YokedGroups As Dictionary(Of String, Integer)
    Sub SetYokedGroup(yokeType As String, groupId As Integer?)
    Function GetYokedGroup(yokeType As String) As Integer?
End Interface
