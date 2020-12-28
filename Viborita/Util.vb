Public Class Util
    ' Se evita que se declaren instancias
    Private Sub New()
    End Sub

    ' Método compartido que devuelve un Label con todas las cualidades
    ' que comparten tanto la comida como los segmentos de la viborita
    Public Shared Function CrearLabel(coordenadas As Point) As Label
        Return New Label With {
            .Size = New Size(Paso, Paso),
            .AutoSize = False,
            .Location = coordenadas,
            .BorderStyle = BorderStyle.FixedSingle
        }
    End Function
End Class
