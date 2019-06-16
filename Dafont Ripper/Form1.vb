Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class Form1


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim listy2 As New ListBox
        Dim letter As String = "a"
        Dim wc As New WebClient
        Dim source As String = wc.DownloadString("https://www.dafont.com/alpha.php?lettre=" & letter & "&page=" & page & "&fpp=100")
        Dim m2 As MatchCollection = Regex.Matches(source, "(?<=<a href=" & Chr(34) & ")[^" & Chr(34) & "]*", RegexOptions.Singleline + RegexOptions.IgnoreCase)
        For Each m As Match In m2
            Dim value As String = m.Groups(0).Value
            If value.Contains("alpha.php?lettre=" & letter & "&page=") Then
                listy2.Items.Add(value)
            End If
        Next
        max = listy2.Items.Count + 1
        amount = max * 100


        Label2.Text = "About (" & amount & ") Fonts detected!"

        ProgressBar1.Maximum = amount

        Timer2.Start()

        Dim thread As New Thread(AddressOf Rip_A)
        thread.Start()
    End Sub


    Dim current As String
    Dim letter As String
    Dim amount As String
    Dim max As String

    Dim page As Integer
    Dim ready As Boolean



    Sub Rip_A()
        ready = False
        Dim listy As New ListBox
        letter = "a"




        Dim wc As New WebClient
        Dim source As String = wc.DownloadString("https://www.dafont.com/alpha.php?lettre=" & letter & "&page=" & page & "&fpp=100")

        Dim m1 As MatchCollection = Regex.Matches(source, "(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?", RegexOptions.Singleline + RegexOptions.IgnoreCase)
        For Each m As Match In m1
            Dim value As String = m.Groups(0).Value
            If value.Contains("dl.dafont.com") Then
                listy.Items.Add(value)
            End If
        Next

        Try
            For Each i As String In listy.Items

                current = "http://" & i
                My.Computer.Network.DownloadFile("http://" & i, TextBox1.Text & "\" & letter & "\" & i.Replace("dl.dafont.com/dl/?f=", "") & ".zip")


            Next
            listy.Items.Clear()
        Catch
        End Try
        If page = max Then
            page = 1
            ready = True

            Exit Sub
        End If
        page = page + 1
        Rip_A()
    End Sub



    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            If current = Nothing Then
                Label1.Text = "-"
            Else
                Label1.Text = current
            End If


        Catch
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
        ready = True

    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Try
            Dim fileCount As Integer = IO.Directory.GetFiles(TextBox1.Text & "\" & letter, "*.zip").Length
            ProgressBar1.Value = fileCount
        Catch
        End Try

    End Sub
End Class
