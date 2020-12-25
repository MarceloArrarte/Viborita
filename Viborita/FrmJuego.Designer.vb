<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmJuego
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnIniciar = New System.Windows.Forms.Button()
        Me.pnlCampoDeJuego = New System.Windows.Forms.Panel()
        Me.tmrMovimiento = New System.Windows.Forms.Timer(Me.components)
        Me.lblPuntaje = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnSalir
        '
        Me.btnSalir.Location = New System.Drawing.Point(13, 415)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(75, 23)
        Me.btnSalir.TabIndex = 0
        Me.btnSalir.TabStop = False
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'btnIniciar
        '
        Me.btnIniciar.Location = New System.Drawing.Point(713, 415)
        Me.btnIniciar.Name = "btnIniciar"
        Me.btnIniciar.Size = New System.Drawing.Size(75, 23)
        Me.btnIniciar.TabIndex = 1
        Me.btnIniciar.TabStop = False
        Me.btnIniciar.Text = "Iniciar"
        Me.btnIniciar.UseVisualStyleBackColor = True
        '
        'pnlCampoDeJuego
        '
        Me.pnlCampoDeJuego.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlCampoDeJuego.Location = New System.Drawing.Point(13, 13)
        Me.pnlCampoDeJuego.Name = "pnlCampoDeJuego"
        Me.pnlCampoDeJuego.Size = New System.Drawing.Size(775, 396)
        Me.pnlCampoDeJuego.TabIndex = 2
        '
        'tmrMovimiento
        '
        Me.tmrMovimiento.Interval = 250
        '
        'lblPuntaje
        '
        Me.lblPuntaje.AutoSize = True
        Me.lblPuntaje.Location = New System.Drawing.Point(357, 420)
        Me.lblPuntaje.Name = "lblPuntaje"
        Me.lblPuntaje.Size = New System.Drawing.Size(55, 13)
        Me.lblPuntaje.TabIndex = 3
        Me.lblPuntaje.Text = "Puntaje: 0"
        Me.lblPuntaje.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblPuntaje.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(476, 420)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Label1"
        '
        'FrmJuego
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblPuntaje)
        Me.Controls.Add(Me.pnlCampoDeJuego)
        Me.Controls.Add(Me.btnIniciar)
        Me.Controls.Add(Me.btnSalir)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "FrmJuego"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnSalir As Button
    Friend WithEvents btnIniciar As Button
    Friend WithEvents pnlCampoDeJuego As Panel
    Friend WithEvents tmrMovimiento As Timer
    Friend WithEvents lblPuntaje As Label
    Friend WithEvents Label1 As Label
End Class
