Public Class ActorData
    Inherits EntityData
    Public Property Tutorials As New Queue(Of String)
    Public Property Places As New ExplorationData
    Public Property Children As New HashSet(Of Integer)
    Public Property Groups As New HashSet(Of Integer)
    Public Property Equipment As New HashSet(Of Integer)
End Class
