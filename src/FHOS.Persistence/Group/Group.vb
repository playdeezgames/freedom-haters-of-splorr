Imports FHOS.Data
Imports Microsoft.Data.Sqlite

Friend Class Group
    Inherits GroupDataClient
    Implements IGroup
    Friend Shared Function FromId(universeData As IUniverseData, connection As SqliteConnection, id As Integer?) As IGroup
        Return If(id.HasValue, New Group(universeData, connection, id.Value), Nothing)
    End Function

    Protected Sub New(universeData As Data.IUniverseData, connection As SqliteConnection, factionId As Integer)
        MyBase.New(universeData, connection, factionId)
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

    Public ReadOnly Property Parents As IEnumerable(Of IGroup) Implements IGroup.Parents
        Get
            Return EntityData.AllParents.Select(Function(x) Group.FromId(UniverseData, Connection, x))
        End Get
    End Property

    Public ReadOnly Property Children As IEnumerable(Of IGroup) Implements IGroup.Children
        Get
            Return EntityData.AllChildren.Select(Function(x) Group.FromId(UniverseData, Connection, x))
        End Get
    End Property
End Class
