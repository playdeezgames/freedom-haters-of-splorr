Imports FHOS.Model
Imports SPLORR.UI

Public Class FHOSController
    Inherits BaseGameController(Of IWorldModel)

    Public Sub New(settings As ISettings, context As IUIContext(Of IWorldModel))
        MyBase.New(settings, context)
        SetCurrentState(BoilerplateState.Splash, True)
    End Sub
End Class
