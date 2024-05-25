Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module MapExtensions
    <Extension>
    Function GetCenterLocation(map As IMap) As ILocation
        Return map.GetLocation(map.Size.Columns \ 2, map.Size.Rows \ 2)
    End Function
End Module
