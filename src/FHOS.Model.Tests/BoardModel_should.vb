Public Class BoardModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.GetLocation((0, 0)).ShouldNotBeNull
    End Sub

    Private Function CreateSut() As IBoardModel
        Dim model = New UniverseModel(initializer:=New OneStepUniverseInitializer(AddressOf BuildUniverse), generationTimeSlice:=0.0)
        model.Generator.Start()
        While Not model.Generator.Done
            model.Generator.Generate()
        End While
        Return model.State.Board
    End Function

    Private Sub BuildUniverse(universe As IUniverse, settings As EmbarkSettings)
        Dim map = universe.Factory.CreateMap("map name", "map type", 1, 1, "location type")
        universe.Avatar.Push(map.GetLocation(0, 0).CreateActor("actor type", "actor name"))
    End Sub
End Class
