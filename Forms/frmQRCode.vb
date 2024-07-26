Imports System.Drawing
Imports System.Windows.Forms
Imports AForge.Video
Imports AForge.Video.DirectShow
Imports ggcAppDriver
Imports ZXing

Public Class frmQRCode
    Private p_oAppDriver As GRider
    Private videoSource As VideoCaptureDevice
    Private videoDevices As FilterInfoCollection
    Private pnLoadx As Integer
    Private poControl As Control

    Private p_bCancelled As Boolean
    Private p_sQRResult As String

    Private Delegate Sub SetPictureBoxImageDelegate(ByVal bmp As Bitmap)
    Private WithEvents scanTimer As New Timer()
    ReadOnly Property QRCodeResult As String
        Get
            Return p_sQRResult
        End Get
    End Property

    ReadOnly Property Cancelled As Boolean
        Get
            Return p_bCancelled
        End Get
    End Property
    Public Sub New(oDriver As GRider)
        InitializeComponent()
        p_oAppDriver = oDriver
        p_bCancelled = True
        p_sQRResult = ""

        ' Set up the timer
        scanTimer.Interval = 750 ' Set the interval to 1 second
    End Sub
    Private Sub Form_Keydown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape

                scanTimer.Stop()
                videoSource.SignalToStop()
                Me.Dispose()
                Me.Close()
            Case Keys.Return, Keys.Down
                SetNextFocus()
            Case Keys.Up
                SetPreviousFocus()
        End Select
    End Sub
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If pnLoadx = 0 Then
            pnLoadx = 1
        End If

        Try
            videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)
            If videoDevices.Count = 0 Then
                MessageBox.Show("No video sources found")
                scanTimer.Stop()
                videoSource.SignalToStop()
                Me.Dispose()
                Me.Close()
                Return
            End If
            Dim deviceMoniker = videoDevices(0).MonikerString
            videoSource = New VideoCaptureDevice(deviceMoniker)
            AddHandler videoSource.NewFrame, AddressOf videoSource_NewFrame
            videoSource.Start()

            ' Start the timer
            scanTimer.Start()
        Catch ex As Exception
            MessageBox.Show("Error initializing webcam: " & ex.Message)
            scanTimer.Stop()
            videoSource.SignalToStop()
            Me.Dispose()
            Me.Close()
        End Try
    End Sub

    Private Sub videoSource_NewFrame(sender As Object, eventArgs As NewFrameEventArgs)
        Dim bitmap As Bitmap = CType(eventArgs.Frame.Clone(), Bitmap)
        If PictureBox1.InvokeRequired Then
            PictureBox1.Invoke(New SetPictureBoxImageDelegate(AddressOf SetPictureBoxImage), bitmap)
        Else
            SetPictureBoxImage(bitmap)
        End If
    End Sub

    Private Sub SetPictureBoxImage(ByVal bmp As Bitmap)
        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
        End If
        PictureBox1.Image = bmp
    End Sub

    Private Sub scanTimer_Tick(sender As Object, e As EventArgs) Handles scanTimer.Tick
        If PictureBox1.Image IsNot Nothing Then
            ' Perform QR code scanning here
            Dim bitmap As Bitmap = CType(PictureBox1.Image.Clone(), Bitmap)

            Dim reader As New BarcodeReader()
            Dim result As Result = reader.Decode(bitmap)
            If result IsNot Nothing Then
                ' Stop the video source
                videoSource.SignalToStop()

                scanTimer.Stop()
                ' Display the QR code content
                p_sQRResult = result.Text
                p_bCancelled = False
                ' Close the form
                Me.Dispose()
                Me.Close()
            End If
        Else

            MessageBox.Show("Error initializing webcam: WebCam is currently unavailable ! Please try Again ")
            'closecamera if not closed properly
            videoSource.SignalToStop()

            scanTimer.Stop()
            Me.Dispose()
            Me.Close()
        End If
    End Sub


End Class