Public Interface ILocation
    Inherits ITypedEntity
    ReadOnly Property Map As IMap
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    Property Actor As IActor
    Function CreateActor(actorType As String, name As String) As IActor

    Property TargetLocation As ILocation
    ReadOnly Property HasTargetLocation As Boolean
    Property Tutorial As String
End Interface
