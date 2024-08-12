Imports FHOS.Data

Friend Class Group
    Inherits GroupDataClient
    Implements IGroup
    Friend Shared Function FromId(universeData As UniverseData, id As Integer?) As IGroup
        Return If(id.HasValue, New Group(universeData, id.Value), Nothing)
    End Function

    Protected Sub New(universeData As Data.UniverseData, factionId As Integer)
        MyBase.New(universeData, factionId)
    End Sub

    Public Sub AddParent(parent As IGroup) Implements IGroup.AddParent
        If Not EntityData.HasParent(parent.Id) Then
            EntityData.AddParent(parent.Id)
            CType(parent, Group).EntityData.AddChild(Id)
        End If
    End Sub

    Public Sub RemoveParent(parent As IGroup) Implements IGroup.RemoveParent
        If parent IsNot Nothing Then
            If EntityData.HasParent(parent.Id) Then
                EntityData.RemoveParent(parent.Id)
                CType(parent, Group).EntityData.RemoveChild(Id)
            End If
        End If
    End Sub

    Public Sub AddValue(valueId As Integer) Implements IGroup.AddValue
        EntityData.AddValue(valueId)
    End Sub

    Public ReadOnly Property Parents As IEnumerable(Of IGroup) Implements IGroup.Parents
        Get
            Return EntityData.AllParents.Select(Function(x) Group.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Children As IEnumerable(Of IGroup) Implements IGroup.Children
        Get
            Return EntityData.AllChildren.Select(Function(x) Group.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Values As IEnumerable(Of Integer) Implements IGroup.Values
        Get
            Return EntityData.Values
        End Get
    End Property
End Class
