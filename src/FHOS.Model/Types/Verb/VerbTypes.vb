Imports FHOS.Persistence

Public Module VerbTypes
    Public ReadOnly DistressSignal As String = NameOf(DistressSignal)
    Public ReadOnly MoveRight As String = NameOf(MoveRight)
    Public ReadOnly MoveUp As String = NameOf(MoveUp)
    Public ReadOnly MoveDown As String = NameOf(MoveDown)
    Public ReadOnly MoveLeft As String = NameOf(MoveLeft)
    Public ReadOnly SPLORRPedia As String = NameOf(SPLORRPedia)
    Public ReadOnly Status As String = NameOf(Status)
    Public ReadOnly Inventory As String = NameOf(Inventory)
    Public ReadOnly Equipment As String = NameOf(Equipment)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, VerbTypeDescriptor) =
        New List(Of VerbTypeDescriptor) From
        {
            New MoveVerbTypeDescriptor(MoveUp, "Move Up", FacingTypes.Up),
            New MoveVerbTypeDescriptor(MoveRight, "Move Right", FacingTypes.Right),
            New MoveVerbTypeDescriptor(MoveDown, "Move Down", FacingTypes.Down),
            New MoveVerbTypeDescriptor(MoveLeft, "Move Left", FacingTypes.Left),
            New DistressSignalVerbTypeDescriptor(),
            New AlwaysAvailableVerbTypeDescriptor(Status, "Status"),
            New AlwaysAvailableVerbTypeDescriptor(SPLORRPedia, "SPLORR!!Pedia"),
            New ConditionalVerbTypeDescriptor(Inventory, "Inventory...", Function(Actor) Actor.Inventory.Items.Any),
            New ConditionalVerbTypeDescriptor(Equipment, "Equipment...", Function(Actor) Actor.Equipment.All.Any)
        }.ToDictionary(Function(x) x.VerbType, Function(x) x)
End Module
