Friend Class ItemProperties
    Inherits ItemDataClient
    Implements IItemProperties

    Public Sub New(universeData As Data.UniverseData, entityId As Integer)
        MyBase.New(universeData, entityId)
    End Sub

    Public Property CanRefuel As Boolean Implements IItemProperties.CanRefuel
        Get
            Return Flags(FlagTypes.CanRefuel)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.CanRefuel) = value
        End Set
    End Property

    Public Property CanRefillOxygen As Boolean Implements IItemProperties.CanRefillOxygen
        Get
            Return Flags(FlagTypes.CanRefillOxygen)
        End Get
        Set(value As Boolean)
            Flags(FlagTypes.CanRefillOxygen) = value
        End Set
    End Property

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IItemProperties
        Return New ItemProperties(universeData, id)
    End Function
End Class
