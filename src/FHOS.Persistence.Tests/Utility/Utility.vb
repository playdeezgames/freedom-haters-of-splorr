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
    Friend Function CreateGroup(
                               Optional groupType As String = "group type",
                               Optional groupName As String = "group name",
                               Optional universe As IUniverse = Nothing,
                               Optional data As UniverseData = Nothing) As IGroup
        Return CreateUniverseFactory(universe, data).CreateGroup(groupType, groupName)
    End Function
    Friend Function CreateMap(
                             Optional mapName As String = "map name",
                             Optional mapType As String = "map type",
                             Optional mapColumns As Integer = 2,
                             Optional mapRows As Integer = 3,
                             Optional locationType As String = "location type",
                             Optional universe As IUniverse = Nothing,
                             Optional data As UniverseData = Nothing) As IMap
        Return CreateUniverseFactory(universe, data).CreateMap(mapName, mapType, mapColumns, mapRows, locationType)
    End Function
    Friend Function CreateStore(
                               Optional value As Integer = 0,
                               Optional minimum As Integer? = Nothing,
                               Optional maximum As Integer? = Nothing,
                               Optional universe As IUniverse = Nothing,
                               Optional data As UniverseData = Nothing) As IStore
        Return CreateUniverseFactory(universe, data).CreateStore(value, minimum, maximum)
    End Function
    Friend Function CreateItem(
                               Optional itemType As String = Nothing,
                               Optional universe As IUniverse = Nothing,
                               Optional data As UniverseData = Nothing) As IItem
        Return CreateUniverseFactory(universe, data).CreateItem(itemType)
    End Function
    Friend Function CreateLocation(
                              Optional mapName As String = "map name",
                              Optional mapType As String = "map type",
                              Optional mapColumns As Integer = 2,
                              Optional mapRows As Integer = 3,
                              Optional locationType As String = "location type",
                              Optional column As Integer = 1,
                              Optional row As Integer = 0,
                              Optional universe As IUniverse = Nothing,
                              Optional data As UniverseData = Nothing) As ILocation

        Return CreateMap(
            mapName,
            mapType,
            mapColumns,
            mapRows,
            locationType,
            universe,
            data).GetLocation(
                column,
                row)
    End Function
    Friend Function CreateActor(
                              Optional mapName As String = "map name",
                              Optional mapType As String = "map type",
                              Optional mapColumns As Integer = 2,
                              Optional mapRows As Integer = 3,
                              Optional locationType As String = "location type",
                              Optional column As Integer = 1,
                              Optional row As Integer = 0,
                              Optional actorType As String = "actor type",
                              Optional actorName As String = "actor name",
                              Optional universe As IUniverse = Nothing,
                              Optional data As UniverseData = Nothing) As IActor
        Return CreateLocation(
            mapName,
            mapType,
            mapColumns,
            mapRows,
            locationType,
            column,
            row,
            universe,
            data).CreateActor(
                actorType,
                actorName)
    End Function
End Module
