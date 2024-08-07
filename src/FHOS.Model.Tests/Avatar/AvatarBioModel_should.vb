﻿Public Class AvatarBioModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.HomePlanet.ShouldBeNull
        sut.Faction.ShouldNotBeNull
        Should.Throw(Of ArgumentNullException)(Sub() sut.Reputation(Nothing))
    End Sub

    Private Function CreateSut() As IAvatarBioModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Bio
    End Function
End Class
