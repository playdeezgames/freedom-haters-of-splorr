﻿Imports System.Runtime.CompilerServices
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
        Dim faction = universeFactory.CreateGroup(
            GroupTypes.Faction,
            factionName)
        faction.MinimumPlanetCount = minimumPlanetCount
        faction.Authority = authority
        faction.Standards = standards
        faction.Conviction = conviction
        Return faction
    End Function
End Module