Public Module Hues
    Public Const Black As Integer = 0
    Public Const Blue As Integer = 1
    Public Const Green As Integer = 2
    Public Const Cyan As Integer = 3
    Public Const Red As Integer = 4
    Public Const Purple As Integer = 5
    Public Const Brown As Integer = 6
    Public Const LightGray As Integer = 7
    Public Const DarkGray As Integer = 8
    Public Const LightBlue As Integer = 9
    Public Const LightGreen As Integer = 10
    Public Const Orange As Integer = 11
    Public Const Pink As Integer = 12
    Public Const Tan As Integer = 13
    Public Const Yellow As Integer = 14
    Public Const White As Integer = 15

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of Integer, HueDescriptor) =
        GenerateDescriptors().ToDictionary(Function(x) x.Hue, Function(x) x)

    Private Function GenerateDescriptors() As IReadOnlyList(Of HueDescriptor)
        Return New List(Of HueDescriptor) From
            {
                New HueDescriptor(Black, "Black"),
                New HueDescriptor(Blue, "Blue"),
                New HueDescriptor(Green, "Green"),
                New HueDescriptor(Cyan, "Cyan"),
                New HueDescriptor(Red, "Red"),
                New HueDescriptor(Purple, "Purple"),
                New HueDescriptor(Brown, "Brown"),
                New HueDescriptor(LightGray, "Light Gray"),
                New HueDescriptor(DarkGray, "Dark Gray"),
                New HueDescriptor(LightBlue, "Light Blue"),
                New HueDescriptor(LightGreen, "Light Green"),
                New HueDescriptor(Orange, "Orange"),
                New HueDescriptor(Pink, "Pink"),
                New HueDescriptor(Tan, "Tan"),
                New HueDescriptor(Yellow, "Yellow"),
                New HueDescriptor(White, "White")
            }
    End Function
    Public Function ForPercentage(percent As Integer) As Integer
        Return {Red, Yellow, Green}(Math.Clamp(percent, 0, 98) \ 33)
    End Function
End Module
