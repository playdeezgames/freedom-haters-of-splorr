Imports FHOS.Persistence

Friend Class PlanetSectionActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Private ReadOnly subtype As String
    Private ReadOnly sectionName As String
    Private ReadOnly isPlanet As Boolean

    Public Sub New(planetType As String, sectionName As String, isPlanet As Boolean)
        MyBase.New(
            ActorTypes.MakePlanetSection(planetType, sectionName),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakePlanetSection(planetType, sectionName), 1}
            },
            New Dictionary(Of String, String))
        Me.subtype = planetType
        Me.sectionName = sectionName
        Me.isPlanet = isPlanet
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        actor.Properties.IsPlanet = isPlanet
        actor.Properties.IsPlanetSection = Not isPlanet
        actor.Properties.Subtype = subtype
        actor.Properties.SectionName = sectionName
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {("It's a planet! It's real big!", Hues.Black)}
    End Function
End Class
