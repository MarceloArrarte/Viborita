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
        Set(value As Integer)
            _puntaje = value
            lblPuntaje.Text = "Puntaje: " & Puntaje
            Dim factorVelocidad As Double = BaseExponencialVelocidad ^ Puntaje
            tmrMovimiento.Interval = IntervaloInicial / factorVelocidad
        End Set
    End Property
    Private Property YaMovioEnEsteIntervalo As Boolean = False

    ' Referencias a objetos importantes en el juego
    Private Property Viborita As Viborita
    Private Property Comida As Label

    Private Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click
        pnlCampoDeJuego.Controls.Clear()
        Viborita = New Viborita(pnlCampoDeJuego.Size)
        Viborita.Direccion = TiposDireccion.Arriba
        tmrMovimiento.Interval = IntervaloInicial

        RefrescarViborita()
        GenerarComida()

        tmrMovimiento.Enabled = True
        ActiveControl = Nothing
        btnIniciar.Visible = False
        lblPuntaje.Visible = True
        Puntaje = 0
    End Sub

    Private Sub tmrMovimiento_Tick(sender As Object, e As EventArgs) Handles tmrMovimiento.Tick
        YaMovioEnEsteIntervalo = False

        Dim perdio As Boolean = Not Viborita.Moverse Or ViboritaChocaConBordes()
        If Not perdio Then
            If ComioComida() Then
                Puntaje += 1
                GenerarComida()
                Viborita.Crecer()
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

    Private Function ViboritaChocaConBordes() As Boolean
        Return Viborita.Cabeza.Location.X < 0 Or
            Viborita.Cabeza.Location.X + Paso > pnlCampoDeJuego.Width Or
            Viborita.Cabeza.Location.Y < 0 Or
            Viborita.Cabeza.Location.Y + Paso > pnlCampoDeJuego.Height
    End Function

    Public Function ComioComida() As Boolean
        Dim rectanguloCabeza As New Rectangle(Viborita.Cabeza.Location, Viborita.Cabeza.Size)
        Dim rectanguloComida As New Rectangle(Comida.Location, Comida.Size)
        If rectanguloCabeza.IntersectsWith(rectanguloComida) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub RefrescarViborita()
        For i = 0 To Viborita.SeccionesCuerpo.Count - 1
            Dim lbl As Label = Controls.Find("Viborita" & i, False).SingleOrDefault
            If lbl Is Nothing Then
                pnlCampoDeJuego.Controls.Add(Viborita.Cola)
            Else
                lbl = Viborita.SeccionesCuerpo(i)
            End If
        Next
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        tmrMovimiento.Enabled = False
        If MsgBox("¿Deseas salir del juego?", MsgBoxStyle.YesNo, "Salir") = MsgBoxResult.Yes Then
            Close()
        Else
            tmrMovimiento.Enabled = True
        End If
    End Sub

    Private Sub FrmJuego_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles MyBase.PreviewKeyDown
        Select Case e.KeyCode
            Case Keys.Up, Keys.Left, Keys.Down, Keys.Right
                e.IsInputKey = True
        End Select
    End Sub

    Private Sub FrmJuego_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If JuegoEnCurso And Not YaMovioEnEsteIntervalo Then
            YaMovioEnEsteIntervalo = True
            Select Case e.KeyCode
                Case Keys.W, Keys.Up
                    Viborita.Direccion = TiposDireccion.Arriba
                    e.Handled = True
                Case Keys.A, Keys.Left
                    Viborita.Direccion = TiposDireccion.Izquierda
                    e.Handled = True
                Case Keys.S, Keys.Down
                    Viborita.Direccion = TiposDireccion.Abajo
                    e.Handled = True
                Case Keys.D, Keys.Right
                    Viborita.Direccion = TiposDireccion.Derecha
                    e.Handled = True
                Case Else
                    YaMovioEnEsteIntervalo = False
            End Select
        End If
    End Sub

    Private Sub btnIniciar_VisibleChanged(sender As Object, e As EventArgs) Handles btnIniciar.VisibleChanged
        JuegoEnCurso = Not btnIniciar.Visible
    End Sub

    Private Sub btnSalir_GotFocus(sender As Object, e As EventArgs) Handles btnSalir.GotFocus
        ActiveControl = Nothing
    End Sub

    Private Sub GenerarComida()
        Comida = Nothing
        Do
            Comida = CrearNueva(pnlCampoDeJuego.Size)
        Loop While AreaComidaCoincideConViborita()

        pnlCampoDeJuego.Controls.RemoveByKey("Comida")
        pnlCampoDeJuego.Controls.Add(Comida)
    End Sub

    Private Function AreaComidaCoincideConViborita() As Boolean
        Dim areaComida As New Rectangle(Comida.Location, Comida.Size)
        For Each s As Label In Viborita.SeccionesCuerpo
            Dim areaSeccion As New Rectangle(s.Location, s.Size)
            If areaSeccion.IntersectsWith(areaComida) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
