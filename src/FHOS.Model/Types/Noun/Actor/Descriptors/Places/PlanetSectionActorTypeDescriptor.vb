﻿Imports FHOS.Persistence

Friend Class PlanetSectionActorTypeDescriptor
    Inherits ActorTypeDescriptor


    Public Sub New(planetType As String, sectionName As String, isPlanet As Boolean)
        MyBase.New(
            ActorTypes.MakePlanetSection(planetType, sectionName),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakePlanetSection(planetType, sectionName), 1}
            },
            flags:={If(isPlanet, FlagTypes.IsPlanet, FlagTypes.IsPlanetSection)},
            subtype:=planetType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        Dim location = actor.Location
        location.EntityType = LocationTypes.Planet
        For Each neighbor In location.GetEmptyCardinalNeighborsOfType(LocationTypes.Void)
            neighbor.EntityType = LocationTypes.PlanetAdjacent
        Next
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim planetGroup = actor.Yokes.Group(YokeTypes.Planet)
        Dim planetVicinityGroup = planetGroup.SingleParent(GroupTypes.PlanetVicinity)
        Dim factionGroup = planetVicinityGroup.SingleParent(GroupTypes.Faction)
        Dim starSystemGroup = planetVicinityGroup.SingleParent(GroupTypes.StarSystem)
        Return New List(Of (Text As String, Hue As Integer)) From
            {
                ($"Planet Type: {PlanetTypes.Descriptors(actor.Descriptor.Subtype).PlanetType}", Hues.LightGray),
                ($"Tech Level: {planetVicinityGroup.Statistics(StatisticTypes.TechLevel)}", Hues.LightGray),
                ($"Star System: {starSystemGroup.EntityName}", Hues.LightGray),
                ($"Faction: {factionGroup.EntityName}", Hues.LightGray),
                ($"Star Gates: {planetVicinityGroup.Statistics(StatisticTypes.StarGateCount)}", Hues.LightGray),
                ($"Ship Yards: {planetVicinityGroup.Statistics(StatisticTypes.ShipyardCount)}", Hues.LightGray),
                ($"Trading Posts: {planetVicinityGroup.Statistics(StatisticTypes.TradingPostCount)}", Hues.LightGray)
            }
    End Function
End Class
