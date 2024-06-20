Public MustInherit Class IdentifiedEntityData
    Inherits EntityData
    Implements IIdentifiedEntityData
    Public Sub New(id As Integer, Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing)
        MyBase.New(statistics)
        Me.Id = id
    End Sub

    Public ReadOnly Property Id As Integer Implements IIdentifiedEntityData.Id
End Class
