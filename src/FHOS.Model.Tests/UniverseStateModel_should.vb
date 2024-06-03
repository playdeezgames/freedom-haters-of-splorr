Public Class UniverseStateModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Turn.ShouldBe(1)
        sut.Board.ShouldNotBeNull
        sut.Avatar.ShouldBeNull
        sut.Messages.ShouldNotBeNull
    End Sub

    Private Function CreateSut() As IUniverseStateModel
        Dim model = New UniverseModel(initializer:=New EmptyUniverseInitializer(0), generationTimeSlice:=0.0)
        model.Generator.Start()
        While Not model.Generator.Done
            model.Generator.Generate()
        End While
        Return model.State
    End Function
End Class
