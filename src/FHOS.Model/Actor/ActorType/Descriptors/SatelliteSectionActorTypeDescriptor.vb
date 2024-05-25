Imports System.Data

Friend Class SatelliteSectionActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Private ReadOnly subType As String
    Private ReadOnly sectionName As String

    Public Sub New(satelliteType As String, sectionName As String)
        MyBase.New(
            ActorTypes.MakeSatelliteSection(satelliteType, sectionName),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeSatelliteSection(satelliteType, sectionName), 1}
            },
            New Dictionary(Of String, String))
        Me.subType = satelliteType
        Me.sectionName = sectionName
    End Sub

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        actor.Properties.IsSatelliteSection = True
        actor.Properties.Subtype = subtype
        actor.Properties.SectionName = sectionName
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {("That's no moon! That's a SPACE STATION!", Hues.Black)}
    End Function
End Class
