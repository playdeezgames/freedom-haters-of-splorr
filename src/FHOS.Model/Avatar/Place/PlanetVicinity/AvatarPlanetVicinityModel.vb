﻿Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarPlanetVicinityModel
    Inherits AvatarPlaceModel
    Implements IAvatarPlanetVicinityModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property LegacyCurrent As IPlanetVicinityModel Implements IAvatarPlanetVicinityModel.LegacyCurrent
        Get
            Dim planet = avatar.Location.Place
            If planet IsNot Nothing Then
                Return New PlanetVicinityModel(planet)
            End If
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property CanApproach As Boolean Implements IAvatarPlanetVicinityModel.CanApproach
        Get
            Return LegacyCurrent IsNot Nothing
        End Get
    End Property

    Public Sub Approach() Implements IAvatarPlanetVicinityModel.Approach
        If CanApproach Then
            DoTurn()
            With avatar.Location.Place
                SetLocation(RNG.FromEnumerable(.Map.Locations.Where(Function(x) x.HasFlag(.Identifier))))
            End With
        End If
    End Sub
End Class