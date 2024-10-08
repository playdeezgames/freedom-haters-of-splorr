﻿Imports FHOS.Data

Friend Class OutOfFuelDialog
    Inherits BaseMessageMenuDialog

    Public Sub New(actor As Persistence.IActor, finalDialog As IDialog)
        MyBase.New(actor, finalDialog)
    End Sub

    Protected Overrides Function InitializeLines() As IEnumerable(Of (Hue As Integer, Text As String))
        Return {
                (Hues.Orange, "Out of Fuel!"),
                (Hues.LightGray, "You are out of fuel!"),
                (Hues.LightGray, ""),
                (Hues.LightGray, "To send a distress signal,"),
                (Hues.LightGray, "press [Space/Enter] from the NAV SCREEN"),
                (Hues.LightGray, "then choose 'Distress Signal'")
                }
    End Function
End Class
