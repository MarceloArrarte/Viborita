Public Class Viborita
    Public Enum TiposDireccion
        Arriba
        Derecha
        Abajo
        Izquierda
    End Enum

    Public Property SeccionesCuerpo As New List(Of Label)
    Public Property Cabeza As Label
        Get
            Return SeccionesCuerpo(0)
        End Get
        Set(value As Label)
            SeccionesCuerpo(0) = value
        End Set
    End Property
    Public Property Cola As Label
        Get
            Return SeccionesCuerpo(SeccionesCuerpo.Count - 1)
        End Get
        Set(value As Label)
            SeccionesCuerpo(SeccionesCuerpo.Count - 1) = value
        End Set
    End Property

    Private _direccion As TiposDireccion
    Public Property Direccion As TiposDireccion
        Get
            Return _direccion
        End Get
        Set(value As TiposDireccion)
            Select Case value
                Case TiposDireccion.Arriba
                    If Direccion = TiposDireccion.Izquierda Or Direccion = TiposDireccion.Derecha Then
                        _direccion = TiposDireccion.Arriba
                    End If
                Case TiposDireccion.Izquierda
                    If Direccion = TiposDireccion.Abajo Or Direccion = TiposDireccion.Arriba Then
                        _direccion = TiposDireccion.Izquierda
                    End If
                Case TiposDireccion.Abajo
                    If Direccion = TiposDireccion.Derecha Or Direccion = TiposDireccion.Izquierda Then
                        _direccion = TiposDireccion.Abajo
                    End If
                Case TiposDireccion.Derecha
                    If Direccion = TiposDireccion.Arriba Or Direccion = TiposDireccion.Abajo Then
                        _direccion = TiposDireccion.Derecha
                    End If
            End Select
        End Set
    End Property

    Private Property PosicionParaCrecer As Point

    Public Sub New(areaDeJuego As Size)
        Direccion = TiposDireccion.Arriba
        Dim posicionInicial As New Point((areaDeJuego.Width - Paso) / 2, (areaDeJuego.Height - Paso) / 2)
        SeccionesCuerpo.Add(CrearSeccion(posicionInicial))
    End Sub

    Public Function Moverse() As Boolean
        PosicionParaCrecer = Cola.Location

        For i = SeccionesCuerpo.Count - 1 To 1 Step -1
            Dim seccion As Label = SeccionesCuerpo(i)
            seccion.Location = SeccionesCuerpo(i - 1).Location
        Next

        Select Case Direccion
            Case TiposDireccion.Arriba
                Cabeza.Location = New Point(Cabeza.Location.X, Cabeza.Location.Y - Paso)
            Case TiposDireccion.Derecha
                Cabeza.Location = New Point(Cabeza.Location.X + Paso, Cabeza.Location.Y)
            Case TiposDireccion.Abajo
                Cabeza.Location = New Point(Cabeza.Location.X, Cabeza.Location.Y + Paso)
            Case TiposDireccion.Izquierda
                Cabeza.Location = New Point(Cabeza.Location.X - Paso, Cabeza.Location.Y)
        End Select

        Return Not ChocaConsigo()
    End Function

    Public Sub Crecer()
        SeccionesCuerpo.Add(CrearSeccion(PosicionParaCrecer))
    End Sub

    Private Function CrearSeccion(coordenadas As Point) As Label
        Dim lbl As Label = CrearLabel(coordenadas)
        lbl.BackColor = Color.MediumAquamarine
        lbl.Name = "Viborita" & SeccionesCuerpo.Count - 1
        Return lbl
    End Function

    Private Function ChocaConsigo() As Boolean
        For i = 4 To SeccionesCuerpo.Count - 1
            Dim seccion As Label = SeccionesCuerpo(i)
            Dim rectanguloSeccion As New Rectangle(seccion.Location, seccion.Size)
            Dim rectanguloCabeza As New Rectangle(Cabeza.Location, Cabeza.Size)
            If rectanguloCabeza.IntersectsWith(rectanguloSeccion) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
