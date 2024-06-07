Public Class ActorData
    Inherits EntityData
    Public Property Tutorials As New Queue(Of String)
    Public Property Children As New HashSet(Of Integer)
    Public Property Equipment As New HashSet(Of Integer)
    Public Property GroupCategories As New Dictionary(Of Integer, String)
End Class
