Public Class ActorData
    Inherits EntityData
    Public Property Tutorials As New Queue(Of String)
    Public Property Children As New HashSet(Of Integer)
    Public Property Equipment As New HashSet(Of Integer)
    Public Property YokedActors As New Dictionary(Of String, Integer)
    Public Property YokedStores As New Dictionary(Of String, Integer)
    Public Property YokedGroups As New Dictionary(Of String, Integer)
End Class
