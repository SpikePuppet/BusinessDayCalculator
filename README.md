# Business Day Calculator
A simple API to calculate the business days between two date, created using the ASP.NET Core Framework, running on .NET Core 3.1.

## Requirements
1. .NET Core 3.1 is required to run this API. If you do not have this installed, please visit [this site](https://dotnet.microsoft.com/download/dotnet-core/3.1) and download/install the SDK version 3.1.2 (this comes with the command line tools which will be required later).
..* For reference i believe any version of 3.1 will work, though this is untested.
2. You should have Postman/Fiddler/Any other http tool which you can use to send a POST request with a custom body

## How to use the calculator
1. Download this repository to your computer
2. Open the command prompt and navigate to the the project
3. Then navigate to the folder called "WebEndpoints"
4. Run *dotnet run* in the command prompt and wait for the program to start up
5. Open your Postmane/Fiddler/whatever tool you chose above and create a POST request which will be sent to **http://localhost:5000/calculate-business-days**. The body of the request should look like this:
```
{
  "StartDate": "2020-04-08T00:00:00.000",
  "EndDate": "2020-04-09T00:00:00.000"
}
```
Feel free to change the dates to whatever is required. Ensure the request body is JSON.
6. Hit send, and inspect the result. You should see the difference between the two dates as "businessDays" in the request body.

## Notes
If you want to run the unit tests for this project, naviagte to the folder called BusinessDayCalculatorApiTests via the command prompt, and run *dotnet test*.
