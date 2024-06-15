Imports System.Data

Friend Class SatelliteSectionActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(satelliteType As String, sectionName As String)
        MyBase.New(
            ActorTypes.MakeSatelliteSection(satelliteType, sectionName),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeSatelliteSection(satelliteType, sectionName), 1}
            },
            New Dictionary(Of String, String),
            isSatelliteSection:=True,
            subtype:=satelliteType)
    End Sub

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result As New List(Of (Text As String, Hue As Integer))
        result.Add(($"Satellite Type: {SatelliteTypes.Descriptors(actor.Descriptor.Subtype).SatelliteType}", Hues.Black))
        Dim planetVicinityGroup = actor.Yokes.Group(YokeTypes.Satellite).SingleParent(GroupTypes.PlanetVicinity)
        Dim starSystemGroup = planetVicinityGroup.SingleParent(GroupTypes.StarSystem)
        result.Add(($"Planet: {planetVicinityGroup.EntityName}", Hues.Black))
        result.Add(($"Star System: {starSystemGroup.EntityName}", Hues.Black))
        Return result
    End Function
End Class
