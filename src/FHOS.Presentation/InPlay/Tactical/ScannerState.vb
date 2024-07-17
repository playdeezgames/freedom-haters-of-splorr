Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ScannerState
    Inherits BoardState
    Implements IState

    Private ReadOnly cursor As (X As Integer, Y As Integer)

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, cursor As (X As Integer, Y As Integer))
        MyBase.New(model, ui, endState)
        Me.cursor = cursor
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.Write((Mood.Prompt, "Arrows: "))
        ui.Write((Mood.Info, "Move | "))
        ui.Write((Mood.Prompt, "Tab: "))
        ui.WriteLine((Mood.Info, "Navigation"))
        RenderBoard(cursor)
        Dim key = ui.ReadKey()
        Select Case key
            Case KeyNames.Tab
                Return New NeutralState(model, ui, endState)
            Case KeyNames.UpArrow
                Return New ScannerState(model, ui, endState, (cursor.X, Math.Max(-ViewRows \ 2, cursor.Y - 1)))
            Case KeyNames.DownArrow
                Return New ScannerState(model, ui, endState, (cursor.X, Math.Min(ViewRows - ViewRows \ 2, cursor.Y + 1)))
            Case KeyNames.LeftArrow
                Return New ScannerState(model, ui, endState, (Math.Max(-ViewColumns \ 2, cursor.X - 1), cursor.Y))
            Case KeyNames.RightArrow
                Return New ScannerState(model, ui, endState, (Math.Min(ViewColumns - ViewColumns \ 2, cursor.X + 1), cursor.Y))
            Case Else
                Return New ScannerState(model, ui, endState, cursor)
        End Select
    End Function
End Class
