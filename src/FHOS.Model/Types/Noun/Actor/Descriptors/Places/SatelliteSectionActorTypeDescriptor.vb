﻿Imports System.Data

Friend Class SatelliteSectionActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(satelliteType As String, sectionName As String)
        MyBase.New(
            ActorTypes.MakeSatelliteSection(satelliteType, sectionName),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeSatelliteSection(satelliteType, sectionName), 1}
            },
            flags:={FlagTypes.IsSatelliteSection},
            subtype:=satelliteType)
    End Sub

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result As New List(Of (Text As String, Hue As Integer)) From {
            ($"Satellite Type: {SatelliteTypes.Descriptors(actor.Descriptor.Subtype).SatelliteType}", Hues.LightGray),
            ($"Tech Level: {actor.Yokes.Group(YokeTypes.Satellite).Statistics(StatisticTypes.TechLevel)}", Hues.LightGray)
        }
        Dim planetVicinityGroup = actor.Yokes.Group(YokeTypes.Satellite).SingleParent(GroupTypes.PlanetVicinity)
        Dim starSystemGroup = planetVicinityGroup.SingleParent(GroupTypes.StarSystem)
        result.Add(($"Planet: {planetVicinityGroup.EntityName}", Hues.LightGray))
        result.Add(($"Star System: {starSystemGroup.EntityName}", Hues.LightGray))
        Return result
    End Function
End Class
