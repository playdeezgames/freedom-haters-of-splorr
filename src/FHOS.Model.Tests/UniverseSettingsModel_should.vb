Public Class UniverseSettingsModel_should
    Private Function CreateSut() As IUniverseSettingsModel
        Return New UniverseModel(initializer:=New EmptyUniverseInitializer(2), generationTimeSlice:=0.0).Settings
    End Function
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.GalacticAge.ShouldNotBeNull
        sut.GalacticDensity.ShouldNotBeNull
        sut.StartingWealth.ShouldNotBeNull
        sut.FactionCount.ShouldNotBeNull
    End Sub
End Class
