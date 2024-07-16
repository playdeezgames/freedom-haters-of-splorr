Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class NeutralState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        With model
            If Not .Generator.Done Then
                Return New GenerateState(model, ui, endState)
                'ElseIf .State.Messages.HasAny Then
                '    Return New MessageState(model, ui, endState)
                'ElseIf .State.Avatar.Status.GameOver Then
                '    Return New GameOverState(model, ui, endState)
                'ElseIf .State.Avatar.StarGate.IsActive Then
                '    Return New StarGateState(model, ui, endState)
                'ElseIf .State.Avatar.Shipyard.IsActive Then
                '    Return New ShipyardState(model, ui, endState)
                'ElseIf .State.Avatar.Trader.IsActive Then
                '    Return New TraderState(model, ui, endState)
            ElseIf .State.Avatar.Interaction.IsActive Then
                Return New InteractionState(model, ui, endState)
            Else
                Return New NavigationState(model, ui, endState)
            End If
        End With
        Return endState
    End Function
End Class
