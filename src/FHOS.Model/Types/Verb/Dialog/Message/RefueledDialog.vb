Imports FHOS.Data

Friend Class RefueledDialog
    Inherits BaseSideEffectDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            If result Is Nothing Then
                Dim fuelRequired = Actor.FuelTank.TopOffAmount.Value
                Const fuelPerJools = 3
                Dim fuelCost = (fuelRequired + fuelPerJools - 1) \ fuelPerJools
                Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= fuelCost
                Actor.FuelTank.CurrentValue += fuelRequired
                result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Refueled!"),
                        (Hues.LightGray, $"You bought {fuelRequired} fuel."),
                        (Hues.LightGray, $"You paid {fuelCost} Jools.")
                    }
            End If
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property Choices As IEnumerable(Of (Text As String, Value As Func(Of IDialog)))
        Get
            Return {(DialogChoices.Ok, AddressOf EndDialog)}
        End Get
    End Property
End Class
