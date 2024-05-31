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
End Module
