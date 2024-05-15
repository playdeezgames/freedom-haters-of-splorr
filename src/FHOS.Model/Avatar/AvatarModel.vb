Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarModel
    Inherits BaseAvatarModel
    Implements IAvatarModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarModel
        If actor Is Nothing Then
            Return Nothing
        End If
        Return New AvatarModel(actor)
    End Function

    Private ReadOnly Property X As Integer
        Get
            Return avatar.Location.Column
        End Get
    End Property

    Private ReadOnly Property Y As Integer
        Get
            Return avatar.Location.Row
        End Get
    End Property

    Public ReadOnly Property MapName As String Implements IAvatarModel.MapName
        Get
            Return avatar.Location.Map.Name
        End Get
    End Property

    Public ReadOnly Property OxygenHue As Integer Implements IAvatarModel.OxygenHue
        Get
            If OxygenPercent <= 33 Then
                Return Hues.Red
            End If
            If OxygenPercent <= 66 Then
                Return Hues.Yellow
            End If
            Return Hues.Green
        End Get
    End Property

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarModel.OxygenPercent
        Get
            Return avatar.LifeSupport.CurrentValue * 100 \ avatar.LifeSupport.MaximumValue.Value
        End Get
    End Property

    Public ReadOnly Property Tutorial As IAvatarTutorialModel Implements IAvatarModel.Tutorial
        Get
            Return New AvatarTutorialModel(avatar)
        End Get
    End Property

    Public ReadOnly Property FuelPercent As Integer Implements IAvatarModel.FuelPercent
        Get
            Return avatar.Fuel * 100 \ avatar.MaximumFuel
        End Get
    End Property

    Public ReadOnly Property FuelHue As Integer Implements IAvatarModel.FuelHue
        Get
            If FuelPercent <= 33 Then
                Return Hues.Red
            End If
            If FuelPercent <= 66 Then
                Return Hues.Yellow
            End If
            Return Hues.Green
        End Get
    End Property

    Public ReadOnly Property IsGameOver As Boolean Implements IAvatarModel.IsGameOver
        Get
            Return IsDead OrElse IsBankrupt
        End Get
    End Property

    Public ReadOnly Property Turn As Integer Implements IAvatarModel.Turn
        Get
            Return avatar.Turn
        End Get
    End Property

    Public ReadOnly Property Jools As Integer Implements IAvatarModel.Jools
        Get
            Return avatar.Jools
        End Get
    End Property

    Public ReadOnly Property MinimumJools As Integer Implements IAvatarModel.MinimumJools
        Get
            Return avatar.MinimumJools
        End Get
    End Property

    Public ReadOnly Property IsDead As Boolean Implements IAvatarModel.IsDead
        Get
            Return avatar.LifeSupport.CurrentValue = avatar.LifeSupport.MinimumValue.Value
        End Get
    End Property

    Public ReadOnly Property IsBankrupt As Boolean Implements IAvatarModel.IsBankrupt
        Get
            Return avatar.Jools < avatar.MinimumJools
        End Get
    End Property

    Public ReadOnly Property CurrentPlace As IPlaceModel Implements IAvatarModel.CurrentPlace
        Get
            If avatar.Location.Place IsNot Nothing Then
                Return PlaceModel.FromPlace(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property IsInteracting As Boolean Implements IAvatarModel.IsInteracting
        Get
            Return avatar.Interactor IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property AvailableCrew As IEnumerable(Of (Name As String, Actor As IActorModel)) Implements IAvatarModel.AvailableCrew
        Get
            Return avatar.Crew.Select(Function(x) (x.Name, ActorModel.FromActor(x)))
        End Get
    End Property

    Public ReadOnly Property Position As (X As Integer, Y As Integer) Implements IAvatarModel.Position
        Get
            Return (X, Y)
        End Get
    End Property

    Public ReadOnly Property Bio As IAvatarBioModel Implements IAvatarModel.Bio
        Get
            Return AvatarBioModel.FromAvatar(avatar)
        End Get
    End Property

    Public ReadOnly Property Verbs As IAvatarVerbsModel Implements IAvatarModel.Verbs
        Get
            Return AvatarVerbsModel.FromAvatar(avatar)
        End Get
    End Property

    Public ReadOnly Property KnowsPlaces As Boolean Implements IAvatarModel.KnowsPlaces
        Get
            Return avatar.KnowsPlaces
        End Get
    End Property

    Public Function KnowsPlacesOfType(placeType As String) As Boolean Implements IAvatarModel.KnowsPlacesOfType
        Return avatar.KnowsPlacesOfType(placeType)
    End Function

    Public Function GetKnownPlacesOfType(placeType As String) As IEnumerable(Of (Text As String, Item As IPlaceModel)) Implements IAvatarModel.GetKnownPlacesOfType
        Return avatar.GetKnownPlacesOfType(placeType).Select(Function(x) (x.Name, PlaceModel.FromPlace(x)))
    End Function

    Public Sub LeaveInteraction() Implements IAvatarModel.LeaveInteraction
        avatar.Interactor = Nothing
    End Sub

    Public Sub Push(actor As IActorModel) Implements IAvatarModel.Push
        avatar.Universe.PushAvatar(ActorModel.GetActor(actor))
    End Sub

    Public Sub Pop() Implements IAvatarModel.Pop
        avatar.Universe.PopAvatar()
    End Sub
End Class
