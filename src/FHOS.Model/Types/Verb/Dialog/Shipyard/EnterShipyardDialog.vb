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
            interactor.EntityName)
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
        Return New ChangeEquipmentDialog(Actor, interactor, Me)
    End Function

    Private Function InstallEquipment() As IDialog
        Return New InstallEquipmentDialog(Actor, interactor, Me)
    End Function

    Private Function UninstallEquipment() As IDialog
        Return New UninstallEquipmentDialog(Actor, interactor, Me)
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return Array.Empty(Of (Hue As Integer, Text As String))
    End Function
End Class
