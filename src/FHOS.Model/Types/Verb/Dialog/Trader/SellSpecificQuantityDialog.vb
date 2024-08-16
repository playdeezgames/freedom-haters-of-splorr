Imports FHOS.Data

Friend Class SellSpecificQuantityDialog
    Inherits BaseDialog

    Private ReadOnly interactor As Persistence.IActor
    Private ReadOnly itemType As String
    Private ReadOnly maximumQuantity As Integer

    Public Sub New(
                  actor As Persistence.IActor,
                  interactor As Persistence.IActor,
                  itemType As String,
                  maximumQuantity As Integer,
                  finalDialog As IDialog)
        MyBase.New(
            DialogType.Input,
            actor,
            finalDialog,
            "How Many?",
            "0")
        Me.interactor = interactor
        Me.itemType = itemType
        Me.maximumQuantity = maximumQuantity
    End Sub

    Public Overrides ReadOnly Property Lines As IEnumerable(Of (Hue As Integer, Text As String))
        Get
            Return Array.Empty(Of (Hue As Integer, Text As String))
        End Get
    End Property

    Public Overrides ReadOnly Property Menu As IEnumerable(Of String)
        Get
            Return Array.Empty(Of String)
        End Get
    End Property

    Public Overrides Function Choose(choice As String) As IDialog
        Dim quantity As Integer = 0
        If Not Integer.TryParse(choice, quantity) OrElse quantity <= 0 Then
            Return finalDialog
        End If
        quantity = Math.Min(quantity, maximumQuantity)
        Return New SellConfirmDialog(actor, interactor, itemType, quantity, finalDialog)
    End Function
End Class
