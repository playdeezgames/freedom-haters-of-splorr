Imports FHOS.Data

Friend Class EnterStarGateDialog
    Inherits BaseInteractorMenuDialog

    Public Sub New(
                  actor As Persistence.IActor,
                  interactor As Persistence.IActor,
                  finalDialog As IDialog)
        MyBase.New(
            actor,
            interactor,
            finalDialog,
            "Destination:")
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim faction = interactor.Yokes.Group(YokeTypes.Faction)
        Dim gates = Actor.Universe.Actors.Where(Function(x) ActorTypes.Descriptors(x.EntityType).Flag(FlagTypes.IsStarGate) AndAlso x.Yokes.Group(YokeTypes.Faction).Id = faction.Id)
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From
            {
                {DialogChoices.Cancel, AddressOf EndDialog}
            }
        For Each gate In gates
            menu.Add(gate.EntityName, UseGate(gate))
        Next
        Return menu
    End Function

    Private Function UseGate(gate As Persistence.IActor) As Func(Of IDialog)
        Return Function()
                   Dim nextDialog As IDialog = finalDialog
                   Actor.GoToOtherActor(
                        gate,
                        Sub(success, otherActor)
                            If Not success Then
                                nextDialog = New DestinationBlockedDialog(Actor, finalDialog)
                            Else
                                Actor.SetStarSystem(otherActor.Yokes.Group(YokeTypes.StarSystem))
                            End If
                        End Sub)
                   Return nextDialog
               End Function
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return Array.Empty(Of (Hue As Integer, Text As String))
    End Function
End Class
