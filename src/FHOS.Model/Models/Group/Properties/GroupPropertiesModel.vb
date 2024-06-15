Imports FHOS.Persistence

Friend Class GroupPropertiesModel
    Implements IGroupPropertiesModel
    Private Const AcceptableText As String = "Acceptable"
    Private Const TolerableText As String = "Tolerable"
    Private Const UnacceptableText As String = "Unacceptable"
    Private Const AcceptableThreshold = 90
    Private Const TolerableThreshold = 75
    Private Const UnacceptableThreshold = 50
    Private Const InexcusableText As String = "Inexcusable"
    Private ReadOnly group As IGroup
    Friend Sub New(group As IGroup)
        Me.group = group
    End Sub

    Public ReadOnly Property StarTypeName As String Implements IGroupPropertiesModel.StarTypeName
        Get
            Dim subType = group.Metadatas(MetadataTypes.Subtype)
            If subType IsNot Nothing Then
                Return StarTypes.Descriptors(subType).StarTypeName
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property Position As (Column As Integer, Row As Integer) Implements IGroupPropertiesModel.Position
        Get
            Return (group.Statistics(StatisticTypes.Column).Value, group.Statistics(StatisticTypes.Row).Value)
        End Get
    End Property

    Public ReadOnly Property PlanetCount As Integer Implements IGroupPropertiesModel.PlanetCount
        Get
            Return group.Statistics(StatisticTypes.PlanetCount).Value
        End Get
    End Property

    Public ReadOnly Property SatelliteCount As Integer Implements IGroupPropertiesModel.SatelliteCount
        Get
            Return group.Statistics(StatisticTypes.SatelliteCount).Value
        End Get
    End Property

    Private Shared Function ToLevelName(value As Integer) As String
        Select Case value
            Case Is > AcceptableThreshold
                Return AcceptableText
            Case Is > TolerableThreshold
                Return TolerableText
            Case Is > UnacceptableThreshold
                Return UnacceptableText
            Case Else
                Return InexcusableText
        End Select
    End Function

    Private Function ToLevelAndValue(statisticType As String) As (LevelName As String, Value As Integer)
        Dim statisticValue = group.Statistics(statisticType).Value
        Return (ToLevelName(statisticValue), statisticValue)
    End Function

    Public ReadOnly Property Authority As (LevelName As String, Value As Integer) Implements IGroupPropertiesModel.Authority
        Get
            Return ToLevelAndValue(StatisticTypes.Authority)
        End Get
    End Property

    Public ReadOnly Property Standards As (LevelName As String, Value As Integer) Implements IGroupPropertiesModel.Standards
        Get
            Return ToLevelAndValue(StatisticTypes.Standards)
        End Get
    End Property

    Public ReadOnly Property Conviction As (LevelName As String, Value As Integer) Implements IGroupPropertiesModel.Conviction
        Get
            Return ToLevelAndValue(StatisticTypes.Conviction)
        End Get
    End Property

    Public ReadOnly Property PlanetTypeName As String Implements IGroupPropertiesModel.PlanetTypeName
        Get
            Dim subType = group.Metadatas(MetadataTypes.Subtype)
            If subType IsNot Nothing Then
                Return PlanetTypes.Descriptors(subType).PlanetType
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property SatelliteTypeName As String Implements IGroupPropertiesModel.SatelliteTypeName
        Get
            Dim subType = group.Metadatas(MetadataTypes.Subtype)
            If subType IsNot Nothing Then
                Return SatelliteTypes.Descriptors(subType).SatelliteType
            End If
            Return Nothing
        End Get
    End Property

    Friend Shared Function FromGroup(group As IGroup) As IGroupPropertiesModel
        Return New GroupPropertiesModel(group)
    End Function
End Class
