﻿Friend Module Utility
    Function CreateOneStepUniverse(
                                  builder As Action(Of IUniverse, EmbarkSettings),
                                  Optional writeStringToFile As Action(Of String, String) = Nothing,
                                  Optional readStringFromFile As Func(Of String, String) = Nothing) As IUniverseModel
        Dim model = New UniverseModel(
                initializer:=New OneStepUniverseInitializer(builder),
                writeStringToFile:=writeStringToFile,
                readStringFromFile:=readStringFromFile,
                generationTimeSlice:=0.0)
        model.Generator.Start()
        While Not model.Generator.Done
            model.Generator.Generate()
        End While
        Return model
    End Function

    Friend Sub BuildLonelyUniverse(universe As IUniverse, settings As EmbarkSettings)
        Dim map = universe.Factory.CreateMap("map name", "map type", 1, 1, "location type")
        Dim actor = map.GetLocation(0, 0).CreateActor("actor type", "actor name")
        Dim group = universe.Factory.CreateGroup("group type", "group name")
        actor.Yokes.Group("Faction") = group
        universe.Avatar.SetActor(actor)
    End Sub

    Friend Sub BuildOneInventoryItemUniverse(universe As IUniverse, settings As EmbarkSettings)
        Dim map = universe.Factory.CreateMap("map name", "map type", 1, 1, "location type")
        Dim actor = map.GetLocation(0, 0).CreateActor("actor type", "actor name")
        Dim item = universe.Factory.CreateItem("Scrap")
        actor.Inventory.Add(item)
        Dim group = universe.Factory.CreateGroup("group type", "group name")
        actor.Yokes.Group("Faction") = group
        universe.Avatar.SetActor(actor)
    End Sub

    Friend Sub BuildTraderUniverse(universe As IUniverse, settings As EmbarkSettings)
        Dim map = universe.Factory.CreateMap("map name", "map type", 2, 1, "location type")
        Dim actor = map.GetLocation(0, 0).CreateActor("actor type", "actor name")
        actor.Yokes.Store("Wallet") = universe.Factory.CreateStore(0, -1)
        actor.Inventory.Add(universe.Factory.CreateItem("Scrap"))
        Dim trader = map.GetLocation(1, 0).CreateActor("trader", "trader")
        trader.Offers.Add("Scrap")
        trader.Prices.Add("Scrap")
        actor.Yokes.Actor("Trader") = trader
        Dim group = universe.Factory.CreateGroup("group type", "group name")
        actor.Yokes.Group("Faction") = group
        universe.Avatar.SetActor(actor)
    End Sub

    Friend Sub BuildShipyardUniverse(universe As IUniverse, settings As EmbarkSettings)
        Dim map = universe.Factory.CreateMap("map name", "map type", 2, 1, "location type")
        Dim actor = map.GetLocation(0, 0).CreateActor("PlayerShip", "actor name")
        actor.Yokes.Store("Wallet") = universe.Factory.CreateStore(0, -1)
        actor.Yokes.Store("FuelTank") = universe.Factory.CreateStore(0, 0, 0)
        Dim item = universe.Factory.CreateItem("FuelSupplyMarkI")
        actor.Equipment.Equip("PrimaryFuelSupply", item)
        actor.Inventory.Add(universe.Factory.CreateItem("FuelSupplyMarkII"))
        Dim Shipyard = map.GetLocation(1, 0).CreateActor("shipyard", "shipyard")
        actor.Yokes.Actor("Shipyard") = shipyard
        Dim group = universe.Factory.CreateGroup("group type", "group name")
        actor.Yokes.Group("Faction") = group
        universe.Avatar.SetActor(actor)
    End Sub
End Module
