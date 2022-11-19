variable "tags" {
  description = "Azure tags put on all resource to en able better understanding owner and billing."
  type = object({
    environment = string
    managed-by  = string
  })
}

variable "resource_location" {
  description = "The location of resources."
  type        = string
}

variable "resource_group_name" {
  description = "The name of the recource group."
  type        = string
}

variable "sql_server_name" {
  description = "The global unique name for the SQL Server name."
  type        = string
}
