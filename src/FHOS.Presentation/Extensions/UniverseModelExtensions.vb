Imports System.IO
Imports System.Runtime.CompilerServices
Imports FHOS.Model

Friend Module UniverseModelExtensions
    <Extension>
    Public Sub LoadGame(model As IUniverseModel, slot As Integer)
        model.Load(SlotFilename(slot))
    End Sub

    <Extension>
    Public Sub SaveGame(model As IUniverseModel, slot As Integer)
        model.Save(SlotFilename(slot))
    End Sub

    <Extension>
    Public Function DoesSlotExist(model As IUniverseModel, slot As Integer) As Boolean
        Return File.Exists(SlotFilename(slot))
    End Function

    Private ReadOnly SlotFilename As IReadOnlyDictionary(Of Integer, String) =
        New Dictionary(Of Integer, String) From
        {
            {0, "scum.json"},
            {1, "slot1.json"},
            {2, "slot2.json"},
            {3, "slot3.json"},
            {4, "slot4.json"},
            {5, "slot5.json"}
        }
    <Extension>
    Public Function LoadableSlots(model As IUniverseModel) As IEnumerable(Of Integer)
        Return SlotFilename.Keys.Where(Function(x) DoesSlotExist(model, x))
    End Function
End Module
