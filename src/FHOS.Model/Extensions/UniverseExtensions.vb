Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module UniverseExtensions
    <Extension>
    Friend Function GetFactions(universe As IUniverse) As IEnumerable(Of IGroup)
        Return universe.Groups.Where(Function(x) x.EntityType = GroupTypes.Faction)
    End Function
End Module
