Imports FHOS.Data
Imports FHOS.Persistence

Friend Class AtmosphericConcentratorItemTypeDescriptor
    Inherits TechLevelItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.AtmosphericConcentrator,
            "AeroSynth Recharger",
            New Dictionary(Of String, Double),
            3,
            price:=500,
            installFee:=25,
            uninstallFee:=15)
    End Sub
    Friend Overrides Function CanEquip(equipSlot As String) As Boolean
        Return EquipSlots.Descriptors(equipSlot).Category = EquipSlotCategories.AccessoryCategory
    End Function

    Public Overrides ReadOnly Property Description(item As IItem) As IEnumerable(Of String)
        Get
            Return {
                "Discover the Future of Planetary Exploration with the AeroSynth Recharger by StarBreathe Technologies",
                "Imagine landing on a new world, breathing in the untouched air, and knowing that your life support system will never run out of fresh, breathable atmosphere. With the AeroSynth Recharger, this is no longer a dream—it's your new reality.",
                "The AeroSynth Recharger is a cutting-edge device engineered by the brilliant minds at StarBreathe Technologies. Designed for explorers, colonists, and spacefarers, the AeroSynth Recharger effortlessly extracts and refines atmospheric elements from any planet, converting them into life-sustaining air for your entire crew.",
                "Compact yet powerful, the AeroSynth Recharger seamlessly integrates with your existing life support systems, recharging them with the perfect blend of gases tailored to human needs. Whether you're on a long-term mission or a short reconnaissance, the AeroSynth Recharger ensures that every breath you take is fresh, clean, and revitalizing.",
                "With StarBreathe Technologies, exploration knows no bounds. Trust the AeroSynth Recharger to keep you breathing easy, wherever your journey takes you.",
                $"Tech Level: {item.Statistics(StatisticTypes.TechLevel)}"}
        End Get
    End Property

    Protected Overrides Sub Initialize(item As IItem)
        item.Flags(FlagTypes.CanRefillOxygen) = True
        item.Statistics(StatisticTypes.TechLevel) = TechLevel
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog)
    End Function
End Class
