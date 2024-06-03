Public Class AvatarStarGateModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.IsActive.ShouldBeFalse
        Should.Throw(Of KeyNotFoundException)(Function() sut.AvailableGates.ToList())
        Should.Throw(Of NullReferenceException)(Sub() sut.Enter(Nothing))
        Should.NotThrow(Sub() sut.Leave())
    End Sub

    Private Shared Function CreateSut() As IAvatarStarGateModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.StarGate
    End Function
End Class
