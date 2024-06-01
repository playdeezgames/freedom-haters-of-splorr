Public Interface IGroup
    Inherits IEntity
    Property Conviction As Integer
    Property PlanetCount As Integer

    ReadOnly Property GroupType As String
    ReadOnly Property Name As String

    ReadOnly Property Parents As IEnumerable(Of IGroup)
    Sub AddParent(parent As IGroup)
    Sub RemoveParent(parent As IGroup)
    ReadOnly Property Children As IEnumerable(Of IGroup)
End Interface
