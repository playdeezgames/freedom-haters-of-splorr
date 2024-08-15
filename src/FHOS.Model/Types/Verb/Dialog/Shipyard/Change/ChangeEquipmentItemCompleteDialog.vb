Imports FHOS.Data
Imports FHOS.Persistence

Friend Class ChangeEquipmentItemCompleteDialog
    Inherits BaseInteractorMenuDialog
    Implements IDialog

    Private ReadOnly equipSlot As String
    Private ReadOnly item As IItem
    Private ReadOnly fee As Integer
    Private ReadOnly canAfford As Boolean
    Private ReadOnly Property MeetsTechLevelRequirement As Boolean
        Get
            Dim interactorTechLevel = interactor.Statistics(StatisticTypes.TechLevel).Value
            If item IsNot Nothing AndAlso item.Descriptor.TechLevel > interactorTechLevel Then
                Return False
            End If
            Dim equippedItem = Actor.Equipment.GetSlot(equipSlot)
            If equippedItem IsNot Nothing AndAlso equippedItem.Descriptor.TechLevel > interactorTechLevel Then
                Return False
            End If
            Return True
        End Get
    End Property

    Public Sub New(
                  actor As IActor,
                  interactor As IActor,
                  equipSlot As String,
                  item As IItem,
                  fee As Integer,
                  finalDialog As IDialog)
        MyBase.New(
            actor,
            interactor,
            finalDialog,
            String.Empty)
        Me.equipSlot = equipSlot
        Me.item = item
        Me.fee = fee
        Me.canAfford = actor.CanAfford(fee)
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Return New Dictionary(Of String, Func(Of IDialog)) From
            {
                {DialogChoices.Ok, AddressOf EndDialog}
            }
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim lines As New List(Of (Hue As Integer, Text As String))
        If Not MeetsTechLevelRequirement Then
            lines.Add((Hues.Red, "Insufficient Tech Level!"))
        ElseIf Not canAfford Then
            lines.Add((Hues.Red, "Insufficient funds!"))
        Else
            If Actor.Equipment.GetSlot(equipSlot) IsNot Nothing Then
                Dim oldItem = Actor.Unequip(equipSlot)
                lines.Add((Hues.LightGray, $"Uninstalled {oldItem.EntityName} from {EquipSlots.Descriptors(equipSlot).DisplayName}."))
            End If
            If item IsNot Nothing Then
                Actor.Equip(equipSlot, item)
                lines.Add((Hues.LightGray, $"Installed {item.EntityName} on {EquipSlots.Descriptors(equipSlot).DisplayName}."))
            End If
            If fee > 0 Then
                lines.Add((Hues.LightGray, $"Paid Fees: {fee}"))
            End If
        End If
        lines.Add((Hues.LightGray, String.Empty))
        lines.Add((Hues.LightGray, $"Jools: {Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue}"))
        Return lines
    End Function
End Class
