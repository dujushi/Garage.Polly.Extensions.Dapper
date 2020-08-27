variable "my_ip" {
  type        = string
  description = "The ip address of my machine"
  default     = "27.252.50.211"
}

variable "username" {
  type        = string
  description = "The SQL Server login"
  default     = "dapper-sql-server"
}

variable "password" {
  type        = string
  description = "The SQL Server password"
  default     = "zGoiDGH3r_f"
}

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>2.24.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "dapper" {
  name     = "dapper-resource-group"
  location = "Australia East"
}

resource "azurerm_sql_server" "dapper" {
  name                         = "dapper-sql-server"
  resource_group_name          = azurerm_resource_group.dapper.name
  location                     = azurerm_resource_group.dapper.location
  version                      = "12.0"
  administrator_login          = var.username
  administrator_login_password = var.password
}

resource "azurerm_sql_firewall_rule" "dapper" {
  name                = "My IP"
  resource_group_name = azurerm_resource_group.dapper.name
  server_name         = azurerm_sql_server.dapper.name
  start_ip_address    = var.my_ip
  end_ip_address      = var.my_ip
}

resource "azurerm_mssql_database" "dapper" {
  name           = "dapper-mssql-database"
  server_id      = azurerm_sql_server.dapper.id
}

output "database_connection_string" {
  value = "Server=tcp:${azurerm_sql_server.dapper.fully_qualified_domain_name};Database=${azurerm_mssql_database.dapper.name};User ID=${var.username};Password=${var.password};Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=True;"
}