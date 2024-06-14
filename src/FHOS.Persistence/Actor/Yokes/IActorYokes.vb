Public Interface IActorYokes
    Property YokedActor(yokeType As String) As IActor
    Property YokedGroup(yokeType As String) As IGroup
    Property YokedStore(yokeType As String) As IStore
End Interface
