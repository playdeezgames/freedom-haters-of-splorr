Imports FHOS.Data
Imports FHOS.Persistence

Friend Class FuelRodDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.FuelRod,
            "Fuel Rod",
            price:=20)
    End Sub

    Public Overrides ReadOnly Property Description(item As IItem) As String
        Get
            Return "You ram this into yer engine in order to fill it with fuel. No, there is nothing sexual about this. Not at all."
        End Get
    End Property

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.Fuel) = 100
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog) From
            {
                {DialogChoices.RefillFuel, New UseFuelRodDialog(actor, item, finalDialog)}
            }
    End Function
End Class
