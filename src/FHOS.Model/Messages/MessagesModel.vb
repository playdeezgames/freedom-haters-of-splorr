Friend Class MessagesModel
    Implements IMessagesModel

    Private ReadOnly universe As Persistence.IUniverse

    Public Sub New(universe As Persistence.IUniverse)
        Me.universe = universe
    End Sub

    Public ReadOnly Property HasAny As Boolean Implements IMessagesModel.HasAny
        Get
            Return universe.Messages.HasAny
        End Get
    End Property

    Public ReadOnly Property Current As IMessageModel Implements IMessagesModel.Current
        Get
            Return New MessageModel(universe.Messages.Current)
        End Get
    End Property

    Public Sub Dismiss() Implements IMessagesModel.Dismiss
        universe.Messages.Dismiss()
    End Sub
End Class
