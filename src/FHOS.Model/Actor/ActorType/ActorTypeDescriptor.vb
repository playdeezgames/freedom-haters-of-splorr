Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class ActorTypeDescriptor
    ReadOnly Property MaximumFuel As Integer
    ReadOnly Property CanSpawn As Func(Of ILocation, Boolean)
    ReadOnly Property SpawnCount As Integer
    ReadOnly Property ActorType As String
    ReadOnly Property CostumeGenerator As IReadOnlyDictionary(Of String, Integer)
    ReadOnly Property Initializer As Action(Of IActor)
    Sub New(
           actorType As String,
           costumeGenerator As IReadOnlyDictionary(Of String, Integer),
           Optional maximumFuel As Integer = 0,
           Optional spawnCount As Integer = 0,
           Optional canSpawn As Func(Of ILocation, Boolean) = Nothing,
           Optional initializer As Action(Of IActor) = Nothing)
        Me.ActorType = actorType
        Me.CostumeGenerator = costumeGenerator
        Me.MaximumFuel = maximumFuel
        If canSpawn Is Nothing Then
            canSpawn = Function(x) True
        End If
        Me.CanSpawn = canSpawn
        Me.SpawnCount = spawnCount
        If initializer Is Nothing Then
            initializer = Sub(x) Return
        End If
        Me.Initializer = initializer
    End Sub
    Function CreateActor(location As ILocation) As IActor
        If Not CanSpawn(location) Then
            Return Nothing
        End If
        Dim actor = location.CreateActor(ActorType)
        actor.Facing = RNG.FromRange(0, Facing.Deltas.Length - 1)
        actor.Costume = RNG.FromGenerator(CostumeGenerator)
        actor.MaximumFuel = MaximumFuel
        actor.Fuel = MaximumFuel
        actor.Jools = 0
        actor.MinimumJools = 0
        actor.Name = ActorType
        Initializer.Invoke(actor)
        Return actor
    End Function
End Class
