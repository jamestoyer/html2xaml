'TODO: Comments
Imports System.Xml
Public Module Converter
    ''' <summary>
    ''' Converts a HTML string into a fully formed xaml string.
    ''' </summary>
    ''' <param name="html">The HTML string to be converted</param>
    ''' <param name="flowDoc">Determines whether the converted XAML should be enclosed in Flow Document tags</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertToXaml(ByVal html As String, ByVal flowDoc As Boolean) As String
        ' Create a result object
        Dim result As String = Nothing

        ' Take the first step of converting the string into XDocument
        html = cleanHtml(html)
        Dim htmlDoc As XmlDocument = New XmlDocument
        Try
            htmlDoc.LoadXml(html)
        Catch ex As Exception
            MessageBox.Show("Error")
        End Try

        ' Convert the html
        result = HtmlToXamlConverter.Convert(htmlDoc, flowDoc)

        ' Return the result
        Return result
    End Function

    Public Function ConvertToHtml(ByVal xaml As String) As String
        Throw New NotImplementedException
    End Function

    Private Function cleanHtml(ByVal html As String) As String
        ' Clean up the html
        cleanHtml = System.Web.HttpUtility.HtmlDecode(html)

        ' Make sure that that string has a root element
        cleanHtml = String.Format("<{0}>{1}</{0}>", HtmlConstants.document, cleanHtml)
    End Function

End Module