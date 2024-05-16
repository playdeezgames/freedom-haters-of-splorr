Public Interface IAvatar
    Sub PushAvatar(avatar As IActor)
    Function PopAvatar() As IActor
    ReadOnly Property AvatarActor As IActor
End Interface
