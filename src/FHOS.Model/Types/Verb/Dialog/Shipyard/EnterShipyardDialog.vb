Imports FHOS.Data

Friend Class EnterShipyardDialog
    Inherits BaseInteractorMenuDialog

    Public Sub New(
                  actor As Persistence.IActor,
                  interactor As Persistence.IActor,
                  finalDialog As IDialog)
        MyBase.New(
            actor,
            interactor,
            finalDialog,
            $"Now What?")
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From
            {
                {DialogChoices.Cancel, AddressOf EndDialog}
            }
        If interactor.CanChangeEquipment(Actor) Then
            menu.Add(DialogChoices.ChangeEquipment, AddressOf ChangeEquipment)
        End If
        If interactor.CanInstallEquipment(Actor) Then
            menu.Add(DialogChoices.InstallEquipment, AddressOf InstallEquipment)
        End If
        If interactor.CanUninstallEquipment(Actor) Then
            menu.Add(DialogChoices.UninstallEquipment, AddressOf UninstallEquipment)
        End If
        Return menu
    End Function

    Private Function ChangeEquipment() As IDialog
        Reset()
        Return New ChangeEquipmentDialog(Actor, interactor, Me)
    End Function

    Private Function InstallEquipment() As IDialog
        Reset()
        Return New InstallEquipmentDialog(Actor, interactor, Me)
    End Function

    Private Function UninstallEquipment() As IDialog
        Reset()
        Return New UninstallEquipmentDialog(Actor, interactor, Me)
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim lines As New List(Of (Hue As Integer, Text As String)) From {
            (Hues.Green, interactor.EntityName),
            (Hues.Green, String.Empty),
            (Hues.Orange, $"{Actor.EntityName}'s Equipment:")
        }
        For Each equipSlot In Actor.Equipment.AllSlots
            Dim item = Actor.Equipment.GetSlot(equipSlot)
            lines.Add((Hues.LightGray, $" - {EquipSlots.Descriptors(equipSlot).DisplayName}: {If(item IsNot Nothing, item.EntityName, "(empty)")}"))
        Next
        lines.Add((Hues.LightGray, String.Empty))
        lines.Add((Hues.LightGray, $"Jools: {Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue}"))
        Return lines
    End Function
End Class
