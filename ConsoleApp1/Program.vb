Imports System
Imports System.Collections.Specialized
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Xml

Module Program
    Sub Main(args As String())
        Dim ge As String = "enctoken Qbmp7Ie7WxSIaf6oEf4xHG/vbNufqVnz6IRcWIV/PE77jtYFHWRmWPCF8ZIXM1vm49WrVTreYSmD1D4rxrQgXz3Ur2zt9A=="
        Dim FileName As String = "C:\Paya Repos\geti\AuthTest\XMLFiles\DisplayXML.xml"
        Dim nodeDoc As New XmlDocument()
        Dim rootNode As XmlNode '= nodeDoc.DocumentElement

        Using reader As New XmlTextReader(New StreamReader(FileName))
            reader.MoveToContent()

            reader.ReadStartElement()

            While reader.Depth >= 1
                If reader.IsStartElement Then
                    nodeDoc.LoadXml(reader.ReadOuterXml)
                    rootNode = nodeDoc.DocumentElement
                End If

            End While

        End Using

        If (ValidateParam(ge)) Then
            Console.WriteLine("Valid")
        End If

        Console.WriteLine(FileName)
        Console.WriteLine("Hello World!")
        'Console.WriteLine(encrypted)
        Console.ReadLine()

    End Sub
    Private key As Byte() = New Byte(7) {1, 2, 3, 4, 5, 6, 7, 8}
    Private iv As Byte() = New Byte(7) {1, 2, 3, 4, 5, 6, 7, 8}
    Public Function ValidateParam(ByVal fileName As String) As Boolean
        'Dim fileName As String = context.Request.QueryString("file")
        'Dim pattern As String = "^[\w\s]" 'This allows all upper and lower case alphabets, numbers and underscore. 
        'Dim regex As Regex = New Regex(pattern)
        'If (Not regex.IsMatch(fileName)) Then
        '    Return False
        'End If
        'Return True

        Dim qscoll As NameValueCollection = HttpUtility.ParseQueryString(fileName)
        Dim reValid As Regex = New Regex("^[\w\s]")
        For Each key As String In qscoll.AllKeys
            If Not reValid.IsMatch(qscoll(key)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function Crypt(ByVal text As String) As String
        Dim algorithm As SymmetricAlgorithm = DES.Create()
        algorithm.Mode = CipherMode.CBC
        Dim transform As ICryptoTransform = algorithm.CreateEncryptor(key, iv)
        Dim inputbuffer As Byte() = Encoding.Unicode.GetBytes(text)
        Dim outputBuffer As Byte() = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length)
        Return Convert.ToBase64String(outputBuffer)
    End Function

    Public Function Decrypt(ByVal text As String) As String
        Dim algorithm As SymmetricAlgorithm = DES.Create()
        algorithm.Mode = CipherMode.CBC
        Dim transform As ICryptoTransform = algorithm.CreateDecryptor(key, iv)
        Dim inputbuffer As Byte() = Convert.FromBase64String(text)
        Dim outputBuffer As Byte() = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length)
        ' Convert.ToBase64String(outputBuffer)
        Return Encoding.Unicode.GetString(outputBuffer)
    End Function


    Private Function ISValidContentType(ByVal contentType As String) As Boolean
        Dim pattern As String = "^((application\/(x-www-form-urlencoded|json|x-javascript))|(text\/(javascript|x-javascript|x-json)))(;+.*)*$" 'This allows all upper and lower case alphabets, numbers and underscore. 
        Dim regex As Regex = New Regex(pattern)
        If (regex.IsMatch(contentType)) Then
            Return True
        End If
        Return False
    End Function


    Private Function AES_Encrypt(ByVal input As String, ByVal pass As String) As String
        Dim AES As New RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = CipherMode.CBC
            Dim DESEncrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateEncryptor
            Dim Buffer As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(input)
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return encrypted
        Catch ex As Exception
        End Try
    End Function

    'Public Function DecryptRijndael(ByVal value As String, ByVal encryptionKey As String) As String
    '    Dim AES As New RijndaelManaged

    '    Dim hash(31) As Byte
    '    AES.Key = hash
    '    AES.Mode = CipherMode.CBC
    '    Dim DESDecryptor As ICryptoTransform = AES.CreateDecryptor


    '    Return decrypted
    'End Function

End Module
