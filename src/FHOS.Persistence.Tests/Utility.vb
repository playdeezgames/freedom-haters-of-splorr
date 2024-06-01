Friend Module Utility
    Friend Function CreateAvatar(Optional universe As IUniverse = Nothing, Optional data As UniverseData = Nothing) As IAvatar
        Return CreateUniverse(universe, data).Avatar
    End Function
    Friend Function CreateMessages(Optional universe As IUniverse = Nothing, Optional data As UniverseData = Nothing) As IMessages
        Return CreateUniverse(universe, data).Messages
    End Function
    Friend Function CreateUniverseFactory(Optional universe As IUniverse = Nothing, Optional data As UniverseData = Nothing) As IUniverseFactory
        Return CreateUniverse(universe, data).Factory
    End Function
    Friend Function CreateUniverse(Optional universe As IUniverse = Nothing, Optional data As UniverseData = Nothing) As IUniverse
        Dim check = universe Is Nothing OrElse data Is Nothing
        check.ShouldBeTrue("When bootstrapping a universe, either supply a universe already or the data behind it, or neither, but not both!")
        Return If(universe, New Universe(If(data, New UniverseData)))
    End Function
    Friend Function CreateGroup(groupType As String, groupName As String, Optional universe As IUniverse = Nothing, Optional data As UniverseData = Nothing) As IGroup
        Return CreateUniverseFactory(universe, data).CreateGroup(groupType, groupName)
    End Function
    Friend Function CreateMap(
                             mapName As String,
                             mapType As String,
                             mapColumns As Integer,
                             mapRows As Integer,
                             locationType As String,
                             Optional universe As IUniverse = Nothing,
                             Optional data As UniverseData = Nothing) As IMap
        Return CreateUniverseFactory(universe, data).CreateMap(mapName, mapType, mapColumns, mapRows, locationType)
    End Function
    Friend Function CreateStore(
                               value As Integer,
                               minimum As Integer?,
                               maximum As Integer?,
                               Optional universe As IUniverse = Nothing,
                               Optional data As UniverseData = Nothing) As IStore
        Return CreateUniverseFactory(universe, data).CreateStore(value, minimum, maximum)
    End Function
End Module
