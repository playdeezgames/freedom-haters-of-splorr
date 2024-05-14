Imports FHOS.Persistence

Friend Class MessagesModel
    Implements IMessagesModel

    Private ReadOnly universe As Persistence.IUniverse

    Protected Sub New(universe As Persistence.IUniverse)
        Me.universe = universe
    End Sub

    Friend Shared Function FromUniverse(universe As IUniverse) As IMessagesModel
        If universe Is Nothing Then
            Return Nothing
        End If
        Return New MessagesModel(universe)
    End Function

    Public ReadOnly Property HasAny As Boolean Implements IMessagesModel.HasAny
        Get
            Return universe.Messages.HasAny
        End Get
    End Property

    Public ReadOnly Property Current As IMessageModel Implements IMessagesModel.Current
        Get
            Return MessageModel.FromMessage(universe.Messages.Current)
        End Get
    End Property

    Public Sub Dismiss() Implements IMessagesModel.Dismiss
        universe.Messages.Dismiss()
    End Sub
End Class
