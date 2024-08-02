Imports System.IO
Imports System.Runtime.CompilerServices
Imports FHOS.Model

Friend Module UniverseModelExtensions
    <Extension>
    Public Sub LoadGame(model As IUniverseModel, slot As Integer)
        model.SaveState.Load(SlotDetails(slot).Filename)
    End Sub

    <Extension>
    Public Sub SaveGame(model As IUniverseModel, slot As Integer)
        model.SaveState.Save(SlotDetails(slot).Filename)
    End Sub

    <Extension>
    Public Function DoesSlotExist(model As IUniverseModel, slot As Integer) As Boolean
        Return File.Exists(SlotDetails(slot).Filename)
    End Function

    Private ReadOnly SlotDetails As IReadOnlyDictionary(Of Integer, (Filename As String, Name As String)) =
        New Dictionary(Of Integer, (Filename As String, Name As String)) From
        {
            {0, ("scum.json", "Scum Slot")},
            {1, ("slot1.json", "Slot 1")},
            {2, ("slot2.json", "Slot 2")},
            {3, ("slot3.json", "Slot 3")},
            {4, ("slot4.json", "Slot 4")},
            {5, ("slot5.json", "Slot 5")}
        }
    <Extension>
    Public Function GetSlotName(model As IUniverseModel, slot As Integer) As String
        Return SlotDetails(slot).Name
    End Function
    <Extension>
    Public Function LoadableSlots(model As IUniverseModel) As IEnumerable(Of Integer)
        Return SlotDetails.Keys.Where(Function(x) DoesSlotExist(model, x))
    End Function
    <Extension>
    Public Function AvailableSlots(model As IUniverseModel) As IEnumerable(Of Integer)
        Return SlotDetails.Keys.Where(Function(x) x > 0)
    End Function
End Module
