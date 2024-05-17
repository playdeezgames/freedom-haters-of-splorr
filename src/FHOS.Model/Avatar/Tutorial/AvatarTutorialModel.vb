Imports FHOS.Persistence

Friend Class AvatarTutorialModel
    Implements IAvatarTutorialModel

    Private actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub
    Public Sub Dismiss() Implements IAvatarTutorialModel.Dismiss
        Dim descriptor = TutorialTypes.Descriptors(actor.CurrentTutorial)
        If descriptor.HasIgnoreFlag Then
            actor.Flags(descriptor.IgnoreFlag) = True
        End If
        actor.DismissTutorial()
    End Sub
    Public ReadOnly Property Current As String Implements IAvatarTutorialModel.Current
        Get
            Return actor.CurrentTutorial
        End Get
    End Property
    Public ReadOnly Property HasAny As Boolean Implements IAvatarTutorialModel.HasAny
        Get
            Return actor.HasTutorial
        End Get
    End Property

    Public ReadOnly Property IgnoreCurrent As Boolean Implements IAvatarTutorialModel.IgnoreCurrent
        Get
            If Not HasAny Then
                Return True
            End If
            Dim descriptor = TutorialTypes.Descriptors(Current)
            If descriptor.HasIgnoreFlag Then
                Return actor.Flags(descriptor.IgnoreFlag)
            End If
            Return False
        End Get
    End Property
End Class
