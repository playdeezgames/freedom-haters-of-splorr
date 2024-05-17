Imports FHOS.Data

Friend Class Store
    Inherits StoreDataClient
    Implements IStore

    Protected Sub New(universeData As UniverseData, storeId As Integer)
        MyBase.New(universeData, storeId)
    End Sub

    Public Property CurrentValue As Integer Implements IStore.CurrentValue
        Get
            Return GetStatistic(StatisticTypes.CurrentValue).Value
        End Get
        Set(value As Integer)
            SetStatistic(StatisticTypes.CurrentValue, value)
        End Set
    End Property

    Public ReadOnly Property MaximumValue As Integer? Implements IStore.MaximumValue
        Get
            Return GetStatistic(StatisticTypes.MaximumValue)
        End Get
    End Property

    Public ReadOnly Property MinimumValue As Integer? Implements IStore.MinimumValue
        Get
            Return GetStatistic(StatisticTypes.MinimumValue)
        End Get
    End Property

    Public ReadOnly Property Percent As Integer? Implements IStore.Percent
        Get
            Dim minimum = MinimumValue
            Dim maximum = MaximumValue
            If minimum.HasValue AndAlso maximum.HasValue Then
                Return (CurrentValue - minimum.Value) * 100 \ (maximum.Value - minimum.Value)
            End If
            Return Nothing
        End Get
    End Property

    Friend Shared Function FromId(universeData As UniverseData, storeId As Integer?) As IStore
        If storeId.HasValue Then
            Return New Store(universeData, storeId.Value)
        End If
        Return Nothing
    End Function
End Class
