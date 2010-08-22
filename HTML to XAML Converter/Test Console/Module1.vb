Module Module1

    Sub Main()
        Dim test As String = Console.ReadLine
        Do Until test = Nothing
            Dim htmlc As String = test

            Console.WriteLine(Html2Xaml.ConvertToXaml(htmlc, True))
            test = Console.ReadLine()
        Loop


    End Sub

End Module
