Imports FHOS.Persistence

Friend Class AvatarTutorialModel
    Implements IAvatarTutorialModel

    Private avatar As IActor

    Public Sub New(avatar As IActor)
        Me.avatar = avatar
    End Sub
    Public Sub Dismiss() Implements IAvatarTutorialModel.Dismiss
        Dim descriptor = TutorialTypes.Descriptors(avatar.CurrentTutorial)
        If descriptor.HasIgnoreFlag Then
            avatar.SetFlag(descriptor.IgnoreFlag)
        End If
        avatar.DismissTutorial()
    End Sub
    Public ReadOnly Property Current As String Implements IAvatarTutorialModel.Current
        Get
            Return avatar.CurrentTutorial
        End Get
    End Property
    Public ReadOnly Property HasAny As Boolean Implements IAvatarTutorialModel.HasAny
        Get
            Return avatar.HasTutorial
        End Get
    End Property

    Public ReadOnly Property IgnoreCurrent As Boolean Implements IAvatarTutorialModel.IgnoreCurrent
        Get
            If Not HasAny Then
                Return True
            End If
            Dim descriptor = TutorialTypes.Descriptors(Current)
            If descriptor.HasIgnoreFlag Then
                Return avatar.HasFlag(descriptor.IgnoreFlag)
            End If
            Return False
        End Get
    End Property
End Class
