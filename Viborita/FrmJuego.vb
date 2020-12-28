Imports Viborita.Viborita
Imports Viborita.Comida

Public Class FrmJuego
    ' Variables usadas en el transcurso del juego
    Private Property JuegoEnCurso As Boolean = False

    Private _puntaje As Integer = 0
    Private Property Puntaje As Integer
        Get
            Return _puntaje
        End Get
        Set(value As Integer)       ' Actualiza Label con el puntaje y aumenta la velocidad de desplazamiento
            _puntaje = value
            lblPuntaje.Text = "Puntaje: " & Puntaje
            ActualizarVelocidad()
        End Set
    End Property
    Private Property MovimientoAcelerado As Boolean = False
    Private Property YaMovioEnEsteIntervalo As Boolean = False

    ' Referencias a objetos importantes en el juego
    Private Property MiViborita As Viborita
    Private Property MiComida As Label

    Private Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click
        pnlCampoDeJuego.Controls.Clear()
        MiViborita = New Viborita(pnlCampoDeJuego.Size)
        Comida.AreaDeJuego = pnlCampoDeJuego.Size       ' Área donde generar la comida
        tmrMovimiento.Interval = IntervaloInicial       ' Velocidad inicial de desplazamiento

        RefrescarViborita()
        GenerarComida()

        ActiveControl = Nothing
        btnIniciar.Visible = False
        lblPuntaje.Visible = True
        Puntaje = 0
        tmrMovimiento.Enabled = True
    End Sub

    Private Sub tmrMovimiento_Tick(sender As Object, e As EventArgs) Handles tmrMovimiento.Tick
        YaMovioEnEsteIntervalo = False

        ' Se pierde si la viborita no realiza un movimiento válido (choca consigo) o choca con un borde
        Dim perdio As Boolean = Not MiViborita.Moverse Or ViboritaChocaConBordes()
        If Not perdio Then
            If ComioComida() Then
                Puntaje += 1
                GenerarComida()
                MiViborita.Crecer()
                RefrescarViborita()
            End If
        Else
            RefrescarViborita()
            tmrMovimiento.Enabled = False
            btnIniciar.Visible = True
            btnIniciar.Text = "Reiniciar"
            MsgBox("¡Se acabó el juego!" & vbNewLine & "Puntaje: " & Puntaje, MsgBoxStyle.Information, "¡Gran partida!")
        End If
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        tmrMovimiento.Enabled = False
        If MsgBox("¿Deseas salir del juego?", MsgBoxStyle.YesNo, "Salir") = MsgBoxResult.Yes Then
            Close()
        Else
            tmrMovimiento.Enabled = True
        End If
    End Sub

    ' Si se presiona una flecha de navegación, se informa que debe ser considerado como entrada y no como tecla de navegación.
    Private Sub FrmJuego_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles MyBase.PreviewKeyDown
        Select Case e.KeyCode
            Case Keys.Up, Keys.Left, Keys.Down, Keys.Right
                e.IsInputKey = True
        End Select
    End Sub

    Private Sub Direcciones_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Evita que el jugador dé un giro de 180° presionando dos flechas entre un movimiento y el siguiente
        If JuegoEnCurso And Not YaMovioEnEsteIntervalo Then
            YaMovioEnEsteIntervalo = True               ' Se asume que la tecla de movimiento es válida.
            Dim direccionPrevia As TiposDireccion = MiViborita.Direccion        ' Almacena la dirección previa al cambio
            Select Case e.KeyCode
                Case Keys.W, Keys.Up
                    MiViborita.Direccion = TiposDireccion.Arriba
                    e.Handled = True
                Case Keys.A, Keys.Left
                    MiViborita.Direccion = TiposDireccion.Izquierda
                    e.Handled = True
                Case Keys.S, Keys.Down
                    MiViborita.Direccion = TiposDireccion.Abajo
                    e.Handled = True
                Case Keys.D, Keys.Right
                    MiViborita.Direccion = TiposDireccion.Derecha
                    e.Handled = True
            End Select

            If direccionPrevia = MiViborita.Direccion Then          ' Si la dirección previa es la misma a la actual
                YaMovioEnEsteIntervalo = False                      ' (ej. va hacia arriba y presiona W), se ignora el movimiento
            End If
        End If
    End Sub

    ' Duplica la velocidad de movimiento
    Private Sub Shift_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If JuegoEnCurso Then
            If e.KeyCode = Keys.ShiftKey Then
                MovimientoAcelerado = True
                ActualizarVelocidad()
            End If
        End If
    End Sub

    ' Vuelve la velocidad a la normal
    Private Sub Shift_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If JuegoEnCurso Then
            If e.KeyCode = Keys.ShiftKey Then
                MovimientoAcelerado = False
                ActualizarVelocidad()
            End If
        End If
    End Sub

    Private Sub btnIniciar_VisibleChanged(sender As Object, e As EventArgs) Handles btnIniciar.VisibleChanged
        JuegoEnCurso = Not btnIniciar.Visible
    End Sub

    ' Evita que el botón consiga el foco y capture la entrada de las flechas de navegación.
    Private Sub btnSalir_GotFocus(sender As Object, e As EventArgs) Handles btnSalir.GotFocus
        ActiveControl = Nothing
    End Sub

    ' Actualiza la posición de cada sección de la viborita en el juego
    Private Sub RefrescarViborita()
        For i = 0 To MiViborita.SeccionesCuerpo.Count - 1       ' Para cada sección que tiene la viborita
            Dim lbl As Label = Controls.Find("Viborita" & i, False).SingleOrDefault     ' Busca el Label cuyo nombre es "Viborita" & el índice de                                                                             la colección que recorre
            If lbl Is Nothing Then                                  ' Si no lo encontró, la viborita creció y agregó una nueva sección
                pnlCampoDeJuego.Controls.Add(MiViborita.Cola)       ' (nueva cola) que hay que agregar a los controles.
            End If
        Next
    End Sub

    ' Retorna True si la posición de la cabeza coincide con la de la comida
    Public Function ComioComida() As Boolean
        Dim rectanguloCabeza As New Rectangle(MiViborita.Cabeza.Location, MiViborita.Cabeza.Size)
        Dim rectanguloComida As New Rectangle(MiComida.Location, MiComida.Size)
        If rectanguloCabeza.IntersectsWith(rectanguloComida) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub GenerarComida()
        Do
            MiComida = CrearNueva()
        Loop While AreaComidaCoincideConViborita()      ' Crea un nuevo objeto hasta que no quede arriba de la viborita

        pnlCampoDeJuego.Controls.RemoveByKey("Comida")
        pnlCampoDeJuego.Controls.Add(MiComida)
    End Sub

    ' Retorna True si la posición de alguna sección de la viborita coincide con la de la comida
    Private Function AreaComidaCoincideConViborita() As Boolean
        Dim areaComida As New Rectangle(MiComida.Location, MiComida.Size)
        For Each s As Label In MiViborita.SeccionesCuerpo
            Dim areaSeccion As New Rectangle(s.Location, s.Size)
            If areaSeccion.IntersectsWith(areaComida) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function ViboritaChocaConBordes() As Boolean
        Return MiViborita.Cabeza.Location.X < 0 Or
            MiViborita.Cabeza.Location.X + Paso >= pnlCampoDeJuego.Width Or
            MiViborita.Cabeza.Location.Y < 0 Or
            MiViborita.Cabeza.Location.Y + Paso >= pnlCampoDeJuego.Height
    End Function

    ' Calcula la velocidad de desplazamiento de la viborita. Si Shift está presionado MovimientoAcelerado es True, y se duplica la velocidad.
    Private Sub ActualizarVelocidad()
        Dim factorVelocidad As Double = BaseExponencialVelocidad ^ Puntaje
        Dim intervalo As Double = IntervaloInicial / factorVelocidad
        If MovimientoAcelerado Then intervalo /= 2
        tmrMovimiento.Interval = intervalo
    End Sub
End Class
