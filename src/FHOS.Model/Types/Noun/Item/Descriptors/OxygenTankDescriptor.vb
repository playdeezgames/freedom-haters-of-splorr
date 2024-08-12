Imports FHOS.Data
Imports FHOS.Persistence

Friend Class OxygenTankDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.OxygenTank,
            "Oxygen Tank",
            New Dictionary(Of String, Double),
            price:=5)
    End Sub

    Public Overrides ReadOnly Property Description(item As IItem) As IEnumerable(Of String)
        Get
            Return {"This item can be used to replenish a vessel's oxygen."}
        End Get
    End Property

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.Oxygen) = 100
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog) From
            {
                {DialogChoices.RefillOxygen, New UseOxygenTankDialog(actor, item, Nothing)}
            }
    End Function
End Class
