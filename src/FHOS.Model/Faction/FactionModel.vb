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

    Public ReadOnly Property Authority As Integer Implements IFactionModel.Authority
        Get
            Return faction.Authority
        End Get
    End Property

    Public ReadOnly Property Standards As Integer Implements IFactionModel.Standards
        Get
            Return faction.Standards
        End Get
    End Property

    Public ReadOnly Property Conviction As Integer Implements IFactionModel.Conviction
        Get
            Return faction.Conviction
        End Get
    End Property
End Class
