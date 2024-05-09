Friend Class FactionModel
    Implements IFactionModel

    Private faction As Persistence.IFaction

    Public Sub New(faction As Persistence.IFaction)
        Me.faction = faction
    End Sub

    Public ReadOnly Property Name As String Implements IFactionModel.Name
        Get
            Return faction.Name
        End Get
    End Property
End Class
