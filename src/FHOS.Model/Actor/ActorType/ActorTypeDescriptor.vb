Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class ActorTypeDescriptor
    ReadOnly Property CanSpawn As Func(Of ILocation, Boolean)
    ReadOnly Property SpawnRolls As IReadOnlyDictionary(Of String, String)
    ReadOnly Property ActorType As String
    ReadOnly Property CostumeGenerator As IReadOnlyDictionary(Of String, Integer)
    ReadOnly Property Initializer As Action(Of IActor)
    ReadOnly Property DescribeInteractor As Func(Of IActor, IEnumerable(Of (Text As String, Hue As Integer)))
    Sub New(
           actorType As String,
           costumeGenerator As IReadOnlyDictionary(Of String, Integer),
           Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing,
           Optional canSpawn As Func(Of ILocation, Boolean) = Nothing,
           Optional initializer As Action(Of IActor) = Nothing,
           Optional describeInteractor As Func(Of IActor, IEnumerable(Of (Text As String, Hue As Integer))) = Nothing)
        Me.ActorType = actorType
        Me.CostumeGenerator = costumeGenerator
        Me.CanSpawn = If(canSpawn, Function(x) True)
        Me.SpawnRolls = If(spawnRolls, New Dictionary(Of String, String))
        Me.Initializer = If(initializer, Sub(x) Return)
        Me.DescribeInteractor = If(describeInteractor, Function(x) Array.Empty(Of (Text As String, Hue As Integer)))
    End Sub
    Function CreateActor(location As ILocation) As IActor
        If Not CanSpawn(location) Then
            Return Nothing
        End If
        Dim actor = location.CreateActor(ActorType)
        actor.State.Facing = RNG.FromRange(0, Facing.Deltas.Length - 1)
        actor.Properties.CostumeType = RNG.FromGenerator(CostumeGenerator)
        actor.Properties.Name = ActorType
        Initializer.Invoke(actor)
        Return actor
    End Function
End Class
