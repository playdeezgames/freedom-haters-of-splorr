Imports FHOS.Data

Friend Class UninstallEquipmentDialog
    Inherits BaseInteractorMenuDialog
    Implements IDialog

    Public Sub New(actor As Persistence.IActor, interactor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, interactor, finalDialog, "Uninstall Which Slot?")
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From
            {
                {DialogChoices.Cancel, AddressOf EndDialog}
            }
        For Each equipSlot In Actor.UninstallableEquipmentSlots
            menu.Add(ToName(equipSlot), UninstallEquipSlot(equipSlot))
        Next
        Return menu
    End Function

    Private Function ToName(equipSlot As String) As String
        Dim item = Actor.Equipment.GetSlot(equipSlot)
        Dim fee = item.Descriptor.UninstallFee
        Return $"{EquipSlots.Descriptors(equipSlot).DisplayName}: {item.EntityName}(fee:{fee})"
    End Function

    Private Function UninstallEquipSlot(equipSlot As String) As Func(Of IDialog)
        Return Function()
                   Dim fee = Actor.Equipment.GetSlot(equipSlot).Descriptor.UninstallFee
                   Return New ChangeEquipmentItemCompleteDialog(Actor, interactor, equipSlot, Nothing, fee, finalDialog)
               End Function
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return Array.Empty(Of (Hue As Integer, Text As String))
    End Function
End Class
