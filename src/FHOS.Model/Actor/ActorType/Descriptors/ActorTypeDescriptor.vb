Imports FHOS.Persistence
Imports SPLORR.Game

Friend MustInherit Class ActorTypeDescriptor
    Friend ReadOnly Property ActorType As String
    Friend ReadOnly Property SpawnRolls As IReadOnlyDictionary(Of String, String)
    Friend MustOverride Function CanSpawn(location As ILocation) As Boolean
    Friend ReadOnly Property CostumeGenerator As IReadOnlyDictionary(Of String, Integer)
    Protected MustOverride Sub Initialize(actor As IActor)
    Friend MustOverride Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
    Friend Sub New(
           actorType As String,
           costumeGenerator As IReadOnlyDictionary(Of String, Integer),
           Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing)
        Me.ActorType = actorType
        Me.CostumeGenerator = costumeGenerator
        Me.SpawnRolls = If(spawnRolls, New Dictionary(Of String, String))
    End Sub
    Friend Function CreateActor(location As ILocation, name As String) As IActor
        Dim actor = location.CreateActor(ActorType, name)
        actor.State.Facing = RNG.FromRange(0, Facing.Deltas.Length - 1)
        actor.Properties.CostumeType = RNG.FromGenerator(CostumeGenerator)
        Initialize(actor)
        Return actor
    End Function
End Class
