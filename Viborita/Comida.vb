Imports Viborita.Util

Public Class Comida
    ' Propiedad que almacena el tamaño del campo de juego, para determinar
    ' los límites de las coordenadas de las comidas generadas
    Private Shared _areaDeJuego As Size
    Public Shared WriteOnly Property AreaDeJuego As Size
        Set(value As Size)
            _areaDeJuego = value
        End Set
    End Property

    ' Genera un Label, modifica las características del mismo de acuerdo
    ' a la comida y lo devuelve
    Public Shared Function CrearNueva() As Label
        Dim lbl As Label = CrearLabel(GenerarCoordenadasComida())
        With lbl
            .BackColor = ColorComida
            .Name = "Comida"
        End With
        Return lbl
    End Function

    ' Genera un par de coordenadas aleatorio según el área de juego almacenada.
    Private Shared Function GenerarCoordenadasComida() As Point
        Dim x, y As Integer
        Static random As New Random
        x = random.Next(1, _areaDeJuego.Width - Paso - 1)
        y = random.Next(1, _areaDeJuego.Height - Paso - 1)
        Return New Point(x, y)
    End Function
End Class
