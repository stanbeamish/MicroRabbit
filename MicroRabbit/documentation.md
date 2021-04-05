# Documentation MicroRabbit Solution

## Prerequisites
- MSSQL Server 2019
Installation of the MSSQL Server is necessary. Every microservice has its own database.
As of being a very simple application, every database has just one table.

- RabbitMQ
Installation and running service of RabbitMQ must be available. See https://www.rabbitmq.com/ for installation manual.

## MicroRabbit.Domain.Core

## MicroRabbit.Infrastructure.Bus

## MicroRabbit.Infrastructure.IoC
Inversion of Control

## MicroRabbit.Banking Microservice
API to offer
- GET to /api/Banking 
Returns all bank accounts.
Receives a message from the Queue TransferCreatedEvent

- POST to /api/Banking
Sends a transfer with an amount from one bank account ot another bank account.
Sends a message to the Queue TransferCreatedEvent

## MicroRabbit.Transfer Microservice
A microservice that has subscribed to the TransferLog message from RabbitMQ.
API to offer
- GET to /api/Transfer
Returns all account transfers.

## MicroRabbit.MVC
ASP.NET MVC Frontend used to POST a a transfer via the API /api/Banking

## MicroRabbit.BlazorWASM
A Blazor WASM app to display the bank accounts and all transfer by calling the corresponding microservice APIs.
Makes use of the standard Bootstrap UI framework.

## MicroRabbit.MatBlazorWASM
A Blazor WASM app to display the bank accounts and all transfer by calling the corresponding microservice APIs.
Makes use of MatBlazor - a Material design components library.
