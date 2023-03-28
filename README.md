# Initial thoughts about `Windows Service` as a project type    
I implemented a solution as a `windows service`  as was requested in the requirements. It is worth pointing out that if it was a real-life project I strongly recommend taking a different approach:  
* [Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview), or even better [Durable Azure Function](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=csharp-inproc) that would give us better observability over job status w/o a need of introducing plenty of boilerplate code that happens when we use `Windows Service`  
* Docker running in K8s - If I don't want to vendor lock I can implement a simple `C# library` or `Console application` that runs in Docker container and delegate triggering it (scheduling) to K8s  
* [Hangfire](https://www.hangfire.io/) scheduler job. I would consider using Hangfire or a similar scheduler only if Azure Function or K8s solution is not an option. Using Hangfire is much better in long-run (maintainability, simplicity, observability) than implementing old-school `Windows Service`  

# Time problem with data returned from PowerService.dll
In the examples provided with the exercise `PowerService.GetTrades` method returns 24 periods. When I call this method, I receive only 23 periods. I don't know where is the missing period. I thought that I need to add the last period from the previous day, but statement from the exercise: "The PowerTrade class contains an array of PowerPeriods for the given day. The period number starts at
1, which is the first period of the day and starts at 23:00 (11 pm) on the previous day." makes me believe that I should have all the data that I need. My aggregated period only exports hours to 21:00 and I don't have 22:00 due to that.

# Few words about codebase
* It is a code smell to close bind your application to an external library model, to address this I implement a DTO layer so the code base is agnostic from `PowerService.dll`. I am a huge fan of not ever binding to external library models because it is really hard to change an app when a model in DLL changes.
* I implemented `date time service pattern` it is a common approach if you want to unit test an application and have full control over the date. It makes it is easy to control time and change it in the future if needed (for example from local time to UTC).
* I access variables from appsetings thru `Service Config`. I am a huge fan of strong types, and this is a common pattern to use appsettings and maintain strong types. I try not to use `magic strings` in my apps.
* For error handling I catch all exceptions in TimedService and return `Environment.Exit(1)` to ask Windows to restart the entire service. It also means that after the service is restarted it will try to run the task again. If you run this app from Visual Studio and not as a Service then of course Windows is not going to restart the service, in this case in case of exception your debugging session will end. This is by design. Again this solution is a `Windows Service` designed to run as a `Windows Service` and not something else. It is worth pointing out that this behavior is different on Linux operating system.
* There is a lot of work that can be done to Unit Tests. I see a lot of space to improve the ones that I've written. Plus there is a space to add plenty of more. I estimate that I can easily spend at least 4 more hours adding more unit tests and making tests look better. I will stop here, I hope that the tests I provided will give a good insight of what kind of logic I test first, and how to structure tests correctly. I am not a fan anymore to write tests that check every single line of code. I believe that this extra time can be better used by adding observability - tracing, numerical observability of data in Grafana, integration or end-to-end tests.


# It is not acceptable to miss a scheduled extract.  
I am not sure If I understand this requirement. I did not implement any complex `retry mechanism`  or logic to check when the last time job succeeded and carry on from that place. My understanding is that this project is already too large for a simple task, which is why I assume that implementing such mechanisms is outside the scope of the project. Again it would be easier to do it if it was `Azure Function`, `K8s` or `Hangfire job`. The way how it works now is in case of an error entire Windows Service will be restarted which will trigger the execution of the job again.  

# How to improve the code base and next steps.  
* Not referencing `PowerService.dll` directly, wrapping it in Nuget Package and importing it as a NuGet dependency.  
* Adding more observability to the project, ideally Open Telemetry, especially traceability, thanks to that we can reduce the number of log calls in app. If possible use App Insights, they are brilliant and easy to configure.  
* If a project is going to grow I recommend extracting `Services` folder and `Models` folder to external projects. As it is implemented now I feel like the project is too small for that and I recommend keeping it all together.  
* Extracting DI configuration from Program file to a separate class.  
* More Unit Tests

# To run app as a service
1. Publish service
2. `sc.exe create "Trade Service" binpath="C:\\Program Files\\dotnet\\dotnet.exe D:\\worek\\repos\\TradePowerWinService\\TradePowerWinService\\bin\\Release\\net6.0\\publish\\TradePowerWinService.dll"`

# To unregister the service
1. `sc stop "Trade Service"`
2. `sc delete "Trade Service"`