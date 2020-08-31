# Garage.Polly.Extensions.Dapper
This project adds transient fault handling support to [Dapper](https://github.com/StackExchange/Dapper) with [Polly](https://github.com/StackExchange/Dapper). It is written by following [SQL Server Retries with Dapper and Polly](https://hyr.mn/Dapper-and-Polly/) from Ben Hyrman. 

The usage is the same as Dapper with `WithRetry` suffix for all methods.

# Garage.Polly.Extensions.Dapper.Sample
This is a sample project to demo how to use the library. It includes a Terraform script to provision a Azure Sql Database, a Fluent Migrator project to set up the database, and a simple web api project which consumes the library.

## Provision Azure Sql Database
First, use the following commands to login to Azure Cli and select a subscription to provision the database to. 
```
az login
az account show
az account set -s "[your subscription id]"
```
Then, go to the `Terraform` folder and run the Terraform commands to provision the database.
```
terraform init
terraform plan
terraform apply
```
When you finish testing, run `terraform destroy` to destroy the resources.

## Set up Database
This sample project uses FluentMigrator to set up database schema. Update `ConnectionString` environment variable for `Garage.Polly.Extensions.Dapper.Sample.Database` with the output value from Terraform. Then run the project to apply the database schema.

## Run Api endpoint
Update `ConnectionString` environment variable. Then you can run the api project to test the library.
