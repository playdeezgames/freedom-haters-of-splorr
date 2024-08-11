Imports FHOS.Persistence
Imports SPLORR.Game

Friend MustInherit Class ActorTypeDescriptor
    Friend ReadOnly Property ActorType As String
    Friend ReadOnly Property Flag(flagType As String) As Boolean
        Get
            Return flags.Contains(flagType)
        End Get
    End Property
    Friend ReadOnly Property Subtype As String
    Friend ReadOnly Property EquipSlots As IReadOnlyList(Of String)
    Private ReadOnly flags As ISet(Of String)
    Friend Function HasEquipSlot(equipSlot As String) As Boolean
        Return EquipSlots.Contains(equipSlot)
    End Function
    Friend Overridable Function GetSpawnCount(mapType As String) As Integer
        Return 0
    End Function
    Friend MustOverride Function CanSpawn(location As ILocation) As Boolean
    Friend ReadOnly Property CostumeGenerator As IReadOnlyDictionary(Of String, Integer)
    Protected MustOverride Sub Initialize(actor As IActor)
    Friend MustOverride Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
    Friend Sub New(
           actorType As String,
           costumeGenerator As IReadOnlyDictionary(Of String, Integer),
           Optional flags As IEnumerable(Of String) = Nothing,
           Optional subtype As String = Nothing,
           Optional availableEquipSlots As IReadOnlyList(Of String) = Nothing)
        Me.ActorType = actorType
        Me.CostumeGenerator = costumeGenerator
        Me.Subtype = subtype
        Me.EquipSlots = If(availableEquipSlots, New List(Of String))
        Me.flags = If(
            flags IsNot Nothing,
            New HashSet(Of String)(flags),
            New HashSet(Of String))
    End Sub
    Friend Function CreateActor(location As ILocation, name As String) As IActor
        Dim actor = location.CreateActor(ActorType, name)
        actor.Statistics(StatisticTypes.Facing) = RNG.FromEnumerable(FacingTypes.All)
        actor.Costume = RNG.FromGenerator(CostumeGenerator)
        Initialize(actor)
        Return actor
    End Function
End Class
