Imports FHOS.Persistence

Friend Class PlanetSectionActorTypeDescriptor
    Inherits ActorTypeDescriptor


    Public Sub New(planetType As String, sectionName As String, isPlanet As Boolean)
        MyBase.New(
            ActorTypes.MakePlanetSection(planetType, sectionName),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakePlanetSection(planetType, sectionName), 1}
            },
            New Dictionary(Of String, String),
            isPlanet:=isPlanet,
            isPlanetSection:=Not isPlanet,
            subtype:=planetType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        Dim location = actor.State.Location
        location.LocationType = LocationTypes.Planet
        For Each neighbor In location.GetEmptyNeighborsOfType(LocationTypes.Void)
            neighbor.LocationType = LocationTypes.PlanetAdjacent
        Next
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {("It's a planet! It's real big!", Hues.Black)}
    End Function
End Class
