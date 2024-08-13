Imports FHOS.Data

Friend Class OxygenRefilledDialog
    Inherits BaseDialog
    Private result As List(Of (Hue As Integer, Text As String)) = Nothing

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(DialogType.Menu, actor, finalDialog, Nothing, Nothing)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            If result Is Nothing Then
                Dim oxygenRequired = Actor.LifeSupport.TopOffAmount.Value
                Const oxygenPerJools = 10
                Dim oxygenCost = (oxygenRequired + oxygenPerJools - 1) \ oxygenPerJools
                Actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= oxygenCost
                Actor.LifeSupport.CurrentValue += oxygenRequired
                result = New List(Of (Hue As Integer, Text As String)) From
                    {
                        (Hues.Orange, "Oxygen Refilled!"),
                        (Hues.LightGray, $"You buy {oxygenRequired} oxygen!"),
                        (Hues.LightGray, $"Cost: {oxygenCost} Jools!")
                    }
            End If
            Return result
        End Get
    End Property

    Private ReadOnly Property LegacyChoices As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Get
            Return New Dictionary(Of String, Func(Of IDialog)) From {{DialogChoices.Ok, AddressOf EndDialog}}
        End Get
    End Property

    Public Overrides ReadOnly Property Menu As IEnumerable(Of String)
        Get
            Return LegacyChoices.Keys
        End Get
    End Property

    Public Overrides Function Choose(choice As String) As IDialog
        Dim value As Func(Of IDialog) = Nothing
        If LegacyChoices().TryGetValue(choice, value) Then
            Return value()
        End If
        Return Me
    End Function
End Class
