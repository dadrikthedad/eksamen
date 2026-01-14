CREATE TABLE Mappeoppgave1.books (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL CHECK (length(Title) <= 200),
    Author TEXT NOT NULL CHECK (length(Author) <= 100),
    PublicationYear INTEGER NOT NULL,
    ISBN TEXT UNIQUE NOT NULL CHECK (length(ISBN) <= 25),
    InStock INTEGER NOT NULL
);
