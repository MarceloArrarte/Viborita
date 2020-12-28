Imports Viborita.Util

Public Class Viborita
    Public Enum TiposDireccion
        Arriba
        Derecha
        Abajo
        Izquierda
    End Enum

    Public Property SeccionesCuerpo As New List(Of Label)       ' Almacena las secciones de la viborita

    Public Property Cabeza As Label     ' Accede a la primera sección de la viborita
        Get
            Return SeccionesCuerpo(0)
        End Get
        Set(value As Label)
            SeccionesCuerpo(0) = value
        End Set
    End Property

    Public Property Cola As Label       ' Accede a la última sección de la viborita
        Get
            Return SeccionesCuerpo(SeccionesCuerpo.Count - 1)
        End Get
        Set(value As Label)
            SeccionesCuerpo(SeccionesCuerpo.Count - 1) = value
        End Set
    End Property

    Private _direccion As TiposDireccion = TiposDireccion.Arriba        ' Dirección en la que se mueve la viborita actualmente.
    Public Property Direccion As TiposDireccion
        Get
            Return _direccion
        End Get
        Set(value As TiposDireccion)
            ' Según la dirección ingresada, verifica si el giro es válido
            ' (ej. sólo puede girar hacia arriba si viene moviéndose hacia la izquierda o la derecha).
            ' Si la dirección no resulta en un giro válido, se ignora.
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

    ' Almacena la posición de la cola antes de moverse, en caso de
    ' haber comido esta es la posición donde se añade la nueva sección.
    Private Property PosicionParaCrecer As Point

    ' Inicializa la viborita con un único Label centrado en el área de juego.
    Public Sub New(areaDeJuego As Size)
        Direccion = TiposDireccion.Arriba
        Dim posicionInicial As New Point((areaDeJuego.Width - Paso) / 2, (areaDeJuego.Height - Paso) / 2)
        AgregarSeccion(posicionInicial)
    End Sub

    ' Función que desplaza la viborita. Retorna True si el movimiento es válido o False si la cabeza choca con alguna sección del cuerpo.
    Public Function Moverse() As Boolean
        PosicionParaCrecer = Cola.Location

        ' Mueve cada sección del cuerpo a la ubicación de la siguiente sección más próxima a la cabeza.
        For i = SeccionesCuerpo.Count - 1 To 1 Step -1
            Dim seccion As Label = SeccionesCuerpo(i)
            seccion.Location = SeccionesCuerpo(i - 1).Location
        Next

        ' Mueve la cabeza de acuerdo a la dirección de movimiento guardada.
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

        ' Se retorna que el movimiento es válido si no choca consigo
        Return Not ChocaConsigo()
    End Function

    ' Agrega una sección a la viborita en la posición que tenía la cola antes de moverse.
    Public Sub Crecer()
        AgregarSeccion(PosicionParaCrecer)
    End Sub

    ' Agrega una sección a la viborita con las coordenadas indicadas
    ' y el nombre "Viborita" & el índice que le corresponde en la lista.
    Private Sub AgregarSeccion(coordenadas As Point)
        Dim lbl As Label = CrearLabel(coordenadas)
        lbl.BackColor = Color.MediumAquamarine
        lbl.Name = "Viborita" & SeccionesCuerpo.Count - 1
        SeccionesCuerpo.Add(lbl)
    End Sub

    ' Retorna True si se detecta que la cabeza se solapa con alguna sección del cuerpo, sino retorna False.
    Private Function ChocaConsigo() As Boolean
        For i = 4 To SeccionesCuerpo.Count - 1          ' No es posible chocarse consigo si hay 4 o menos secciones
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
