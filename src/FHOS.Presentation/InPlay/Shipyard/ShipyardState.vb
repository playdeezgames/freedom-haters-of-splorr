Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ShipyardState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        With model.State.Avatar.Shipyard
            ui.Clear()
            Dim menu As New List(Of (String, String)) From
            {
                (Choices.Cancel, Choices.Cancel)
            }
            If .CanChangeEquipment Then
                menu.Add((Choices.ChangeEquipment, Choices.ChangeEquipment))
            End If
            If .CanInstallEquipment Then
                menu.Add((Choices.InstallEquipment, Choices.InstallEquipment))
            End If
            If .CanUninstallEquipment Then
                menu.Add((Choices.UninstallEquipment, Choices.UninstallEquipment))
            End If
            Dim choice = ui.Choose(
            (Mood.Prompt, .Specimen.Name),
            menu.ToArray)
            Select Case choice
                Case Choices.ChangeEquipment
                    Return New ChangeEquipmentState(model, ui, endState)
                Case Choices.InstallEquipment
                    Return New InstallEquipmentState(model, ui, endState)
                Case Choices.UninstallEquipment
                    Return New UninstallEquipmentState(model, ui, endState)
                Case Else
                    .Leave()
                    Return New NeutralState(model, ui, endState)
            End Select
        End With
    End Function
End Class
