# Business Day Calculator
A simple API to calculate the business days between two date, created using the ASP.NET Core Framework, running on .NET Core 3.1.

## Requirements
1. .NET Core 3.1 is required to run this API. If you do not have this installed, please visit [this site](https://dotnet.microsoft.com/download/dotnet-core/3.1) and download/install the SDK version 3.1.2 (this comes with the command line tools which will be required later).
  * For reference i believe any version of 3.1 will work, though this is untested.
2. You should have Postman/Fiddler/Any other http tool which you can use to send a POST request with a custom body

## How to use the calculator
1. Download this repository to your computer
2. Open the command prompt and navigate to the the project
3. Then navigate to the folder called "WebEndpoints"
4. Run *dotnet run* in the command prompt and wait for the program to start up
5. Open your Postmane/Fiddler/whatever tool you chose above and create a POST request with the URL **http://localhost:5000/calculate-business-days**. The body of the request should look like this:
```
{
  "StartDate": "2020-04-08T00:00:00.000",
  "EndDate": "2020-04-09T00:00:00.000"
}
```
Feel free to change the dates to whatever is required. Ensure the request body is JSON.
6. Hit send, and inspect the result. You should see the difference between the two dates as "businessDays" in the request body.

## Setting up public holidays
Public holidays are currently calculated by reading in a CSV file (located under the WebEndpoints directory, called PublicHolidays.csv). This is configurable through the **appsettings.json** file, however it does need to follow a specific format (see below for an explanation). The format is: 
```
SetDate,Month,Day,AdjustForWeekend,DayOfWeek,WeekOffset
```

* The SetDate field check if the holiday is on a specific day. So if this is marked as true, and you have month set to 4 and day set to 5, then it will register a public holiday on the 5th or April. If this is set as false, then the public holiday is a RelativeDate holiday (see below). This is a boolean field.
* The Month Field is which month the holiday should occur (This is used for a SetDate or RelativeDate holiday). This is an integer.
* The Day field states which day the holiday should occur (this is only used for a SetDate holiday). This is an integer.
* The AdjustForWeekend field determines if the holiday occurs on the next available weekday. If this is set to true, and the public holiday falls on a weekend, it will find the next available weekday and return that as the public holiday.
* DayOfWeek is a Enum provided by C#, which is used to determine what day to schedule a public holiday on if it's a RelativeDate. This when combined with WeekOffset will calculate which day the holiday should occur. For example, if you use Monday, set the Offset to 3 and set the Month Field to 6, then it will calculate the 3rd Monday in June as a public holiday
* WeekOffset is a integer, which helps determines which day should be used for a public holiday. See above point for an explanation.

Some examples of this:

```
false,08,06,true,Tuesday,1
```
Which would give you the first Tuesday in August as a public holiday.

```
true,01,26,false,Monday,0
```
Which would give you the 26th of January for 

```
true,01,26,true,Wednesday,0
```
Which would give you the 26th of January, but if it falls on a weekend it will get the next Monday.

Year is calculated in the service which gets this information.

## Notes
If you want to run the unit tests for this project, naviagte to the folder called BusinessDayCalculatorApiTests via the command prompt, and run *dotnet test*.
