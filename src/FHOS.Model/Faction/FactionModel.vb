Imports FHOS.Persistence

Friend Class FactionModel
    Implements IFactionModel

    Private faction As Persistence.IFaction

    Protected Sub New(faction As Persistence.IFaction)
        Me.faction = faction
    End Sub

    Friend Shared Function FromFaction(faction As IFaction) As IFactionModel
        If faction Is Nothing Then
            Return Nothing
        End If
        Return New FactionModel(faction)
    End Function

    Public ReadOnly Property Name As String Implements IFactionModel.Name
        Get
            Return faction.Name
        End Get
    End Property

    Private Shared Function ToLevelName(value As Integer) As String
        Select Case value
            Case Is > 90
                Return "Acceptable"
            Case Is > 75
                Return "Tolerable"
            Case Is > 50
                Return "Unacceptable"
            Case Else
                Return "Inexcusable"
        End Select
    End Function

    Public ReadOnly Property Authority As (LevelName As String, Value As Integer) Implements IFactionModel.Authority
        Get
            Return (ToLevelName(faction.Authority), faction.Authority)
        End Get
    End Property

    Public ReadOnly Property Standards As (LevelName As String, Value As Integer) Implements IFactionModel.Standards
        Get
            Return (ToLevelName(faction.Standards), faction.Standards)
        End Get
    End Property

    Public ReadOnly Property Conviction As (LevelName As String, Value As Integer) Implements IFactionModel.Conviction
        Get
            Return (ToLevelName(faction.Conviction), faction.Conviction)
        End Get
    End Property

    Public ReadOnly Property PlanetCount As Integer Implements IFactionModel.PlanetCount
        Get
            Return faction.PlanetCount
        End Get
    End Property

    Public Function RelationNameTo(otherFaction As IFactionModel) As String Implements IFactionModel.RelationNameTo
        Dim deltaAuthority = otherFaction.Authority.Value - Authority.Value
        Dim deltaStandards = otherFaction.Standards.Value - Standards.Value
        Dim deltaConviction = otherFaction.Conviction.Value - Conviction.Value
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
