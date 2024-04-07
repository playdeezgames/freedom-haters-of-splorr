Imports FHOS.Model

Friend Module Glyphs
    Friend ReadOnly CharacterType As IReadOnlyDictionary(Of String, IEnumerable(Of (FontName As String, Indices As Integer(), Hue As Integer))) =
        New Dictionary(Of String, IEnumerable(Of (FontName As String, Indices As Integer(), Hue As Integer))) From
        {
            {CharacterTypes.Player, {("Sprites", {64, 65, 66, 67}, 7)}}
        }
    Friend ReadOnly TerrainType As IReadOnlyDictionary(Of String, IEnumerable(Of (FontName As String, Index As Integer, Hue As Integer))) =
        New Dictionary(Of String, IEnumerable(Of (FontName As String, Index As Integer, Hue As Integer))) From
        {
            {TerrainTypes.Void, {("Sprites", 63, 0)}}
        }
End Module
