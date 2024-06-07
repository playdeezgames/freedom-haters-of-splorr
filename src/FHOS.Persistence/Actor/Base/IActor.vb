Public Interface IActor
    Inherits INamedEntity
    ReadOnly Property Family As IActorFamily
    ReadOnly Property Properties As IActorProperties
    ReadOnly Property Equipment As IActorEquipment
    Property GroupCategory(group As IGroup) As String
    ReadOnly Property GroupsOfCategory(categoryType As String) As IEnumerable(Of IGroup)
    Property YokedActor(yokeType As String) As IActor
    Property YokedStore(yokeType As String) As IStore
    Property Location As ILocation
End Interface
