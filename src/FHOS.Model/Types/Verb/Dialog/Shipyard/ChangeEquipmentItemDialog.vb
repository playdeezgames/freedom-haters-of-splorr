Imports FHOS.Data

Friend Class ChangeEquipmentItemDialog
    Inherits BaseInteractorMenuDialog
    Implements IDialog

    Private ReadOnly equipSlot As String

    Public Sub New(actor As Persistence.IActor, interactor As Persistence.IActor, equipSlot As String, finalDialog As IDialog)
        MyBase.New(actor, interactor, finalDialog, "Which item?")
        Me.equipSlot = equipSlot
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From
            {
                {DialogChoices.Cancel, AddressOf EndDialog}
            }
        For Each item In Actor.InstallableItems(equipSlot)
            menu.Add(ToName(item), InstallItem(item))
        Next
        Return menu
    End Function

    Private Function InstallItem(item As Persistence.IItem) As Func(Of IDialog)
        Return Function() New ChangeEquipmentItemCompleteDialog(Actor, interactor, equipSlot, item, ToFee(item), finalDialog)
    End Function

    Private Function ToName(item As Persistence.IItem) As String
        Dim changeFee = ToFee(item)
        Return $"{item.EntityName}{If(changeFee > 0, $"(fee: {changeFee})", String.Empty)}"
    End Function

    Private Function ToFee(item As Persistence.IItem) As Integer
        Return If(Actor.Equipment.GetSlot(equipSlot)?.UninstallFee, 0) + item.InstallFee
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return Array.Empty(Of (Hue As Integer, Text As String))
    End Function
End Class
