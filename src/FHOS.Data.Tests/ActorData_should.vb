﻿Public Class ActorData_should
    Inherits GroupedEntityData_should(Of ActorData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.AllEquipment.ShouldBeEmpty
        sut.AllItems.ShouldBeEmpty
        sut.HasOfferedItems.ShouldBeFalse
        sut.AllOfferedItems.ShouldBeEmpty
        sut.HasPricedItems.ShouldBeFalse
        sut.AllPricedItems.ShouldBeEmpty
    End Sub
    <Fact>
    Sub add_item()
        Dim sut = CreateSut()
        sut.AddInventoryItem(1)
        sut.AllItems.ShouldHaveSingleItem
    End Sub
    <Fact>
    Sub remove_item()
        Dim sut = CreateSut()
        sut.AddInventoryItem(1)
        sut.RemoveInventoryItem(1)
        sut.AllItems.ShouldBeEmpty
    End Sub
    <Fact>
    Sub default_given_yoked_item_to_nothing()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        sut.GetYokedItem(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub default_given_yoked_actor_to_nothing()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        sut.GetYokedActor(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub set_yoked_actor()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        Const actorId = 1
        sut.SetYokedActor(yokeType, actorId)
        sut.GetYokedActor(yokeType).ShouldBe(actorId)
    End Sub
    <Fact>
    Sub set_yoked_item()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        Const itemId = 1
        sut.SetYokedItem(yokeType, itemId)
        sut.GetYokedItem(yokeType).ShouldBe(itemId)
    End Sub
    <Fact>
    Sub clear_yoked_actor()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        Const actorId = 1
        sut.SetYokedActor(yokeType, actorId)
        sut.SetYokedActor(yokeType, Nothing)
        sut.GetYokedActor(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub default_given_yoked_store_to_nothing()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        sut.GetYokedStore(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub set_yoked_store()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        Const storeId = 1
        sut.SetYokedStore(yokeType, storeId)
        sut.GetYokedStore(yokeType).ShouldBe(storeId)
    End Sub
    <Fact>
    Sub clear_yoked_store()
        Dim sut = CreateSut()
        Const yokeType = "yoke type"
        Const storeId = 1
        sut.SetYokedStore(yokeType, storeId)
        sut.SetYokedStore(yokeType, Nothing)
        sut.GetYokedStore(yokeType).ShouldBeNull
    End Sub
    <Theory>
    <InlineData(" ")>
    <InlineData("  ")>
    <InlineData("")>
    <InlineData(Nothing)>
    Sub disallow_invalid_yoke_types(yokeType As String)
        Dim sut = CreateSut()
        Should.Throw(Of InvalidOperationException)(Function() sut.GetYokedActor(yokeType))
        Should.Throw(Of InvalidOperationException)(Sub() sut.SetYokedActor(yokeType, 1))
        Should.Throw(Of InvalidOperationException)(Function() sut.GetYokedStore(yokeType))
        Should.Throw(Of InvalidOperationException)(Sub() sut.SetYokedStore(yokeType, 1))
    End Sub
    <Fact>
    Sub add_offer()
        Dim sut = CreateSut()
        Const itemType = "item type"
        sut.AddOfferedItem(itemType)
        sut.HasOfferedItems.ShouldBeTrue
        sut.AllOfferedItems.ShouldHaveSingleItem
        sut.AllOfferedItems.Single.ShouldBe(itemType)
    End Sub
    <Fact>
    Sub add_price()
        Dim sut = CreateSut()
        Const itemType = "item type"
        sut.AddPricedItem(itemType)
        sut.HasPricedItems.ShouldBeTrue
        sut.AllPricedItems.ShouldHaveSingleItem
        sut.AllPricedItems.Single.ShouldBe(itemType)
    End Sub
    Protected Overrides Function CreateSut() As ActorData
        Return New ActorData With {.Id = 0}
    End Function
End Class
