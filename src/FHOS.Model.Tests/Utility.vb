Friend Module Utility
    Function CreateOneStepUniverse(builder As Action(Of IUniverse, EmbarkSettings)) As IUniverseModel
        Dim model = New UniverseModel(initializer:=New OneStepUniverseInitializer(builder), generationTimeSlice:=0.0)
        model.Generator.Start()
        While Not model.Generator.Done
            model.Generator.Generate()
        End While
        Return model
    End Function

    Friend Sub BuildLonelyUniverse(universe As IUniverse, settings As EmbarkSettings)
        Dim map = universe.Factory.CreateMap("map name", "map type", 1, 1, "location type")
        universe.Avatar.Push(map.GetLocation(0, 0).CreateActor("actor type", "actor name"))
    End Sub
End Module
