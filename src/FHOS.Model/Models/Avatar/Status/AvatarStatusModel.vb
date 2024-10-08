﻿Imports FHOS.Persistence

Friend Class AvatarStatusModel
    Inherits BaseAvatarModel
    Implements IAvatarStatusModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property GameOver As Boolean Implements IAvatarStatusModel.GameOver
        Get
            Return Dead OrElse Bankrupt
        End Get
    End Property

    Public ReadOnly Property Dead As Boolean Implements IAvatarStatusModel.Dead
        Get
            Return actor.LifeSupport.CurrentValue = actor.LifeSupport.MinimumValue.Value
        End Get
    End Property

    Public ReadOnly Property Bankrupt As Boolean Implements IAvatarStatusModel.Bankrupt
        Get
            Return If(actor.GetJools = actor.Yokes.Store(YokeTypes.Wallet).MinimumValue, False)
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarStatusModel
        Return New AvatarStatusModel(actor)
    End Function
End Class
