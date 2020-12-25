Public Class Comida
    Private ReadOnly Property AreaDeJuego As Size

    Public Shared Function CrearNueva(areaDeJuego As Size) As Label
        Dim lbl As Label = CrearLabel(GenerarCoordenadasComida(areaDeJuego))
        With lbl
            .BackColor = Color.DarkRed
            .Name = "Comida"
        End With
        Return lbl
    End Function

    Private Shared Function GenerarCoordenadasComida(areaDeJuego As Size) As Point
        Dim x, y As Integer
        Static random As New Random
        x = random.Next(1, areaDeJuego.Width - Paso - 1)
        y = random.Next(1, areaDeJuego.Height - Paso - 1)
        Return New Point(x, y)
    End Function
End Class
