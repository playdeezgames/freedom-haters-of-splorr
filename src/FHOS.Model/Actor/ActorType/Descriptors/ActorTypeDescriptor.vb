Imports FHOS.Persistence
Imports SPLORR.Game

Friend MustInherit Class ActorTypeDescriptor
    Friend ReadOnly Property ActorType As String
    Friend ReadOnly Property BuysScrap As Boolean
    Friend ReadOnly Property CanRefillOxygen As Boolean
    Friend ReadOnly Property IsStarGate As Boolean
    Friend ReadOnly Property CanSalvage As Boolean
    Friend ReadOnly Property CanRefuel As Boolean
    Friend ReadOnly Property SpawnRolls As IReadOnlyDictionary(Of String, String)
    Friend MustOverride Function CanSpawn(location As ILocation) As Boolean
    Friend ReadOnly Property CostumeGenerator As IReadOnlyDictionary(Of String, Integer)
    Protected MustOverride Sub Initialize(actor As IActor)
    Friend MustOverride Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
    Friend Sub New(
           actorType As String,
           costumeGenerator As IReadOnlyDictionary(Of String, Integer),
           Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing,
           Optional canRefillOxygen As Boolean = False,
           Optional isStarGate As Boolean = False,
           Optional canSalvage As Boolean = False,
           Optional buysScrap As Boolean = False,
           Optional canRefuel As Boolean = False)
        Me.ActorType = actorType
        Me.CostumeGenerator = costumeGenerator
        Me.SpawnRolls = If(spawnRolls, New Dictionary(Of String, String))
        Me.CanRefillOxygen = canRefillOxygen
        Me.IsStarGate = isStarGate
        Me.CanSalvage = canSalvage
        Me.BuysScrap = buysScrap
        Me.CanRefuel = canRefuel
    End Sub
    Friend Function CreateActor(location As ILocation, name As String) As IActor
        Dim actor = location.CreateActor(ActorType, name)
        actor.State.Facing = RNG.FromRange(0, Facing.Deltas.Length - 1)
        actor.Properties.CostumeType = RNG.FromGenerator(CostumeGenerator)
        Initialize(actor)
        Return actor
    End Function
End Class
