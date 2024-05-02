Imports FHOS.Persistence

Friend Class AvatarModel
    Inherits BaseAvatarModel
    Implements IAvatarModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
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
            Return LegacyStarSystem.CanEnter OrElse
                LegacyPlanetVicinity.CanApproach OrElse
                LegacyStarVicinity.CanApproach OrElse
                LegacyPlanet.CanRefillOxygen OrElse
                LegacyStar.CanRefillFuel OrElse
                KnowsStarSystems
        End Get
    End Property

    Public ReadOnly Property Tutorial As IAvatarTutorialModel Implements IAvatarModel.Tutorial
        Get
            Return New AvatarTutorialModel(avatar)
        End Get
    End Property

    Public ReadOnly Property LegacyStarSystem As IAvatarStarSystemModel Implements IAvatarModel.LegacyStarSystem
        Get
            Return New AvatarStarSystemModel(avatar)
        End Get
    End Property

    Public ReadOnly Property LegacyStarVicinity As IAvatarStarVicinityModel Implements IAvatarModel.LegacyStarVicinity
        Get
            Return New AvatarStarVicinityModel(avatar)
        End Get
    End Property

    Public ReadOnly Property LegacyPlanetVicinity As IAvatarPlanetVicinityModel Implements IAvatarModel.LegacyPlanetVicinity
        Get
            Return New AvatarPlanetVicinityModel(avatar)
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

    Public ReadOnly Property CanMove As Boolean Implements IAvatarModel.CanMove
        Get
            Return FuelPercent > 0
        End Get
    End Property

    Public ReadOnly Property IsGameOver As Boolean Implements IAvatarModel.IsGameOver
        Get
            Return IsDead OrElse IsBankrupt
        End Get
    End Property

    Public ReadOnly Property LegacyPlanet As IAvatarPlanetModel Implements IAvatarModel.LegacyPlanet
        Get
            Return New AvatarPlanetModel(avatar)
        End Get
    End Property

    Public ReadOnly Property LegacyStar As IAvatarStarModel Implements IAvatarModel.LegacyStar
        Get
            Return New AvatarStarModel(avatar)
        End Get
    End Property

    Public ReadOnly Property KnowsStarSystems As Boolean Implements IAvatarModel.KnowsStarSystems
        Get
            Return avatar.KnowsStarSystems
        End Get
    End Property

    Public ReadOnly Property StarSystemList As IEnumerable(Of (Text As String, Item As IStarSystemModel)) Implements IAvatarModel.StarSystemList
        Get
            Return avatar.KnownStarSystems.Select(Function(x) (x.Name, CType(New StarSystemModel(x), IStarSystemModel)))
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

    Public ReadOnly Property KnowsPlanetVicinities As Boolean Implements IAvatarModel.KnowsPlanetVicinities
        Get
            Return avatar.KnowsPlanetVicinities
        End Get
    End Property

    Public ReadOnly Property KnowsStarVicinities As Boolean Implements IAvatarModel.KnowsStarVicinities
        Get
            Return avatar.KnowsStarVicinities
        End Get
    End Property

    Public ReadOnly Property StarVicinityList As IEnumerable(Of (Text As String, Item As IStarVicinityModel)) Implements IAvatarModel.StarVicinityList
        Get
            Return avatar.KnownStarVicinities.Select(Function(x) (x.Name, CType(New StarVicinityModel(x), IStarVicinityModel)))
        End Get
    End Property

    Public ReadOnly Property PlanetVicinityList As IEnumerable(Of (Text As String, Item As IPlanetVicinityModel)) Implements IAvatarModel.PlanetVicinityList
        Get
            Return avatar.KnownPlanetVicinities.Select(Function(x) (x.Name, CType(New PlanetVicinityModel(x), IPlanetVicinityModel)))
        End Get
    End Property

    Public ReadOnly Property MinimumJools As Integer Implements IAvatarModel.MinimumJools
        Get
            Return avatar.MinimumJools
        End Get
    End Property

    Public ReadOnly Property IsDead As Boolean Implements IAvatarModel.IsDead
        Get
            Return avatar.Oxygen = 0
        End Get
    End Property

    Public ReadOnly Property IsBankrupt As Boolean Implements IAvatarModel.IsBankrupt
        Get
            Return avatar.Jools < avatar.MinimumJools
        End Get
    End Property

    Public ReadOnly Property KnowsPlaces As Boolean Implements IAvatarModel.KnowsPlaces
        Get
            Return KnowsPlanetVicinities OrElse KnowsStarSystems OrElse KnowsStarVicinities
        End Get
    End Property

    Public ReadOnly Property LegacySatellite As IAvatarSatelliteModel Implements IAvatarModel.LegacySatellite
        Get
            Return New AvatarSatelliteModel(avatar)
        End Get
    End Property

    Public ReadOnly Property Place As IAvatarPlaceModel Implements IAvatarModel.Place
        Get
            Return New AvatarPlaceModel(avatar)
        End Get
    End Property

    Public Sub Move(delta As (X As Integer, Y As Integer)) Implements IAvatarModel.Move
        If Not CanMove Then
            Return
        End If
        DoTurn()
        avatar.Fuel -= 1
        If Not avatar.HasFuel Then
            avatar.TriggerTutorial(TutorialTypes.OutOfFuel)
        End If
        Dim location = avatar.Location
        Dim nextColumn = location.Column + delta.X
        Dim nextRow = location.Row + delta.Y
        Dim map = location.Map
        Dim nextLocation = map.GetLocation(nextColumn, nextRow)
        If nextLocation IsNot Nothing Then
            If nextLocation.HasTeleporter Then
                SetLocation(nextLocation.Teleporter.Target)
            Else
                SetLocation(nextLocation)
            End If
        End If
    End Sub

    Public Sub SetFacing(facing As Integer) Implements IAvatarModel.SetFacing
        avatar.Facing = facing
    End Sub

    Public Sub DoDistressSignal() Implements IAvatarModel.DoDistressSignal
        Dim fuelAdded = avatar.MaximumFuel - avatar.Fuel
        Dim fuelPrice = 1 'TODO: don't just pick a magic number!
        Dim price = fuelPrice * fuelAdded
        avatar.Fuel = avatar.MaximumFuel
        avatar.Jools -= fuelAdded * fuelPrice
        avatar.AddMessage(
            "Emergency Refuel",
            ($"Added {fuelAdded} fuel!", Hue.Black),
            ($"Price {price} jools!", Hue.Black))
    End Sub
End Class
