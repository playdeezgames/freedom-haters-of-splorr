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

    Public ReadOnly Property PlanetCount As Integer Implements IFactionModel.PlanetCount
        Get
            Return faction.PlanetCount
        End Get
    End Property

    Public Function RelationNameTo(otherFaction As IFactionModel) As String Implements IFactionModel.RelationNameTo
        Dim deltaAuthority = otherFaction.Authority - Authority
        Dim deltaStandards = otherFaction.Standards - Standards
        Dim deltaConviction = otherFaction.Conviction - Conviction
        Select Case CInt(Math.Sqrt(deltaAuthority * deltaAuthority) + (deltaStandards * deltaStandards) + (deltaConviction * deltaConviction))
            Case Is >= 50
                Return "Hostile"
            Case Is >= 25
                Return "Neutral"
            Case Else
                Return "Friendly"
        End Select
    End Function
End Class
