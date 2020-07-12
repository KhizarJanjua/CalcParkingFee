# Car Parking Fee Calc - Azure Function

## Motivation
A development task to demonstrate how quickly we can roll out an API that takes car entry & exit timestamp, evaluate which package is applicable and calculate the parking fee. The scope was to build an API. The code is written in C# using VS 2019 for Azure Functions.

## How to use?
**Option 1: Through Visual Studio 2019:** You can download the code and run in VS 2019. It should generate local URL that you can call directly from your browser or through postman tool.

The calculator engine API expects following payload in the body:

**{
	"startDT": "03/07/2020 23:00",
	"endDT": "04/07/2020 15:00"
}**

Note that the date formate is **DD/MM/YYYY HH:MM**

**Option 2: From Azure cloud:** 

Alternatively, open up **postman tool** and POST the URL https://carparkfeecalc.azurewebsites.net/api/CalcRates?code=gk4jk28FxZCZoYpfWxXWEj4P9KrKSHT6beHVsDsM1zSpPa0W5An4Dw== 
with above payload in the body.

![Alt text](/Docs/postman_call.png?raw=true "postman preview")


## Features
The main function **CalcRates** accepts start and end date and return the calculated fee as json.
**{
    "name": "Standard Rate",
    "price": 20.0
}**

Internally it calls two APIs (normal rates, special rates) to calculate indivisual rates and then return the cheapest.

## Tech/framework used
C# code using VS 2019, deployed to Azure as Function App.

Function level authentication is used.

![Alt text](/Docs/InfoFlowDiagrame.png?raw=true "Info Flow")


## Build status
Version 1.0.1 built and released.

## Debugging
Download the code and run in local VS 2019 to debug.

## Enhancements
Possibly:
 - Tighten the validation
 - Log the exceptions / timeouts
 - tidy up code, espacially date conversions
 - write a small front end app to cosume API
