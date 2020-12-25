Module Util
    Friend Function CrearLabel(coordenadas As Point) As Label
        Return New Label With {
            .Size = New Size(Paso, Paso),
            .AutoSize = False,
            .Location = coordenadas,
            .BorderStyle = BorderStyle.FixedSingle
        }
    End Function
End Module
