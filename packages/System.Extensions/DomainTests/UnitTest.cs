namespace System.DomainTests;

public abstract class UnitTest
{
    public Action Action { get; set; }

    public UnitTest()
    {
        Arrange();

        try
        {
            Act();
        }
        catch (Exception)
        {

        }        
    }

    public abstract void Arrange();

    public abstract void Act();
}