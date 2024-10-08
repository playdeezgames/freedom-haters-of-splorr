﻿Public Class GroupPropertiesModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        Should.Throw(Of InvalidOperationException)(Function() sut.Authority)
        Should.Throw(Of InvalidOperationException)(Function() sut.Standards)
        Should.Throw(Of InvalidOperationException)(Function() sut.Conviction)
        Should.Throw(Of InvalidOperationException)(Function() sut.PlanetCount)
        Should.Throw(Of InvalidOperationException)(Function() sut.SatelliteCount)
        Should.Throw(Of InvalidOperationException)(Function() sut.Position)
        sut.StarTypeName.ShouldBeNull
        sut.PlanetTypeName.ShouldBeNull
        sut.SatelliteTypeName.ShouldBeNull
        Should.Throw(Of InvalidOperationException)(Function() sut.TechLevel)
        sut.Values.ShouldBeEmpty
    End Sub

    Private Function CreateSut() As IGroupPropertiesModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Bio.Faction.Properties
    End Function
End Class
