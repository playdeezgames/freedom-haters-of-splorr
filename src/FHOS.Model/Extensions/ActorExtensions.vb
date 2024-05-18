Imports System.Runtime.CompilerServices
Imports FHOS.Persistence

Friend Module ActorExtensions
    <Extension>
    Function NeedsOxygen(actor As IActor) As Boolean
        Return actor.State.LifeSupport.NeedsTopOff
    End Function
End Module
