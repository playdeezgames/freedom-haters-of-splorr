﻿Imports SPLORR.Presentation

Friend Module Messages
    Friend ReadOnly AboutLines As IReadOnlyList(Of (Mood, String)) =
        New List(Of (Mood, String)) From
        {
            (Mood.Info, "About Freedom Haters of SPLORR!!"),
            (Mood.Info, "A Production of TheGrumpyGameDev")
        }
    Friend Const Generating = "Generating..."
    Friend Const Status = "Status"
    Friend Const Scanner = "SCANNER"
    Friend Const InsufficientFunds = "Insufficient Funds."
End Module
