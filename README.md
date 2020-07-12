# Car Parking Fee Calc - Azure Function

## Motivation
A development task to roll out an API that takes car entry & exit timestamp as input and decide amoung various packages that which one is applicable to the user.

## How to use?

Open up **postman** tool and POST the URL 

https://carparkfeecalc.azurewebsites.net/api/CalcRates?code=gk4jk28FxZCZoYpfWxXWEj4P9KrKSHT6beHVsDsM1zSpPa0W5An4Dw== 

with following payload in the body:

**{
	"startDT": "03/07/2020 23:00",
	"endDT": "04/07/2020 15:00"
}**

Date formate is DD/MM/YYYY HH:mm

![Alt text](/Docs/postman_call.png?raw=true "postman preview")

## Debugging
Download the code and run in local VS 2019 to debug.

## Features
The main function **CalcRates** accepts start and end date and return the calculated fee as json:
**{
    "name": "Standard Rate",
    "price": 20.0
}**

Internally it calls two APIs (normal rates, special rates) to calculate indivisual rates and then return the cheapest.

## Tech/framework used
C# code using VS 2019, deployed to Azure as Function App.

Function level authentication is used.

## Screenshots

![Alt text](/Docs/localURLs.png?raw=true "Local VS2019 enviorement URLs")
![Alt text](/Docs/postman_local_call.png?raw=true "Local postman call")

## Build status
Version 1.0.1 built and released.

## Enhancements
Possibly:
 - Tighten the input validation
 - Log the exceptions / timeouts
 - tidy up code, maybe import Special/Normal rates calculation inside main service
 - write a front end app to cosume API
