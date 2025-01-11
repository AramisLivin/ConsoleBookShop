# BookStore Console Application

A simple .NET console application for managing a book store.

## Features
- **Commands**:
    - `get`: Filter and sort books by title, author, date, or count.
    - `buy`: Purchase a book by its ID.
    - `restock`: Restock books randomly or by specific ID and count.
- Supports **SQLite** and **PostgreSQL** via Entity Framework.

## How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/AramisLivin/ConsoleBookShop
   cd ConsoleBookShop
   ```
2. Build and run:
   ```bash
   dotnet run
   ```

## Example Commands
```bash
get --title="C#" --order-by="count"
buy --id=1
restock --id=2 --count=5
```
