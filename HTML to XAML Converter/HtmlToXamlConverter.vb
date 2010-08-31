Imports System.Xml
'TODO: Comments
Friend Module HtmlToXamlConverter

    Public Function Convert(ByVal html As XmlDocument, ByVal createAsFlowDoc As Boolean) As String
        ' Create a string to get the result
        Dim result As String = String.Empty

        ' Create a flow document root element in a new XmlDocument
        Dim xamlDoc As XmlDocument = New XmlDocument
        Dim rootElement As XmlNode
        If createAsFlowDoc Then
            rootElement = xamlDoc.CreateElement(XamlConstants.flowDocument, XamlConstants.xamlNamespace)
        Else
            rootElement = xamlDoc.CreateElement(XamlConstants.section, XamlConstants.xamlNamespace)
        End If

        ' Parse this information to the converter
        xamlDoc.AppendChild(Convert(html, rootElement))
        result = xamlDoc.OuterXml
        Return result
    End Function

    Private Function Convert(ByVal html As XmlDocument, ByVal rootElement As XmlNode) As XmlNode
        ' Get the children the html element
        Dim content As XmlElement = CType(html.FirstChild, XmlElement)

        ' The root element is the <html> tag, so lets see what children it has and convert them
        Convert = checkForChildren(content, rootElement)
    End Function

    Private Function checkForChildren(ByVal content As XmlNode, ByVal rootElement As XmlNode) As XmlNode
        For Each c As XmlNode In content.ChildNodes
            ' Create a new element with the correct xaml formatting and attributes
            Dim unknownElement As Boolean = New Boolean
            Dim newElement As XmlNode = replaceWithXaml(c, rootElement, unknownElement)

            ' This is all good and well but we haven't considered children of the element
                rootElement.AppendChild(checkForChildren(c, newElement))
        Next

        ' Return the new root element
        Return rootElement
    End Function

    Private Function replaceWithXaml(ByVal c As XmlNode, ByVal owner As XmlNode, ByRef unknownElement As Boolean) As XmlNode
        ' Compare the elements name with some standard html elements
        Dim newElement As XmlNode
        Select Case c.Name
            Case HtmlConstants.paragraph
                newElement = owner.OwnerDocument.CreateElement(XamlConstants.paragraph, XamlConstants.xamlNamespace)

            Case HtmlConstants.hyperlink
                newElement = makeHyperlink(c, owner.OwnerDocument)

            Case HtmlConstants.image
                newElement = makeImage(c, owner.OwnerDocument)

            Case HtmlConstants.text
                newElement = owner.OwnerDocument.CreateTextNode(c.OuterXml)

            Case HtmlConstants.script, HtmlConstants.comment
                ' HACK: Here I'm making a concious decision to remove all scripting and its contents
                newElement = owner.OwnerDocument.CreateTextNode("")

            Case HtmlConstants.heading1, HtmlConstants.heading2, HtmlConstants.heading3, HtmlConstants.heading4, HtmlConstants.heading5, HtmlConstants.heading6
                newElement = constructHeading(c, owner.OwnerDocument)

            Case HtmlConstants.lineBreak
                newElement = owner.OwnerDocument.CreateElement(XamlConstants.lineBreak, XamlConstants.xamlNamespace)

            Case HtmlConstants.bold
                newElement = owner.OwnerDocument.CreateElement(XamlConstants.bold, XamlConstants.xamlNamespace)

            Case Else
                ' Do not convert the element as it is an unknown element
                newElement = owner.OwnerDocument.CreateElement(XamlConstants.span, XamlConstants.xamlNamespace)

        End Select
        Return newElement
    End Function

    Private Function makeHyperlink(ByVal c As XmlNode, ByVal owner As XmlDocument) As XmlNode
        ' Get the href attribute of the link so as to evaluate it
        Dim link As String = findAttribute(HtmlConstants.hyperlinkHref, c)

        ' Evaluate the href string
        Dim newElement As XmlNode
        If IsNothing(link) Then
            ' Just return a span if the href is nothing
            newElement = owner.CreateElement(XamlConstants.span, XamlConstants.xamlNamespace)
        Else
            ' Create the new link element
            newElement = owner.CreateElement(XamlConstants.hyperlink, XamlConstants.xamlNamespace)

            ' Lets evaluate the link string
            Dim linkParts As String() = (link.Trim).Split(New Char() {CChar("#")})

            ' Create the link
            Dim newAttribute As XmlAttribute = owner.CreateAttribute(XamlConstants.hyperlinkNavigate)
            newAttribute.Value = linkParts(0)
            newElement.Attributes.Append(newAttribute)
        End If

        Return newElement
    End Function

    Private Function findAttribute(ByVal attribute As String, ByVal c As XmlNode) As String
        ' Find the selected attribute
        For a = 0 To c.Attributes.Count - 1
            ' Once we find the value return it
            If c.Attributes(a).Name = Attribute Then Return c.Attributes(a).Value
        Next

        ' No match, therefore return nothing
        Return Nothing
    End Function

    Private Function makeImage(ByVal c As XmlNode, ByVal owner As XmlDocument) As XmlNode
        ' Create a new element to hold the element
        Dim newElement As XmlNode

        ' Get a list of important attributes
        Dim attributes As New List(Of XmlAttribute)
        attributes.Add(owner.CreateAttribute(XamlConstants.imageSource))
        attributes.Last.Value = findAttribute(HtmlConstants.imageSource, c)
        attributes.Add(owner.CreateAttribute(XamlConstants.imageHeight))
        attributes.Last.Value = findAttribute(HtmlConstants.imageHeight, c)
        attributes.Add(owner.CreateAttribute(XamlConstants.imageWidth))
        attributes.Last.Value = findAttribute(HtmlConstants.imageWidth, c)
        attributes.Add(owner.CreateAttribute(XamlConstants.imageAlign))
        attributes.Last.Value = findAttribute(HtmlConstants.imageAlign, c)

        ' Go through the attributes and add them
        If attributes.Count = 0 Then
            newElement = owner.CreateTextNode("")
        Else
            ' Create the new image element
            newElement = owner.CreateElement(XamlConstants.image, XamlConstants.xamlNamespace)
            For Each att In attributes
                If att.Name = XamlConstants.imageHeight OrElse att.Name = XamlConstants.imageWidth Then
                    Dim text As Integer = InStr(att.Value, "px")
                    If text > 0 Then att.Value = Left(att.Value, text - 1)
                End If
                newElement.Attributes.Append(att)
            Next
        End If

        Return newElement
    End Function

    Private Function constructHeading(ByVal c As XmlNode, ByVal xmlDocument As XmlDocument) As XmlNode
        ' Create a new paragraph element to store the header in
        Dim newElement As XmlNode = xmlDocument.CreateElement(XamlConstants.paragraph, XamlConstants.xamlNamespace)

        ' Create a font size attribute then assign the correct value to it
        Dim newAttribute As XmlAttribute = xmlDocument.CreateAttribute(XamlConstants.heading)
        Select Case c.Name
            Case HtmlConstants.heading1
                newAttribute.Value = XamlConstants.heading1
            Case HtmlConstants.heading2
                newAttribute.Value = XamlConstants.heading2
            Case HtmlConstants.heading3
                newAttribute.Value = XamlConstants.heading3
            Case HtmlConstants.heading4
                newAttribute.Value = XamlConstants.heading4
            Case HtmlConstants.heading5
                newAttribute.Value = XamlConstants.heading5
            Case HtmlConstants.heading6
                newAttribute.Value = XamlConstants.heading6
        End Select

        ' Add the attribute to the element and return
        newElement.Attributes.Append(newAttribute)
        Return newElement
    End Function

End Module
