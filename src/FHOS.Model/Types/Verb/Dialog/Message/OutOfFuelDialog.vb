Imports FHOS.Data

Friend Class OutOfFuelDialog
    Inherits BaseDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(DialogType.Menu, actor, finalDialog)
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            Return {
                (Hues.Orange, "Out of Fuel!"),
                (Hues.LightGray, "You are out of fuel!"),
                (Hues.LightGray, ""),
                (Hues.LightGray, "To send a distress signal,"),
                (Hues.LightGray, "press [Space/Enter] from the NAV SCREEN"),
                (Hues.LightGray, "then choose 'Distress Signal'")
                }
        End Get
    End Property

    Private ReadOnly Property LegacyChoices As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Get
            Return New Dictionary(Of String, Func(Of IDialog)) From {
                {DialogChoices.Ok, AddressOf EndDialog}
                }
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
