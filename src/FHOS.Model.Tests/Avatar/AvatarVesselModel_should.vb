﻿Public Class AvatarVesselModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.AvailableCrew.ShouldBeEmpty
        Should.Throw(Of NullReferenceException)(Function() sut.OxygenPercent)
        sut.FuelPercent.ShouldBeNull
    End Sub

    Private Function CreateSut() As IAvatarVesselModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Vessel
    End Function
End Class
