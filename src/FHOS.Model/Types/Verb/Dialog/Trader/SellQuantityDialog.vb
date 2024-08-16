Imports FHOS.Data

Friend Class SellQuantityDialog
    Inherits BaseInteractorMenuDialog

    Private ReadOnly itemType As String

    Public Sub New(
                  actor As Persistence.IActor,
                  interactor As Persistence.IActor,
                  itemType As String,
                  finalDialog As IDialog)
        MyBase.New(
            actor,
            interactor,
            finalDialog,
            "Sell How Many?")
        Me.itemType = itemType
    End Sub

    Protected Overrides Function InitializeMenu() As IReadOnlyDictionary(Of String, Func(Of IDialog))
        Dim quantity As Integer = GetQuantity()
        Dim menu As New Dictionary(Of String, Func(Of IDialog)) From {
            {DialogChoices.Cancel, AddressOf EndDialog},
            {"One(1)", SellQuantity(1)},
            {$"All({quantity})", SellQuantity(quantity)}
        }
        If quantity > 3 Then
            menu.Add($"Half({quantity \ 2})", SellQuantity(quantity \ 2))
        End If
        If quantity > 2 Then
            menu.Add("Specific Quantity...", SellSpecificQuantity(quantity))
        End If
        Return menu
    End Function

    Private Function SellSpecificQuantity(quantity As Integer) As Func(Of IDialog)
        Return Function() New SellSpecificQuantityDialog(actor, interactor, itemType, quantity, finalDialog)
    End Function

    Private Function SellQuantity(quantity As Integer) As Func(Of IDialog)
        Return Function() New SellConfirmDialog(actor, interactor, itemType, quantity, finalDialog)
    End Function

    Private Function GetQuantity() As Integer
        Return actor.Inventory.ItemCountOfType(itemType)
    End Function

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Dim lines As New List(Of (Hue As Integer, Text As String))
        Return lines
    End Function
End Class
