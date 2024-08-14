Imports FHOS.Data

Friend Class ChangeEquipmentDialog
    Inherits BaseInteractorMenuDialog
    Implements IDialog

    Public Sub New(actor As Persistence.IActor, interactor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, interactor, finalDialog, "Which Slot?")
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From
            {
                {DialogChoices.Cancel, AddressOf EndDialog}
            }
        For Each equipSlot In Actor.ChangeableEquipmentSlots
            menu.Add(EquipSlots.Descriptors(equipSlot).DisplayName, PickItemForEquipSlot(equipSlot))
        Next
        Return menu
    End Function

    Private Function PickItemForEquipSlot(equipSlot As String) As Func(Of IDialog)
        Return Function() New ChangeEquipmentItemDialog(Actor, interactor, equipSlot, Me)
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return Array.Empty(Of (Hue As Integer, Text As String))
    End Function
End Class
