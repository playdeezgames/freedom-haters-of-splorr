Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class ActorTypeDescriptor
    ReadOnly Property CanSpawn As Func(Of ILocation, Boolean)
    ReadOnly Property SpawnRolls As IReadOnlyDictionary(Of String, String)
    ReadOnly Property ActorType As String
    ReadOnly Property CostumeGenerator As IReadOnlyDictionary(Of String, Integer)
    ReadOnly Property Initializer As Action(Of IActor)
    Sub New(
           actorType As String,
           costumeGenerator As IReadOnlyDictionary(Of String, Integer),
           Optional spawnRolls As IReadOnlyDictionary(Of String, String) = Nothing,
           Optional canSpawn As Func(Of ILocation, Boolean) = Nothing,
           Optional initializer As Action(Of IActor) = Nothing)
        Me.ActorType = actorType
        Me.CostumeGenerator = costumeGenerator
        If canSpawn Is Nothing Then
            canSpawn = Function(x) True
        End If
        Me.CanSpawn = canSpawn
        Me.SpawnRolls = If(spawnRolls, New Dictionary(Of String, String))
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
        actor.State.Facing = RNG.FromRange(0, Facing.Deltas.Length - 1)
        actor.Properties.CostumeType = RNG.FromGenerator(CostumeGenerator)
        actor.Properties.Name = ActorType
        Initializer.Invoke(actor)
        Return actor
    End Function
End Class
