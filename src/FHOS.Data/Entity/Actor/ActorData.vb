Public Class ActorData
    Inherits EntityData
    Implements IActorData
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics:=statistics)
    End Sub
    Public Property Children As New HashSet(Of Integer) Implements IActorData.Children
    Public Property Equipment As New HashSet(Of Integer) Implements IActorData.Equipment
    Public Property YokedActors As New Dictionary(Of String, Integer) Implements IActorData.YokedActors
    Public Property YokedStores As New Dictionary(Of String, Integer) Implements IActorData.YokedStores
    Public Property YokedGroups As New Dictionary(Of String, Integer) Implements IActorData.YokedGroups
    Public Property Inventory As New HashSet(Of Integer) Implements IActorData.Inventory
End Class
