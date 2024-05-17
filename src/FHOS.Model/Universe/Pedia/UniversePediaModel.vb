Imports FHOS.Persistence

Friend Class UniversePediaModel
    Implements IUniversePediaModel

    Private ReadOnly universe As IUniverse

    Public Sub New(universe As IUniverse)
        Me.universe = universe
    End Sub

    Friend Shared Function FromUniverse(universe As IUniverse) As IUniversePediaModel
        Return New UniversePediaModel(universe)
    End Function

    Public ReadOnly Property FactionList As IEnumerable(Of (Text As String, Faction As IFactionModel)) Implements IUniversePediaModel.FactionList
        Get
            Return universe.Factions.Select(Function(x) (x.Name, FactionModel.FromFaction(x)))
        End Get
    End Property

    Public ReadOnly Property StarSystems As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniversePediaModel.StarSystems
        Get
            Return universe.
                GetPlacesOfType(PlaceTypes.StarSystem).
                OrderBy(Function(x) x.Properties.Name).
                Select(Function(x) (x.Properties.Name, PlaceModel.FromPlace(x)))
        End Get
    End Property

    Public ReadOnly Property Planets As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniversePediaModel.Planets
        Get
            Return universe.
                GetPlacesOfType(PlaceTypes.Planet).
                OrderBy(Function(x) x.Properties.Name).
                Select(Function(x) (x.Properties.Name, PlaceModel.FromPlace(x)))
        End Get
    End Property

    Public ReadOnly Property Satellites As IEnumerable(Of (Text As String, Place As IPlaceModel)) Implements IUniversePediaModel.Satellites
        Get
            Return universe.
                GetPlacesOfType(PlaceTypes.Satellite).
                OrderBy(Function(x) x.Properties.Name).
                Select(Function(x) (x.Properties.Name, PlaceModel.FromPlace(x)))
        End Get
    End Property
End Class
