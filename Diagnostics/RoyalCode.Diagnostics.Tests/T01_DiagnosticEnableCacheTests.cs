using Xunit;

namespace RoyalCode.Diagnostics.Tests;

public class T01_DiagnosticEnableCacheTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void T01_TestCheckEnable(bool isEnable)
    {
        var cache = new DiagnosticEnableCache(_ => isEnable);
        var enable = cache.IsEnable(string.Empty, DateTimeOffset.Now);
        
        Assert.Equal(isEnable, enable);
    }

    [Fact]
    public void T02_TestCacheByNames()
    {
        var eventName1 = "TestName1";
        var eventName2 = "TestName2";
        
        int calls = 0;
        bool checkEnable(string name)
        {
            calls++;
            return true;
        }
        
        var cache = new DiagnosticEnableCache(checkEnable);

        cache.IsEnable(eventName1, DateTimeOffset.Now);
        cache.IsEnable(eventName1, DateTimeOffset.Now);
        cache.IsEnable(eventName2, DateTimeOffset.Now);
        cache.IsEnable(eventName2, DateTimeOffset.Now);
        
        Assert.Equal(2, calls);
    }

    [Fact]
    public void T03_TestExpiration()
    {
        var now = DateTimeOffset.Now;
        var late = now.AddDays(1);
        int calls = 0;
        bool checkEnable(string name)
        {
            calls++;
            return calls is 1;
        }
        
        var cache = new DiagnosticEnableCache(checkEnable);

        var response1 = cache.IsEnable(string.Empty, now);
        var response2 = cache.IsEnable(string.Empty, now);
        var response3 = cache.IsEnable(string.Empty, late);
        var response4 = cache.IsEnable(string.Empty, late);
        
        Assert.True(response1);
        Assert.True(response2);
        Assert.False(response3);
        Assert.False(response4);
        Assert.Equal(2, calls);
    }
}