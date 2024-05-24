Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module UniverseFactoryExtensions
    <Extension>
    Function CreateFaction(
                        universeFactory As IUniverseFactory,
                        factionName As String,
                        minimumPlanetCount As Integer,
                        authority As Integer,
                        standards As Integer,
                        conviction As Integer) As IGroup
        Return universeFactory.CreateGroup(GroupTypes.Faction, factionName, minimumPlanetCount, authority, standards, conviction)
    End Function
End Module
