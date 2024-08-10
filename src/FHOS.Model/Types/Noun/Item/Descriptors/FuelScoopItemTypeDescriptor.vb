Imports FHOS.Data
Imports FHOS.Persistence

Friend Class FuelScoopItemTypeDescriptor
    Inherits TechLevelItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.FuelScoop,
            "Fuel Scoop",
            7,
            price:=10000,
            installFee:=100,
            uninstallFee:=50)
    End Sub

    Friend Overrides Function CanEquip(equipSlot As String) As Boolean
        Return EquipSlots.Descriptors(equipSlot).Category = EquipSlotCategories.AccessoryCategory
    End Function

    Public Overrides ReadOnly Property Description(item As IItem) As IEnumerable(Of String)
        Get
            Return {
                "Tap into the Power of the Stars with the SolarForge Extractor by HeliosDrive Industries",
                "Why settle for conventional fuel sources when you can harness the raw, untamed power of a star? The SolarForge Extractor is your gateway to limitless energy, revolutionizing the way you refuel in the vast expanse of space.",
                "Brought to you by HeliosDrive Industries, the pioneers of stellar energy technology, the SolarForge Extractor is designed for the boldest explorers and the most advanced fleets. This state-of-the-art device captures and condenses stellar energy directly from a star’s core, transforming it into a stable, high-density fuel ready for storage in your fuel systems.",
                "Compact, efficient, and incredibly powerful, the SolarForge Extractor allows you to refuel your vessels with ease, no matter where your adventures take you. Whether you're on the fringes of the galaxy or orbiting a distant sun, the SolarForge Extractor ensures you never run out of the energy you need to keep moving forward.",
                "With HeliosDrive Industries, you're not just exploring the stars—you’re harnessing them. Equip your fleet with the SolarForge Extractor and experience the true power of the cosmos.",
                $"Tech Level: {item.Statistics(StatisticTypes.TechLevel)}"}
        End Get
    End Property

    Protected Overrides Sub Initialize(item As IItem)
        item.Flags(FlagTypes.CanRefuel) = True
        item.Statistics(StatisticTypes.TechLevel) = TechLevel
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog)
    End Function
End Class
