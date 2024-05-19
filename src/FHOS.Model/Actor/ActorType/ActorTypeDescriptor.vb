Imports FHOS.Persistence
Imports SPLORR.Game

Friend MustInherit Class ActorTypeDescriptor
    Friend ReadOnly Property ActorType As String
    Friend ReadOnly Property SpawnRolls As IReadOnlyDictionary(Of String, String)
    Friend MustOverride Function CanSpawn(location As ILocation) As Boolean
    Friend ReadOnly Property CostumeGenerator As IReadOnlyDictionary(Of String, Integer)
    Protected MustOverride Sub Initialize(actor As IActor)

    Friend ReadOnly Property DescribeInteractor As Func(Of IActor, IEnumerable(Of (Text As String, Hue As Integer)))
    Friend Sub New(
           actorType As String,
           costumeGenerator As IReadOnlyDictionary(Of String, Integer),
           Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing,
           Optional describeInteractor As Func(Of IActor, IEnumerable(Of (Text As String, Hue As Integer))) = Nothing)
        Me.ActorType = actorType
        Me.CostumeGenerator = costumeGenerator
        Me.SpawnRolls = If(spawnRolls, New Dictionary(Of String, String))
        Me.DescribeInteractor = If(describeInteractor, Function(x) Array.Empty(Of (Text As String, Hue As Integer)))
    End Sub
    Friend Function CreateActor(location As ILocation) As IActor
        Dim actor = location.CreateActor(ActorType)
        actor.State.Facing = RNG.FromRange(0, Facing.Deltas.Length - 1)
        actor.Properties.CostumeType = RNG.FromGenerator(CostumeGenerator)
        actor.Properties.Name = ActorType
        Initialize(actor)
        Return actor
    End Function
End Class
