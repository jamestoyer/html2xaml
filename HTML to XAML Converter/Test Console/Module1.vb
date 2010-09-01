﻿Module Module1

    Sub Main()
        Dim test As String = Console.ReadLine
        Do Until test = Nothing
            'Dim htmlc As String = test

            'Console.WriteLine(Html2Xaml.ConvertToXaml(htmlc, True))
            'test = Console.ReadLine()
            If test.ToLower = "go" Then
                Console.WriteLine(PlainHtml)
                test = Console.ReadLine()
            End If
        Loop


    End Sub

    Private Function PlainHtml() As Object
        Dim html As String = "<p> I like planning. I really do. I think calendars are pretty sexy and my OCD acts up when things aren&rsquo;t color coded, have point values or do barrel rolls. This summer, I planned to sneak a few smaller items onto my teams backlog and positively surprise the Council of Stellar Management (CSM) when they got here. I ended up going to all these CSM summits, talking about features new and old, but never actually got to tell the CSM about some of the small things we had lined up. My plan didn&rsquo;t work out, but the job got done, which is what matters.</p><p> Usually, smaller changes get bunched up and released with the coming expansion. This particular change is a little different though, as part of the code is needed for a bugfix, meaning we will be deploying it in the near future instead of this winter. The change we are introducing is going to end ghost datacore accumulation.</p><p>Currently, any character collecting research points from an agent will continue to do so, even after your account lapses due to inactivity. This is a pretty massive loophole to making substantial amounts of money and it is now being closed. When this change is deployed, characters will stop collecting research points when your account lapses into inactivity. You won&rsquo;t lose the points you&rsquo;ve accumulated up to that point, but simply won&rsquo;t gain anymore. When you activate your account, your character will automatically begin earning RPs again. This change is long overdue and will hopefully benefit our active players.</p><p>On a sidenote, this was an issue our team picked off the CSMs list of priorities. It&rsquo;s a great list that outlines both big and small concerns in the community and hopefully we can continue to address it. Summers are pretty quiet here, due to holidays and it&rsquo;s a great time to go over the list and pick a few items for your team. Hopefully, we can bring you more items off the CSMs list, but till then, here&rsquo;s one of them.</p><p>If anyone is interested, the CSM item is <a href=""http://wiki.eveonline.com/en/wiki/End_ghost_%28unpaid_account%29_datacore_production_%28CSM%29"">here</a>.</p><p>The complete list is <a href=""http://wiki.eveonline.com/en/wiki/August_2010_Prioritization_Crowdsourcing_%28CSM%29"">here</a>.</p><p>Anyway, I should have more blogs coming up as we speed up for the winter release.</p><!-- AddThis Button BEGIN --><p><script type=""text/javascript""><!--addthis_pub = 'ccpgames';// --></script><a onclick=""return addthis_sendto()"" onmouseover=""return addthis_open(this, '', '[URL]', '[TITLE]')"" onmouseout=""addthis_close()"" href=""http://www.addthis.com/bookmark.php""><img src=""http://s9.addthis.com/button1-share.gif"" border=""0"" alt="""" width=""125"" height=""16"" align=""right"" /></a><script src=""http://s7.addthis.com/js/152/addthis_widget.js"" type=""text/javascript""></script></p><!-- AddThis Button END --><p>Till then, toodles.</p>"
                Dim result As String = Html2Xaml.Converter.ConvertToXaml(html, True)

                Return result
    End Function

End Module
