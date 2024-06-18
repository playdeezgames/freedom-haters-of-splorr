Public Class ActorData
    Inherits EntityData
    Implements IActorData
    Public Sub New(Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics:=statistics)
    End Sub
    Public Property Children As New HashSet(Of Integer)
    Public Property Equipment As New HashSet(Of Integer) Implements IActorData.Equipment
    Public Property YokedActors As New Dictionary(Of String, Integer) Implements IActorData.YokedActors
    Public Property YokedStores As New Dictionary(Of String, Integer) Implements IActorData.YokedStores
    Public Property YokedGroups As New Dictionary(Of String, Integer) Implements IActorData.YokedGroups
    Public Property Inventory As New HashSet(Of Integer) Implements IActorData.Inventory

    Public ReadOnly Property HasChildren As Boolean Implements IActorData.HasChildren
        Get
            Return Children.Any
        End Get
    End Property

    Public ReadOnly Property AllChildren As IEnumerable(Of Integer) Implements IActorData.AllChildren
        Get
            Return Children
        End Get
    End Property

    Public ReadOnly Property AllEquipment As IEnumerable(Of Integer) Implements IActorData.AllEquipment
        Get
            Return Equipment
        End Get
    End Property

    Public Sub AddChild(childId As Integer) Implements IActorData.AddChild
        Children.Add(childId)
    End Sub

    Public Sub AddEquipment(itemId As Integer) Implements IActorData.AddEquipment
        Equipment.Add(itemId)
    End Sub
End Class
