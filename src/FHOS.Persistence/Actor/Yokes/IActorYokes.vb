Public Interface IActorYokes
    Property Actor(yokeType As String) As IActor
    Property Group(yokeType As String) As IGroup
    Property Store(yokeType As String) As IStore
End Interface
