Public Class ActorData
    Inherits EntityData
    Implements IActorData
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics:=statistics)
    End Sub
    Public Property LegacyChildren As New HashSet(Of Integer) Implements IActorData.LegacyChildren
    Public Property LegacyEquipment As New HashSet(Of Integer) Implements IActorData.LegacyEquipment
    Public Property YokedActors As New Dictionary(Of String, Integer) Implements IActorData.YokedActors
    Public Property YokedStores As New Dictionary(Of String, Integer) Implements IActorData.YokedStores
    Public Property YokedGroups As New Dictionary(Of String, Integer) Implements IActorData.YokedGroups
    Public Property LegacyInventory As New HashSet(Of Integer) Implements IActorData.LegacyInventory

    Public ReadOnly Property HasChildren As Boolean Implements IActorData.HasChildren
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property Children As IEnumerable(Of Integer) Implements IActorData.Children
        Get
            Return Array.Empty(Of Integer)
        End Get
    End Property
End Class
