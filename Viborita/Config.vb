Public NotInheritable Class Config
    Private Sub New()
    End Sub

    ' Constantes para la configuración del juego
    Public Const Paso As Integer = 16
    Public Const IntervaloInicial As Integer = 250
    Public Const BaseExponencialVelocidad As Double = 1.06

    ' Colores utilizados en el juego
    Public Shared ReadOnly ColorComida As Color = Color.DarkRed
    Public Shared ReadOnly ColorViborita As Color = Color.MediumAquamarine
End Class
