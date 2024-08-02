Public Module OperationTypes
    Public ReadOnly DistressSignal As String = NameOf(DistressSignal)
    Public ReadOnly Equipment As String = NameOf(Equipment)
    Public ReadOnly Inventory As String = NameOf(Inventory)
    Public ReadOnly MoveDown As String = NameOf(MoveDown)
    Public ReadOnly MoveLeft As String = NameOf(MoveLeft)
    Public ReadOnly MoveRight As String = NameOf(MoveRight)
    Public ReadOnly MoveUp As String = NameOf(MoveUp)
    Public ReadOnly SPLORRPedia As String = NameOf(SPLORRPedia)
    Public ReadOnly Status As String = NameOf(Status)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, OperationTypeDescriptor) =
        New List(Of OperationTypeDescriptor) From
        {
            New MoveOperationTypeDescriptor(MoveUp, "Move Up", FacingTypes.Up),
            New MoveOperationTypeDescriptor(MoveRight, "Move Right", FacingTypes.Right),
            New MoveOperationTypeDescriptor(MoveDown, "Move Down", FacingTypes.Down),
            New MoveOperationTypeDescriptor(MoveLeft, "Move Left", FacingTypes.Left),
            New DistressSignalOperationTypeDescriptor(),
            New AlwaysAvailableOperationTypeDescriptor(Status, "Status"),
            New AlwaysAvailableOperationTypeDescriptor(SPLORRPedia, "SPLORR!!Pedia"),
            New ConditionalOperationTypeDescriptor(Inventory, "Inventory...", Function(Actor) Actor.Inventory.Items.Any),
            New ConditionalOperationTypeDescriptor(Equipment, "Equipment...", Function(Actor) Actor.Equipment.All.Any)
        }.ToDictionary(Function(x) x.OperationType, Function(x) x)
End Module
