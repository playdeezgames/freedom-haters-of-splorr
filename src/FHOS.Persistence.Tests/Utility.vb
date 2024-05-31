Friend Module Utility
    Friend Function CreateAvatar(Optional universe As IUniverse = Nothing, Optional data As UniverseData = Nothing) As IAvatar
        Return If(universe, CreateUniverse(data)).Avatar
    End Function
    Friend Function CreateMessages(Optional universe As IUniverse = Nothing, Optional data As UniverseData = Nothing) As IMessages
        Return If(universe, CreateUniverse(data)).Messages
    End Function
    Friend Function CreateUniverseFactory(Optional universe As IUniverse = Nothing, Optional data As UniverseData = Nothing) As IUniverseFactory
        Return If(universe, CreateUniverse(data)).Factory
    End Function
    Friend Function CreateUniverse(Optional data As UniverseData = Nothing) As IUniverse
        Return New Universe(If(data, New UniverseData))
    End Function
End Module
