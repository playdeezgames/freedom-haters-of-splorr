Public Class UniverseGeneratorModel_should
    <Fact>
    Sub start_generating_universe()
        Dim sut = CreateSut()
        sut.Start()
        sut.StepsRemaining.ShouldBeGreaterThan(0)
        sut.StepsCompleted.ShouldBe(0)
        sut.Done.ShouldBeFalse
    End Sub

    <Fact>
    Sub partially_generate_universe()
        Dim sut = CreateSut()
        sut.Start()
        sut.Generate()
        sut.StepsRemaining.ShouldBeGreaterThan(0)
        sut.StepsCompleted.ShouldBeGreaterThan(0)
        sut.Done.ShouldBeFalse
    End Sub

    <Fact>
    Sub completely_generate_universe()
        Dim sut = CreateSut()
        sut.Start()
        While Not sut.Done
            sut.Generate()
        End While
        sut.StepsRemaining.ShouldBe(0)
        sut.StepsCompleted.ShouldBeGreaterThan(0)
        sut.Done.ShouldBeTrue
    End Sub

    Private Shared Function CreateSut() As IUniverseGeneratorModel
        Return New UniverseModel(initializer:=New EmptyUniverseInitializer(2), generationTimeSlice:=0.0).Generator
    End Function
End Class
