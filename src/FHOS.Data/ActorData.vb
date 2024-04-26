Public Class ActorData
    Inherits EntityData
    Public Property Tutorials As New Queue(Of String)
    Public Property StarSystems As New ExplorationData
    Public Property StarVicinities As New ExplorationData
End Class
