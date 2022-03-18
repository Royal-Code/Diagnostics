using System.Diagnostics;
using Xunit;

namespace RoyalCode.Diagnostics.Tests;

public class T04_ArgumentAdapterTests
{

    public void T01_()
    {
        var diagnostic = new DiagnosticListener(nameof(T04_ArgumentAdapterTests));
        
        //diagnostic.Write(nameof(T01_), new ArgumentAdapterEventFoo() );
        
        


    }
}

public class ArgumentAdapterEventFoo
{
    
}

public class ArgumentAdapterEventBar
{
    
}