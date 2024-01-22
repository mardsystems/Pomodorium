using BoDi;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueRetrievers;

namespace Pomodorium.Support;

[Binding]
public class WebApiHooks
{
    private readonly ObjectContainer _objectContainer;
    private readonly AppHostingContext _appHosting;
    private readonly WebApiContext _webApiContext;
    private readonly TestFolders _testFolders;
    private readonly ScenarioContext _scenarioContext;

    public WebApiHooks(
        ObjectContainer objectContainer,
        AppHostingContext appHosting,
        WebApiContext webApiContext,
        TestFolders testFolders,
        ScenarioContext scenarioContext)
    {
        _objectContainer = objectContainer;
        _appHosting = appHosting;
        _webApiContext = webApiContext;
        _testFolders = testFolders;
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void StartApp(ObjectContainer objectContainer)
    {
        AppHostingContext.StartApp();

        var database = AppHostingContext.GetDatabase();
        
        var taskSyncronizer = AppHostingContext.GetTaskSyncronizer();

        objectContainer.RegisterInstanceAs(database);

        objectContainer.RegisterInstanceAs(taskSyncronizer);

        Service.Instance.ValueRetrievers.Register(new NullValueRetriever("<null>"));
        Service.Instance.ValueComparers.Register(new NullValueComparer("<null>"));
    }

    [AfterScenario]
    public void WriteLog()
    {
        if (_scenarioContext.TestError != null)
        {
            var fileName = _testFolders.GetScenarioSpecificFileName(".log");

            _webApiContext.SaveLog(_testFolders.OutputFolder, fileName);
        }
    }

    [AfterTestRun]
    public static void StopApp()
    {
        AppHostingContext.StopApp();
    }
}
