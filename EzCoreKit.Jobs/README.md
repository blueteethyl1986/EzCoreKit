# EzCoreKit.File
安全的刪除檔案幫助類別。

### 安裝
1. [Nuget](https://www.nuget.org/packages/EzCoreKit.Jobs)
2. [MyGet](https://www.myget.org/feed/xpy/package/nuget/EzCoreKit.Jobs)
```
PM> Install-Package EzCoreKit.Jobs
```

### 快速上手
```csharp
[Route("api/[controller]")]
public class ValuesController : Controller {
    // GET api/values
    [HttpGet("{id}")]
    public string Get(Guid id) {
        return JobManager.Get(id).Processing.ToString();
    }

    // GET api/values/5
    [HttpGet("NewJob")]
    public string NewJob() {
        var job = new MyJob();
        JobManager.AddAndRun(job);
        return job.Id.ToString();
    }

    [HttpDelete("{id}")]
    public void KillJob(Guid id) {
        JobManager.RemoveAndCancel(id);
    }
}

public class MyJob : Job {
    public MyJob() {
        Command = "dotnet";
        Arguments = @"TestJobProcess.dll";
    }

    public override void OnOutput(string line) {
        if (int.TryParse(line, out int processing)) {
            this.Processing = processing;
        }
    }
}
```