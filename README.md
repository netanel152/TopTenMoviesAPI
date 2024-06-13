## Getting Started

### Initial Setup

To set up the project for the first time, follow these steps:

1. Open the **Package Manager Console** in Visual Studio.
2. Run the following command to create an initial migration:
   ```sh
   Add-Migration InitialCreate
   ```
3. Apply the migration to the database by running:
   ```sh
   Update-Database
   ```

These steps will set up the database schema based on your Entity Framework Core model.

### Seeding the Database

When you run the backend, the database will be seeded with 10 rows of data. 

### Verifying Data Load

To verify that the data has been loaded correctly, you can use the following `curl` command to fetch the data:

```sh
curl -X 'POST' \
  'https://localhost:7037/all-movies' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "take": 10,
  "skip": 0,
  "searchType": 2
}'
```

This command will retrieve the 10 seeded rows from the database.

### Additional Information

For more details on using Entity Framework Core with .NET 8, please refer to the [official documentation](https://docs.microsoft.com/en-us/ef/core/).

---

Feel free to customize this section further to match your project's requirements.
