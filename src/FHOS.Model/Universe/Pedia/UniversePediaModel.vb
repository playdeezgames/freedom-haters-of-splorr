Imports FHOS.Persistence

Friend Class UniversePediaModel
    Implements IUniversePediaModel

    Private ReadOnly universe As IUniverse

    Public Sub New(universe As IUniverse)
        Me.universe = universe
    End Sub

    Friend Shared Function FromUniverse(universe As IUniverse) As IUniversePediaModel
        Return New UniversePediaModel(universe)
    End Function

    Public ReadOnly Property FactionList As IEnumerable(Of (Text As String, Faction As IGroupModel)) Implements IUniversePediaModel.FactionList
        Get
            Return universe.Groups.Select(Function(x) (x.Name, GroupModel.FromGroup(x)))
        End Get
    End Property
End Class
