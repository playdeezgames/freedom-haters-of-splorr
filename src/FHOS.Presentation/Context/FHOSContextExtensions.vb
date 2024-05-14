Imports System.Runtime.CompilerServices
Imports FHOS.Model
Imports SPLORR.UI

Friend Module FHOSContextExtensions
    <Extension>
    Friend Function ChooseOrCancel(context As IUIContext(Of IUniverseModel)) As String
        Return context.ControlsText(aButton:="Choose", bButton:="Cancel")
    End Function
End Module
