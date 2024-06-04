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
        If Not EntityData.Parents.Contains(parent.Id) Then
            EntityData.Parents.Add(parent.Id)
            CType(parent, Group).EntityData.Children.Add(Id)
        End If
    End Sub

    Public Sub RemoveParent(parent As IGroup) Implements IGroup.RemoveParent
        If parent IsNot Nothing Then
            If EntityData.Parents.Contains(parent.Id) Then
                EntityData.Parents.Remove(parent.Id)
                CType(parent, Group).EntityData.Children.Remove(Id)
            End If
        End If
    End Sub

    Public ReadOnly Property EntityName As String Implements IGroup.EntityName
        Get
            Return EntityData.Metadatas(LegacyMetadataTypes.Name)
        End Get
    End Property

    Public ReadOnly Property Parents As IEnumerable(Of IGroup) Implements IGroup.Parents
        Get
            Return EntityData.Parents.Select(Function(x) Group.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Children As IEnumerable(Of IGroup) Implements IGroup.Children
        Get
            Return EntityData.Children.Select(Function(x) Group.FromId(UniverseData, x))
        End Get
    End Property
End Class
