# Migrations

This folder contains database migration for each driver (mysql, postgresql, mssql)

## Dependencies

Install dotnet ef tool by command:

```dotnet tool install --global dotnet-ef```

Add correct configuration block to appsettings.<env>.json, example for PostgreSQL:

```
"DbConfig": {
    "Driver": "postgresql", #can be sqlite, mysql, mssql, postgresql
    "Params": {
      "Address": "localhost",
      "Port": 5432,
      "DatabaseName": "IdentityDB",
      "User": "postgres",
      "Password": "qwerty"
    }
}
```

## Create migration

We can create migration per sql driver via using needed DbContext and seperated folders.

`dotnet ef migrations add <Name> -o Migrations/<Driver> -c <Context> -p Kirel.Identity.Server.Infrastructure -s Kirel.Identity.Server.API -v`

where:\
`<Name>` - Migration name.\
`<Driver>` - MySQL, PostgreSQL or MSSQL.\
`<Context>` - CoursesMssqlIdentityDbContext, CoursesMysqlIdentityDbContext or CoursesPostgresqlIdentityDbContext.