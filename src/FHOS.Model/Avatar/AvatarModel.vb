Imports FHOS.Persistence

Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly avatar As IActor

    Public Sub New(avatar As IActor)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property X As Integer Implements IAvatarModel.X
        Get
            Return avatar.Location.Column
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IAvatarModel.Y
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
                Return Hue.Red
            End If
            If OxygenPercent <= 66 Then
                Return Hue.Yellow
            End If
            Return Hue.Green
        End Get
    End Property

    Public ReadOnly Property OxygenPercent As Integer Implements IAvatarModel.OxygenPercent
        Get
            Return avatar.Oxygen * 100 \ avatar.MaximumOxygen
        End Get
    End Property

    Public ReadOnly Property HasActions As Boolean Implements IAvatarModel.HasActions
        Get
            Return StarSystem.CanEnter OrElse Planet.CanApproach OrElse Star.CanApproach
        End Get
    End Property

    Public ReadOnly Property Tutorial As IAvatarTutorialModel Implements IAvatarModel.Tutorial
        Get
            Return New AvatarTutorialModel(avatar)
        End Get
    End Property

    Public ReadOnly Property StarSystem As IAvatarStarSystemModel Implements IAvatarModel.StarSystem
        Get
            Return New AvatarStarSystemModel(avatar)
        End Get
    End Property

    Public ReadOnly Property Star As IAvatarStarModel Implements IAvatarModel.Star
        Get
            Return New AvatarStarModel(avatar)
        End Get
    End Property

    Public ReadOnly Property Planet As IAvatarPlanetModel Implements IAvatarModel.Planet
        Get
            Return New AvatarPlanetModel(avatar)
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
                Return Hue.Red
            End If
            If FuelPercent <= 66 Then
                Return Hue.Yellow
            End If
            Return Hue.Green
        End Get
    End Property

    Public Sub Move(delta As (X As Integer, Y As Integer)) Implements IAvatarModel.Move
        Dim location = avatar.Location
        Dim nextColumn = location.Column + delta.X
        Dim nextRow = location.Row + delta.Y
        Dim map = location.Map
        Dim nextLocation = map.GetLocation(nextColumn, nextRow)
        If nextLocation IsNot Nothing Then
            If nextLocation.HasTeleporter Then
                avatar.Location = nextLocation.Teleporter.Target
            Else
                avatar.Location = nextLocation
            End If
        End If
    End Sub

    Public Sub SetFacing(facing As Integer) Implements IAvatarModel.SetFacing
        avatar.Facing = facing
    End Sub
End Class
