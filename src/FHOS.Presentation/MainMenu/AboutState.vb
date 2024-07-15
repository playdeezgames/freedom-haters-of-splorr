Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class AboutState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        For Each aboutLine In Messages.AboutLines
            ui.WriteLine(aboutLine)
        Next
        ui.Message((Mood.Info, String.Empty))
        Return endState
    End Function
End Class
