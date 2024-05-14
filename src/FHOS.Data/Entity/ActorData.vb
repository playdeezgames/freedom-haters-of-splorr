Public Class ActorData
    Inherits EntityData
    Public Property Tutorials As New Queue(Of String)
    Public Property Places As New ExplorationData
    Public Property PlanetVicinities As New ExplorationData
    Public Property Crew As New HashSet(Of Integer)
End Class
