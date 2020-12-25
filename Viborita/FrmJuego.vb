Public Class FrmJuego
    Private Enum TiposDireccion
        Arriba
        Derecha
        Abajo
        Izquierda
    End Enum

    ' Constantes para la configuración del juego
    Private Const Paso As Integer = 16
    Private Const IntervaloInicial As Integer = 250
    Private Const BaseExponencialVelocidad As Double = 1.06

    ' Variables usadas en el transcurso del juego
    Private Property Direccion As TiposDireccion = TiposDireccion.Arriba
    Private Property YaMovioEnEsteIntervalo As Boolean = False
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
            Label1.Text = "Intervalo: " & tmrMovimiento.Interval
        End Set
    End Property

    ' Referencias a objetos importantes en el juego
    Private Property SeccionesCuerpo As New List(Of Label)
    Private Property Cabeza As Label
    Private Property Cola As Label
    Private Property Comida As Label

    Private Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click
        pnlCampoDeJuego.Controls.Clear()
        SeccionesCuerpo.Clear()
        Direccion = TiposDireccion.Arriba
        tmrMovimiento.Interval = IntervaloInicial

        Cabeza = CrearSeccionCuerpo((pnlCampoDeJuego.Width - Paso) / 2, (pnlCampoDeJuego.Height - Paso) / 2)
        AgregarSeccionCuerpo(Cabeza)

        GenerarComida()

        tmrMovimiento.Enabled = True
        ActiveControl = Nothing
        btnIniciar.Visible = False
        lblPuntaje.Visible = True
        Puntaje = 0
    End Sub

    Private Sub tmrMovimiento_Tick(sender As Object, e As EventArgs) Handles tmrMovimiento.Tick
        YaMovioEnEsteIntervalo = False
        Cola = SeccionesCuerpo.Last
        Dim posicionCola As Point = Cola.Location

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

        If ComioComida() Then
            GenerarComida()
            AgregarSeccionCuerpo(CrearSeccionCuerpo(posicionCola.X, posicionCola.Y))
        End If

        If ChocaConBordes() Or ChocaConsigo() Then
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

    Private Sub Form1_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles MyBase.PreviewKeyDown
        Select Case e.KeyCode
            Case Keys.Up, Keys.Left, Keys.Down, Keys.Right
                e.IsInputKey = True
        End Select
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If JuegoEnCurso And Not YaMovioEnEsteIntervalo Then
            YaMovioEnEsteIntervalo = True
            Select Case e.KeyCode
                Case Keys.W, Keys.Up
                    If Direccion = TiposDireccion.Izquierda Or Direccion = TiposDireccion.Derecha Then
                        Direccion = TiposDireccion.Arriba
                    End If
                    e.Handled = True
                Case Keys.A, Keys.Left
                    If Direccion = TiposDireccion.Abajo Or Direccion = TiposDireccion.Arriba Then
                        Direccion = TiposDireccion.Izquierda
                    End If
                    e.Handled = True
                Case Keys.S, Keys.Down
                    If Direccion = TiposDireccion.Derecha Or Direccion = TiposDireccion.Izquierda Then
                        Direccion = TiposDireccion.Abajo
                    End If
                    e.Handled = True
                Case Keys.D, Keys.Right
                    If Direccion = TiposDireccion.Arriba Or Direccion = TiposDireccion.Abajo Then
                        Direccion = TiposDireccion.Derecha
                    End If
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

    Private Function ChocaConBordes() As Boolean
        Return Cabeza.Location.X < 0 Or
            Cabeza.Location.X + Paso > pnlCampoDeJuego.Width Or
            Cabeza.Location.Y < 0 Or
            Cabeza.Location.Y + Paso > pnlCampoDeJuego.Height
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

    Public Function ComioComida() As Boolean
        Dim rectanguloCabeza As New Rectangle(Cabeza.Location, Cabeza.Size)
        Dim rectanguloComida As New Rectangle(Comida.Location, Comida.Size)
        If rectanguloCabeza.IntersectsWith(rectanguloComida) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub AgregarSeccionCuerpo(seccion As Label)
        SeccionesCuerpo.Add(seccion)
        pnlCampoDeJuego.Controls.Add(seccion)
    End Sub

    Private Sub GenerarComida()
        If Comida IsNot Nothing Then
            Puntaje += 1
        End If

        Comida = CrearComida()

        pnlCampoDeJuego.Controls.RemoveByKey("Comida")
        pnlCampoDeJuego.Controls.Add(Comida)
    End Sub

    Private Function GenerarCoordenadasComida() As Point
        Dim x, y As Integer
        Dim areaDeJuego As Size = pnlCampoDeJuego.Size
        Static random As New Random
        Dim coordenadas As Point
        Dim rectanguloTentativoComida As Rectangle
        Do
            x = random.Next(1, areaDeJuego.Width - Paso - 1)
            y = random.Next(1, areaDeJuego.Height - Paso - 1)
            coordenadas = New Point(x, y)
            rectanguloTentativoComida = New Rectangle(coordenadas, New Size(Paso, Paso))
        Loop While AreaComidaCoincideConViborita(rectanguloTentativoComida)
        Return coordenadas
    End Function

    Private Function AreaComidaCoincideConViborita(areaComida As Rectangle) As Boolean
        For Each s As Label In SeccionesCuerpo
            Dim areaSeccion As New Rectangle(s.Location, s.Size)
            If areaSeccion.IntersectsWith(areaComida) Then
                tmrMovimiento.Enabled = False
                MsgBox("Colision")
                tmrMovimiento.Enabled = True
                Return True
            End If
        Next
        Return False
    End Function

    Private Function CrearLabel(coordenadas As Point) As Label
        Return New Label With {
            .Size = New Size(Paso, Paso),
            .AutoSize = False,
            .Location = coordenadas,
            .BorderStyle = BorderStyle.FixedSingle
        }
    End Function

    Private Function CrearComida() As Label
        Dim lbl As Label = CrearLabel(GenerarCoordenadasComida)
        With lbl
            .BackColor = Color.DarkRed
            .Name = "Comida"
        End With
        Return lbl
    End Function

    Private Function CrearSeccionCuerpo(posX As Integer, posY As Integer) As Label
        Dim lbl As Label = CrearLabel(New Point(posX, posY))
        lbl.BackColor = Color.MediumAquamarine
        Return lbl
    End Function




















End Class
