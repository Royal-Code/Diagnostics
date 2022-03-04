using Xunit;

namespace RoyalCode.Diagnostics.Tests;

public class T02_DiagnosticsOperationFactoryTests
{
    [Fact]
    public void T01_AssertNames()
    {
        Assert.Equal("RoyalCode.Diagnostics.Tests", TestsDiagnostic.ListenerName);
        Assert.Equal("RoyalCode.Diagnostics.Other", OtherTestsDiagnostic.ListenerName);
    }
}

[DiagnosticListenerName("RoyalCode.Diagnostics.Tests")]
public class TestsDiagnostic : DiagnosticOperationFactory<TestsDiagnostic>
{
    
}

[DiagnosticListenerName("RoyalCode.Diagnostics.Other")]
public class OtherTestsDiagnostic : DiagnosticOperationFactory<OtherTestsDiagnostic>
{
    
}

[DiagnosticListenerName("MyCompany.Diagnostics.MyComponent")]
public class MyComponentDiagnostics : DiagnosticOperationFactory<MyComponentDiagnostics>
{
    public static DiagnosticOperation CreateDoingSomethingOperation() 
        => CreateOperation("DoingSomething");
}